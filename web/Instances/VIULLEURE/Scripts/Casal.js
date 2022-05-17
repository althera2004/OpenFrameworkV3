var monitoresOficiales = [];
var dragItems = [];
var dropAreas = [];
var originId = null;
var targetId = null;

function CustomAfterFillForm() {
	CASA_MetodoPagoShowData();
	CASAL_DocumentacioRequeridaShowData();
}

function FieldSetUnsignable() { }

function CASAL_MetodoPagoChanged() {
    var res = "";
    if ($("#Chk_MP_E").prop("checked") === true) { res += "E"; }
    if ($("#Chk_MP_B").prop("checked") === true) { res += "B"; }
    if ($("#Chk_MP_T").prop("checked") === true) { res += "T"; }
    $("#MetodoPago").val(res);
}

function CASA_MetodoPagoShowData() {	
    if (HasPropertyValue(itemData.MetodoPago)) {
        if (itemData.MetodoPago.indexOf('E') !== -1) { $("#Chk_MP_E").prop("checked", true); }
        if (itemData.MetodoPago.indexOf('B') !== -1) { $("#Chk_MP_B").prop("checked", true); }
        if (itemData.MetodoPago.indexOf('T') !== -1) { $("#Chk_MP_T").prop("checked", true); }
    }
}

function CASAL_MetodoPagoLayout() {
    var res = "";
    res += "<input type=\"checkbox\" id=\"Chk_MP_E\" onclick=\"CASAL_MetodoPagoChanged();\" /> Efectiu";
    res += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"checkbox\" id=\"Chk_MP_B\" onclick=\"CASAL_MetodoPagoChanged();\" /> Tranferència bancària";
    res += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"checkbox\" id=\"Chk_MP_T\" onclick=\"CASAL_MetodoPagoChanged();\" /> Tarjeta dèbit";
    $("#MetodoPago").hide();
    $("#MetodoPago").after(res);
	CASA_MetodoPagoShowData();
}

function CASAL_DocumentacioRequeridaChanged() {
    var res = "";
    if ($("#Chk_DR_0").prop("checked") === true) { res += "1"; } else { res += "0"; }
    if ($("#Chk_DR_1").prop("checked") === true) { res += "1"; } else { res += "0"; }
    if ($("#Chk_DR_2").prop("checked") === true) { res += "1"; } else { res += "0"; }
    if ($("#Chk_DR_3").prop("checked") === true) { res += "1"; } else { res += "0"; }
    if ($("#Chk_DR_4").prop("checked") === true) { res += "1"; } else { res += "0"; }
    $("#DocRequerida").val(res);
}

function CASAL_DocumentacioRequeridaShowData() {
    if (HasPropertyValue(itemData.DocRequerida)) {
        if (itemData.DocRequerida.charAt(0) === '1') { $("#Chk_DR_0").prop("checked", true); }
        if (itemData.DocRequerida.charAt(1) === '1') { $("#Chk_DR_1").prop("checked", true); }
        if (itemData.DocRequerida.charAt(2) === '1') { $("#Chk_DR_2").prop("checked", true); }
        if (itemData.DocRequerida.charAt(3) === '1') { $("#Chk_DR_3").prop("checked", true); }
        if (itemData.DocRequerida.charAt(4) === '1') { $("#Chk_DR_4").prop("checked", true); }
    }
}

function CASAL_DocumentacioRequeridaLayout() {
    var res = "";
    res += "<input type=\"checkbox\" id=\"Chk_DR_0\" onclick=\"CASAL_DocumentacioRequeridaChanged();\" /> DNI Tutor";
    res += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"checkbox\" id=\"Chk_DR_1\" onclick=\"CASAL_DocumentacioRequeridaChanged();\" /> Llibre familia";
    res += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"checkbox\" id=\"Chk_DR_2\" onclick=\"CASAL_DocumentacioRequeridaChanged();\" /> Tarjeta sanitària";
    res += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"checkbox\" id=\"Chk_DR_3\" onclick=\"CASAL_DocumentacioRequeridaChanged();\" /> Vacunes";
    res += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
    res += "<input type=\"checkbox\" id=\"Chk_DR_4\" onclick=\"CASAL_DocumentacioRequeridaChanged();\" /> DGJ";
    $("#DocRequerida").hide();
    $("#DocRequerida").after(res);
	CASAL_DocumentacioRequeridaShowData();
}

