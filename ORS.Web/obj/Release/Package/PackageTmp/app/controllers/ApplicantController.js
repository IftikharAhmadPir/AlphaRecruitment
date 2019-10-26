'use strict';
angular.module('app').controller('personalinfoController', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', function ($scope, $stateParams, $state, applicantService, cookieService) {

    $scope.refresh = function () {
        $stateParams.id = cookieService.readCookie('userid');
        applicantService.getpersonalinfo($stateParams).then(function (result) {
            $scope.displaypersonalinfo = result.data;
        })
    };
    $scope.refresh();
}])
angular.module('app').controller('addpersonalinfoController', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {
    $scope.refresh = function () {
        $stateParams.id = cookieService.readCookie('userid');
        applicantService.getpersonalinfo($stateParams).then(function (result) {
            $scope.personalinfo = result.data;
        })
    };
    $scope.refresh();
    $scope.save = function () {
        $stateParams.id = cookieService.readCookie('userid');
        applicantService.addpersonalinfo($stateParams, $scope).then(function (response) {
            if (response != null) {
                $state.go("jdashboard.profile");
            }
        });
    }

    $scope.uploadFile = function (input) {

        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {

                //Sets the Old Image to new New Image
                $('#photo-id').attr('src', e.target.result);

                //Create a canvas and draw image on Client Side to get the byte[] equivalent
                var canvas = document.createElement("canvas");
                var imageElement = document.createElement("img");

                imageElement.setAttribute('src', e.target.result);
                canvas.width = imageElement.width;
                canvas.height = imageElement.height;
                var context = canvas.getContext("2d");
                context.drawImage(imageElement, 0, 0);
                var base64Image = canvas.toDataURL("image/jpeg");

                //Removes the Data Type Prefix 
                //And set the view model to the new value
                $scope.personalinfo.picture = base64Image.replace(/data:image\/jpeg;base64,/g, '');
            }

            //Renders Image on Page
            reader.readAsDataURL(input.files[0]);
        }
    };
}])
angular.module('app').controller('educationinfoController', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    $scope.refresh = function () {
        $stateParams.id = cookieService.readCookie('userid');
        applicantService.geteducationinfo($stateParams).then(function (result) {
            $scope.educationinfo = result.data[0].educations;
        })
    };
    $scope.refresh();

    $scope.deleducation = function (id) {
        notificationService.notify(
            {
                title: 'Confirmation',
                text: 'Are you sure you want to delete?',
                hide: false,
                confirm: {
                    confirm: true
                },
                buttons: {
                    closer: false,
                    sticker: false
                },
                history: {
                    history: false
                }
            }).get().on('pnotify.confirm', function () {
                applicantService.deleducationinfo(id).then(function (response) {

                    if (response != null) {
                        notificationService.success('Secessfully Deleted', 'bottom_right');
                        $state.go($state.current, {}, { reload: true });
                    }
                })
            }).on('pnotify.cancel', function () {
                $state.go($state.current, {});
            });
    };


}]);
angular.module('app').controller('editeducation', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {


    $scope.refresh = function () {
        //        $stateParams.id = $scope.educationinfo.id;
        applicantService.geteducationbyid($stateParams).then(function (result) {
            $scope.educationinfo = result.data;

        })
    };

    $scope.refresh();

    $scope.inlineOptions = { showWeeks: false };
    $scope.format = 'dd.MM.yyyy';
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };

    $scope.save = function () {
        applicantService.editeducationbyid($stateParams, $scope).then(function (response) {
            if (response != null) {
                $state.go("jdashboard.profile");
                notificationService.success('Secessfully Update', 'bottom_right');
            }
        });
    }
}])
angular.module('app').controller('addeducationinfoController', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    $scope.inlineOptions = { showWeeks: true };
    $scope.format = 'dd MM yyyy';
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };

    $scope.notcompleted = false;

    if ($scope.notcompleted) {


    } else { }

    $scope.save = function () {
        $stateParams.id = cookieService.readCookie('userid');
        applicantService.addeducationinfo($stateParams, $scope).then(function (response) {
            if (response != null) {
                notificationService.success('Secessfully Added', 'bottom_right');
                $state.go("jdashboard.profile", { reload: true });
            }
        });
    }
}]);

