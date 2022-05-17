function LEARNINGASSISTANT_CustomActions() { 
	console.log("LEARNINGASSISTANT", "CustomActions Loaded");
	LEARNINGASSISTANT_GenderLayout();
	FieldToLabel("LearningId");
}


function LEARNINGASSISTANT_GenderLayout() {
	var res = "";
	res += "<input type=\"radio\" id=\"RBGender0\" name=\"RBGender\">&nbsp;Home";
	res += "&nbsp;&nbsp;&nbsp;";
	res += "<input type=\"radio\" id=\"RBGender1\" name=\"RBGender\">&nbsp;Dona";
	
	$("#Gender").hide();
	$("#Gender").parent().append(res);
}