function RISC_EstatColumn(data, row) {
	var res = {
		"data": data,
		"title": ""
	};
	
	switch(data) {
		case 1: res.data = "<i class=\"fa fa-circle green\"></i>"; res.title = "No significatiu"; break;
		case 2: res.data = "<i class=\"fa fa-circle orange\"></i>"; res.title = "Assumit"; break;
		case 3: res.data = "<i class=\"fa fa-circle red\"></i>"; res.title = "Significatiu"; break;
	}
	
	return res;	
}

function RISC_ResultColumn(data, row) {
	console.log(data , row);
	var res = data;
	
	if(row.E === 3) {
		res = {
			"data": "<strong class=\"red\">"+data+"</strong>",
			"title": "Significatiu"
		};
	}
	else if(row.E === 2) {
		res = {
			"data": "<strong class=\"orange\">"+data+"</strong>",
			"title": "Assumit"
		};
	} else {
		res = {
			"data": "<strong class=\"green\">"+data+"</strong>",
			"title": "No significatiu"
		};
	}
	
	return res;	
}

function RISC_CUSTOM_AfterFill() {
	console.log("RISC_CUSTOM_AfterFill");
	$.each($("#Risc_Custom_ListBody tr"), function(a,i) {
		var cell = $(i).children()[0];
		if($(cell).data("orderdata") === 1)
		{
			$(i).css("backgroundColor", "#f0f9f2");
		}
		if($(cell).data("orderdata") === 2)
		{
			$(i).css("backgroundColor", "#f9f0f2");
		}
		if($(cell).data("orderdata") === 3)
		{
			$(i).css("backgroundColor", "#f9f0f2");
		}
	});
}