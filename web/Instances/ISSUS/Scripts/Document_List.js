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

function DOCUMENT_FilterList() {
    var DocumentCategoryId = $("#Filter_DocumentCategoryId").val() * 1;
    var OriginId = $("#Filter_Origin").val() * 1;
	var active = $("#FInactivate_0").prop("checked");
	var inactive = $("#FInactivate_1").prop("checked");
	
	ListSources[0]["ExtraFilter"] = [];
	if(DocumentCategoryId > 0) { 
		ListSources[0]["ExtraFilter"].push({ "Field": "DocumentCategoryId", "Subfield": "Id", "Value": DocumentCategoryId });
	}
	
	// if(OriginId > 0) { 
		// ListSources[0]["ExtraFilter"].push({ "Field": "Origin", "Value": OriginId });
	// }
	
	if($("#Origin_1").prop("checked") === true && $("#Origin_2").prop("checked") === false) {
		ListSources[0]["ExtraFilter"].push({ "Field": "Origin", "Value": 1 });
	}
	
	if($("#Origin_1").prop("checked") === false && $("#Origin_2").prop("checked") === true) {
		ListSources[0]["ExtraFilter"].push({ "Field": "Origin", "Value": 2 });
	}
	
	if(active == false || inactive == false){
		if(active === true){
			ListSources[0]["ExtraFilter"].push({ "Field": "FInactivate", "Value": "ISNULL" });
		}
		if(inactive === true){
			ListSources[0]["ExtraFilter"].push({ "Field": "FInactivate", "Value": "NOTNULL" });
		}
	}
	
	/*if(all === true) {		
		$(".widget-title").html("Todos los contratos")
	}
	else{		
        listSources[0]["ExtraFilter"].push({ "Field": "FFIN", "Value": "ISNULL" });
		$(".widget-title").html("Contratos actuales")
    }

    if (almacenId > 0) {
        listSources[0]["ExtraFilter"].push({ "Field": "AlmacenId", "Subfield": "Id", "Value": almacenId });
    }*/
	console.log(ListSources[0]["ExtraFilter"]);
	SearchList();
}