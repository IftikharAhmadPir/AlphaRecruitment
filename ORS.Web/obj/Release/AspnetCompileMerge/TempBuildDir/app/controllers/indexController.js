'use strict';
app.controller('indexController', ['$scope', '$location', 'authService', 'cookieService', function ($scope, $location, authService, cookieService) {

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/home');
    }
    $scope.authentication = authService.authentication;
    $scope.authentication.userid = cookieService.readCookie('userid');
}]);