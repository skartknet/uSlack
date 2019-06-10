using System.Threading.Tasks;

namespace uSlack
{
    public interface IMessageService
    {
        Task SendMessageAsync(string txt, string blocks);
    }
}