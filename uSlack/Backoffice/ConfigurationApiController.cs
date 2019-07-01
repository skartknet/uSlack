// <copyright file="ConfigurationApiController.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Core.Composing;
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
        private readonly IMessageService _msgService;

        public ConfigurationApiController(IMessageService msgService)
        {
            _msgService = msgService;
        }

        public IHttpActionResult GetConfiguration()
        {
            return Ok(UslackConfiguration.Current.AppConfiguration);
        }

        [HttpPut]
        public IHttpActionResult SaveConfiguration(AppConfigurationList model)
        {
            try
            {
                UslackConfiguration.Current.SaveAppConfiguration(model);
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
                var response = await _msgService.GetChannelsAsync(token);

                if (!response.ok)
                {
                    return new SlackErrorResult(response.error, Request);
                }

                var mappedModels = response.channels.Select(c => new SlackEntity
                {
                    Id = c.id,
                    Name = c.name
                });

                return Ok(mappedModels);
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(ConfigurationApiController), ex);
                return InternalServerError();
            }

        }
    }
}
