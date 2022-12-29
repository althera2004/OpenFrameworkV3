console.log("NYRS", "InstanceCommon loaded");

// this function must exists
function InstanceCommonAfterLoad () {
	console.log("InstanceCommonAfterLoad");
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