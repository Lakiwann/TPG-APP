(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradeEditCtrl", ["trade", "tradeResource", "tradeStageResource", "$state", "$scope", "$ngConfirm", "$stateParams", TradeEditCtrl]);

    function TradeEditCtrl(trade, tradeResource, tradeStageResource, $state, $scope, $ngConfirm, $stateParams) {
        
        var vm = this;
       // vm.data = [{ 'id': '1', 'name': { 'first': 'Lakshan', 'last': 'W' }, 'address': '23903 Brio Ct', 'price': '23', 'isActive': '1' }, { 'id': '2', 'name': { 'first': 'Aparna', 'last': 'W' }, 'address': '23903 Brio Ct', 'price': '23', 'isActive': '0' }];
       // alert(vm.data[0].id + "|" + vm.data[0].address + vm.data[1].id + "|" + vm.data[1].address);
        vm.trade = trade;
        vm.availableStageOptions = [];
        vm.selectedStageDropdownParams = "";
        vm.selectedStageDate = "";
        vm.selectedStage = "";
        if (vm.trade && vm.trade.tradePoolStages) {
            vm.selectedStage = vm.trade.tradePoolStages.slice(-1)[0] ? vm.trade.tradePoolStages.slice(-1)[0].lU_TradeStage : "";
        }
       // vm.selectedStage = vm.trade.tradePoolStages.slice(-1)[0] ? vm.trade.tradePoolStages.slice(-1)[0].lU_TradeStage : "";
       
        //vm.selectedStageDropdownParams = "{name: "+ vm.selectedStage.stageName + ", value: " + vm.selectedStage.stageID +"}";
        if (vm.selectedStage) {
            vm.selectedStageDropdownParams = vm.selectedStage.stageID;
            //alert(vm.selectedStage.trade.tradePoolStages.slice(-1)[0].tradeStageDate);
            vm.selectedStageDate = new Date(vm.trade.tradePoolStages.slice(-1)[0].tradeStageDate);
        }
        
        
        tradeStageResource.query(function (data) {
            vm.availableStageOptions = data;
        });

        if (vm.trade && vm.trade.tradeID) {
            vm.title = "Edit:" + vm.trade.tradeName;
            vm.datePickerDate = new Date(vm.trade.estSettlementDate);
        }
        else {
            vm.title = "New Trade";
            vm.newTrade = true;
        }

        //Calendar event function
        vm.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            vm.opened = !vm.opened;
        };

        //Calendar event function
        vm.open2 = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            vm.opened2 = !vm.opened2;
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
                var today = new Date();
                var enteredStageDate = monthNames[vm.selectedStageDate.getMonth()] + '/' + vm.selectedStageDate.getDate() + '/' + vm.selectedStageDate.getFullYear();
                //If the stage selection's stage id is greater than the current stageId for the trade then create a new tradePoolStage
                if ((vm.selectedStageDropdownParams != "") && ((vm.selectedStage == "") || (vm.selectedStageDropdownParams > vm.selectedStage.stageID))) {
                    vm.trade.tradePoolStages.push(
                        {
                            'tradeID': vm.trade.tradeID,
                            'stageID': vm.selectedStageDropdownParams,
                            'tradeStageDate': enteredStageDate
                        })
                }
                //If the stage selection's stage id is the same as the current stage id, then only the stage date may have been changed by the user so update the stage date
                if ((vm.selectedStageDropdownParams != "") && ((vm.selectedStage != ""))) {
                    //vm.trade.tradePoolStages.slice(-1)[0].tradeStageDate = enteredStageDate;
                    for (var i = 0; i < vm.trade.tradePoolStages.length; i++) {
                        if(vm.trade.tradePoolStages[i].stageID == vm.selectedStageDropdownParams)
                        {
                            vm.trade.tradePoolStages[i].tradeStageDate = enteredStageDate;
                        }
                    }
                }
                tradeResource.update({ Id: vm.trade.tradeID }, vm.trade);
                //Reload the trade information to get the tradePoolStages with the DB generated IDs
                //vm.trade = tradeResource.get({ Id: vm.trade.tradeID }).$promise;
            }
            toastr.success("Save Successful");
            //$scope.tradeEditForm.$pristine = true;
            $state.go('trading');

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