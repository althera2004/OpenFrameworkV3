function PageForm(config) {
    var _this = this;
    this.Data = [];
    this.FormId = config.FormId;
    this.ItemDefinition = ItemDefinitionById(config.ItemId);
    this.ItemName = this.ItemDefinition.ItemName;
    this.Definition = null;
    this.DefaultTabId = '';
    this.HasApplicationUserFields = false;

    this.Init = function () {
        this.Definition = ItemFormById(this.ItemDefinition, this.FormId);

        // Definir tabDefault;
        this.DefaultTabId = '';
        for (var t = 0; t < this.Definition.Tabs.length; t++) {
            if (HasPropertyEnabled(this.Definition.Tabs[t].Persistent) === false) {
                if (this.DefaultTabId === "") {
                    this.DefaultTabId = this.Definition.Tabs[t].Id;
                }
                if (HasPropertyEnabled(this.Definition.Tabs[t].Default)) {
                    this.DefaultTabId = this.Definition.Tabs[t].Id;
                    break;
                }
            }
        }

        if (this.Definition === null) {
            this.Definition = this.FormDefinitionDefault();
        }
    };

    this.Fill = function (data) {
        var itemDefinition = this.ItemDefinition;
        $.each($(".form-control"), function () {
            var fieldName = $(this)[0].id;
            var dataValue = null;
            var itemField = GetFieldDefinition(fieldName, itemDefinition);

            switch (itemField.Type.toLowerCase()) {               
                case "datetime":
                    if (typeof data[fieldName] === "undefined") {
                        dataValue = null;
                    }
                    else if (typeof data[fieldName] === "string") {
                        dataValue = data[fieldName];
                        if (dataValue.indexOf('T') != -1) {
                            dataValue = GetDateTextFromZulu(dataValue, "/", false);
                        }
                    }
                    else {
                        dataValue = GetDateText(data[fieldName], "/", false);
                    }
                    break;
                case "long":
                    if (IsFK(this.itemDefinition, itemField.Name)) {
                        var fkItemName = itemField.Name.substr(0, itemField.Name.length - 2);
                        console.log("formtools", fkItemName);
                        FillComboFromFK(itemField.Name, fkItemName, ItemData.OriginalItemData[itemField.Name]);
                        dataValue = data[fieldName] === null ? 0 : data[fieldName];
                    }
                    else {
                        dataValue = data[fieldName];
                    }
                    break;
                case "bool":
                    var checkValue = false;

                    if (data[fieldName] === true) {
                        checkValue = true;
                    }

                    $("#" + itemField.Name).prop("checked", checkValue);

                    break;
                case "fixedlist":
                    dataValue = data[fieldName];
                    var fixedListValue = data[fieldName];
                    $("#RB_" + fieldName + "_" + fixedListValue).prop("checked", true);
                    break;
                case "applicationuser":
                    dataValue = data[fieldName];
                    if (dataValue === null) {
                        dataValue = -1;
                    }
                    break;
                default:
                    dataValue = data[fieldName];
                    break;
            }

            if (data !== null) {
                $(this).val(dataValue);
            }
        });

        $.each($(".fixedList_check"), function () {
            var fieldName = $(this)[0].id.split('_')[0];
            var dataValue = data[fieldName];
            console.log(fieldName, (dataValue >>> 0).toString(2));

            var binary = (dataValue >>> 0).toString(2);
            var test = 0;
            for (var b = binary.length - 1; b >= 0; b--) {
                console.log(Math.pow(2, binary.length - 1 - b), binary[b]);
                if (binary[b] === '1') {
                    test += Math.pow(2, binary.length - 1 - b);
                    $("#CK_" + fieldName + "_" + (binary.length - 1 - b)).prop("checked", true);
                }
            }

            console.log(dataValue, test);

        });

        $.each($(".simple-slider"), function () {
            var fieldName = $(this)[0].id.split('_')[0];
            var dataValue = data[fieldName];
            console.log(fieldName, dataValue);
            document.getElementById(fieldName + "_Slider").noUiSlider.set([dataValue, null]);
            $("#" + fieldName + "_Slider").data("loaded", 1);
        });

        $.each($("input.format-money"), function () {
            console.log($("#" + this.id).val());
            $("#" + this.id).val(ToMoneyFormat($("#" + this.id).val()));
            console.log($("#" + this.id).val());
        });

        var afterFillAction = itemDefinition.ItemName.toUpperCase() + "_FormFillAfter";
        var type = eval("typeof " + afterFillAction);
        if (type === "function") {
            window[afterFillAction]();
        }

        $(".TabMustExists").removeClass("TabMustExists");
    }

    this.FormDefinitionDefault = function () {
        var formDefinition = {
            "Id": "Custom",
            "FormType": "Custom",
            "DefaultSelected": true,
            "Actions": [],
            "Tabs": [{
                "Id": "autogenerated",
                "Label": "Dades bàsiques",
                "Rows": []
            }]
        };

        // Si hay itemDefinition se crean los campos en el tab
        if (typeof this.ItemDefinition !== "undefined" && this.ItemDefinition !== null) {
            for (var f = 0; f < this.ItemDefinition.Fields.length; f++) {
                var field = this.ItemDefinition.Fields[f];
                if (field.Name !== "Id") {
                    if (HasPropertyEnabled(field.Internal)) { continue; }
                    var row = { "Fields": [{ "Name": field.Name }] };
                    formDefinition.Tabs[0].Rows.push(row);
                }
            }
        }

        return formDefinition;
    }

    this.RenderPopup = function () {
        var tab = this.Definition.Tabs[0];
        var res = "<div class=\"hpanel\">";
        res += "<div class=\"panel-body panel-body-form\">";

        var rows = tab.Rows;
        for (var r = 0; r < rows.length; r++) {
            res += this.RenderRow(rows[r]);
        }

        res += "</div>";
        res += "</div>";
        return res;
    }

    this.Render = function (targetId) {
        console.log("Life cycle", "RenderForm init");
        this.RenderPersistent();
        this.RenderTabs(targetId);
        this.RenderContent(targetId);
        this.RenderFooterActions();
        $(".form-control").on("keyup", ItemUpdateData);
        $(".form-control").on("change", ItemUpdateData);
        $(".datepicker").on("change", ItemUpdateData);
        $(".datepicker").localDatePicker();
        $(".form-radio").on("click", ItemUpdateDataFormRadio);
        $(".input-group-button-url").on("click", GroupButtonUrlClicked);
        console.log("Life cycle", "RenderForm end " + ListSources.length);
        InitSliders();

        if ($(".CmbAppplicationUsers").length > 0) {
            FillCmbApplicationUsers();
        }

        $("input.format-money").on("keyup", numberDecimalUp);
        $("input.format-money").on("keydown", numberDecimalDown);
        $("input.format-money").on("focus", numberDecimalFocus);
        $("input.format-money").on("blur", moneyBlur);
    }

    this.RenderFooterActions = function () {
        res = "";
        if (HasPropertyValue(this.Definition.Actions)) {

            var tabs = _.keys(_.countBy(ItemDefinition.Forms[0].Actions, function (data) { return data.Tab; }));

            for (var t = 0; t < tabs.length; t++) {

                var tabId = tabs[t];
                var ButtonLabel = "Accions ";
                var buttonStyle = "style=\"display:none;\"";
                var ButtonClass=" tabbed";
                if (tabId === "undefined") {
                    var actionsNoGroup = this.Definition.Actions.filter(function (item) { return typeof item.Group === "undefined" && typeof item.Tab === "undefined"; });
                    var actionsGroup = this.Definition.Actions.filter(function (item) { return typeof item.Group !== "undefined" && typeof item.Tab === "undefined"; });
                    buttonStyle = "";
                    ButtonClass = "";
                } else {
                    var actionsNoGroup = this.Definition.Actions.filter(function (item) { return typeof item.Group === "undefined" && item.Tab === tabId; });
                    var actionsGroup = this.Definition.Actions.filter(function (item) { return typeof item.Group !== "undefined" && item.Tab === tabId; });
                    ButtonLabel += this.ItemDefinition.Forms[0].Tabs.filter(function (item) { return item.Id === tabId; })[0].Label.toLowerCase();

                    if (this.DefaultTabId === tabId) {
                        buttonStyle = "";
                    }
                }


                actionsGroup.sort(function (a, b) {
                    return (a.Group > b.Group) ? 1 : -1;
                });
                res += "<div class=\"btn-group dropup\">";
                res += " <button type=\"button\" id=\"dropdown-menu-up-launcher-" + tabId + "\" data-toggle=\"dropdown\" class=\"btn btn-info dropdown-toggle" + ButtonClass + "\" aria-expanded=\"false\" " + buttonStyle + "> <span id=\"dropdown-menu-up1-launcherLabel\">" + ButtonLabel + "</span>&nbsp;<span class=\"ace-icon fa fa-caret-up icon-only\"></span></button>";
                res += "   <ul class=\"dropdown-menu dropdown-inverse dropdown-menu-up\" id=\"dropdown-menu-up1\" style=\"text-align: left;\">";

                var actualGroup = "";
                for (var x = 0; x < actionsGroup.length; x++) {
                    var action = actionsGroup[x];
                    var title = action.Group;
                    if (typeof title === "undefined") { title = ""; }

                    if (title !== actualGroup) {
                        actualGroup = title;
                        res += "<li class=\"dropup-title\">" + title + "</li>";
                    }

                    res += "    <li id=\"BtnAction" + action.Action + "\" onclick=\"" + action.Action + "();\"><a href=\"#\"><i class=\"fa " + action.Icon + " bigger-110\" style=\"width: 25px;\"></i><span id=\"BtnAction" + action.Action + "_Label\">" + action.Label + "</span></a></li>";
                }

                if (actionsNoGroup.length > 0 && actionsGroup.length > 0) {
                    res += "<li><hr style=\"margin-top:8px;margin-bottom:4px;\"></li>";
                }

                if (actionsNoGroup.length > 0) {
                    for (var x = 0; x < actionsNoGroup.length; x++) {
                        var action = actionsNoGroup[x];
                        res += "    <li id=\"BtnAction" + action.Action + "\" onclick=\"" + action.Action + "();\"><a href=\"#\"><i class=\"fa " + action.Icon + " bigger-110\" style=\"width: 25px;\"></i><span id=\"BtnAction" + action.Action + "_Label\">" + action.Label + "</span></a></li>";
                    }
                }

                res += "  </ul>";
                res += "</div>";
            }
        }

        $("#FormBtnDelete").before(res);
    }

    this.RenderPersistent = function () {
        var persistentTab = null;
        for (var t = 0; t < this.Definition.Tabs.length; t++) {
            if (this.Definition.Tabs[t].Persistent === true) {
                persistentTab = this.Definition.Tabs[t];
                break;
            }
        }

        if (persistentTab !== null) {
            var rows = persistentTab.Rows;
            var res = "<div class=\"form-group col-xs-12\">";
            for (var r = 0; r < rows.length; r++) {
                res += this.RenderRow(rows[r]);
            }

            res += "</div>";
            $("#PersistentFields").html(res);
        }
        else {
            console.log("No persistent Tab");
        }
    }

    this.RenderTabs = function (targetId) {
        var res = "";
        var defaultTab = 0;
        var realTab = 0;
        for (var t = 0; t < this.Definition.Tabs.length; t++) {
            if (this.Definition.Tabs[t].Persistent !== true) {
                var tab = this.Definition.Tabs[t];
                var active = realTab === defaultTab ? "active" : " ";
                var mustExists = "";
                if (HasPropertyEnabled(tab.MustExists)) {
                    mustExists = " TabMustExists";
                }

                res += "<li id=\"tabSelect-" + tab.Id + "\" class=\"tabSelect " + active + mustExists + "\"><a data-toggle=\"tab\" href=\"#tab-" + tab.Id + "\" aria-expanded=\"false\">" + tab.Label + "</a></li>";
                realTab++;
            }
        }

        if (ItemDefinition_HasFeature(this.ItemDefinition, "Attachs")) {
            res += "<li id=\"tabSelect-Attachs\" class=\"tabSelect\"><a data-toggle=\"tab\" href=\"#tab-Attachs\" aria-expanded=\"false\"><i class=\"fa fa-paperclip\"></i>&nbsp;" + Dictionary.Feature_Attachment_TabTitle + "</a></li>";
        }

        $("#" + targetId + "Tabs").html(res);
        $(".tabSelect").on("click", LayoutTabSelected)
    }

    this.RenderContent = function (targetId) {
        var res = "";

        var defaultTab = 0;
        var realTab = 0;
        for (var t = 0; t < this.Definition.Tabs.length; t++) {
            if (this.Definition.Tabs[t].Persistent !== true) {
                var tab = this.Definition.Tabs[t];

                var active = realTab === defaultTab ? "active" : "";

                res += "<div id=\"tab-" + tab.Id + "\" class=\"tab-pane " + active + "\">";
                res += "<div class=\"hpanel\">";
                res += "<div class=\"panel-body panel-body-form\">";

                var rows = tab.Rows;
                for (var r = 0; r < rows.length; r++) {
                    res += this.RenderRow(rows[r]);
                }

                res += "</div>";
                res += "</div>";
                res += "</div>";

                realTab++;
            }
        }

        if (ItemDefinition_HasFeature(this.ItemDefinition, "Attachs")) {
            res += "<div id=\"tab-Attachs\" class=\"tab-pane\">";
            res += "<div class=\"hpanel\">";
            res += "<div class=\"panel-body panel-body-form\">";
            res += "</div>";
            res += "</div>";
            res += "</div>";
        }

        $("#" + targetId + "Content").html(res);
    }

    this.CalculateRowSpan = function (row) {
        if (HasPropertyValue(row.Fields)) {
            var totalSpan = 0;
            for (var f = 0; f < row.Fields.length; f++) {
                var field = row.Fields[f];
                if (HasPropertyValue(field.ColSpan)) {
                    totalSpan += field.ColSpan - 1;
                }
            }

            return 12 / (row.Fields.length + totalSpan);
        }

        return 1;
    };

    this.RenderRow = function (row) {
        var res = "<div class=\"row\">";

        if (HasPropertyValue(row.Fields)) {
            var fields = row.Fields;

            var span = this.CalculateRowSpan(row);


            for (var f = 0; f < fields.length; f++) {
                var field = fields[f];
                if (field.Type === "Blank") {
                    res += RenderFieldBlank(span);
                    continue;
                }
                if (field.Type === "Free") {
                    res += RenderFieldFree(span, field);
                    continue;
                }

                if (field.Type === "Button") {
                    res += RenderFieldButton(span, field);
                    continue;
                }

                res += this.RenderField(field, span);
            }
        }

        if (HasPropertyValue(row.ListId)) {
            var itemName = row.ItemList;
            var itemDefinition = ItemDefinitionByName(itemName);
            var listDefinition = ItemListById(itemDefinition, row.ListId)
            ListSources.push(new PageList({ "ListDefinition": listDefinition, "ItemName": row.ItemList, "ItemDefinition": itemDefinition }));

            var styleHeight = "";
            if (HasPropertyValue(listDefinition.ForcedHeight) === true) {
                styleHeight = " style=\"height:" + listDefinition.ForcedHeight + "px\"";
            }

            var componentPrefix = itemName + "_" + listDefinition.Id;
            var Title = GetPropertyValue(listDefinition.Title, this.ItemDefinition.Layout.LabelPlural);
            res += "<div id=\"" + componentPrefix + "_List\" class=\"ListContainer\">";
            /*res += "  <div class=\"hpanel hblue hpanel-table\" style=\"margin:0;\" id=\"" + componentPrefix + "_PanelBody\">";
            res += "    <div class=\"panel-heading hbuilt\">";
            res += "      <span id=\"" + componentPrefix + "_ListTitle\">" + Title + "***</span>";
            res += "        <div class=\"panel-tools\">";
            res += "          <a id=\"Instancia_Custom_AddBtn\"><i class=\"fa fa-plus\"></i>&nbsp;<span id=\"Instancia_Custom_AddBtnLabel\">Afegir</span></a>";
            res += "        </div>";
            res += "      </div >";
            res += "      <div class=\"tableHead\">";
            res += "        <table class=\"table\">";
            res += "          <thead id=\"" + componentPrefix + "_ListHead\"></thead>";
            res += "        </table>";
            res += "      </div>";
            res += "      <div class=\"panel-body2 panel-body-list-inForm\" id=\"" + componentPrefix + "_PanelBodyList\"" + styleHeight + ">";
            res += "        <div class=\"table-responsive\" style=\"max-height: 100%; height: 100%; overflow-y: scroll; overflow-x: hidden\">";
            res += "          <div class=\"table-body\" style=\"max-height: 100%; height: 100%\">";
            res += "            <table class=\"table\" style=\"max-height: 100%\" cellpadding=\"1\" cellspacing=\"1\">";
            res += "              <tbody id=\"" + componentPrefix + "_ListBody\">";
            res += "              </tbody>";
            res += "            </table>";
            res += "          </div>";
            res += "        </div>";
            res += "      </div>";
            res += "      <div class=\"panel-footer\">";
            res += "        Nº de registros: <strong id=\"" + componentPrefix + "_ListCount\"></strong>";
            res += "      </div>";
            res += "  </div>";*/
            res += "</div>";
        }

        res += "</div>";
        return res;
    }

    this.RenderField = function (fieldForm, span) {
        var res = "";

        if (HasPropertyValue(fieldForm.Type) === true) {
            if (fieldForm.Type.toLowerCase() === "placeholder") {
                res = "<div id=\"Placeholder_" + fieldForm.Name + "\">";
                res += "</div>";
            }
        }
        else {
            var field = FieldByName(this.ItemDefinition, fieldForm.Name)

            if (field !== null) {

                if (HasLayout(fieldForm, "nolabel") === false) {
                    res += this.RenderFieldLabel(field, fieldForm);
                }

                //if (HasPropertyValue(fieldForm.Layout)) {
                //    if (fieldForm.Layout.toLowerCase().indexOf("nolabel") === -1) {
                //        res += this.RenderFieldLabel(field, fieldForm);
                //    }
                //} else {
                //    res += this.RenderFieldLabel(field, fieldForm);
                //}

                var finalSpan = span;
                if (HasPropertyValue(fieldForm.ColSpan)) {
                    finalSpan = span * fieldForm.ColSpan;
                }

                switch (field.Type.toLowerCase()) {
                    case "datetime":
                        res += RenderFieldDate(field, finalSpan);
                        break;
                    case "long":
                    case "int":
                        if (IsFK(this.ItemDefinition, field.Name)) {
                            res += RenderFieldFK(field, finalSpan, fieldForm);
                        }
                        else {
                            res += RenderFieldNumeric(field, finalSpan);
                        }
                        break;
                    case "decimal":
                        res += RenderFieldDecimal(field, finalSpan, fieldForm);
                        break;
                    case "money":
                        res += RenderFieldMoney(field, finalSpan, fieldForm);
                        break;
                    case "fixedlist":
                        res += RenderFieldFixedList(field, finalSpan, fieldForm);
                        break;
                    case "fixedlistmultiple":
                        res += RenderFieldFixedListMultiple(field, finalSpan, fieldForm);
                        break;
                    case "textarea":
                        res += RenderFieldTextArea(field, finalSpan, fieldForm);
                        break;
                    case "boolean":
                        //if (HasPropertyValue(fieldForm.Layout)) {
                        //    switch (fieldForm.Layout.toLowerCase()) {
                        //        case "check": res += RenderFieldCheckBox(field, finalSpan, fieldForm); break;
                        //        case "radio": res += RenderFieldRadioButton(field, finalSpan, fieldForm); break;
                        //    }
                        //}
                        if (HasLayout(fieldForm, "check")) {
                            res += RenderFieldCheckBox(field, finalSpan, fieldForm);
                        }
                        else if (HasLayout(fieldForm, "radio")) {
                            res += RenderFieldRadioButton(field, finalSpan, fieldForm);
                        }
                        else {
                            res += RenderFieldCheckBox(field, finalSpan, fieldForm);
                        }

                        break;
                    case "email":
                        res += RenderFieldEmail(field, finalSpan, fieldForm);
                        break;
                    case "url":
                        res += RenderFieldUrl(field, finalSpan, fieldForm);
                        break;
                    case "documentfile":
                        res += RenderFieldDocumentFile(field, finalSpan, fieldForm);
                        break;
                    case "applicationuser":
                        this.HasApplicationUserFields = true;
                        res += RenderFieldApplicationUser(field, finalSpan, fieldForm);
                        break;
                    case "range":
                        res += RenderFieldRange(field, finalSpan);
                        break;
                    case "text":
                        res += RenderFieldText(field, finalSpan);
                        break;
                    default:
                        res += field.Name;
                        break;
                }

            }
        }
        if (res  === "") {
            res += "<div class=\"col-sm-12\">" + fieldForm.Name + " no existe</div>";
        }

        return res;
    }

    this.RenderFieldLabel = function (field, fieldForm) {

        if (HasLayout(fieldForm, "Splited")) { return ""; }

        //if (HasPropertyValue(fieldForm.Layout)) {
        //    if (fieldForm.Layout === "Splited") {
        //        return "";
        //    }
        //}

        res = "";
        var hidden = GetPropertyValue(fieldForm.Hidden, false) ? " style=\"visibility:hidden;\"" : "";
        var label = fieldForm.Label;
        var required = GetPropertyValue(field.Required, false) ? "<span id=\"" + field.Name + "_LabelRequired\" class=\"formFieldRequired\">*</span>" : "";

        if (typeof label === "undefined" || label === null) {
            label = field.Label;
        }

        res += "<label id=\"" + field.Name + "_Label\" class=\"col-sm-1 control-label\"" + hidden + ">" + label + required + "</label>";
        return res;
    }
}

