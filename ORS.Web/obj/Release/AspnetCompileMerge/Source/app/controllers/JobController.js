'use strict';
app.controller('JobController', ['$scope', 'countriesService', function ($scope, countriesService) {

    $scope.countries = [];
    countriesService.getCountries().then(function (results) {
        $scope.countries = results.data;
    }, function (error) {
        //alert(error.data.message);
    });

}])
app.controller('countriesedit', ['$scope', '$stateParams', '$state', 'countriesService', function ($scope, $stateParams, $state, countriesService) {

    countriesService.getCountryById($stateParams).then(function (results) {
        $scope.country = results.data;
    }, function (error) {
        //alert(error.data.message);
    });

    $scope.save = function () {
        countriesService.updateCountryById($stateParams,$scope).then(function (response) {
            $state.go("cdashboard.countries");
        });
    }
}]);
app.controller('countriesdelete', ['$scope', '$stateParams', '$state', 'countriesService', function ($scope, $stateParams, $state, countriesService) {

    countriesService.getCountryById($stateParams).then(function (results) {
        $scope.country = results.data;
    }, function (error) {
        //alert(error.data.message);
    });

    $scope.save = function () {
        countriesService.deleteCountryById($stateParams).then(function (response) {
            $state.go("cdashboard.countries");
        });
    }
}]);
app.controller('countriesadd', ['$scope', '$state', 'countriesService', function ($scope, $state, countriesService) {

    $scope.country  = {};

    $scope.save = function () {
        countriesService.addCountry($scope).then(function (response) {
            $state.go("cdashboard.countries");

        });
    }

}]);