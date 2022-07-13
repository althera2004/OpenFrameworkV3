function INDICADOR_CustomActions() {
	console.log("INDICADOR_CustomActions");
	INDICADOR_FormLayout();
	INDICADOR_RenderSemaphore();
}

function INDICADOR_RenderSemaphore() {
	var res = "<table style=\"width:100%;\">";
    res += "  <tbody>";
	res += "    <tr>";
    res += "      <td style=\"width:11%\" rowspan=\"2\">&nbsp;</td>";
    res += "      <td style=\"text-align:center;width:26%;\" id=\"celldangerLabel\">Alarma</td>";
    res += "      <td style=\"text-align:center;width:26%;\">Meta no assolida</td>";
    res += "      <td style=\"text-align:center;width:26%;\">Meta assolida</td>";
    res += "      <td style=\"width:11%\" rowspan=\"2\">&nbsp;</td>";
    res += "    </tr>";
    res += "    <tr style=\"height:25px;\">";
    res += "      <td style=\"text-align:center;width:26%;\" class=\"btn-danger\" id=\"celldanger\">menor que (&lt;) 1,00</td>";
    res += "      <td style=\"text-align:center;width:26%;\" class=\"btn-warning\" id=\"cellwarning\">&nbsp;</td>";
    res += "      <td style=\"text-align:center;width:26%;\" class=\"btn-success\" id=\"cellsuccess\">major que (&gt;) 5,00</td>";
    res += "    </tr>";
    res += "  </tbody>";
	res += "</table>";
	$("#Placeholder_metagraphic").html(res);
}

function INDICADOR_FormLayout() {
	$("#Periodicity").css("width", "50%");
	$("#Periodicity").css("float", "left");
	$("#Periodicity").after("&nbsp;díes");
}