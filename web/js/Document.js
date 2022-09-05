var PopupUploadFile_Context = {
    "itemName": "",
    "fieldName": "",
    "itemId": "",
    "mode": null
};

function FieldDocument_View(e) {
    var fieldName = FieldDocument_GetFieldName(e);
    var value = FieldDocument_GetValue(fieldName);
    if (value !== null && value !== "") {
        alert("FieldDocument_View " + fieldName);
    }
}

function FieldDocument_Upload(e) {
    var fieldName = FieldDocument_GetFieldName(e);
    UploadFile(fieldName, "field", false, false, false, null);
}

function FieldDocument_Delete(e) {
    var fieldName = FieldDocument_GetFieldName(e);
    var value = FieldDocument_GetValue(fieldName);
    if (value !== null && value !== "") {
        alert("FieldDocument_Delete " + fieldName);
    }
}

function FieldDocument_Sign(e) {
    var fieldName = FieldDocument_GetFieldName(e);
    var value = FieldDocument_GetValue(fieldName);
    if (value !== null && value !== "") {
        alert("FieldDocument_Sign " + fieldName);
    }
}

function FieldDocument_History(e) {
    console.log(e);
}

function FieldDocument_GetFieldName(e) {
    var sender = e.target;
    if ($(sender).hasClass("fa")) {
        sender = sender.parentNode;
    }
    var fieldName = sender.id.substr(0, sender.id.lastIndexOf('_'));
    return fieldName;
}

function FieldDocument_GetValue(fieldName) {
    if (typeof ItemData.ActualData[fieldName] !== "undefined" && ItemData.ActualData[fieldName] !== null) {
        return ItemData.ActualData[fieldName];
    }

    return null;
}


function UploadFileInputChanged() {
    // Reset new file layout


    $("#PopupUploadFileError").html("");
    $("#PopupUploadFileBtnOk").hide();
    $("#PopupUploadFileNewFile").html("<i>" + Dictionary.Common_None_Male + "<i>");
    $("#PopupUploadFileNewLength").html("");
    $("#PopupUploadFileNewType").html("");
    $("#PopupUploadFileNewFile").html("&nbsp;<i class=\"fa fa-cog fa-spin\"></i>&nbsp;Verificant fitxer");

    if (!$("#PopupUploadFileInput").val()) //check empty input filed
    {
        $("#PopupUploadFileError").html(Dictionary.Common_Error_NoFileSelected);
        //PopupWarning(Dictionary.Common_Error_NoFileSelected);
        return false;
    }

    $("#PopupUploadFileNewFile").html($("#PopupUploadFileInput")[0].files[0].name);
    var fsize = $("#PopupUploadFileInput")[0].files[0].size;
    var ftype = $("#PopupUploadFileInput")[0].files[0].type;
    if (ftype === "") {
        ftype = "<i>" + Dictionary.Common_Upload_UnknowType + "</i>";
    }

    var ok = true;
    if (fsize > 1024 * 1024 * 15) {
        ok = false;
        $("#PopupUploadFileNewLength").html(FormatBytes(fsize, 2) + " <span style=\"color:#f00\">" + Dictionary.Common_MaximumUploadSize + " " + FormatBytes(1024 * 1024 * 15, 2) + "</span>");
    }
    else {
        $("#PopupUploadFileNewLength").html(FormatBytes(fsize, 2));
    }

    //allow only valid image file types 
    switch (ftype) {
        case "text/plain":
        case "image/png":
        case "image/jpeg":
        case "image/pjpeg":
        case "image/gif":
        case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
        case "application/pdf":
        case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
        case "application/vnd.ms-excel":
        case "application/vnd.openxmlformats-officedocument.wordprocessingml.template":
        case "application/vnd.oasis.opendocument.text":
        case "application/vnd.oasis.opendocument.spreadsheet":
        case "application/vnd.oasis.opendocument.graphics":
        case "application/msword":
        case "message/rfc822":
            $("#PopupUploadFileNewType").html(ftype);
            break;
        default:
            $("#PopupUploadFileNewType").html(ftype + " <span style=\"color:#f00\">" + Dictionary.Common_Warning_MimeType + "</span>");
            ok = false;
            return false;
    }

    if (ok === true) {
        $("#PopupUploadFileBtnOk").show();
    }
}

