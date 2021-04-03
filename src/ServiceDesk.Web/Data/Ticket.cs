using System;

namespace ServiceDesk.Web.Data
{
    public class Ticket
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string SubmitterName { get; set; }
        public string SubmitterEmail { get; set; }
        public TicketState State { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime? ClosedDate { get; set; }
    }

    public enum TicketState
    {
        New = 1,
        Active,
        Pending,
        Resolved,
        Closed,
        Cancelled
    }
}