(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradeAssetCtrl", ["tradeAsset", 'tradeResource', TradeAssetCtrl]);

    function TradeAssetCtrl(tradeAsset, tradeResource) {

        var vm = this;
        vm.tradeAsset = tradeAsset;

        tradeResource.query({ tradeId: vm.tradeAsset.tradeID }, function (data) {
            vm.tradeSummary = data[0];
        }).$promise;
        

       
    }
}
());