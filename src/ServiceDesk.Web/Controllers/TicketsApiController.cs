using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceDesk.Web.ApiModels;
using ServiceDesk.Web.Data;
using ServiceDesk.Web.Infrastructure;

namespace ServiceDesk.Web.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    public class TicketsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<TicketsApiController> _logger;

        public TicketsApiController(
            ApplicationDbContext db,
            IEmailSender emailSender,
            ILogger<TicketsApiController> logger)
        {
            _db = db;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateTicket(
            TicketCreateDto dto,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating ticket");

            var ticket = new Ticket
            {
                Title = dto.Title,
                Details = dto.Details,
                SubmitterName = dto.SubmitterName,
                SubmitterEmail = dto.SubmitterEmail,
                Created = dto.Now,
                LastUpdated = dto.Now,
                State = TicketState.New
            };

            try
            {
                _db.Add(ticket);

                await _db.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Ticket created {ticket.Id}");
                
                var message = new EmailMessage
                {
                    Subject = "We have received your email",
                    To = ticket.SubmitterEmail,
                    Body = "We have received your ticket"
                };

                await _emailSender.SendAsync(message);

                var ticketDetails = new TicketDetailsDto
                {
                    Title = ticket.Title,
                    Details = ticket.Details,
                    SubmitterName = ticket.SubmitterName,
                    SubmitterEmail = ticket.SubmitterEmail,
                    Id = ticket.Id,
                    State = ticket.State.ToString(),
                    Created = ticket.Created
                };
                
                return CreatedAtRoute("GetTicketById", new { id = ticket.Id }, ticketDetails);
            }
            catch (Exception exception)
            {
                var message = "Error occurred when submitting the ticket";
                _logger.LogError(exception, message);
                return StatusCode((int)HttpStatusCode.InternalServerError, message);
            }
        }

        [HttpGet("{id:long}", Name = "GetTicketById")]
        public async Task<IActionResult> GetTicketById(
            [FromRoute] long id,
            CancellationToken cancellationToken = default)
        {
            var ticket = await _db.Tickets
                .Select(t => new TicketDetailsDto
                {
                    Title = t.Title,
                    Details = t.Details,
                    SubmitterName = t.SubmitterName,
                    SubmitterEmail = t.SubmitterEmail,
                    Id = t.Id,
                    State = t.State.ToString(),
                    Created = t.Created
                })
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            return Ok(ticket);
        }
    }
}