angular.module('app').controller('experienceinfo', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    $scope.refresh = function () {
        $stateParams.id = cookieService.readCookie('userid');
        applicantService.getexperienceinfo($stateParams).then(function (result) {
            $scope.experienceinfo = result.data[0].experiences;
        })
    };
    $scope.refresh();


    $scope.delexperience = function (id) {
        notificationService.notify(
            {
                title: 'Confirmation',
                text: 'Are you sure you want to delete?',
                hide: false,
                confirm: {
                    confirm: true
                },
                buttons: {
                    closer: false,
                    sticker: false
                },
                history: {
                    history: false
                }
            }).get().on('pnotify.confirm', function () {
                applicantService.delexperienceinfo(id).then(function (response) {
                    if (response != null) {
                        notificationService.success('Secessfully Deleted', 'bottom_right');
                        $state.go($state.current, {}, { reload: true });
                    }
                })
            }).on('pnotify.cancel', function () {
                $state.go($state.current, {});
            });
    };


}]);
angular.module('app').controller('editexperience', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {


    $scope.refresh = function () {
        applicantService.getexperiencebyid($stateParams).then(function (result) {
            $scope.experienceinfo = result.data;
        })
    };

    $scope.refresh();

    $scope.inlineOptions = { showWeeks: false };
    $scope.format = 'dd.MM.yyyy';
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };

    $scope.save = function () {
        applicantService.editexperiencebyid($stateParams, $scope).then(function (response) {
            if (response != null) {
                $state.go("jdashboard.profile", { reload: true });
                notificationService.success('Secessfully Added', 'bottom_right');
            }
        });
    }
}])
angular.module('app').controller('addexperience', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    $scope.inlineOptions = { showWeeks: true };
    $scope.format = 'ddMMyyyy';
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };
    $scope.save = function () {

        $stateParams.id = cookieService.readCookie('userid');
        applicantService.addexperienceinfo($stateParams, $scope).then(function (response) {
            if (response != null) {
                $state.go("jdashboard.profile", { reload: true });
                notificationService.success('Secessfully Added', 'bottom_right');
            }
        });
    }
}]);

angular.module('app').controller('publicationinfo', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    $scope.refresh = function () {
        $stateParams.id = cookieService.readCookie('userid');
        applicantService.getpublicationinfo($stateParams).then(function (result) {
            $scope.publicationinfo = result.data[0].publications;
        })
    };
    $scope.refresh();


    $scope.delpublication = function (id) {
        notificationService.notify(
            {
                title: 'Confirmation',
                text: 'Are you sure you want to delete?',
                hide: false,
                confirm: {
                    confirm: true
                },
                buttons: {
                    closer: false,
                    sticker: false
                },
                history: {
                    history: false
                }
            }).get().on('pnotify.confirm', function () {
                applicantService.delpublicationinfo(id).then(function (response) {
                    if (response != null) {
                        notificationService.success('Secessfully Deleted', 'bottom_right');
                        $state.go($state.current, {}, { reload: true });
                    }
                })
            }).on('pnotify.cancel', function () {
                $state.go($state.current, {});
            });
    };


}]);
angular.module('app').controller('editpublication', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {


    $scope.refresh = function () {
        applicantService.getpublicationbyid($stateParams).then(function (result) {
            $scope.publicationinfo = result.data;
        })
    };

    $scope.refresh();

    $scope.inlineOptions = { showWeeks: false };
    $scope.format = 'dd.MM.yyyy';
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };

    $scope.save = function () {
        applicantService.editpublicationbyid($stateParams, $scope).then(function (response) {
            if (response != null) {
                $state.go("jdashboard.profile", { reload: true });
                notificationService.success('Secessfully Added', 'bottom_right');
            }
        });
    }
}])
angular.module('app').controller('addpublication', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    $scope.inlineOptions = { showWeeks: true };
    $scope.format = 'ddMMyyyy';
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };
    $scope.save = function () {

        $stateParams.id = cookieService.readCookie('userid');
        applicantService.addpublicationinfo($stateParams, $scope).then(function (response) {
            if (response != null) {
                $state.go("jdashboard.profile", { reload: true });
                notificationService.success('Secessfully Added', 'bottom_right');
            }
        });
    }
}]);

