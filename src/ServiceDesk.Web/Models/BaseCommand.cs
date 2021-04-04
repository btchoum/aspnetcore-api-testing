using System;

namespace ServiceDesk.Web.Models
{
    public class BaseCommand
    {
        public DateTime Now { get; set; } = DateTime.UtcNow;
    }
}