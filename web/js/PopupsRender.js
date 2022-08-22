var PopupConfirm_Context = {
    "YesCallBackAction": null,
    "NoCallBackAction": null
};

var PopupInfo_Context = {
    "CallBack": null
};

var PopupSuccess_Context = {
    "CallBack": null
};

var PopupDocumentHistory_Context = {
    "ItemName": null,
    "ItemField": null,
    "ItemId": null
};

var PopupBAR_Context = {
    "ItemDefinition": null,
    "FormData": null,
    "DirtyFormData": false,
    "DeleteId": null
}

var PopupDefault_Context = {};
var PopupWarningLaunched = false;
var PopupActivateCallBackAction = null;

var PopupDeleteContext = {
    "ItemDefinition": null,
    "ItemId": null,
    "DeleteCallBackAction": null
}

var popupContext = {
    "Info": false,
    "Warning": false,
    "Success": false,
    "Activate": false,
    "Delete": false,
    "Image": false,
    "ImageView": false,
    "ExportList": false,
    "Map": false,
    "ImageGallery": false,
    "Confirm": false,
    "FAQs": false,
    "UserConversion": false,
    "SaveInfo": false,
    "ContactPerson": false,
    "DocumentHistory": false,
    "DocumentGallery": false,
    "LoginAndContinue": false,
    "DeleteResponse": false,
    "BAR": false
};

function PopupRenderLoginAndContinue() {
    if (popupContext.LoginAndContinue === false) {
        var popup = "<div id=\"PopupLoginAndContinue\" style=\"z-index:3000!important;\" class=\"modal fade\" tabindex=\"-1\">";
        popup += "        <div class=\"modal-dialog\">";
        popup += "            <div class=\"modal-content\">";
        popup += "                <div class=\"modal-header no-padding\">";
        popup += "                    <div class=\"table-header bgred\">";
        popup += "                        <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">";
        popup += "                            <span class=\"white\">&times;</span>";
        popup += "                        </button>";
        popup += "                        <span id=\"PopupLoginAndContinueTitle\" class=\"white\">Security action</span>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-body\">";
        popup += "                    <div class=\"row\">";
        popup += "                        <div class=\"col-xs-2\">";
        popup += "                            <i class=\"ace-icon fa fa-lock red fa-2x\"></i>";
        popup += "                        </div>";
        popup += "                        <div class=\"col-xs-10\"><p id=\"PopupLoginAndContinueMessage\">";
        popup += "                           S'ha detectat una llarga durada d'inactivitat.<br />Introdueixi de nou la seva paraula de pas per a continuar.<br>";
        popup += "                        </p>";
        popup += "                        <table>";
        popup += "                           <tr><td style=\"padding:4px;\">Usuari:</td><td style=\"padding:4px;\"><strong> " + ApplicationUser.Profile.FullName + "</strong></td></tr>";
        popup += "                           <tr><td style=\"padding:4px;\">Paraula de pas:</td><td style=\"padding:4px;\"><input type=\"password\" id=\"LAC\" /></td></tr>";
        popup += "                        </table>";
        popup += "                        </div>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-footer no-margin-top\">";
        popup += "                    <span id=\"PopupLoginAndContinueErrorMessage\" style=\"margin-top:8px;color:#a7250f;float:left;\"></span>";
        popup += "                    <button type=\"button\" class=\"btn btn-sm pull-right btn-danger\" id=\"PopupLoginAndContinueBtnCancel\" onclick=\"PopupLoginAndContinueCloseCallBack();\">";
        popup += "                        <i class=\"ace-icon fa fa-sign-out\"></i>&nbsp;Sortir de l'aplicació";
        popup += "                    </button>";
        popup += "                    <button type=\"button\" class=\"btn btn-sm pull-right btn-success\" id=\"PopupLoginAndContinueBtnRelogin\" onclick=\"PopupLoginAndContinueCloseRelogin();\">";
        popup += "                        <i class=\"ace-icon fa fa-check\"></i>&nbsp;" + Dictionary.Common_Accept;
        popup += "                    </button>";
        popup += "                    <button type=\"button\" style=\"display:none;\" class=\"btn btn-sm pull-right btn-danger\" id=\"PopupLoginAndContinueBtnClose\" data-dismiss=\"modal\">";
        popup += "                        <i class=\"ace-icon fa fa-sign-out\"></i>&nbsp;Sortir de l'aplicació";
        popup += "                    </button>";
        popup += "                </div>";
        popup += "            </div>";
        popup += "        </div>";
        popup += "    </div>";
        $("#PopupsContentHolder").append(popup);
        $("#PopupLoginAndContinueErrorMessage").html("");
    }
}

function PopupRenderDeleteResponse() {
    if (popupContext.DeleteResponse === false) {
        var popup = "<div id=\"PopupDeleteResponse\" style=\"z-index:3000!important;\" class=\"modal fade\" tabindex=\"-1\">";
        popup += "        <div class=\"modal-dialog\">";
        popup += "            <div class=\"modal-content\">";
        popup += "                <div class=\"modal-header no-padding\">";
        popup += "                    <div class=\"table-header bgblue\">";
        popup += "                        <span class=\"white\">" + Dictionary.Common_Info + "</span>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-body\">";
        popup += "                    <div class=\"row\">";
        popup += "                        <div class=\"col-xs-2\">";
        popup += "                            <i class=\"ace-icon fa fa-info-circle blue fa-2x\"></i>";
        popup += "                        </div>";
        popup += "                        <div class=\"col-xs-10\">&quot;<strong>" + PopupDeleteContext.Message + "</strong>&quot; eliminado ha sido eliminado.</div>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-footer no-margin-top\">";
        popup += "                    <button type=\"button\" class=\"btn btn-sm pull-right btn-success\" id=\"PopupDeleteResponseAccept\" onclick=\"$('#FormBtnCancel').click();\">";
        popup += "                        <i class=\"ace-icon fa fa-check\"></i>&nbsp;" + Dictionary.Common_Accept;
        popup += "                    </button>";
        popup += "                </div>";
        popup += "            </div>";
        popup += "        </div>";
        popup += "    </div>";
        $("#PopupsContentHolder").append(popup);
    }
}

