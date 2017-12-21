(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradeDetailCtrl", ["trade",  TradeDetailCtrl]);

    function TradeDetailCtrl(trade) {

        var vm = this;
        vm.trade = trade;

        vm.toggleState = false;

        vm.ToggleDropDown = function () {
            vm.toggleState = !vm.toggleState;
        }
    }
}
());