function CASAL_CustomActions() {
    $("#PlaceHolder_Acollida").html("<div class=\"col-xs-2\">&nbsp;</div><div class=\"col-xs-3\" style=\"text-align:center;font-weight:bold;font-size:16px;\">Dia</div><div class=\"col-xs-3\" style=\"text-align:center;font-weight:bold;font-size:16px;\">Set</div><div class=\"col-xs-3\" style=\"text-align:center;font-weight:bold;font-size:16px;\">Casal</div>");
    $("#PlaceHolder_Acollida").show();
    $("#Acollida1Label").html("Matinal");
    $("#Acollida1Label").removeClass("col-xs-1");
    $("#Acollida1Label").addClass("col-xs-2");
    $("#Acollida2Label").remove();
    $("#Acollida3Label").remove();
    $("#Acollida4Label").html("Tarda");
    $("#Acollida4Label").removeClass("col-xs-1");
    $("#Acollida4Label").addClass("col-xs-2");
    $("#Acollida5Label").remove();
    $("#Acollida6Label").remove();
    $("#Acollida7Label").html("Matinal i tarda");
    $("#Acollida7Label").removeClass("col-xs-1");
    $("#Acollida7Label").addClass("col-xs-2");
    $("#Acollida8Label").remove();
    $("#Acollida9Label").remove();
	
	monitoresOficiales = FK.Monitor.Data.filter(function (el) { return el.Practicas === false; });
	
	if(itemData.Id > 0){
        CASAL_AsginacionRender();
	}
	else{
		$("#TabAssignacioGrupos").hide();
    }

    CASAL_MetodoPagoLayout();
	CASAL_DocumentacioRequeridaLayout();
}

function PopupItemCallback_CasalMonitorApoyo_ByCasal() {
	var disponibles = FK.Monitor.Data.filter( function(el) { return MONITOR_IsDisponible(el)} );
	
	if(disponibles.length > 0)
	{	
		for(var x=0; x< disponibles.length; x++)
		{
			res += "<option value=\""+ disponibles[x].Id+"\">"+ disponibles[x].Description+"</option>";
		}
	}
	else{
		$("#PopupItemBtnCancel").click();
		PopupWarning("No hi ha monitors disponibles per a aquesta opció", Dictionary.Common_Warning);
	}
	
	$("#PopupItem #MonitorId").html(res);
}

function PopupItemCallback_CasalGrupo_ByCasal() {
	var res = "<option value=\"-1\">"+ Dictionary.Common_SelectOne+"</option>";
    var disponibles = monitoresOficiales;//.filter( function(el){ return MONITOR_IsDisponible(el)});
	
	if(disponibles.length > 0)
	{	
		for(var x=0; x< disponibles.length; x++)
		{
			res += "<option value=\""+ disponibles[x].Id+"\">"+ disponibles[x].Description+"</option>";
		}
	}
	else{
		$("#PopupItemBtnCancel").click();
		PopupWarning("No hi ha monitors disponibles per a aquesta opció", Dictionary.Common_Warning);
	}

    var periodos = PageListById("CasalPeriodo", "ByCasal").Data;
    var resPeriodos = "<option value=\"-1\">" + Dictionary.Common_SelectOne + "</option>";
    for (var p = 0; p < periodos.length; p++) {
        resPeriodos += "<option value=\"" + periodos[p].Id + "\">" + periodos[p].Code + " - " + periodos[p].Name + "</option>";
    }

    $("#PopupItem #CasalPeriodoId").html(resPeriodos);
	$("#PopupItem #MonitorId").html(res);
}

function MONITOR_IsDisponible(data){
	console.log(data);
	var listData1 = PageListById("CasalGrupo", "ByCasal").Data;
	for(var x1=0; x1 < listData1.length; x1++){
		if(listData1[x1].MonitorId.Id === data.Id){
			return false;
		}
	}
	
	var listData2 = PageListById("CasalMonitorApoyo", "ByCasal").Data;
	for(var x2=0; x2 < listData2.length; x2++){
		if(listData2[x2].MonitorId.Id === data.Id){
			return false;
		}
	}
	
	return true;
}