var LAC = 3;
function _PopupLoginAndContinueCloseRelogin() {
    $("#PopupLoginAndContinueErrorMessage").html("");
    var password = $("#LAC").val();

    if (password === "") {
        $("#PopupLoginAndContinueErrorMessage").html("Paraula de pas obligatòria");
        return;
    }

    var test = $.base64.encode(password);
    if (test === ApplicationUser.Password) {
        $("#PopupLoginAndContinueBtnClose").click();
        popupContext.LoginAndContinue = false;
        $("form").removeClass("blur-filter");
        SessionRestart();
        localStorage.setItem("LAC", false);
    }
    else {
        if (LAC === 0) {
            document.location = "/";
        }
        LAC--;
        $("#PopupLoginAndContinueBtnRelogin").html(Dictionary.Common_Accept + " - " + LAC);
        $("#PopupLoginAndContinueErrorMessage").html("Número d'intents restants: " + LAC);
        $("#LAC").val("");
    }
}

function PopupLoginAndContinueCloseRelogin() {
    var credential = ApplicationUser.Id + "||||" + $("#LAC").val() + "||||" + Company.Id + '||||' + Instance.Name;
    var data = {
        "credential": btoa(unescape(encodeURIComponent(credential)))
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/SecurityService.asmx/MaintainSession",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            if (msg.d.Success === true) {
                $("#PopupLoginAndContinueBtnClose").click();
                popupContext.LoginAndContinue = false;
                $("form").removeClass("blur-filter");
                SessionRestart();
                localStorage.setItem("LAC", false);
            }
            else {
                if (LAC === 0) {
                    document.location = "/";
                }
                LAC--;
                $("#PopupLoginAndContinueBtnRelogin").html(Dictionary.Common_Accept + " - " + LAC);
                $("#PopupLoginAndContinueErrorMessage").html("Número d'intents restants: " + LAC);
                $("#LAC").val("");
            }
        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}

function PopupLoginAndContinueCloseCallBack() {
    document.location = "/";
}

function PopupRenderMap() {
    if (popupContext.Info === false) {
        var popup = "<div id=\"PopupMap\" class=\"modal fade\" tabindex=\"-1\">";
        popup += "    <div class=\"modal-dialog\" style=\"width:800px;\">";
        popup += "        <div class=\"modal-content\">";
        popup += "            <div class=\"modal-header no-padding\">";
        popup += "                <div class=\"table-header blue\">";
        popup += "                    <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">";
        popup += "                        <span class=\"white\">&times;</span>";
        popup += "                    </button>";
        popup += "                    <span id=\"PopupMapTitle\" style=\"color:#fff;\"></span>";
        popup += "                </div>";
        popup += "            </div>";
        popup += "            <div class=\"modal-body\" style=\"padding-top:0;\">";
        popup += "                <h4 id=\"PopupMapAddress\"></h4>";
        popup += "                <div class=\"row\">";
        popup += "                    <label class=\"col-sm-2\"><strong>Dirección:</strong></label>";
        popup += "                    <div class=\"col-sm-10\" id=\"PopupMapAddressText\"></div>";
        popup += "                </div>";
        popup += "                <div class=\"row\">";
        popup += "                    <label class=\"col-sm-2\">" + Dictionary.Common_Map_Latitude + ":</label>";
        popup += "                    <div class=\"col-sm-3\" id=\"PopupMapLatitude\"></div>";
        popup += "                    <label class=\"col-sm-2\">" + Dictionary.Common_Map_Longitude + ":</label>";
        popup += "                    <div class=\"col-sm-3\" id =\"PopupMapLongitude\"></div>";
        popup += "                    <div class=\"col-sm-2\">&nbsp;</div>";
        popup += "                </div>";
        popup += "                <div class=\"row\" style=\"display:none;\" id=\"DivNewLatLng\">";
        popup += "                    <label class=\"col-sm-2\">" + Dictionary.Common_Map_NewLatitude + ":</label>";
        popup += "                    <div class=\"col-sm-3\" id=\"PopupMapNewLatitude\"></div>";
        popup += "                    <label class=\"col-sm-2\">" + Dictionary.Common_Map_NewLongitude + ":</label>";
        popup += "                    <div class=\"col-sm-3\" id=\"PopupMapNewLongitude\"></div>";
        popup += "                    <div class=\"col-sm-2\">&nbsp;</div>";
        popup += "                </div>";
        popup += "                <div id=\"map_extended\" style=\"height:400px;\" class=\"col-sm-12\"></div>";
        popup += "            </div>";
        popup += "            <div class=\"modal-footer no-margin-top\">";
        popup += "                <button type=\"button\" class=\"btn btn-sm pull-right\" data-dismiss=\"modal\" id=\"PopupMapBtnCancel\">";
        popup += "                    <i class=\"ace-icon fa fa-times\"></i>";
        popup += "                    " + Dictionary.Common_Cancel;
        popup += "            </button>";
        popup += "                <button type=\"button\" class=\"btn btn-sm pull-right btn-info\" style=\"margin-right:8px;\" id=\"PopupMapBtnAccuracy\" onclick=\"PopupMapBtnAccuracyClicked();\">";
        popup += "                    <i class=\"ace-icon fa fa-map-marker\"></i>";
        popup += "                    " + Dictionary.Common_Map_Accuracy;
        popup += "            </button>";
        popup += "                <button type=\"button\" class=\"btn btn-sm pull-right btn-info\" style=\"margin-right:8px;display:none;\" id=\"PopupMapBtnResolve\" onclick=\"PopupMapBtnResolveClicked();\">";
        popup += "                    <i class=\"ace-icon fa fa-map-marker\"></i>";
        popup += "                    " + Dictionary.Common_Map_Resolve;
        popup += "            </button>";
        popup += "                <button type=\"button\" class=\"btn btn-sm pull-right btn-success\" style=\"margin-right:8px;display:none;\" id=\"PopupMapBtnSave\" onclick=\"MapShowSave();\">";
        popup += "                    <i class=\"ace-icon fa fa-save\"></i>";
        popup += "                    " + Dictionary.Common_Save;
        popup += "            </button>";
        popup += "            </div>";
        popup += "        </div>";
        popup += "    </div>";
        popup += "</div>";
        $("#PopupsContentHolder").append(popup);
    }

    popupContext.Map = true;
    $("#PopupMap").bind("cssClassChanged", MapPopupResize);
}

