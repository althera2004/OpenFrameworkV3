function ActiveLayout() {
    $(".showhide").on("click", function () {
        var link = $(this).find("i:first");
        if (link.hasClass("fa-chevron-up")) {
            link.replaceClass("fa-chevron-up", "fa-chevron-down");
            $(this).parent().parent().next().slideUp(200);
            $(this).parent().parent().next().next().slideUp(200);
        } else {
            link.replaceClass("fa-chevron-down", "fa-chevron-up");
            $(this).parent().parent().next().slideDown(200);
            $(this).parent().parent().next().next().slideDown(200);
        }
    });
}

//tdActions += "    <a id=\"BtnCustomAction-" + button.Name + "-" + data.Id + "\" class=\"customAction actionBtn-" + button.Name + " ace-icon " + icon + " bigger-120\" id =\"" + button.Function + "_" + data.Id + "\" onclick=\"" + button.Function + "(this);\"" + color + "></a>";

function ListButtonRow(dataId, icon, color, action, title, name) {
    var customAction = false;
    if (typeof name !== "undefined") {
        customAction = true;
    }

    var res = "";
    res += "<a class=\"" + color + " ace-icon " + icon + " bigger-120";
    if (customAction === true) {
        res += " customAction actionBtn-" + name;
    }
    res += "\" id=\"" + (customAction ? "BtnCustomAction-" + name + "-" : "" ) + dataId + "\" onclick=\"" + action + "\"";
    if (typeof title !== "undefined" && title !== null && title !== "") {
        res += " title=\"" + title + "\"";
    }

    res += "></a>";
    return res;
}

function MenuSelectOption(optionId) {
    $("#" + optionId).addClass("active")
    $("#" + optionId).parent().parent().addClass("active");
    $("#" + optionId).parent().parent().addClass("open");
}

function RenderMenu() {
    var menu = LocalStorageGetJson("Menu");
    var res = "";
    for (var x = 0; x < menu.length; x++) {
        var label = menu[x].Label;
        if (typeof Dictionary[menu[x].Label] !== "undefined") {
            label = Dictionary[menu[x].Label];
        }

        if (HasArrayValues(menu[x].Options)) {
            res += "<li>";
            res += "  <a class=\"dropdown-toggle\">";
            res += "    <i class=\"menu-icon " + menu[x].Icon + "\"></i>";
            res += "      <span class=\"menu-text\">" +label + "</span>";
            res += "				<b class=\"arrow fa fa-angle-down\"></b>";
            res += "  </a>";
            res += "  <b class=\"arrow\"></b>";
            res += "  <ul class=\"submenu\">";
            for (var o = 0; o < menu[x].Options.length; o++) {
                res += RenderSubmenuOption(menu[x].Options[o]);
            }
            res += "  </ul>";
            res += "</li>"
        }
        else {
            res += RenderMenuOption(menu[x]);
        }
    }

    $(".nav-list").html(res);
}

function RenderSubmenuOption(option) {
    var label = option.Label;
    if (typeof Dictionary[option.Label] !== "undefined") {
        label = Dictionary[option.Label];
    }

    var linkAction = "";
    var id = option.Id;
    if (option.ListId !== "") {
        linkAction = " onclick=\"GoEncryptedList('" + option.ItemName + "', '" + option.ListId + "', []);\"";
        id = option.ItemName + "_" + option.ListId;
    }


    res = "<li id=\"" + id + "\">";
    res += "  <a " + linkAction + ">";
    res += "    <i class=\"menu-icon fa fa-caret-right\"></i> " + label + "</a>";
    res += "    <b class=\"arrow\"></b>";
    res += "</li>";
    return res;
}

