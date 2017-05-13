(function () {
    'use strict';

    var appInterceptor = function ($log, $q) {
        var interceptor = {
            request: function (config) {
                // eleminate all template requests
                if (config.url.indexOf("app/views/") === -1) {
                    $log.log("[Request] to '" + config.url + "'");
                    $log.log({ method: config.method, data: config.data });
                }
                return config;
            },
            
            requestError: function (rejection) {
                return $q.reject(rejection);
            },

            response: function (response) {
                // eleminate all template requests
                if (response.config.url.indexOf("app/views/") === -1) {
                    $log.log("[Response] from '" + response.config.url + "'");
                    $log.log({ method: response.config.method, data: response.data, status: response.status, statusText: response.statusText });
                }
                return response;
            },
            
            responseError: function (rejection) {
                if (Helpers.isNullOrEmpty(rejection.data)) {
                    rejection.data = {
                        statusCode: rejection.status,
                        statusText: rejection.statusText
                    };
                } else {
                    rejection.data.statusCode = rejection.status;
                    rejection.data.statusText = rejection.statusText;
                }
                return $q.reject(rejection);
            }
        };

        return interceptor;
    };

    ageRange.clientApp.factory('AppInterceptor', appInterceptor);
}());