function RenderFieldApplicationUser(field, span, fieldForm) {
    var extraClass = HasLayout(fieldForm, "chose") ? " selectChosen" : "";
    //if (HasPropertyValue(fieldForm.Layout)) {
    //    if (fieldForm.Layout.toLowerCase().indexOf("chosen") !== -1) {
    //        extraClass+= " selectChosen"
    //    }
    //}

    var res = "";
    res += "<div class=\"col-sm-" + (span - 1) + "\">";
    res += "<select id=\"" + field.Name + "\" class=\"form-control CmbAppplicationUsers" + extraClass + "\">";
    res += "</select>";
    res += "</div>";
    return res;
}

function RenderFieldUrl(field, span, fieldForm) {
    var res = "";
    res += "<div class=\"col-sm-" + (span - 1) + "\">";
    res += "  <div class=\"input-group date\">";
    res += "    <input id=\"" + field.Name + "\" type=\"text\" class=\"form-control\" autocomplete=\"off\">";
    res += "    <span id=\"" + field.Name + "_Btn\" class=\"input-group-addon input-group-button-url\"><i class=\"fa fa-globe\"></i></span>";
    res += "  </div>";
    res += "</div>";
    return res;
}

function RenderFieldEmail(field, span, fieldForm) {
    var res = "";
    res += "<div class=\"col-sm-" + (span - 1) + "\">";
    res += "  <div class=\"input-group date\">";
    res += "    <input id=\"" + field.Name + "\" type=\"text\" class=\"form-control\" autocomplete=\"off\">";
    res += "    <span id=\"" + field.Name + "_Btn\" class=\"input-group-addon input-group-button-emal\"><i class=\"fa fa-envelope\"></i></span>";
    res += "  </div>";
    res += "</div>";
    return res;
}

