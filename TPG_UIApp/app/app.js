(function () {

    "use strict";

    var app = angular.module("palisadesDashboard", ["common.services"
                                                    , "ui.router"
                                                    , "ui.mask"
                                                    , "ui.bootstrap",
                                                    , "cp.ngConfirm"
                                                    , "angularFileUpload"
                                                    , "ngTouch"
                                                    , "ui.grid"
                                                    , "ui.grid.pagination"
                                                    , "ui.grid.autoResize"
                                                    ]);

    app.config(['$locationProvider',
         function ($locationProvider) {
             $locationProvider.hashPrefix('');
         }
    ]);

    app.config(["$stateProvider", "$urlRouterProvider",
        function ($stateProvider, $urlRouterProvider) {
            $stateProvider
            .state("Home",
              {
                  url: "/",
                  templateUrl: "/app/WelcomeView.html"
              })
            .state("trading",
              {
                  cache: false,
                  url: "/trading",
                  templateUrl: "/app/trading/tradeListView.html",
                  controller: "tradeListCtrl as vm",

                  resolve: {
                      tradeResource: "tradeResource",
                      trades: function (tradeResource) {
                          return tradeResource.query().$promise;
                      }
                  }
              })
            .state("tradeEdit",
            {
                url: "/trading/edit/:tradeId",
                templateUrl: "/app/trading/tradeEditView.html",
                controller: "tradeEditCtrl as vm",

                resolve: {
                    tradeResource: "tradeResource",
                    trade: function (tradeResource, $stateParams) {
                        var tradeId = $stateParams.tradeId;
                        if (tradeId != 0)
                            return tradeResource.get({Id:tradeId}).$promise;
                    }
                }
               
            })
            
            //.state("productEdit.info", {
            //    url: "/info",
            //    templateUrl: "/app/products/productEditInfoView.html",
            //})
            // .state("productEdit.price", {
            //     url: "/price",
            //     templateUrl: "/app/products/productEditPriceView.html",
            // })
            // .state("productEdit.tags", {
            //     url: "/tags",
            //     templateUrl: "/app/products/productEditTagsView.html",
            // })
            .state("tradeDetail", {
                url: "/trading/detail/:tradeId",
                templateUrl: "/app/trading/tradeDetailView.html",
                controller: "tradeDetailCtrl as vm",

                resolve: {
                    tradeResource: "tradeResource",
                    trade: function (tradeResource, $stateParams) {
                        var tradeId = $stateParams.tradeId;
                        return tradeResource.get({ Id: tradeId }).$promise;
                    }
                }
            })
            .state("tradeAsset",
            {
                url: "/trading/asset/:assetId",
                templateUrl: "/app/trading/tradeAssetSummaryView.html",
                controller: "tradeAssetCtrl as vm",

                resolve: {
                    tradeAssetResource: "tradeAssetResource",
                    tradeAsset: function (tradeAssetResource, $stateParams) {
                        var assetId = $stateParams.assetId;
                        if(assetId != 0)
                            return tradeAssetResource.get({Id:assetId}).$promise;
                    }
                }
            })
            $urlRouterProvider.otherwise("/");
        }
    ])
}());