var CASAL_AsginacionItems = 0;
function CasalGrupo_ByCasal_Callback() {
    CASAL_AsginacionItems++;
    CASAL_AsginacionRender();
}

function Inscripcion_ByCasal_Callback() {
    CASAL_AsginacionItems++;
    CASAL_AsginacionRender();
}

function CASAL_AsginacionRender() {

    if (CASAL_AsginacionItems < 2) {
        $("#PlaceHolder_Asignaciones").html("<h6>Obtenint dades de asistents</h6>");
        $("#PlaceHolder_Asignaciones").show();
        return;
    }

    
	
    var res = "<div class=\"row\">";
    res += "<label class=\"col-xs-2\">Seleccionar torn:</label>";
    res += "<div class=\"col-xs-4\">";
    res += "<select id=\"AsignacionPeriodo\" onchange=\"AsignacionPeriodo_Changed();\">";
    res += "<option value=\"-1\">" + Dictionary.Common_SelectOne + "</option>";
    var periodos = PageListById("CasalPeriodo", "ByCasal").Data;
    var resPeriodos = "<option value=\"-1\">" + Dictionary.Common_SelectOne + "</option>";
    for (var p = 0; p < periodos.length; p++) {
        res += "<option value=\"" + periodos[p].Id + "\">" + periodos[p].Code + " - " + periodos[p].Name + "</option>";
    }
    res += "</select>";
    res += "</div>";
    res += "<div id=\"AsignacionPeriodoError\" class=\"col-xs-6\"><i class=\"fa fa-exclamation-triangle red\"></i>&nbsp;S'ha de seleccionar un torn</div>";
    res += "<div id=\"AsignacionPeriodoImprimir\" style=\"display:none;\"><span style=\"cursor:pointer;\" class=\"col-xs-6 blue\" onclick=\"CASALPERIODO_Export();\"><i class=\"fa fa-print blue\"></i>&nbsp;Imprimir torn</span></div>";
    res += "</div>";
    res += "<div class=\"row\">";
    res += "</div>";
	res += "<div class=\"col-xs-6\" id=\"AsistentesDisponibles\"></div>"
	res += "<div class=\"col-xs-6\" id=\"GruposDisponibles\"></div>";
	
	
	res += "</div>";
	
	
	$("#PlaceHolder_Asignaciones").html(res);
    $("#PlaceHolder_Asignaciones").show();

}

