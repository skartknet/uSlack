// <copyright file="ConfigurationApiController.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using uSlack.Backoffice.Models;
using uSlack.Configuration;
using uSlack.Services;


namespace uSlack.Backoffice
{
    [PluginController("uslack")]
    public class ConfigurationApiController : UmbracoAuthorizedJsonController
    {
        private readonly ManagingService _slackService;
        private readonly IConfiguration _configuration;

        public ConfigurationApiController(ManagingService slackService,
                                            IConfiguration configuration)
        {
            _slackService = slackService;
            _configuration = configuration;
        }

        /// <summary>
        /// Returns all the uSlack configurations for the current site.
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetConfiguration()
        {
            return Ok(_configuration.Groups);
        }

        /// <summary>
        /// Returns a default configuration. Used as a base to create custom configurations.
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetDefaultConfiguration()
        {
            return Ok(_configuration.DefaultConfigurationGroup);
        }

        [HttpPut]
        public IHttpActionResult SaveConfiguration(IEnumerable<ConfigurationGroup> model)
        {
            try
            {
                _configuration.SaveAppConfiguration(model);
            }
            catch (Exception ex)
            {

                Logger.Error(typeof(ConfigurationApiController), ex, "Error saving configuration.");
                return InternalServerError(new Exception("There was an error saving the configuration."));
            }

            return Ok(model);
        }

        [HttpGet]
        public async Task<IHttpActionResult> LoadChannels(string token)
        {

            try
            {
                //TODO: properly manage response and display error messages
                var response = await _slackService.GetChannelsAsync(token);

                if (!response.ok)
                {
                    return new SlackErrorResult(response.error, Request);
                }

                var mappedModels = response.channels.Select(c => new SlackEntity
                {
                    Id = c.Id,
                    Name = c.Name
                });

                return Ok(mappedModels);
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(ConfigurationApiController), ex);
                return InternalServerError(ex);
            }

        }
    }
}
