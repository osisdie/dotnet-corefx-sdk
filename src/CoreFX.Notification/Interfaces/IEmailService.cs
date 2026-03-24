using System.Threading.Tasks;
using CoreFX.Abstractions.Bases.Interfaces;

namespace CoreFX.Notification.Interfaces
{
    public interface IEmailService
    {
        Task<ISvcResponseBaseDto> SendAsync(string subject, string html, string from = null, string to = null);
    }
}
