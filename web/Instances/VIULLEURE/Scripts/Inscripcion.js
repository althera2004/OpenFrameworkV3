function INSCRIPCION_CustomActions() {
	
	if(itemData.Id > 0)
	{
		FieldToLabel("CasalId");
		FieldToLabel("Code");
        FieldToLabel("FechaSolicitud");
        $("#Code_Labeled").after("&nbsp;<a href=\"http://viulleure.cat/InscripcionStatus.aspx?" + itemData.Code + "\" target=\"_blank\">Veure inscripció</a>");
	}
	else{
		$("#CasalId").on("change", INSCRIPCION_PeriodosLayoutEditablePre);
	}
	
	if(ApplicationUser.Admin || UserInGroup(6) || UserInGroup(7))
    {
        console.log("puede editar");
	}
	else
	{
		FieldToLabel("AsistenteNombre");
		FieldToLabel("AsistenteApellidos");
		FieldToLabel("FNac");
		FieldToLabel("NecesidadesEspeciales");
		FieldToLabel("Obervaciones");
    }

    $("#CasalId").on("change", INSCRIPCION_StatusLayout);

    /*$("#Acollida").hide();
    if (HasPropertyValue(itemData.Acollida)) {
        var text = "";
        switch (itemData.Acollida) {
            case 1: text = "Matinal / dia"; break;
            case 2: text = "Matinal / set"; break;
            case 3: text = "Matinal / casal"; break;
            case 4: text = "Tarda / dia"; break;
            case 5: text = "Tarda / set"; break;
            case 6: text = "Tarda / casal"; break;
            case 7: text = "Matinal i tarda / dia"; break;
            case 8: text = "Matinal i tarda / set"; break;
            case 9: text = "Matinal i tarda / casal"; break;
        }

        $("#Acollida").after(text);
    } else {
        $("#Acollida").after("Sense acollida");
    }*/
	
	INSCRIPCION_StatusLayout();
	
	if(itemData.Id > 0){
		$("#RBStatus" + itemData.Status).prop("checked", true);
        INSCRIPCION_PeriodosLayout();
        INSCRIPCION_AcollidaLayout();
	}
	else {
		INSCRIPCION_PeriodosLayoutEditable();
	}
}

function RBStatusChanged(value){
	$("#Status").val(value);
}

function INSCRIPCION_PeriodosLayoutEditablePre() {
    itemData.CasalId = $("#CasalId").val() * 1;
    INSCRIPCION_PeriodosLayout();
}

function INSCRIPCION_StatusLayout() {
	var res = "<div id=\"RBPeriodos\"><input type=\"radio\" name=\"RBStatus\" id=\"RBStatus0\" onclick=\"RBStatusChanged(0);\" /> Rebuda&nbsp;&nbsp;&nbsp;";
	res += "<input type=\"radio\" name=\"RBStatus\" id=\"RBStatus1\" onclick=\"RBStatusChanged(1);\" /> En tràmit&nbsp;&nbsp;&nbsp;";
	res += "<input type=\"radio\" name=\"RBStatus\" id=\"RBStatus2\" onclick=\"RBStatusChanged(2);\" /> Aceptada&nbsp;&nbsp;&nbsp;";
	res += "<input type=\"radio\" name=\"RBStatus\" id=\"RBStatus3\" onclick=\"RBStatusChanged(3);\" /> Pedent pagament&nbsp;&nbsp;&nbsp;";
	res += "<input type=\"radio\" name=\"RBStatus\" id=\"RBStatus4\" onclick=\"RBStatusChanged(4);\" /> Rebutjada</div>";

    $("#RBPeriodos").remove();
	$("#Status").hide();
	$("#Status").parent().append(res);
}

function INSCRIPCION_AcollidaLayout() {
    if (HasPropertyValue(itemData.Acollida)) {
        $("#Acollida").val(itemData.Acollida);
    }
    else {
        $("#Acollida").val(0);
    }

    var res = "<div id=\"RBAcollida\">";
    res += "<input type=\"radio\" name=\"RBAcollida\" id=\"RBAcollida0\" onclick=\"RBAcollidaChanged(0);\" /> Sense acollida&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"radio\" name=\"RBAcollida\" id=\"RBAcollida1\" onclick=\"RBAcollidaChanged(1);\" /> Matinal / dia&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"radio\" name=\"RBAcollida\" id=\"RBAcollida2\" onclick=\"RBAcollidaChanged(2);\" /> Matinal / set&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"radio\" name=\"RBAcollida\" id=\"RBAcollida3\" onclick=\"RBAcollidaChanged(3);\" /> Matinal / casal&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"radio\" name=\"RBAcollida\" id=\"RBAcollida4\" onclick=\"RBAcollidaChanged(4);\" /> Tarda / dia&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"radio\" name=\"RBAcollida\" id=\"RBAcollida5\" onclick=\"RBAcollidaChanged(5);\" /> Tarda / set&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"radio\" name=\"RBAcollida\" id=\"RBAcollida6\" onclick=\"RBAcollidaChanged(6);\" /> Tarda / casal&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"radio\" name=\"RBAcollida\" id=\"RBAcollida7\" onclick=\"RBAcollidaChanged(7);\" /> Matinal i tarda / dia&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"radio\" name=\"RBAcollida\" id=\"RBAcollida8\" onclick=\"RBAcollidaChanged(8);\" /> Matinal i tarda / set&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"radio\" name=\"RBAcollida\" id=\"RBAcollida9\" onclick=\"RBAcollidaChanged(9);\" /> Matinal i tarda / casal&nbsp;&nbsp;&nbsp;";
    res += "</div>";


    $("#RBAcollida").remove();
    $("#Acollida").hide();
    $("#Acollida").parent().append(res);
    var acollidaValue = $("#Acollida").val();
    $("#RBAcollida" + acollidaValue).prop("checked", true);
}

