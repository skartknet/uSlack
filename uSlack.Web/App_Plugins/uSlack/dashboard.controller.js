function uSlackDashboardController($scope) {
    var vm = this;

    vm.config = {
        contentService: {            
            published: true,
            unpublished: true,
            deleted: true,
            moved: true,
            rolledBack: true,
            trashed: true,
        },
        mediaService: {
            saved: true,
            deleted: true,
            moved: true,
            trashed: true,            
        },
        memberService: {
            saved: true,
            deleted: true,            
        },
        userService: {
            saved: true,
            deleted: true,
            userGroupDeleted: true
        }
    }
}

angular.module("umbraco").controller("uslack.dashboard.controller", uSlackDashboardController);