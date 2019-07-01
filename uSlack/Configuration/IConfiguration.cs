﻿// <copyright file="IConfiguration.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Collections.Generic;

namespace uSlack.Configuration
{
    public interface IConfiguration
    {
        IEnumerable<AppConfig> AppConfiguration { get; }
        IDictionary<string, MessageConfiguration> Messages { get; }

        MessageConfiguration GetMessage(string alias);

        void SaveAppConfiguration(IEnumerable<AppConfig> model);
    }
}