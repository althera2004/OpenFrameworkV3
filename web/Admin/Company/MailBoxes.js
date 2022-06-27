function CustomActions() {
    console.log("wwww");

    $("#ChkSameAddress").on("click", MAILBOXES_ChkSameAddress_Changed);
    MAILBOXES_ChkSameAddress_Changed();
}

function MAILBOXES_ChkSameAddress_Changed() {
    if ($("#ChkSameAddress").prop("checked") === true) {
        $("#Third_MailAddress").disable();
        $("#Third_SenderName").disable();
        $("#Third_Server").disable();
        $("#Third_MailUser").disable();
        $("#Third_MailPassword").disable();
        $("#Third_SendPort").disable();
    }
    else {
        $("#Third_MailAddress").enable();
        $("#Third_SenderName").enable();
        $("#Third_Server").enable();
        $("#Third_MailUser").enable();
        $("#Third_MailPassword").enable();
        $("#Third_SendPort").enable();
    }
}

function MAILBOXES_CheckBlackListMain() {
    MAILBOXES_CheckBlackList($("#MainMailMadress").val().split('@')[1]);
}

function MAILBOXES_CheckBlackList(domainName) {
    window.open("https://mxtoolbox.com/SuperTool.aspx?action=blacklist%3a" + domainName);
}

function MAILBOXES_SaveMain() {
    var newMailBox = {
        "Id": mainAddress.Id,
        "CompanyId": mainAddress.CompanyId,
        "Main": true,
        "MailAddress": $("#MainMailMadress").val(),
        "SenderName": $("#MainSenderName").val(),
        "MailUser": $("#MainMailUser").val(),
        "MailPassword": $("#MainMailPassword").val(),
        "Server": $("#MainServer").val(),
        "SendPort": $("#MainSendPort").val() * 1,
        "ReadPort": mainAddress.ReadPort,
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
            mainAddress = newMailBox;

            Swal.fire({
                "icon": msg.d.Success ? 'success' : 'error',
                "title": msg.d.Success ? "Good job!" : "Oh!",
                "text": msg.d.Success ? msg.d.ReturnValue : msg.d.MessageError
            });
        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}