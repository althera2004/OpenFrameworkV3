window.onload = function () {

    $("#FooterStatus").html("");
    $("#FormBtnDelete").remove();

    $("#FormBtnSave").before("<button class=\"btn btn-info\"><i class=\"fa fa-recycle\"></i> Recarregar diccionary</button>");
    $("#FormBtnSave").before("<button class=\"btn btn-info\"><i class=\"fa fa-download\"></i> Descarregar fitxer</button>");

    ResizeWorkArea();
    GetDictionaryData();
};

window.onresize = function () {
    ResizeWorkArea();
};

function ResizeWorkArea() {
    $(".table-body").height($(window).height() - 337);
}

function GetDictionaryData() {
    var data = {
        "language": $("#CmbLanguage").val(),
        "instanceName": Instance.Name
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/InstanceService.asmx/DictionaryGetCorpus",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            var responseData = eval(response.d);

            if (responseData.Success == true) {
                console.log(responseData.ReturnValue);
                eval("CorpusData = " + responseData.ReturnValue + ";");
                RenderDictionaryData();
            }

            console.log(CorpusData);
        }
    });
}

function RenderDictionaryData() {
    /*var corpusCommon = CorpusData.filter(item => item.Key.startsWith('Common_'));
    var res = "";
    for (var cc = 0; cc < corpusCommon.length; cc++) {
        res += "<tr><td>" +corpusCommon[cc].Key + "</td><td>" + corpusCommon[cc].Value + "</td></tr>";
    }

    $("#CorpusCommon_ListBody").html(res);
    $("#CorpusCommon_ListBody").show();
    $("#CorpusCommon_ListCount").html(corpusCommon.length);*/
    RenderDiactionaryDataSection("Common");
    RenderDiactionaryDataSection("Core");
    RenderDiactionaryDataSection("Billing");
    RenderDiactionaryDataSection("Feature");
}

function RenderDiactionaryDataSection(section) {
    $("#Corpus" + section + "_ListBody").html("");
    var corpusCommon = CorpusData.filter(item => item.Key.startsWith(section + '_'));
    //var res = "";
    //for (var cc = 0; cc < corpusCommon.length; cc++) {
    //    res += "<tr><td>" + corpusCommon[cc].Key + "</td><td>" + escapeHTML(corpusCommon[cc].Value) + "</td></tr>";
    //}

    var target = document.getElementById("Corpus" + section + "_ListBody");
    for (var cc = 0; cc < corpusCommon.length; cc++) {
        //res += "<tr><td>" + corpusCommon[cc].Key + "</td><td>" + escapeHTML(corpusCommon[cc].Value) + "</td></tr>";

        var tr = document.createElement("TR");
        var tdKey = document.createElement("TD");
        var tdValue = document.createElement("TD");
        tdKey.appendChild(document.createTextNode(corpusCommon[cc].Key));
        tdValue.appendChild(document.createTextNode(corpusCommon[cc].Value));
        tr.appendChild(tdKey);
        tr.appendChild(tdValue);
        target.appendChild(tr);

    }
    //$("#Corpus" + section +"_ListBody").html(res);
    $("#Corpus" + section +"_ListBody").show();
    $("#Corpus" + section +"_ListCount").html(corpusCommon.length);
}