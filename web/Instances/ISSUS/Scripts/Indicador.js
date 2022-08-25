function INDICADOR_CustomActions() {
	console.log("INDICADOR_CustomActions");
	INDICADOR_Events();
}

function INDICADOR_RenderSemaphore() {
	var meta = $("#Meta").val() *1;
	var metaValue = $("#MetaValue").val();
	var alarma = $("#Alarma").val();
	var alarmaValue = $("#AlarmaValue").val();
	var res = "";
	if(meta === 0){
		res += "<table style=\"width:100%;\">";
		res += "  <tbody>";
		res += "    <tr>";
		res += "      <td style=\"width:11%\">&nbsp;</td>";
		res += "      <td><i class=\"fa fa-info-circle\"></i>&nbsp;<i>S' ha de especificar la meta</i></td>";
		res += "      <td style=\"width:11%\">&nbsp;</td>";
		res += "    </tr>";
		res += "  </tbody>";
		res += "</table>";
	}
	else {
		
		var existsAlarma = false;
		var width = 33;
		
		var metaText = FixedLists["IndicadorMeta"][meta];
		var alarmaText = "";
		
		if(alarma !== null && alarma > 0){
			existsAlarma = true;
			width = 26;
			alarmaText = FixedLists["IndicadorAlarma"][alarma * 1];
		}
		
		res += "<table style=\"width:100%;\">";
		res += "  <tbody>";
		res += "    <tr>";
		res += "      <td style=\"width:11%\" rowspan=\"2\">&nbsp;</td>";
		if(existsAlarma === true)
		{
			res += "      <td style=\"text-align:center;width:26%;\" id=\"celldangerLabel\">Alarma</td>";
		}
		res += "      <td style=\"text-align:center;width:"+ width +"%;\">Meta no assolida</td>";
		res += "      <td style=\"text-align:center;width:"+ width +"%;\">Meta assolida</td>";
		res += "      <td style=\"width:11%\" rowspan=\"2\">&nbsp;</td>";
		res += "    </tr>";
		res += "    <tr style=\"height:25px;\">";
		if(existsAlarma === true)
		{
			res += "      <td style=\"text-align:center;width:26%;\" class=\"btn-danger\" id=\"celldanger\">"+alarmaText+" "+$("#AlarmaValue").val()+"</td>";
		}
		res += "      <td style=\"text-align:center;width:"+ width +"%;\" class=\"btn-warning\" id=\"cellwarning\">&nbsp;</td>";
		res += "      <td style=\"text-align:center;width:"+ width +"%;\" class=\"btn-success\" id=\"cellsuccess\">"+metaText+" "+$("#MetaValue").val()+"</td>";
		res += "    </tr>";
		res += "  </tbody>";
		res += "</table>";
	}
	$("#Placeholder_metagraphic").html(res);
}

function INDICADOR_SetProcessTypeLabel() {
	var processId = $("#ProcessId").val() * 1;
	var process = GetFKById("Process", processId);
	var res = "";
	if(typeof process !== "undefined" && process !== null) {
		if(HasPropertyValue(process.ProcessTypeId)) {
			var processType = GetFKById("ProcessType", process.ProcessTypeId);
			if(processType !== null) {
				res = processType.Description;
			}
		}
	}
	
	$("#ProceesTypeLabel").html(res);
}

function INDICADOR_Events() {
	$("#ProcessId").on("change", INDICADOR_SetProcessTypeLabel);
	
	$("#Meta").on("change", INDICADOR_RenderSemaphore);
	$("#MetaValue").on("change", INDICADOR_RenderSemaphore);
	$("#Alarma").on("change", INDICADOR_RenderSemaphore);
	$("#AlarmaValue").on("change", INDICADOR_RenderSemaphore);
}

function INDICADOR_CUSTOM_AfterRender() {
	
	$("#Periodicity").css("width", "50%");
	$("#Periodicity").css("float", "left");
	$("#Periodicity").after("&nbsp;díes");
	
	$("#ProcessId").after("<span id=\"ProceesTypeLabel\" class=\"subText\"></span>");
	
	INDICADOR_RenderSemaphore();
}

function INDICADOR_CUSTOM_FormFillAfter() {
	INDICADOR_SetProcessTypeLabel();
	INDICADOR_RenderSemaphore();
}