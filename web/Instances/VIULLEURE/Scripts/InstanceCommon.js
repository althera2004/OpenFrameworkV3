console.log("ViuLleure", "InstanceCommon loaded");

function InstanceCommonAfterLoad (){
	console.log("InstanceCommonAfterLoad");
}

function LEARNINGASSISTANT_Signature(data, row) {
	var res = "";
	
	if(typeof data !== "undefined" && data !== null && data !== "") {
		res = "SÍ";
	}
	else{
		res = "NO";
	}
	
	return res;
}

function INSCRIPCION_Status(data, row) {
    switch (data) {
        case -1: return "Incomplerta";
        case 0: return "Rebuda";
        case 1: return "En tràmit";
        case 2: return "Aceptada";
        case 3: return "Pendent pagament";
        case 4: return "Rebutjada";
        case 5: return "LLista espera";
    }
}

function MONITOR_NombreCompleto(data, row) {
    var res = "";
    console.log(row);
    if (HasPropertyValue(row.FirstName)) {
        res = row.FirstName;
    }

    if (HasPropertyValue(row.LastName)) {
        res += " " + row.LastName;
    }

    return res;
}

function ASISTENTE_NombreCompleto(data, row) {
    var res = "";
    console.log(row);
    if (HasPropertyValue(row.AsistenteApellidos)) {
        res = row.AsistenteApellidos;
    }

    if (HasPropertyValue(row.AsistenteNombre)) {
        res += ", " + row.AsistenteNombre;
    }

    return res;
}

function INSCRIPCION_Periodos(data) {	
	var res = "";
	var first = true;
	var parts= data.split('|');
	for(var x =0; x < parts.length; x++){
		if(parts[x]!== ''){
			var periodo = GetFKValueById("CasalPeriodo", parts[x]*1);
			if(periodo !== null){
				if(first!== true){
					res += ", ";
				}
				
				res+= periodo.Code;
				
				first = false;
			}
		}
	}
	
	return res;
}

function MONITOR_ViewProfileByApoyo(sender) {
    var monitorapoyoId = sender.id.split('-')[2] * 1;
    var data = null;
    var listData = PageListById("CasalMonitorApoyo", "ByCasal").Data;
    for (var x = 0; x < listData.length; x++) {
        if (listData[x].Id === monitorapoyoId) {
            data = listData[x];
        }
    }

    console.log(data);
    MONITOR_PopupProfile(data.MonitorId.Id);
}

function MONITOR_ViewProfileByGrupo(sender) {
    var monitorapoyoId = sender.id.split('-')[2] * 1;
    var data = null;
    var listData = PageListById("CasalGrupo", "ByCasal").Data;
    for (var x = 0; x < listData.length; x++) {
        if (listData[x].Id === monitorapoyoId) {
            data = listData[x];
        }
    }

    console.log(data);
    MONITOR_PopupProfile(data.MonitorId.Id);
}

function MONITOR_PopupProfile(monitorId) {
    GetItemDataJson("Monitor", monitorId, MONITOR_PopupProfile2);
}

var monitor = null;
function MONITOR_PopupProfile2(data) {
    eval("monitor=" + data + ";");
    console.log(monitor);
    $("#PopupDefaultTitle").html("Dades del monitor");
    var res = "";
    res += "    <div class=\"col-sm-12\">";
    res += "        <div class=\"row\">";
    res += "            <label class=\"col-xs-3\">Nom</label>";
    res += "            <div class=\"col-xs-12\">";
    res += monitor.FirstName + " " + monitor.LastName;
    if (monitor.Practicas === true) { res += " <i>(en pràctiques)</i>"; }
    res += "            </div>";
    res += "        </div>";
    res += "        <div class=\"row\">";
    res += "            <label class=\"col-xs-3\">NIF</label>";
    res += "            <div class=\"col-xs-12\">";
    res += monitor.NIF;
    res += "            </div>";
    res += "        </div>";
    res += "        <div class=\"row\">";
    res += "            <label class=\"col-xs-3\">Email</label>";
    res += "            <div class=\"col-xs-12\">";
    if (monitor.Email !== null) { res += monitor.Email; } else { res += "<i style=\"color:red\">No informat</i>"; }
    res += "            </div>";
    res += "        </div>";
    res += "        <div class=\"row\">";
    res += "            <label class=\"col-xs-3\">Telèfon</label>";
    res += "            <div class=\"col-xs-12\">";
    if (monitor.Phone !== null) { res += monitor.Phone; } else { res += "<i style=\"color:red\">No informat</i>"; }
    res += "            </div>";
    res += "        </div>";
    res += "        <div class=\"row\">";
    res += "            <label class=\"col-xs-3\">Certificat antecedents</label>";
    res += "            <div class=\"col-xs-12\">";
    if (monitor.CertificacioAntecedentes !== null) { res += "<a href=\"/Instances/VIULLEURE/Data/Monitor/" + monitor.Id + "/" + monitor.CertificacioAntecedentes + "\" target=\"_blank\">" + monitor.CertificacioAntecedentes+ "</a>" } else { res += "<i style=\"color:red\">No adjuntat</i>"; }
    res += "            </div>";
    res += "        </div>";
    res += "        <div class=\"row\" id=\"PopupAdjuntos\" style=\"display:none;\">";
    res += "        <h6>Altra documentació</h6>";
    res += "        <table style=\"width:100%;\">";
    res += "        <tbody id=\"PopupAdjuntosList\"></tbody>";
    res += "        </table>";
    res += "        </div>";
    res += "</div>";

    $("#PopupDefaultBody").html(res);
    $("#PopupDefaultBtnOk").hide();
    $("#PopupDefaultBtnDelete").hide();

    $("#LauncherPopupDefault").click();

    MONITOR_GetAttachs(monitor.Id);
}

function MONITOR_GetAttachs(monitorId) {
    var data = {
        "itemName": "Monitor",
        "itemId": monitorId
    };
    $.ajax({
        "type": "POST",
        "url": "/Async/ItemDataServices.asmx/ItemAttachs",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            var attachs = [];
            eval("attachs = " + msg.d + ";");
            var list = "";

            if (attachs.length > 0) {

                for (var x = 0; x < attachs.length; x++) {
                    var file = attachs[x];
                    console.log(file);
                    list += "<tr>";
                    list += "  <td id=\"TD-File-0\" title=\"" + file.Name + "\">";
                    list += "    <div class=\"truncate\" style=\"width:400px;\">" + file.Name +"</div>";
                    list += "  </td>";
                    list += "   <td style=\"width: 120px;text-align:right;\">" + FormatBytes(file.Bytes, 3) + "&nbsp;</td>";
                    list += "              <td style=\"width: 120px;text-align:center;\">" + file.CreatedOn + "</td>";
                    list += "              <td class=\"action-buttons\" style=\"width: 111px;\">";
                    list += "                  <a class=\"blue ace-icon fal fa-eye bigger-120\" id=\"" + file.Id + "\" title=\"" + Dictionary.Common_Upload_Tooltip_View + "\" onclick=\"LaunchDocumentLink('/Instances/ViuLleure/Data/DocumentsGallery/Monitor/" + monitor.Id + "/" + file.Name + "','Adjunto');\"></a>";
                    list += "              </td>";
                    list += "</tr>";
                }

                $("#PopupAdjuntosList").html(list);
                $("#PopupAdjuntos").show();
            }
            else {
                $("#PopupAdjuntosList").html("");
                $("#PopupAdjuntos").hide();
            }
        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}