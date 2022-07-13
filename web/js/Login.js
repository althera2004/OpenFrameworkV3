localStorage.removeItem("ApplicationUser");
localStorage.removeItem("ItemDefinitions");
localStorage.removeItem("Instance");
localStorage.removeItem("Company");
localStorage.removeItem("Menu");
localStorage.removeItem("ace_state_id-sidebar-toggle-icon");
localStorage.removeItem("ace_state_id-sidebar");
localStorage.removeItem("LAC");

var landpage = "";
var instanceName = "";
var ip = "";
$(document).ready(function () {
    getIp();

    var ddData = [
        {
            "text": "Català",
            "value": "ca",
            "selected": true,
            "description": "Català",
            "imageSrc": "/assets/img/flags/cat.png"
        },
        {
            "text": "Castellano",
            "value": "es",
            "selected": false,
            "description": "Castellano",
            "imageSrc": "/assets/img/flags/esp.png"
        }
    ];

    $("#CmbPais").ddslick({
        "data": ddData,
        "width": 400,
        "imagePosition": "left",
        "selectText": "Seleccioni idioma",
        "onSelected": function (data) {
            IdiomaChanged(data.selectedData.value);
        }
    });

    $("#ErrorMessage").hide();
    $("#BtnLogin").on("click", Login);
    $("#BtnPublicAccess").on("click", PublicAccess);
    $("#BtnRemember").on("click", Remember);

    // weke: falta recordar contraseña
    $("#BtnRemember").remove();
    $(".TxtUserName").focus();
    $(".TxtPassword").vkb({ "IsPassword": true });
	//$(".TxtUserName").val("");
	//$(".TxtPassword").val("");
    $(".TxtName").on("keyup", TxtNameChanged);
    $("img").on("dragstart", function (event) { event.preventDefault(); });
});

$(document).keypress(function (e) {
    if (e.which === 13) {
        Login();
    }
});

function Login() {
    $("#BtnRemember").html(Dictionary[language].Remember);
    $("#ErrorSpan").hide();
    $("#Remember").hide();
    $("#BtnLogin").attr("disabled", "disabled");
    $("#BtnLogin").hide();
    $("#BtnLoginQR").hide();
    $("#BtnPublicAccess").attr("disabled", "disabled");
    $("#BtnPublicAccess").hide();
    $("#LoadingMessage").show();
    var ok = true;
    var errorMessage = "";
    if ($(".TxtUserName").val() === "") {
        ok = false;
        $(".TxtUserName").css("background-color", "#fcc");
        errorMessage = Dictionary[language].UserRequired;
    }
    else {
        $(".TxtUserName").css("background-color", "transparent");
    }

    if ($(".TxtPassword").val() === "") {
        ok = false;
        $(".TxtPassword").css("background-color", "#fcc");
        if (errorMessage !== "") {
            errorMessage += "<br />";
        }

        errorMessage += "La contraseña es obligatoria.";
    }
    else {
        $(".TxtPassword").css("background-color", "transparent");
    }

    if (ok) {
        $("#BtnLogin").html(Dictionary[language].Cnn);
        var credential = $(".TxtUserName").val() + "||||" + $(".TxtPassword").val() + "||||" + instanceName + '||||' + ip;
        var data = {
            "credential": btoa(unescape(encodeURIComponent(credential)))
        };

        $.ajax({
            "type": "POST",
            "url": "/Async/SecurityService.asmx/LogOn",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (msg) {
                var result = msg.d;

                console.log(result);

                //return false;

                if (result.Success === true) {

                    var logOnResult = null;
                    eval("logOnResult = " + result.ReturnValue + ";");
                    console.log(logOnResult);

                    if (logOnResult.Id === -1) {
                        $("#ErrorSpan").show();
                        $("#Remember").show();
                        $("#BtnLogin").removeAttr("disabled");
                        $("#BtnLogin").html(Dictionary[language].Btn);
                        $("#BtnPublicAccess").removeAttr("disabled");
                        $("#BtnPublicAccess").html(Dictionary[language].BtnPublicAccess);
                        $("#BtnLogin").show();
                        $("#BtnLoginQR").show();
                        $("#LoadingMessage").hide();
                        return false;
                    }

                    if (logOnResult.Locked === true) {
                        $("#ErrorSpan").show();
                        $("#ErrorSpan").html("Acceso bloqueado");
                        $("#Remember").show();
                        $("#BtnLogin").removeAttr("disabled");
                        $("#BtnLogin").html(Dictionary[language].Btn);
                        $("#BtnPublicAccess").removeAttr("disabled");
                        $("#BtnPublicAccess").html(Dictionary[language].BtnPublicAccess);
                        $("#BtnLogin").show();
                        $("#BtnLoginQR").show();
                        $("#LoadingMessage").hide();
                        return false;
                    }

                    if (logOnResult.MultipleCompany === true) {
                        document.location = "/Select.aspx?" + $.base64.encode((result.ReturnValue.Id * 1975).toString() + "-weke");
                        return false;
                    }

                    if (logOnResult.Corporative === false) {
                        var query = "&I=" + instanceName;
                        query += "&C=" + logOnResult.CompanyId;
                        query += "&U=" + logOnResult.Id;
                        query += "&L=" + language;
                        document.location = "/InitSession.aspx?" + $.base64.encode(guid() + query);
                    }
                    else {
                        var lp = ""
                        if (landPage !== "") {
                            lp = landPage;
                        }
                        else {
                            lp = "/Instances/" + instanceName + "/Pages/DashBoard.aspx";
                        }
                        $("#UserId").val(result.ReturnValue.Id);
                        $("#CompanyId").val(result.ReturnValue.CompanyId);
                        $("#LandPage").val(lp);
                        $("#Password").val($(".TxtUserName").val());
                        CoporativeLayoutShow(result);
                    }
                }
                else {
                    $("#ErrorSpan").show();
                    $("#ErrorSpan").html(result.MessageError);
                    $("#Remember").show();
                    $("#BtnLogin").removeAttr("disabled");
                    $("#BtnLogin").html(Dictionary[language].Btn);
                    $("#BtnPublicAccess").removeAttr("disabled");
                    $("#BtnPublicAccess").html(Dictionary[language].BtnPublicAccess);
                    $("#BtnLogin").show();
                    $("#BtnLoginQR").show();
                    $("#LoadingMessage").hide();
                    return false;
                }

                if (result.ReturnValue.MustResetPassword === true) {
                    $("#UserId").val(result.Id);
                    $("#CompanyId").val(result.CompanyId);
                    $("#Password").val($(".TxtPassword").val());
                    document.getElementById("LoginForm").action = "ResetPassword.aspx";
                    $("#LoginForm").submit();
                    return false;
                }

                return false;
            },
            "error": function (msg) {
                $("#BtnLogin").removeAttr("disabled");
                $("#BtnLogin").html(Dictionary[language].Btn);
                $("#BtnLogin").show();
                $("#BtnLoginQR").show();
                $("#BtnPublicAccess").html(Dictionary[language].BtnPublicAccess);
                $("#BtnPublicAccess").show();
                $("#LoadingMessage").hide();
                $("#ErrorSpan").show(msg.statusText);
                $("#ErrorSpan").show();
                $("#Remember").show();
            }
        });
    }
    else {
        $("#ErrorMessage").html(errorMessage);
        $("#ErrorMessage").show();
        $("#BtnLogin").removeAttr("disabled");
        $("#BtnLogin").show();
        $("#BtnLoginQR").show();
        $("#BtnPublicAccess").removeAttr("disabled");
        $("#BtnPublicAccess").show();
        $("#LoadingMessage").hide();
    }
}

