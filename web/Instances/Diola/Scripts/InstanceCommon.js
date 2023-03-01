console.log("InstanceCommon", "loaded");

// js script file for all instance pages

function CENTRO_CategoriaColumn(data, row) {
	var res = "-";
	title = "";
	
	switch(data) {
		case 0: res = "<i class=\"fa fa-star\"></i><i class=\"fal fa-star\"></i><i class=\"fal fa-star\"></i><i class=\"fal fa-star\"></i><i class=\"fal fa-star\"></i>"; title="1 Estrella"; break;
		case 1: res = "<i class=\"fa fa-star\"></i><i class=\"fa fa-star\"></i><i class=\"fal fa-star\"></i><i class=\"fal fa-star\"></i><i class=\"fal fa-star\"></i>"; title="2 Estrellas"; break;
		case 2: res = "<i class=\"fa fa-star\"></i><i class=\"fa fa-star\"></i><i class=\"fa fa-star\"></i><i class=\"fal fa-star\"></i><i class=\"fal fa-star\"></i>"; title="3 Estrellas"; break;
		case 3: res = "<i class=\"fa fa-star\"></i><i class=\"fa fa-star\"></i><i class=\"fa fa-star\"></i><i class=\"fa fa-star\"></i><i class=\"fal fa-star\"></i>"; title="4 Estrellas"; break;
		case 4: res = "<i class=\"fa fa-star\"></i><i class=\"fa fa-star\"></i><i class=\"fa fa-star\"></i><i class=\"fa fa-star\"></i><i class=\"fa fa-star\"></i>"; title="5 Estrellas"; break;
		case 5: res = "<span style=\"color:#ff6a01\"><i class=\"fa fa-star\"></i><i class=\"fa fa-star\"></i><i class=\"fa fa-star\"></i><i class=\"fa fa-star\"></i><i class=\"fa fa-star\"></i></span>"; title="5 Estrellas superior"; break;
	}
	
	return {
		"data": res,
		"title": title
	};
}