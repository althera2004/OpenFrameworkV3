var groupsLabels = {
    "Metge": 4,
    "Administració": 5,
    "Direcció": 6,
    "Coordinació": 7,
    "Treball social": 8,
    "Administratiu": 9,
    "Fisiotrapeuta": 10,
    "Infermeria": 11,
    "Terapia ocupacional": 12,
    "Educador social": 13,
    "Psicòleg": 14,
    "Auxiliar": 16
};

var groups = {
    "Medicos": 4,
    "Administracion": 5,
    "Gerencia": 6,
    "Coordinador": 7,
    "TreballSocial": 8,
    "Administrativos": 9,
    "Fisio": 10,
    "Enfermeria": 11,
    "TerapiaOcupacional": 12,
    "EducadorSocial": 13,
    "Psicologo": 14,
    "Auxiliar": 16
};

var listados = {
    "UsuariosCentro": 1,
    "Diabetes": 2,
    "Anticoagulantes": 3,
    "Antinafres": 4,
    "RiscCaigudes": 5,
    "Incontinencias": 6,
    "DeterioroCognitivo": 7,
    "Alergias": 8,
    "Dietas": 9,
    "Contenciones": 10,
    "7Horas": 11,
    "GuardaDeFet": 12,
    "SondaVesical": 13,
    "Incapacitacion": 14,
    "Disfagia": 15,
    "Oxigenoterapia": 16,
    "Grado": 17,
    "Curas": 18,
    "UPP": 19,
    "Medicacion": 20,
    "PreciosSC": 21,
    "Cuotas": 22,
    "Cumpleanos": 23,
    "RegistroCaidas": 24,
    "IMC": 25,
    "VisitasMedicas": 26,
    "AltasBajas": 27,
    "UsuariosOut": 28,
    "MedicacionZona": 29
};

var RIEvolucion = {
    "Mantenir": 1,
    "Replantejar": 2,
    "Resolucio": 3,
    "Desestimar": 4
}

var RIEvaluacio = {
    "Resolució": 1,
    "Guariment": 2,
    "Millora": 3,
    "Sense canvis": 4,
    "Empijorament": 5
}

var imc_umbral = { "max": 25, "min": 18.5 };

function InstanceCommonAfterLoad() {
    console.log("InstanceCommonAfterLoad");

    if (CompanyId < 8 && CompanyId !== 1) {
        $('*[data-optionId="30000"]').remove();
    }

    if (UserInGroup(groups.Coordinador) || UserInGroup(groups.Gerencia) || UserInGroup(groups.Administracion) || ApplicationUser.Admin) {
        var option = "<li class=\"hightlight\" \"data-itemname\"=\"Residente\" data-optionid=\"-2\">";
        option += "<a href=\"#\" lang=\"ca-es\" onclick=\"Go('/Instances/Genogues/Pages/SeguimentsCoordinadora.aspx?YWM9MSZvsdfpb25JZD0w');\">";
        option += "        <i class=\"menu-icon far fa-sticky-note\"></i>&nbsp;<span>Seguiments / constants</span>";
        option += "</a><b class=\"arrow\"></b></li>";

        var optionUser = $('*[data-optionid="2"]');
        optionUser.after(option);
    }
    else if(UserInGroup(groups.Auxiliar)){
        var optionAuxiliar = "<li class=\"hightlight\" \"data-itemname\"=\"Residente\" data-optionid=\"-2\">";
        optionAuxiliar += "<a href=\"#\" lang=\"ca-es\" onclick=\"Go('/Instances/Genogues/Pages/SeguimentsCoordinadora.aspx?YWM9MSZvsdfpb25JZD0w');\">";
        optionAuxiliar += "        <i class=\"menu-icon far fa-sticky-note\"></i>&nbsp;<span>Seguiments dels usuaris</span>";
        optionAuxiliar += "</a><b class=\"arrow\"></b></li>";

        $(".nav-list").append(optionAuxiliar);
    }

    if (UserInGroup(groups.Administracion)) {
        var option = "<li class=\"hightlight\" \"data-itemname\"=\"Core_Group\" data-optionid=\"1002\">";
        option += "<a href=\"#\" lang=\"ca-es\" onclick=\"Go('/Admin/SecurityGroupList.aspx');\">";
        option += "        <i class=\"menu-icon far fa-sticky-note\"></i>&nbsp;<span>" + Dictionary.Core_SecurityGroup_MenuLabel + "</span>";
        option += "</a><b class=\"arrow\"></b></li>";

        var optionUser = $($('*[data-optionid="1001"]')[1]);
        optionUser.after(option);
    }

    /*if (pageType !== "userBlocked") {
        var resHelp = "";
        resHelp += "<li class=\"blue dropdown-modal\">";
        resHelp += "<a data-toggle=\"dropdown\" class=\"dropdown-toggle\" href=\"#\" id=\"MenuChangeModeButton\" onclick=\"document.location='/instances/Genogues/Pages/VideoTutorial.aspx';\"><i class=\"ace-icon fa fa-question-circle\"></i>&nbsp;<span class=\"hidden-1000\">Ajuda</span></a>";
        resHelp += "</li>";
        $(".ace-nav").prepend(resHelp);
    }*/

    // Alerts
    $("#ListDataTableHeaderAlerts_Custom #th0").html("Professional");

    var SOSButton = "<button type=\"button\" class=\"btn btn-danger\" style=\"width:90%;\" onclick=\"PopupUrgentLaunch();\"><i class=\"fa fa-exclamation-circle\"></i>&nbsp;Urgent</button>";
    SOSButton += "<a href=\"#PopupUrgent\" role=\"button\" data-toggle=\"modal\" id=\"LauncherPopupUrgent\" style=\"display:none;\"> Urgent </a>";
    $("#sidebar-shortcuts").html(SOSButton);

    ListsMustUpdate();
}

