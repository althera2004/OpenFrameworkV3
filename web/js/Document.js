var PopupUploadFile_Context = {
    "itemName": "",
    "fieldName": "",
    "itemId": "",
    "mode": null
};

var DeleteFieldDocument_Context = {
    "fieldName": null,
    "fileName": null
}

function FieldDocument_View(e) {
    var fieldName = FieldDocument_GetFieldName(e);
    var value = FieldDocument_GetValue(fieldName);
    if (value !== null && value !== "") {
        var path = "/Instances/" + Instance.Name + "/Data/" + ItemDefinition.ItemName + "/" + ItemData.OriginalItemData.Id + "/" + value + "?" + guid();
        window.open(path);
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

        DeleteFieldDocument_Context.fieldName = fieldName;
        DeleteFieldDocument_Context.fileName = value;

        PopupConfirm("Vol eliminar <strong>"+value+"</strong>?<br/><i>Aquesta acció no es pot desfer.</i>", Dictionary.Common_Warning, FieldDocument_Delete_Confirmed);
    }
}

function FieldDocument_Delete_Confirmed() {
    var data = {
        "itemName": ItemDefinition.ItemName,
        "fieldName": DeleteFieldDocument_Context.fieldName,
        "fileName": DeleteFieldDocument_Context.fileName,
        "itemId": ItemData.ActualData.Id,
        "applicationUserId": ApplicationUser.Id,
        "companyId": Company.Id,
        "instanceName": Instance.Name
    };
    $.ajax({
        "type": "POST",
        "url": "/Async/ItemService.asmx/FieldDocummentDelete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            if (msg.d.Success === true) {
                FormFieldDocument_SetVoid(DeleteFieldDocument_Context.fieldName);
            }
        },
        "error": function (msg) {
            PopupWaring(msg.responseText);
        }
    });
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
            GetFileAttributes(PopupUploadFile_Context.itemName, fieldName, PopupUploadFile_Context.itemId);
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
        if (fieldValue !== null) {
            $("#PopupUploadFileFieldValue").html(fieldValue + "&nbsp;<i class=\"fa fa-eye blue\" style=\"cursor:pointer;\" onclick=\"$('#" + fieldName+"_BtnView').click();\"></i>");
        } else {
            $("#PopupUploadFileFieldValue").html("<i>No hi ha document</i>");
        }
        $("#LauncherPopupUploadFile").click();
        popupContext.FileUpload = true;
    }
    else {
        PopupWarning(Dictionary.Common_Upload_NoSavedData);
    }
}

