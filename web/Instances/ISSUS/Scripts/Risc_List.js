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
	var res = data;
	
	if(row.E === 3) {
		res = {
			"data": "<strong class=\"red\">"+data+"</strong>",
			"title": ""
		};
	}
	else if(row.E === 2) {
		res = {
			"data": "<strong>"+data+"</strong>",
			"title": ""
		};
	}
	
	return res;	
}