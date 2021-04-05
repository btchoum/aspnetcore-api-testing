using System.Collections.Generic;
using ServiceDesk.Web.Data;

namespace ServiceDesk.Web.ApiModels
{
    public class TicketCreateDto : BaseCommand
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public string SubmitterName { get; set; }
        public string SubmitterEmail { get; set; }

        public Ticket MapToTicket()
        {
            return new()
            {
                Id = 0,
                Title = Title,
                Details = Details,
                SubmitterName = SubmitterName,
                SubmitterEmail = SubmitterEmail,
                State = TicketState.New,
                Created = Now,
                LastUpdated = Now,
                Comments = new List<Comment>()
            };
        }
    }
}
