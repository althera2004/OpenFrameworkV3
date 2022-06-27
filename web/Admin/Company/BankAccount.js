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
    res += "      <a class=\"btn btn-xs btn-circle btn-info-white\" id=\"101\" onclick=\"BANKACCOUNT_Edit(" + data.Id + ");\">";
    res += "        <i class=\"fa fa-pencil-alt\"></i>";
    res += "      </a>";
    res += "      <a class=\"btn btn-xs btn-circle btn-danger\" id=\"101\" onclick=\"BANKACCOUNT_Delete(" + data.Id + ")\">";
    res += "        <i class=\"fa fa-times\"></i>";
    res += "      </a>";
    res += "    </td>";
    res += "</tr>";
    return res;
}

function BANKACCOUNT_ShowForm() {
    $("#BankAccountForm").show();
    var formHeight = $("#BankAccountForm").outerHeight();

    $("#BankAccountList .panel-body").height($("#BankAccountList .panel-body").height() - formHeight);
    $("#BANKACCOUNT_AddBtn").hide();
}

function BANKACCOUNT_HideForm() {
    var formHeight = $("#BankAccountForm").outerHeight();
    $("#BankAccountForm").hide();

    $("#BankAccountList .panel-body").height($("#BankAccountList .panel-body").height() + formHeight);
    $("#BANKACCOUNT_AddBtn").show();
}

function BANKACCOUNT_Add() {
    BANKACCOUNT_ShowForm();
}

function BANKACCOUNT_AddCancel() {
    BANKACCOUNT_HideForm();
}

function BANKACCOUNT_Edit(id) {
    var bankAccount = BANKACCOUNT_ById(id);
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