window.onload = function () {
    $("#RBComplexity" + Company.SecurityConfig.PasswordComplexity).prop("checked", true);
    $("#FailedAttempts").val(Company.SecurityConfig.FailedAttempts);
    $("#RBCaducidad1").on("click", COMPANYSECURITY_CaducityLayout);
    $("#RBCaducidad2").on("click", COMPANYSECURITY_CaducityLayout);

    if (Company.PasswordCaducity === true) {
        $("#RBCaducidad2").prop("checked", true);
        $("#PasswordRepeat").prop("checked", Company.SecurityConfig.PasswordRepeat);
        $("#PasswordCaducityDays").val(Company.SecurityConfig.PasswordCaducityDays);
    }
    else {
        $("#RBCaducidad1").prop("checked", true);
        $("#PasswordRepeat").prop("checked", false);
        $("#PasswordCaducityDays").val("");
    }

    COMPANYSECURITY_CaducityLayout();
}

function COMPANYSECURITY_CaducityLayout() {
    if ($("#RBCaducidad2").prop("checked") === true) {
        $(".PaswordCaducity").visible();
    }
    else {
        $(".PaswordCaducity").invisible();
    }
}