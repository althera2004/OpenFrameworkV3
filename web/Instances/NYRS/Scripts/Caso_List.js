function CASO_ListOnLoad()
{
  console.log("CASO_list Loaded");
}

function CASO_Tipo(data, row) {
	
	if(row["EstudioStatus"] === "NULO") {
		return {
			"data": "<strong style=\"color:#b90909\">Caso nulo</strong>",
			"title": "Nulo"
		}
	}
	
	if(row["Code"][0] === "I") {
		return {
			"data": "<strong style=\"color:#09b909\">Intervención</strong>",
			"title": "Intervención"
		};
	}
	
	return {
		"data": "<strong style=\"color:#333\">Control</strong>",
		"title": "Control"
	};
}

function CASO_PacienteNombreCompleto(data, row){
	var res = data;
	if(row["PacienteApellidos"] !== null) {
		res+= " " + row["PacienteApellidos"];
	}
	
	return res;
}

function CASO_FINIText(data, row) {
	if(typeof data === "undefined" || data === null || data === "") {
		return "";
	}
	
	return data;
}
