function EQUIPMENT_CustomActions() {
	console.log("EQUIPMENT_CustomActions");
	EQUIPMENT_Layout();
	EQUIPMENT_Events();
}

function EQUIPMENT_CUSTOM_FormFillAfter() {
	if(ItemData.OriginalItemData.Id > 0) {
		var recordTypeBin = '000' + dec2bin(ItemData.OriginalItemData.RecordType);
		recordTypeBin = recordTypeBin.substr(recordTypeBin.length-3,3);
		console.log(recordTypeBin);
		
		if(recordTypeBin[0] === '1') { $("#tabSelect-mateniments").show(); }
		if(recordTypeBin[1] === '1') { $("#tabSelect-varificacio").show(); }
		if(recordTypeBin[2] === '1') { $("#tabSelect-calibratge").show(); }
	}
}

function EQUIPMENT_Layout() {
	$("#RecordType").parent().append("<span style=\"clear:both;display:none;\" class=\"errorMessage\" id=\"RecordType_Error_Required\"><br />Com a mínim un tipus de registre</span>");
}

function EQUIPMENT_Events() {
	$(".CK_RecordType").on("click", EQUIPMENT_RecordTypeChanged);
}

function EQUIPMENT_RecordTypeChanged() {
	$("#RecordType_Error_Required").hide();
	var total =0;
	if($("#CK_RecordType_0").prop("checked") === true){
		$("#tabSelect-calibratge").removeClass("TabMustExists");
		$("#tabSelect-calibratge").show();
		total++;
	} else{
		$("#tabSelect-calibratge").hide();
	}
	if($("#CK_RecordType_1").prop("checked") === true){
		$("#tabSelect-varificacio").removeClass("TabMustExists");
		$("#tabSelect-varificacio").show();
		total++;
	} else{
		$("#tabSelect-varificacio").hide();
	}
	if($("#CK_RecordType_2").prop("checked") === true){
		$("#tabSelect-mateniments").removeClass("TabMustExists");
		$("#tabSelect-mateniments").show();
		total++;
	} else{
		$("#tabSelect-mateniments").hide();
	}
	
	if(total === 0) {
		$("#RecordType_Error_Required").show();
	}
}