// <copyright file="IConfigurationService.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Collections.Generic;

namespace uSlack.Configuration
{
    public interface IConfigurationService
    {
        Dictionary<string, string> Messages { get; }
        UslackConfiguration AppConfiguration { get; }

        ConfigurationService Initialize();

        /// <summary>
        /// Saves the configuration
        /// </summary>
        /// <param name="model"></param>
        void SaveAppConfiguration(UslackConfiguration model);    

        /// <summary>
        /// Gets a parameter from the App configuration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        T GetParameter<T>(string parameter, string section);

        /// <summary>
        /// Gets a message body
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        string GetMessage(string alias);
    }
}