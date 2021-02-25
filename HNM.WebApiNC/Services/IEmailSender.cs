using HNM.DataNC.Models;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, Setting setting);
    }
}

