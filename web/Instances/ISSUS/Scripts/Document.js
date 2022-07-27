function DOCUMENT_CustomActions() {
	console.log("DOCUMENT_CustomActions");
	
	$("#RB_Origin_1").on("click", DOCUMENT_OriginChanged);
	$("#RB_Origin_2").on("click", DOCUMENT_OriginChanged);
}

function DOCUMENT_NewRevision() {
	alert("DOCUMENT_NewRevision");
}

function DOCUMENT_OriginChanged() {
	if($("#RB_Origin_1").prop("checked") === true){
		$("#DocumentProcedenciaId_Label").invisible();
		$("#DocumentProcedenciaId").parent().invisible();
	}
	if($("#RB_Origin_2").prop("checked") === true){
		$("#DocumentProcedenciaId_Label").visible();
		$("#DocumentProcedenciaId").parent().visible();
	}
}


function DOCUMENT_FormFillAfter(){
	DOCUMENT_OriginChanged();
}