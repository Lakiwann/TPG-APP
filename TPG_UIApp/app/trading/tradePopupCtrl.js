(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradePopupCtrl", ["$uibModalInstance", "prevSelection", 'tradeResource', TradePopupCtrl]);

    function TradePopupCtrl($uibModalInstance, prevSelection, tradeResource) {
        var vm = this;
        vm.counterPartyName = "";
        vm.cpID = "";
        vm.serverErrors = "";
        vm.close = function () {
            $uibModalInstance.close(prevSelection);
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
                console.log(error);
                vm.serverErrors = error.data.message;
                err = true;
            });
        }
    }
}
());