angular.module('app').controller('traininginfo', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    $scope.refresh = function () {
        $stateParams.id = cookieService.readCookie('userid');
        applicantService.gettraininginfo($stateParams).then(function (result) {
            $scope.traininginfo = result.data[0].trainings;
        })
    };
    $scope.refresh();


    $scope.deltraining = function (id) {
        notificationService.notify(
            {
                title: 'Confirmation',
                text: 'Are you sure you want to delete?',
                hide: false,
                confirm: {
                    confirm: true
                },
                buttons: {
                    closer: false,
                    sticker: false
                },
                history: {
                    history: false
                }
            }).get().on('pnotify.confirm', function () {
                applicantService.deltraininginfo(id).then(function (response) {
                    if (response != null) {
                        notificationService.success('Secessfully Deleted', 'bottom_right');
                        $state.go($state.current, {}, { reload: true });
                    }
                })
            }).on('pnotify.cancel', function () {
                $state.go($state.current, {});
            });
    };


}]);
angular.module('app').controller('edittraining', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {


    $scope.refresh = function () {
        applicantService.gettrainingbyid($stateParams).then(function (result) {
            $scope.traininginfo = result.data;
        })
    };

    $scope.refresh();

    $scope.inlineOptions = { showWeeks: false };
    $scope.format = 'dd.MM.yyyy';
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };

    $scope.save = function () {
        applicantService.edittrainingbyid($stateParams, $scope).then(function (response) {
            if (response != null) {
                $state.go("jdashboard.profile", { reload: true });
                notificationService.success('Secessfully Added', 'bottom_right');
            }
        });
    }
}])
angular.module('app').controller('addtraining', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    $scope.inlineOptions = { showWeeks: true };
    $scope.format = 'ddMMyyyy';
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };
    $scope.save = function () {

        $stateParams.id = cookieService.readCookie('userid');
        applicantService.addtraininginfo($stateParams, $scope).then(function (response) {
            if (response != null) {
                $state.go("jdashboard.profile", { reload: true });
                notificationService.success('Secessfully Added', 'bottom_right');
            }
        });
    }
}]);

angular.module('app').controller('membershipinfo', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    $scope.refresh = function () {
        $stateParams.id = cookieService.readCookie('userid');
        applicantService.getmembershipinfo($stateParams).then(function (result) {
            $scope.membershipinfo = result.data[0].memberships;
        })
    };
    $scope.refresh();


    $scope.delmembership = function (id) {
        notificationService.notify(
            {
                title: 'Confirmation',
                text: 'Are you sure you want to delete?',
                hide: false,
                confirm: {
                    confirm: true
                },
                buttons: {
                    closer: false,
                    sticker: false
                },
                history: {
                    history: false
                }
            }).get().on('pnotify.confirm', function () {
                applicantService.delmembershipinfo(id).then(function (response) {
                    if (response != null) {
                        notificationService.success('Secessfully Deleted', 'bottom_right');
                        $state.go($state.current, {}, { reload: true });
                    }
                })
            }).on('pnotify.cancel', function () {
                $state.go($state.current, {});
            });
    };


}]);
angular.module('app').controller('editmembership', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {


    $scope.refresh = function () {
        applicantService.getmembershipbyid($stateParams).then(function (result) {
            $scope.membershipinfo = result.data;
        })
    };

    $scope.refresh();

    $scope.inlineOptions = { showWeeks: false };
    $scope.format = 'dd.MM.yyyy';
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };

    $scope.save = function () {
        applicantService.editmembershipbyid($stateParams, $scope).then(function (response) {
            if (response != null) {
                $state.go("jdashboard.profile", { reload: true });
                notificationService.success('Secessfully Added', 'bottom_right');
            }
        });
    }
}])
angular.module('app').controller('addmembership', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    $scope.inlineOptions = { showWeeks: true };
    $scope.format = 'ddMMyyyy';
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };
    $scope.save = function () {

        $stateParams.id = cookieService.readCookie('userid');
        applicantService.addmembershipinfo($stateParams, $scope).then(function (response) {
            if (response != null) {
                $state.go("jdashboard.profile", { reload: true });
                notificationService.success('Secessfully Added', 'bottom_right');
            }
        });
    }
}]);

