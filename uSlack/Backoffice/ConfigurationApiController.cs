﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using uSlack.Configuration;

namespace uSlack.Backoffice
{
    [PluginController("uslack")]
    public class ConfigurationApiController : UmbracoAuthorizedJsonController
    {
        private readonly IConfigurationService _config;

        public ConfigurationApiController()
        { }

        public ConfigurationApiController(IConfigurationService config)
        {
            _config = config;
        }
        public IHttpActionResult GetConfiguration()
        {
            return Ok(_config.AppConfiguration);
        }

        [HttpPut]
        public IHttpActionResult SaveConfiguration(UslackConfiguration model)
        {
            return Ok();
        }
    }
}
