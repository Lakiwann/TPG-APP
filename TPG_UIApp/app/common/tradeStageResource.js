(function () {
    "use strict";
    var app = angular.module("common.services");
    app.factory("tradeStageResource", ["$resource", TradeStageResource]);

    function TradeStageResource($resource) {
        return $resource("http://localhost:3666/" + "api/tradestages/:Id");
    }
}());