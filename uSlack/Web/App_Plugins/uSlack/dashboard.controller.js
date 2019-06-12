function uSlackDashboardController($scope, $http) {
    var vm = this;

    


    $http.get("/umbraco/backoffice/uslack/configurationapi/getconfiguration").then(function (res) {
        vm.config = res.data;
    });
}

angular.module("umbraco").controller("uslack.dashboard.controller", uSlackDashboardController);