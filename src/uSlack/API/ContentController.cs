using System.Threading.Tasks;
using System.Web.Http;
using uSlack.Interactive;

namespace uSlack.API
{
    public class ContentController : InteractiveApiControllerBase
    {

        public async Task<IHttpActionResult> Unpublish(string src)
        {
            if (int.TryParse(src, out int id))
            {
                var node = Services.ContentService.GetById(id);
                if (node == null) return BadRequest("Node doesn't exist");
                Services.ContentService.Unpublish(node);
            }
            else
            {
                return BadRequest("Value passed is not a valid id. It needs to be an integer.");
            }

            return Ok();
        }
    }
}
