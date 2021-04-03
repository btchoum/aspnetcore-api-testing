using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceDesk.Web.Controllers
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