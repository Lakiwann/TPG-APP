(function () {
    "use strict";
    var app = angular.module("common.services");
    app.service("IframeModalService", ['$rootScope', '$q', IframeModalService]);

    function IframeModalService($rootScope, $q) {
        var iframeModalShowFunc, iframePromise;

        function iframeModalHelper(opts) {
            if (iframeModalShowFunc) {
                iframePromise = $q.defer();
                iframeModalShowFunc.call(null, opts);
                return iframePromise.promise;
            }
        }

        return {

            registerShowFunction: function (fn) {
                iframeModalShowFunc = fn;
            },

            iframeCancel: function (data) {
                iframePromise.reject(data);
                $rootScope.$apply();
                // if its IE8 and lower, refresh page.
                if (window.navigator.appName == 'Microsoft Internet Explorer') {
                    var ieV;
                    var ua = window.navigator.userAgent;
                    var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
                    if (re.exec(ua) != null) ieV = parseFloat(RegExp.$1);
                    if (ieV < 9) { // lower than IE9
                        location.reload(true);
                    }
                }
            },

            iframeReturn: function (data) {
                iframePromise.resolve(data);
                $rootScope.$apply();
                // if its IE8 and lower, refresh page.
                if (window.navigator.appName == 'Microsoft Internet Explorer') {
                    var ieV;
                    var ua = window.navigator.userAgent;
                    var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
                    if (re.exec(ua) != null) ieV = parseFloat(RegExp.$1);
                    if (ieV < 9) { // lower than IE9
                        location.reload(true);
                    }
                }
            },

            showGoogleModal: function (bookID, chapterID) {
                console.log('IframeModalService: Showing');
                return iframeModalHelper({
                    //url: 'http://www.bing.com',
                    //url: 'https://palisadesgroup.sharepoint.com/sites/PalisadesSoftwareDevelopment/_layouts/15/guestaccess.aspx?guestaccesstoken=LRb1iQ5jOtGFx2JV%2F%2B4fVSs2k0Emonk8%2BSZHCoCWrSk%3D&docid=2_15cefec90b98e4ffd85178e1ad6123df8&rev=1&e=896fa4951159496a88b8660333135cd8&action=embedview',
                    url: 'https://palisadesgroup.sharepoint.com/sites/PalisadesSoftwareDevelopment/_layouts/15/WopiFrame.aspx?sourcedoc={5cefec90-b98e-4ffd-8517-8e1ad6123df8}&action=embedview&wdAllowInteractivity=False&wdHideGridlines=True&wdHideHeaders=True&wdDownloadButton=True&wdInConfigurator=True',
                    width: 1200,
                    height: 800
                });
            },

        };
    }
}());



