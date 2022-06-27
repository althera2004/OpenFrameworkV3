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

var PopupDefault_Context = {};
var PopupWarningLaunched = false;
var PopupActivateCallBackAction = null;
var PopupDeleteCallBackAction = null;

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
    "DocumentGallery": false
};

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
        popup += "                        <span id=\"PopupDeleteTitle\">" + Dictionary.Common_Delete_Popup_Title + "</span>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-body\">";
        popup += "                    <div class=\"row\">";
        popup += "                        <div class=\"col-xs-2\">";
        popup += "                            <i class=\"ace-icon fa fa-trash red fa-2x\"></i>";
        popup += "                        </div>";
        popup += "                        <div class=\"col-xs-10\"><p>" + Dictionary.Common_Delete + "&nbsp;&quot;<strong><span id=\"ItemDeleteName\"></span></strong>&quot;?</p></div>";
        popup += "                    </div>";
        popup += "                </div>";
        popup += "                <div class=\"modal-footer no-margin-top\">";
        popup += "                    <button type=\"button\" class=\"btn btn-sm pull-right\" data-dismiss=\"modal\" id=\"PopupDeleteBtnCancel\">";
        popup += "                        <i class=\"ace-icon fa fa-times\"></i>&nbsp;" + Dictionary.Common_Cancel;
        popup += "                    </button>";
        popup += "                    <button type=\"button\" class=\"btn btn-sm btn-danger pull-right\" onclick=\"DeleteItemConfirmed();\">";
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