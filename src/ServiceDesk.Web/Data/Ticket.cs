using System;
using System.Collections.Generic;

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

        public List<Comment> Comments { get; set; }

        public Ticket()
        {
            Comments = new List<Comment>();
        }
    }
}
