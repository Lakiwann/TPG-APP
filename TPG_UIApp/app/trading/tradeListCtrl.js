(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradeListCtrl", ["tradeResource", TradeListCtrl]);

    function TradeListCtrl(tradeResource) {
        var vm = this;
        vm.trades = [];

        tradeResource.query(function (data) {
            vm.trades = data;
        });
    }


}
());