window.onload = function () {
    $("#nav-search").remove();

    $("#userEmail").html(ApplicationUser.Email);
    $("#SpanUserName").html(ApplicationUser.Profile.FullName);
    $("#firstName").html(ApplicationUser.Profile.Name);
    $("#lastName").html(ApplicationUser.Profile.LastName);
    $("#lastName2").html(ApplicationUser.Profile.LastName2);
    $("#BreadCrumbLabel").html(" " + ApplicationUser.Profile.FullName);

    $("#avatar").attr("src", "/Instances/" + Instance.Name + "/Data/Core_Users/" + ApplicationUser.Id + "/Avatar.jpg");
    $("#avatar").attr("alt", ApplicationUser.Profile.FullName);
    $("#signature").attr("src", "/Instances/" + Instance.Name + "/Data/Core_Users/" + ApplicationUser.Id + "/signature.png");
    PROFILE_FooterLayout();
    PROFILE_PasswordLayout();
    ResizeWorkArea();
}

function PROFILE_PasswordLayout() {
    $("#PasswordErrorMessageMinValue").html(Instance.Security.MinimumPasswordLength);
    $("#ComplexityText").html("Mayúsculas, minúsculas, cifras, caracteres especiales")
}

function PROFILE_FooterLayout() {
    $("#FormBtnDelete").remove();
    $("#FooterStatus").html("");

    var buttons = "<button class=\"btn btn-info\" type=\"button\" id=\"FormBtnPassword\" onclick=\"PROFILE_LaunchPopupChangePassword();\"><i class=\"fa fa-key bigger-110\"></i><span class=\"hidden-xs\" style=\"float: right; margin-left:4px;\">Canviar contrasenya</span></button>";
    buttons += "<div class=\"btn-group dropup\"> \
        <button type =\"button\" id=\"dropdown-menu-up-launcher-signature\" data-toggle=\"dropdown\" class=\"btn btn-info dropdown-toggle\" aria-expanded=\"false\">\
            <span id =\"dropdown-menu-up1-launcherLabel\">Signatura </span>&nbsp;<span class=\"ace-icon fa fa-caret-up icon-only\"></span></button>\
                <ul class=\"dropdown-menu dropdown-inverse dropdown-menu-up\" id=\"dropdown-menu-up1\" style=\"text-align: left; top: -226px;\">\
                    <li id =\"BtnActionSignature\"><a href=\"#\"><i class=\"fa fa fa-signature bigger-110\" style=\"width: 25px;\"></i><span id=\"BtnActionLock_Label\">Signar amb tauleta</span></a></li>\
                    <li id =\"BtnActionSignatureQR\"><a href=\"#\"><i class=\"fa fa fa-qrcode bigger-110\" style=\"width: 25px;\"></i><span id=\"BtnActionLock_Label\">Signar amb mòbil</span></a></li>\
                    <li id =\"BtnActionUploadSignature\"><a href=\"#\"><i class=\"fa fa fa-upload bigger-110\" style=\"width: 25px;\"></i><span id=\"BtnActionLock_Label\">Pujar signatura</span></a></li>\
        </li></ul></span></div>";
    $("#FormBtnSave").before(buttons);
};

function ResizeWorkArea() {
    $(".panel-body").height($(window).height() - 220);
}

function PROFILE_LaunchPopupChangePassword() {
    $("#ComplexityText").html(Dictionary["Instance_Security_PasswordComplexity_" + Instance.Security.PasswordComplexity]);
    $("#PasswordActual").val("");
    $("#PasswordNew").val("");
    $("#PasswordConfirmation").val("");
    $("#resultQuality").html("");
    $("#resultConfirmation").hide();
    $("#PopupChangePasswordOk").disable();
    $("#PopupChangePasswordErrorMessage").hide();
    $("#LauncherPopupChangePassword").click();
    Profile_PasswordResetErrors();
}

function Profile_PasswordResetErrors() {
    // Ocultar mensajes de error
    $("#PasswordErrorActual").hide();
    $("#PasswordErrorConfirmation").hide();
    $("#PasswordErrorMessageMin").css("color", LayoutColor.Label);
    $("#PasswordErrorNew").html("");

}

function PROFILE_PasswordValidation() {
    var ok = true;
    Profile_PasswordResetErrors();
    var actual = $("#PasswordActual").val();
    var newPass = $("#PasswordNew").val();
    var newConf = $("#PasswordConfirmation").val();

    if (actual === "") {
        $("#PasswordErrorActual").show();
    }

    if (newPass !== "") {
        if (newPass.length < Instance.Security.MinimumPasswordLength) {
            $("#PasswordErrorMessageMin").css("color", LayoutColor.Error);
            ok = false;
        }
        PROFILE_PasswordStrengthChecker(newPass);
    }
    else {
    }

    if (newConf !== "") {
        if (newConf !== newPass) {
            $("#PasswordErrorConfirmation").show();
            ok = false;
        }
    }

    if (ok === false) {
        $("#PopupChangePasswordOk").disable();
    }
    else {
        $("#PopupChangePasswordOk").enable();
    }
}

function PROFILE_ChangePasswordGo() {
    $("#PopupChangePasswordError").html("");
    $("#PopupChangePasswordBtnCancel").disable();
    $("#PopupChangePasswordOk").disable();
    var credential = ApplicationUser.Id + "||||" + $("#PasswordActual").val() + "||||" + $("#PasswordNew").val() + "||||" + Instance.Name + '||||';
    var data = {
        "credential": btoa(unescape(encodeURIComponent(credential)))
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/SecurityService.asmx/ChangePassword",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg);
            if (msg.d.ReturnValue < 0) {
                $("#PopupChangePasswordError").html("<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;" + Dictionary.PopupChangePassword_ErrorMessage);
                $("#PopupChangePasswordBtnCancel").enable();
                $("#PopupChangePasswordOk").enable();
            }
            else {
                $("#PopupChangePasswordBtnCancel").click();
                PopupInfo(Dictionary.PopupChangePassword_SuccessMessage, Dictionary.Common_Success, CORE_Logout_Confirmed);
            }
        },
        "error": function (msg) {
            $("#PopupChangePasswordBtnCancel").enable();
            $("#PopupChangePasswordOk").enable();
        }
    });
}

function PROFILE_PasswordStrengthChecker(PasswordParameter) {
    let strongPassword = new RegExp('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])(?=.{8,})')
    let mediumPassword = new RegExp('((?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])(?=.{6,}))|((?=.*[a-z])(?=.*[A-Z])(?=.*[^A-Za-z0-9])(?=.{8,}))')


    if (strongPassword.test(PasswordParameter)) {
        $("#PasswordErrorNew").html("<i class=\"fa fa-check\"></i> strong");
        $("#PasswordErrorNew").css("background-color", "#87B87F");
    } else if (mediumPassword.test(PasswordParameter)) {
        $("#PasswordErrorNew").html("<i class=\"fa fa-exclamation\"></i> medim");
        $("#PasswordErrorNew").css("background-color", "#FFB752");
    } else {
        $("#PasswordErrorNew").html("<i class=\"fa fa-exclamation-triangle\"></i> weak");
        $("#PasswordErrorNew").css("background-color", "#D15B47");
    }
}