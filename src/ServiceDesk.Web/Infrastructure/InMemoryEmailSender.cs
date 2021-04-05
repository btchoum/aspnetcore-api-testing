using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceDesk.Web.ApiModels;

namespace ServiceDesk.Web.Infrastructure
{
    public class InMemoryEmailSender : IEmailSender
    {
        private readonly List<EmailMessage> _sentEmails = new();

        public IReadOnlyList<EmailMessage> SentMessages => _sentEmails.AsReadOnly();

        public Task SendAsync(EmailMessage message)
        {
            _sentEmails.Add(message);
            
            return Task.CompletedTask;
        }
    }
}
