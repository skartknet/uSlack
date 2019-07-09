using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;
using uSlack.Models;

namespace uSlack.Controllers
{
    public class ContentInteractiveController : InteractiveApiController
    {
        private readonly IContentService _contentService;

        public ContentInteractiveController(IContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpPost]
        public IHttpActionResult Unpublish(InteractiveResponse model)
        {
            //var node = _contentService.GetById(id);

            //if (node == null) return NotFound();
            //_contentService.Unpublish(node);

            return Ok();
        }
    }
}
