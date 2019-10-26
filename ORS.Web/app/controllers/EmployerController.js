'use strict';
app.controller('EmployerController', ['$scope', '$stateParams', '$state', 'employerService', 'cookieService', 'catalogueService', function ($scope, $stateParams, $state, employerService, cookieService, catalogueService) {
    
    $stateParams.id = cookieService.readCookie('userid');
    $scope.employer = [
        {
            countries_Id: null
        }
    ];
    $scope.itemArray = [
        {},
    ];
    $scope.selected = { value: $scope.itemArray[0] };

    catalogueService.getCountries().then(function (results) {
        if (results != null) {
            $scope.itemArray = results.data;
        }
    });
    employerService.getEmployerById($stateParams).then(function (results) {
        if (results != null) {
            $scope.employer = results.data;
            var index = $scope.employer.countries_Id - 1;
            $scope.selected = { value: $scope.itemArray[index] };
        }
    });

    $scope.save = function () {
        $stateParams.id = cookieService.readCookie('userid');
        $scope.employer.Id = cookieService.readCookie('userid');
        $scope.employer.countries_Id = $scope.selected.value.id;
        employerService.updateEmployerById($stateParams, $scope.employer).then(function (response) {
            if (response != null) {
                $state.go("cdashboard.empjobslist");
            }
        });
    }



}])
app.controller('PostJobController', ['$scope', '$stateParams', '$state', 'employerService', 'cookieService', function ($scope, $stateParams, $state, employerService, cookieService) {
    $scope.inlineOptions = { showWeeks: true };
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };
    $scope.save = function () {
        $stateParams.id = cookieService.readCookie('userid');
        employerService.postJob($stateParams, $scope).then(function (response) {
            if (response != null) {
                $state.go("cdashboard.empjobslist");
            }
        });
    }
}])
app.controller('EditJobController', ['$scope', '$stateParams', '$state', 'employerService', 'cookieService', function ($scope, $stateParams, $state, employerService, cookieService) {
    $scope.inlineOptions = { showWeeks: true };
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };
    employerService.getJobById($stateParams).then(function (results) {
        if (results != null) {
            $scope.job = results.data[0];
        }
    });

    $scope.save = function () {
        employerService.editJobById($stateParams, $scope).then(function (response) {
            if (response != null) {
                $state.go("cdashboard.empjobslist");
            }
        });
    }
}])
app.controller('EmpJobListController', ['$scope', '$stateParams', '$state', 'employerService', 'cookieService',  function ($scope, $stateParams, $state, employerService, cookieService) {
    $scope.employers = [];
    $scope.search = '';
    $scope.isclosed = false;
    $scope.currentdate = new Date();

    $stateParams.id = cookieService.readCookie('userid');
    employerService.getEmployerJobs($stateParams).then(function (results) {
        if (results != null) {
            $scope.employers = results.data;
        }
    });

}])
app.controller('AppReceivedController', ['$scope', '$stateParams', '$state', 'employerService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, employerService, cookieService, notificationService) {
    $scope.applications = [];

    $scope.search = '';
    $scope.isclosed = false;
    $scope.currentdate = new Date();

    $scope.refresh = function (){

    $stateParams.empid = cookieService.readCookie('userid');
    employerService.getReceivedApps($stateParams).then(function (results) {
        if (results != null) {
            $scope.applications = results.data;
        }
    });
    };

    $scope.refresh();
    $scope.getapplicants = function (jobid) {
        employerService.getJobApplicants(jobid).then(function (results) {
            if (results != null) {
                $state.go("cdashboard.receivedapps.jobapplicants", {}, { reload: false });
                return $scope.applicants = results.data;
            }
        });
    };
    
    $scope.getapplicantscv = function (applicantid) {

        employerService.getApplicantCV(applicantid).then(function (results) {
            if (results != null) {
                $state.go("cdashboard.receivedapps.jobapplicants.cv", {}, { reload: false });
                return $scope.applicantcv = results.data;
            }
        });
    };

    $scope.shortList = function (appid) {

        employerService.shortListApplicant(appid).then(function (results) {
            if (results != null) {
                notificationService.success('Short Listed', 'bottom_right');
                $state.go("^", {}, { reload: true });
            }
        });
    };

}])
app.controller('ShortlistedAppController', ['$scope', '$stateParams', '$state', 'employerService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, employerService, cookieService, notificationService) {
    $scope.applications = [];

    $scope.search = '';
    $scope.isclosed = false;
    $scope.currentdate = new Date();

    $scope.refresh = function () {

        $stateParams.empid = cookieService.readCookie('userid');
        employerService.getShortlistedApps($stateParams).then(function (results) {
            if (results != null) {
                $scope.applications = results.data;
            }
        });
    };

    $scope.refresh();
    $scope.getapplicants = function (jobid) {
        employerService.getJobShortlistedApplicants(jobid).then(function (results) {
            if (results != null) {
                $state.go("cdashboard.shortlistedapps.jobapplicants", {}, { reload: false });
                return $scope.applicants = results.data;
            }
        });
    };

    $scope.getapplicantscv = function (applicantid) {

        employerService.getApplicantCV(applicantid).then(function (results) {
            if (results != null) {
                $state.go("cdashboard.shortlistedapps.jobapplicants.cv", {}, { reload: false });
                return $scope.applicantcv = results.data;
            }
        });
    };


    // call for interview section

    $scope.inlineOptions = { showWeeks: false };
    $scope.format = 'dd.MM.yyyy';
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };

    $scope.interviewcall = function () {

        $scope.interview.application_Id = $stateParams.id;
        employerService.callForInterview($scope).then(function (results) {
            if (results != null) {
                $state.go("cdashboard.shortlistedapps", {}, { reload: true });
            }
        });
    };
}])
app.controller('InterviewsController', ['$scope', '$stateParams', '$state', 'employerService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, employerService, cookieService, notificationService) {


    $scope.applications = [];
    $scope.search = '';
    $scope.isclosed = false;
    $scope.currentdate = new Date();

    $scope.refresh = function () {

        $stateParams.empid = cookieService.readCookie('userid');
        employerService.getAppswithpendinginterviews($stateParams).then(function (results) {
            if (results != null) {
                $scope.applications = results.data;
            }
        });
    };

    $scope.refresh();
    $scope.getapplicants = function (jobid) {
        employerService.getPendingInterviewApplicants(jobid).then(function (results) {
            if (results != null) {
                $state.go("cdashboard.pendinginterviewsapps.applicants", {}, { reload: false });
                return $scope.applicants = results.data;
            }
        });
    };
    // Popup window code
    $scope.newPopup = function (url) {
        
        $scope.popupWindow = window.open(
            url, 'popUpWindow', 'height=600,width=800,left=10,top=10,resizable=yes,scrollbars=yes,toolbar=no,menubar=no,location=no,directories=no,status=yes'
            )
    };

}])
;