function RenderFieldText(field, span) {
    var res = "";
    res += "<div class=\"col-sm-" + (span - 1) + "\">";
    res += "    <input id=\"" + field.Name + "\" type=\"text\" class=\"form-control\" value=\"\" />";
    res += "</div>";
    return res;
}

function RenderFieldDocumentFile(field, span, fieldForm) {
    var res = "";
    var config = GetPropertyValue(fieldForm.Config, "LVADUSH");

    res += "<div class=\"col-sm-" + (span - 1) + "\">";
    res += "  <div class=\"input-group date\">";
    res += "    <input id=\"" + field.Name + "\" type=\"text\" class=\"form-control\" autocomplete=\"off\" readonly=\"readonly\">";

    if (config.indexOf("A") !== -1) {
        res += "    <span id=\"" + field.Name + "_BtnView\" class=\"input-group-addon\" title=\"Veure\"><i class=\"fa fa-eye\"></i></span>";
    }

    if (config.indexOf("U") !== -1) {
        res += "    <span id=\"" + field.Name + "_BtnUpload\" class=\"input-group-addon\" title=\"Pujar\"><i class=\"fa fa-upload\"></i></span>";
    }

    if (config.indexOf("D") !== -1) {
        res += "    <span id=\"" + field.Name + "_BtnDelete\" class=\"input-group-addon\" title=\"Eliminar\"><i class=\"fa fa-times grey\"></i></span>";
    }

    if (config.indexOf("S") !== -1) {
        res += "    <span id=\"" + field.Name + "_BtnSign\" class=\"input-group-addon\" title=\"Signar\"><i class=\"fa fa-file-signature\"></i></span>";
    }

    if (config.indexOf("H") !== -1) {
        res += "    <span id=\"" + field.Name + "_BtnHistory\" class=\"input-group-addon\ title=\"Veure historial\"><i class=\"fa fa-list\"></i></span>";
    }

    res += "  </div>";
    res += "</div>";
    return res;
}

