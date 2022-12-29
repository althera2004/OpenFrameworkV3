function BANKACCOUNT_RenderTable() {
    var res = "";
    for (var b = 0; b < BankAccounts.length; b++) {
        res += BANKACCOUNT_RenderRow(BankAccounts[b]);
    }

    $("#BANKACCOUNT_TBody").html(res);
    $("#BANKACCOUNT_TotalRows").html(BankAccounts.length);
}


function BANKACCOUNT_RenderRow(data) {
    var res = "";
    res += "<tr>";
    res += "    <td style=\"width:250px;\">" + data.IBAN + "</td>";
    res += "    <td>" + data.BankName + "</td>";
    res += "    <td style=\"width:250px;\">" + data.Alias + "</td>";
    res += "    <td style=\"width:120px;text-align: center;\">" + (data.Main ? "<i class=\"fa fa-check\"></i>" : "") + "</td>";
    res += "    <td class=\"action-buttons\" data-buttons=\"2\" style=\"width: 77px; white-space: nowrap;\">";
    res += "      <a onclick=\"BANKACCOUNT_Edit(" + data.Id + ");\" class=\"BankAccountActionButton\">";
    res += "        <i class=\"fa fa-pencil-alt\"></i>";
    res += "      </a>";
    res += "      <a onclick=\"BANKACCOUNT_Delete(" + data.Id + ")\" class=\"BankAccountActionButton\">";
    res += "        <i class=\"fa fa-times red\"></i>";
    res += "      </a>";
    res += "    </td>";
    res += "</tr>";
    return res;
}

var BANKACCOUNT_FormShowed = false;
function BANKACCOUNT_ShowForm() {
    BANKACCOUNT_FormShowed = true;
    $("#BankAccountForm").show();
    var formHeight = $("#BankAccountForm").outerHeight();

    $("#BankAccountList .panel-body").height($("#BankAccountList .panel-body").height() - formHeight);
    $("#BANKACCOUNT_AddBtn").hide();
}

function BANKACCOUNT_HideForm() {
    BANKACCOUNT_FormShowed = false;
    var formHeight = $("#BankAccountForm").outerHeight();
    $("#BankAccountForm").hide();

    $("#BankAccountList .panel-body").height($("#BankAccountList .panel-body").height() + formHeight);
    $("#BANKACCOUNT_AddBtn").show();
}

function BANKACCOUNT_Add() {
    if (BANKACCOUNT_FormShowed) {
        PopupWarning("Editando", Dictionary.Common_Warning);
        return false;
    }

    $("#BANKACCOUNT_Id").val(-1);
    $("#BankName").val("");
    $("#IBAN").val("");
    $("#SWIFT").val("");
    $("#Alias").val("");
    $("#ContractId").val("");
    $("#CBMain").prop("checked", false);
    $("#PaymentType").val(0);
    $("#RBPaymentPRE").prop("checked", false);
    $("#RBPaymentFSDD").prop("checked", false);
    $("#BANKACCOUNTBtnDelete").hide();
    BANKACCOUNT_ShowForm();
}

function BANKACCOUNT_AddCancel() {
    $("#BANKACCOUNTBtnDelete").show();
    BANKACCOUNT_HideForm();
}

function BANKACCOUNT_Edit(id) {
    if (BANKACCOUNT_FormShowed) {
        PopupWarning("Editando", Dictionary.Common_Warning);
        return false;
    }
    var bankAccount = BANKACCOUNT_ById(id);
    $("#BANKACCOUNT_Id").val(id);
    $("#BankName").val(bankAccount.BankName);
    $("#IBAN").val(bankAccount.IBAN);
    $("#SWIFT").val(bankAccount.Swift);
    $("#Alias").val(bankAccount.Alias);
    $("#ContractId").val(bankAccount.ContractId);
    $("#CBMain").prop("checked", bankAccount.Main);
    $("#PaymentType").val(bankAccount.PaymentType);
    $("#RBPaymentPRE").prop("checked", bankAccount.PaymentType === "PRE");
    $("#RBPaymentFSDD").prop("checked", bankAccount.PaymentType === "FSDD");

    BANKACCOUNT_ShowForm();
}

function BANKACCOUNT_ById(id) {
    for (var b = 0; b < BankAccounts.length; b++) {
        if (BankAccounts[b].Id === id) {
            return BankAccounts[b];
        }
    }

    return null;
}

function BANKACCOUNT_Save() {
    if (BANKACCOUNT_FormValidate() == false) {
        return;
    }

    var data = {
        "account": {
            "Id": $("#BANKACCOUNT_Id").val() * 1,
            "CompanyId": Company.Id,
            "IBAN": $("#IBAN").val(),
            "Swift": $("#SWIFT").val(),
            "BankName": $("#BankName").val(),
            "Main": $("#CBMain").prop("checked"),
            "Alias": $("#Alias").val(),
            "ContractId": $("#ContractId").val(),
            "PaymentType": $("#RBPaymentPRE").prop("checked") ? 1 : ($("#RBPaymentFSDD").prop("checked") ? 2 : 0)
        },
        "applicationUserId": ApplicationUser.Id,
        "instanceName": Instance.Name
    }

    console.log(data);

    $.ajax({
        "type": "POST",
        "url": "/Async/CompanyService.asmx/BankAccountSave",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg);

            if (msg.d.Success === true) {
                BANKACCOUNT_HideForm();
                BANKACCOUNT_ByCompany();

                var text = "IBAN " + (action === "UPDATE" ? "modificada" : "añadida") + " correctamente.";

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

function BANKACCOUNT_FormValidate() {
    var ok = true;
    return ok;
}

function BANKACCOUNT_ByCompany() {
    var data = {
        "companyId": Company.Id,
        "instanceName": Instance.Name
    };
    $.ajax({
        "type": "POST",
        "url": "/Async/CompanyService.asmx/BankAccountByCompany",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg.d);

            eval("BankAccounts=" + msg.d + ";");
            BANKACCOUNT_RenderTable();

        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}

function BANKACCOUNT_Delete(id) {
    if (typeof id === "undefined") {
        id = $("#BANKACCOUNT_Id").val() * 1;
    }
    var bankAccount = BANKACCOUNT_ById(id);
    if (bankAccount.Main === true) {
        PopupWarning(Dictionary.Feature_BankAccount_ErrorMessage_MainUndelete);
        return false;
    }
}