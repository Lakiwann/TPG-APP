(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradeAssetCtrl", ["tradeAsset", 'tradeResource', 'uiGridConstants', TradeAssetCtrl]);

    function TradeAssetCtrl(tradeAsset, tradeResource, uiGridConstants) {

        var vm = this;
        if (tradeAsset.palID == "")
        {
            tradeAsset.palID = 'Pending';
        }

        vm.tradeAsset = tradeAsset;

        //alert(tradeAsset.palId);

        tradeResource.get({ id: vm.tradeAsset.tradeID }, function (data) {
            vm.tradeSummary = data;
        }).$promise

        vm.assetGridOption = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.ALWAYS,
                enableFiltering: false,
                enableColumnResizing: true,
                data: [tradeAsset],
                columnDefs: [
                    {
                        field: 'palId'
                        , enableHiding: false
                        , enableFiltering: true
                        , width: 100
                    },
                    {
                        field: 'status'
                        , enableHiding: false
                        , enableFiltering: true
                        , width: 100
                    },
                    {
                        field: 'originalBalance'
                        //, name: 'Reprice Amt'
                        , enableHiding: false
                        , enableFiltering: false
                        , cellFilter: 'currency'
                         , width: 100
                    },
                    {
                        field: 'currentBalance'
                        //, name: 'Reprice Amt'
                        , enableHiding: false
                        , enableFiltering: false
                        , cellFilter: 'currency'
                         , width: 100
                    },
                    {
                        field: 'forbearanceBalance'
                        //, name: 'Reprice Amt'
                        , enableHiding: false
                        , enableFiltering: false
                        , cellFilter: 'currency'
                         , width: 100
                    },
                    {
                        field: 'numberOfIssues'
                        , name: 'Issues'
                        , enableHiding: false
                        , enableFiltering: false
                          , width: 100
                    },
                    {
                        field: 'bpo'
                        //, name: 'Reprice Amt'
                        , enableHiding: false
                        , enableFiltering: false
                        , cellFilter: 'currency'
                          , width: 100
                    },
                    {
                        field: 'bpoDate'
                        //, name: 'Reprice Amt'
                        , enableHiding: false
                        , enableFiltering: false
                        , cellFilter: 'date'
                        , width: 100
                    }
                    ,
                     {
                         field: 'currentPmt'
                         //, name: 'Reprice Amt'
                        , enableHiding: false
                        , enableFiltering: false
                        , cellFilter: 'currency'
                         , width: 100
                     },
                    {
                        field: 'paidToDate'
                        //, name: 'Reprice Amt'
                        , enableHiding: false
                        , enableFiltering: false
                        , cellFilter: 'date'
                        , width: 100
                    },
                     {
                         field: 'nextDueDate'
                         //, name: 'Reprice Amt'
                        , enableHiding: false
                        , enableFiltering: false
                        , cellFilter: 'date'
                         , width: 100
                     },
                      {
                          field: 'maturityDate'
                          //, name: 'Reprice Amt'
                        , enableHiding: false
                        , enableFiltering: false
                        , cellFilter: 'date'
                         , width: 100
                      },
                    {
                        field: 'streetAddress1'
                        , enableHiding: false
                        , enableFiltering: false
                        , width: 100
                    },
                    {
                        field: 'city'
                        , enableHiding: false
                        , enableFiltering: false
                        , width: 100
                    }
                    ,
                    {
                        field: 'state'
                        , enableHiding: false
                        , enableFiltering: false
                        , width: 100
                    },
                    {
                        field: 'zip'
                        , enableHiding: false
                        , enableFiltering: false
                         , width: 100
                    },
                    {
                        field: 'cbsa'
                        , enableHiding: false
                        , enableFiltering: false
                         , width: 100
                    },
                    {
                        field: 'cbsaName'
                        , enableHiding: false
                        , enableFiltering: false
                         , width: 100
                    },
                    {
                        field: 'proType'
                        , enableHiding: false
                        , enableFiltering: false
                        , width: 100
                    },
                    {
                        field: 'loanPurp'
                        , enableHiding: false
                        , enableFiltering: false
                        , width: 100
                    },
                    {
                        field: 'propType'
                        , enableHiding: false
                        , enableFiltering: false
                        , width: 100
                    }
                    ,
                    {
                        field: 'origFico'
                        , enableHiding: false
                        , enableFiltering: false
                         , width: 100
                    },
                    {
                        field: 'currFico'
                        , enableHiding: false
                        , enableFiltering: false
                         , width: 100
                    },
                    {
                        field: 'currFicoDate'
                        , enableHiding: false
                        , enableFiltering: false
                        , cellFilter: 'date'
                         , width: 100
                    },
                    {
                        field: 'payString'
                        , enableHiding: false
                        , enableFiltering: false
                         , width: 500
                    }
                ]
            };

           
       
       

        

        

        
    }
}
());