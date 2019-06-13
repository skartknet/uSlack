using System.Threading.Tasks;

namespace uSlack.Services
{
    public interface IMessageService
    {
        Task SendMessageAsync(string txt, string blocks);
    }
}