// <copyright file="IMessageService.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Threading.Tasks;

namespace uSlack.Services
{
    public interface IMessageService
    {
        Task SendMessageAsync(string txt, string blocks);
    }
}