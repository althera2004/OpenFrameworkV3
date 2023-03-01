function DOCUMENT_List_CustomActions(){
	
}

function DOCUMENT_List_Print(){
	var ids = [];
	for(var x = 0; x < ListSources[0].FilteredData.length; x++) {
		ids.push(ListSources[0].FilteredData[x].Id);
	}
	
	console.log(ids);
	console.log($("#nav-search-input").val());
	console.log(ListSources[0].ExtraFilter);
}

function DOCUMENT_Columnlocation (data, row) {
	console.log(row);
	if(row.Document !== "") {
		var res = {
			"data": "<a href=\"/Instances/ISSUS/Data/Document/"+row.Id+"/"+row.Document+"?" + guid() + "\" target=\"_blank\">" + data + "</a>",
			"title": data 
		};
		return res;
	}
	
	return data;
}