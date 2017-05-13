(function ($) {
    UtilityClass = function () { };

    UtilityClass.prototype = {
        init: UtilityClass,

        rootUrl: '',

        isNull: function (element) {
            return (typeof element === "undefined" || element === null || element.length == 0);
        },

        isNullOrEmpty: function (element) {
            return (Helpers.isNull(element) || element === '');
        },

        isNumber: function (str) {
            var intRegex = /^\d+$/;
            var floatRegex = /^((\d+(\.\d *)?)|((\d*\.)?\d+))$/;

            if (intRegex.test(str) || floatRegex.test(str)) {
                return true;
            }

            return false;
        },

        resolveUrl: function (url) {
            return Helpers.rootUrl + url;
        },
                
        //extend from an object
        extend: function (protobj, skipBaseConstructor) {
            protobj = protobj || {};
            var subClass = null;
            var baseConstructor = this;
            if (typeof (baseConstructor) != "function") {
                baseConstructor = this.init;
            }

            if (protobj.init) {
                subClass = function () {
                    if (!skipBaseConstructor) {
                        baseConstructor.apply(this, arguments);
                    }
                    protobj.init.apply(this, arguments);
                };
            } else {
                subClass = function () {
                    if (!skipBaseConstructor) {
                        baseConstructor.apply(this, arguments);
                    }
                };
                protobj.init = baseConstructor;
            }
            subClass.prototype = subClass.prototype || {};
            $.extend(true, subClass.prototype, this.prototype, protobj);
            subClass.extend = this.extend;
            return subClass;
        },
    };
    Helpers = new UtilityClass();

    //*************Data Core functions********************************************//
    // Summary: this extension will handle all ultilities for Data
    Helpers.DataCore = Helpers.extend({
        clone: function (source) {
            if (!Helpers.isNull(source)) {
                return $.extend(true, [], source);
            }
            return null;
        }
    });
    Helpers.dataHelper = new Helpers.DataCore();
})(jQuery);