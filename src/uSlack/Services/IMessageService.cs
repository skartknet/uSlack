// <copyright file="IMessageService.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Core.Models.Membership;

namespace uSlack.Services
{
    public interface IMessageService
    {        
        Task SendMessageAsync(string service, string evt, IDictionary<string, string> properties = null, IUser user = null);        
    }
}