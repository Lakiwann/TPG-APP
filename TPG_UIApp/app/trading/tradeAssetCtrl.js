(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradeAssetCtrl", ["tradeAsset", TradeAssetCtrl]);

    function TradeAssetCtrl(tradeAsset) {

        var vm = this;
        vm.tradeAsset = tradeAsset;
        alert
       
    }
}
());