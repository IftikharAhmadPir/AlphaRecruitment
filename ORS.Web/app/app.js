/// <reference path="../scripts/angularjs/angular.min.js" />
/// <reference path="../scripts/angularjs/ocLazyLoad.min.js" />
/// <reference path="../scripts/angularjs/ui-bootstrap-tpls.min.js" />
var app = angular.module("app", [
    "ui.router",
    "ui.bootstrap",
    "oc.lazyLoad",
    "ngSanitize",
    "LocalStorageModule",
    "ors.loadingBar",
    "ui.select",
    "ui.mask",
    "ui.pnotify"
]);

app.config(['$ocLazyLoadProvider', function ($ocLazyLoadProvider) {
    $ocLazyLoadProvider.config({
        // global configs go here
    });
}]);

app.config(['$controllerProvider', function ($controllerProvider) {
    // this option might be handy for migrating old apps, but please don't use it
    // in new ones!

    $controllerProvider.allowGlobals();
}]);

app.config(function (cfpLoadingBarProvider) {
    cfpLoadingBarProvider.includeSpinner = true;
})

app.config(['notificationServiceProvider', function (notificationServiceProvider) {
    notificationServiceProvider
        .setDefaults({
            delay: 2000,
            buttons: {
                closer: false,
                closer_hover: false,
                sticker: false,
                sticker_hover: false
            },
            type: 'error'
        })

        // Configure a stack named 'bottom_right' that append a call 'stack-bottomright'
        .setStack('bottom_right', 'stack-bottomright', {
            dir1: 'up',
            dir2: 'left',
            firstpos1: 300,
            firstpos2: 500
        })

        // Configure a stack named 'top_left' that append a call 'stack-topleft'
        .setStack('top_left', 'stack-topleft', {
            dir1: 'down',
            dir2: 'right',
            push: 'top'
        });

}])
/* Setup global settings */
app.factory('settings', ['$rootScope', function ($rootScope) {
    // supported languages
    var settings = {
        //layout: {
        //    pageSidebarClosed: false, // sidebar menu state
        //    pageContentWhite: true, // set page content layout
        //    pageBodySolid: false, // solid body color state
        //    pageAutoScrollOnLoad: 1000 // auto scroll to top on page load
        //},
        //assetsPath: '../assets',
        //globalPath: '../assets/global',
        //layoutPath: '../assets/layouts/layout4',
    };

    $rootScope.settings = settings;
    return settings;
}]);

/* Setup App Main Controller */
app.controller('AppController', ['$scope', '$rootScope', function ($scope, $rootScope) {
    $scope.$on('$viewContentLoaded', function () {
        App.initComponents(); // init core components
        //Layout.init(); //  Init entire layout(header, footer, sidebar, etc) on page load if the partials included in server side instead of loading with ng-include directive 
    });
}]);

/* Setup Layout Part - Header */
app.controller('HeaderController', ['$scope', function ($scope) {
    $scope.$on('$includeContentLoaded', function () {
        Layout.initHeader(); // init header
    });
}]);

/* Setup Layout Part - Sidebar */
app.controller('SidebarController', ['$scope', function ($scope) {
    $scope.$on('$includeContentLoaded', function () {
        Layout.initSidebar(); // init sidebar
    });
}]);

/* Setup Layout Part - Sidebar */
app.controller('PageHeadController', ['$scope', function ($scope) {
    $scope.$on('$includeContentLoaded', function () {
        Demo.init(); // init theme panel
    });
}]);

/* Setup Layout Part - Footer */
app.controller('FooterController', ['$scope', function ($scope) {
    $scope.$on('$includeContentLoaded', function () {
        //Layout.initFooter(); // init footer
    });
}]);