function GetFileAttributes(itemName, fieldName, itemId) {
    var data = {
        "itemName": itemName,
        "fileName": ItemData.ActualData[fieldName],
        "itemId": itemId,
        "instanceName": Instance.Name
    };
    $.ajax({
        "type": "POST",
        "url": "/Async/ItemService.asmx/GetFieldFileAttributes",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg.d.ReturnValue);
            var result = null;
            eval("result=" + msg.d.ReturnValue + ";");
            if (result !== null) {
                if (result.Length > 0) {
                    $("#PopupUploadFileActuaLength").html(FormatBytes(result.Length, 2));
                    $("#PopupUploadFileActuaCreatedOn").html(result.CreatedOn);
                    $("#PopupUploadFileInfo").show();
                }
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
        fd.append("instanceName", Instance.Name);
        fd.append("companyId", Company.Id);
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
                        Feature_Attach_Retrieve();
                    }

                    if (PopupUploadFile_Context.mode === UploadFileMode.Field) {
                        $("#" + PopupUploadFile_Context.fieldName + "_BtnView i").enable();
                        $("#" + PopupUploadFile_Context.fieldName + "_BtnView i").css("color", LayoutColor.InconButtonActive);
                        $("#" + PopupUploadFile_Context.fieldName + "_BtnDelete i").enable();
                        $("#" + PopupUploadFile_Context.fieldName + "_BtnDelete i").css("color", LayoutColor.IconButtonDelete);
                        ItemData.OriginalItemData[PopupUploadFile_Context.fieldName] = result;
                        ItemData.ActualData[PopupUploadFile_Context.fieldName] = result;
                    }

                    if (HasPropertyEnabled(PopupUploadFile_Context.signature)) {
                        $("#" + PopupUploadFile_Context.fieldName + "_BtnView i").removeAttr("disabled");
                        $("#" + PopupUploadFile_Context.fieldName + "_BtnView i").css("color", "#333");
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

                    // weke-signature
                    //SignatureContext_RemoveForUpload(PopupUploadFile_Context.fieldName);
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

function FormFieldDocument_SetVoid(id) {
    $("#" + id + "_BtnView").disable();
    $("#" + id + "_BtnView i").css("color", LayoutColor.Inactive);
    $("#" + id + "_BtnDelete").disable();
    $("#" + id + "_BtnDelete i").css("color", LayoutColor.Inactive);
    $("#" + id).val("");
    ItemData.OriginalItemData[id] = null;
    ItemData.ActualData[id] = null;
}

// ------------- Generación de documentos
// -------------------------------------------
var DocumentCreateSilence = [];
function Document_Create_PreCondition(id) {
    var preConditionName = itemDefinition.ItemName.toUpperCase() + "_" + id;
    console.log("PreCondition", preConditionName);
    var preCondition = eval("typeof " + preConditionName + ";");
    if (preCondition === "function") {
        return eval(itemDefinition.ItemName.toUpperCase() + "_" + id + "();");
    }

    return { "Result": true };
}

function Document_Create(url, fieldName) {
    $("#DocumentAddIcon" + fieldName).addClass("fa-spin");
    $("#DocumentAddIcon" + fieldName).addClass("fa-spinner");
    $("#DocumentAddIcon" + fieldName).removeClass("fa-plus");
    $.getJSON(url,
        function (data) {
            var silence = false;
            var temp = [];

            // Si el campo está oculto por no existir se muestra
            $("#" + data.FieldName + "Container").show();
            $("#" + data.FieldName + "Label").show();

            if (data.FileUrl.indexOf("SignaturesTemporal/") === -1) {
                SetFieldValue(data.FieldName, data.FileUrl);
                // Si el documento es de un campo se lanza desde el botón "view" del campo
                if (HasPropertyValue(data.FieldName)) {
                    FEATURE_Documents_RestoreAddButton(data.FieldName);

                    if ($("#DocumentSign" + data.FieldName).length > 0) {
                        $("#DocumentSign" + data.FieldName).css("color", "#000");
                    }

                    // Se muestra el campo por si estaba oculto al no haber documento
                    $("#" + data.FieldName + "Label").show();
                    $("#" + data.FieldName + "Container").show();

                    // Primero se revisa si el documento se crea en modo silencio
                    for (var s = 0; s < DocumentCreateSilence.length; s++) {
                        if (DocumentCreateSilence[s] === fieldName) {
                            silence = true;
                            break;
                        }
                    }

                    // En caso de crearse en modo silencio se elimina de la lista
                    if (silence === true) {
                        for (var s2 = 0; s2 < DocumentCreateSilence.length; s2++) {
                            if (DocumentCreateSilence[s2].FieldName !== fieldName) {
                                temp.push(DocumentCreateSilence[s2]);
                            }
                        }

                        DocumentCreateSilence = temp;
                    }

                    if (silence === false) {
                        Form_ShowFile(data.FieldName, itemData.Id);
                    }
                }
                else {
                    window.open(data.FileUrl);
                    if (Feature_Attach_Retrieve()) {
                        Feature_Attach_Retrieve();
                    }
                }
            }
            else {
                // Se restaura el botón "+"
                $("#DocumentAddIcon" + data.FieldName).removeClass("fa-spinner");
                $("#DocumentAddIcon" + data.FieldName).removeClass("fa-spin");
                $("#DocumentAddIcon" + data.FieldName).addClass("fa-plus");
                FieldSetSignable(data.FieldName);
                SignaturesContext_UpdateCount(data.FieldName, 0);

                // Primero se revisa si el documento se crea en modo silencio
                for (var ss = 0; ss < DocumentCreateSilence.length; ss++) {
                    if (DocumentCreateSilence[ss] === fieldName) {
                        silence = true;
                        break;
                    }
                }

                // En caso de crearse en modo silencio se elimina de la lista
                if (silence === true) {
                    for (var ss2 = 0; ss2 < DocumentCreateSilence.length; ss2++) {
                        if (DocumentCreateSilence[ss2].FieldName !== fieldName) {
                            temp.push(DocumentCreateSilence[ss2]);
                        }
                    }

                    DocumentCreateSilence = temp;
                }

                if (silence === false) {
                    window.open("/Instances/" + InstanceName + "/Data/" + itemDefinition.ItemName + "/" + itemData.Id + "/" + data.FileUrl + "?" + guid());
                }

                var lastId = -1;
                for (var x = 0; x < SignatureStatus.length; x++) {
                    if (SignatureStatus[x].Id === lastId) {
                        lastId--;
                    }
                }

                SignatureStatus.push({
                    "CompanyId": CompanyId,
                    "Count": 0,
                    "FieldName": fieldName,
                    "Id": lastId,
                    "ItemDefinitionId": 203,
                    "ItemId": itemData.Id
                });

            }

            // El callback del botón es independiente de si se firma o no
            var callback = data.FieldName + "BtnAddDocument_AfterCallback";
            var typeofCallback = eval("typeof " + callback);
            if (typeofCallback === "function") {
                eval(callback + "();");
            }

            PopupWorkingHide();
        }).error(function (e) {
            console.log(e);
            if (e.readyState === 4 && e.status === 200) {
                if (typeof data !== "undefined") {
                    if (HasPropertyValue(data.FieldName)) {
                        FEATURE_Documents_RestoreAddButton(data.FieldName);
                    }
                }

                console.log(eval(e.responseText));
            }
        });
}
// -------------------------------------------