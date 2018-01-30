(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradeDetailCtrl", ["trade", "tradeAssetResource", "$scope", "IframeModalService", "tradeResource", TradeDetailCtrl]);

    function TradeDetailCtrl(trade, tradeAssetResource, $scope, IframeModalService, tradeResource) {

        var vm = this;

        vm.filterValue = '';
        
        vm.gridOption = {
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            enableFiltering: false,
            //onRegisterApi: function(gridApi) {
            //    vm.gridApi = gridApi;
            //    vm.gridApi.grid.registerRowsProcessor(vm.singleFilter, 200);
            //},
            columnDefs: [
                {
                    field: 'assetID'
                    , name: ' '
                    , cellTemplate: '<a ui-sref="tradeAsset({assetId:{{COL_FIELD}}})"><span class="glyphicon glyphicon-option-horizontal"></span></a>'
                    , enableHiding: false
                    , enableFiltering: false
                    , enableSorting: false
                    , width: 20
                },
                {
                    field: 'sellerAssetId'
                    , enableHiding: false
                    , enableFiltering: true
                },
                {
                    field: 'status'
                    , enableHiding: false
                    , enableFiltering: true
                },
                {
                    field: 'numberOfIssues'
                    , name: 'Issues'
                    , enableHiding: false
                    , enableFiltering: false
                },
                {
                    field: 'totalRepriceAmount'
                    //, name: 'Reprice Amt'
                    , enableHiding: false
                    , enableFiltering: false
                    , cellFilter: 'currency',
                },
                {
                    field: 'originalDebt'
                    //, name: 'Reprice Amt'
                    , enableHiding: false
                    , enableFiltering: false
                    , cellFilter: 'currency',
                },
                {
                    field: 'currentDebt'
                    //, name: 'Reprice Amt'
                    , enableHiding: false
                    , enableFiltering: false
                    , cellFilter: 'currency',
                },
                {
                    field: 'originalPrice'
                    //, name: 'Original Prc'
                    , enableHiding: false
                    , enableFiltering: false
                    , cellFilter: 'currency',
                },
                {
                    field: 'currentPrice'
                    //, name: 'Current Prc'
                    , enableHiding: false
                    , enableFiltering: false
                    , cellFilter: 'currency',
                },
                {
                    field: 'zip'
                    //, name: 'Prop. Zip'
                    , enableHiding: false
                    , enableFiltering: false
                }
            ]
        };

        vm.trade = trade;

        tradeResource.getsummary({ tradeId: vm.trade.tradeID }, function (data) {
            vm.trade.summary = data;
        }).$promise;

        vm.trade.assetSummaries = [];
        tradeResource.getassetsummary({ tradeId: vm.trade.tradeID }, function (data) {
            vm.gridOption.data = [];
            //vm.gridOption.data = data.assetSummaries;
            vm.gridOption.data = data;
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

        vm.filter = function () {
            //vm.gridApi.grid.refresh();
            var filterCriteria = '(TradeID eq ' + vm.trade.tradeID + ')';
            if (vm.filterValue.trim().length > 0) {
                //filterCriteria = filterCriteria + ' and (Zip eq \'' + vm.filterValue + '\'))'
                filterCriteria = vm.filterValue.trim();
            }
            tradeResource.getassetsummary({ tradeId: vm.trade.tradeID, $filter: filterCriteria}, function (data) {
                vm.gridOption.data = data;
            })
        };

        //vm.singleFilter = function (renderableRows) {
        //    var value = vm.filterValue ? vm.filterValue.toLowerCase() : "";
        //    if(value != ""){
        //        var matcher = new RegExp(value);
        //        alert(matcher);
        //        renderableRows.forEach(function (row) {
        //            var match = false;
        //            alert(row);
        //            vm.gridOption.columnDefs.forEach(function (field) {
        //                var val = row.entity[field.name] ? row.entity[field.name].toLowerCase() : "";
        //                alert(val);
        //                if (val.match(matcher)) {
        //                    match = true;
        //                    alert(true);
        //                }
        //            });
        //            if (!match) {
        //                row.visible = false;
        //            }
        //        });
        //    }
        //    return renderableRows;
        //};
    }
}
());