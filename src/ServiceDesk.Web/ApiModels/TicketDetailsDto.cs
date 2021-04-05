using System;

namespace ServiceDesk.Web.ApiModels
{
    public class TicketDetailsDto
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public string SubmitterName { get; set; }
        public string SubmitterEmail { get; set; }
        public long Id { get; set; }
        public string State { get; set; }
        public DateTime Created { get; set; }
    }
}