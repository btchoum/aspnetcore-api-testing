using System;

namespace ServiceDesk.Web.Data
{
    public class Comment
    {
        public long Id { get; set; }
        public long TicketId { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
    }
}