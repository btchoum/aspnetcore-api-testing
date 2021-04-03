using System.Threading.Tasks;

namespace ServiceDesk.Web.Controllers
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage message);
    }
}