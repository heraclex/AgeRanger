﻿(function () {
    'use strict';
    var personController = function ($scope, $location, $uibModal, personService) {

        var viewModel = function () {

            var init = function () {
                loadPersonDataSource();
            };

            var addNewPerson = function () {
                var modalInstance = $uibModal.open({
                    animation: true,
                    size: '', //'',sm,lg
                    templateUrl: Helpers.rootUrl + 'app/views/addPersonPopup.html',
                    controller: "addPersonModalController",
                    backdrop: 'static',
                    keyboard: false
                });

                modalInstance.result.then(function (callbackValue) {
                    if (callbackValue.message == "success") {
                        loadPersonDataSource();
                        toastr.success(callbackValue.data.FirstName + ", " + callbackValue.data.LastName + " has been added with #" + callbackValue.data.Id,
                            "Success", { positionClass: "toast-bottom-right", preventDuplicates: true });
                    }
                }, function () {});
            }; 

            var editPerson = function (model) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    size: '', //'',sm,lg
                    templateUrl: Helpers.rootUrl + 'app/views/editPersonPopup.html',
                    controller: "editPersonModalController",
                    backdrop: 'static',
                    keyboard: false,
                    resolve: {
                        // I don't want to use model object at client to avoiding some changes happend on server
                        // always get latest data from server
                        modelId: function () { return model.Id; }
                    }
                });

                modalInstance.result.then(function (callbackValue) {
                    if (callbackValue.message == "success") {
                        loadPersonDataSource();
                        toastr.success("(#" + callbackValue.data.Id + ") " + callbackValue.data.FirstName + ", " + callbackValue.data.LastName + " has been updated",
                            "Success", { positionClass: "toast-bottom-right", preventDuplicates: true });
                    }
                }, function () { });
            }; 

            var deletePerson = function (id) {
                personService.deletePerson(id).then(function (response) {
                    if (response.status == httpStatusCode.OK && response.data > 0) {
                        toastr.success("Person with ID #" + response.data + " is permanent deleted",
                            "Success", { positionClass: "toast-bottom-right", preventDuplicates: true });
                        loadPersonDataSource();
                    }
                }, function (errorReponse) {
                    toastr.error(errorReponse.data, errorReponse.statusText, { preventDuplicates: true })
                });
            }; 

            return {
                search: "",
                init: init,
                dataReady: false,
                personDataSoure: [],
                reloadPersonDataSource: function (filter) {
                    loadPersonDataSource(filter);
                },
                addNewPerson: addNewPerson,
                editPerson: editPerson,
                deletePerson: deletePerson
            };

            function loadPersonDataSource(filter) {
                viewModel.dataReady = false;
                personService.filter(filter).then(function (response) {
                    viewModel.personDataSoure = response.data;
                    viewModel.dataReady = true;
                }, function (error) {
                    if (error.data.statusCode === httpStatusCode.NotFound)
                    {
                        // reset data source
                        viewModel.personDataSoure = []
                        toastr.info("No result found", "info", { positionClass: "toast-bottom-right",  preventDuplicates:true});
                    }
                });
            }
        }();

        $scope.viewModel = viewModel;
        $scope.$watch(angular.bind(viewModel, function () {
            return viewModel.search;
        }), function (value) {
            viewModel.reloadPersonDataSource(value);
        });
    };

    ageRange.clientApp.controller("personController", personController);
}());