function LoginResultToRext(value) {
    for (var x = 0; x < LoginResult.length; x++) {
        if (LoginResult[x].value === value) {
            return LoginResult[x].text;
        }
    }

    return "undefined";
}

window.onload = function () {
    $(".TxtUserName").focus();
};

function PassKeyBoardToggle() {
    $(function () {
        $(".vkb-js-key").click();
    });
}

function Remember() {
    document.location = "Remember.aspx?" + btoa(unescape(encodeURIComponent(instanceName)));
}

function PublicAccess() {
    alert("PublicAccess");
}

function CoporativeLayoutShow() {
    $(".TxtUserName").attr("disabled", "disabled");
    $(".TxtPassword").attr("disabled", "disabled");
    $("#LoadingMessage").hide();
    $("#Corporative").show();
    $("#BtnCorporativeOk").attr("disabled", "disabled");
    $(".TxtName").focus();
}

function CoporativeLayoutHide() {
    $(".TxtUserName").removeAttr("disabled");
    $(".TxtPassword").removeAttr("disabled");
    $(".TxtName").val("");
    $("#BtnLogin").removeAttr("disabled");
    $("#BtnLogin").html(Dictionary[language].Btn);
    $("#BtnLogin").show();
    $("#Corporative").hide();
    $("#BtnCorporativeOk").attr("disabled", "disabled");
}

function TxtNameChanged() {
    if ($(".TxtName").val().split(' ').join('').length < 3) {
        $("#BtnCorporativeOk").attr("disabled", "disabled");
    }
    else {
        $("#BtnCorporativeOk").removeAttr("disabled");
    }
}

function CorporativeOk() {
    var name = $.base64.encode($(".TxtName").val());
    if (MFA !== "") {
        document.getElementById("LoginForm").action = "MFA.aspx";
        $("#LoginForm").submit();
        return false;
    }

    if (landPage !== "") {
        document.location = landPage + "?" + name;
    }
    else {
        document.location = "/Instances/" + instanceName + "/Pages/DashBoard.aspx?" + name;
    }
}

function IdiomaChanged(idiomaId) {
    var idioma = Dictionary[idiomaId];
    language = idiomaId;
    $("#PageTitle").html(idioma.Title);
    $("#BtnLogin").html(idioma.Btn);
}

function getIp() {
    $.get('https://www.cloudflare.com/cdn-cgi/trace', function (data) {
        // Convert key-value pairs to JSON
        // https://stackoverflow.com/a/39284735/452587
        data = data.trim().split('\n').reduce(function (obj, pair) {
            pair = pair.split('=');
            return obj[pair[0]] = pair[1], obj;
        }, {});
        console.log(data);

        ip = data.ip;
        $("#IpSpan").html(DOMPurify.sanitize(ip));
    });
}

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }

    return s4() + s4() + "-" + s4() + "-" + s4() + "-" + s4() + "-" + s4() + s4() + s4();
}