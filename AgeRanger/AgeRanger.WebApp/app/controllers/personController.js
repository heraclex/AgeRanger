(function () {
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
                    if (callbackValue == "success") {
                        loadPersonDataSource();
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
                        dataModel: function () {
                            // to avoid ref type on the same object
                            return angular.copy(model);
                        }
                    }
                });

                modalInstance.result.then(function (callbackValue) {
                    if (callbackValue == "success") {
                        loadPersonDataSource();
                    }
                }, function () { });
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
