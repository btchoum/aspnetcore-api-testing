using System;
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

            var ticket = dto.MapToTicket();

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

                var ticketDetails = new TicketDetailsDto(ticket);
                
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
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            return Ok(new TicketDetailsDto(ticket));
        }
    }
}
