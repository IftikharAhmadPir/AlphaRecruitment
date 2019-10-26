'use strict';
angular.module('app').controller('ResetController', ['$scope', '$location', '$timeout', 'authService', function ($scope, $location, $timeout, authService) {

    $scope.savedSuccessfully = false;
    $scope.message = "";


    $scope.vm= {
        email: ""
    };
    $scope.email = "";

    $scope.resetPassword = function () {

        authService.resetPassword($scope.vm).then(function (response) {
            $scope.savedSuccessfully = true;
            $scope.message = "Your request for reset password has been received please check your email for further instructions";
            startTimer();

        },
         function (response) {
             var errors = [];
             for (var key in response.data.modelState) {
                 for (var i = 0; i < response.data.modelState[key].length; i++) {
                     errors.push(response.data.modelState[key][i]);
                 }
             }
             $scope.message = "Failed to reset password due to:" + errors.join(' ');
         });
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $location.path('/login');
        }, 5000);
    }

}]);