(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradeEditCtrl", ["trade", "tradeResource", "tradeStageResource", "$state", "$scope", "$ngConfirm", "$stateParams", TradeEditCtrl]);

    function TradeEditCtrl(trade, tradeResource, tradeStageResource, $state, $scope, $ngConfirm, $stateParams) {
        
        var vm = this;
        vm.trade = trade;
        vm.availableStageOptions = [];
        
        tradeStageResource.query(function (data) {
            vm.availableStageOptions = data;
        });

        if (vm.trade && vm.trade.tradeID) {
            vm.title = "Edit:" + vm.trade.tradeName;
            vm.datePickerDate = new Date(vm.trade.estSettlementDate);
        }
        else {
            vm.title = "New Trade";
            //vm.trade = { tradeID: 0, tradeName: '', estSettlementDate: '', managerName: '', managerInitials: '' };
            vm.newTrade = true;
        }

        vm.selectedTradeStage = function (stageID) {
            if(stageID == vm.trade.tradePoolStages.slice(-1)[0].lU_TradeStage.stageID) {
                vm.isValidTradeStageSelected = true;
                return true;
            }
            return false;
        }

        //Calendar event function
        vm.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            vm.opened = !vm.opened;
        };

        vm.submit = function () {
            var monthNames = ["January", "February", "March", "April", "May", "June",
  "July", "August", "September", "October", "November", "December"
            ];
            vm.trade.estSettlementDate = monthNames[vm.datePickerDate.getMonth()] + ' ' + vm.datePickerDate.getDate() + ', ' + vm.datePickerDate.getFullYear();
            if (vm.newTrade == true) {
                vm.trade.tradeType = "Sale";
                vm.trade = tradeResource.save(vm.trade);
                vm.title = "Edit:" + vm.trade.tradeName;
            }
            else {
                vm.trade.$save(function (data) {

                });
            }
            toastr.success("Save Successful");
            $scope.tradeEditForm.$pristine = true;

        }

        vm.deleteTrade = function (trade) {
            $ngConfirm({
                title: 'Please confirm!!',
                content: "Do you want to delete the trade - " + vm.trade.tradeName + "?",
                scope: $scope.tradeEditForm.deleteBtn,
                buttons: {
                    yesDelete: {
                        text: 'Yes, delete',
                        btnClass: 'btn-blue',
                        action: function (scope, button) {
                            tradeResource.delete({ Id: vm.trade.tradeID });
                            //reload the state params so the modal will get reset
                            $state.transitionTo($state.current, $stateParams, {
                                reload: true,
                                inherit: false,
                                notify: true
                            });
                            $state.go('trading');
                            $ngConfirm("The " + vm.trade.tradeName + " has been deleted.");
                        }
                    },
                    close: function (scope, button) {
                        //closes the modal
                    }
                }
            })
        }

        vm.cancel = function () {
            $state.go('trading');
        }
    }
}
());