function RenderFieldCheckBox(field, span) {
    var res = "";
    res += "<div class=\"col-sm-" + (span - 1) + "\">";
    res += "    <input type=\"checkbox\" id=\"" + field.Name + "\" class=\"form-control\" />";
    res += "</div>";
    return res;
}

function RenderFieldRadioButton(field, span) {
    var res = "";
    res += "<div class=\"col-sm-" + (span - 1) + "\">";
    res += "    <input type=\"radio\" name=\"" + field.Name + "\" id=\"" + field.Name + "_No\" class=\"form-radio form-radio-" + field.Name + "\" />&nbsp;" + Dictionary.Common_No;
    res += "&nbsp;&nbsp;&nbsp;";
    res += "    <input type=\"radio\" name=\"" + field.Name + "\" id=\"" + field.Name + "_Yes\" class=\"form-radio form-radio-" + field.Name + "\" />&nbsp;" + Dictionary.Common_Yes;
    res += "</div>";
    return res;
}

function RenderFieldTextArea(field, span, fieldForm) {
    var splited = false;
    if (HasPropertyValue(fieldForm.Layout) === true) {
        if (fieldForm.Layout === "Splited") {
            splited = true;
        }
    }

    var realSpan = splited ? span : span - 1;

    var rows = GetPropertyValue(fieldForm.Rows, 3);
    var res = "";
    res += "<div class=\"col-sm-" + realSpan + "\">";
    if (splited) {
        res += "<label>" + field.Label + "</label>";
    }
    res += "    <textarea id=\"" + field.Name + "\" class=\"form-control\" value=\"" + field.Type + "\" rows=\"" + rows + "\"></textarea>";
    res += "</div>";
    return res;
}