function PopupRenderBAR() {
    if (popupContext.BAR === false) {
        var popup = "";
        popup += "<div id=\"PopupBAR\" style=\"z-index:3000!important;\" class=\"modal fade\" tabindex=\"-1\">";
        popup += "  <div class=\"modal-dialog\">";
        popup += "    <div class=\"modal-content\" style=\"border:none;\">";
        popup += "      <div class=\"modal-header no-padding\">";
        popup += "        <div class=\"table-header bgblue\">";
        popup += "          <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">";
        popup += "            <span class=\"white\">&times;</span>";
        popup += "          </button>";
        popup += "          <span id=\"PopupBARTitle\" class=\"white\">"+ PopupBAR_Context.ItemDefinition.Layout.LabelPlural +"</span>";
        popup += "        </div>";
        popup += "      </div>";
        popup += "      <div class=\"modal-body\" style=\"padding:12px;\">";
        popup += "        <table class=\"table\" style=\"width:100%;border:1px solid #ccc;\">";
        popup += "          <thead>";
        popup += "            <tr><th colspan=\"4\">" + PopupBAR_Context.ItemDefinition.Layout.Label +"</th></tr>";
        popup += "          </thead>";
        popup += "            <tbody id=\"PopupBARTable\">";
        popup += "              </tbody>";
        popup += "          </table>";
        popup += "          <table class=\"table\" style=\"width:100%;maring-top:12px;display:none;\" cellspacing=\"2\" cellpadding=\"2\" id=\"PopupBarForm\"><tbody>";
        popup += "              <tr><td colspan=\"4\" style=\"border:none;\"><strong style=\"color:#307ECC;\" id=\"PopupBAR_FormTitle\"></strong></td></tr>";
        popup += "              <tr>";
        popup += "                <td>" + PopupBAR_Context.ItemDefinition.Layout.Label + "</td>";
        popup += "                <td><input type=\"text\" class=\"col-xs-11\" id=\"PopupBarDescription\" onkeyup=\"PopupBAR_DetectDirtyFormData();\" /></td>";
        popup += "                <td style=\"width:30px;\"><button onclick=\"PopupBAR_Save();\" id=\"PopupBAR_BtnSave\" class=\"btn-icon\"><i class=\"fa fa-save green\" title=\"Desar\"></i></button></td>";
        popup += "                <td style=\"width:30px;\"><button onclick=\"PopupBAR_CancelEdit();\" id=\"PopupBAR_BtnCancelEdition\" class=\"btn-icon\"><i class=\"fa fa-ban red\" title=\"Cancel·lar\"></i></button></td>";
        popup += "              </tr>";
        popup += "            </tbody>";
        popup += "          </table>";
        popup += "        </div>";
        popup += "      </div>";
        popup += "      <div class=\"modal-footer no-margin-top\">";
        popup += "        <span id=\"PopupBAR_ErrorMessage\" style=\"float:left;color:red;\"></span>";
        popup += "        <button type=\"button\" class=\"btn btn-sm pull-right\" data-dismiss=\"modal\" id=\"PopupBARBtnCancel\" onclick=\"PopupInfoCloseCallBack();\">";
        popup += "           " + Dictionary.Common_Close;
        popup += "        </button>";
        popup += "        <button type=\"button\" class=\"btn btn-sm pull-right btn-info\" id=\"PopupBARBtnAdd\" onclick=\"PopupBAR_Add();\">";
        popup += "          <i class=\"ace-icon fa fa-check\"></i>&nbsp;" + Dictionary.Common_Add;
        popup += "        </button>";
        popup += "      </div>";
        popup += "    </div>";
        popup += "  </div>";
        popup += "</div>";
        $("#PopupsContentHolder").append(popup);
    }

    popupContext.BAR = true;
}

function PopupBAR_DetectDirtyFormData() {
    PopupBAR_Context.DirtyFormData = $("#PopupBarDescription").val() !== PopupBAR_Context.FormData.Description;
    if (PopupBAR_Context.DirtyFormData === true) {
        $(".btn-icon").disable();
        $("#PopupBAR_BtnSave").enable();
        $("#PopupBAR_BtnCancelEdition").enable();
        $("#PopupBARBtnAdd").disable();
        $("#PopupBARBtnCancel").disable();
    } else {
        $(".btn-icon").enable();
        $("#PopupBARBtnAdd").enable();
        $("#PopupBARBtnCancel").enable();
    }
}

function PopupBAR_Check(id) {
    $("#" + PopupBAR_Context.FieldName).val(id);
    $("#PopupBARBtnCancel").click();
    ItemData.UpdateData(PopupBAR_Context.FieldName, id);
}

function PopupBAR_Add() {
    $("#PopupBAR_FormTitle").html("Afegir " + PopupBAR_Context.ItemDefinition.Layout.Label.toLowerCase());
    PopupBAR_Context.FormData = { "id": -1, "value": "" };
    $("#PopupBarDescription").val("");
    $("#PopupBAR_ErrorMessage").html("");
    $("#PopupBarForm").show();
}

