// <copyright file="IConfiguration.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Collections.Generic;

namespace uSlack.Configuration
{
    public interface IConfiguration
    {
        AppConfiguration AppConfiguration { get; }
        Dictionary<string, string> Messages { get; }

        string GetMessage(string alias);
        T GetParameter<T>(string parameter, string section);
        void SaveAppConfiguration(AppConfiguration model);
    }
}