angular.module('app').controller('languageinfo', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    $scope.refresh = function () {
        $stateParams.id = cookieService.readCookie('userid');
        applicantService.getlanguageinfo($stateParams).then(function (result) {
            $scope.languageinfo = result.data[0].languages;
        })
    };
    $scope.refresh();


    $scope.dellanguage = function (id) {
        notificationService.notify(
            {
                title: 'Confirmation',
                text: 'Are you sure you want to delete?',
                hide: false,
                confirm: {
                    confirm: true
                },
                buttons: {
                    closer: false,
                    sticker: false
                },
                history: {
                    history: false
                }
            }).get().on('pnotify.confirm', function () {
                applicantService.dellanguageinfo(id).then(function (response) {
                    if (response != null) {
                        notificationService.success('Secessfully Deleted', 'bottom_right');
                        $state.go($state.current, {}, { reload: true });
                    }
                })
            }).on('pnotify.cancel', function () {
                $state.go($state.current, {});
            });
    };


}]);
angular.module('app').controller('editlanguage', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {


    $scope.refresh = function () {
        applicantService.getlanguagebyid($stateParams).then(function (response) {
            if (response != null) {
                $scope.languageinfo = response.data;
            }
        })
    };

    $scope.refresh();

    $scope.inlineOptions = { showWeeks: false };
    $scope.format = 'dd.MM.yyyy';
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };

    $scope.save = function () {
        applicantService.editlanguagebyid($stateParams, $scope).then(function (response) {
            if (response != null) {
                $state.go("jdashboard.profile", { reload: true });
                notificationService.success('Secessfully Added', 'bottom_right');
            }
        });
    }
}])
angular.module('app').controller('addlanguage', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    $scope.inlineOptions = { showWeeks: true };
    $scope.format = 'ddMMyyyy';
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };
    $scope.save = function () {

        $stateParams.id = cookieService.readCookie('userid');
        applicantService.addlanguageinfo($stateParams, $scope).then(function (response) {
            if (response != null) {
                $state.go("jdashboard.profile", { reload: true });
                notificationService.success('Secessfully Added', 'bottom_right');
            }
        });
    }
}]);

