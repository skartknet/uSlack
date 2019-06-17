// <copyright file="ConfigurationApiController.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Newtonsoft.Json;
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
            try
            {
                _config.SaveAppConfiguration(model);
            }
            catch (Exception ex)
            {

                Logger.Error(typeof(ConfigurationApiController), ex, "Error saving configuration.");
                return InternalServerError(new Exception("There was an error saving the configuration."));
            }

            return Ok(model);
        }
    }
}