/* Site Routing */
app.config(['$stateProvider', '$locationProvider', '$urlRouterProvider', '$ocLazyLoadProvider', function ($stateProvider, $locationProvider, $urlRouterProvider, $ocLazyLoadProvider) {
    // Redirect any unmatched url
    $urlRouterProvider.otherwise("/");
    $stateProvider

        .state('home', {
            url: "/",
            views: {
                "MainView": {
                    controller: 'homeController',
                    templateUrl: '/app/views/home.html'
                }
            },
            resolve: {
                loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                    // you can lazy load files for an existing module
                    return $ocLazyLoad.load(
                        [
                            'content/css/search.min.css',
                            'app/controllers/homeController.js'
                        ]);
                }]
            }
        })

        .state('login', {
            url: "/login",
            views: {
                "MainView": {
                    controller: 'loginController', // This view will use AppCtrl loaded below in the resolve
                    templateUrl: '/app/views/login.html'
                }
            },
            resolve: {
                loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load('app/controllers/loginController.js');
                }]
            }
        })

        .state('signup', {
            url: "/signup",
            views: {
                "MainView": {
                    controller: 'signupController', // This view will use AppCtrl loaded below in the resolve
                    templateUrl: '/app/views/signup.html'
                }
            },
            resolve: {
                loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load('app/controllers/signupController.js');
                }]
            }
        })
            .state('resetpassword', {
                url: "/forgetpassword",
                views: {
                    "MainView": {
                        controller: 'ResetController', // This view will use AppCtrl loaded below in the resolve
                        templateUrl: '/app/views/forgetpassword.html'
                    }
                },
                resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load('app/controllers/resetController.js');
                    }]
                }
            })

    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });

}]);
//company routing
app.config(['$stateProvider', '$locationProvider', '$urlRouterProvider', function ($stateProvider, $locationProvider, $urlRouterProvider) {

    $stateProvider
        .state('cdashboard', {
            url: "",
            views: {
                "MainView": {
                    controller: 'homeController', // This view will use AppCtrl loaded below in the resolve
                    templateUrl: '/app/views/employer/cdashboard.html'
                }
            },
            resolve: {
                loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load('app/controllers/homeController.js');
                }]
            }
        })
                                .state('cdashboard.home', {
                                    url: "/Dashboard",
                                    templateUrl: '/app/views/employer/home.html',
                                    resolve: {
                                        loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                            return $ocLazyLoad.load('app/controllers/homeController.js');
                                        }]
                                    }
                                })

            .state('cdashboard.empdata', {
                url: "/employer",
                templateUrl: "/app/views/employer/empdata.html",
                controller: "EmployerController",
            })
            .state('cdashboard.postjob', {
                url: "/newjob",
                templateUrl: "/app/views/employer/postjob.html",
                controller: "PostJobController",
            })
            .state('cdashboard.editjob', {
                url: "/editjob/:id",
                templateUrl: "/app/views/employer/postjob.html",
                controller: "EditJobController",
            })

            .state('cdashboard.empjobslist', {
                url: "/empjobslist",
                templateUrl: "/app/views/employer/empjoblist.html",
                controller: "EmpJobListController",
            })

                .state('cdashboard.receivedapps', {
                    url: "/receivedapps",
                    views: {
                        "": {
                            controller: 'AppReceivedController',
                            templateUrl: '/app/views/employer/applicationsinfo.html'
                        }
                    },
                    resolve: {
                        loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                            return $ocLazyLoad.load('app/controllers/EmployerController.js');
                        }]
                    }
                })
                    .state('cdashboard.receivedapps.jobapplicants', {
                        url: "/jobapplicants",
                        views: {
                            "ApplicantsView": {
                                controller: 'AppReceivedController',
                                templateUrl: '/app/views/employer/jobapplicants.html'
                            }
                        }
                    })
                            .state('cdashboard.receivedapps.jobapplicants.cv', {
                                url: "/cv",
                                views: {
                                    "viewcv": {
                                        controller: 'AppReceivedController',
                                        templateUrl: '/app/views/employer/cv/applicantcv.html',
                                    }
                                }
                            })


                .state('cdashboard.shortlistedapps', {
                    url: "/shortlistedapps",
                    views: {
                        "": {
                            controller: 'ShortlistedAppController',
                            templateUrl: '/app/views/employer/shortlisted/shortlistedappsinfo.html'
                        }
                    },
                    resolve: {
                        loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                            return $ocLazyLoad.load('app/controllers/EmployerController.js');
                        }]
                    }
                })
                    .state('cdashboard.shortlistedapps.jobapplicants', {
                        url: "/shortlistedapplicants",
                        views: {
                            "ApplicantsView": {
                                controller: 'ShortlistedAppController',
                                templateUrl: '/app/views/employer/jobapplicants.html'
                            }
                        }
                    })
                            .state('cdashboard.shortlistedapps.jobapplicants.cv', {
                                url: "/scv",
                                views: {
                                    "viewcv": {
                                        controller: 'ShortlistedAppController',
                                        templateUrl: '/app/views/employer/cv/applicantcv.html',
                                    }
                                }
                            })


                                .state('cdashboard.shortlistedapps.jobapplicants.interview', {
                                    url: "/interview/:id",
                                    views: {
                                        "viewcv": {
                                            controller: 'ShortlistedAppController',
                                            templateUrl: '/app/views/employer/interview/interviewcall.html',
                                        }
                                    }
                                })



                            .state('cdashboard.pendinginterviewsapps', {
                                url: "/interviews",
                                views: {
                                    "": {
                                        controller: 'InterviewsController',
                                        templateUrl: '/app/views/employer/interview/pendinginterviewsappsinfo.html'
                                    }
                                },
                                resolve: {
                                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                        return $ocLazyLoad.load('app/controllers/EmployerController.js');
                                    }]
                                }
                            })
                                        .state('cdashboard.pendinginterviewsapps.applicants', {
                                            url: "/applicants",
                                            views: {
                                                "ApplicantsView": {
                                                    controller: 'InterviewsController',
                                                    templateUrl: '/app/views/employer/interview/applicants.html'
                                                }
                                            }
                                        })

                                               .state('cdashboard.pendinginterviewsapps.applicants.result', {
                                                   url: "/result",
                                                   views: {
                                                       "viewcv": {
                                                           controller: 'InterviewsController',
                                                           templateUrl: '/app/views/employer/interview/interviewresult.html'
                                                       }
                                                   }
                                               })





                    .state('cdashboard.changepassword', {
                        url: "/resetpassword",
                        views: {
                            "": {
                                controller: 'changePasswordController',
                                templateUrl: '/app/views/changepassword.html'
                            }
                        },
                        resolve: {
                            loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                return $ocLazyLoad.load('app/controllers/changePassword.js');
                            }]
                        }
                    })

    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
}]);

