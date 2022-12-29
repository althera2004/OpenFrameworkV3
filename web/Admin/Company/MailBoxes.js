function CustomActions() {
    $("#ChkSameAddress").on("click", MAILBOXES_ChkSameAddress_Changed);
    MAILBOXES_ChkSameAddress_Changed();
}

function MAILBOXES_ChkSameAddress_Changed() {
    if ($("#ChkSameAddress").prop("checked") === true) {
        $("#Third_MailAddress").disable();
        $("#Third_SenderName").disable();
        $("#Third_Server").disable();
        $("#ThirdMailBoxType").disable();
        $("#Third_MailUser").disable();
        $("#Third_MailPassword").disable();
        $("#Third_SendPort").disable();
        $("#ThirdBtnSendTest").disable();
        $("#ThirdBtnCheck").disable();
    }
    else {
        $("#Third_MailAddress").enable();
        $("#Third_SenderName").enable();
        $("#Third_Server").enable();
        $("#ThirdMailBoxType").enable();
        $("#Third_MailUser").enable();
        $("#Third_MailPassword").enable();
        $("#Third_SendPort").enable();
        $("#Third_SSL").enable();
        $("#ThirdBtnSendTest").enable();
        $("#ThirdBtnCheck").enable();
    }
}

function MAILBOXES_CheckBlackListMain() {
    if ($("#MainMailMadress").val() === "") {
        PopupWarning(Dictionary.Core_MailBox_Error_EmailRequired, Dictionary.Common_Warning);
    }
    else {
        MAILBOXES_CheckBlackList($("#MainMailMadress").val().split('@')[1]);
    }
}

function MAILBOXES_CheckBlackList(domainName) {
    window.open("https://mxtoolbox.com/SuperTool.aspx?action=blacklist%3a" + domainName);
}

function MAILBOXES_Validate() {
    var ok = true;
    var errorMessages = [];

    if ($("#MainMailMadress").val() === "") {
        ok = false;
        errorMessages.push(Dictionary.Core_MailBox_Error_EmailRequired);
    } else {
        if (validateEmail($("#MainMailMadress").val()) === false) {
            ok = false;
            errorMessages.push(Dictionary.Core_MailBox_Error_EmailMalformed);
        }
    }

    if ($("#MainServer").val() === "") {
        ok = false;
        errorMessages.push(Dictionary.Core_MailBox_Error_ServerRequired);
    }

    if ($("#MainMailUser").val() === "") {
        ok = false;
        errorMessages.push(Dictionary.Core_MailBox_Error_UserRequired);
    }

    if ($("#MainMailPassword").val() === "") {
        ok = false;
        errorMessages.push(Dictionary.Core_MailBox_Error_PasswordRequired);
    }

    if ($("#MainSendPort").val() === "") {
        ok = false;
        errorMessages.push(Dictionary.Core_MailBox_Error_PortRequired);
    }

    if (ok === false) {
        var errorText = Dictionary.Common_ErrorResumeTitle + ":<ul>";
        for (var e = 0; e < errorMessages.length; e++) {
            errorText += "<li>" + errorMessages[e] + "</li>";
        }

        errorText += "</ul>";
        PopupWarning(errorText, Dictionary.Common_Warning);
    }

    return ok;
}

function MAILBOXES_SaveMain() {

    if (MAILBOXES_Validate() === false) {
        return;
    }

    var newMailBox = {
        "Id": typeof mainAddress.Id === "undefined" ? -1 : mainAddress.Id,
        "CompanyId": Company.Id,
        "Main": true,
        "MailAddress": $("#MainMailMadress").val(),
        "SenderName": $("#MainSenderName").val(),
        "MailUser": $("#MainMailUser").val(),
        "MailPassword": $("#MainMailPassword").val(),
        "Server": $("#MainServer").val(),
        "SendPort": $("#MainSendPort").val() * 1,
        "ReadPort": typeof mainAddress.ReadPort === "undefined" ? 0 : mainAddress.ReadPort,
        "MailBoxType": $("#MainMailBoxType").val(),
        "SSL": $("#MainSSL").prop("checked") === true,
        "Description": "",
        "Active": true
    }
    var data = {
        "mailBox": newMailBox,
        "applicationUserId": ApplicationUser.Id,
        "instanceName": Instance.Name
    };

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/CompanyService.asmx/MailBoxSave",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg);

            if (msg.d.Success === true) {

                var action = msg.d.ReturnValue.split('|')[0];
                var id = msg.d.ReturnValue.split('|')[1];

                mainAddress = newMailBox;
                mainAddress.Id = id * 1;

                var text = "Dirección de correo " + (action === "UPDATE" ? "modificada" : "añadida") + " correctamente.";

                $("#MainBtnSave").notify(text, { "position": "top", "className": "success" });
            }
            else {
                PopupWarning(msg.d.MessageError, Dictionary.Common_Warning);
            }

        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}