function RBAcollidaChanged(value) {
    $("#Acollida").val(value);
}

function INSCRIPCION_PeriodosLayout(){
    console.log("INSCRIPCION_PeriodosLayout");
    $("#ChkPeriodosDiv").remove();
    var res = "<div id=\"ChkPeriodosDiv\">";
    //if (HasPropertyValue(itemData.Periodos)) {
        var data = FK.CasalPeriodo.Data.filter(function (e) { return e.CasalId === itemData.CasalId && e.Active === true; });
        var periodos = $("#Periodos").val().split('|');
        var first = true;
            for (var y = 0; y < data.length; y++) {
                var checked = "";
                for (var x = 0; x < periodos.length; x++) {
                    if (data[y].Id === periodos[x] * 1) {
                        checked = " checked=\"checked\"";
                        break;
                    }
                }
                res += first ? "" : "&nbsp;&nbsp;&nbsp;&nbsp;";
                res += "<input type=\"checkbox\" onclick=\"INSCRIPCION_PeriodosChanged();\" class=\"chkPeriodo\" id=\"chkPeriodo_" + data[y].Id + "\" " + checked + "/>&nbsp;" + data[y].Description;
                first = false;
            }
    //}

    res += "</div>";
	
	$("#Periodos").hide();
	$("#Periodos").parent().append(res);
}

function INSCRIPCION_PeriodosChanged() {
    var res = "";
    $(".chkPeriodo").each(function () {
        if ($(this)[0].checked) {
            res += "|" + $(this)[0].id.split('_')[1];
        }
    });

    $("#Periodos").val(res);
}

function INSCRIPCION_PeriodosLayoutEditable(){
	var res = "";
	var periodosCasal = $.grep(FK.CasalPeriodo.Data, function(v) {
		return v.CasalId === $("#CasalId").val() *1;
	});
	
	if(periodosCasal.length > 0)
    {
        res = "ok";
	}
	else
	{
		res = "No hi ha periodes disponibles";
	}
	
	$("#Periodos").parent().append(res);
}

function CustomAfterFillForm() {
    INSCRIPCION_PeriodosLayout();
}

function INSCRIPCION_CambiarActividad () {
	$("#PopupErrorDiv").html("");
	var casalActual = "";
	var res = "";
    res += "<div class=\"row\">";
    res += "    <label class=\"col-sm-3\">Actividad actual:</label>";
    res += "    <label class=\"col-sm-9\" id=\"CasalActual\"></label>";
    res += "  </div>";
    res += "</div>";
    res += "<div class=\"row\">";
    res += "  <label class=\"col-sm-3\">Traslladar a<span style=\"color:#f00;\">*</span>:</label>";
    res += "  <div class=\"col-sm-9\">";
    res += "    <select Id=\"CmbCasal\">";
	res += "      <option value=\"-1\">Seleccionar...</option>";
	
	for(var c = 0; c < FK.Casal.Data.length; c++) {
		var casal = FK.Casal.Data[c];
		if(casal.Id !== itemData.CasalId && casal.Active === true) {
			res +="  <option value=\""+ casal.Id+"\">"+ casal.Description+"</option>";
		}
		
		if(casal.Id === itemData.CasalId) {
			casalActual = casal.Description;
		}
	}
	
	res += "    </select>";
    res += "  </div>";
    res += "</div>";

    $("#PopupDefaultBody").html(res);

			$("#CasalActual").html(casalActual);

    PopupDefault({
        "Title": "Cambiar a otra actividad",
        "BtnOk": {
            "Text": "Confirmar",
            "Icon": "fa fa-save",
            "Click": INSCRIPCION_CambiarActividadConfirmed
        },
		"Width": 600
    });
}

function INSCRIPCION_CambiarActividadConfirmed() {
		$("#PopupErrorDiv").hide();
	var newCasalId = $("#CmbCasal").val() *1;
	
	if(newCasalId < 0) {
		$("#PopupErrorDiv").html("Hay que indicar el casal destino");
		$("#PopupErrorDiv").show();
		return false;
	}
	
	PopupWorking("Traslladant inscripció de casal");
    var data = {
        "inscripcionId": itemData.Id,
        "casalId": newCasalId,
        "applicationUserId": ApplicationUser.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/Instances/VIULLEURE/Data/ItemDatabase.aspx/INSCRIPCION_CambiarActividad",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg);
			$("#CasalId").val($("#CmbCasal").val() *1);
			itemData.CasalId = $("#CmbCasal").val() *1;
			originalItemData.CasalId = $("#CmbCasal").val() *1;
			
			for(var c = 0; c < FK.Casal.Data.length; c++) {
				if(FK.Casal.Data[c].Id === itemData.CasalId) {
					$("#CasalId_Labeled").html(FK.Casal.Data[c].Description);
					break;
				}
			}
			
			$("#PopupDefaultBtnCancel").click();
            PopupWorkingHide();
        },
        "error": function (msg) {
            PopupWorkingHide();
            PopupWarning(msg.responseText);
        }
    });
}
	