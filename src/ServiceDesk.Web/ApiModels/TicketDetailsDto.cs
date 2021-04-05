using ServiceDesk.Web.Data;
using System;

namespace ServiceDesk.Web.ApiModels
{
    public class TicketDetailsDto
    {
        public TicketDetailsDto(Ticket ticket)
        {
            Title = ticket.Title;
            Id = ticket.Id;
            Created = ticket.Created;
            Details = ticket.Details;
            State = ticket.State.ToString();
            SubmitterEmail = ticket.SubmitterEmail;
            SubmitterName = ticket.SubmitterName;
        }

        public TicketDetailsDto()
        {
            
        }
        
        public string Title { get; set; }
        public string Details { get; set; }
        public string SubmitterName { get; set; }
        public string SubmitterEmail { get; set; }
        public long Id { get; set; }
        public string State { get; set; }
        public DateTime Created { get; set; }
    }
}