function AsignacionPeriodo_Changed() {
    $("#AsignacionPeriodoError").hide();
    $("#AsignacionPeriodoImprimir").hide();
    var periodoId = $("#AsignacionPeriodo").val() * 1;

    if (periodoId < 0) {
        $("#AsignacionPeriodoError").show();
        $("#AsistentesDisponibles").html("");
        $("#GruposDisponibles").html("");

        return;
    }


    $("#AsignacionPeriodoImprimir").show();
    var listDataAsistentes = PageListById("Inscripcion", "ByCasal").Data;
    var asistentesList = "";
    for (var a = 0; a < listDataAsistentes.length; a++) {
        var asistente = listDataAsistentes[a];
        //if (asistente.Status !== 2 && asistente.Status !== 3) {
        //    continue;
        //}
        console.log(asistente);

        if (AsistenteInPeriodo(asistente.Periodos, periodoId)) {
            asistentesList += "<span class=\"AsistenteDiv\" draggable=\"true\" id=\"A_" + asistente.Id + "\">" + asistente.AsistenteApellidos + ", " + asistente.AsistenteNombre + " - " + asistente.Curso + "</span>";
        }
    }

    var listDataGrupos = PageListById("CasalGrupo", "ByCasal").Data;
    var gruposLists = "";
    if (listDataGrupos.length > 0) {
        for (var g = 0; g < listDataGrupos.length; g++) {
            var grupo = listDataGrupos[g];
            console.log(grupo);
            if (grupo.CasalPeriodoId.Id !== periodoId) {
                continue;
            }

            gruposLists += "<div>";
            gruposLists += "<input style=\"display:none;\" class=\"AsignadosData\" id=\"GA_" + grupo.Id + "\" value=\"" + (grupo.Asistentes.split('|').join('|A_').split(' ').join('')) + "\" />";
            gruposLists += "<h6>" + grupo.Name + " - " + grupo.MonitorId.Value;
            gruposLists += "&nbsp;<i class=\"fa fa-chevron-double-down toExpand grey\" onclick=\"GrupoToogle(this," + grupo.Id + ");\"></i>";
            gruposLists += "</h6>";
            gruposLists += "<div class=\"grupoDataDiv\" id=\"G_" + grupo.Id + "\" style=\"display:none;border:1px solid #aae;min-height:400px;\" droppable=\"true\" class=\"col-xs-12\"></div>";
            gruposLists += "</div>";
        }
    }
    else {
        gruposLists = "<span style=\"color:#f00;font-size:16px;\"><i class=\"fa fa-exclamation-triangle\"></i> Encara falta definir els grups del casal.</span>";
    }

    $("#AsistentesDisponibles").html("<h4>Asistents disponibles</h4><div id=\"AsistentesDisponiblesData\" droppable=\"true\" style=\"border:1px solid #aae;min-height:400px;\">" + asistentesList + "</div>");
    $("#GruposDisponibles").html("<h4>Grups disponibles</h4>" + gruposLists);

    // traspasar asistentes asignados
    // -----------------------------------------
    $(".AsignadosData").each(function () {
        var GAId = $(this)[0].id;
        var value = $("#" + GAId).val();
        console.log(GAId, value);
        if (value !== "") {
            var GAParts = value.split('|');
            for (var ga = 0; ga < GAParts.length; ga++) {
                if (GAParts[ga] === "") { continue; }
                $("#G_" + GAId.split('_')[1]).append(document.getElementById(GAParts[ga].trim()));
            }
        }
    });
    // -----------------------------------------

    dropAreas = document.querySelectorAll("[droppable=true]");
    dragItems = document.querySelectorAll("[draggable=true]");
    $(".AsistenteDiv").css("border", "1px solid #a7a7e6");
    $(".AsistenteDiv").css("backgroundColor", "#e5e5f5");
    $(".AsistenteDiv").css("padding", "4px");
    $(".AsistenteDiv").css("margin", "4px");
    $(".AsistenteDiv").css("float", "left");
    $(".AsistenteDiv").css("width", "40%");
    ASSIGNATION_ConfigDrag();
}

function GetAsistenteById(id) {
    var listDataAsistentes = PageListById("Inscripcion", "ByCasal").Data;
    for (var x = 0; x < listDataAsistentes.length; x++) {
        if (listDataAsistentes[x].Id === id) {
            return listDataAsistentes[x];
        }
    }

    return null;
}

