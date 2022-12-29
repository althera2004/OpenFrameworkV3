var Feature_ContactPersonData = [];

function Feature_ContactPerson_RenderTab() {
    var res = "";
    res += "<div class=\"row\">";
    res += "  <h4 clasS=\"col-xs-8\">" + Dictionary.Feature_ContactPerson_TabTitle + "</h4>";
    res += "  <div class=\"col-xs-4\" style=\"text-align:right;padding:6px;\">";
    res += "    <a href=\"#\" id=\"Feature_ContactPerson_BtnAdd\" onclick=\"Feature_ContactPerson_Add();\"><i class=\"fa fa-plus\"></i>&nbsp;" + Dictionary.Item_ContactPerson_Btn_New + "</a>";
    res += "  </div>";
    res += "</div>";
    res += "<div class=\"tableHead\">";
    res += "  <table cellpadding=\"1\" cellspacing=\"1\" class=\"table\">";
    res += "    <thead id=\"Feature_ContactPerson_ListHead\">";
    res += "      <tr>";
    res += "        <th id=\"tha0\" onclick=\"Feature_ContactPerson_List_Sort(0);\" class=\"sort search ASC\"> Nom</th>";
    res += "        <th id=\"tha1\" onclick=\"Feature_ContactPerson_List_Sort(1);\" style=\"width: 150px;\" class=\"sort search\"> Posició</th>";
    res += "        <th id=\"tha2\" onclick=\"Feature_ContactPerson_List_Sort(2);\" style=\"width: 100px;\" class=\"sort search\"> Telèfon</th>";
    res += "        <th id=\"tha3\" onclick=\"Feature_ContactPerson_List_Sort(3);\" style=\"width: 100px;\" class=\"sort search\"> Mòvil</th>";
    res += "        <th id=\"tha3\" onclick=\"Feature_ContactPerson_List_Sort(3);\" style=\"width: 200px;\" class=\"sort search\"> Email</th>";
    res += "        <th style=\"width: 117px;\">&nbsp;</th>";
    res += "    </thead>";
    res += "  </table>";
    res += "</div>";
    res += "<div class=\"panel-body panel-body-list\" style=\"\">";
    res += "  <div class=\"table-responsive\">";
    res += "    <div class=\"table-body\" style=\"max-height:100%;height:100%\">";
    res += "      <table class=\"table\" style=\"max-height:100%\">";
    res += "        <tbody style=\"max-height:100%\" id=\"Feature_ContactPerson_ListBody\"></tbody>";
    res += "      </table>";
    res += "    </div>";
    res += "  </div>";
    res += "</div>";
    return res;
}

function Feature_ContactPerson_Retrieve() {
    var data = {
        "itemDefinitionId": ItemDefinition.Id,
        "itemId": ItemData.OriginalItemData.Id,
        "instanceName": Instance.Name
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/ItemService.asmx/ItemContactPerson",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            eval("Feature_ContactPersonData = " + msg.d + ";");
            console.log(Feature_ContactPersonData);
            Feature_ContactPerson_FillTable();
        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}

function Feature_ContactPerson_FillTable() {
    var list = "";
    for (var x = 0; x < Feature_ContactPersonData.length; x++) {
        list += Feature_ContactPerson_RenderRow(Feature_ContactPersonData[x]);
    }

    $("#Feature_ContactPerson_ListDataCount").html(Feature_ContactPersonData.length);
    $("#Feature_ContactPerson_ListBody").html(list);
}

function Feature_ContactPerson_RenderRow(data) {
    var res = "";
    res += "<tr>";
    res += "<td>" + data.FullName + "</td>";
    res += "<td style=\"width:150px;\">" + FixedLists["JobPosition"][data.JobPosition] + "</td>";
    res += "<td style=\"width:100px;\">" + data.Phone + "</td>";
    res += "<td style=\"width:100px;\">" + data.Mobile + "</td>";
    res += "<td style=\"width:200px;\">" + data.Email + "</td>";
    res += "<td class=\"action-buttons\" style=\"width:100px;text-align:center;\">";
    res += "<a class=\"RowButtonAction blue ace-icon fal fa-eye bigger-120\" id=\"" + data.Id + "\" onclick=\"Feature_Attach_View(" + data.Id + ")\" title=\"" + Dictionary.Common_View + "\"></a>";
    res += "&nbsp;"
    res += "<a class=\"RowButtonAction red ace-icon fal fa-times bigger-120\" id=\"" + data.Id + "\" onclick=\"\" title=\"" + Dictionary.Common_Delete + "\"></a>";
    res += "</td>";
    res += "</tr>";
    return res;
}

function Feature_ContactPerson_ById(id) {
    var res = attachs.filter(a => a.Id === id);
    if (res.length > 0) {
        return res[0];
    }

    return null;
}

function Feature_ContactPerson_List_Sort(index) {
    alert(index);
}