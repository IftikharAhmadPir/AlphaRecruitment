'use strict';
angular.module('app').controller('changePasswordController', ['$scope', '$location', '$timeout', 'authService', 'cookieService', '$state', function ($scope, $location, $timeout, authService, cookieService, $state) {

    $scope.savedSuccessfully = false;
    $scope.message = "";
    $scope.vm = {
        userId: cookieService.readCookie('userid'),
        currentPassword: "",
        password: "",
        confirmPassword: ""
    };

    $scope.ChangePassword = function () {
        if ($scope.vm.password != $scope.vm.confirmPassword) {
            $scope.savedSuccessfully = true;
            $scope.message = "Confirmation mismatch";
            return null;
        };
        authService.changePassword($scope.vm).then(function (response) {
            $scope.savedSuccessfully = true;
            $scope.message = "Password Updated successfully";
            //startTimer();

        },
         function (response) {
             var errors = [];
             for (var key in response.data.modelState) {
                 for (var i = 0; i < response.data.modelState[key].length; i++) {
                     errors.push(response.data.modelState[key][i]);
                 }
             }
             $scope.message = "Failed to change password due to:" + errors.join(' ');
         });
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $state.go('^');
//            $location.path('/empjobslist');
        }, 2000);
    }

}]);