function PopupBAR_Edit(id) {
    $("#PopupBAR_FormTitle").html("Editar " + PopupBAR_Context.ItemDefinition.Layout.Label.toLowerCase());
    var item = GetFKById(PopupBAR_Context.ItemDefinition.ItemName, id);
    if (item !== null) {
        PopupBAR_Context.FormData = item;
        $("#PopupBarDescription").val(item.Description);
    }
    else {
        $("#PopupBarDescription").val("");
    }

    $("#PopupBAR_ErrorMessage").html("");
    $("#PopupBarForm").show();
}

function PopupBAR_CancelEdit() {
    PopupBAR_Context.DirtyFormData = false;
    PopupBAR_Context.FormData = null;
    $("#PopupBarDescription").val("");
    $("#PopupBAR_ErrorMessage").html("");
    $("#PopupBarForm").hide();
    $(".btn-icon").enable();
    $("#PopupBARBtnAdd").enable();
    $("#PopupBARBtnCancel").enable();
}

function PopupBAR_Save() {
    // comprovar que hi ha canvis
    $("#PopupBAR_ErrorMessage").html("");
    if ($("#PopupBarDescription").val() === PopupBAR_Context.FormData.Description) {

        $("#PopupBAR_ErrorMessage").html("<i class=\"fa fa-exclamation-circle\"></i>&nbsp;No hi ha canvis");
        return false;
    }

    var data = {
        "itemName": PopupBAR_Context.ItemDefinition.ItemName,
        "id": PopupBAR_Context.FormData.Id,
        "description": $("#PopupBarDescription").val(),
        "applicationUserId": ApplicationUser.Id,
        "companyId": Company.Id,
        "instanceName": Instance.Name
    };

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/ItemService.asmx/ItemBarSave",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function () {
            GetFKItem(PopupBAR_Context.ItemDefinition.ItemName, PopupBAR_DataUpdated);
            $("#PopupBAR_BtnCancelEdition").click();
        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });

    console.log(data);
}

function PopupBAR_DataUpdated() {
    var fieldName = PopupBAR_Context.ItemDefinition.ItemName + "Id";
    FillComboFromFK(fieldName, PopupBAR_Context.ItemDefinition.ItemName, ItemData.ActualData[fieldName]);
    PopupBARFillDataTable();
}

function PopupBAR_Delete(id) {
    if (PopupBAR_Context.FormData !== null) {
        if (PopupBAR_Context.FormData.Id === id) {
            $("#PopupBAR_ErrorMessage").html("<i class=\"fa fa-exclamation-circle\"></i>&nbsp;No es pot eliminar si s'està editant");
            return false;
        }
    }

    PopupBAR_Context.DeleteId = id;
    var item = GetFKById(PopupBAR_Context.ItemDefinition.ItemName, id);
    PopupConfirm("Vol eliminar <strong>" + item.Description + "</strong>?", Dictionary.Common_Delete, PopupBAR_DeleteConfirmed);
}

function PopupBAR_DeleteConfirmed() {
    var data = {
        "itemName": PopupBAR_Context.ItemDefinition.ItemName,
        "id": PopupBAR_Context.DeleteId,
        "applicationUserId": ApplicationUser.Id,
        "instanceName": Instance.Name
    };

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/ItemService.asmx/ItemBarDelete",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {

            console.log(msg.d);
            if (msg.d.Success === true) {
                PopupBAR_Context.DeleteId = null;
                GetFKItem(PopupBAR_Context.ItemDefinition.ItemName, PopupBAR_DataUpdated);
                $("#PopupBAR_BtnCancelEdition").click();
            }
            else {
                var message = msg.d.MessageError;
                if (msg.d.MessageError === "Exists") {
                    message = "No es pot eliminar perque està en ús";
                }
                PopupWarning(message);
            }
        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}

function PopupRenderInfo() {
    if (popupContext.Info === false) {
        var popup = "<div id=\"PopupInfo\" style=\"z-index:3000!important;\" class=\"modal fade\" tabindex=\"-1\">";
        popup += "        <div class=\"modal-dialog\">";
        popup += "            <div class=\"modal-content\">";
        popup += "                <div class=\"modal-header no-padding\">";
        popup += "                    <div class=\"table-header bgblue\">";
        popup += "                        <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">";
        popup += "                            <span class=\"white\">&times;</span>";
        popup += "                        </button>";
        popup += "                        <span id=\"PopupInfoTitle\" class=\"white\">" + Dictionary.Common_Info + "</span>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-body\">";
        popup += "                    <div class=\"row\">";
        popup += "                        <div class=\"col-xs-2\">";
        popup += "                            <i class=\"ace-icon fa fa-info-circle blue fa-2x\"></i>";
        popup += "                        </div>";
        popup += "                        <div class=\"col-xs-10\"><p id=\"PopupInfoMessage\"></p></div>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-footer no-margin-top\">";
        popup += "                    <button type=\"button\" class=\"btn btn-sm pull-right btn-success\" data-dismiss=\"modal\" id=\"PopupInfoBtnCancel\" onclick=\"PopupInfoCloseCallBack();\">";
        popup += "                        <i class=\"ace-icon fa fa-check\"></i>&nbsp;" + Dictionary.Common_Accept;
        popup += "                    </button>";
        popup += "                </div>";
        popup += "            </div>";
        popup += "        </div>";
        popup += "    </div>";
        $("#PopupsContentHolder").append(popup);
    }

    popupContext.Info = true;
}

