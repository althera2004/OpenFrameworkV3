window.onload = function () {
    $("#SpanUserName").html(ApplicationUser.Profile.FullName);
    $("#firstName").html(ApplicationUser.Profile.Name);
    $("#lastName").html(ApplicationUser.Profile.LastName);
    $("#lastName2").html(ApplicationUser.Profile.LastName2);
    $("#BreadCrumbLabel").html(" " + ApplicationUser.Profile.FullName);

    $("#avatar").attr("src", "/Instances/" + Instance.Name + "/Data/Core_Users/" + ApplicationUser.Id + "/Avatar.jpg");
    $("#avatar").attr("alt", ApplicationUser.Profile.FullName);
    $("#signature").attr("src", "/Instances/" + Instance.Name + "/Data/Core_Users/" + ApplicationUser.Id + "/signature.png");
    PROFILE_FooterLayout();
    ResizeWorkArea();
}

function PROFILE_FooterLayout() {
    $("#FormBtnDelete").remove();
    $("#FooterStatus").html("");

    var buttons = "<button class=\"btn btn-info\" type=\"button\" id=\"FormBtnPassword\"><i class=\"fa fa-key bigger-110\"></i><span class=\"hidden-xs\" style=\"float: right; margin-left:4px;\">Canviar contrasenya</span></button>";
    buttons += "<div class=\"btn-group dropup\"> \
        <button type =\"button\" id=\"dropdown-menu-up-launcher-signature\" data-toggle=\"dropdown\" class=\"btn btn-info dropdown-toggle\" aria-expanded=\"false\">\
            <span id =\"dropdown-menu-up1-launcherLabel\">Signatura </span>&nbsp;<span class=\"ace-icon fa fa-caret-up icon-only\"></span></button>\
                <ul class=\"dropdown-menu dropdown-inverse dropdown-menu-up\" id=\"dropdown-menu-up1\" style=\"text-align: left; top: -226px;\">\
                    <li id =\"BtnActionSignature\"><a href=\"#\"><i class=\"fa fa fa-signature bigger-110\" style=\"width: 25px;\"></i><span id=\"BtnActionLock_Label\">Signar amb tauleta</span></a></li>\
                    <li id =\"BtnActionSignatureQR\"><a href=\"#\"><i class=\"fa fa fa-qrcode bigger-110\" style=\"width: 25px;\"></i><span id=\"BtnActionLock_Label\">Signar amb mòbil</span></a></li>\
                    <li id =\"BtnActionUploadSignature\"><a href=\"#\"><i class=\"fa fa fa-upload bigger-110\" style=\"width: 25px;\"></i><span id=\"BtnActionLock_Label\">Pujar signatura</span></a></li>\
        </li></ul></span></div>";
    $("#FormBtnSave").before(buttons);
};

function ResizeWorkArea() {
    $(".panel-body").height($(window).height() - 220);
}