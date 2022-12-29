function DOCUMENT_CustomActions() {
	console.log("DOCUMENT_CustomActions");
	DOCUMENT_Layout();
	DOCUMENT_Events();	
}

function DOCUMENT_Layout() {
	console.log("No DOCUMENT layout");
}

function DOCUMENT_Events() {
	$("#RB_Origin_1").on("click", DOCUMENT_OriginChanged);
	$("#RB_Origin_2").on("click", DOCUMENT_OriginChanged);
}

function DOCUMENT_NewRevision() {
	alert("DOCUMENT_NewRevision");
}

function DOCUMENT_ToArchive() {
	alert("DOCUMENT_ToArchive");
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

function DOCUMENT_CUSTOM_FormFillAfter(){
	DOCUMENT_OriginChanged();
}

function DOCUMENT_ToArchive() {
}

function DOCUMENT_NewRevision (){
}