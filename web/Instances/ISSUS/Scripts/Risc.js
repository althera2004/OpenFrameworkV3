function RISC_CustomActions() {
	console.log("RISC_CustomActions");
}

function Probability_Slider_Changed() {
	RISC_SetResult();
}

function Severity_Slider_Changed() {
	RISC_SetResult();
}

function RISC_SetResult(){
	var result = $("#Probability").val()*$("#Severity").val();
	var normaId = $("#NormaId").val()*1;
	var norma = GetFKById("Norma", normaId);
	
	$("#Result").val(result);
	
	if(norma !== null) {	if(norma.IPR*1 <= result) { $("#Result").css("backgroundColor", "red").css("color", "yellow"); }
		 else { $("#Result").css("backgroundColor", "green").css("color", "white"); }
		 
		 $("#IPR").html(norma.IPR);
	}
		
	
}