//admin routing
app.config(['$stateProvider', '$locationProvider', '$urlRouterProvider', function ($stateProvider, $locationProvider, $urlRouterProvider) {

    $stateProvider


.state('adashboard', {
    url: "",
    views: {
        "MainView": {
            controller: 'homeController',
            templateUrl: '/app/views/admin/adashboard.html'
        }
    },
    resolve: {
        loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
            return $ocLazyLoad.load('app/controllers/homeController.js');
        }]
    }
})
        .state('adashboard.home', {
            url: "/DASHBOARD",
            templateUrl: "/app/views/admin/home.html",
            controller: "homeController",
        })


    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
}]);

//jobseeker routing
app.config(['$stateProvider', '$locationProvider', '$urlRouterProvider', function ($stateProvider, $locationProvider, $urlRouterProvider) {

    $stateProvider
                .state('jdashboard', {
                    url: "",
                    views: {
                        "MainView": {
                            controller: 'homeController',
                            templateUrl: '/app/views/jobseeker/jdashboard.html'
                        }
                    },
                    resolve: {
                        loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                            return $ocLazyLoad.load('app/controllers/homeController.js');
                        }]
                    }
                })
                        .state('jdashboard.home', {
                            url: "/dashboard",
                            templateUrl: '/app/views/jobseeker/home.html',
                            resolve: {
                                loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                    return $ocLazyLoad.load('app/controllers/homeController.js');
                                }]
                            }
                        })

                .state('jdashboard.profile', {
                    url: "/UserProfile",
                    templateUrl: '/app/views/jobseeker/userprofile.html',
                    resolve: {
                        loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                            return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                        }]
                    }
                })

        .state('jdashboard.changepassword', {
            url: "/changepassword",
            views: {
                "": {
                    controller: 'changePasswordController',
                    templateUrl: '/app/views/changepassword.html'
                }
            },
            resolve: {
                loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load('app/controllers/changePassword.js');
                }]
            }
        })


            .state('jdashboard.personalinfo', {
                url: "/personal",
                templateUrl: "/app/views/jobseeker/personalinfo.html",
                controller: "personalinfoController",
                resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                    }]
                }

            })
                .state('jdashboard.addpersonal', {
                    url: "/addpersonal",
                    templateUrl: "/app/views/jobseeker/addpersonalinfo.html",
                    controller: "addpersonalinfoController",
                    resolve: {
                        loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                            return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                        }]
                    }

                })

            .state('jdashboard.educationinfo', {
                url: "/education",
                templateUrl: "/app/views/jobseeker/educationinfo.html",
                controller: "educationinfoController",
                resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                    }]
                }

            })

                    .state('jdashboard.addeducation', {
                        url: "/addeducation",
                        templateUrl: "/app/views/jobseeker/addeducationinfo.html",
                        controller: "addeducationinfoController",
                        resolve: {
                            loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                            }]
                        }

                    })
                            .state('jdashboard.editeducation', {
                                url: "/editeducation/:id",
                                templateUrl: "/app/views/jobseeker/addeducationinfo.html",
                                controller: "editeducation",
                            })


            .state('jdashboard.experienceinfo', {
                url: "/experience",
                templateUrl: "/app/views/jobseeker/experienceinfo.html",
                controller: "experienceinfo",
                resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                    }]
                }

            })
                    .state('jdashboard.addexperience', {
                        url: "/addexperience",
                        templateUrl: "/app/views/jobseeker/addexperienceinfo.html",
                        controller: "addexperience",
                        resolve: {
                            loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                            }]
                        }

                    })
                                        .state('jdashboard.editexperience', {
                                            url: "/editexperience/:id",
                                            templateUrl: "/app/views/jobseeker/addexperienceinfo.html",
                                            controller: "editexperience",
                                            resolve: {
                                                loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                                    return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                                                }]
                                            }

                                        })
            .state('jdashboard.publicationinfo', {
                url: "/publication",
                templateUrl: "/app/views/jobseeker/publicationinfo.html",
                controller: "publicationinfo",
                resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                    }]
                }

            })

                    .state('jdashboard.addpublication', {
                        url: "/addpublication",
                        templateUrl: "/app/views/jobseeker/addpublicationinfo.html",
                        controller: "addpublication",
                        resolve: {
                            loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                            }]
                        }

                    })
                                        .state('jdashboard.editpublication', {
                                            url: "/editpublication/:id",
                                            templateUrl: "/app/views/jobseeker/addpublicationinfo.html",
                                            controller: "editpublication",
                                            resolve: {
                                                loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                                    return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                                                }]
                                            }

                                        })




            .state('jdashboard.traininginfo', {
                url: "/training",
                templateUrl: "/app/views/jobseeker/traininginfo.html",
                controller: "traininginfo",
                resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                    }]
                }

            })
                            .state('jdashboard.addtraining', {
                                url: "/addtraining",
                                templateUrl: "/app/views/jobseeker/addtraininginfo.html",
                                controller: "addtraining",
                                resolve: {
                                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                        return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                                    }]
                                }

                            })
                                        .state('jdashboard.edittraining', {
                                            url: "/edittraining/:id",
                                            templateUrl: "/app/views/jobseeker/addtraininginfo.html",
                                            controller: "edittraining",
                                            resolve: {
                                                loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                                    return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                                                }]
                                            }

                                        })
            .state('jdashboard.membershipinfo', {
                url: "/memberships",
                templateUrl: "/app/views/jobseeker/membershipinfo.html",
                controller: "membershipinfo",
                resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                    }]
                }

            })
                            .state('jdashboard.addmembership', {
                                url: "/addmembership",
                                templateUrl: "/app/views/jobseeker/addmembershipinfo.html",
                                controller: "addmembership",
                                resolve: {
                                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                        return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                                    }]
                                }

                            })
                                        .state('jdashboard.editmembership', {
                                            url: "/editmembership/:id",
                                            templateUrl: "/app/views/jobseeker/addmembershipinfo.html",
                                            controller: "editmembership",
                                            resolve: {
                                                loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                                    return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                                                }]
                                            }

                                        })

            .state('jdashboard.languageinfo', {
                url: "/languages",
                templateUrl: "/app/views/jobseeker/languageinfo.html",
                controller: "languageinfo",
                resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                    }]
                }

            })
                            .state('jdashboard.addlanguage', {
                                url: "/addlanguage",
                                templateUrl: "/app/views/jobseeker/addlanguageinfo.html",
                                controller: "addlanguage",
                                resolve: {
                                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                        return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                                    }]
                                }

                            })
                                        .state('jdashboard.editlanguage', {
                                            url: "/editlanguage/:id",
                                            templateUrl: "/app/views/jobseeker/addlanguageinfo.html",
                                            controller: "editlanguage",
                                            resolve: {
                                                loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                                    return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                                                }]
                                            }

                                        })
            .state('jdashboard.referenceinfo', {
                url: "/references",
                templateUrl: "/app/views/jobseeker/referenceinfo.html",
                controller: "referenceinfo",
                resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                    }]
                }

            })
                            .state('jdashboard.addreference', {
                                url: "/addreference",
                                templateUrl: "/app/views/jobseeker/addreferenceinfo.html",
                                controller: "addreference",
                                resolve: {
                                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                        return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                                    }]
                                }

                            })
                                        .state('jdashboard.editreference', {
                                            url: "/editreference/:id",
                                            templateUrl: "/app/views/jobseeker/addreferenceinfo.html",
                                            controller: "editreference",
                                            resolve: {
                                                loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                                                    return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                                                }]
                                            }

                                        })


            .state('jdashboard.jobsinfo', {
                url: "/availablejobs",
                views: {
                    "": {
                        controller: 'jobinfo', // This view will use AppCtrl loaded below in the resolve
                        templateUrl: '/app/views/jobseeker/jobsinfo.html'
                    }
                },
                resolve: {
                    loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                    }]
                }
            })
                .state('jdashboard.appsinfo', {
                    url: "/userapps",
                    views: {
                        "": {
                            controller: 'appinfo',
                            templateUrl: '/app/views/jobseeker/appsinfo.html'
                        }
                    },
                    resolve: {
                        loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                            return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                        }]
                    }
                })

                .state('jdashboard.interviews', {
                    url: "/userinterviews",
                    views: {
                        "": {
                            controller: 'userinterviews',
                            templateUrl: '/app/views/jobseeker/interview/interviewinfo.html'
                        }
                    },
                    resolve: {
                        loadMyCtrl: ['$ocLazyLoad', function ($ocLazyLoad) {
                            return $ocLazyLoad.load('app/controllers/ApplicantController.js');
                        }]
                    }
                })

    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
}]);

/* Init global settings and run the app */
app.run(['$templateCache', '$rootScope', '$state', '$stateParams', function ($templateCache, $rootScope, $state, $stateParams) {
    // <ui-view> contains a pre-rendered template for the current view
    // caching it will prevent a round-trip to a server at the first page load
    var view = angular.element('#ui-view');
    $templateCache.put(view.data('tmpl-url'), view.html());

    // Allows to retrieve UI Router state information from inside templates
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
    //$rootScope.$settings = settings;

    $rootScope.$on('$stateChangeSuccess', function (event, toState) {
        // Sets the layout name, which can be used to display different layouts (header, footer etc.)
        // based on which page the user is located
        //$rootScope.layout = toState.layout;
    });

}]);


var serviceBase = 'http://localhost:26264/';
//var serviceBase = 'http://alpharecruiters.org:7521/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'ngAuthApp'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);
