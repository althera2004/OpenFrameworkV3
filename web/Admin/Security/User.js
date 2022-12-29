window.onload = function () {
    SECURITYUSER_SetLayout();

    if (originalUserData.Id < 0) {
        $("#BreadCrumbLabel").html(Dictionary.Core_ApplicationUser_NewUser);
    }
    else {
        $("#BreadCrumbLabel").html(userData.Email + " - " + userData.Profile.FullName);
    }

    $("#Email").val(userData.Email);
    $("#IMEI").val(userData.IMEI);
    $("#FirstName").val(userData.Profile.Name);
    $("#LastName").val(userData.Profile.LastName);
    $("#LastName2").val(userData.Profile.LastName2);

    $("#Core").prop("checked", userData.Core);
    $("#Principal").prop("checked", userData.AdminUser);

    // Tipus d'usuari
    // ----------------------------
    if (userData.Core === true) { $("#Core").prop("checked", true); }
    if (userData.AdminUser === true) { $("#Owner").prop("checked", true); }
    // ----------------------------

    // Dades personals
    // ----------------------------
    $("#Phone").val(userData.Profile.Phone);
    $("#EmergencyPhone").val(userData.Profile.EmergencyPhone);
    $("#Mobile").val(userData.Profile.Mobile);
    $("#IdentificationCard").val(userData.Profile.IdentificationCard);
    $("#EmailAlternative").val(userData.Profile.EmailAlternative);
    // ----------------------------


    // Xarxes socials
    // ----------------------------
    $("#LinkedIn").val(userData.Profile.LinkedIn);
    $("#Twitter").val(userData.Profile.Twitter);
    $("#Instagram").val(userData.Profile.Instagram);
    $("#Facebook").val(userData.Profile.Facebook);
    // ----------------------------

    $("#FormBtnCancel").on("click", function () { GoEncryptedPage('/Admin/Security/UserList.aspx'); });
    ReplaceClick("FormBtnSave", SECURITYUSER_Save);
    ReplaceClick("FormBtnDelete", SECURITYUSER_Delete);
    $("#FormBtnSave").enable();
    $("#FormBtnSave").show();
    $("#FooterStatus").invisible();
    $("#logofooter").show();
    var demo2 = $('.demo2').bootstrapDualListbox({
        nonSelectedListLabel: 'Grups disponibles',
        selectedListLabel: 'Grups seleccionats',
        preserveSelectionOnMove: 'moved',
        moveOnSelect: false,
        nonSelectedFilter: '',
        selectorMinimalHeight: 300,
        infoText: 'Mostrant tots {0}',
        infoTextFiltered: '<span class="badge badge-warning">Mostrant</span> {0} de {1}',
        infoTextEmpty: 'Cap'
    });
}

function SECURITYUSER_SetLayout() {
    if (typeof Instance.Profile !== "undefined") {
        if (Instance.Profile.NameFormat === 0) {
            $("#LastName").hide();
            $("#LastName2").hide();
            $("#LastName_Label").hide();
            $("#FirstName").parent().replaceClass("col-sm-2", "col-sm-4");
        }
        else if (Instance.Profile.NameFormat === 1) {
            $("#LastName2").hide();
            $("#LastName").css("width", "75%");
        }
    }

    if (Instance.Profile.Gender === true) { $("#DivGender").show(); }
    if (Instance.Profile.BirthDate === true) { $("#DivBirthDate").show(); }
    if (Instance.Profile.IdentificationCard === true) { $("#DivIdentificationCard").show(); }
    if (Instance.Profile.Nacionality === true) { $("#DivNacionality").show(); }
    if (Instance.Profile.Mobile === true) { $("#DivMobile").show(); }
    if (Instance.Profile.Fax === true) { $("#DivFax").show(); }
    if (Instance.Profile.EmailAlternative === true) { $("#DivEmailAlternative").show(); }

    var existsSocial = false;
    if (Instance.Profile.LinkedIn === true) {
        existsSocial = true; $("#DivLinkedIn").show();
    }
    if (Instance.Profile.Twiter === true) {
        existsSocial = true; $("#DivTwitter").show();
    }
    if (Instance.Profile.Instagram === true) {
        existsSocial = true; $("#DivInstagram").show();
    }
    if (Instance.Profile.Facebook === true) {
        existsSocial = true; $("#DivFacebook").show();
    }

    if (existsSocial === true) {
        $("#SocialTitle").show();
        $("#SocialData").show();
    }

}

function SECURITYUSER_Save() {
    // user, long applicationUserId, long companyId, string instanceName
    var data = {
        "user": {
            "Id": userData.Id,
            "Email": $("#Email").val(),
            "Password": "*******************",
            "Language": { "Id": 1},
            "Profile": {
                "ApplicationUserId": userData.Id,
                "Name": $("#FirstName").val(),
                "LastName": $("#LastName").val(),
                "LastName2": $("#LastName2").val(),
                "Phone": $("#Phone").val(),
                "Mobile": $("#Mobile").val(),
                "Fax": $("#Fax").val(),
                "IMEI": null,
                "EmailAlternative": $("#EmailAlternative").val(),
                "IdentificationCard": null,
                "Nacionality": 0,
                "Gender": 0
            },
            "Grants": [],
            "Groups": []
        },
        "applicationUserId": ApplicationUser.Id,
        "companyId": Company.Id,
        "instanceName": Instance.Name
    }

    $.ajax({
        "type": "POST",
        "url": "/Async/SecurityService.asmx/ApplicationUserSave",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg);
            var result = msg.d.ReturnValue;
            console.log(result);
            SECURITYUSER_ById(result.split('|')[1] * 1);
        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}

function SECURITYUSER_Delete() {
    alert("SECURITYUSER_Delete");
}

function SECURITYUSER_ById(applicationUserId) {
    var data = {
        "applicationUserId": applicationUserId,
        "instanceName": Instance.Name
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/SecurityService.asmx/ApplicationUserById",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            console.log(msg);
            eval("userData = " + msg.d.ReturnValue + ";");
            SECURITYSERVER_FillForm();
        },
        "error": function (msg) {
            PopupWarning(msg.responseText);
        }
    });
}

function SECURITYSERVER_FillForm() {
    $("#BreadCrumbLabel").html(userData.Email + " - " + userData.Profile.FullName);
    $("#Email").val(userData.Email);
    $("#FirstName").val(userData.Profile.Name);
    $("#LastName").val(userData.Profile.LastName);
    $("#LastName2").val(userData.Profile.LastName2);
}