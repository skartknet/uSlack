function uSlackDashboardController($scope, $http, notificationsService) {
    var vm = this;
    vm.buttonState = "init";
    vm.loadChannelsState = "init";

    vm.configurations = [];
    vm.panelsVisibility = [];

    function init() {        
        $http.get("/umbraco/backoffice/uslack/configurationapi/GetDefaultConfiguration").then(function (res) {
            vm.defaultConfiguration = res.data;
            
            getCurrentConfigurations();
        });
    }

    function getCurrentConfigurations() {
        $http.get("/umbraco/backoffice/uslack/configurationapi/getconfiguration").then(function (res) {

            if (res.data == null || res.data.length <= 0) {
                addNewConfigGroup();
                return;
            }

            vm.configurations = res.data;

            for (var i = 0; i < vm.configurations.length; i++) {

                vm.panelsVisibility.push(false);

                var config = vm.configurations[i];
                if (config.token) {
                    vm.loadChannels(config);
                }
            }
        });
    }

    function addNewConfigGroup() {
        var configCopy = angular.copy(vm.defaultConfiguration);
        configCopy.name = "Configuration " + (vm.configurations.length + 1);
        vm.configurations.push(configCopy);
    }

    function save() {
        return $http.put("/umbraco/backoffice/uslack/configurationapi/saveconfiguration", vm.configurations).then(
            function (res) {
                vm.configurations = res.data;
                notificationsService.success("Success", "Configuration was saved succesfully");

            },
            function (res) {
                //we revert changes if something went wrong
                vm.configurations = vm.tempconfig;
                notificationsService.error("Error", "Configuration couldn't be saved!");
                throw "Error saving configuration";
            }
        );
    }

    vm.togglePanel = function (idx) {
        vm.panelsVisibility[idx] = !vm.panelsVisibility[idx];
    }


    vm.loadChannels = function (config) {
        vm.loadChannelsState = "busy";
        $http.get("/umbraco/backoffice/uslack/configurationapi/loadchannels?token=" + config.token).then(function (res) {
            config.channels = res.data;
            vm.loadChannelsState = "success";
        },
            function (res) {
                vm.loadChannelsState = "error";
                notificationsService.error(res.data.Message, res.data.ExceptionMessage);
            });
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

    vm.toggleSwitch = function (config, section, param) {

        vm.tempconfig = vm.configurations;

        config.sections[section].parameters[param] = !config.sections[section].parameters[param];
        save();
    }

    vm.addNewConfig = function () {
        addNewConfigGroup();
    }

    init();

}

angular.module("umbraco").controller("uslack.dashboard.controller", uSlackDashboardController);