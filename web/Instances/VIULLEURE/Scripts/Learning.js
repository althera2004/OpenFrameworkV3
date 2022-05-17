function LEARNING_CustomActions() {
    LEARNING_MetodoPagoLayout();
}

function LEARNING_MetodoPagoChanged() {
    var res = "";
    if ($("#Chk_MP_E").prop("checked") === true) { res += "E"; }
    if ($("#Chk_MP_B").prop("checked") === true) { res += "B"; }
    if ($("#Chk_MP_T").prop("checked") === true) { res += "T"; }

    $("#MetodoPago").val(res);
}

function LEARNING_MetodoPagoLayout() {
    var res = "";
    res += "<input type=\"checkbox\" id=\"Chk_MP_E\" onclick=\"LEARNING_MetodoPagoChanged();\" /> Efectiu";
    res += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"checkbox\" id=\"Chk_MP_B\" onclick=\"LEARNING_MetodoPagoChanged();\" /> Tranferència bancària";
    res += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"checkbox\" id=\"Chk_MP_T\" onclick=\"LEARNING_MetodoPagoChanged();\" /> Tarjeta dèbit";
    $("#MetodoPago").hide();
    $("#MetodoPago").after(res);

    if (HasPropertyValue(itemData.MetodoPago)) {
        if (itemData.MetodoPago.indexOf('E') !== -1) { $("#Chk_MP_E").prop("checked", true); }
        if (itemData.MetodoPago.indexOf('B') !== -1) { $("#Chk_MP_B").prop("checked", true); }
        if (itemData.MetodoPago.indexOf('T') !== -1) { $("#Chk_MP_T").prop("checked", true); }
    }
}
