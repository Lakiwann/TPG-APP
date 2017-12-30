(function () {
    "use strict";
    var app = angular.module("common.services");
    app.factory("tradeAssetResource", ["$resource", TradeAssetResource]);

    function TradeAssetResource($resource) {
        return $resource(
              "http://localhost:3666/" + "api/tradeassets?:tradefilter",
              null,

              {
                  'update': {
                      method: 'PUT'
                  },
                  
              });
    }
}());