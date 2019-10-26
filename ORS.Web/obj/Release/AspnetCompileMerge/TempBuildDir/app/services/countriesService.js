'use strict';
app.factory('countriesService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var countriesServiceFactory = {};

    var _getCountries = function () {
        return $http.get(serviceBase + 'api/countries').then(function (results) {
            return results;
        });
    };

    var _addCountry = function ($scope) {

        return $http.post(serviceBase + 'api/countries', $scope.country).then(function (results) {
            return results
        });
    };

    var _updateCountryById = function ($stateParams, $scope) {

        return $http.put(serviceBase + 'api/countries/' + $stateParams.id, $scope.country).then(function (results) {
            return results
        });
    };

    var _getCountryById = function ($stateParams) {

        return $http.get(serviceBase + 'api/countries/' + $stateParams.id).then(function (results) {
            return results
        });
    };

    var _deleteCountryById = function ($stateParams) {

        return $http.delete(serviceBase + 'api/countries/' + $stateParams.id).then(function (results) {
            return results
        });
    };


    countriesServiceFactory.getCountries = _getCountries;
    countriesServiceFactory.addCountry = _addCountry;
    countriesServiceFactory.updateCountryById = _updateCountryById;
    countriesServiceFactory.getCountryById = _getCountryById;
    countriesServiceFactory.deleteCountryById = _deleteCountryById;

    return countriesServiceFactory;
}]);