function UploadFile(fieldName, mode, signature, historial, gallery, callback) {
    $("#PopupUploadFileInput").val("");
    $("#PopupUploadFileError").html("");
    $("#PopupUploadFileBtnOk").html("<i class=\"ace-icon fal fa-upload\"></i>&nbsp;" + Dictionary.Common_Btn_UploadDocument);
    $("#UploadFileSignatureWarning").hide();
    if (ItemData.OriginalItemData.Id) {
        if (mode === "field") {
            $("#PopupUploadPreviousFileTitle").show();
            $("#PopupUploadPreviousFile").show();

            if (signature === true) {
                $("#UploadFileSignatureWarning").show();
            }
        }
        else {
            $("#PopupUploadPreviousFileTitle").hide();
            $("#PopupUploadPreviousFile").hide();
        }

        $("#PopupUploadFileInfo").hide();
        $("#PopupUploadFileBtnOk").hide();
        $("#PopupUploadFileBtnOk").enable();
        $("#PopupUploadFileNewFile").html("");
        $("#PopupUploadFileNewLength").html("");
        $("#PopupUploadFileNewType").html("");

        $("#PopupUploadFileInput").on("change", UploadFileInputChanged);
        $("#PopupUploadFileInput").on("close", function () { alert("close"); });

        var description = "";
        PopupUploadFile_Context.fieldName = fieldName;
        PopupUploadFile_Context.itemName = ItemDefinition.ItemName;
        PopupUploadFile_Context.itemId = ItemData.OriginalItemData.Id;
        PopupUploadFile_Context.mode = mode;
        PopupUploadFile_Context.signature = typeof signature === "undefined" ? false : signature;
        PopupUploadFile_Context.historial = typeof historial === "undefined" ? false : historial;
        PopupUploadFile_Context.gallery = typeof gallery === "undefined" ? false : gallery;

        var field = GetFieldDefinition(fieldName, ItemDefinition);
        var title = Dictionary.Common_Upload_Popup_Title;
        var fieldLabel = fieldName + ": ";
        var fieldValueText = $("#" + fieldName).val();

        if (typeof fieldValueText === "undefined" || fieldValueText === null || fieldValueText === "") {
            fieldValueText = "<i>" + Dictionary.Common_None_Male + "</i>";
        }
        else {
            var extension = "";
            var parts = fieldValueText.split('.');
            if (parts.length > 1) {
                extension = parts[parts.length - 1];
            }

            fieldValueText = "<strong>" + fieldValueText + "</strong>";
            GetFileAttributes(PopupUploadFile_Context.itemName, fieldName, PopupUploadFile_Context.itemId, extension);
        }

        if (typeof callback !== "undefined") {
            PopupUploadFile_Context.AfterUploadCallback = callback;
        }
        else {
            PopupUploadFile_Context.AfterUploadCallback = null;
        }

        if (mode === "field") {
            title = Dictionary.Common_Upload_Popup_Title;
            if (field !== null) {
                title = Dictionary.Common_Upload_Popup_Title_Prefix + " " + field.Label.toLowerCase();
                fieldLabel = field.Label + ": ";
            }
        }
        else {
            title = Dictionary.Common_Upload_Popup_TitleAttach;
        }

        var fieldValue = FieldDocument_GetValue(fieldName);
        console.log(fieldValue);
        $("#PopupModalFileTitle").html(title);
        $("#PopupUploadFileFieldName").html(fieldLabel);
        $("#PopupUploadFileFieldValue").html(fieldValue);
        $("#LauncherPopupUploadFile").click();
        popupContext.FileUpload = true;
    }
    else {
        PopupWarning(Dictionary.Common_Upload_NoSavedData);
    }
}



function GetFileAttributes(itemName, fieldName, itemId, extension) {
    var data = {
        "itemName": itemName,
        "fieldName": fieldName,
        "itemId": itemId,
        "extension": extension
    };
    $.ajax({
        "type": "POST",
        "url": "/Async/ItemDataServices.asmx/FileAttributes",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg.d.ReturnValue);
            var result = null;
            eval("result=" + msg.d.ReturnValue + ";");
            if (result.Length > 0) {
                $("#PopupUploadFileActuaLength").html(FormatBytes(result.Length, 2));
                $("#PopupUploadFileActuaCreatedOn").html(result.CreatedOn);
                $("#PopupUploadFileInfo").show();
            }
        },
        "error": function (msg) {
            PopupWaring(msg.responseText);
        }
    });
}

