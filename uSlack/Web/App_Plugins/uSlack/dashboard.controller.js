function uSlackDashboardController($scope, $http) {
    var vm = this;

    


    $http.get("/umbraco/backoffice/uslack/configurationapi/getconfiguration").then(function (res) {
        vm.config = res.data;
    });

    vm.save = function () {
        $http.put("/umbraco/backoffice/uslack/configurationapi/saveconfiguration", vm.config).then(function (res) {
            vm.config = res.data;
        });
    }

    vm.toggleSwitch = function (section, param) {
        vm.config.sections[section].parameters[param] = !vm.config.sections[section].parameters[param];
        vm.save();
    }
}

angular.module("umbraco").controller("uslack.dashboard.controller", uSlackDashboardController);