function PopupRenderSaveInfo() {
    if (popupContext.SaveInfo === false) {
        var popup = "<div id=\"PopupSaveInfo\" class=\"modal fade\" tabindex=\"-1\">";
        popup += "        <div class=\"modal-dialog\">";
        popup += "            <div class=\"modal-content\">";
        popup += "                <div class=\"modal-header no-padding\">";
        popup += "                    <div class=\"table-header bgblue\">";
        popup += "                        <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">";
        popup += "                            <span class=\"white\">&times;</span>";
        popup += "                        </button>";
        popup += "                        <span id=\"PopupSaveInfoTitle\" class=\"white\">" + Dictionary.Common_Info + "</span>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-body\">";
        popup += "                    <div class=\"row\">";
        popup += "                        <div class=\"col-xs-2\">";
        popup += "                            <i class=\"ace-icon fa fa-info-circle blue fa-2x\"></i>";
        popup += "                        </div>";
        popup += "                        <div class=\"col-xs-10\"><p id=\"PopupSaveInfoMessage\"></p></div>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-footer no-margin-top\">";
        popup += "                    <button type=\"button\" class=\"btn btn-sm pull-right btn-success\" data-dismiss=\"modal\" id=\"PopupSaveInfoBtnOkAndExit\" onclick=\"$('#BtnCancel').click();\">";
        popup += "                        <i class=\"fas fa-sign-out-alt\"></i>&nbsp;" + Dictionary.Common_AcceptAndExit;
        popup += "                    </button>";
        popup += "                    <button type=\"button\" class=\"btn btn-sm pull-right btn-success\" data-dismiss=\"modal\" id=\"PopupSaveInfoBtnOk\">";
        popup += "                        <i class=\"ace-icon fa fa-check\"></i>&nbsp;" + Dictionary.Common_Accept;
        popup += "                    </button>";
        popup += "                </div>";
        popup += "            </div>";
        popup += "        </div>";
        popup += "    </div>";
        $("#PopupsContentHolder").append(popup);
    }

    popupContext.SaveInfo = true;
}

function PopupRenderWarning() {
    if (popupContext.Warning === false) {
        var popup = "<div id=\"PopupWarning\" style=\"z-index:3000!important;\" class=\"modal fade\" tabindex=\"-1\">";
        popup += "        <div class=\"modal-dialog\">";
        popup += "            <div class=\"modal-content\">";
        popup += "                <div class=\"modal-header no-padding\">";
        popup += "                    <div class=\"table-header bgred\">";
        popup += "                        <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">";
        popup += "                            <span class=\"white\">&times;</span>";
        popup += "                        </button>";
        popup += "                        <span id=\"PopupWarningTitle\">" + Dictionary.Common_Warning + "</span>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-body\">";
        popup += "                    <div class=\"row\">";
        popup += "                        <div class=\"col-xs-2\">";
        popup += "                            <i class=\"ace-icon fa fa-exclamation-triangle red fa-2x\"></i>";
        popup += "                        </div>";
        popup += "                        <div class=\"col-xs-10\"><p id=\"PopupWarningMessage\"></p></div>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-footer no-margin-top\">";
        popup += "                    <button type=\"button\" class=\"btn btn-sm pull-right btn-success\" data-dismiss=\"modal\" id=\"PopupWarningBtnCancel\">";
        popup += "                        <i class=\"ace-icon fa fa-times\"></i>&nbsp;" + Dictionary.Common_Accept;
        popup += "                    </button>";
        popup += "                </div>";
        popup += "            </div>";
        popup += "        </div>";
        popup += "    </div>";
        $("#PopupsContentHolder").append(popup);
    }

    popupContext.Warning = true;
}

function PopupRenderSuccess() {
    if (popupContext.Success === false) {
        var popup = "<div id=\"PopupSuccess\" class=\"modal fade\" tabindex=\"-1\">";
        popup += "       <div class=\"modal-dialog\">";
        popup += "            <div class=\"modal-content\">";
        popup += "                <div class=\"modal-header no-padding\">";
        popup += "                    <div class=\"table-header bggreen\">";
        popup += "                        <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">";
        popup += "                            <span class=\"white\">&times;</span>";
        popup += "                        </button>";
        popup += "                        <span id=\"PopupSuccessTitle\" class=\"white\">" + Dictionary.Common_Success_Title + "</span>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-body\">";
        popup += "                    <div class=\"row\">";
        popup += "                        <div class=\"col-xs-2\">";
        popup += "                            <i class=\"ace-icon fa fa-check green fa-2x\"></i>";
        popup += "                        </div>";
        popup += "                        <div class=\"col-xs-10\"><p id=\"PopupSuccessMessage\"></p></div>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-footer no-margin-top\">";
        popup += "                    <button type=\"button\" class=\"btn btn-sm pull-right btn-success\" data-dismiss=\"modal\" id=\"PopupSuccessBtnCancel\">";
        popup += "                        <i class=\"ace-icon fa fa-check\"></i>&nbsp;" + Dictionary.Common_Accept;
        popup += "                    </button>";
        popup += "                </div>";
        popup += "            </div>";
        popup += "        </div>";
        popup += "    </div>";
        $("#PopupsContentHolder").append(popup);
    }

    popupContext.Success = true;
}

