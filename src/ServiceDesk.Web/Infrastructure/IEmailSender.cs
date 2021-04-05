using System.Threading.Tasks;
using ServiceDesk.Web.ApiModels;

namespace ServiceDesk.Web.Infrastructure
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage message);
    }
}