function RenderFieldNumeric(field, span) {
    var res = "";
    res += "<div class=\"col-sm-" + (span - 1) + "\">";
    res += "    <input id=\"" + field.Name + "\" type=\"text\" class=\"form-control\" value=\"\" />";
    res += "</div>";
    return res;
}

function RenderFieldFK(field, span, fieldForm) {
    var bar = HasLayout(fieldForm, "BAR");
    //if (HasPropertyValue(fieldForm.Layout)) {
    //    if (fieldForm.Layout.indexOf("BAR") !== -1) {
    //        bar = true;
    //    }
    //}

    var res = "";

    var hidden = GetPropertyValue(fieldForm.Hidden, false) ? " style=\"visibility:hidden;\"" : "";

    res += "<div class=\"col-sm-" + (span - 1) + "\"" + hidden + ">";
    if (bar === true) {
        res += "  <div class=\"input-group BAR\">";
    }
    res += "    <select id=\"" + field.Name + "\" class=\"form-control\">";
    res += "    </select>";
    if (bar === true) {

        res += "    <span id=\"" + field.Name + "_Btn\" data-itemdefinition=\"" + field.Name.split("Id")[0] + "\" class=\"input-group-addon\" onclick=\"PopupBAR('" + field.Name.split("Id")[0] + "')\"><i class=\"fa fa-ellipsis-h\"></i></span>";
        res += "</div>";
    }
    res += "</div>";
    return res;
}

