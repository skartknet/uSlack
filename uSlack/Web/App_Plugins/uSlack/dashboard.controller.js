function uSlackDashboardController($scope, $http) {
    var vm = this;
    vm.buttonState = "init";



    $http.get("/umbraco/backoffice/uslack/configurationapi/getconfiguration").then(function (res) {
        vm.config = res.data;
    });

    vm.save = function () {

        vm.buttonState = "busy";
        $http.put("/umbraco/backoffice/uslack/configurationapi/saveconfiguration", vm.config).then(
            function (res) {
                vm.config = res.data;
                console.log("saved succesful");
                vm.buttonState = "success";

            },
            function (res) {
                //we revert changes if something went wrong
                vm.config = vm.tempconfig;
                console.log("error saving");
                vm.buttonState = "error";
            }
        );


    }

    vm.toggleSwitch = function (section, param) {

        vm.tempconfig = vm.config;

        vm.config.sections[section].parameters[param] = !vm.config.sections[section].parameters[param];
        vm.save();
    }
}

angular.module("umbraco").controller("uslack.dashboard.controller", uSlackDashboardController);