(function () {
    'use strict';

    var personService = function ($q, $http, $filter, $timeout) {
        var personApiUrl = Helpers.resolveUrl("api/person");

        var getPerson = function (id) {
            return $http.get(personApiUrl + "/" + id);
        };

        var filter = function (name) {
            if (Helpers.isNullOrEmpty(name))
            {
                return $http.get(personApiUrl + "/filter?name=");
            }
            return $http.get(personApiUrl + "/filter/" + name);
        };

        var addPerson = function (personModel) {            
            return $http.post(personApiUrl, personModel);
        };

        var updatePerson = function (personModel) {
            var targetUrl = personApiUrl + "/update/" + personModel.Id;
            return $http.post(targetUrl, personModel);
        };

        return {
            getPerson: getPerson,
            filter: filter,
            addPerson: addPerson,
            updatePerson: updatePerson,
        };
    };

    ageRange.clientApp.factory("personService", personService);
}());