var ListsToPrint = [];
function ListsMustUpdate() {
    $.ajax({
        "type": "POST",
        "url": "/Instances/GENOGUES/Data/ItemDataBase.aspx/ListMustUpdate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify({ "companyId": CompanyId }, null, 2),
        "success": function (msg) {
            ListsToPrint = eval(msg.d);
            console.log(ListsToPrint);

            // Pregutna si es > 1 porque el primero siempre es "0"
            if (ListsToPrint.length > 1) {
                $('*[data-optionId="11"] a').append("&nbsp;<span class=\"fa fa-circle\" style=\"font-size:10px; color: #D15B47!important\" title=\"Hi ha llistats que s'han de tornar a imprimir\"></i>");
                if (typeof listPage !== "undefined" && listPage === true) {
                    LISTADO_ShowAlerts();
                }
            }

        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}

function COREGROUP_Label(data, row) {
    var res = Dictionary["Core_SecurityGroup_Name_" + data.Id];

    if (typeof res === undefined) {
        res = "-";
    }

    console.log(data);
    return res;
}

function RESIDENTEMALATIA_Actual(data, row) {
    var actual = data === true;
    var title = actual ? "Sí" : "No";
    var icon = actual ? "-check" : "";
    var cssclass = actual ? "" : " Antecedente";
    return "<div class=\"truncate" + cssclass + "\" style=\"width:50px;\" title=\"" + title + "\"><i class=\"fal fa-lg fa-fw fa" + icon + "-square\"></i></div>";
}

function ResidenteMalaltia_ByResidente_AfterFill() {
    $.each($(".Antecedente"), function () {
        $(this).parent().parent().parent().css("background-color", "#d15b47").css("color", "#fff");
        var row = $(this).parent().parent().parent();
        $("#" + row[0].id + " .blue").removeClass("blue");
        $("#" + row[0].id + " a").css("color", "#fff");

    });
    //$(".Antecedente").parent().parent().parent().css("background-color", "#d15b47").css("color", "#fff");
    //var row = $(".Antecedente").parent().parent().parent();
    //if (row.length > 0) {
    //    $("#" + row[0].id + " .blue").removeClass("blue");
    //    $("#" + row[0].id + " a").css("color", "#fff");
    //}
}

function DEPOSITO_TipoColumn(data, row) {
    var text = "Banc";
    if (data === 1) { text = "Efectiu"; }
    return "<span title=\"Clickar per canviar el tipus de pagament\" id=\"BtnDepositoTipo_" + row.I + "\" onclick=\"DEPOSITO_TypeChanged(this);\" data-tipo=\"" + data + "\" style=\"cursor:pointer;\">" + text + "</span>";
}

function DEPOSITO_TypeChanged(sender) {
    var id = sender.id.split("_")[1];
    var tipo = $(sender).data("tipo");

    var data = {
        "depositoId": id * 1,
        "type": tipo,
        "applicationUserId": ApplicationUser.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/Instances/GENOGUES/Data/ItemDataBase.aspx/Deposito_ChangeType",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            var result = msg.d.ReturnValue;
            var id = result.split('|')[0];
            var newTipo = result.split('|')[1] * 1;
            $("#BtnDepositoTipo_" + id).data("tipo", newTipo);
            $("#D_" + id + " #T").html(DEPOSITO_TipoColumn(newTipo, { "I": id }));

        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}

function RESIDENTEUPP_Row(data) {
    $("#List_ResidenteCura_Header").hide();
    $("#scrollTableDiv_ResidenteCura_ByResidente").hide();
    var res = "";

    if (data.Active === false) {
        return "";
    }

    var tractament = "";
    if (HasPropertyValue(data.T)) {
        if (data.T.length > 75) {

            var original = data.T;
            while (original.length > 75) {
                tractament += original.substring(0, 75);
                original = original.substring(75);

                var posBlank = original.indexOf(' ');
                if (posBlank > 10 || posBlank < 0) { posBlank = original.indexOf(','); }
                if (posBlank > 10 || posBlank < 0) { posBlank = original.indexOf('.'); }

                if (posBlank >= 0 && posBlank < 11) {
                    tractament += original.substring(0, posBlank) + "<br />";
                    original = original.substring(posBlank);
                }
            }

            tractament += original;

        }
        else {
            tractament = data.T;
        }
    }

    if (data.G === "-1") {
        res += "<tr>";
        res += "  <td colspan=\"7\">";
        res += "    <table style=\"width:99%;background-color:" + (data.F === null ? "transparent" : "#def5de") + "!important;border:1px solid #ddd;\" class=\"TableInnerUPP\">";
        res += "      <tr id=\"" + data.Id + "\">";
        res += "        <td rowspan=\"4\" style=\"width:150px!important;text-align:center;\"><strong>Cura</strong><br />";
        if (data.P !== "") {
            res += "          <img style=\"max-height:90px;max-width:145px;\" src=\"/Instances/Genogues/Data/Images/ResidenteCuraSeguimiento/" + data.P + "?" + guid() + "\" onclick=\"RESDIENTEUPPSeguimiento_PrePhotoLauncher(this, '" + data.I + "')\" />";
        }
        else {
            res += "          <img style=\"max-height:90px;max-width:145px;\" src=\"/img/nofoto.png\" alt=\"No foto\" title=\"No hi ha imatge disponible\" />";
        }
        res += "</td>";
        res += "        <td style=\"width:100px;text-align:left;\" data-order=\"7\">Data aparició:</td>";
        res += "        <td style=\"width:90px;text-align:left;\" data-order=\"7\"><strong> " + data.I + "</strong></td>";
        res += "        <td style=\"width:190px;text-align:left;\" data-order=\"8\">Data curació: <strong>" + (data.F === null ? "-" : data.F) + "</strong></td>";
        res += "        <td data-order=\"9\">";
        res += "          &nbsp;";
        res += "        </td>";
        res += "        <td style=\"width:190px;text-align:left;\" data-order=\"8\"></td>";
        res += "        <td rowspan=\"5\" class=\"action-buttons\" data-buttons=\"1\" style=\"width:47px;white-space: nowrap;\">";
        res += "          <a style=\"width:24px;\" class=\"blue ace-icon fal fa-pencil-alt bigger-120\" id=\"" + data.Id + "\" onclick=\"GoEncryptedPageBlank('/Instances/Genogues/Pages/Cura.aspx', { 'CuraId': " + data.Id + ", 'ResidenteId':" + itemData.Id + "});\"></a>";
        res += "        </td>";
        res += "      </tr>";
        res += "      <tr>";
        res += "        <td>Localitació</td>";
        res += "        <td colspan=\"4\" data-order=\"1\"><div class=\"truncate\" title=\"" + data.L + "\">" + data.L + "</div></td>";
        res += "      </tr>";
        res += "      <tr>";
        res += "        <td>Tractament</td>";
        res += "        <td colspan=\"4\" data-order=\"1\"><div class=\"truncate\" title=\"" + data.T + "\">" + tractament + "</div></td>";
        res += "      </tr>";
        res += "      <tr>";
        res += "        <td>Horari cures:</td>";
        res += "        <td colspan=\"4\" data-order=\"1\">";
        var horari = data.Pr.split('|').join('   ');
        res += "          <div class=\"truncate\" title=\"" + horari + "\"><strong>" + horari + "</strong></div>";
        res += "        </td>";
        res += "      </tr>";
        res += "    </table>";
        res += "  </td>";
        res += "</tr>";
    }
    else {
        res += "<tr>";
        res += "  <td colspan=\"7\">";
        res += "    <table style=\"width:99%;background-color:" + (data.F === null ? "transparent" : "#def5de") + "!important;\" class=\"TableInnerUPP\">";
        res += "      <tr id=\"" + data.Id + "\">";
        res += "        <td rowspan=\"6\" style=\"width:150px!important;text-align:center;\"><strong>UPP</strong><br />";
        if (data.P !== "") {
            res += "          <img style=\"max-height:90px;max-width:145px;\" src=\"/Instances/GENOGUES/Data/Images/ResidenteUPPSeguimiento/" + data.P + "?" + guid() + "\" onclick=\"RESDIENTEUPPSeguimiento_PrePhotoLauncher(this, '" + data.I + "')\" />";
        }
        else {
            res += "          <img style=\"max-height:90px;max-width:145px;\" src=\"/img/nofoto.png\" alt=\"No foto\" title=\"No hi ha imatge disponible\" />";
        }
        if (data.N !== "") {
            //res += "<br /><a href =\"/Instances/Genogues/Data/ResidenteUPP/" + data.Id + "/" + data.N + "\" target=\"_blank\"><i class=\"fa fa-eye\"></i>&nbsp;Veure document</a>";
        }
        res += "        </td>";
        res += "        <td style=\"width:100px;text-align:left;\">Data aparició:</td>";
        res += "        <td style=\"width:90px;text-align:left;\"><strong>" + data.I + "</strong></td > ";
        res += "        <td style=\"width:100px;text-align:left;\">Data curació:</td>";
        res += "        <td style=\"width:90px;text-align:left;\"><strong>" + (data.F === null ? "-" : data.F) + "</strong></td>";
        res += "        <td style=\"width:120px;\" data-order=\"9\">";
        res += "          <div class=\"truncate\" style=\"width: 80px; \" title=\"" + data.G + "\">Grau: <strong>" + data.G + "</strong></div>";
        res += "        </td>";
        res += "        <td style=\"text-align:left;\" data-order=\"8\">Darrera revisió: <strong>" + (data.D === null ? "-" : data.D) + "</strong></td>";
        res += "        <td rowspan=\"5\" class=\"action-buttons\" data-buttons=\"1\" style=\"width:47px;white-space: nowrap;\">";
        res += "          <a style=\"width:24px;\" class=\"blue ace-icon fal fa-pencil-alt bigger-120\" id=\"" + data.Id + "\" onclick=\"GoEncryptedPageBlank('/Instances/Genogues/Pages/UPP.aspx', { 'UppId': " + data.Id + ", 'ResidenteId':" + itemData.Id + "});\"></a>";
        res += "        </td>";
        res += "      </tr>";
        res += "      <tr>";
        res += "        <td style=\"\">Localitació:</td>";
        res += "        <td style=\"\" colspan=\"5\" data-order=\"1\">";
        res += "          <div class=\"truncate\" title=\"" + data.L + "\">" + data.L + "</div>";
        res += "        </td>";
        res += "      </tr>";
        res += "      <tr>";
        res += "        <td style=\"\">Procedència:</td>";
        res += "        <td style=\"\" colspan=\"5\" data-order=\"0\">";
        res += "          <div class=\"truncate\" style=\"width:650px;\" title=\"" + data.Pr.split('^')[0] + "\">" + data.Pr.split('^')[0] + "</div>";
        res += "        </td>";
        res += "      </tr>";
        res += "      <tr>";
        res += "        <td style=\"\">Tractament:</td>";
        res += "        <td style=\"\" colspan=\"5\" data-order=\"0\">";
        res += "          <div class=\"truncate\" style=\"width:650px;\" title=\"" + data.T + "\">" + data.T + "</div>";
        res += "        </td>";
        res += "      </tr>";
        res += "      <tr>";
        res += "        <td>Horari cures:</td>";
        res += "        <td colspan=\"4\" data-order=\"1\">";
        var horariUPP = data.Pr.split('^')[1].split('|').join('   ');
        res += "          <div class=\"truncate\"><strong>" + horariUPP + "</strong></div>";
        res += "        </td>";
        res += "      </tr>";
        res += "      <tr>";
        res += "        <td style=\"\">Evolució:</td>";
        res += "        <td style=\"\" colspan=\"5\" data-order=\"0\">";
        res += "          <div class=\"truncate\" style=\"width:650px;\" title=\"" + data.E + "\">" + data.E + "</div>";
        res += "        </td>";
        res += "      </tr>";
        res += "    </table>";
        res += "  </td>";
        res += "</tr>";
    }

    return res;
}

function RESIDENTEUPPMedico_Row(data) {
    $("#List_ResidenteCura_Header").hide();
    $("#scrollTableDiv_ResidenteCura_ByResidente").hide();
    var res = "";

    if (data.Active === false) {
        return "";
    }

    if (data.G === "-1") {
        res += "<tr>";
        res += "  <td colspan=\"7\">";
        res += "    <table style=\"width:99%;background-color:transparent!important;border:1px solid #ddd;\" class=\"TableInnerUPP\">";
        res += "      <tr id=\"" + data.Id + "\">";
        res += "        <td rowspan=\"4\" style=\"width:155px!important;text-align:center;\"><strong>Cura</strong><br />";
        if (data.P !== "") {
            res += "          <img style=\"max-height:90px;max-width:145px;\" src=\"/Instances/GENOGUES/Data/Images/Cura/" + data.P + "?" + guid() + "\" onclick=\"RESDIENTEUPPSeguimiento_PrePhotoLauncher(this, '" + data.I + "')\" />";
        }
        else {
            res += "          <img style=\"max-height:90px;max-width:145px;\" src=\"/img/nofoto.png\" alt=\"No foto\" title=\"No hi ha imatge disponible\" />";
        }
        res += "</td>";
        res += "        <td style=\"width:190px;text-align:left;\" data-order=\"7\">Data aparició: <strong>" + data.I + "</strong></td>";
        res += "        <td style=\"width:190px;text-align:left;\" data-order=\"8\">Data curació: <strong>" + (data.F === null ? "-" : data.F) + "</strong></td>";
        res += "        <td style=\"width:150px;\" data-order=\"9\">";
        res += "          &nbsp;";
        res += "        </td>";
        res += "        <td style=\"width:190px;text-align:left;\" data-order=\"8\"></td>";
        res += "        <td rowspan=\"5\" class=\"action-buttons\" data-buttons=\"1\" style=\"width:47px;white-space: nowrap;\">";
        res += "        </td>";
        res += "      </tr>";
        res += "      <tr>";
        res += "        <td style=\"width: 150px;\" >Localitació</td>";
        res += "        <td colspan=\"3\" data-order=\"1\">";
        res += "          <div class=\"truncate\" title=\"" + data.L + "\">" + data.L + "</div>";
        res += "        </td>";
        res += "      </tr>";
        res += "      <tr>";
        res += "        <td style=\"width: 150px;\" >Tractament</td>";
        res += "        <td colspan=\"3\" data-order=\"1\">";
        res += "          <div class=\"truncate\" title=\"" + data.T + "\">" + data.T + "</div>";
        res += "        </td>";
        res += "      </tr>";
        res += "      <tr>";
        res += "        <td style=\"width: 150px;\" >Horari cures:</td>";
        res += "        <td colspan=\"3\" data-order=\"1\">";
        var horari = data.Pr.split('|').join('   ');
        res += "          <div class=\"truncate\" title=\"" + horari + "\"><strong>" + horari + "</strong></div>";
        res += "        </td>";
        res += "      </tr>";
        res += "    </table>";
        res += "  </td>";
        res += "</tr>";
    }
    else {
        res += "<tr>";
        res += "  <td colspan=\"7\">";
        res += "    <table style=\"width:99%;background-color:transparent!important;border:1px solid #ddd;\" class=\"TableInnerUPP\">";
        res += "      <tr id=\"" + data.Id + "\">";
        res += "        <td rowspan=\"5\" style=\"width:155px!important;text-align:center;\">UPP<br />";
        if (data.P !== "") {
            res += "          <img style=\"max-height:90px;\" src=\"/Instances/GENOGUES/Data/Images/ResidenteUPP/" + data.P + "?" + guid() + "\" onclick=\"RESDIENTEUPPSeguimiento_PrePhotoLauncher(this, '" + data.I + "')\" />";
        }
        else {
            res += "          <img style=\"max-height:90px;\" src=\"/img/nofoto.png\" alt=\"No foto\" title=\"No hi ha imatge disponible\" />";
        }
        if (data.N !== "") {
            res += "<br /><a href =\"/Instances/Genogues/Data/ResidenteUPP/" + data.Id + "/" + data.N + "\" target=\"_blank\"><i class=\"fa fa-eye\"></i>&nbsp;Veure document</a>";
        }
        res += "        </td>";
        res += "        <td style=\"width:190px;text-align:left;\" data-order=\"7\">Data aparició: <strong>" + data.I + "</strong></td>";
        res += "        <td style=\"width:190px;text-align:left;\" data-order=\"8\">Data curació: <strong>" + (data.F === null ? "-" : data.F) + "</strong></td>";
        res += "        <td style=\"width:150px;\" data-order=\"9\">";
        res += "          <div class=\"truncate\" style=\"width: 130px; \" title=\"" + data.G + "\">Grau: <strong>" + data.G + "</strong></div>";
        res += "        </td>";
        res += "        <td style=\"width:190px;text-align:left;\" data-order=\"8\">Darrera revisió: <strong>" + (data.D === null ? "-" : data.D) + "</strong></td>";
        res += "        <td rowspan=\"5\" class=\"action-buttons\" data-buttons=\"1\" style=\"width:47px;white-space: nowrap;\">";
        //res += "          <a style=\"width:24px;\" class=\"blue ace-icon fal fa-pencil-alt bigger-120\" id=\"" + data.Id + "\" onclick=\"GoEncryptedPageBlank('/Instances/Genogues/Pages/UPP.aspx', { 'UppId': " + data.Id + ", 'ResidenteId':" + itemData.Id + "});\"></a>";
        res += "        </td>";
        res += "      </tr>";
        res += "      <tr>";
        res += "        <td style=\"width: 150px;\" >Localitació</td>";
        res += "        <td colspan=\"3\" data-order=\"1\">";
        res += "          <div class=\"truncate\" title=\"" + data.L + "\">" + data.L + "</div>";
        res += "        </td>";
        res += "      </tr>";
        res += "      <tr>";
        res += "        <td style=\"width: 150px;\" >Procedència</td>";
        res += "        <td colspan=\"3\" data-order=\"0\">";
        res += "          <div class=\"truncate\" title=\"" + data.Pr + "\">" + data.Pr + "</div>";
        res += "        </td>";
        res += "      </tr>";
        res += "      <tr>";
        res += "        <td style=\"width: 150px;\" >Tractament</td>";
        res += "        <td colspan=\"3\" data-order=\"0\">";
        res += "          <div class=\"truncate\" title=\"" + data.T + "\">" + data.T + "</div>";
        res += "        </td>";
        res += "      </tr>";
        res += "      <tr>";
        res += "        <td style=\"width: 150px;\" >Evolució</td>";
        res += "        <td colspan=\"2\" data-order=\"0\">";
        res += "          <div class=\"truncate\" title=\"" + data.E + "\">" + data.E + "</div>";
        res += "        </td>";
        res += "      </tr>";
        res += "    </table>";
        res += "  </td>";
        res += "</tr>";
    }

    return res;
}

function RESIDENTEMEDICACION_Row(data) {
    var width = $($("#ListDataTableHeaderResidenteMedicacion_ByResidente TH")[0]).width() + 6;
    var res = "";
    res += "<tr id=\"" + data.Id + "\">";
    res += "  <td data-order=\"0\" style=\"width:" + width + "px;\">";
    res += "    <div class=\"truncate\"style=\"width:" + (width - 16) + "px;\" title=\"" + data.MedicamentoId.Value + "\">" + data.MedicamentoId.Value + "</div>";
    res += "  </td>";
    res += "  <td style=\"width: 150px;\" data-order=\"1\">";
    res += "    <div class=\"truncate\" style=\"width: 130px; \" title=\"" + data.AdministracionId.Value + "\">" + data.AdministracionId.Value + "</div>";
    res += "  </td>";

    if (data.SegonsPauta === true) {
        res += "  <td colspan=\"5\" style=\"width:300px;text-align:left;\" data-order=\"2\">&nbsp;Segons pauta</td>";
    }
    else if (data.Franja1 === false && data.Franja2 === false && data.Franja3 === false && data.Franja4 === false && data.Franja5 === false) {
        if (data.Hora1 !== "") {
            var t1 = data.Hora1 + " (" + (data.Dosis1 !== null ? data.Dosis1 : "") + ")";
            res += "  <td style=\"width:58px;text-align:center;\" data-order=\"2\" title=\"" + t1 + "\">" + t1 + "</td>";
        }
        else {
            res += "  <td style=\"width:58px;\">&nbsp;</td>";
        }
        if (data.Hora2 !== "") {
            var t2 = data.Hora2 + " (" + (data.Dosis2 !== null ? data.Dosis2 : "") + ")";
            res += "  <td style=\"width:58px;text-align:center;\" data-order=\"2\" title=\"" + t2 + "\">" + t2 + "</td>";
        }
        else {
            res += "  <td style=\"width:58px;\">&nbsp;</td>";
        }
        if (data.Hora3 !== "") {
            var t3 = data.Hora3 + " (" + (data.Dosis3 !== null ? data.Dosis3 : "") + ")";
            res += "  <td style=\"width:58px;text-align:center;\" data-order=\"2\" title=\"" + t3 + "\">" + t3 + "</td>";
        }
        else {
            res += "  <td style=\"width:58px;\">&nbsp;</td>";
        }
        if (data.Hora4 !== "") {
            var t4 = data.Hora4 + " (" + (data.Dosis4 !== null ? data.Dosis4 : "") + ")";
            res += "  <td style=\"width:58px;text-align:center;\" data-order=\"2\" title=\"" + t4+ "\">" + t4 + "</td>";
        }
        else {
            res += "  <td style=\"width:58px;\">&nbsp;</td>";
        }
        if (data.Hora5 !== "") {
            var t5 = data.Hora5 + " (" + (data.Dosis5 !== null ? data.Dosis5 : "") + ")";
            res += "  <td style=\"width:58px;text-align:center;\" data-order=\"2\" title=\"" + t5 + "\">" + t5 + "</td>";
        }
        else {
            res += "  <td style=\"width:58px;\">&nbsp;</td>";
        }
    }
    else {
        if (data.Franja1 === true) {
            //var tf1 = "M (" + (data.Dosis1 !== null ? data.Dosis1 : "") + ")";
            var tf1 = data.Dosis1 !== null ? data.Dosis1 : "";
            res += "  <td style=\"width:58px;text-align:center;\" data-order=\"2\" title=\"" + tf1 + "\">" + tf1 + "</td>";
        }
        else {
            res += "  <td style=\"width:58px;\">&nbsp;</td>";
        }
        if (data.Franja2 === true) {
            //var tf2 = "MD (" + (data.Dosis2 !== null ? data.Dosis2 : "") + ")";
            var tf2 = data.Dosis2 !== null ? data.Dosis2 : "";
            res += "  <td style=\"width:58px;text-align:center;\" data-order=\"2\" title=\"" + tf2 + "\">" + tf2 + "</td>";
        }
        else {
            res += "  <td style=\"width:58px;\">&nbsp;</td>";
        }
        if (data.Franja3 === true) {
            //var tf3 = "T (" + (data.Dosis3 !== null ? data.Dosis3 : "") + ")";
            var tf3 = data.Dosis3 !== null ? data.Dosis3 : "";
            res += "  <td style=\"width:58px;text-align:center;\" data-order=\"2\" title=\"" + tf3 + "\">" + tf3 + "</td>";
        }
        else {
            res += "  <td style=\"width:58px;\">&nbsp;</td>";
        }
        if (data.Franja4 === true) {
            //var tf4 = "N (" + (data.Dosis4 !== null ? data.Dosis4 : "") + ")";
            var tf4 = data.Dosis4 !== null ? data.Dosis4 : "";
            res += "  <td style=\"width:58px;text-align:center;\" data-order=\"2\" title=\"" + tf4 + "\">" + tf4 + "</td>";
        }
        else {
            res += "  <td style=\"width:58px;\">&nbsp;</td>";
        }
        if (data.Franja5 === true) {
            //var tf5 = "S (" + (data.Dosis5 !== null ? data.Dosis5 : "") + ")";
            var tf5 = data.Dosis5 !== null ? data.Dosis5 : "";
            res += "  <td style=\"width:58px;text-align:center;\" data-order=\"2\" title=\"" + tf5 + "\">" + tf5 + "</td>";
        }
        else {
            res += "  <td style=\"width:58px;\">&nbsp;</td>";
        }
    }

    res += "  <td style=\"width:88px;text-align:center;\" data-order=\"7\">" + data.FINI + "</div></td>";
    res += "  <td style=\"width:88px;text-align:center;\" data-order=\"8\">" + (data.FFIN === null ? "" : data.FFIN) + "</div></td>";
    res += "  <td style=\"width:148px;\" data-order=\"9\">";
    res += "    <div class=\"truncate\" style=\"width: 128px; \" title=\"" + data.Observaciones + "\">" + data.Observaciones +"</div>";
    res += "  </td>";
    res += "  <td class=\"action-buttons\" data-buttons=\"1\" style=\"width:47px;white-space: nowrap;\">";
    res += "    <a style=\"width:24px;\" class=\"blue ace-icon fal fa-pencil-alt bigger-120\" id=\"" + data.Id + "\" onclick=\"PopupItem('ResidenteMedicacion', 'ByResidente', this.id);\"></a>";
    res += "  </td>";
    res += "</tr>";

    return res;
}

function SUVERY_Row(data) {
    var days = data.RecurrentDays;
    if (days === null) {
        days = "";
    }
    var res = "<tr id=\"" + data.Id + "\">";
    res += "  <td data-order=\"0\">";
    res += "    <div class=\"truncate\" title=\"" + data.Nombre + "\">";
    res += "      <a id=\"" + data.Id + "\" href=\"#\" onclick=\"GoEncryptedView('Survey', " + data.Id + ", 'Custom', []);\" >" + data.Nombre + "</a>";
    res += "   </div>";
    res += "  </td>";
    res += "  <td style=\"width: 300px;\" data-order=\"1\"><div class=\"truncate\" style=\"width: 280px;\" >";
    res += "  <a href=\"" + data.Url + "\" target=\"_blank\">" + data.Url + "<a>";
    res += "  </div></td> ";
    res += "  <td style=\"width:90px;text-align:center;\" data-order=\"2\">";
    res += "    <div class=\"truncate\" style=\"width:70px;\">" + data.Data + "</div>";
    res += "  </td>";
    res += "  <td style=\"width:90px;text-align:center;\" data-order=\"3\">";
    res += "    <div class=\"truncate\" style=\"width:70px;\" title=\"true\">";
    res += data.Recurrent === true ? "Sí" : "No";
    res += "    </div>";
    res += "</td>";
    res += "  <td style=\"width:90px;text-align:right;\" data-order=\"4\">";
    res += "    <div class=\"truncate\" style=\"width:70px;\" title=\"" + days + "\">" + days +"</div>";
    res += "</td > ";
    res += "  <td class=\"action-buttons\" data-buttons=\"" + data.Id + "\" style=\"width:47px;white-space:nowrap;\">";
    res += "    <a style=\"width:24px;\" class=\"blue ace-icon fal fa-pencil-alt bigger-120\" id=\"" + data.Id + "\" onclick=\"GoItemView('Survey', this.id)\"></a>";
    res += "  </td>";
    res += "</tr>";
    return res;
}

function RESIDENTE_Row(data) {
    var res = "<tr id=\"" + data.Id + "\" style=\"background-color:" + data.Color + "\" >";
	res += "  <td>";
	res += "  <div style=\"float:left;height:80px;width:80px;background-position:center;background-size: cover;background-image:url('/Instances/GeNogues/Data/Images/Residente/Photo_"+data.Id+".jpg');\" onclick=\"GoItemView('Residente', "+ data.Id+");\" /></div>";
    res += "  <div style=\"float:left;border-right:none;\">";
	res += "      <div style=\"float:left;margin-left:8px;width:200px;\">";
	res += "          <span style=\"font-size:16px;font-weight:bold;\">" + data.Nombre + "</span><br />";
	res += `           DNI: <strong>${data.DNI}</strong> - ${data.Code}<br />`;
	res += `           Habitación: <strong>${data.HabitacionId.Value}</strong><br />`;
	res += `           F.Nacimiento: <strong>${data.FechaNAC}</strong>`;
	res += "      </div>";
	
	res += "  <div style=\"float:left;margin-left:8px;width:100px;\">";
	res += "     " + ToBooleanCheck(data.D1) + (data.D1 === true ? "<strong>Diabetes</strong>" : "Diabetes") + "<br />";
    res += "     " + ToBooleanCheck(data.D2) + (data.D2 === true ? "<strong>Sintrón</strong>" : "Sintrón") + "<br />";
    res += "     " + ToBooleanCheck(data.D3) + (data.D3 === true ? "<strong>I.C.</strong>" : "I.C.") + "<br />";
    res += "     " + ToBooleanCheck(data.D4) + (data.D4 === true ? "<strong>M.Pasos</strong>" : "M.Pasos") + "<br />";
    res += "     " + ToBooleanCheck(data.D5) + (data.D5 === true ? "<strong>Epilépsia</strong>" : "Epilépsia") + "<br />";
    res += "     " + ToBooleanCheck(data.D6) + (data.D6 === true ? "<strong>Aseo</strong>" : "Aseo") + "<br />";
	res += "  </div>";
	
	res += "  <div style=\"width:80px;text-align:center;color:#fff;text-shadow: 2px 2px 2px #000000;-webkit-border-radius: 12px;-moz-border-radius: 12px;border-radius: 12px;float:left;margin-left:8px;background-color:";
	if(data.GDepend === "1") { res+= "#05a205"; }
	if(data.GDepend === "2") { res+= "#decc10"; }
	if(data.GDepend === "3") { res+= "#bf3636"; }
	res += "; font-size: 36px;\">"+ data.GDepend+ "<div style=\"color:#000;font-size:11px;font-weight:bold;text-shadow: none;\">" + data.FechaPAE + "<br /></div></div>";
	
	res += "  <div style=\"width:200px;float:left;margin-left:8px;\">";	
	res += "<strong>Prótesis:</strong> "+ data.Protesis + "<br />";
	res += "<strong>Alergias:</strong> "+ data.Alergias;
	res += "</div>";
	
	res += "  <div style=\"width:200px;float:left;margin-left:8px;\">";	
	res += "<strong>F.Alta:</strong> "+ (HasPropertyValue(data.FechaAlta) ? data.FechaAlta : "") + "<br />";
	res += "<strong>F.CIP:</strong> "+ (HasPropertyValue(data.FechaCIP) ? data.FechaCIP  : "") + "<br />";
	res += "<strong>CIP:</strong> "+ (HasPropertyValue(data.CIP) ? data.CIP  : "") + "<br />";
	res += "<strong>NASS:</strong> "+ (HasPropertyValue(data.NASS) ? data.NASS : "");
	res += "</div>";
	
	var BN = data.BNCF + data.BNEM + data.BNA + data.BNM + data.BNI;
	var color = "#ccf";
	if(BN < 10) { color = "#f77";}
	else if(BN < 13) { color = "#ff7";}
	else if(BN < 15) { color = "#fc7";}
	
	res += "  <div style=\"width:40px;text-align:center;text-shadow: 2px 2px 2px #000000;float:left;color:";
	res += color;
    res += ";font-size: 36px;\">" + BN + "<div style=\"color:#000;font-size:11px;font-weight:bold;text-shadow: none;\">Norton<br /></div></div>";

    res += "  </td>";
	res += "  <td class=\"action-buttons\" style=\"width:50px;font-size:18px;\">";
	res += "      <a class=\"blue ace-icon fal fa-pencil-alt bigger-120\" id=\""+ data.Id + "\" onclick=\"GoItemView('Residente', this.id)\"></a>";
	res += "      <a class=\"red ace-icon fal fa-times bigger-120\" id=\"" + data.Id + "\" onclick=\"DeleteItem('Residente','custom', this.id, 'list');\"></a>";
	res += "      <a class=\"green ace-icon fa fa-pills bigger-120\" id=\"ResidenteVerMedicacion(" + data.Id + ");\" title=\"Ver medicación\"></a>";
	res += "  </td>";
    res += "</tr>";
    return res;
}

function RESDIENTEUPPSeguimiento_PrePhotoLauncher(sender, fecha) {
    if (typeof sender.src !== "undefined") {
        ModalImageViewStandalone(sender, fecha);
    }
}

function URGENT_GoRegistrarCaida() {
    Navigation_Context.detectDirty = false;
    Navigation_Context.Urgent = true;
    GoEncryptedPageBlank("/Instances/Genogues/Pages/RegistreCaigudes.aspx", { "Id": -1 });
}

function URGENT_GoTrasllatHospital() {
    Navigation_Context.detectDirty = false;
    Navigation_Context.Urgent = true;
    GoEncryptedPageBlank("/Instances/Genogues/Pages/TrasllatHospital.aspx");
}

function URGENT_GoCanviMedicacio() {
    Navigation_Context.detectDirty = false;
    Navigation_Context.Urgent = true;
    GoEncryptedPageBlank("/Instances/Genogues/Pages/CanviMedicacio.aspx");
}

function Feature_AfterLogin_Action(action, itemName, itemId, rowId) {
    FEATURE_ALERTS_AFTERLOGIN_Context = {
        "action": action,
        "itemName": itemName,
        "itemId": itemId,
        "rowId": rowId
    };
    $("#AfterLoginLocker").hide();
    $("#AfterLoginContent").hide();

    switch (action) {
        case "GoSeguimentsCoordinadora":
            Go('/Instances/Genogues/Pages/SeguimentsCoordinadora.aspx?YWM9MSZvsdfpb25JZD0w');
            break;
        case "AvisFamilia":
            FEATURE_AFTERLOGIN_AvisFamilia(itemName, itemId);
            break;
        case "DIETESSERVIDES_NoYesterday":
            DIETESSERVIDES_NoYesterday();
            break;
        case "FacturaIncompleta":
            GoEncryptedPage("/Instances/Genogues/Billing/View.aspx", { "Id": itemId });
            break;
    }
}

function FEATURE_AFTERLOGIN_AvisFamilia(itemName, itemId) {
    var data = GetItemDataJson(itemName, itemId, FEATURE_AFTERLOGIN_AvisFamiliaCallback);
}

function FEATURE_AFTERLOGIN_AvisFamiliaCallback(data) {
    var realData = null;
    eval("realData = " + data + ";");
    var contacte = realData.ContactePrincipalName;
    var telefon = realData.ContactePrincipalTel1;
    PopupConfirm("Caiguda registrada.<br />S'ha d'informar a la familia de la caiguda.<br />Les dades de contacte son:<br /><br /><ul><li>Persona de contacte: <strong>" + contacte + "</strong></li><li>Telèfon: <strong>" + telefon + "</strong></li></ul>Si ha contactat amb la familia premi <strong>SI</strong>, si no ha estat possible premi <strong>NO</strong>.", "Atenci&oacute;n", RESIDENTECAIDA_FamiliaAvisadaFromAlert);
}

function RESIDENTECAIDA_FamiliaAvisadaFromAlert() {
    var dataSend = {
        "residenteId": FEATURE_ALERTS_AFTERLOGIN_Context.itemId,
        "companyId": CompanyId,
        "applicationUserId": ApplicationUser.Id
    };
    $.ajax({
        "type": "POST",
        "url": "/Instances/GENOGUES/Data/ItemDataBase.aspx/ResidenteCaigudaSetFamiliaAvisada",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(dataSend, null, 2),
        "success": function (msg) {
            $("#TR_AL_" + FEATURE_ALERTS_AFTERLOGIN_Context.rowId).hide();

            if (AfterLoginPopupActive === true) {
                $("#AfterLoginLocker").show();
                $("#AfterLoginContent").show();
            }
            PopupWorkingHide();
        },
        "error": function (msg) {
            PopupWorkingHide();
            PopupWarning(msg.responseText);
        }
    });
}

function DIETESSERVIDES_NoYesterday() {
    document.location = "/Instances/Genogues/Pages/DietesServides.aspx";
}

function TOOLS_ResidenteGetContactMails() {
    $.ajax({
        "type": "POST",
        "url": "/Instances/GENOGUES/Data/ItemDataBase.aspx/ResidenteGetContactMails",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify({ "companyId": CompanyId }, null, 2),
        "success": function (msg) {
            console.log(msg);
            $("#ContactMails").val(msg.d);
        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}

function RESIDENTE_BtnPesame(value, itemId, rowData) {
    if (value === "true") {
        return "<i class=\"fa fa-check green\" title=\"Procés de condol finalitzat\"></i>";
    }
    return "<i id=\"icon_" + itemId + "\" class=\"fa fa-cog blue\" title=\"Marcar procés de baixa com a finalitzat\"></i>";
}

var ItemResidenteBajaProcesoId = null;
function RESIDENTE_BtnPesameClicked(sender) {
    ItemResidenteBajaProcesoId = sender.target.id.split('_')[1];
    PopupConfirm("Confirmi les següentes accions:<ul><li>S'ha trasmés el condol a la familia</li><li>S'ha pujat el certificat de defunció</li><li>S'ha trucat al tanatori per les flors</li></ul>", Dictionary.Common_Warning, RESIDENTE_BtnPesameClickedConfirmed);
}

function RESIDENTE_BtnPesameClickedConfirmed() {
    $.ajax({
        "type": "POST",
        "url": "/Instances/GENOGUES/Data/ItemDataBase.aspx/Residente_PesameFinalizado",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify({ "residenteId": ItemResidenteBajaProcesoId }, null, 2),
        "success": function (msg) {
            if (msg.d.MessageError === "") {
                $("#icon_" + ItemResidenteBajaProcesoId).parent().html("<i class=\"fa fa-check green\" title=\"Procés de condol finalitzat\"></i>");
                ItemResidenteBajaProcesoId = null;

            } else {
                PopupWarning(msg.d.MessageError);
            }

        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}

function ListadosSetToPrint(ids) {
    var data = {
        "ids": ids,
        "companyId": CompanyId
    };
    $.ajax({
        "type": "POST",
        "url": "/Async/ItemDataServices.asmx/ListadosSetToPrint",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
        },
        "error": function (msg) {
            console.log("ListadosSetToPrint", msg.responseText);
        }
    });
}

var listadosUpdateIds = "";
function ListadosSetUpdated(ids) {
    listadosUpdateIds = ids;
    var data = {
        "ids": ids,
        "companyId": CompanyId
    };
    $.ajax({
        "type": "POST",
        "url": "/Async/ItemDataServices.asmx/ListadosSetUpdated",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log("listadosUpdateIds", listadosUpdateIds);
        },
        "error": function (msg) {
            console.log("ListadosSetToPrint", msg.responseText);
        }
    });
}

function RECEIPT_Print(id) {
    var data = {
        "id": id,
        "companyId": CompanyId
    };
    $.ajax({
        "type": "POST",
        "url": "/Async/BillingActions.asmx/BillingReceipt_Print",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            window.open(msg.d.ReturnValue + "?" + guid());
        },
        "error": function (msg) {
            PopupWorkingHide();
            PopupWarning(msg.responseText);
        }
    });
}

function ESTADOSOLICITUD_TypeColumn(value, row) {
    switch (value) {
        case 1: return "PAO";
        case 2: return "Interconsulta";
        case 3: return "Analítica";
        default: return "";
    }
}

function ESTADOSOLICITUD_StatusColumn(value, row) {
    var res = "<span id=\"ES_Status_" + row.Id + "\" onclick=\"ESTADOSOLICTUD_ChangeStatus(" + row.Id + "," + value + ");\" style=\"cursor:pointer;\">";
    if (value * 1 === 1) {
        res += "<span style=\"color:green;\">Realitzat</span>";
    } else {
        res += "<span style=\"color:red;\">Pendent</span>";
    }
    res += "</span>";
    return res;
}

function ESTADOSOLICTUD_ChangeStatus(id, status) {
    var data = {
        "solicitudId": id,
        "status": status,
        "companyId": CompanyId,
        "applicationUserId": ApplicationUser.Id
    };

    $("#ES_Status_" + id).html("<i class=\"fa fa-cog fa-spin blue\"></i>");

    $.ajax({
        "type": "POST",
        "url": "/Instances/Genogues/Data/ItemDataBase.aspx/Item_EstadoSolicitud_ChangeStatus",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg);
            var res = msg.d.ReturnValue;
            var status = ESTADOSOLICITUD_StatusColumn(res.split('|')[1], { "Id": res.split('|')[0] } );
            $("#ES_Status_" + res.split('|')[0]).parent().html(status);

            // Actualizar listSource
            var id = res.split('|')[0] * 1;
            for (var x = 0; x < listSources[0].Data.length; x++) {
                if (listSources[0].Data[x].Id === id) {
                    listSources[0].Data[x].Status = res.split('|')[1] * 1;
                    break;
                }
            }
            ESTADOSOLICITUD_FilterList();
        },
        "error": function (msg) {
            console.log(msg.responseText);
        }
    });
}

function RESIDENTECAIDA_DocumentoColumn(value, row) {
    var res = "<i class=\"fa fa-file-pdf blue\" onclick=\"RESIDENTECAIDA_Print(" + row.Id + ")\"></i>";
    res += "&nbsp;<i class=\"fa fa-times red\" onclick=\"RESIDENTECAIDA_Delete(" + row.Id + ")\"></i>";
    return res;
}

function RESIDENTECAIDA_Print(id) {
    var data = {
        "id": id,
        "residenteId": itemData.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/Instances/Genogues/Export/RegistreCaiguda2.aspx/Generate",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg);
            window.open(msg.d);
        },
        "error": function (msg) {
            console.log(msg.responseText);
        }
    });
}

var RESIDENTECAIDA_ToDeleteId = null;
function RESIDENTECAIDA_Delete(id) {
    RESIDENTECAIDA_ToDeleteId = id;
    PopupConfirm("Vol eliminar el registre d'aqueta caiguda", Dictionary.Common_Warning, RESIDENTECAIDA_DeleteConfirmed);
}

function RESIDENTECAIDA_DeleteConfirmed() {
    var data = {
        "id": RESIDENTECAIDA_ToDeleteId,
        "applicationUserId": ApplicationUser.Id
    };

    $.ajax({
        "type": "POST",
        "url": "/Instances/Genogues/Data/ItemDataBase.aspx/ResidenteCaidaDelete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            var listoToUpdate = PageListById("ResidenteCaida", "ByResidente");
            listoToUpdate.GetData();
        },
        "error": function (msg) {
            console.log(msg.responseText);
        }
    });
}

function Incidencia_Column_Tipo(data, row) {
    var res = "";

    switch (data) {
        case "2":
            res = "Evolutiu";
            break;
        case "3":
            res = "Correcció de dades";
            break;
        case "4":
            res = "Actuació";
            break;
        default:
            res = "Incidència";
            break;
    }

    return res;
}