window.onload = function () {
    SECURITYUSER_SetLayout();

    $("#BreadCrumbLabel").html(userData.Email + " - " + userData.Profile.FullName);

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
    alert("SECURITYUSER_Save");
}

function SECURITYUSER_Delete() {
    alert("SECURITYUSER_Delete");
}