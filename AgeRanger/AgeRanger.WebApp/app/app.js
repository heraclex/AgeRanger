var ageRange = window.ageRange || {};
if (!ageRange["clientApp"]) { ageRange.clientApp = {}; }
(function () {
    'use strict';
    // register external module
    ageRange.clientApp = angular.module('clientApp',
        ['ngRoute', 'ui.bootstrap', 'kendo.directives']);

    // Global Configuration
    ageRange.clientApp.config(function ($routeProvider, $locationProvider, $httpProvider) {

        $routeProvider            
            .when('/home', { templateUrl: Helpers.rootUrl + '/app/views/person.html', controller: 'personController' })
            .otherwise({ redirectTo: '/home' });

        $locationProvider.html5Mode(true);
        $httpProvider.defaults.timeout = 60000;
        $httpProvider.interceptors.push('AppInterceptor');
    });

}());