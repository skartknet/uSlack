function uSlackDashboardController($scope, $http, notificationsService, authResource) {
    var vm = this;
    vm.buttonState = "init";
    vm.loadChannelsState = "init";
    vm.channels;
    vm.appsettings = { token: undefined, configurationGroups: [] };
    vm.panelsVisibility = [];

    function init() {
        $http.get("/umbraco/backoffice/uslack/configurationapi/GetDefaultConfiguration").then(function (res) {
            vm.defaultConfiguration = res.data;

            setConfiguration();
            getUserGroups();
            vm.loadChannels();
        });
    }

    function getUserGroups() {
        authResource.getCurrentUser().then(function (data) {
            vm.userGroups = data.userGroups;
        });
    }

    function setConfiguration() {
        $http.get("/umbraco/backoffice/uslack/configurationapi/getconfiguration").then(function (res) {

            if (!res.data) {
                addNewConfigGroup();
                return;
            }
            vm.appsettings = res.data;

            for (var i = 0; i < vm.appsettings.configurationGroups.length; i++) {

                vm.panelsVisibility.push(false);

                var config = vm.appsettings.configurationGroups[i];
                if (vm.appsettings.token) {
                    vm.loadChannels(config);
                }
            }
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

        if (!vm.appsettings.token) return;

        vm.loadChannelsState = "busy";
        $http.get("/umbraco/backoffice/uslack/configurationapi/loadchannels?token=" + vm.appsettings.token).then(function (res) {
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

        vm.tempconfig = vm.appsettings;

        config.sections[section].handlers[handler] = !config.sections[section].handlers[handler];
        save();
    }

    vm.addNewConfig = function () {
        addNewConfigGroup();
    }

    init();

}

angular.module("umbraco").controller("uslack.dashboard.controller", uSlackDashboardController);