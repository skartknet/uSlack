// <copyright file="InitUSlack.cs" company="Mario Lopez">
// Copyright (c) 2019 Mario Lopez.
// Licensed under the Apache License, Version 2.0.
// </copyright>

// https://our.umbraco.com/Documentation/implementation/composing/


using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Services.Implement;
using uSlack.Configuration;
using uSlack.EventHandlers;

namespace uSlack
{
    public class InitUSlack : IComponent
    {
        private readonly IConfigurationService _config;
        private readonly ContentHandlers _contentHandlers;
        private readonly MediaHandlers _mediaHandlers;
        private readonly MemberHandlers _memberHandlers;
        private readonly UserHandlers _userHandlers;

        public InitUSlack(IConfigurationService config,
                            ContentHandlers contentHandlers,
                            MediaHandlers mediaHandlers,
                            MemberHandlers memberHandlers,
                            UserHandlers userHandlers)
        {
            _config = config;
            _contentHandlers = contentHandlers;
            _mediaHandlers = mediaHandlers;
            _memberHandlers = memberHandlers;
            _userHandlers = userHandlers;
        }
        // initialize: runs once when Umbraco starts
        public void Initialize()
        {
            _config.Initialize();

            ContentService.Published += _contentHandlers.ContentService_Published;
            ContentService.Unpublished += _contentHandlers.ContentService_Unpublished;
            ContentService.Trashed += _contentHandlers.ContentService_Trashed;
            ContentService.RolledBack += _contentHandlers.ContentService_RolledBack;
            ContentService.Deleted += _contentHandlers.ContentService_Deleted;
            ContentService.Moved += _contentHandlers.ContentService_Moved;

            MediaService.Deleted += _mediaHandlers.MediaService_Deleted;
            MediaService.Moved += _mediaHandlers.MediaService_Moved;
            MediaService.Saved += _mediaHandlers.MediaService_Saved;
            MediaService.Trashed += _mediaHandlers.MediaService_Trashed;

            MemberService.Saved += _memberHandlers.MemberService_Saved;
            MemberService.Deleted += _memberHandlers.MemberService_Deleted;

            UserService.SavedUser += _userHandlers.UserService_SavedUser;
            UserService.DeletedUserGroup += _userHandlers.UserService_DeletedUserGroup;
            UserService.DeletedUser += _userHandlers.UserService_DeletedUser;
        }

        // terminate: runs once when Umbraco stops
        public void Terminate()
        {
            // do something when Umbraco terminates
        }
    }

    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class InitUSlackComposer : ComponentComposer<InitUSlack>
    {
        // nothing needed to be done here!
    }
}