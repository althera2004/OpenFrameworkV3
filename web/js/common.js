function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }

    return s4() + s4() + "-" + s4() + "-" + s4() + "-" + s4() + "-" + s4() + s4() + s4();
}

function none() {
    console.log(FKLoaded);
    return false;
}

// JQuery extensions
// -----------------------------------------------
jQuery.fn.visible = function () {
    return this.css("visibility", "visible");
};

jQuery.fn.invisible = function () {
    return this.css("visibility", "hidden");
};

jQuery.fn.localDatePicker = function () {
    var iso = "ca";
    if (HasPropertyValue(ApplicationUser.Language)) {
        if (HasPropertyValue(ApplicationUser.Language.JavaScriptISO)) {
            iso = ApplicationUser.Language.JavaScriptISO;
        }
    }

    return this.datepicker({ "autoclose": true, "todayHighlight": true, "language": iso });
}

jQuery.fn.replaceClass = function (oldClass, newClass) {
    return this.removeClass(oldClass).addClass(newClass);
}

jQuery.fn.enable = function () {
    return this.removeAttr("disabled");
}

jQuery.fn.disable = function () {
    return this.attr("disabled", "disabled");
}

jQuery.fn.readOnly = function (status) {
    if (typeof status !== "undefined" && status !== null && status === false) {
        return this.attr("readonly", "readonly");
    }

    return this.removeAttr("readonly");
}
// -----------------------------------------------

function LocalStorageSetJson(key, data) {
    localStorage.setItem(key, JSON.stringify(data));
}
function LocalStorageSet(key, data) {
    localStorage.setItem(key, data);
}

function LocalStorageGetJson(key) {
    var res = null;
    try {
        res = JSON.parse(localStorage.getItem(key));
    }
    catch (ex) {
        console.log(ex);
    }

    return res;
}

function LocalStorageGet(key) {
    var res = null;
    try {
        res = localStorage.getItem(key);
    }
    catch (ex) {
        console.log(ex);
    }

    return res;
}

// ------------------ Detecting data
function HasPropertyEnabled(property) {
    if (typeof property !== "undefined" && property !== null && property === true) {
        return true;
    }

    return false;
}

function HasPropertyValue(property) {
    if (typeof property !== "undefined" && property !== null && property !== "") {
        return true;
    }

    return false;
}

function GetPropertyValue(property, defaultValue) {
    var res = defaultValue ;
    if (HasPropertyValue(property)) {
        res = property;
    }

    return res;
}

function HasPropertyValueNumeric(property) {
    if (typeof property !== "undefined" && property !== null && property > 0) {
        return true;
    }

    return false;
}

function HasArrayValues(array) {
    if (typeof array !== "undefined" && array !== null && array.length > 0) {
        return true;
    }

    return false;
}


// ------------------ Data transforming
function escapeHTML(html) {
    return html.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
}

function GetDate(date, separator, nullable) {
    if (typeof date === "undefined" || date === null || date === "") {
        if (typeof nullable === "undefined" || nullable === null) {
            return null;
        }
        else {
            if (nullable === false) {
                return null;
            }
            else {
                return new Date(1970, 1, 1);
            }
        }
    }

    if (typeof separator === "undefined") { separator = "-" };
    if (separator === "/") { separator = "-"; }
    if (separator === null) { separator = "-"; }
    date = date.split("/").join("-");
    var day = date.split(separator)[0] * 1;
    var month = (date.split(separator)[1] * 1) - 1;
    var year = date.split(separator)[2] * 1;
    if (year < 100) {
        year += 2000;
    }

    return new Date(year, month, day);
}

function GetDateText(date, separator, nullable) {
    if (typeof date === "undefined" || date === null || date === "") {
        if (typeof nullable === "undefined" || nullable === null) {
            return null;
        }
        else {
            if (nullable === false) {
                return null;
            }
            else {
                return new Date(1970, 1, 1);
            }
        }
    }

    if (typeof separator === "undefined") { separator = "/" };
    if (separator === "-") { separator = "/"; }
    if (separator === null) { separator = "/"; }

    var days = date.getDate();
    if (days < 10) { days = "0" + days };

    // Enero = 0
    var months = date.getMonth() + 1;

    if (months < 10) { months = "0" + months };

    return days + separator + months + separator + date.getFullYear();

}


