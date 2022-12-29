function Feature_Attachs_RenderTab() {
    var res = "";
    res += "<div class=\"row\">";
    res += "  <h4 clasS=\"col-xs-8\">Adjunts</h4>";
    res += "  <div class=\"col-xs-4\" style=\"text-align:right;padding:6px;\">";
    res += "    <a href=\"#\" id=\"Feature_Attachhs_BtnAdd\" onclick=\"Feature_Attachs_LaunchPopup();\"><i class=\"fa fa-plus\"></i>&nbsp;" + Dictionary.Item_Attachment_Btn_New + "</a>";
    res += "  </div>";
    res += "</div>";
    res += "<div class=\"tableHead\">";
    res += "  <table cellpadding=\"1\" cellspacing=\"1\" class=\"table\">";
    res += "    <thead id=\"Feature_Attachs_ListHead\">";
    res += "      <tr>";
    res += "        <th id=\"tha0\" onclick=\"Feature_Attach_List_Sort(0);\" class=\"sort search ASC\"> Document</th>";
    res += "        <th id=\"tha1\" onclick=\"Feature_Attach_List_Sort(1);\" style=\"width: 100px;\" class=\"sort search\"> Data</th>";
    res += "        <th id=\"tha2\" onclick=\"Feature_Attach_List_Sort(2);\" style=\"width: 100px;\" class=\"sort search\"> Mida</th>";
    res += "        <th id=\"tha3\" onclick=\"Feature_Attach_List_Sort(3);\" style=\"width: 200px;\" class=\"sort search\"> Autor</th>";
    res += "        <th style=\"width: 117px;\">&nbsp;</th>";
    res += "    </thead>";
    res += "  </table>";
    res += "</div>";
    res += "<div class=\"panel-body panel-body-list\" style=\"\">";
    res += "  <div class=\"table-responsive\">";
    res += "    <div class=\"table-body\" style=\"max-height:100%;height:100%\">";
    res += "      <table class=\"table\" style=\"max-height:100%\">";
    res += "        <tbody style=\"max-height:100%\" id=\"Feature_Attachs_ListBody\"></tbody>";
    res += "      </table>";
    res += "    </div>";
    res += "  </div>";
    res += "</div>";
    return res;
}

var DropZone_context = {
    "exists": false,
    "files": [],
    "component": null
};

function Feature_Attachs_LaunchPopup() {
    $("#Feature_Attach_Upload_ItemDefintionId").val(ItemDefinition.Id);
    $("#Feature_Attach_Upload_ItemId").val(ItemData.ActualData.Id);
    $("#dropzone").attr("action", "/Async/FeatureAttachUpload.aspx?i=" + ItemData.ActualData.Id + "&id=" + ItemDefinition.Id);

    $("#dropzone").addClass("dropzone");
    if (DropZone_context.exists === false) {
        var previewNode = document.querySelector("#template");
        previewNode.id = "";
        var previewTemplate = previewNode.parentNode.innerHTML;
        previewNode.parentNode.removeChild(previewNode);

        DropZone_context.files = [];
        DropZone_context.component = new Dropzone("#dropzone",
            {
                "url": "/Async/FeatureAttachUpload.aspx",
                "paramName": "file", // The name that will be used to transfer the file
                "maxFilesize": 2, // MB
                "maxFiles": 20,
                "parallelUploads": 20,
                "thumbnailWidth": 50,
                "thumbnailHeight": 50,
                "previewTemplate": previewTemplate,
                "previewsContainer": "#previews",
                "uploadMultiple": false,
                "dictFileTooBig": Dictionary.Feature_Attach_Message_TooBigSize,
                //"acceptedFiles": "image/jpeg, image/png",
                "addRemoveLinks": true,
                "autoProcessQueue": true,
                "accept": function (file, done) {
                    if (file.name.indexOf(".zip") != -1) {
                        done("Naha, you don't.");
                    }
                    else { done(); }
                }
            });
        DropZone_context.exists = true;

        DropZone_context.component.on("addedfile", file => {
            console.log(file);
            DropZone_context.files.push(file);
            $("#PopupUploadAttacheBtnOk").show();
            $(".dz-success .fa-check").hide()
        });

        DropZone_context.component.on("sending", function (file, xhr, formData) {
            formData.append('instanceName', Instance.Name);
            formData.append('companyId', Company.Id);
            formData.append('applicationUserId', ApplicationUser.Id);
            formData.append('itemDefinitionId', ItemDefinition.Id);
            formData.append('itemId', ItemData.OriginalItemData.Id);
        });

        DropZone_context.component.on("removedfile", file => {
            console.log(file);
            DropZone_context.files = DropZone_context.files.filter(f => f !== file);
            if (DropZone_context.files.length === 0) {
                $("#PopupUploadAttacheBtnOk").hide();
            }
        });

        DropZone_context.component.on("complete", file => {
            console.log("completed", file);
            $(".dz-success .fa-check").show();
            $(".dz-error .fa-check").hide();
            Feature_Attach_Retrieve();
        });

        $("#BtnSend").on("click", function () {
            console.log(DropZone_context.files);
        });
    }

    $("#LauncherPopupUploadAttach").click();
    $("#previews").html("");
}

