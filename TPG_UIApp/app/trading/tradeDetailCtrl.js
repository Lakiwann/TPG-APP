(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradeDetailCtrl", ["trade", "tradeAssetResource", "$scope", "IframeModalService", "tradeResource", TradeDetailCtrl]);

    function TradeDetailCtrl(trade, tradeAssetResource, $scope, IframeModalService, tradeResource) {

        var vm = this;

        vm.gridOption = {
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { field: 'assetID', name: 'Asset Id', cellTemplate: '<div class="ui-grid-cell-contents"><a ui-sref="tradeAsset({assetId:{{COL_FIELD}}})">{{COL_FIELD}}</a>' },
                { field: 'inOutStatus', name: 'Status' },
                { field: 'numberOfIssues', name: 'Issues' },
                { field: 'totalRepriceAmount', name: 'Reprice Amt' },
                { field: 'originalPrice', name: 'Original Prc' },
                { field: 'currentPrice', name: 'Current Prc' }
            ]
        };

        vm.trade = trade;

        tradeResource.getsummary({ tradeId: vm.trade.tradeID }, function (data) {
            vm.trade.summary = data;
        }).$promise;

        vm.trade.assetSummaries = [];
        tradeResource.getassetsummary({ tradeId: vm.trade.tradeID }, function (data) {
            vm.gridOption.data = [];
            vm.gridOption.data = data.assetSummaries;
        }).$promise;

        //vm.tradeAssets = [];
        //tradeAssetResource.query({ tradefilter: "$filter=TradeID eq " + vm.trade.tradeID }, function (data) {
        //    vm.tradeAssets = data;
        //}).$promise;

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