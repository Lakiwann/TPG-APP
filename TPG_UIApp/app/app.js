(function () {

    "use strict";

    var app = angular.module("palisadesDashboard", ["common.services"
                                                    , "ui.router"
                                                    , "ui.mask"
                                                    , "ui.bootstrap",
                                                    , "cp.ngConfirm"
                                                    , "angularFileUpload"
                                                    , "ngTouch"
                                                    , "ui.grid"]);

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
                  url: "/trading",
                  templateUrl: "/app/trading/tradeListView.html",
                  controller: "tradeListCtrl as vm"
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
                    productResource: "tradeResource",
                    trade: function (tradeResource, $stateParams) {
                        var tradeId = $stateParams.tradeId;
                        return tradeResource.get({ Id: tradeId }).$promise;
                    }
                }
            })

            $urlRouterProvider.otherwise("/");
        }
    ])
}());