function RenderFieldFixedList(field, span, fieldForm) {
    var list = FixedLists[field.FixedListName];
    var res = "";

    var layout = "";
    if (HasPropertyValue(fieldForm.Layout)) {
        layout = fieldForm.Layout.toLowerCase();
    }

    switch (layout) {
        case "radio":
            res += "<div class=\"col-sm-" + (span - 1) + "\" style=\"min-height:36px;\">";
            res += "    <input id=\"" + field.Name + "\" type=\"text\" style=\"display:none;\" class=\"form-control\" value=\"" + field.Type + "\" />";

            if (list !== null) {
                // Empieza desde 1 porque 0 es "no definit"
                for (var x = 1; x < list.length; x++) {
                    if (x > 1) {
                        res += "&nbsp;&nbsp;&nbsp;&nbsp;";
                    }

                    res += "<input type=\"radio\" onclick=\"Layout_FixedListRB_Clicked(this);\" name=\"RB_" + field.Name + "\" id=\"RB_" + field.Name + "_" + x + "\" />";
                    res += "&nbsp;" + list[x];
                }
            }
            else {
                res += "Fixedlist " + field.FixedListName;
            }

            res += "</div>";
            break;
        case "check":
            res += "<div class=\"col-sm-" + (span - 1) + "\" style=\"min-height:36px;\">";
            res += "    <input id=\"" + field.Name + "\" type=\"text\" style=\"display:none;\" class=\"form-control fixedList_check\" value=\"\" />";

            if (list !== null) {
                for (var x = 0; x < list.length; x++) {
                    if (x > 0) {
                        res += "&nbsp;&nbsp;&nbsp;&nbsp;";
                    }

                    res += "<label onclick=\"Layout_FixedListCK_Clicked('" + field.Name + "'," + x + ")\">";
                    res += "  <input type=\"checkbox\" class=\"CK_" + field.Name + "\" style=\"float:left;margin:5px 0 0 0!important;\" id=\"CK_" + field.Name + "_" + x + "\" />";
                    res += "  <span style=\"float:left;margin-top:3px;margin-left:4px;padding:0\">" + list[x] + "</span>";
                    res += "</label>";
                }
            }
            else {
                res += "Fixedlist " + field.FixedListName;
            }

            res += "</div>";
            break;
        default:
            res += "<div class=\"col-sm-" + (span - 1) + "\">";
            res += "<select id=\"" + field.Name + "\" class=\"form-control\">";
            if (list !== null) {
                for (var x = 0; x < list.length; x++) {
                    var text = list[x];
                    if (x === 0) {
                        text = "Seleccionar";
                    }

                    res += "<option value=\"" + x + "\">" + text + "</option>";
                }
            }
            res += "</select>";
            res += "</div>";
            break;
    }

    return res;
}

