window.onload = function () {
    $("#Email").val(userData.Email);
    $("#IMEI").val(userData.IMEI);
    $("#FirstName").val(userData.Profile.Name);
    $("#LastName").val(userData.Profile.LastName);
    $("#LastName2").val(userData.Profile.LastName2);

    $("#Core").prop("checked", userData.Core);
    $("#Principal").prop("checked", userData.AdminUser);

    $("#FormBtnCancel").on("click", function () { GoEncryptedPage('/Admin/Security/UserList.aspx'); });
    ReplaceClick("FormBtnSave", SECURITYUSER_Save);
    ReplaceClick("FormBtnDelete", SECURITYUSER_Delete);
    $("#FormBtnSave").enable();
    $("#FooterStatus").invisible();
    $("#logofooter").show();
}

function SECURITYUSER_Save() {
    alert("SECURITYUSER_Save");
}

function SECURITYUSER_Delete() {
    alert("SECURITYUSER_Delete");
}