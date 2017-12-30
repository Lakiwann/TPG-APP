(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradeDetailCtrl", ["trade",  "tradeAssetResource", TradeDetailCtrl]);

    function TradeDetailCtrl(trade, tradeAssetResource) {

        var vm = this;
        vm.trade = trade;
        vm.tradeAssets = [];
        tradeAssetResource.query({ tradefilter: "$filter=TradeID eq " + vm.trade.tradeID }, function (data) {
            vm.tradeAssets = data;
            alert(vm.tradeAssets.length);
        }).$promise;

        vm.toggleState = false;

        vm.ToggleDropDown = function () {
            vm.toggleState = !vm.toggleState;
        }
    }
}
());