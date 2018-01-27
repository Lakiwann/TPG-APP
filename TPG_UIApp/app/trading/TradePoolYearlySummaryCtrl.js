(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradePoolYearlySummaryCtrl", ['tradeYearlySummaries', 'uiGridConstants', TradePoolYearlySummaryCtrl]);

    function TradePoolYearlySummaryCtrl(tradeYearlySummaries, uiGridConstants) {
        var vm = this;

        vm.yearlySummaryGridOption = {
            //showGridFooter: true,
            showColumnFooter: true,
            columnDefs: [
                {
                    field: 'year',
                    //name: 'Year',
                    cellTemplate: '<div class="ui-grid-cell-contents"><a ui-sref="trading({year:{{COL_FIELD}}})">{{COL_FIELD}}</a></div>',
                    aggregationType: '',
                    enableHiding: false,
                    enableFiltering: true,
                    footerCellTemplate: '<div class="ui-grid-cell-contents" ><a ui-sref="trading()">All</a> (Total/Avg) </div>'
                },
                {
                    field: 'trades',
                    //name: 'Trades',
                    enableColumnMenu: false,
                    aggregationType: uiGridConstants.aggregationTypes.sum,
                    footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue()}}</div>'
                },
                {
                    field: 'counterParties',
                    //name: 'Counter Parties',
                    enableColumnMenu: false,
                    aggregationType: uiGridConstants.aggregationTypes.sum,
                    footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue()}}</div>'
                },
                //{
                //    field: 'rpLoans',
                //    //name: 'RPLs',
                //    enableColumnMenu: false,
                //    aggregationType: uiGridConstants.aggregationTypes.sum,
                //    footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue()}}</div>'
                //},
                //{
                //    field: 'npLoans',
                //    //name: 'NPLs',
                //    enableColumnMenu: false,
                //    aggregationType: uiGridConstants.aggregationTypes.sum,
                //    footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue()}}</div>'
                //},
                {
                    field: 'mixedLoans',
                    //name: 'Mixed',
                    enableColumnMenu: false,
                    aggregationType: uiGridConstants.aggregationTypes.sum,
                    footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue()}}</div>'
                },
                {
                    field: 'tradeAmount',
                    //name: 'Total Trade$',
                    enableColumnMenu: false,
                    cellFilter: 'currency',
                    aggregationType: uiGridConstants.aggregationTypes.sum,
                    footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue() | currency}}</div>'
                },
                {
                    field: 'bidAmount',
                    //name: 'Total Trade$',
                    enableColumnMenu: false,
                    cellFilter: 'currency',
                    aggregationType: uiGridConstants.aggregationTypes.sum,
                    footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue() | currency}}</div>'
                },
                //{
                //    field: 'repriceAmount',
                //    //name: 'Total Reprice$',
                //    enableColumnMenu: false,
                //    cellFilter: 'currency',
                //    aggregationType: uiGridConstants.aggregationTypes.sum,
                //    footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue() | currency}}</div>'
                //},
                //{
                //    field: 'averageCloseTime',
                //    //name: 'Average Close Time',
                //    enableColumnMenu: false,
                //    aggregationType: uiGridConstants.aggregationTypes.avg,
                //    footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue()}}</div>'
                //},
                //{
                //    field: 'averageFallOut',
                //    //name: 'Average Fall Out',
                //    enableColumnMenu: false,
                //    aggregationType: uiGridConstants.aggregationTypes.avg,
                //    footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue()}}</div>'
                //},
                {
                    field: 'purchasesAmount',
                    //name: 'Total Purchases',
                    enableColumnMenu: false,
                    cellFilter: 'currency',
                    aggregationType: uiGridConstants.aggregationTypes.sum,
                    footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue() | currency}}</div>'
                },
                //{
                //    field: 'salesAmount',
                //    //name: 'Total Sales',
                //    enableColumnMenu: false,
                //    cellFilter: 'currency',
                //    aggregationType: uiGridConstants.aggregationTypes.sum,
                //    footerCellTemplate: '<div class="ui-grid-cell-contents" >{{col.getAggregationValue() | currency}}</div>'
                //},
            ]
        };

        vm.yearlySummaryGridOption.data = tradeYearlySummaries;
    }
}
());