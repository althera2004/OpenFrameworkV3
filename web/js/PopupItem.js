var PopupItem_Context = {
    "ItemDefinition": null
};

function PopupItem_Render() {
    $("#PopupItem").remove();
    var title = PopupItem_Context.ItemId < 0 ? Dictionary.Common_Add : Dictionary.Common_Edit;
    title += " " + PopupItem_Context.ItemDefinition.Layout.Label;

    var popup = "<div id=\"PopupItem\" class=\"modal fade\" tabindex=\"-1\">";
    popup += "        <div class=\"modal-dialog\">";
    popup += "            <div class=\"modal-content\">";
    popup += "                <div class=\"modal-header no-padding\">";
    popup += "                    <div class=\"table-header bgblue\">";
    popup += "                        <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">";
    popup += "                            <span class=\"white\">&times;</span>";
    popup += "                        </button>";
    popup += "                        <span id=\"PopupSaveInfoTitle\" class=\"white\">" + title + "</span>";
    popup += "                    </div>";
    popup += "                </div>";
    popup += "                <div class=\"modal-body\" id=\"PopupItemForm\">";

    popup += PopupItem_Context.Form.RenderPopup();

    popup += "                </div>";
    popup += "                <div class=\"modal-footer no-margin-top\">";
    popup += "                  <span id=\"PopupEditErrorResume\" style=\"color:red;float:left;\"></span>";
    popup += "                    <button type=\"button\" class=\"btn btn-sm pull-right\" data-dismiss=\"modal\" id=\"PopupItemBtnClose\">";
    popup += "                        <i class=\"ace-icon fa fa-check\"></i>&nbsp;" + Dictionary.Common_Exit;
    popup += "                    </button>";
    popup += "                    <button type=\"button\" class=\"btn btn-sm pull-right btn-success\" id=\"PopupSaveInfoBtnOkAndExit\" onclick=\"PopupItem_SaveData();\">";
    popup += "                        <i class=\"fas fa-sign-out-alt\"></i>&nbsp;" + Dictionary.Common_Save;
    popup += "                    </button>";
    popup += "                </div>";
    popup += "            </div>";
    popup += "        </div>";
    popup += "    </div>";
    $("#PopupsContentHolder").append(popup);
}

function PopupItem(config) {

    PopupItem_Context.ItemDefinition = ItemDefinitionByName(config.ItemName);
    PopupItem_Context.ListId = config.ListId;
    PopupItem_Context.ItemId = config.ItemId;

    // FKS
    // ---------------------------
    var fks = PopupItem_Context.ItemDefinition.ForeignValues;
    PopupItem_Context.FKsToLoad = fks.length;
    for (var f = 0; f < fks.length; f++) {
        if (typeof FK[fks[f].ItemName] === "undefined") {
            GetFKItem(fks[f].ItemName, PopupItemFKLoaded);
        }
        else {
            PopupItem_Context.FKsToLoad--;
        }
    }
    // ---------------------------

    PopupItem_Context.ListDefinition = PopupItem_Context.ItemDefinition.Lists.filter(function (l) { return l.Id === config.ListId; })[0];
    PopupItem_Context.Form = new PageForm({ "ItemId": PopupItem_Context.ItemDefinition.Id, "FormId": PopupItem_Context.ListDefinition.FormId });
    PopupItem_Context.Form.Init();
    PopupItem_Render();
    PopupItemFKLoaded();
}

function PopupItemFKLoaded() {
    PopupItem_Context.FKsToLoad--;
    console.log(PopupItem_Context.FKsToLoad);
    
    if (PopupItem_Context.FKsToLoad < 0) {

        // Fill combos
        var fks = PopupItem_Context.ItemDefinition.ForeignValues;
        for (var f = 0; f < fks.length; f++) {
            var data = FK[fks[f].ItemName].Data;
            console.log(data);
            FillComboFromFK(fks[f].ItemName + "Id", fks[f].ItemName);
        }

        $("#LauncherPopupItem").click();
        console.log(PopupItem_Context.ListDefinition);

        for (var p = 0; p < PopupItem_Context.ListDefinition.Parameters.length; p++) {
            var parameter = PopupItem_Context.ListDefinition.Parameters[p];
            if (parameter.Value = "#actualItemId#") {
                $("#" + parameter.Name).val(ItemData.OriginalItemData.Id);
                $("#" + parameter.Name).disable();
                FieldToLabel(parameter.Name);
                break;
            }
        }
    }


    $("#PopupItemForm .datepicker").localDatePicker();
}

function PopupItem_SaveData() {
    var differences = [];

    $.each($("#PopupItemForm .form-control"), function () {
        var id = $(this)[0].id;

        console.log(id, $("#" + id).val());
        differences.push({ "Field": id, "Original": null, "Actual": $("#" + id).val() });
    });

    var data = {
        "itemDefinitionId": PopupItem_Context.ItemDefinition.Id,
        "itemId": PopupItem_Context.ItemId,
        "itemData": JSON.stringify(differences, null, 2),
        "applicationUserId": ApplicationUser.Id,
        "instanceName": Instance.Name,
        "companyId": Company.Id
    };

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/ItemService.asmx/Save",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            if (msg.d.Success === true) {
                $("#PopupItemBtnClose").click();
                NotifySaveSuccess(msg.d.ReturnValue);
                PageListById(PopupItem_Context.ItemDefinition.ItemName, PopupItem_Context.ListId).GetData();
                
            }
            else {
                NotifySaveError(msg.d.MessageError);
            }
        },
        "error": function (msg) {
            NotifySaveError(msg.responseText);
        }
    });
}