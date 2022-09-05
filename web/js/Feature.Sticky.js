function Feature_Sticky_ShowPopup() {
    if(HasPropertyValue(ItemDefinition.Id)) {
        $("#PopupStickyItemDefinitionId").val(ItemDefinition.Id);
        $("#PopupStickyItemId").val(ItemData.ActualData.Id);
    }
    else {
        $("#PopupStickyItemDefinitionId").val(-1);
        $("#PopupStickyItemId").val(-1);
    }

    $("#PopupStickyRBU").prop("checked", false);
    $("#PopupStickyRBG").prop("checked", false);
    //("#PopupStickyCmbUser").hide();
    //$("#PopupStickyCmbGroup").hide();
    $("#PopupStickyCmbUser").val(-1);
    $("#PopupStickyCmbGroup").val(-1);
    $("#PopupStickyText").val("");


    $("#PopupStickyError").hide();
    $("#LauncherPopupSticky").click();
}

function Feature_Sticky_SavePopup() {
    var data = {
        "itemDefinitionId": ItemDefinition.Id,
        "itemId": ItemData.ActualData.Id,
        "companyId": Company.Id,
        "text": $("#PopupStickyText").val(),
        "target": $("#PopupStickyTarget").val(),
        "applicationUserId": ApplicationUser.Id,
        "instanceName": Instance.Name
    };
    console.log(data);
    $.ajax({
        "type": "POST",
        "url": "/Async/ItemService.asmx/FeatureStickySave",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg);
            NotifySuccess("Operación realizada con éxito", Dictionary.Feature_Sticky);
            $("#PopupStickyBtnCancel").click();
            
        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}

function Feature_Sticky_Delete(id) {
    var data = {
        "stickyId": id,
        "companyId": CompanyId,
        "applicationUserId": ApplicationUser.Id
    };
    console.log(data);
    $.ajax({
        "type": "POST",
        "url": "/Async/ItemDataServices.asmx/FeatureStickyDelete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg.d.ReturnValue);
            NotifySuccess(Dictionary.Feature_Sticky + " eliminat correctament", Dictionary.Feature_Sticky);
            $(".Sticky_" + id).fadeOut();
        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}

function Feature_Sticky_RBTargetsChanged() {
    if ($("#StickyRB0").prop("checked") === true) {
        $("#StickyPopup_TargetsDiv").hide();
        $("#PopupStickyTarget").val(ApplicationUser.Id);
    }
    if ($("#StickyRB1").prop("checked") === true) {
        $("#StickyPopup_TargetsDiv").hide();
        $("#PopupStickyTarget").val("-1");
    }
    if ($("#StickyRB2").prop("checked") === true) {
        $("#StickyPopup_TargetsDiv").show();
        $("#PopupStickyTarget").val("");
    }
}

function Feature_Sticky_TargetChecked() {
    var res = "";
    $("#StickyPopup_TargetsDiv input:checked").each(function () {
        res += $(this)[0].id.split('_')[1] + "|";
    });

    $("#PopupStickyTarget").val(res);
}