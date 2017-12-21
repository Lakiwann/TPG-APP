(function () {
    "use strict";
    var app = angular.module("common.services");
    app.factory("tradeResource", ["$resource", TradeResource]);

    function TradeResource($resource) {
        return $resource("http://localhost:3666/" + "api/tradepools/:Id", null, {
            'update': { method: 'PUT' }
        });
    }
}());