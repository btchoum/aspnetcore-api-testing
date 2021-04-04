using System.Threading.Tasks;
using ServiceDesk.Web.Models;

namespace ServiceDesk.Web.Infrastructure
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage message);
    }
}
