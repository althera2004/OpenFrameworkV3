function USERPROFILECONFIG_ChkTextChanged(id) {
    var active = $("#ChkText" + id).prop("checked") === true;
    if (active === true) {
        $("#Text" + id).enable();
    }
    else {
        $("#Text" + id).disable();
    }
}

function USERPROFILECONFIG_ChkDocChanged(id) {
    var active = $("#ChkDoc" + id).prop("checked") === true;
    if (active === true) {
        $("#Doc" + id).enable();
    }
    else {
        $("#Doc" + id).disable();
    }
}