function RenderMenuOption(option) {
    var label = option.Label;
    if (typeof Dictionary[option.Label] !== "undefined") {
        label = Dictionary[option.Label];
    }

    var linkAction = "";
    var id = option.Id;
    if (option.ListId !== "")
    {
        linkAction = " onclick=\"GoEncryptedList('" + option.ItemName + "', '" + option.ListId + "', []);\"";
        id = option.ItemName + "_" + option.ListId;
    }

    res = "<li id=\"" + id + "\">";;
    res += "  <a " + linkAction + ">";
    res += "    <i class=\"menu-icon " + option.Icon + "\"></i>";
    res += "    <span class=\"menu-text\"> " + label + " </span>";
    res += "  </a >";
    res += "  <b class=\"arrow\"></b>";
    res += "</li>";
    return res;
}

function RenderBreadCrumb() {
    if (PageType === "PageView") {

        if (typeof ListId !== "undefined") {
            list = ItemListById(ItemDefinition, ListId);
            var listLabel = ItemDefinition.Layout.LabelPlural;

            if (HasPropertyValue(list.Title)) {
                listLabel = list.Title;
            }

            $(".breadcrumb").append("<li><a style=\"cursor:pointer;\" onclick=\"GoEncryptedList('" + ItemDefinition.ItemName + "','" + ListId + "',[],);\">" + listLabel + "</a></li>");
        }

        $(".breadcrumb").append("<li class=\"active\"><span id=\"BreadCrumbLabel\">" + ItemData.GetDescription() + "</span></li>");
    }
    else if (PageType === "PageList") {
        list = ItemListById(ItemDefinition, ListId);
        var listLabel = ItemDefinition.Layout.LabelPlural;

        if (HasPropertyValue(list.Title)) {
            listLabel = list.Title;
        }
        $(".breadcrumb").append("<li class=\"active\"><span id=\"BreadCrumbLabel\">" + ItemData.GetDescription() + "</span></li>");
    }
}

function LayoutTabSelected(e) {
    console.log(e.currentTarget.id.substr(10));
    $(".tabbed").hide();
    $("#dropdown-menu-up-launcher-" + e.currentTarget.id.substr(10)).show();
}

function PageLoadingShow() {
    $("#PageLoadingMask").show();
    $("form").addClass("blur-filter");
}

function PageLoadingHIde() {
    $("#PageLoadingMask").hide();
    $("form").removeClass("blur-filter");
}

function ToBooleanText(value) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }
    if (value === true || value === "true") { return Dictionary.Common_Yes; }
    if (value === false || value === "false") { return Dictionary.Common_No; }
    return "";
}

function ToBooleanIcon(value) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }
    if (value === true || value === "true") { return "<i class=\"fal fa-lg fa-fw fa-check\"></i>"; }
    if (value === false || value === "false") { return "<i class=\"fal fa-lg fa-fw fa-times\"></i>"; }
    return "";
}

function ToBooleanCheck(value) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }
    if (value === true || value === "true") { return "<i class=\"fal fa-lg fa-fw fa-check-square\"></i>"; }
    if (value === false || value === "false") { return "<i class=\"fal fa-lg fa-fw fa-square grey\"></i>"; }
    return "";
}

function ToBooleanCheckNull(value) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }
    if (value === true || value === "true") { return "<i class=\"fal fa-lg fa-fw fa-check\"></i>"; }
    return "";
}

function Notify(message, type) {

    if (typeof type === "undefined") {
        type = "info";
    }

    $.notify(message, type);
}

function NotifySaveSuccess(message) {
    var actionType = message.split('|')[0];
    var text = "OK";
    if (actionType === "UPDATE") {
        text = "Actualizado " + ItemDefinition.Layout.Label;
    } else {
        text = "Añadido " + ItemDefinition.Layout.Label;
    }

    $("#FormBtnSave").notify(text, { "position": "top", "className": "success" });
}

function NotifySaveError(message) {
    $("#FormBtnSave").notify(message, { "position": "top", "className": "error" });
}

function GroupButtonUrlClicked(sender) {
    var fieldName = sender.currentTarget.id.split('_')[0];
    var url = $("#" + fieldName).val();
    var res = validateUrl(url);
    if (res === true) {
        window.open(url, fieldName);
    }
    else {
        PopupWarning("<strong>" + url + "</strong> no és una url vàlida.", Dictionary.Common_Warning);
    }
}