function ASSIGNATION_ConfigDrag() {
    for (var i = 0; i < dragItems.length; i++) {
        addEvent(dragItems[i], "dragstart", function (event) {
            event.dataTransfer.setData('obj_id', this.id);
            originId = this.parentNode.id;
            selectedId = this.id;
            var asistente = GetAsistenteById(selectedId.split("_")[1] * 1);
            console.log("ItemId", asistente.AsistenteNombre + " " + asistente.AsistenteApellidos);
            console.log("OriginId", originId);
            return false;
        });
    }


    // dragover event handler
    addEvent(dropAreas, "dragover", function (event) {
        if (event.preventDefault) event.preventDefault();
        return false;
    });

    // dragleave event handler
    addEvent(dropAreas, "dragleave", function (event) {
        if (event.preventDefault) event.preventDefault();
        //dragCounter--;
        // if (dragCounter === 0) {
        this.style.borderColor = "#ccc";
        this.style.backgroundColor = "transparent";
        //}
        return false;
    });

    // dragenter event handler
    addEvent(dropAreas, "dragenter", function (event) {
        if (event.preventDefault) event.preventDefault();
        //dragCounter++;
        targetId = this.id;
        console.log("TargetId", targetId);
        this.style.borderColor = "#000";
        this.style.backgroundColor = "#dfd";
        return false;
    });

    // drop event handler
    addEvent(dropAreas, "drop", function (event) {
        if (event.preventDefault) event.preventDefault();

        setTimeout(function () {
            console.log("originId", originId);
            console.log("targetId", targetId);
            console.log("selectedId", selectedId);

            // El destino es igual que el origen
            if (originId === targetId) {
                console.log("No hay cambio");
                //return;
            }

            console.log("cambiar de " + originId + " a " + targetId);

            var grupoId = "";

            if (targetId[0] === "G") {
                console.log("Asignar a grupo");
                grupoId = targetId.split('_')[1];
                var actual = $("#GA_" + targetId.split('_')[1]).val().trim();
                $("#GA_" + targetId.split('_')[1]).val(actual +"|" + selectedId);
            }
            else {
                console.log("Quitar de grupo");
                grupoId = originId.split('_')[1];
                var oldData = $("#GA_" + originId.split('_')[1]).val().split('|');
                var newData = "";
                for (var x = 0; x < oldData.length; x++) {
                    if (oldData[x] !== "") {
                        if (oldData[x] !== selectedId) {
                            newData += "|" + oldData[x];
                        }
                    }
                }
                
                $("#GA_" + originId.split('_')[1]).val(newData);
            }

            $("#" + targetId).append(document.getElementById(selectedId));
            $("#" + targetId).css("background-color", "#fff");


            var sendData = {
                "grupoCasalId": grupoId,
                "asistentes": $("#GA_" + grupoId).val(),
                "companyId": CompanyId
            };

            console.log(sendData);

            $.ajax({
                "type": "POST",
                "url": "/Instances/VIULLEURE/Data/ItemDataBase.aspx/CasalGrupo_SetAsignacion",
                "contentType": "application/json; charset=utf-8",
                "dataType": "json",
                "data": JSON.stringify(sendData, null, 2),
                "success": function (msg) {
                    console.log(msg);

                    //Actualizar lista de grupos
                    var listDataGrupos = PageListById("CasalGrupo", "ByCasal").Data;
                    for (var g1 = 0; g1 < listDataGrupos.length; g1++) {
                        if (listDataGrupos[g1].Id === grupoId * 1) {
                            PageListById("CasalGrupo", "ByCasal").Data[g1].Asistentes = $("#GA_" + grupoId).val().split('|A_').join('|');
                        }
                    }
                },
                "error": function (msg) {
                    PopupWarning(msg.responseText);
                }
            });

        }, 500);

        return false;
    });
}

var addEvent = (function () {
    if (document.addEventListener) {
        console.log("opcion1");
        return function (el, type, fn) {
            if (el && el.nodeName || el === window) {
                el.removeEventListener(type, fn, false);
                el.addEventListener(type, fn, false);
            } else if (el && el.length) {
                for (var i = 0; i < el.length; i++) {
                    addEvent(el[i], type, fn);
                }
            }
        };
    } else {
        return function (el, type, fn) {
            if (el && el.nodeName || el === window) {
                el.attachEvent('on' + type, function () { return fn.call(el, window.event); });
            } else if (el && el.length) {
                for (var i = 0; i < el.length; i++) {
                    addEvent(el[i], type, fn);
                }
            }
        };
    }
})();

function AsistenteInPeriodo(periodos, periodoId) {
    if (periodos === "") {
        return false;
    }

    var parts = periodos.split('|');
    for (var x = 0; x < parts.length; x++) {
        if (parts[x] === "") {
            continue;
        }

        if (parts[x] * 1 === periodoId) {
            return true;
        }
    }

    return false;
}

function GrupoToogle(sender, grupoId) {
    $(".grupoDataDiv").hide();
    $("#G_" + grupoId).show();
}

function CASALPERIODO_Export() {
    var data = {
        "casalPeriodoId": $("#AsignacionPeriodo").val() * 1
    };

    PopupWorking("Imprimint torn");

    $.ajax({
        "type": "POST",
        "url": "/Instances/ViuLleure/Export/ExportCasalPeriodo.aspx/Generate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg);
            window.open(msg.d);
            PopupWorkingHide();
        },
        "error": function (msg) {
            PopupWorkingHide();
            PopupWarning(msg.responseText);
        }
    });
}

