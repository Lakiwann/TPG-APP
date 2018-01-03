(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradeDetailCtrl", ["trade", "tradeAssetResource", "$scope", "IframeModalService", TradeDetailCtrl]);

    function TradeDetailCtrl(trade, tradeAssetResource, $scope, IframeModalService) {

        var vm = this;
        vm.trade = trade;
        vm.tradeAssets = [];
        tradeAssetResource.query({ tradefilter: "$filter=TradeID eq " + vm.trade.tradeID }, function (data) {
            vm.tradeAssets = data;
            //alert(vm.tradeAssets.length);
        }).$promise;

        vm.toggleState = false;

        vm.ToggleDropDown = function () {
            vm.toggleState = !vm.toggleState;
        }

        
        //IFrame
        $scope.timeSpent = '';

        $scope.showModal = function () {
            IframeModalService.showGoogleModal().then(function (data) {
                $scope.timeSpent = data + ' seconds spent with modal open.';
            });
        };
    }
}
());