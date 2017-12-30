(function () {
    "use strict";
    var app = angular.module("common.services");
    app.factory("tradeTapeResource", ["$resource", TradeTapeResource]);

    function TradeTapeResource($resource) {
        return $resource(
              "http://localhost:3666/" + "api/tradetapes?:tradefilter",
              {tapeId:'@id'}, 
              
              { 
            'update': {
                method: 'PUT'
            },
            'importtape': {
                method: 'POST',
                url: "http://localhost:3666/" + "api/tradetapes/:tapeId/import"
            }
        });
    }
}());