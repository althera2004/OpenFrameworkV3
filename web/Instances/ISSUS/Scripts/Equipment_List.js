function Equipment_Name_Column (data, row){
	return row.Code +" - " + data;
}

function EQUIPMENT_RecordTypeFilter() {
	var res = 0;
	if($("#Filter_RecordType_0").prop("checked") === true) { res +=1;}
	if($("#Filter_RecordType_1").prop("checked") === true) { res +=2;}
	if($("#Filter_RecordType_2").prop("checked") === true) { res +=4;}	
	return res === 0 ? null : res.toString();
}