function PopupRenderConfirm() {
    if (popupContext.Confirm === false) {
        var popup = "<div id=\"PopupConfirm\" style=\"z-index:3000!important;\" class=\"modal fade\" tabindex=\"-1\">";
        popup += "       <div class=\"modal-dialog\">";
        popup += "            <div class=\"modal-content\">";
        popup += "                <div class=\"modal-header no-padding\">";
        popup += "                    <div class=\"table-header bgblue\">";
        popup += "                        <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">";
        popup += "                            <span class=\"white\">&times;</span>";
        popup += "                        </button>";
        popup += "                        <span id=\"PopupConfirmTitle\" class=\"white\">" + Dictionary.Common_Confirm_Title + "</span>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-body\">";
        popup += "                    <div class=\"row\">";
        popup += "                        <div class=\"col-xs-2\">";
        popup += "                            <i class=\"ace-icon fa fa-question blue fa-2x\"></i>";
        popup += "                        </div>";
        popup += "                        <div class=\"col-xs-10\"><p id=\"PopupConfirmMessage\"></p></div>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-footer no-margin-top\">";
        popup += "                    <button type=\"button\" class=\"btn btn-sm pull-right btn-danger\" data-dismiss=\"modal\" id=\"PopupConfirmBtnNo\" onclick=\"PopupConfirmLaunchNo();\">";
        popup += "                        <i class=\"ace-icon fa fa-check\"></i>&nbsp;" + Dictionary.Common_No;
        popup += "                    </button>";
        popup += "                    <button type=\"button\" class=\"btn btn-sm pull-right btn-success\" data-dismiss=\"modal\" id=\"PopupConfirmBtnYes\" onclick=\"PopupConfirmLaunchYes();\">";
        popup += "                        <i class=\"ace-icon fa fa-check\"></i>&nbsp;" + Dictionary.Common_Yes;
        popup += "                    </button>";
        popup += "                </div>";
        popup += "            </div>";
        popup += "        </div>";
        popup += "    </div>";
        $("#PopupsContentHolder").append(popup);
    }

    popupContext.Confirm = true;
}

function PopupRenderActivate() {
    if (popupContext.Activate === false) {
        var popup = "<div id=\"PopupActivate\" class=\"modal fade\" tabindex=\"-1\">";
        popup += "        <div class=\"modal-dialog\">";
        popup += "            <div class=\"modal-content\">";
        popup += "                <div class=\"modal-header no-padding\">";
        popup += "                    <div class=\"table-header bggreen\">";
        popup += "                        <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">";
        popup += "                            <span class=\"white\">&times;</span>";
        popup += "                        </button>";
        popup += "                        <span id=\"PopupActivateTitle\">" + Dictionary.Common_Activate_Popup_Title + "</span>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-body\">";
        popup += "                    <div class=\"row\">";
        popup += "                        <div class=\"col-xs-2\">";
        popup += "                            <i class=\"ace-icon fa fa-recycle green fa-2x\"></i>";
        popup += "                        </div>";
        popup += "                        <div class=\"col-xs-10\"><p>" + Dictionary.Common_Activate_Popup_Question + "&nbsp;&quot;<strong><span id=\"ItemActivateName\"></span></strong>&quot;?</p></div>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-footer no-margin-top\">";
        popup += "                    <button type=\"button\" class=\"btn btn-sm pull-right\" data-dismiss=\"modal\" id=\"PopupActivateBtnCancel\">";
        popup += "                        <i class=\"ace-icon fa fa-times\"></i>&nbsp;" + Dictionary.Common_Cancel;
        popup += "                    </button>";
        popup += "                    <button type=\"button\" class=\"btn btn-sm btn-success pull-right\" onclick=\"ActivateItemConfirmed();\">";
        popup += "                        <i class=\"ace-icon fa fa-trash\"></i>&nbsp;" + Dictionary.Common_Activate_Popup_BtnOk;
        popup += "                    </button>";
        popup += "                </div>";
        popup += "            </div>";
        popup += "        </div>";
        popup += "    </div>";
        $("#PopupsContentHolder").append(popup);
    }

    popupContext.Activate = true;
}

function PopupRenderDelete() {
    if (popupContext.Delete === false) {
        var popup = "<div id=\"PopupDelete\" class=\"modal fade\" tabindex=\"-1\">";
        popup += "        <div class=\"modal-dialog\">";
        popup += "            <div class=\"modal-content\">";
        popup += "                <div class=\"modal-header no-padding\">";
        popup += "                    <div class=\"table-header bgred\">";
        popup += "                        <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">";
        popup += "                            <span class=\"white\">&times;</span>";
        popup += "                        </button>";
        popup += "                        <span id=\"PopupDeleteTitle\">" + Dictionary.Common_Delete + " " + PopupDeleteContext.ItemDefinition.Layout.Label.toLowerCase() + "</span>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-body\">";
        popup += "                    <div class=\"row\">";
        popup += "                        <div class=\"col-xs-2\">";
        popup += "                            <i class=\"ace-icon fa fa-trash red fa-2x\"></i>";
        popup += "                        </div>";
        popup += "                        <div class=\"col-xs-10\"><p>" + Dictionary.Common_Delete + "&nbsp;&quot;<strong>" + PopupDeleteContext.Message + "</strong>&quot;?</p></div>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-footer no-margin-top\">";
        popup += "                    <button type=\"button\" class=\"btn btn-sm pull-right\" data-dismiss=\"modal\" id=\"PopupDeleteBtnCancel\">";
        popup += "                        <i class=\"ace-icon fa fa-times\"></i>&nbsp;" + Dictionary.Common_Cancel;
        popup += "                    </button>";
        popup += "                    <button type=\"button\" class=\"btn btn-sm btn-danger pull-right\" onclick=\"Item_InactivateConfirm();\">";
        popup += "                        <i class=\"ace-icon fa fa-trash\"></i>&nbsp;" + Dictionary.Common_Delete_Popup_BtnOk;
        popup += "                    </button>";
        popup += "                </div>";
        popup += "            </div>";
        popup += "        </div>";
        popup += "    </div>";
        $("#PopupsContentHolder").append(popup);
    }

    popupContext.Delete = true;
}

