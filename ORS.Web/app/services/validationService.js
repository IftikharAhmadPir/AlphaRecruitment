    'use strict';
    app.factory('validationService', ['notificationService', function (notificationService) {

        var validationServiceFactory = {};
        var _validationplusexception = function ($scope) {

            if (($scope.statusText) && ($scope.statusText != "Bad Request")) {
                $scope.errormsg = $scope.statusText;
                notificationService.error($scope.errormsg);
            }
            if ($scope.data.exceptionMessage) {
                $scope.errormsg = $scope.data.exceptionMessage;
                notificationService.error($scope.errormsg);
            } return null;
        };
        var _allerrorfound = function ($scope) {
            if ($scope.data.modelState) {
                for (var key in $scope.data.modelState) {
                    $scope.errormsg = $scope.data.modelState[key] + "\r\n";
                    notificationService.error($scope.errormsg);
                }
            }
            if (($scope.statusText) && ($scope.statusText != "Bad Request")) {
                $scope.errormsg = $scope.statusText;
                notificationService.error($scope.errormsg);
            }
            if ($scope.data.exceptionMessage) {
                $scope.errormsg = $scope.data.exceptionMessage;
                notificationService.error($scope.errormsg);
            }
            return null;
        };

        validationServiceFactory.validationplusexception = _validationplusexception;
        validationServiceFactory.allerrorfound = _allerrorfound;
        return validationServiceFactory;
    }]);