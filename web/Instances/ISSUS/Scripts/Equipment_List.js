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

function EQUIPMENT_LIST_CostView() {
	GoEncryptedList('Equipment', 'EquipmentCosts', []);
}

function EQUIPMENT_LIST_EquipmentView () {
	GoEncryptedList('Equipment', 'Custom', []);
}

function EQUIPMENT_LIST_RecordFilter(listFilter) {
	var optionsLabels = listFilter.Options.split('|');
	var options = [];
	var count = 1;
	for(var x = 0; x < optionsLabels.length; x++)
	{
		if(optionsLabels[x] !== "") {
			options.push({"Id": count, "Value": optionsLabels[x]});
			count++;
		}
	}
	return "<span><label>Registres:&nbsp;</label>"+SelectChosenMultiple(listFilter.Id, options, 500)+"</span>";
}