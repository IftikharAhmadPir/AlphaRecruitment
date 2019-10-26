'use strict';
app.factory('employerService', ['$http', 'ngAuthSettings', 'validationService', function ($http, ngAuthSettings, validationService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var employerServiceFactory = {};

    var _updateEmployerById = function ($stateParams, $scope) {
        return $http.post(serviceBase + 'api/' + $stateParams.id + '/addemployerinfo', $scope).then(function (results) {
        //return $http.put(serviceBase + 'api/Employer/' + $stateParams.id, $scope).then(function (results) {
            return results
        }, function (response) {
            return validationService.allerrorfound(response);
        });
    };
    var _getEmployerById = function ($stateParams) {
        return $http.get(serviceBase + 'api/Employer/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) {
            return validationService.validationplusexception(response);
        });
    };

    var _postJob = function ($stateParams, $scope) {
        return $http.post(serviceBase + 'api/' +  $stateParams.id + '/Job', $scope.job).then(function (results) {
            return results
        }, function (response) {
            return validationService.allerrorfound(response);
        });
    };
    var _getEmployerJobs = function ($stateParams) {
        return $http.get(serviceBase + 'api/job/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) {
            return validationService.validationplusexception(response);
        });
    };
    var _getJobById = function ($stateParams) {
        return $http.get(serviceBase + 'api/jobbyid/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) {
            return validationService.validationplusexception(response);
        });
    };
    var _editJobById = function ($stateParams, $scope) {
        return $http.put(serviceBase + 'api/editjobbyid/' + $stateParams.id, $scope.job).then(function (results) {
            return results
        }, function (response) {
            return validationService.allerrorfound(response);
        });
    };
    var _deleteJobById = function (id) {
        return $http.delete(serviceBase + 'api/deljob/' + id).then(function (results) {
            return results
        }, function (response) {
            return validationService.validationplusexception(response);
        });
    };

    var _getReceivedApps = function ($stateParams) {
        return $http.get(serviceBase + 'api/jobwithappscount/' + $stateParams.empid).then(function (results) {
            return results
        }, function (response) {
            return validationService.validationplusexception(response);
        });
    };
    var _getJobApplicants = function (jobid) {
        return $http.get(serviceBase + 'api/jobapplicants/' + jobid).then(function (results) {
            return results
        }, function (response) {
            return validationService.validationplusexception(response);
        });
    };

    var _getApplicantCV = function (applicantid) {
        return $http.get(serviceBase + 'api/jobapplicantCv/' + applicantid).then(function (results) {
            return results
        }, function (response) {
            return validationService.validationplusexception(response);
        });
    };

    var _shortListApplicant = function (appid) {
        return $http.post(serviceBase + 'api/shortlistapplicant/' + appid).then(function (results) {
            return results
        }, function (response) {
            return validationService.validationplusexception(response);
        });
    };

    var _getShortlistedApps = function ($stateParams) {
        return $http.get(serviceBase + 'api/shortlistedjobwithappscount/' + $stateParams.empid).then(function (results) {
            return results
        }, function (response) {
            return validationService.validationplusexception(response);
        });
    };
    var _getJobShortlistedApplicants = function (jobid) {
        return $http.get(serviceBase + 'api/jobshortlistedapplicants/' + jobid).then(function (results) {
            return results
        }, function (response) {
            return validationService.validationplusexception(response);
        });
    };
    var _callForInterview = function ($scope) {
        console.log($scope.interview);
        return $http.post(serviceBase + 'api/callforinterview/', $scope.interview).then(function (results) {
            return results
        }, function (response) {
            return validationService.validationplusexception(response);
        });
    };

    var _getAppswithpendinginterviews = function ($stateParams) {
        return $http.get(serviceBase + 'api/jobwithappsinterviewscount/' + $stateParams.empid).then(function (results) {
            return results
        }, function (response) {
            return validationService.validationplusexception(response);
        });
    };
    var _getPendingInterviewApplicants = function (jobid) {
        return $http.get(serviceBase + 'api/jobapplicantsinterviews/' + jobid).then(function (results) {
            return results
        }, function (response) {
            return validationService.validationplusexception(response);
        });
    };
    employerServiceFactory.updateEmployerById = _updateEmployerById;
    employerServiceFactory.getEmployerById = _getEmployerById;

    employerServiceFactory.getEmployerJobs = _getEmployerJobs;
    employerServiceFactory.postJob = _postJob;
    employerServiceFactory.getJobById = _getJobById;
    employerServiceFactory.editJobById = _editJobById;
    employerServiceFactory.deleteJobById = _deleteJobById;

    employerServiceFactory.getReceivedApps = _getReceivedApps;
    employerServiceFactory.getJobApplicants = _getJobApplicants;
    employerServiceFactory.getApplicantCV = _getApplicantCV;
    employerServiceFactory.shortListApplicant = _shortListApplicant;

    employerServiceFactory.getShortlistedApps = _getShortlistedApps;
    employerServiceFactory.getJobShortlistedApplicants = _getJobShortlistedApplicants;

    employerServiceFactory.callForInterview = _callForInterview;

    employerServiceFactory.getAppswithpendinginterviews = _getAppswithpendinginterviews;
    employerServiceFactory.getPendingInterviewApplicants = _getPendingInterviewApplicants;
    
    

    return employerServiceFactory;
}]);