function PopupRenderSessionTimeout() {
    if (popupContext.SessionTimeout === false) {
        var popup = "<div id=\"PopupSessionTimeout\" class=\"modal fade\" tabindex=\"-1\">";
        popup += "        <div class=\"modal-dialog\">";
        popup += "            <div class=\"modal-content\">";
        popup += "                <div class=\"modal-header no-padding\">";
        popup += "                    <div class=\"table-header bgred\">";
        popup += "                        <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">";
        popup += "                            <span class=\"white\">&times;</span>";
        popup += "                        </button>";
        popup += "                        <span id=\"PopupWarningTitle\">" + Dictionary.Common_SessionTimeoutMessage + "</span>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-body\">";
        popup += "                    <div class=\"row\">";
        popup += "                        <div class=\"col-xs-2\">";
        popup += "                            <i class=\"ace-icon fa fa-exclamation-triangle red fa-2x\"></i>";
        popup += "                        </div>";
        popup += "                        <div class=\"col-xs-10\"><p id=\"PopupWarningMessage\"></p></div>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "            </div>";
        popup += "        </div>";
        popup += "    </div>";
        $("#PopupsContentHolder").append(popup);
    }

    popupContext.SessionTimeout = true;
}

function PopupWarning(message, title, block, action) {
    PopupRenderWarning();
    $("#PopupWarningMessage").html(message);
    if (typeof title !== "undefined" && title !== null && title !== "") {
        $("#PopupWarningTitle").html(title);
    }
    else {
        $("#PopupWarningTitle").html(Dictionary.Common_Warning);
    }

    $("#LauncherPopupWaring").click();

    if (block === true) {
        $(".close").hide();
        $("#PopupWarningBtnCancel").hide();
    }
    else {
        $(".close").show();
        $("#PopupWarningBtnCancel").show();
    }
}

function PopupWorking(message) {
    $("#PopupWorkingMessage").html(message);
    $("#LauncherPopupWorking").click();
    PopupWarningLaunched = true;
}

function PopupWorkingHide() {
    if (PopupWarningLaunched === true) {
        $("#PopupWorkingBtnCancel").click();
        PopupWarningLaunched = false;
    }
}

function PopupDefault(config) {
    $("#PopupErrorDiv").hide();
    $("#PopupDefault .extrabutton").remove();
    $("#PopupDefaultPersistentFields").html("");
    $("#PopupDefaultErrorMessage").remove();
    $("#PopupDefault .customButton").remove();
    $("#PopupDefaultBody").css("margin-top", "0");
    $("#PopupDefault .extraFooterButtons").remove();

    // Eliminar mensajes de error anteriores
    $("#PopupErrorMessage").html("");

    if (typeof config.Width !== "undefined") {
        $("#PopupDefault .modal-content").width(config.Width);
    }
    else {
        $("#PopupDefault .modal-content").width(800);
    }

    if (HasPropertyEnabled(config.BtnDelete)) {
        $("#PopupDefaultBtnDelete").show();
    }
    else {
        $("#PopupDefaultBtnDelete").hide();
    }

    if (typeof config.BtnCancelText !== "undefined") {
        $("#PopupDefaultBtnCancel").html("<i class=\"fa fa-ban\"></i>&nbsp;" + config.BtnCancelText);
    }

    if (typeof config.BtnOk !== "undefined" && config.BtnOK !== null) {
        if (config.BtnOk === false) {
            $("#PopupDefaultBtnOk").hide();
        }
        else {
            var okConfig = config.BtnOk;
            var icon = HasPropertyValue(okConfig.Icon) ? okConfig.Icon : "fa fa-check";
            var text = HasPropertyValue(okConfig.Text) ? okConfig.Text : Dictionary.Common_Accept;
            var res = "<i class=\"ace-icon " + icon + "\"></i>&nbsp;" + text;
            $("#PopupDefaultBtnOk").html(res);
            ButtonSetOnclick("PopupDefaultBtnOk", config.BtnOk.Click);
            $("#PopupDefaultBtnOk").show();
        }
    }

    $("#PopupDefaultTitle").html(config.Title);
   //weke $("#PopupDefault .date-picker").datepicker({ "autoclose": true, "todayHighlight": true, "language": ApplicationUser.Language.JavaScriptISO });
    $("#LauncherPopupDefault").click();

    if (typeof config.AfterShowCallback === "function") {
        config.AfterShowCallback();
    }
}

function PopupLoginAndContinue() {
    if (popupContext.LoginAndContinue === true) {
        return;
    }
    PopupRenderLoginAndContinue();
    $("form").addClass("blur-filter");
    $("#LauncherPopupLoginAndContinue").click();
    $("#PopupLoginAndContinue .close").remove();
    $("#LAC").val("");
    $("#PopupLoginAndContinueBtnRelogin").html(Dictionary.Common_Accept);
    LAC = 3;
    popupContext.LoginAndContinue = true;
    localStorage.setItem("LAC", true);
}

function PopupBAR(itemDefinitionName) {
    var itemDefinition = ItemDefinitionByName(itemDefinitionName);
    var field = itemDefinition.ItemName + "Id";
    PopupBAR_Context.ItemDefinition = itemDefinition;
    PopupBAR_Context.FieldName = field;
    PopupBAR_Context.Value = $("#" + PopupBAR_Context.FieldName).val() * 1;
    PopupRenderBAR();
    PopupBARFillDataTable();
    $("#LauncherPopupBAR").click();
}

function PopupBARFillDataTable() {
    var data = FK[PopupBAR_Context.ItemDefinition.ItemName].Data;
    var tablePopup = "";
    for (var x = 0; x < data.length; x++) {
        if (data[x].Active === true) {
            tablePopup += "              <tr id=\"PopupBar_" + data[x].Id + "\">";

            var selectButton = PopupBAR_Context.Value === data[x].Id ? "" : "<button class=\"btn-icon\" title=\"" + Dictionary.Common_Select + "\" style=\"cursor:pointer;\" onclick=\"PopupBAR_Check(" + data[x].Id + ");\"><i class=\"fa fa-check green\"></i></button>";
            var editButton = HasPropertyEnabled(data[x].Core) ? "" : "<button class=\"btn-icon\" title=\"" + Dictionary.Common_Edit + "\" style=\"cursor:pointer;\" onclick=\"PopupBAR_Edit(" + data[x].Id + ");\"><i class=\"fa fa-pencil blue\"></i></button>";
            var deleteButton = HasPropertyEnabled(data[x].Core) || PopupBAR_Context.Value === data[x].Id ? "" : "<button class=\"btn-icon\" title=\"" + Dictionary.Common_Delete + "\" style=\"cursor:pointer;\" onclick=\"PopupBAR_Delete(" + data[x].Id + ");\"><i class=\"fa fa-times red\"></i></button>";
            var text = PopupBAR_Context.Value !== data[x].Id ? data[x].Description : "<strong>" + data[x].Description + "</strong>";

            tablePopup += "                <td id=\"PopupBar_Description_" + data[x].Id + "\">" + text + "</td>";
            tablePopup += "                <td style=\"width:30px;\">" + selectButton + "</td>";
            tablePopup += "                <td style=\"width:30px;\">" + editButton + "</td>";
            tablePopup += "                <td style=\"width:30px;\">" + deleteButton + "</td>";

            tablePopup += "              </tr>";
        }
    }

    $("#PopupBARTable").html(tablePopup);
}