function Feature_Attachs_Go() {
    alert(DropZone_context.files.length);

    var queued = DropZone_context.files.filter(f => f.status === "queued");
    for (var x = 0; x < queued.length; x++) {
        DropZone_context.component.processFile(queued[x]);
    }
}

function Feature_Attach_Retrieve() {
    var data = {
        "itemDefinitionId": ItemDefinition.Id,
        "itemId": ItemData.OriginalItemData.Id,
        "instanceName": Instance.Name
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/ItemService.asmx/ItemAttachs",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            eval("attachs = " + msg.d + ";");
            console.log(attachs);
            Feature_Attachment_FillTable();
        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}

function Feature_Attachment_FillTable() {
    var list = "";
    var grid = "";
    for (var x = 0; x < attachs.length; x++) {
        list += Feature_Attachment_RenderRow(attachs[x]);
        //grid += AttachDocumentTag(attachs[x]);
    }

    if (attachs.length === 0) {
        grid += "<img src=\"/img/arrowup-" + ApplicationUser.Language.ISO + ".png\" style=\"float:right;\" />";
    }

    $("#Feature_Attachment_ListDataCount").html(attachs.length);
    $("#Feature_Attachs_ListBody").html(list);
    $("#Feature_Attachment_Grid").html(grid);
}

function Feature_Attachment_RenderRow(attach) {
    var res = "";
    res += "<tr>";
    res += "<td>" + attach.FileName + "</td>";
    res += "<td style=\"width:100px;\">" + attach.CreatedOn + "</td>";
    res += "<td style=\"width:100px;text-align:right\">" + FormatBytes(attach.Size, decimals = 2) + "</td>";
    res += "<td style=\"width:200px;\">" + attach.CreatedBy.Value + "</td>";
    res += "<td class=\"action-buttons\" style=\"width:100px;text-align:center;\">";
    res += "<a class=\"RowButtonAction blue ace-icon fal fa-eye bigger-120\" id=\"" + attach.Id + "\" onclick=\"Feature_Attach_View(" + attach.Id + ")\" title=\"" + Dictionary.Common_View + "\"></a>";
    res += "&nbsp;"
    res += "<a class=\"RowButtonAction green ace-icon fal fa-copy bigger-120\" id=\"" + attach.Id + "\" onclick=\"\" title=\"" + Dictionary.Common_Copy + "\"></a>";
    res += "&nbsp;"
    res += "<a class=\"RowButtonAction red ace-icon fal fa-times bigger-120\" id=\"" + attach.Id + "\" onclick=\"\" title=\"" + Dictionary.Common_Delete + "\"></a>";
    res += "</td>";
    res += "</tr>";
    return res;
}

function Feature_Attach_View(id) {
    var attach = Feature_Attach_ById(id);
    if (attach !== null) {
        window.open("/Instances/" + Instance.Name + "/Data/" + ItemDefinition.ItemName + "/" + ItemData.OriginalItemData.Id + "/Attachs/" + attach.FileName);
    }
}

function Feature_Attach_ById(id) {
    var res = attachs.filter(a => a.Id === id);
    if (res.length > 0) {
        return res[0];
    }

    return null;
}

function Feature_Attach_List_Sort(index){
    alert(index);
}