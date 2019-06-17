function uSlackDashboardController($scope, $http, notificationsService) {
    var vm = this;
    vm.buttonState = "init";



    $http.get("/umbraco/backoffice/uslack/configurationapi/getconfiguration").then(function (res) {
        vm.config = res.data;
    });

    vm.save = function () {
        return $http.put("/umbraco/backoffice/uslack/configurationapi/saveconfiguration", vm.config).then(
            function (res) {
                vm.config = res.data;
                notificationsService.success("Success", "Configuration was saved succesfully");

            },
            function (res) {
                //we revert changes if something went wrong
                vm.config = vm.tempconfig;
                notificationsService.error("Error", "Configuration couldn't be saved!");

            }
        );


    }

    vm.btnSave = function(){
        vm.buttonState = "busy";
        vm.save().then(function () {
            vm.buttonState = "success";
        },
            function () {
                vm.buttonState = "error";
            });
    }

    vm.toggleSwitch = function (section, param) {

        vm.tempconfig = vm.config;

        vm.config.sections[section].parameters[param] = !vm.config.sections[section].parameters[param];
        vm.save();
    }
}

angular.module("umbraco").controller("uslack.dashboard.controller", uSlackDashboardController);