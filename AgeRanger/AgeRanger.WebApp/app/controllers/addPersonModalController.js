(function () {
    'use strict';
    var addPersonModalController = function ($scope, $uibModalInstance, personService) {

        var popupViewModel = function () {

            var init = function () {
                // Init Model
                popupViewModel.personModel = {
                    Id: 0,
                    FirstName: null,
                    LastName: null,
                    Age: 0
                }
            };

            var add = function () {
                if (popupViewModel.Form.$valid) {
                    personService.addPerson(popupViewModel.personModel).then(function (response) {
                        if (response.status == httpStatusCode.Created && response.data.Id > 0) {
                            $uibModalInstance.close({ message: "success", data: response.data});
                        }
                    }, function (errorReponse) {
                        toastr.error(errorReponse.data, errorReponse.statusText, { preventDuplicates: true })
                    });
                } else {
                    toastr.warning("First/Last Name is required. Age is greater than 0", "Validation Failed", { positionClass: "toast-bottom-right", preventDuplicates: true })
                }       
            }

            var cancel = function () {
                $uibModalInstance.dismiss("cancel");
            }

            return {
                init: init,
                personModel: null,
                add: add,
                cancel: cancel
            }
        }();

        $scope.popupViewModel = popupViewModel
    };

    ageRange.clientApp.controller("addPersonModalController", addPersonModalController);
}());