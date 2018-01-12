(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradePopupCtrl", ["$uibModalInstance", 'tradeResource', TradePopupCtrl]);

    function TradePopupCtrl($uibModalInstance, tradeResource) {
        var vm = this;
        
        vm.counterPartyName = "";
        vm.cpID = "";
        vm.close = function () {
            $uibModalInstance.close('-1');
        }

        vm.submit = function () {
            var err = false;
            console.log("posting the new counterparty");
            tradeResource.postcounterparty({ counterPartyName: vm.counterPartyName }).$promise.then(
            function (data) {
                vm.cpID = data.counterPartyID;
                console.log("Success counterpartyID:" + vm.cpID + " !!");
                console.log('calling dismiss');
                $uibModalInstance.close(vm.cpID);
            },
            function (error) {
                alert(error);
                console.log(error);
                err = true;
            });
        }
    }


}
());