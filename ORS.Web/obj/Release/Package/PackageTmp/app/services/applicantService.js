'use strict';
app.factory('applicantService', ['$http', 'ngAuthSettings', 'notificationService', 'validationService', function ($http, ngAuthSettings, notificationService, validationService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var applicantServiceFactory = {};

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

    var _getpersonalinfo = function ($stateParams) {
        return $http.get(serviceBase + 'api/personalinfo/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) {
            return validationService.validationplusexception(response);
        }

        );
    };
    var _addpersonalinfo = function ($stateParams, $scope) {
        return $http.post(serviceBase + 'api/' + $stateParams.id + '/addpersonalinfo', $scope.personalinfo).then(function (results) {
            notificationService.success('Secessfully Update', 'bottom_right');
            return results
        }, function (response) {
        return _allerrorfound(response);
        });
    };
    var _geteducationinfo = function ($stateParams) {
        return $http.get(serviceBase + 'api/educationinfo/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) {

            return _validationplusexception(response);
        });
    };
    var _addeducationinfo = function ($stateParams, $scope) {
        return $http.post(serviceBase + 'api/' + $stateParams.id + '/addeducationinfo', $scope.educationinfo).then(function (results) {
            return results
        }, function (response) {
            return _allerrorfound(response);
        });
    };
    var _geteducationbyid = function ($stateParams) {
        return $http.get(serviceBase + 'api/educationbyid/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) { return _validationplusexception(response); });
    };
    var _editeducationbyid = function ($stateParams, $scope) {
        return $http.put(serviceBase + 'api/editeducationbyid/' + $stateParams.id, $scope.educationinfo).then(function (results) {
            return results
        }, function (response) {
            return _allerrorfound(response);
        });
    };
    var _deleducationinfo = function (id) {
        return $http.delete(serviceBase + 'api/deleducation/' + id).then(function (results) {
            return results
        }, function (response) { return _validationplusexception(response); });
    };

    var _getexperienceinfo = function ($stateParams) {
        return $http.get(serviceBase + 'api/experienceinfo/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) {
            return _validationplusexception(response);
        });
    };
    var _addexperienceinfo = function ($stateParams, $scope) {
        return $http.post(serviceBase + 'api/' + $stateParams.id + '/addexperienceinfo', $scope.experienceinfo).then(function (data) {
            return data
        }, function (response) {
            return _allerrorfound(response);
        }
      );
    };
    var _getexperiencebyid = function ($stateParams) {
        return $http.get(serviceBase + 'api/experiencebyid/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) { return _validationplusexception(response); });
    };
    var _editexperiencebyid = function ($stateParams, $scope) {
        return $http.put(serviceBase + 'api/editexperiencebyid/' + $stateParams.id, $scope.experienceinfo).then(function (results) {
            return results
        }, function (response) {return _allerrorfound(response);});
    };
    var _delexperienceinfo = function (id) {
        return $http.delete(serviceBase + 'api/delexperience/' + id).then(function (results) {
            return results
        }, function (response) { return _validationplusexception(response); });
    };

    var _getpublicationinfo = function ($stateParams) {
        return $http.get(serviceBase + 'api/publicationinfo/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) {
            return _validationplusexception(response);
        });
    };
    var _addpublicationinfo = function ($stateParams, $scope) {
        return $http.post(serviceBase + 'api/' + $stateParams.id + '/addpublicationinfo', $scope.publicationinfo).then(function (data) {
            return data
        }, function (response) {
            return _allerrorfound(response);
        }
      );
    };
    var _getpublicationbyid = function ($stateParams) {
        return $http.get(serviceBase + 'api/publicationbyid/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) { return _validationplusexception(response); });
    };
    var _editpublicationbyid = function ($stateParams, $scope) {
        return $http.put(serviceBase + 'api/editpublicationbyid/' + $stateParams.id, $scope.publicationinfo).then(function (results) {
            return results
        }, function (response) { return _allerrorfound(response); });
    };
    var _delpublicationinfo = function (id) {
        return $http.delete(serviceBase + 'api/delpublication/' + id).then(function (results) {
            return results
        }, function (response) { return _validationplusexception(response); });
    };

    var _gettraininginfo = function ($stateParams) {
        return $http.get(serviceBase + 'api/traininginfo/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) {
            return _validationplusexception(response);
        });
    };
    var _addtraininginfo = function ($stateParams, $scope) {
        return $http.post(serviceBase + 'api/' + $stateParams.id + '/addtraininginfo', $scope.traininginfo).then(function (data) {
            return data
        }, function (response) {
            return _allerrorfound(response);
        }
      );
    };
    var _gettrainingbyid = function ($stateParams) {
        return $http.get(serviceBase + 'api/trainingbyid/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) { return _validationplusexception(response); });
    };
    var _edittrainingbyid = function ($stateParams, $scope) {
        return $http.put(serviceBase + 'api/edittrainingbyid/' + $stateParams.id, $scope.traininginfo).then(function (results) {
            return results
        }, function (response) { return _allerrorfound(response); });
    };
    var _deltraininginfo = function (id) {
        return $http.delete(serviceBase + 'api/deltraining/' + id).then(function (results) {
            return results
        }, function (response) { return _validationplusexception(response); });
    };

    var _getmembershipinfo = function ($stateParams) {
        return $http.get(serviceBase + 'api/membershipinfo/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) {
            return _validationplusexception(response);
        });
    };
    var _addmembershipinfo = function ($stateParams, $scope) {
        return $http.post(serviceBase + 'api/' + $stateParams.id + '/addmembershipinfo', $scope.membershipinfo).then(function (data) {
            return data
        }, function (response) {
            return _allerrorfound(response);
        }
      );
    };
    var _getmembershipbyid = function ($stateParams) {
        return $http.get(serviceBase + 'api/membershipbyid/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) { return _validationplusexception(response); });
    };
    var _editmembershipbyid = function ($stateParams, $scope) {
        return $http.put(serviceBase + 'api/editmembershipbyid/' + $stateParams.id, $scope.membershipinfo).then(function (results) {
            return results
        }, function (response) { return _allerrorfound(response); });
    };
    var _delmembershipinfo = function (id) {
        return $http.delete(serviceBase + 'api/delmembership/' + id).then(function (results) {
            return results
        }, function (response) { return _validationplusexception(response); });
    };

    var _getlanguageinfo = function ($stateParams) {
        return $http.get(serviceBase + 'api/languageinfo/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) {
            return _validationplusexception(response);
        });
    };
    var _addlanguageinfo = function ($stateParams, $scope) {
        return $http.post(serviceBase + 'api/' + $stateParams.id + '/addlanguageinfo', $scope.languageinfo).then(function (data) {
            return data
        }, function (response) {
            return _allerrorfound(response);
        }
      );
    };
    var _getlanguagebyid = function ($stateParams) {
        return $http.get(serviceBase + 'api/languagebyid/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) { return _validationplusexception(response); });
    };
    var _editlanguagebyid = function ($stateParams, $scope) {
        return $http.put(serviceBase + 'api/editlanguagebyid/' + $stateParams.id, $scope.languageinfo).then(function (results) {
            return results
        }, function (response) { return _allerrorfound(response); });
    };
    var _dellanguageinfo = function (id) {
        return $http.delete(serviceBase + 'api/dellanguage/' + id).then(function (results) {
            return results
        }, function (response) { return _validationplusexception(response); });
    };

    var _getreferenceinfo = function ($stateParams) {
        return $http.get(serviceBase + 'api/referenceinfo/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) {
            return _validationplusexception(response);
        });
    };
    var _addreferenceinfo = function ($stateParams, $scope) {
        return $http.post(serviceBase + 'api/' + $stateParams.id + '/addreferenceinfo', $scope.referenceinfo).then(function (data) {
            return data
        }, function (response) {
            return validationService.allerrorfound(response);
        }
      );
    };
    var _getreferencebyid = function ($stateParams) {
        return $http.get(serviceBase + 'api/referencebyid/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) { return _validationplusexception(response); });
    };
    var _editreferencebyid = function ($stateParams, $scope) {
        return $http.put(serviceBase + 'api/editreferencebyid/' + $stateParams.id, $scope.referenceinfo).then(function (results) {
            return results
        }, function (response) { return _allerrorfound(response); });
    };
    var _delreferenceinfo = function (id) {
        return $http.delete(serviceBase + 'api/delreference/' + id).then(function (results) {
            return results
        }, function (response) { return _validationplusexception(response); });
    };


    var _getjobinfo = function ($stateParams) {
        return $http.get(serviceBase + 'api/jobinfo/' + $stateParams.userid).then(function (results) {
            return results
        }, function (response) {
            return _validationplusexception(response);
        });
    };
    var _getjobbyid = function ($stateParams) {
        return $http.get(serviceBase + 'api/jobdetails/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) { return _validationplusexception(response); });
    };
    var _applyforjob = function ($stateParams) {
        return $http.post(serviceBase + 'api/applyforjob/' + $stateParams.userid+ '/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) { return _allerrorfound(response); });
    };

    var _getappinfo = function ($stateParams) {
        return $http.get(serviceBase + 'api/userappsinfo/' + $stateParams.userid).then(function (results) {
            return results
        }, function (response) {
            return _validationplusexception(response);
        });
    };
    var _getappbyid = function ($stateParams) {
        return $http.get(serviceBase + 'api/jobdetails/' + $stateParams.id).then(function (results) {
            return results
        }, function (response) { return _validationplusexception(response); });
    };

    var _getinterviewsinfo = function ($stateParams) {
        return $http.get(serviceBase + 'api/userinterviewinfo/' + $stateParams.userid).then(function (results) {
            return results
        }, function (response) {
            return _validationplusexception(response);
        });
    };

    applicantServiceFactory.validationplusexception = _validationplusexception;
    applicantServiceFactory.allerrorfound = _allerrorfound;

    applicantServiceFactory.addpersonalinfo = _addpersonalinfo;
    applicantServiceFactory.getpersonalinfo = _getpersonalinfo;

    applicantServiceFactory.geteducationinfo = _geteducationinfo;
    applicantServiceFactory.addeducationinfo = _addeducationinfo;
    applicantServiceFactory.geteducationbyid = _geteducationbyid;
    applicantServiceFactory.editeducationbyid = _editeducationbyid;
    applicantServiceFactory.deleducationinfo = _deleducationinfo;

    applicantServiceFactory.getexperienceinfo = _getexperienceinfo;
    applicantServiceFactory.addexperienceinfo = _addexperienceinfo;
    applicantServiceFactory.getexperiencebyid = _getexperiencebyid;
    applicantServiceFactory.editexperiencebyid = _editexperiencebyid;
    applicantServiceFactory.delexperienceinfo = _delexperienceinfo;

    applicantServiceFactory.getpublicationinfo = _getpublicationinfo;
    applicantServiceFactory.addpublicationinfo = _addpublicationinfo;
    applicantServiceFactory.getpublicationbyid = _getpublicationbyid;
    applicantServiceFactory.editpublicationbyid = _editpublicationbyid;
    applicantServiceFactory.delpublicationinfo = _delpublicationinfo;

    applicantServiceFactory.gettraininginfo = _gettraininginfo;
    applicantServiceFactory.addtraininginfo = _addtraininginfo;
    applicantServiceFactory.gettrainingbyid = _gettrainingbyid;
    applicantServiceFactory.edittrainingbyid = _edittrainingbyid;
    applicantServiceFactory.deltraininginfo = _deltraininginfo;

    applicantServiceFactory.getmembershipinfo = _getmembershipinfo;
    applicantServiceFactory.addmembershipinfo = _addmembershipinfo;
    applicantServiceFactory.getmembershipbyid = _getmembershipbyid;
    applicantServiceFactory.editmembershipbyid = _editmembershipbyid;
    applicantServiceFactory.delmembershipinfo = _delmembershipinfo;

    applicantServiceFactory.getlanguageinfo = _getlanguageinfo;
    applicantServiceFactory.addlanguageinfo = _addlanguageinfo;
    applicantServiceFactory.getlanguagebyid = _getlanguagebyid;
    applicantServiceFactory.editlanguagebyid = _editlanguagebyid;
    applicantServiceFactory.dellanguageinfo = _dellanguageinfo;

    applicantServiceFactory.getreferenceinfo = _getreferenceinfo;
    applicantServiceFactory.addreferenceinfo = _addreferenceinfo;
    applicantServiceFactory.getreferencebyid = _getreferencebyid;
    applicantServiceFactory.editreferencebyid = _editreferencebyid;
    applicantServiceFactory.delreferenceinfo = _delreferenceinfo;

    applicantServiceFactory.getjobinfo = _getjobinfo;
    applicantServiceFactory.getjobbyid = _getjobbyid;
    applicantServiceFactory.applyforjob = _applyforjob;

    applicantServiceFactory.getappinfo = _getappinfo;
    applicantServiceFactory.getappbyid = _getappbyid;
    
    applicantServiceFactory.getinterviewsinfo = _getinterviewsinfo;
    return applicantServiceFactory;
}]);