'use strict';
app.factory('catalogueService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var catalogueServiceFactory = {};

    var _getCountries = function () {
        return $http.get(serviceBase + 'api/countriescat').then(function (results) {
            return results;
        });
    };

    var _getCitiesByCountryId = function ($stateParams) {
        return $http.get(serviceBase + 'api/citiesbycountry/' + $stateParams.id).then(function (results) {
            return results.cities
        });
    };

    catalogueServiceFactory.getCountries = _getCountries;
    catalogueServiceFactory.getCitiesByCountryId = _getCitiesByCountryId;

    return catalogueServiceFactory;
}]);