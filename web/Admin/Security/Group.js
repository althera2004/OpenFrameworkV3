window.onload = function () {

    $("#BreadCrumbLabel").html(groupData.Name);

    $("#Core").title("Aquesta opció no és modificable");
    $("#Core").disable();
    if (HasPropertyEnabled(groupData.Core)) {
        $("#Deletable").disable();
        $("#Deletable").title("Si es CORE, no es pot modificar");
    }


    $("#FormBtnCancel").on("click", function () { GoEncryptedPage('/Admin/Security/GroupList.aspx'); });
    ReplaceClick("FormBtnSave", SECURITYGROUP_Save);
    ReplaceClick("FormBtnDelete", SECURITYGROUP_Delete);
    $("#FormBtnSave").enable();
    $("#FooterStatus").invisible();
    $("#logofooter").show();

    var membership = $(".membership").bootstrapDualListbox({
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
    alert("SECURITYGROUP_Save");
}

function SECURITYGROUP_Delete() {
    if (HasPropertyEnabled(groupData.Core) || !HasPropertyEnabled(groupData.Deleteable)) {
        PopupWarning("Ande vas....");
        return false;
    }
}