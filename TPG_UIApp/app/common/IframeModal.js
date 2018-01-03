(function () {
    "use strict";
    var app = angular.module("common.services");
    app.directive("iframeModal", ['$rootScope', 'IframeModalService', IFrameModal]);

    function IFrameModal($rootScope, IframeModalService) {
        var link = function (scope, elem, attrs) {

            var showTime, hideTime;

            var backdropElem = jQuery('.backdrop', elem),
                modalElem = jQuery('.modal-container', elem);

            var resize = function () {
                modalElem.css({
                    top: (jQuery(window).height() - modalElem.height()) / 2,
                    left: (jQuery(window).width() - modalElem.width()) / 2
                });
            };

            var show = function (opts) {
                showTime = (new Date()).getTime();
                angular.extend(scope, opts);
                $('iframe', modalElem).attr('src', scope.url);
                modalElem.width(scope.width);
                modalElem.height(scope.height);
                elem.fadeIn();
                jQuery(window).bind('resize', resize).resize();
            };

            var hide = function (ev, data) {
                hideTime = (new Date()).getTime();
                elem.fadeOut();
                $('iframe', modalElem).removeAttr('src');
                jQuery(window).unbind('resize', resize);
            };

            // Registers the directive with the service
            IframeModalService.registerShowFunction(show);

            scope.iframeCancel = function (data) {
                hide();
                IframeModalService.iframeCancel(data);
            };

            scope.iframeReturn = function (data) {
                hide();
                IframeModalService.iframeReturn((hideTime - showTime) / 1000);
            };

        };

        return {
            restrict: 'E',
            link: link,
            scope: true,
            template:
                '<div id="iframe-modal" class="modal">' +
                    '<div class="modal-backdrop"></div>' +
                    '<div class="modal-container">' +
                        '<button ng-click="iframeReturn()">Close</button>' +
                        '<iframe></iframe>' +
                    '</div>' +
                '</div>',
            replace: true
        };
    }
}());

/*
 * These function are for closing the iframe modal from outside of the
 * AngularJS world.  You can use jQuery to call them from the modal.
 */

function iframeCancel(data) {
    jQuery('#iframe-modal').scope().iframeCancel(data);
}

function iframeReturn(data) {
    jQuery('#iframe-modal').scope().iframeReturn(data);
}