function PopupInfo(message, title, action) {
    PopupRenderInfo();
    PopupInfo_Context.CallBack = action;
    $("#PopupInfoMessage").html(message);
    if (typeof title !== "undefined" && title !== null && title !== "") {
        $("#PopupInfoTitle").html(title);
    }
    else {
        $("#PopupInfoTitle").html(Dictionary.Common_Info);
    }

    $("#LauncherPopupInfo").click();
}

function PopupSaveInfo(message) {
    PopupRenderSaveInfo();

    if (PopupRenderSaveInfoPostMessage.length > 0) {
        message + "<br /><ul>";
        for (var x = 0; x < PopupRenderSaveInfoPostMessage.length; x++) {
            message += "<li>" + PopupRenderSaveInfoPostMessage[x] + "</li>";
        }
        message += "</ul>";
    }

    PopupRenderSaveInfoPostMessage = [];

    $("#PopupSaveInfoMessage").html(message);
    if (typeof title !== "undefined" && title !== null && title !== "") {
        $("#PopupSaveInfoTitle").html(title);
    }
    else {
        $("#PopupSaveInfoTitle").html(Dictionary.Common_Info);
    }

    $("#LauncherSavePopupInfo").click();
}

function PopupSuccess(message, title, action) {
    PopupRenderSuccess();
    PopupSuccess_Context.CallBack = action;
    $("#PopupSuccessMessage").html(message);
    if (typeof title !== "undefined" && title !== null && title !== "") {
        $("#PopupSuccessTitle").html(title);
    }
    else {
        $("#PopupSuccessTitle").html(Dictionary.Common_Success_Title);
    }

    $("#LauncherPopupSuccess").click();
}

function NotifySuccess(message, title, action) {
    $.notify({
        "icon": "fa fa-check",
        "title": title,
        "message": message
    }, {
        "onShow": action,
        "delay": 100,
        "type": "success",
        "template": '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
            '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
            '<span data-notify="icon"></span> ' +
            '<span data-notify="title"><strong>{1}</strong></span><br />' +
            '<span data-notify="message">{2}</span>' +
            '</div>'
    });
}

function NotifyError(message, title, action) {
    $.notify({
        "icon": "fa fa-exclamation-triangle",
        "title": title,
        "message": message
    }, {
        "onShow": action,
        "delay": 100,
        "type": "danger",
        "template": '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
            '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
            '<span data-notify="icon"></span> ' +
            '<span data-notify="title"><strong>{1}</strong></span><br />' +
            '<span data-notify="message">{2}</span>' +
            '</div>'
    });
}

function NotifyMessagingFromBoss(message, title, action) {
    $.notify({
        "icon": "fa fa-comments",
        "title": title,
        "message": message
    }, {
        "onShow": action,
        "delay": -1,
        "type": "danger",
        "template": '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
            '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
            '<span data-notify="icon"></span> ' +
            '<span data-notify="title"><strong>{1}</strong></span><br />' +
            '<span data-notify="message">{2}</span>' +
            '</div>'
    });
}

function PopupConfirm(message, title, actionYes, actionNo) {
    if (typeof actionYes === "undefined") { actionYes = null; }
    if (typeof actionNo === "undefined") { actionNo = null; }
    PopupRenderConfirm();
    PopupConfirm_Context.YesCallBackAction = actionYes;
    PopupConfirm_Context.NoCallBackAction = actionNo;
    $("#PopupConfirmMessage").html(message);
    if (typeof title !== "undefined" && title !== null && title !== "") {
        $("#PopupConfirmTitle").html(title);
    }
    else {
        $("#PopupConfirmTitle").html(Dictionary.Common_Confirm_Title);
    }

    $("#LauncherPopupConfirm").click();
}

function PopupConfirmLaunchYes() {
    console.log("PopupConfirmLaunchYes");
    if (PopupConfirm_Context.YesCallBackAction !== null) {
        PopupConfirm_Context.YesCallBackAction();
    }

    if (AfterLoginPopupActive === true) {
        $("#AfterLoginLocker").show();
        $("#AfterLoginContent").show();
    }
}

function PopupConfirmLaunchNo() {
    console.log("PopupConfirmLaunchNo");
    if (PopupConfirm_Context.NoCallBackAction !== null) {
        PopupConfirm_Context.NoCallBackAction();
    }

    if (AfterLoginPopupActive === true) {
        $("#AfterLoginLocker").show();
        $("#AfterLoginContent").show();
    }
}

function PopupInfoCloseCallBack() {
    if (typeof PopupInfo_Context.CallBack !== "undefined" && PopupInfo_Context.CallBack !== null) {
        PopupInfo_Context.CallBack();
    }
}

function PopupSuccessCloseCallBack() {
    if (typeof PopupSuccess_Context.CallBack !== "undefined" && PopupSuccess_Context.CallBack !== null) {
        PopupSuccess_Context.CallBack();
    }
}