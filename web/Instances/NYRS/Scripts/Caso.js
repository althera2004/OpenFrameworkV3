function CASO_CustomActions() { 
	console.log("CASO#", "CustomActions Loaded"); 
	CASO_Layout();
	CASO_Events();
}

function CASO_STAIPopup(staiId) {
	var res = "";
	res += "<h3>";
	var data = "";
	switch(staiId) {
		case 1: 
			res += "Datos estudio incial";
			data = ItemData.OriginalItemData.EscalaSTAI1;
		break;
		case 2: 
			res += "Datos estudio intermedio";
			data = ItemData.OriginalItemData.EscalaSTAI2;
			break;
		case 3: 
			res += "Datos estudio final";
			data = ItemData.OriginalItemData.EscalaSTAI3;
			break;
	}
	
	console.log("STAI",data);
	
	res += "</h3>";
	res += "<table class=\"table\">";
	res += "<tr style=\"border-top:none;\">";
	res += "<th style=\"border-top:none;\"></th>";
	res += "<th style=\"border-top:none;\">Nada</th>";
	res += "<th style=\"border-top:none;\">Algo</th>";
	res += "<th style=\"border-top:none;\">Mucho</th>";
	res += "<th style=\"border-top:none;\">Bastante</th>";
	res += "</tr>";
	
	res+="<tr>";
	res += "<td>Se siente cómodo/a (está a gusto)</td>";
	res += "<td style=\"text-align:center;\">" + ( data[0] === '3' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[0] === '2' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[0] === '1' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[0] === '0' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res+= "</tr>";
	
	res+="<tr>";
	res += "<td>Se siente angustiado/a</td>";
	res += "<td style=\"text-align:center;\">" + ( data[1] === '0' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[1] === '1' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[1] === '2' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[1] === '3' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res+= "</tr>";
	
	res+="<tr>";
	res += "<td>Se siente confortable</td>";
	res += "<td style=\"text-align:center;\">" + ( data[2] === '3' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[2] === '2' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[2] === '1' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[2] === '0' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res+= "</tr>";
	
	res+="<tr>";
	res += "<td>Se siente nervioso/a</td>";
	res += "<td style=\"text-align:center;\">" + ( data[3] === '0' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[3] === '1' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[3] === '2' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[3] === '3' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res+= "</tr>";
	
	res+="<tr>";
	res += "<td>Se siente preocupado/a</td>";
	res += "<td style=\"text-align:center;\">" + ( data[4] === '0' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[4] === '1' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[4] === '2' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[4] === '3' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res+= "</tr>";
	
	res+="<tr>";
	res += "<td>En este momento se siente bien</td>";
	res += "<td style=\"text-align:center;\">" + ( data[5] === '3' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[5] === '2' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[5] === '1' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res += "<td style=\"text-align:center;\">" + ( data[5] === '0' ? "<i class=\"fa fa-check green\"></i>" : "" ) + "</td>";
	res+= "</tr>";
	
	res += "<table>";
	
	
	$("#PopupDefaultBody").html(res);

	PopupDefault({
		"Title": "Test de ansiedad STAI-e 6",
		"BtnDelete": false,
		"BtnOk": false,
		"Width": 600
	});
}


function CASO_Layout() {
	CASO_LayoutConsentimientoInformado();
	
	var TrasladoHtml = "";
	TrasladoHtml += "<div class=\"row\">";
	TrasladoHtml += "  <label class=\"col-sm-2 control-label\">Tipo de transporte</label>";
	TrasladoHtml += "  <label class=\"col-sm-2 control-label\" id=\"TipoTransporte\">-</label>";
	TrasladoHtml += "  <label class=\"col-sm-2 control-label\">Tipo de unidad</label>";
	TrasladoHtml += "  <label class=\"col-sm-2 control-label\" id=\"TipoUnidad\">-</label>";
	TrasladoHtml += "  <label class=\"col-sm-2 control-label\">Terapia aplicada</label>";
	TrasladoHtml += "  <label class=\"col-sm-2 control-label\" id=\"TerapiaAplicada\">-</label>";
	TrasladoHtml += "</div>";
	$("#Traslado_Label").parent().after(TrasladoHtml);
	$("#Traslado_Label").parent().hide();
	
	$("#TA1").html("Tensión arterial");
	$("#TA2").html("Tensión arterial");
	$("#TA3").html("Tensión arterial");
	
	$("#EscalaSTAI1_Label").replaceClass("col-sm-1", "col-sm-2");
	$("#EscalaSTAI1").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#Confort1").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#TAD1").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#TAS1").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#TAM1").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#FC1_Label").replaceClass("col-sm-1", "col-sm-2");
	$("#FC1").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#FR1_Label").replaceClass("col-sm-1", "col-sm-2");
	$("#FR1").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#SO1_Label").replaceClass("col-sm-1", "col-sm-2");
	$("#SO1").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#EscalaSTAI2_Label").replaceClass("col-sm-1", "col-sm-2");
	$("#EscalaSTAI2").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#Confort2").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#TAD2").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#TAS2").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#TAM2").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#FC2_Label").replaceClass("col-sm-1", "col-sm-2");
	$("#FC2").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#FR2_Label").replaceClass("col-sm-1", "col-sm-2");
	$("#FR2").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#SO2_Label").replaceClass("col-sm-1", "col-sm-2");
	$("#SO2").replaceClass("col-sm-2", "col-sm-1");
	$("#EscalaSTAI3_Label").replaceClass("col-sm-1", "col-sm-2");
	$("#EscalaSTAI3").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#Confort3").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#TAD3").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#TAS3").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#TAM3").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#FC3_Label").replaceClass("col-sm-1", "col-sm-2");
	$("#FC3").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#FR3_Label").replaceClass("col-sm-1", "col-sm-2");
	$("#FR3").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#SO3_Label").replaceClass("col-sm-1", "col-sm-2");
	$("#SO3").parent().replaceClass("col-sm-2", "col-sm-1");
	$("#TiempoLlegada_Label").replaceClass("col-sm-1", "col-sm-2");
	$("#TiempoLlegada").parent().parent().replaceClass("col-sm-3", "col-sm-2");
	$("#HoraLlegada_Label").replaceClass("col-sm-1", "col-sm-2");
	$("#HoraLlegada").parent().parent().replaceClass("col-sm-3", "col-sm-2");
}

function CASO_Events() {
}

function CASO_LayoutConsentimientoInformado() {
	$("#ConsentimientoOral_Label").replaceClass("col-sm-1", "col-sm-3");
	$("#ConsentimientoOral_Label").html("Consentimiento informado");
	$("#ConsentimientoOral").parent().html("<i class=\"fa fa-eye blue\" style=\"cursor:pointer;\" onclick=\"Form_AddDocument('Consentimiento');\"></i>");
}

function CASO_CUSTOM_FormFillAfter() {
	if(ItemData.OriginalItemData.Code[0] !== 'I') {
		$("#tabSelect-visionado").hide();
	}
	
	CASO_PlaceHolderVisionadoRender();
	CASO_LauoytTraslado();
	
	
	FieldToLabel("EscalaSTAI1");
	FieldToLabel("EscalaSTAI2");
	FieldToLabel("EscalaSTAI3");
	
	FieldToLabel("Confort1");
	FieldToLabel("Dolor1");
	FieldToLabel("TAD1");
	FieldToLabel("TAS1");
	FieldToLabel("TAM1");
	FieldToLabel("FC1");
	FieldToLabel("FR1");
	FieldToLabel("SO1");
	
	FieldToLabel("Confort2");
	FieldToLabel("Dolor2");
	FieldToLabel("TAD2");
	FieldToLabel("TAS2");
	FieldToLabel("TAM2");
	FieldToLabel("FC2");
	FieldToLabel("FR2");
	FieldToLabel("SO2");
	
	FieldToLabel("Confort3");
	FieldToLabel("Dolor3");
	FieldToLabel("TAD3");
	FieldToLabel("TAS3");
	FieldToLabel("TAM3");
	FieldToLabel("FC3");
	FieldToLabel("FR3");
	FieldToLabel("SO3");
	
	CASO_LayoutEscalas();
	CASO_LayoutDatosClinicos();
}

function CASO_LayoutEscalas() {
	var escalaStai1Result = 0;
	if(ItemData.OriginalItemData.EscalaSTAI1 !== null) {
		for(var x = 0; x < ItemData.OriginalItemData.EscalaSTAI1.length; x++){
			escalaStai1Result+= ItemData.OriginalItemData.EscalaSTAI1[x]*1;
		}	
	
		$("#EscalaSTAI1_Labeled").html(escalaStai1Result);
		if(isNaN(escalaStai1Result) === false) {
			$("#EscalaSTAI1_Label").append("&nbsp;<i class=\"fa fa-eye blue\" onclick=\"CASO_STAIPopup(1);\"></i>");
		}
	}
	
	var escalaStai2Result = 0;
	if(ItemData.OriginalItemData.EscalaSTAI2 !== null) {
		for(var x = 0; x < ItemData.OriginalItemData.EscalaSTAI2.length; x++){
			escalaStai2Result+= ItemData.OriginalItemData.EscalaSTAI2[x]*1;
		}	
		
		$("#EscalaSTAI2_Labeled").html(escalaStai2Result);
		if(isNaN(escalaStai2Result) === false) {
			$("#EscalaSTAI2_Label").append("&nbsp;<i class=\"fa fa-eye blue\" onclick=\"CASO_STAIPopup(2);\"></i>");
		}
	}
	
	var escalaStai3Result = 0;
	if(ItemData.OriginalItemData.EscalaSTAI3 !== null) {
		for(var x = 0; x < ItemData.OriginalItemData.EscalaSTAI3.length; x++){
			escalaStai3Result+= ItemData.OriginalItemData.EscalaSTAI3[x]*1;
		}
		
		$("#EscalaSTAI3_Labeled").html(escalaStai3Result);
		if(isNaN(escalaStai3Result) === false) {
			$("#EscalaSTAI3_Label").append("&nbsp;<i class=\"fa fa-eye blue\" onclick=\"CASO_STAIPopup(3);\"></i>");
		}
	}
	
	$("#tab-datosEstudio span").each(function(){
		if($(this).html() === "-1" || $(this).html() === "") {
			$(this).html("-");
		}
	});	
}

function CASO_LauoytTraslado() {
	var traslado = ItemData.OriginalItemData.Traslado;
	
	if(typeof traslado === "undefined" || traslado == null || traslado == "") {
		return;
	}
	
	switch (traslado[0]) {
		case "1": $("#TipoTransporte").html("Terrestre"); break;
		case "2": $("#TipoTransporte").html("Aéreo");break;
	}
	switch(traslado[1]){
		case "1": $("#TipoUnidad").html("USVAE");break;
		case "2": $("#TipoUnidad").html("USVAM");break;
		case "3": $("#TipoUnidad").html("Otro");break;
	}
	switch(traslado[2]){
		case "1": $("#TerapiaAplicada").html("ICP");break;
		case "2": $("#TerapiaAplicada").html("Fobronólisis");break;
		case "3": $("#TerapiaAplicada").html("Desconocido");break;
	}
}

function CASO_PlaceHolderVisionadoRender() {
	
	var iconYes = "<i class=\"fa fa-check green\"></i>";
	var iconNo = "<i class=\"fa fa-times red\"></i>";
	
	var visualizado1 = ItemData.OriginalItemData.Video1Played ? moment.utc(ItemData.OriginalItemData.Video1Viewed * 1000).format('mm:ss') : "-";
	var visualizado1Percent = ItemData.OriginalItemData.Video1Completed ? "100%" : (ItemData.OriginalItemData.Video1Played ? (ToMoneyFormat(ItemData.OriginalItemData.Video1Viewed * 100 / ItemData.OriginalItemData.Video1Duration) + "%") : "-");
	
	var visualizado2 = ItemData.OriginalItemData.Video2Played ? moment.utc(ItemData.OriginalItemData.Video2Viewed * 1000).format('mm:ss') : "-";
	var visualizado2Percent = ItemData.OriginalItemData.Video2Completed ? "100%" : (ItemData.OriginalItemData.Video2Played ? (ToMoneyFormat(ItemData.OriginalItemData.Video2Viewed * 100 / ItemData.OriginalItemData.Video2Duration) + "%") : "-");
	
	var visualizado3 = ItemData.OriginalItemData.Video3Played ? moment.utc(ItemData.OriginalItemData.Video3Viewed * 1000).format('mm:ss') : "-";
	var visualizado3Percent = ItemData.OriginalItemData.Video3Completed ? "100%" : (ItemData.OriginalItemData.Video3Played ? (ToMoneyFormat(ItemData.OriginalItemData.Video3Viewed * 100 / ItemData.OriginalItemData.Video3Duration) + "%") : "-");
	
	var visualizado4 = ItemData.OriginalItemData.Video4Played ? moment.utc(ItemData.OriginalItemData.Video4Viewed * 1000).format('mm:ss') : "-";
	var visualizado4Percent = ItemData.OriginalItemData.Video4Completed ? "100%" : (ItemData.OriginalItemData.Video4Played ? (ToMoneyFormat(ItemData.OriginalItemData.Video4Viewed * 100 / ItemData.OriginalItemData.Video4Duration) + "%") : "-");
	
	var visualizado5 = ItemData.OriginalItemData.Video5Played ? moment.utc(ItemData.OriginalItemData.Video5Viewed * 1000).format('mm:ss') : "-";
	var visualizado5Percent = ItemData.OriginalItemData.Video5Completed ? "100%" : (ItemData.OriginalItemData.Video5Played ? (ToMoneyFormat(ItemData.OriginalItemData.Video5Viewed * 100 / ItemData.OriginalItemData.Video5Duration) + "%") : "-");
	
	var res = "";
	res += "<table class=\"table\">";
	res += "<thead><tr>";
	res += "  <th>Video</th>";
	res += "  <th style=\"text-align:center;\">Visualizado</th>";
	res += "  <th>Tiempo visualizado</th>";
	res += "  <th>%</th>";
	res += "</tr></thead>";
	
	res += "<tbody>";
	
	res += " <tr>";
	res += "    <td>Video 0. Introducción</td>";
	res += "    <td style=\"text-align:center;\">" +  iconYes + "</td>";
	res += "    <td>1:47</td>";
	res += "    <td>100%</td>";
	res += " </tr>";
	
	res += " <tr>";
	res += "    <td>Video 1. La historia de Alfredo</td>";
	res += "    <td style=\"text-align:center;\">" + (ItemData.OriginalItemData.Video1Played ? iconYes : iconNo) + "</td>";
	res += "    <td>"+ visualizado1 +"</td>";
	res += "    <td>"+ visualizado1Percent +"</td>";
	res += " </tr>";	
	
	res += " <tr>";
	res += "    <td>Video 2. ¿Qué me está pasando?</td>";
	res += "    <td style=\"text-align:center;\">" + (ItemData.OriginalItemData.Video2Played ? iconYes : iconNo) + "</td>";
	res += "    <td>"+ visualizado2 +"</td>";
	res += "    <td>"+ visualizado2Percent +"</td>";
	res += " </tr>";
	
	res += " <tr>";
	res += "    <td>Video 3. ¿Dónde vamos?</td>";
	res += "    <td style=\"text-align:center;\">" + (ItemData.OriginalItemData.Video3Played ? iconYes : iconNo) + "</td>";
	res += "    <td>"+ visualizado3 +"</td>";
	res += "    <td>"+ visualizado3Percent +"</td>";
	res += " </tr>";
	
	res += " <tr>";
	res += "    <td>Video 4. ¿Qué me van a hacer?</td>";
	res += "    <td style=\"text-align:center;\">" + (ItemData.OriginalItemData.Video4Played ? iconYes : iconNo) + "</td>";
	res += "    <td>"+visualizado4 +"</td>";
	res += "    <td>"+ visualizado4Percent +"</td>";
	res += " </tr>";
	
	res += " <tr>";
	res += "    <td>Video 5. Vamos a relajarnos</td>";
	res += "    <td style=\"text-align:center;\">" + (ItemData.OriginalItemData.Video5Played ? iconYes : iconNo) + "</td>";
	res += "    <td>"+ visualizado5 +"</td>";
	res += "    <td>"+ visualizado5Percent +"</td>";
	res += " </tr>";
	
	res += "</tbody>";
	
	res +="<table>";
	
	
	$("#Placeholder_Videos").html(res);
}

function CASO_LayoutDatosClinicos() {
	var textData = "";
	if(HasPropertyValue(ItemData.OriginalItemData.DatosClinicos)) {
		for (var x = 0; x < ItemData.OriginalItemData.DatosClinicos.length; x++) {
			switch (ItemData.OriginalItemData.DatosClinicos[x]) {
				case '1': textData += "Sí|"; break;
				case '2': textData += "No|"; break;
				default: textData += "-|"; break;
			}
		}
	}

	var radioData = textData.split('|');
	
	var localizacionDolorText = "-";
	if(HasPropertyValue(ItemData.OriginalItemData.LocalizacionDolor)) {
		var first = true;
		
		if(ItemData.OriginalItemData.LocalizacionDolor.indexOf('B') !==-1) {
			localizacionDolorText = "Brazo";
			first = false;
		}
		
		if(ItemData.OriginalItemData.LocalizacionDolor.indexOf('C') !==-1) {
			if(first === true) {
				localizacionDolorText ="Cuello";
			} else{
				localizacionDolorText +=", cuello";
			}			
			
			first = false;
		}
		
		if(ItemData.OriginalItemData.LocalizacionDolor.indexOf('E') !==-1) {
			if(first === true) {
				localizacionDolorText ="Espalda";
			} else{
				localizacionDolorText +=", espalda";
			}			
			
			first = false;
		}
		
		if(ItemData.OriginalItemData.LocalizacionDolor.indexOf('T') !==-1) {
			if(first === true) {
				localizacionDolorText ="Estómago";
			} else{
				localizacionDolorText +=", estómago";
			}			
			
			first = false;
		}
		
		if(ItemData.OriginalItemData.LocalizacionDolor.indexOf('M') !==-1) {
			if(first === true) {
				localizacionDolorText ="Mandíbula";
			} else{
				localizacionDolorText +=", mandíbula";
			}			
			
			first = false;
		}
		
		if(ItemData.OriginalItemData.LocalizacionDolor.indexOf('P') !==-1) {
			if(first === true) {
				localizacionDolorText ="Pecho";
			} else{
				localizacionDolorText +=", pecho";
			}			
			
			first = false;
		}
		
	}
	
	var localizacionInfartoText = "-";
	if(HasPropertyValue(ItemData.OriginalItemData.LocalizacionInfarto)) {
		var first = true;
		
		if(ItemData.OriginalItemData.LocalizacionInfarto.indexOf('A') !==-1) {
			localizacionInfartoText = "Anterior";
			first = false;
		}
		
		if(ItemData.OriginalItemData.LocalizacionInfarto.indexOf('C') !==-1) {
			if(first === true) {
				localizacionInfartoText ="Apical";
			} else{
				localizacionInfartoText +=", apical";
			}			
			
			first = false;
		}
		
		if(ItemData.OriginalItemData.LocalizacionInfarto.indexOf('I') !==-1) {
			if(first === true) {
				localizacionInfartoText ="Inferior";
			} else{
				localizacionInfartoText +=", inferior";
			}			
			
			first = false;
		}
		
		if(ItemData.OriginalItemData.LocalizacionInfarto.indexOf('L') !==-1) {
			if(first === true) {
				localizacionInfartoText ="Lateral";
			} else{
				localizacionInfartoText +=", lateral";
			}			
			
			first = false;
		}
		
		if(ItemData.OriginalItemData.LocalizacionInfarto.indexOf('P') !==-1) {
			if(first === true) {
				localizacionInfartoText ="Posterior";
			} else{
				localizacionInfartoText +=", posterior";
			}			
			
			first = false;
		}
		
		if(ItemData.OriginalItemData.LocalizacionInfarto.indexOf('S') !==-1) {
			if(first === true) {
				localizacionInfartoText ="Septal";
			} else{
				localizacionInfartoText +=", septal";
			}			
			
			first = false;
		}
		
		if(ItemData.OriginalItemData.LocalizacionInfarto.indexOf('V') !==-1) {
			if(first === true) {
				localizacionInfartoText ="VD";
			} else{
				localizacionInfartoText +=", VD";
			}			
			
			first = false;
		}
		
	}

	var res = "";
	res += "<div class=\"row\">";
	res += "<label class=\"col-sm-2 control-label\">Presencia de palidez</label>";
	res += "<label class=\"col-sm-1 control-label\" style=\"font-weight:bold;\">" + radioData[0] + "</label>";
	res += "<label class=\"col-sm-3 control-label\">Presencia de náuses y/o vómitos</label>";
	res += "<label class=\"col-sm-1 control-label\" style=\"font-weight:bold;\">" + radioData[1] + "</label>";
	res += "<label class=\"col-sm-2 control-label\">Presencia de disnea</label>";
	res += "<label class=\"col-sm-1 control-label\" style=\"font-weight:bold;\">" + radioData[2] + "</label>";
	res += "<div class=\"col-sm-3\">&nbsp;</div>";
	res += "</div>";

	res += "<div class=\"row\">";
	res += "<label class=\"col-sm-2 control-label\">Localizacion del dolor</label>";
	res += "<label class=\"col-sm-10 control-label\" style=\"font-weight:bold;\">" + localizacionDolorText + "</label>";
	res += "<label class=\"col-sm-2 control-label\">Hora de aparición</label>";
	res += "<label class=\"col-sm-10 control-label\" style=\"font-weight:bold;\">" + ItemData.OriginalItemData.HoraAparicionDolor + "</label>";
	res += "<label class=\"col-sm-2 control-label\">Localización del infarto</label>";
	res += "<label class=\"col-sm-10 control-label\" style=\"font-weight:bold;\">" + localizacionInfartoText + "</label>";
	res += "</div>";
	res += "<div class=\"row\">";
	res += "<label class=\"col-sm-2 control-label\">Hora de realización ECQ</label>";
	res += "<label class=\"col-sm-1 control-label\" style=\"font-weight:bold;\">" + ItemData.OriginalItemData.HoraECQ + "</label>";
	res += "<label class=\"col-sm-2 control-label\">ECQ</label>";
	res += "<label class=\"col-sm-1 control-label\" style=\"font-weight:bold;\">";
	if (HasPropertyValue(ItemData.OriginalItemData.ECQ)) {
		var imageUrl = ItemData.OriginalItemData.ECQ;
		res += "<img src=\"/Instances/NYRS/Data/Images/Caso/" + imageUrl + "\" style=\"width:120px;height:90px;\" onclick=\"ModalImageView('" + imageUrl + "','ECQ');\" />";
	}
	else {
		res += "-";
	}
	res += "</label>";
	res += "</div>";

	res += "<h5 style=\"clear:both;\">Antecedentes</h5>";
	res += "<div class=\"row\">";
	res += "<label class=\"col-sm-2 control-label\">IAM</label>";
	res += "<label class=\"col-sm-1 control-label\" style=\"font-weight:bold;\">" + radioData[3] + "</label>";
	res += "<label class=\"col-sm-2 control-label\">Patología psiquiátrica</label>";
	res += "<label class=\"col-sm-1 control-label\" style=\"font-weight:bold;\">" + radioData[4] + "</label>";
	res += "<label class=\"col-sm-2 control-label\">Traslado sanitarioprevio</label>";
	res += "<label class=\"col-sm-1 control-label\" style=\"font-weight:bold;\">" + radioData[5] + "</label>";
	res += "<label class=\"col-sm-2 control-label\">Angor</label>";
	res += "<label class=\"col-sm-1 control-label\" style=\"font-weight:bold;\">" + radioData[6] + "</label>";
	res += "</div>";

	res += "<div class=\"row\">";
	res += "<label class=\"col-sm-2 control-label\">Diabetes melitus</label>";
	res += "<label class=\"col-sm-1 control-label\" style=\"font-weight:bold;\">" + radioData[7] + "</label>";
	res += "<label class=\"col-sm-2 control-label\">Hipertensión</label>";
	res += "<label class=\"col-sm-1 control-label\" style=\"font-weight:bold;\">" + radioData[8] + "</label>";
	res += "<label class=\"col-sm-2 control-label\">Insuficiencia cardiaca</label>";
	res += "<label class=\"col-sm-1 control-label\" style=\"font-weight:bold;\">" + radioData[9] + "</label>";
	res += "<label class=\"col-sm-2 control-label\">Ictus</label>";
	res += "<label class=\"col-sm-1 control-label\" style=\"font-weight:bold;\">" + radioData[10] + "</label>";
	res += "</div>";

	res += "<div class=\"row\">";
	res += "<label class=\"col-sm-2 control-label\">Neoplasias</label>";
	res += "<label class=\"col-sm-1 control-label\" style=\"font-weight:bold;\">" + radioData[11] + "</label>";
	res += "<label class=\"col-sm-2 control-label\">Otros</label>";
	res += "<label class=\"col-sm-1 control-label\" style=\"font-weight:bold;\">" + radioData[12] + "</label>";
	res += "<label class=\"col-sm-6\"><strong>" + ItemData.OriginalItemData.TipoAntecedente + "</strong></label>";
	res +="</div>";
	
	$("#Placeholder_DatosClinicosRadio").parent().after(res);
	
}

function INCIDENCIA_ColumnCinetosis(data, row) {
	switch(data)
	{
		case 1: return "Sí";
		case 2: return "No";
		default: return "-";
	}
}

function INCIDENCIA_ColumnUsoDispositivo(data, row) {
	switch(data)
	{
		case 1: return "Sí";
		case 2: return "No";
		default: return "-";
	}
}

function INCIDENCIA_ColumnHora(data, row) {
	console.log(data);
	
	if(data === "00:00:00.0000000") { return "-"; }
	
	var parts = data.split(':');
	if(parts.length> 1){
		return parts[0]+":"+parts[1];
	}
	
	
	return data;
}

var DocumentAddId = null;
function Form_AddDocument(id) {
    if (DocumentAddId !== null) {
        id = DocumentAddId;
        DocumentAddId = null;
	}

    if (id === "Consentimiento") {
        var data  = {
            "casoId": ItemData.OriginalItemData.Id
        }; 
        $.ajax({
            "type": "POST",
            "url": "/Instances/NYRS/Export/ConsentimientoInformado.aspx/Generate",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (msg) {
                console.log(msg);
                window.open(msg.d.ReturnValue);
                PopupWorkingHide();
            },
            "error": function (msg) {
                PopupWorkingHide();
                PopupWarning(msg.responseText);
            }
        });

        return false;
    }

    var preCondition = Document_Create_PreCondition(id);
    if (preCondition.Result === false) {
        PopupWarning(preCondition.ErrorMessage);
        return false;
    }

    switch (id) {
        default:
            Document_Create(preCondition.Url, id);
            break;
    }
}