function INSCRIPCION_FilterList() {
    $("#PopupDefaultTitle").html("Llistats personalitzats");

    var res = "";
    res += "<br />";
    res += "<table style=\"width:100%;\">";
    res += "<tr><td colspan=\"6\"><strong style=\"font-size:18px;\">Dades a mostrat</strong></td></tr>";
    res += "<tr>";
    res += "<td><input type=\"checkbox\" class=\"FilterField\" id=\"FF_1\" />&nbsp;Curs</td>";
    res += "<td><input type=\"checkbox\" class=\"FilterField\" id=\"FF_2\" />&nbsp;Tutor</td>";
    res += "<td><input type=\"checkbox\" class=\"FilterField\" id=\"FF_11\" />&nbsp;Adreça</td>";
    res += "<td><input type=\"checkbox\" class=\"FilterField\" id=\"FF_12\" />&nbsp;Autoritzacions</td>";
    res += "<td><input type=\"checkbox\" class=\"FilterField\" id=\"FF_3\" />&nbsp;Periode</td>";
    res += "<td><input type=\"checkbox\" class=\"FilterField\" id=\"FF_4\" />&nbsp;Observacions</td>";
    res += "</tr><tr>";
    res += "<td><input type=\"checkbox\" class=\"FilterField\" id=\"FF_5\" />&nbsp;Alergies</td>";
    res += "<td><input type=\"checkbox\" class=\"FilterField\" id=\"FF_6\" />&nbsp;Medicació</td>";
    res += "<td><input type=\"checkbox\" class=\"FilterField\" id=\"FF_7\" />&nbsp;Dieta</td>";
    res += "<td><input type=\"checkbox\" class=\"FilterField\" id=\"FF_8\" />&nbsp;Cansanci</td>";
    res += "<td><input type=\"checkbox\" class=\"FilterField\" id=\"FF_9\" />&nbsp;Insomni</td>";
    res += "<td><input type=\"checkbox\" class=\"FilterField\" id=\"FF_10\" />&nbsp;Enuresi</td>";
    res += "</tr><tr>";
    res += "<td colspan=\"3\"><input type=\"checkbox\" class=\"FilterField\" id=\"FF_13\" />&nbsp;Necessitats especials</td>";
    res += "<td colspan=\"3\"><input type=\"checkbox\" class=\"FilterField\" id=\"FF_14\" />&nbsp;Municipi</td>";
    res += "</tr>";
    res += "</table>";
    res += "<br />";
    res += "<table style=\"width:100%;\">";
    res += "<tr><td colspan=\"3\"><strong style=\"font-size:18px;\">Criteri de resultats</strong></td></tr>";
    res += "<tr><td colspan=\"3\"><strong>Segons periodes d'assistència</strong> <i>(si no es selecciona cap sortiran tots)</i></td></tr>";
    res += "<tr><td colspan=\"3\">";

    var periodos = PageListById("CasalPeriodo", "ByCasal").Data;
    var resPeriodos = "";
    for (var p = 0; p < periodos.length; p++) {
        resPeriodos += "<div style=\"float:left;margin-right:16px;\"><input type=\"checkbox\" class=\"fp\" id=\"FP_" + periodos[p].Id + "\">&nbsp;" + periodos[p].Code + " - " + periodos[p].Name + "</div>";
    }

    res += resPeriodos;

    res += "</td></tr>";
    res += "<tr><td>&nbsp;</td></tr>";
    res += "<tr><td colspan=\"3\"><strong>Assistents que compleixin les següents condicions</strong></td><tr>";
    res += "<td><input type=\"checkbox\" class=\"FilterCriteria\" id=\"FC_1\" />&nbsp;Amb alergies</td>";
    res += "<td><input type=\"checkbox\" class=\"FilterCriteria\" id=\"FC_2\" />&nbsp;Amb medicació</td>";
    res += "<td><input type=\"checkbox\" class=\"FilterCriteria\" id=\"FC_3\" />&nbsp;Amb dieta</td>";
    res += "</tr><tr>";
    res += "<td><input type=\"checkbox\" class=\"FilterCriteria\" id=\"FC_4\" />&nbsp;Amb cansanci</td>";
    res += "<td><input type=\"checkbox\" class=\"FilterCriteria\" id=\"FC_5\" />&nbsp;Amb insomni</td>";
    res += "<td><input type=\"checkbox\" class=\"FilterCriteria\" id=\"FC_6\" />&nbsp;Amb enuresi</td>";
    res += "</tr><tr>";
    res += "<td colspan=\"3\"><input type=\"checkbox\" class=\"FilterCriteria\" id=\"FC_7\" />&nbsp;Amb necessitats especials</td>";
    res += "</tr>";
    res += "</table>";

    var resButton = "<button class=\"btn btn-sm btn-success\" id=\"INSCRIPCIONES_BtnPrint\" onclick=\"INSCRIPCION_PrintFilter();\"><i class=\"ace-icon fa fa-print\"></i>&nbsp;Imprimir</button >";
    $("#INSCRIPCIONES_BtnPrint").remove();
    $("#PopupDefaultBody").html(res);
    $("#PopupDefaultBtnOk").before(resButton);
    $("#PopupDefaultBtnDelete").hide();
    $("#PopupDefaultBtnOk").hide();
    $("#SelectedLabel").remove();
    $("#LauncherPopupDefault").click();
}

