using System.Threading.Tasks;

namespace uSlack
{
    public interface IMessageService
    {
        Task SendMessageAsync(string message);
    }
}