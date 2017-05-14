(function () {
    'use strict';
    var editPersonModalController = function ($scope, $uibModalInstance, personService, modelId) {

        var popupViewModel = function () {

            var init = function () {
                personService.getPerson(modelId).then(function (response) {
                    popupViewModel.personModel = response.data;
                }, function (errorReponse) {
                    toastr.error(errorReponse.data, errorReponse.statusText, { preventDuplicates: true })
                });

            };

            var update = function () {
                if (popupViewModel.Form.$valid && popupViewModel.Form.$dirty) {
                    personService.updatePerson(popupViewModel.personModel).then(function (response) {
                        if (response.status == httpStatusCode.OK) {
                            $uibModalInstance.close({ message: "success", data: response.data });
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
                update: update,
                cancel: cancel
            }
        }();

        $scope.popupViewModel = popupViewModel
    };

    ageRange.clientApp.controller("editPersonModalController", editPersonModalController);
}());