function RenderFieldFixedListMultiple(field, span, fieldForm) {
    var list = FixedLists[field.FixedListName];
    var res = "";

    res += "<div class=\"col-sm-" + (span - 1) + "\" style=\"min-height:36px;\">";
    res += "    <input id=\"" + field.Name + "\" type=\"text\" style=\"display:none;\" class=\"form-control fixedList_check\" value=\"\" />";

    if (list !== null) {
        for (var x = 0; x < list.length; x++) {
            if (x > 0) {
                res += "&nbsp;&nbsp;&nbsp;&nbsp;";
            }

            res += "<label onclick=\"Layout_FixedListCK_Clicked('" + field.Name + "'," + x + ")\">";
            res += "  <input type=\"checkbox\" class=\"CK_" + field.Name + "\" style=\"float:left;margin:5px 0 0 0!important;\" id=\"CK_" + field.Name + "_" + x + "\" />";
            res += "  <span style=\"float:left;margin-top:3px;margin-left:4px;padding:0\">" + list[x] + "</span>";
            res += "</label>";
        }
    }
    else {
        res += "Fixedlist " + field.FixedListName;
    }

    res += "</div>";
    return res;
}

function RenderFieldDate(field, span) {
    var res = "";
    res += "<div class=\"col-sm-" + (span - 1) + "\">";
    res += "  <div class=\"input-group date\" style=\"width:112px;\">";
    res += "    <input id=\"" + field.Name + "\" type=\"text\" class=\"form-control datepicker\" autocomplete=\"off\" style=\"width:85px;\">";
    res += "    <span id=\"" + field.Name + "BtnDatepicker\" class=\"input-group-addon\"><i class=\"fa fa-calendar\"></i></span>";
    res += "  </div>";
    res += "</div>";
    return res;
}

function RenderFieldDecimal(field, span, fieldForm) {
    var precission = 3;
    if (HasPropertyValue(field.Precission)) {
        precission = field.Precission;
    }

    var res = "";
    res += "<div class=\"col-sm-" + (span - 1) + "\">";
    res += "  <input id=\"" + field.Name + "\" type=\"text\" class=\"form-control format-money\" data-precision=\"" + precission + "\">";
    res += "</div>";
    return res;
}

function RenderFieldMoney(field, span, fieldForm) {
    var res = "";
    res += "<div class=\"col-sm-" + (span - 1) + "\">";
    res += "  <div class=\"input-group date\" style=\"width:112px;\">";
    res += "    <input id=\"" + field.Name + "\" type=\"text\" class=\"form-control format-money\">";
    res += "    <span id=\"" + field.Name + "_Btn\" class=\"input-group-addon\"><i class=\"fa fa-euro-sign\"></i></span>";
    res += "  </div>";
    res += "</div>";
    return res;
}

function RenderFieldBlank(span) {
    var res = "";
    res += "<div class=\"col-sm-" + (span - 1) + "\">&nbsp;</div>";
    return res;
}

