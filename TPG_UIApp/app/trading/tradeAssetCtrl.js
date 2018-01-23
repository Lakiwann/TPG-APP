(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradeAssetCtrl", ["tradeAsset", 'tradeResource', TradeAssetCtrl]);

    function TradeAssetCtrl(tradeAsset, tradeResource) {

        var vm = this;
        if (tradeAsset.palID == "")
        {
            tradeAsset.palID = 'Pending';
        }

        vm.tradeAsset = tradeAsset;


        tradeResource.get({ id: vm.tradeAsset.tradeID }, function (data) {
            vm.tradeSummary = data;
        }).$promise;
        

       
    }
}
());