(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradeEditCtrl", ["trade", "tradeResource", "tradeStageResource", "$state", "$scope", "$ngConfirm", "$stateParams", "FileUploader", "tradeTapeResource", "$uibModal", TradeEditCtrl]);

    function TradeEditCtrl(trade, tradeResource, tradeStageResource, $state, $scope, $ngConfirm, $stateParams,  FileUploader, tradeTapeResource, $uibModal) {
        
        var vm = this;
        vm.trade = trade;
        vm.availableStageOptions = [];
        vm.selectedStageDropdownParams = "";
        vm.selectedStageDate = "";
        vm.selectedStage = ""; //This is used to keep track of the selected stage id.  This also serves as the modal for the counterparty dropdown disablement (ie when selected stage has a value the counter party drop down will be disabled)
        vm.availableCounterPartyOptions = [];
        vm.selectedCounterPartyId = "";
        vm.validationErrors = "";

        //data modals setup
        if (vm.trade && vm.trade.tradePoolStages) {
            vm.selectedStage = vm.trade.tradePoolStages.slice(-1)[0] ? vm.trade.tradePoolStages.slice(-1)[0].lU_TradeStage : "";
        }
        if (vm.selectedStage) {
            vm.selectedStageDropdownParams = vm.selectedStage.stageID;
            vm.selectedStageDate = new Date(vm.trade.tradePoolStages.slice(-1)[0].tradeStageDate);
        }


        tradeStageResource.query(function (data) {
            vm.availableStageOptions = data;
        });

        tradeResource.getcounterparties(function (data) {
            vm.availableCounterPartyOptions = data;
            vm.availableCounterPartyOptions.push({ counterPartyID: -1, counterPartyName: "+ New", bold: true });
            if (vm.trade != null) {
                vm.selectedCounterPartyId = vm.trade.counterPartyID;
            }
            else {
                vm.selectedCounterPartyId = vm.availableCounterPartyOptions[0].counterPartyID;
            }
        });

        $scope.uploader = new FileUploader();
        $scope.uploader.url = "http://localhost:3666/" + "api/tradetapes";
        $scope.queueLimit = 1;
        $scope.uploader.onBeforeUploadItem = function (fileItem) {
            fileItem.formData.push({ TradeID: vm.trade.tradeID });
            fileItem.formData.push({ Name: vm.trade.tradeName });
            fileItem.formData.push({ Description: 'Desc' });
            vm.validationErrors = "";
            vm.FileUploading = true;
            vm.progressBarValue = 0;
            vm.progressStatusPrefix = "File uploading - "
        };
        $scope.uploader.onAfterAddingFile = function (fileItem) {
            vm.FileUploading = false; //reset this flag to hide the status bar from a previous upload
            vm.validationErrors = ""; //reset this to hide validation errors from a previous upload
        }

        $scope.uploader.onProgressAll = function (val) {
            vm.progressBarValue = val;
            vm.progressStatus = vm.progressStatusPrefix + val + "%";
        }

        $scope.uploader.onCompleteAll = function () {
            console.log("File uploaded successfully");
            //toastr.success("File uploaded successfully");
            //vm.FilUploading = false;
            $scope.uploader.clearQueue();
            tradeTapeResource.query({ tradefilter: "$filter=TradeID eq " + vm.trade.tradeID }, function (data) {
                vm.tradeTape = data[0];
                console.log("Calling parser validation");
                //toastr.warning("File being imported....");
                vm.progressBarValue = 20;
                vm.progressStatus = "Validating the tape...."
                tradeTapeResource.validatetape({ id: vm.tradeTape.tapeID },
                    function (data) {
                        vm.progressBarValue = 60;
                        vm.progressStatus = "Importing the tape...."
                        tradeTapeResource.importtape({ id: vm.tradeTape.tapeID },
                            function (data) {
                                //toastr.success("File successfully imported!");
                                vm.progressBarValue = 90;
                                vm.progressStatus = "Finalizing...."
                                var newdate = new Date();
                            //Change the Pool stage from 'Pool Identified' to 'Out for Bid'
                                vm.trade.tradePoolStages.push(
                                    {
                                        'tradeID': vm.trade.tradeID,
                                        'stageID': vm.availableStageOptions[1].stageID, // the second stage option is 'Out for Bid' vm.selectedStageDropdownParams + 1, 
                                        'tradeStageDate': (newdate.getMonth() + 1) + "/" + newdate.getDate() + "/" + newdate.getFullYear()
                                    })
                                tradeResource.update({ Id: vm.trade.tradeID }, vm.trade)
                                vm.progressBarValue = 100;
                                vm.progressStatus = "Import successful...."
                                $state.go('tradeDetail', { tradeId: vm.trade.tradeID });
                        })
                    },
                    //Tape validation errors
                    function (error) {
                        console.log("validation errors: " + error.data);
                        //vm.progressBarValue = 100;
                        //vm.progressStatus = "Validation failed!!"
                        var parserError = JSON.parse(error.data.modelState.tapeParserError);
                        vm.validationErrors = JSON.stringify(parserError, null, "    ");
                    })
                
            })
        }

        
        
        vm.counterPartyChange = function () {
            
            if(vm.trade != null && vm.trade.counterPartyID == "-1")
            {
                console.log('Opening new counterparty pop up');
                var modalInstance = $uibModal.open({
                    templateUrl: '/app/trading/tradeCounterPartyModalView.html',
                    controller: 'tradePopupCtrl',
                    controllerAs: 'vmCtrl',
                    resolve: {
                        prevSelection: vm.selectedCounterPartyId
                    }
                    
                });

                modalInstance.result.then(function (selectedID) {
                    
                    console.log("selectedID : " + selectedID);
                    //In case the user cancels the modal dialog or in case of an error then +New will be selected in the dropdown.  Set it back to the previous value
                    console.log(vm.trade.counterPartyID);
                    if (selectedID != "-1") {
                        console.log("Refresh the counter party list")
                        //Refresh the counter party list
                        tradeResource.getcounterparties(function (data) {
                            vm.availableCounterPartyOptions = data;
                            vm.availableCounterPartyOptions.push({ counterPartyID: -1, counterPartyName: "+ New", bold: true });
                        });
                        vm.trade.counterPartyID = vm.selectedCounterPartyId = selectedID;
                    }
                });
               
            }

            vm.selectedCounterPartyId = vm.trade.counterPartyId;
        }

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
                vm.trade.tradeType = "Purchase";
                vm.trade = tradeResource.save(vm.trade);
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
                    for (var i = 0; i < vm.trade.tradePoolStages.length; i++) {
                        if(vm.trade.tradePoolStages[i].stageID == vm.selectedStageDropdownParams)
                        {
                            vm.trade.tradePoolStages[i].tradeStageDate = enteredStageDate;
                        }
                    }
                }
                tradeResource.update({ Id: vm.trade.tradeID }, vm.trade);
                //Reload the trade information to get the tradePoolStages with the DB generated IDs
            }

            $ngConfirm({
                content: "The " + vm.trade.tradeName + " Saved successfully!!",
                buttons: {
                    ok: {
                        text: 'OK',
                        btnClass: 'btn-blue',
                        action: function (scope, button) {
                            //$state.go('trading', {}, { reload: true });
                            $state.go('tradeDetail', { tradeId: vm.trade.tradeID });
                            }
                    }

                }
            });

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
                            $ngConfirm({
                                content: "The " + vm.trade.tradeName + " has been deleted.",
                                buttons: {
                                    ok: {
                                        text: 'OK',
                                        btnClass: 'btn-blue',
                                        action: function (scope, button) {
                                            $state.go('tradingYearlySummaries', {}, { reload: true });
                                        }
                                    }
                                    
                                }
                            });
                        }
                    },
                    close: function (scope, button) {
                    }
                }
            });
        }

        vm.cancel = function () {
            //$state.go('trading');
            if (vm.trade && vm.trade.tradeID) {
                $state.go('tradeDetail', { tradeId: vm.trade.tradeID });
            }
            else {
                $state.go('tradingYearlySummaries', {}, { reload: true });
            }
           
        }
    }
}
());