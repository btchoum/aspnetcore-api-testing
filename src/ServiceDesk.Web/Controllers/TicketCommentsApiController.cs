using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceDesk.Web.Data;
using ServiceDesk.Web.Infrastructure;
using ServiceDesk.Web.Models;

namespace ServiceDesk.Web.Controllers
{
    [ApiController]
    public class TicketCommentsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<TicketCommentsApiController> _logger;

        public TicketCommentsApiController(
            ApplicationDbContext db,
            IEmailSender emailSender,
            ILogger<TicketCommentsApiController> logger)
        {
            _db = db;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpPost("api/tickets/{id:long}/comments")]
        public async Task<IActionResult> AddComment(
            [FromRoute] long id,
            [FromBody] AddCommentDto dto)
        {
            var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            try
            {
                _db.Add(new Comment
                {
                    TicketId = ticket.Id,
                    Text = dto.Text,
                    Created = dto.Now
                });

                await _db.SaveChangesAsync();

                var body = $"A comment was added to your ticket <br /> {dto.Text}";
                var message = new EmailMessage
                {
                    Subject = "A comment was added to your ticket",
                    To = ticket.SubmitterEmail,
                    Body = body
                };

                await _emailSender.SendAsync(message);

                return Ok();
            }
            catch (Exception exception)
            {
                var message = "An error occurred while adding the comment"; 
                _logger.LogError(exception, message);
                return StatusCode((int)HttpStatusCode.InternalServerError, message);

            }
        }
    }
}