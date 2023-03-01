window.onload = function () {
    if (groupData.Id > 0) {
        $("#BreadCrumbLabel").html(groupData.Name);
    }
    else {
        $("#BreadCrumbLabel").html("<i>" + Dictionary.Security_Group_NewGroupLabel + "</i>");

    }

    $("#Core").title("Aquesta opció no és modificable");
    $("#Core").disable();
    if (HasPropertyEnabled(groupData.Core)) {
        $("#Deletable").disable();
        $("#Deletable").title("Si es CORE, no es pot modificar");
    }


    $("#FormBtnCancel").on("click", function () { GoEncryptedPage("/Admin/Security/GroupList.aspx"); });
    ReplaceClick("FormBtnSave", SECURITYGROUP_Save);
    ReplaceClick("FormBtnDelete", SECURITYGROUP_Delete);
    $("#FormBtnSave").enable();
    $("#FormBtnSave").show();
    $("#FooterStatus").invisible();
    $("#logofooter").show();

    $(".membership").bootstrapDualListbox({
        "nonSelectedListLabel": "Usuaris disponibles",
        "selectedListLabel": "Usuaris seleccionats",
        //"preserveSelectionOnMove": "moved",
        "moveOnSelect": true,
        "nonSelectedFilter": "",
        "selectorMinimalHeight": 300,
        "infoText": "Mostrant tots {0}",
        "infoTextFiltered": "<span class=\"badge badge-warning\">Mostrant</span> {0} de {1}",
        "infoTextEmpty": "Cap"
    });
    $(".btn-outline-secondary").remove();
    $(".bootstrap-duallistbox-container label").css("font-weight", "bold");
}

function SECURITYGROUP_Save() {
    var membership = "";
    var membershipVal = $(".membership").val();
    var first = true;
    for (var m = 0; m < membershipVal; m++) {

        var val = membershipVal[m];

        if (typeof val !== "undefined") {

            if (first === true) {
                first = false;
            }
            else {
                membership + ",";
            }

            membership += val;
        }
    }

    var data = {
        "securityGroupId": groupData.Id,
        "name": $("#Name").val(),
        "description": $("#Description").val(),
        "membership": membership,
        "grants": SECURITYGROUP_GetGrants(),
        "applicationUserId": ApplicationUser.Id,
        "companyId": Company.Id,
        "instanceName": Instance.Name
    }

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/SecurityService.asmx/SecurityGroupUpdate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("attachs = " + msg.d + ";");
            var message = Dictionary.Security_Group_Message_SeaveSuccess.split('#').join($("#Name").val());
            Notify(message, "success");
        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}

function SECURITYGROUP_Delete() {
    if (HasPropertyEnabled(groupData.Core) || !HasPropertyEnabled(groupData.Deleteable)) {
        PopupWarning("Ande vas....");
        return false;
    }
}

function SECURITYGROUP_GetGrants() {
    var res = "";
    $.each($(".GrantRow"), function (indice, valor) {
        var id = valor.id.split('_')[1];

        res += id+".";
        res += $("#chk_" + id + "_R").prop("checked") ? "R" : "";
        res += $("#chk_" + id + "_W").prop("checked") ? "W" : "";
        res += $("#chk_" + id + "_D").prop("checked") ? "D" : "";
        res += "|";
    });

    return res;
}

function SECURITYGROUP_GrantChanged(action, itemId) {
    if (action === "R") {
        if ($("#chk_" + itemId + "_R").prop("checked") === false) {
            $("#chk_" + itemId + "_W").prop("checked", false);
            $("#chk_" + itemId + "_D").prop("checked", false);
        }
    }

    if (action === "W") {
        if ($("#chk_" + itemId + "_W").prop("checked") === true) {
            $("#chk_" + itemId + "_R").prop("checked", true);
        }

        if ($("#chk_" + itemId + "_W").prop("checked") === false) {
            $("#chk_" + itemId + "_D").prop("checked", false);
        }
    }

    if (action === "D") {
        if ($("#chk_" + itemId + "_D").prop("checked") === true) {
            $("#chk_" + itemId + "_R").prop("checked", true);
            $("#chk_" + itemId + "_W").prop("checked", true);
        }
    }
}