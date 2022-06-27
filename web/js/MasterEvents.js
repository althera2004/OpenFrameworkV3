(function () {
    var originalAddClassMethod = jQuery.fn.addClass;
    jQuery.fn.addClass = function () {
        var result = originalAddClassMethod.apply(this, arguments);
        jQuery(this).trigger("cssClassChanged");
        return result;
    };
})();

$(function () {
    $(".dropup").bind("cssClassChanged", positionInitDropdown);
   // $(".sidebar-toggle").bind("cssClassChanged", positioningFooter);
    $("#filterDiv").bind("cssClassChanged", resizeFn);
    //$("#widget-box-1").bind("cssClassChanged", fullScreened);
    $("#PopupMap").bind("cssClassChanged", MapRedraw);
    $("#sidebar").bind("cssClassChanged", SideBarChanged);
    $("#nav-search").bind("cssClassChanged", SearchChanged);
});

function SearchChanged() {
    console.log("SearchChanged");
}

// Soluciona problema de ver los textos del menú cuando está colapsado
function SideBarChanged() {
    if ($("#sidebar").hasClass("menu-min")) {
        $("#sidebar li a span").hide();
    }
    else {
        $("#sidebar li a span").show();
    }
}

function fullScreened() {
    resizeFn();
    TableRendered();
}

//function positioningFooter() {
//    if (containerWidth > 768) {
//        $("#logofooter").css("padding-left", ($("#sidebar").width() + 6) + "px");
//    }
//    TableRendered();
//}

function MapRedraw() {
    mymap.invalidateSize();
}

var resizeFn = function () {
    if (typeof Resize === "function") {
        Resize();
    }
};

function positionInitDropdown() {
    $("#dropdown-menu-up1").css("top", $("#dropdown-menu-up1").height() * -1 - 20);
    $("#dropdown-menu-up2").css("top", $("#dropdown-menu-up2").height() * -1 - 20);
}

window.addEventListener("devtoolschange", function (e) {
    console.log("is DevTools open?", e.detail.open);
    console.log("and DevTools orientation?", e.detail.orientation);
});