using System.Threading.Tasks;

namespace HNM.WebApiNC.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
