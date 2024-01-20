using System.Threading.Tasks;

namespace Hyper.Services.Messages
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string message, params string[] gsmNumbers);
    }
}