angular.module('app').controller('referenceinfo', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    $scope.refresh = function () {
        $stateParams.id = cookieService.readCookie('userid');
        applicantService.getreferenceinfo($stateParams).then(function (result) {
            $scope.referenceinfo = result.data[0].references;
        })
    };
    $scope.refresh();


    $scope.delreference = function (id) {
        notificationService.notify(
            {
                title: 'Confirmation',
                text: 'Are you sure you want to delete?',
                hide: false,
                confirm: {
                    confirm: true
                },
                buttons: {
                    closer: false,
                    sticker: false
                },
                history: {
                    history: false
                }
            }).get().on('pnotify.confirm', function () {
                applicantService.delreferenceinfo(id).then(function (response) {
                    if (response != null) {
                        notificationService.success('Secessfully Deleted', 'bottom_right');
                        $state.go($state.current, {}, { reload: true });
                    }
                })
            }).on('pnotify.cancel', function () {
                $state.go($state.current, {});
            });
    };


}]);
angular.module('app').controller('editreference', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {


    $scope.refresh = function () {
        applicantService.getreferencebyid($stateParams).then(function (result) {
            $scope.referenceinfo = result.data;
        })
    };

    $scope.refresh();

    $scope.inlineOptions = { showWeeks: false };
    $scope.format = 'dd.MM.yyyy';
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };

    $scope.save = function () {
        applicantService.editreferencebyid($stateParams, $scope).then(function (response) {
            if (response != null) {
                $state.go("jdashboard.profile", { reload: true });
                notificationService.success('Secessfully Added', 'bottom_right');
            }
        });
    }
}])
angular.module('app').controller('addreference', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    $scope.inlineOptions = { showWeeks: true };
    $scope.format = 'ddMMyyyy';
    $scope.dateOptions = { formatYear: 'yyyy' };
    $scope.popupcd = { opened: false };
    $scope.opencd = function () { $scope.popupcd.opened = true; };
    $scope.opened = function () { $scope.popuped.opened = true; };
    $scope.popuped = { opened: false };
    $scope.save = function () {

        $stateParams.id = cookieService.readCookie('userid');
        applicantService.addreferenceinfo($stateParams, $scope).then(function (response) {
            if (response != null) {
                $state.go("jdashboard.profile", { reload: true });
                notificationService.success('Secessfully Added', 'bottom_right');
            }
        });
    }
}]);

angular.module('app').controller('jobinfo', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    $scope.refresh = function () {
        $stateParams.userid = cookieService.readCookie('userid');
        applicantService.getjobinfo($stateParams).then(function (result) {
            $scope.jobinfo = result.data;
        })
    };
    $scope.refresh();

    $scope.jobdetails = function ($stateParams) {
        applicantService.getjobbyid().then(function (result) {
            $scope.jobinfo = result.data;
        })
    };

    $scope.apply = function (id) {
        $stateParams.userid = cookieService.readCookie('userid');
        $stateParams.id = id;
        applicantService.applyforjob($stateParams).then(function (response) {

            if (response != null) {
                notificationService.success('Application Submitted Successfullly', 'bottom_right');
                $state.go($state.current, {}, { reload: true });

            }

        })
    };



}]);

angular.module('app').controller('appinfo', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    $scope.refresh = function () {
        $stateParams.userid = cookieService.readCookie('userid');
        applicantService.getappinfo($stateParams).then(function (result) {
            $scope.appinfo = result.data;
        })
    };
    $scope.refresh();

    $scope.appdetails = function ($stateParams) {
        applicantService.getjobbyid().then(function (result) {
            $scope.appinfo = result.data;
        })
    };


}]);
angular.module('app').controller('userinterviews', ['$scope', '$stateParams', '$state', 'applicantService', 'cookieService', 'notificationService', function ($scope, $stateParams, $state, applicantService, cookieService, notificationService) {

    // Popup window code
    $scope.newPopup = function (url) {
        $scope.popupWindow = window.open(
            url, 'popUpWindow', 'height=600,width=800,left=10,top=10,resizable=yes,scrollbars=yes,toolbar=no,menubar=no,location=no,directories=no,status=yes'
            )
    };
    $scope.currentdate = new Date();
    $scope.refresh = function () {
        $stateParams.userid = cookieService.readCookie('userid');
        applicantService.getinterviewsinfo($stateParams).then(function (result) {
            $scope.appinfo = result.data;
        })
    };
    $scope.refresh();

    $scope.appdetails = function ($stateParams) {
        applicantService.getjobbyid().then(function (result) {
            $scope.appinfo = result.data;
        })
    };


}]);
