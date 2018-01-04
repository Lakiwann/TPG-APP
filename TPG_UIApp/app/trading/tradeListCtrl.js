(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradeListCtrl", ["trades", "tradeResource", TradeListCtrl]);

    function TradeListCtrl(trades, tradeResource) {
        var vm = this;
        vm.trades = [];

        //tradeResource.query(function (data) {
        //    vm.trades = data;
        //});

        vm.trades = trades;
    }


}
());