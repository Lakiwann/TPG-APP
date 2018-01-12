(function () {
    "use strict";
    var app = angular.module("common.services");
    app.factory("tradeResource", ["$resource", TradeResource]);

    function TradeResource($resource) {
        return $resource("http://localhost:3666/" + "api/tradepools/:Id",
            {tradeId:'@tradeId'}, //Parameters that can be passed into the resouce.  They can be used in the other resource methods (for example in the 'summary' method below)
            {
                'update': {
                    method: 'PUT'
                },
                'getsummary': {
                    method: 'GET',
                    url: "http://localhost:3666/" + "api/tradepools/:tradeId/summary"
                },
                'getassetsummary': {
                    method: 'GET',
                    isArray: true,
                    url: "http://localhost:3666/" + "api/tradepools/:tradeId/assetsummary"
                },
                'getyearlysummaries': {
                    method: 'GET',
                    isArray: true,
                    url: "http://localhost:3666/" + "api/tradepools/yearlysummary/:year"
                },
                'getcounterparties': {
                    method: 'GET',
                    isArray: true,
                    url: "http://localhost:3666/" + "api/tradepools/counterparties/"
                },
                'postcounterparty': {
                    method: 'POST',
                    url: "http://localhost:3666/" + "api/tradepools/counterparties/"
                }
            });
    }
}());