function RenderFieldFree(span, formField) {
    var res = "";
    var splited = false;
    var realSpan = span;
    var labelSpan = 1;

    if (HasPropertyValue(formField.Layout)) {
        if (formField.Layout === "Splited") {
            splited = true;
            realSpan = 12;
            labelSpan = 12;
        }
    }

    if (splited === true) {
        res += "<div class=\"col-sm-" + span  + "\">";
    }

    if (HasPropertyValue(formField.Label)) {
        res += "<label id=\"" + formField.Id + "_Label\" class=\"col-sm-" + labelSpan + " control-label\">" + formField.Label + "</label>";
    }

    res += "<div id=\"" + formField.Id + "\" class=\"col-sm-" + realSpan + "\">Free:" + formField.Id + "</div>";

    if (splited === true) {
        res += "</div>";
    }

    return res;
}

function RenderFieldButton(span, field) {
    var icon = "";
    if (HasPropertyValue(field.Icon)) {
        icon = "<i class=\"" + field.Icon + "\"></i>&nbsp;";
    }

    var title = "";
    if (HasPropertyValue(field.Help)) {
        title = " title=\"" + field.Help + "\"";
    }

    var action = "";
    if (HasPropertyValue(field.Action)) {
        action = " onclick=\"" + field.Action + "();\"";
    }

    var res = "";
    res += "<div id=\"" + field.Id + "_Div\" class=\"col-sm-" + (span - 1) + "\">";
    res += "<button id=\"" + field.Id + "\" type=\"button\" style=\"padding-top:0;padding-bottom:0;\" class=\"btn btn-xs btn-info\"" + action + title + ">" + icon + field.Label + "</button>";
    res += "</div>";
    return res;
}

function RenderFieldRange(field, span) {
    var res = "";
    res += "<div class=\"col-sm-" + (span - 1) + "\">";
    res += "  <input type=\"hidden\" id=\"" + field.Name + "\" />";
    res += "  <label class=\"col-xs-2\" id=\"" + field.Name + "_Value\">0</label>";
    res += "  <div style=\"border:1px solid red;\" class=\"col-xs-10 simple-slider\" id=\"" + field.Name + "_Slider\" data-loaded=\"0\" data-min=\"" + field.Values[0] + "\" data-max=\"" + field.Values[1] + "\"/></span>";
    res += "</div>";

    return res;
}

function FillCmbApplicationUsers() {
}

function FieldToLabel(fieldName) {
    if ($("#" + fieldName).length === 0) {
        return false;
    }

    $("#" + fieldName + "_Labeled").remove();
    if ($("#" + fieldName + ":checkbox").length > 0) {
        $("#" + fieldName + "Label").prepend($("#" + fieldName).prop("checked") === true ? "<i class=\"fa fa-check-square green\" title=\"" + Dictionary.Common_Yes + "\"></i>" : "<i class=\"fa fa-square\" title=\"" + Dictionary.Common_No + "\"></i>");
        $("#" + fieldName).remove();
    }
    else if ($("#Div" + fieldName + " select").length > 0) {
        $("#" + fieldName).hide();
        $("#" + fieldName + "_chosen").hide();
        $("#" + fieldName).after("<span id=\"" + fieldName + "_Labeled\">" + $("#" + fieldName + " option:selected").text()) + "</span>";
    }
    else if ($("#" + fieldName + "Select").length > 0) {
        $("#" + fieldName).hide();
        $("#" + fieldName + "Select").hide();
        $("#" + fieldName).after("<span id=\"" + fieldName + "_Labeled\">" + $("#" + fieldName + "Select option:selected").text()) + "</span>";
    }
    else if ($("#" + fieldName + " :selected").length > 0) {
        var text = $("#" + fieldName + " :selected").text();
        $("#" + fieldName).append(text);
    }
    else {
        $("#" + fieldName).hide();
        $("#" + fieldName + "BtnDatepicker").hide();

        var value = $("#" + fieldName).val();
        if (typeof itemData !== "undefined" && itemData !== null) {
            if (HasPropertyValue(itemData[fieldName])) {
                value = itemData[fieldName];
            }
        }

        $("#" + fieldName).after("<span id=\"" + fieldName + "_Labeled\">" + value + "</span>");
    }

    var label = $("#" + fieldName + "Label");
    if (typeof label[0] !== "undefined") {
        if (label[0].textContent.indexOf('*') !== -1) {
            label.html(label[0].textContent.split('*')[0]);
        }
    }

    $("#AuxButton-" + fieldName).remove();

    if ($("#" + fieldName + "BtnMoneyAddon").length > 0) {
        $("#" + fieldName + "BtnMoneyAddon").remove();
        $("#" + fieldName + "_Labeled").html(ToMoneyFormat(StringToNumber($("#" + fieldName).val())) + "&nbsp;&euro;");
    }
}

function HasLayout(fieldForm, layout) {
    if (HasPropertyValue(fieldForm.Layout)) {
        if (fieldForm.Layout.toLowerCase().indexOf(layout.toLowerCase()) !== -1) {
            return true;
        }
    }

    return false;
}