// --------- FEATURES
function FeatureEnabled(featureName) {
    return false;
}


// --------- GRANTS ---------------
function GrantsByItem(itemName) {
    return {
        "Grants": "RWD"
    };
}

function GrantCanWriteByItem(itemName) {
    return true;
}

function GrantCanReadByItem(itemName) {
    return true;
}

function GrantCanDeleteByItem(itemName) {
    return true;
}
// --------------------------------


// ------- LAYOUT -----------------
function Layout_FixedListRB_Clicked(sender) {
    var parts = sender.id.split('_');
    $("#" + parts[1]).val(parts[2] * 1);
    var id = parts[1];
    ItemData.UpdateData(id, $("#" + id).val() * 1);
}

function Layout_FixedListCK_Clicked(fieldName, value) {
    console.log(fieldName, value);
    console.log($("#CK_" + fieldName + "_" + value).prop("checked"));
    var res = 0;
    $(".CK_" + fieldName + ":checked").each(function () {
        res += Math.pow(2, (this.id.split('_')[2] * 1));
    });

    $("#" + fieldName).val(res);
    var realValue = res === 0 ? null : res;
    ItemData.UpdateData(fieldName, realValue);
}


// --------------------------------

function NormalizeData(data) {
    var res = {};
    var keys = Object.keys(data);
    for (var d = 0; d < keys.length; d++) {
        if (typeof data[keys[d]] === "string") {
            res[keys[d]] = data[keys[d]].trim();
        }
        else if (Object.prototype.toString.call(data[keys[d]]) === "[object Date]") {
            res[keys[d]] = GetDateText(data[keys[d]], "/", false);
        }
        else {
            res[keys[d]] = data[keys[d]];
        }
    }

    return res;
}

function TodayText() {
    var today = new Date();
    var day = today.getDate();
    var month = today.getMonth() + 1;
    var year = today.getFullYear();

    var res = "";
    if (day < 10) { res += "0"; }
    res += day + "/";
    if (month < 10) { res += "0"; }
    res += month + "/" + year;
    return res;
}

function DateHumanText(date) {
    if (DateIsToday(date)) { return "Avui"; }
    if (DateIsYesterday(date)) { return "Ahir"; }
    return GetDateText(date, "/", false);
}

function DateIsToday(date) {
    var today = new Date();
    if (date.getDate() === today.getDate()) {
        if (date.getMonth() === today.getMonth()) {
            if (date.getFullYear() === today.getFullYear()) {
                return true;
            }
        }
    }

    return false;
}

function DateIsYesterday(date) {
    var yesterday = new Date();
    yesterday.setDate(yesterday.getDate() - 1);
    if (date.getDate() === yesterday.getDate()) {
        if (date.getMonth() === yesterday.getMonth()) {
            if (date.getFullYear() === yesterday.getFullYear()) {
                return true;
            }
        }
    }

    return false;
}

// ------------------- data validation
// -------------------------------------------------------
function validateUrl(value) {
    return /^(?:(?:(?:https?|ftp):)?\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})))(?::\d{2,5})?(?:[/?#]\S*)?$/i.test(value);
}

function validateEmail(value) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(value);
}
// -------------------------------------------------------

function ReplaceClick(objectId, action) {
    $("#" + objectId).removeAttr("onclick");
    $("#" + objectId).on("click", action);
}

// money format
// -------------------------
$("input.money-bank").on("keyup", numberDecimalUp);
$("input.money-bank").on("keydown", numberDecimalDown);
$("input.money-bank").on("focus", numberDecimalFocus);
$("input.money-bank").on("blur", moneyBlur);
// -------------------------