function RISC_CustomActions() {
	console.log("RISC_CustomActions");
	RISC_Layout();
	RISC_Events();
}

function Probability_Slider_Changed() {
	RISC_SetResult();
}

function Severity_Slider_Changed() {
	RISC_SetResult();
}

function RISC_SetResult(){
	var result = $("#Probability").val() * $("#Severity").val();
	var normaId = $("#NormaId").val() * 1;
	var norma = GetFKById("Norma", normaId);
	
	$("#Result").val(result);
	ItemData.UpdateData("Result", result);
	$("#Result_Labeled").html($("#Result").val());
	
	if(typeof norma !== "undefined" && norma !== null) {	
		if(norma.IPR*1 <= result) {
			$("#Result_Labeled").css("color", "red");
			$("#ApplyAction_No_Label").html("Assumir risc");
		}
		else { 
			$("#Result_Labeled").css("color", "green");
			$("#ApplyAction_No_Label").html("No");
		}
		 
		 $("#IPR").html("IPR actual:&nbsp;<span style=\"font-size:18px;font-weight:bold;\">" + norma.IPR + "</span>");
		 RISC_SetAssumed();
	}
}

function RISC_SetAssumed() {
	$("#Assumed").prop("checked", false);
	
	// 1.- IPR superado
	var result = $("#Probability").val() * $("#Severity").val();
	var normaId = $("#NormaId").val() * 1;
	var norma = GetFKById("Norma", normaId);
	if(norma !== null) {	
		if(norma.IPR * 1 <= result) {
			if($("#ApplyAction_No").prop("checked") === true){
				$("#Assumed").prop("checked", true);
			}
		}
	}
	
}

function RISC_Layout() {
	FormFieldHide("Assumed");
	FieldToLabel("Result");
	$("#Result_Labeled").css("font-size", "18px").css("font-weight", "bold");
	$("#ApplyAction_Label").replaceClass("col-sm-1","col-sm-2");
	$("#ApplyAction_No").parent().replaceClass("col-sm-11","col-sm-10");
	$("#IPR").html("IPR actual:&nbsp;<span style=\"font-size:18px;font-weight:bold;\">-</span>");
}

function RISC_Events() {
	$("#NormaId").on("change", RISC_SetResult);
	$("#ApplyAction_No").on("click", RISC_SetAssumed);
	$("#ApplyAction_Yes").on("click", RISC_SetAssumed);
}