﻿function uSlackDashboardController($scope, $http, notificationsService, authResource, userService, userGroupsResource) {
    var vm = this;
    vm.buttonState = "init";
    vm.loadChannelsState = "init";
    vm.channels;
    vm.appsettings = { configurationGroups: [] };
    vm.panelsVisibility = [];

    function init() {
        $http.get("/umbraco/backoffice/uslack/configurationapi/GetDefaultConfiguration").then(function (res) {
            vm.defaultConfiguration = res.data;
            initConfiguration();
        }, function (res) {
            notificationsService.error("Error", "Default configuration couldn't be created");
        });
    }



    function initConfiguration() {
        $http.get("/umbraco/backoffice/uslack/configurationapi/getconfiguration").then(function (res) {

            vm.appsettings = res.data;

            if (!vm.appsettings.configurationGroups) {
                vm.appsettings.configurationGroups = [];
                addNewConfigGroup();
            }

            vm.panelsVisibility = new Array(vm.appsettings.configurationGroups.length).fill(false);

            getUserGroups();
            vm.loadChannels();
        }, function (res) {
            notificationsService.error("Error", "Error loading configuration");
        });
    }

    function getUserGroups() {
        userService.getCurrentUser().then(function (user) {
            currentUser = user;
            // Get usergroups
            userGroupsResource.getUserGroups({ onlyCurrentUserGroups: false }).then(function (userGroups) {

                // only allow editing and selection if user is member of the group or admin
                vm.userGroups = _.map(userGroups, function (ug) {
                    ug.hasAccess = user.userGroups.indexOf(ug.alias) !== -1 || user.userGroups.indexOf("admin") !== -1;
                    return ug;
                });
                vm.filteredUserGroups = vm.userGroups;
            });
        });
    }


    function addNewConfigGroup() {
        var configCopy = angular.copy(vm.defaultConfiguration);
        configCopy.name = "Configuration " + (vm.appsettings.configurationGroups.length + 1);
        vm.appsettings.configurationGroups.push(configCopy);
    }


    function save() {
        return $http.put("/umbraco/backoffice/uslack/configurationapi/saveconfiguration", vm.appsettings).then(
            function (res) {
                vm.appsettings = res.data;
                notificationsService.success("Success", "Configuration was saved succesfully");

            },
            function (res) {
                //we revert changes if something went wrong
                vm.appsettings = vm.tempconfig;
                notificationsService.error("Error", "Configuration couldn't be saved!");
                throw "Error saving configuration";
            }
        );
    }

    vm.togglePanel = function (idx) {
        vm.panelsVisibility[idx] = !vm.panelsVisibility[idx];
    }


    vm.loadChannels = function () {


        vm.loadChannelsState = "busy";
        $http.get("/umbraco/backoffice/uslack/configurationapi/loadchannels").then(function (res) {
            vm.channels = res.data;
            vm.loadChannelsState = "success";
        },
            function (res) {
                vm.loadChannelsState = "error";
                notificationsService.error(res.data.Message, res.data.ExceptionMessage);
            });
    }

    vm.selectAllGroups = function (config) {
        config.groups = angular.copy(vm.userGroups);
    }

    vm.btnSave = function () {
        vm.buttonState = "busy";
        save().then(function () {
            vm.buttonState = "success";
        },
            function () {
                vm.buttonState = "error";
            });
    }

    vm.toggleSwitch = function (config, section, handler) {
        config.sections[section].handlers[handler].value = !config.sections[section].handlers[handler].value;
    }

    vm.addNewConfig = function () {
        addNewConfigGroup();
    }

    vm.deleteGroup = function (idx) {
        vm.appsettings.configurationGroups.splice(idx, 1);
    }

    init();

}

angular.module("umbraco").controller("uslack.dashboard.controller", uSlackDashboardController);