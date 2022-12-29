function COMPANYADDRESS_RenderTable() {
    var res = "";
    for (var b = 0; b < CompanyAddreses.length; b++) {
        res += COMPANYADDRESS_RenderRow(CompanyAddreses[b]);
    }

    $("#COMPANYADDRESS_TBody").html(res);
    $("#COMPANYADDRESS_TotalRows").html(CompanyAddreses.length);
}


function COMPANYADDRESS_RenderRow(data) {
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

function COMPANYADDRESS_ShowForm() {
    $("#CompanyAddressForm").show();
    var formHeight = $("#CompanyAddressForm").outerHeight();

    $("#CompanyAddressList .panel-body").height($("#CompanyAddressList .panel-body").height() - formHeight);
    $("#COMPANYADDRESS_AddBtn").hide();
}

function COMPANYADDRESS_HideForm() {
    var formHeight = $("#CompanyAddressForm").outerHeight();
    $("#CompanyAddressForm").hide();

    $("#CompanyAddressList .panel-body").height($("#CompanyAddressList .panel-body").height() + formHeight);
    $("#BANKACCOUNT_AddBtn").show();
}

function COMPANYADDRESS_Add() {
    COMPANYADDRESS_ShowForm();
}

function COMPANYADDRESS_AddCancel() {
    COMPANYADDRESS_HideForm();
}

function COMPANYADDRESS_Edit(id) {
    var bankAccount = COMPANYADDRESS_ById(id);
    $("#BankName").val(bankAccount.BankName);
    $("#IBAN").val(bankAccount.IBAN);
    $("#SWIFT").val(bankAccount.Swift);
    $("#Alias").val(bankAccount.Alias);
    $("#ContractId").val(bankAccount.ContractId);
    $("#CBMain").prop("checked", bankAccount.Main);
    $("#PaymentType").val(bankAccount.PaymentType);
    $("#RBPaymentPRE").prop("checked", bankAccount.PaymentType === "PRE");
    $("#RBPaymentFSDD").prop("checked", bankAccount.PaymentType === "FSDD");

    COMPANYADDRESS_ShowForm();
}