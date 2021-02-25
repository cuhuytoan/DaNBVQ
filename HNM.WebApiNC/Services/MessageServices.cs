using HNM.DataNC.Models;
using HNM.WebApiNC.Utils;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public AuthMessageSender(IOptions<SMSoptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public SMSoptions Options { get; }  // set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message, Setting setting)
        {
            // Plug in your email service here to send an email.
            Util.SendMail("", email, "", subject, message, setting);
            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.

            // Please check MessageServices_twilio.cs or MessageServices_ASPSMS.cs
            // for implementation details.
            Util.SendSMS(message, number);
            return Task.FromResult(0);
        }
    }
}
