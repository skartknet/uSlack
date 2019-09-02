﻿// <copyright file="IConfiguration.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Newtonsoft.Json;
using System.Collections.Generic;

namespace uSlack.Configuration
{
    public interface IConfiguration
    {
        string Token { get; set; }

        IEnumerable<ConfigurationGroup> Groups { get; }
        ConfigurationGroup DefaultConfigurationGroup { get; }

        IReadOnlyDictionary<string, MessageConfiguration> Messages { get; }
        MessageConfiguration GetMessage(string alias);
        void SaveAppConfiguration(IEnumerable<ConfigurationGroup> model);
    }
   
}