function UploadFileGo() {
    if (window.File && window.FileReader && window.FileList && window.Blob) {
        if (!$("#PopupUploadFileInput").val()) {
            $("#PopupUploadFileError").html(Dictionary.Common_Error_NoFileSelected);
            return false;
        }

        var fsize = $("#PopupUploadFileInput")[0].files[0].size;
        var ftype = $("#PopupUploadFileInput")[0].files[0].type;

        //allow only valid image file types 
        switch (ftype) {
            case "text/plain":
            case "image/png":
            case "image/jpeg":
            case "image/pjpeg":
            case "image/gif":
            case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
            case "application/pdf":
            case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
            case "application/vnd.ms-excel":
            case "application/vnd.openxmlformats-officedocument.wordprocessingml.template":
            case "application/vnd.oasis.opendocument.text":
            case "application/vnd.oasis.opendocument.spreadsheet":
            case "application/vnd.oasis.opendocument.graphics":
            case "application/msword":
            case "message/rfc822":
                break;
            default:
                $("#PopupUploadFileError").html("<i>" + ftype + "</i><br />&nbsp;" + Dictionary.Common_Warning_MimeType);
                return false;
        }

        if (fsize > 1024 * 1024 * 15) {
            $("#PopupUploadFileError").html(Dictionary.Common_MaximumUploadSize + " " + FormatBytes(1024 * 1024 * 15, 2));
            return false;
        }


        document.getElementById("PopupUploadFileBtnOk").disabled = true;
        var fd = new FormData();
        var file = document.getElementById("PopupUploadFileInput");
        for (var i = 0; i < $("#PopupUploadFileInput")[0].files.length; i++) {
            fd.append("_file", $("#PopupUploadFileInput")[0].files[i]);
        }

        fd.append("itemName", PopupUploadFile_Context.itemName);
        fd.append("instanceName", InstanceName);
        fd.append("companyId", CompanyId);
        fd.append("itemId", PopupUploadFile_Context.itemId);
        fd.append("fieldName", PopupUploadFile_Context.fieldName);
        fd.append("signature", PopupUploadFile_Context.signature);
        fd.append("historial", PopupUploadFile_Context.historial);
        fd.append("gallery", PopupUploadFile_Context.gallery);
        fd.append("description", $("#PopupUploadFileInput").val());
        fd.append("applicationUserId", ApplicationUser.Id);
        fd.append("mode", PopupUploadFile_Context.mode);

        $("#PopupUploadFileBtnOk").attr("disabled", "disabled");
        $("#PopupUploadFileBtnOk").html("<i class=\"ace-icon fal fa-cog fa-spin\"></i>&nbsp;" + Dictionary.Common_Uploading + "...");

        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/Async/UploadFile.aspx", true);
        xhr.onreadystatechange = function () {
            console.log(xhr.readyState);
            if (xhr.readyState === 4) {
                if (xhr.status === 200) {
                    $("#PopupUploadFileBtnOk").removeAttr("disabled");
                    $("#PopupUploadFileBtnOk").html("<i class=\"ace-icon fal fa-upload\"></i>&nbsp" + Dictionary.Common_Uploading + "...");
                    var result;
                    eval("result= " + xhr.responseText + ";");
                    result = result.split('|')[0];
                    console.log("UploadFileGo", result);
                    $("#PopupUploadFileBtnCancel").click();
                    $("#" + PopupUploadFile_Context.fieldName).val(result);
                    PopupSuccess(Dictionary.Common_Upload_Success_Message + " <strong>" + xhr.responseText + "</strong> " + Dictionary.Common_Upload_Success_Message2, Dictionary.Common_Upload_Success_Title);
                    if (PopupUploadFile_Context.mode === UploadFileMode.Gallery) {
                        Feature_Attachment_Retrieve();
                    }

                    if (PopupUploadFile_Context.mode === UploadFileMode.Field) {
                        $(".AuxButtonView_" + PopupUploadFile_Context.fieldName + " i").css("color", LayoutColor.InconButtonActive);
                        $(".AuxButtonDelete_" + PopupUploadFile_Context.fieldName + " i").css("color", LayoutColor.IconButtonDelete);
                        $(".AuxButtonDelete_" + PopupUploadFile_Context.fieldName).removeAttr("disabled");
                        itemData[PopupUploadFile_Context.fieldName] = result;
                        originalItemData[PopupUploadFile_Context.fieldName] = result;
                    }

                    if (HasPropertyEnabled(PopupUploadFile_Context.signature)) {
                        $("#DocumentDownloadIcon" + PopupUploadFile_Context.fieldName).removeAttr("disabled");
                        $("#DocumentDownloadIcon" + PopupUploadFile_Context.fieldName).css("color", "#333");
                        $("#DocumentDeleteIcon" + PopupUploadFile_Context.fieldName).removeAttr("disabled");
                        $("#DocumentDeleteIcon" + PopupUploadFile_Context.fieldName).css("color", "#f33");
                        $("#DocumentSignIcon" + PopupUploadFile_Context.fieldName).attr("title", Dictionary.Common_Btn_SignedDocument);
                        $("#DocumentSignIcon" + PopupUploadFile_Context.fieldName).disable();
                        $("#DocumentSignIcon" + PopupUploadFile_Context.fieldName).css("color", "#aaa");
                        $("#" + PopupUploadFile_Context.fieldName + "Label").css("color", "#000");
                        $("#" + PopupUploadFile_Context.fieldName + "Label").removeAttr("title");
                    }

                    if (typeof PopupUploadFile_Context.AfterUploadCallback === "function") {
                        PopupUploadFile_Context.AfterUploadCallback();
                    }

                    SignatureContext_RemoveForUpload(PopupUploadFile_Context.fieldName);
                }
                else {
                    $("#PopupUploadFileError").html(xhr.responseText);

                    $("#PopupUploadFileBtnOk").removeAttr("disabled");
                    $("#PopupUploadFileBtnOk").html("<i class=\"ace-icon fal fa-upload\"></i>&nbsp;Pujar...");
                    //PopupWaring("Error", xhr.responseText);
                }
            }
        };

        xhr.send(fd);
    }
    else {
        $("#PopupUploadFileError").html(Dictionary.Common_Warning_BrowserOld);
        //PopupWarning(Dictionary.Common_Warning_BrowserOld);
        return false;
    }
}