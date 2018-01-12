(function () {
    "use strict";
    var app = angular.module("palisadesDashboard");
    app.controller("tradeAssetsDiligenceCtrl", ["tradeAsset", 'tradeAssetResource', TradeAssetsDiligenceCtrl]);

    function TradeAssetsDiligenceCtrl(tradeAsset, tradeAssetResource) {

        var vm = this;
        vm.tradeAsset = tradeAsset;

        vm.panelTitle = "New Diligence Item for AssetID: " + vm.tradeAsset.assetID;

        vm.diligenceItem = [];
        vm.diligenceTypes = [];


        vm.diligenceItem.push({
            assetID: vm.tradeAsset.assetID,
            bidAmount: 0,
            bidPricePct: 0,
            typeName: '',
            categoryID: '',
            descriptionID: '',
        });

        vm.diligenceGridOption = "",

        tradeAssetResource.getdiligencetypes().$promise.then(function (data) {
            alert(data);
            alert(data.length);
            vm.diligenceGridOption.columnDefs[4].editDropdownOptionsArray = data;
            vm.diligenceGridOption.columnDefs[4].editableDropdownIdLabel = 'id';
            vm.diligenceGridOption.columnDefs[4].editableDropdownValueLabel = 'typeName';
            //vm.diligenceGridOption.columnDefs[4].cellFilter = 'griddropdown';

        });

        vm.diligenceGridOption = {
            //showGridFooter: true,
            showColumnFooter: true,
            cellFilter: 'griddropdown:this',
            columnDefs: [
                {
                    field: 'assetID',
                    name: 'Asset Id',
                    cellTemplate: '<div class="ui-grid-cell-contents"><a ui-sref="tradeAsset({assetId:{{COL_FIELD}}})">{{COL_FIELD}}</a></div>',
                    enableColumnMenu: false,
                    enableCellEdit: true
                },
                {
                    field: 'bidAmount',
                    name: 'Bid Amt',
                    enableColumnMenu: false,
                    enableCellEdit: true,
                },
                {
                    field: 'bidAmtValue',
                    name: 'Bid Amt Val',
                    enableColumnMenu: false,
                    enableCellEdit: true,
                },
                {
                    field: 'bidPricePct',
                    name: 'Bit Price %',
                    enableColumnMenu: false,
                    enableCellEdit: true,
                },
                {
                    field: 'typeName',
                    name: 'Type',
                    enableColumnMenu: false,
                    editableCellTemplate: 'ui-grid/dropdownEditor',
                    enableCellEdit: true,
                   
                },
                {
                    field: 'categoryName',
                    name: 'Category',
                    enableColumnMenu: false,
                    editableCellTemplate: 'ui-grid/dropdownEditor',
                    editDropdownOptionsArray: vm.diligenceCategory,
                    enableCellEdit: true,
                },
                {
                    field: 'description',
                    name: 'Description',
                    enableColumnMenu: false,
                    editableCellTemplate: 'ui-grid/dropdownEditor',
                    editDropdownOptionsArray: vm.deligenceDescription,
                    enableCellEdit: true,
                },

            ]
        };
        vm.diligenceGridOption.data = vm.diligenceItem;
    }
}
()
//);

);