function INSCRIPCION_GetMails() {
    $("#PopupDefaultTitle").html("Mails dels tutors");
    var res = "";

    res += "<row>";
    res += "<label class=\"col-xs-12\">Mails dels tutors</label>";
    res += "<div class=\"col-xs-12\">";
    res += "<textarea rows=\"6\" class=\"col-xs-12\">";
    var listDataAsistentes = PageListById("Inscripcion", "ByCasal").Data;
    for (var x = 0; x < listDataAsistentes.length; x++) {
        console.log(listDataAsistentes[x]);
        res += listDataAsistentes[x].TurorEmail + "; ";
    }

    res += "</textarea>";
    res += "</div>";
    res += "</row>";

    $("#PopupDefaultBody").html(res);
    $("#PopupDefaultBtnOk").hide();
    $("#PopupDefaultBtnDelete").hide();
    $("#SelectedLabel").remove();
    $("#LauncherPopupDefault").click();
}

function INSCRIPCION_PrintFilter() {
    var ff = "";
    var fp = "";
    var fc = "";

    $(".FilterField").each(function () {
        if ($(this).prop("checked") === true) {
            ff += $(this)[0].id.split('_')[1] + "|";
        }
    });

    $(".fp").each(function () {
        if ($(this).prop("checked") === true) {
            fp += $(this)[0].id.split('_')[1] + "|";
        }
    });

    $(".FilterCriteria").each(function () {
        if ($(this).prop("checked") === true) {
            fc += $(this)[0].id.split('_')[1] + "|";
        }
    });

    var data = {
        "casalId": itemData.Id,
        "ff": "|" + ff,
        "fp": fp,
        "fc": fc,
        "companyId": CompanyId
    };

    console.log(data); PopupWorking("Imprimint llistat");
    $("#PopupDefaultBtnCancel").click();

    $.ajax({
        "type": "POST",
        "url": "/Instances/ViuLleure/Export/ExportInscripcionListado.aspx/Generate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg);
            window.open(msg.d);
            PopupWorkingHide();
        },
        "error": function (msg) {
            PopupWorkingHide();
            PopupWarning(msg.responseText);
        }
    });
}

function INSCRIPCION_ListAcollida() {
    var data = {
        "casalId": itemData.Id,
        "companyId": CompanyId
    };

    console.log(data); PopupWorking("Imprimint llistat");
    $("#PopupDefaultBtnCancel").click();

    $.ajax({
        "type": "POST",
        "url": "/Instances/ViuLleure/Export/ExportInscripcionAcollida.aspx/Generate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg);
            window.open(msg.d);
            PopupWorkingHide();
        },
        "error": function (msg) {
            PopupWorkingHide();
            PopupWarning(msg.responseText);
        }
    });
}