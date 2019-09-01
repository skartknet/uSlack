using System.Net.Http;
using System.Threading.Tasks;

namespace uSlack.Security
{
    public interface ISecurityService
    {
        Task<bool> IsValidRequestAttemptAsync(HttpRequestMessage request);
    }
}
