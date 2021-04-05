using System;

namespace ServiceDesk.Web.ApiModels
{
    public class BaseCommand
    {
        public DateTime Now { get; set; } = DateTime.UtcNow;
    }
}