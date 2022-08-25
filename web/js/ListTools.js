var ListRenderTopFirst = 100;

function PageList(config) {
    var _this = this;
    this.Data = [];
    this.FilteredData = [];
    this.ListDefinition = config.ListDefinition;
    this.ItemDefinition = ItemDefinitionByName(config.ItemName);
    this.CustomAjaxSource = config.ListDefinition.CustomAjaxSource;
    this.Parameters = config.ListDefinition.Parameters;
    this.Filter = GetFilter(config.ListDefinition.Columns);
    this.ExtraFilter = [];
    this.ItemName = this.ItemDefinition.ItemName;
    this.ListId = this.ListDefinition.Id;
    this.ShowHiddenData = false;
    this.Widths = [];
    this.DefaultColumnWidth = 0;
    this.Origin = typeof config.Origin !== "undefined" ? config.Origin : null;
    this.ComponentId = this.ItemDefinition.ItemName + "_" + this.ListDefinition.Id;
    this.PageIndex = 0;
    this.ActionsButtonsCount = 0;
    this.TabId = typeof config.TabId !== "undefined" ? config.TabId : "NoTab";

    // --------------------- RENDERING
    this.Search = function () {
        console.log("nav-search-input", $("#nav-search-input").val());
        this.ExtraFilter = [];
        if (HasArrayValues(this.ListDefinition.Filter)) {
            for (var x = 0; x < this.ListDefinition.Filter.length; x++) {
                var criteria = this.ListDefinition.Filter[x];

                console.log(criteria);

                if (criteria.Type === '') {
                    var value = $("#Filter_" + criteria.DataProperty).val() * 1;
                    if (value > 0) {
                        ListSources[0]["ExtraFilter"].push({ "Field": "" + criteria.DataProperty, "Subfield": "Id", "Value": value });
                    }
                }
                else if (criteria.Type.toLowerCase() === "isnull") {
                    var active = $("#" + criteria.DataProperty + "_0").prop("checked");
                    var inactive = $("#" + criteria.DataProperty + "_1").prop("checked");

                    if (active == false || inactive == false) {
                        if (active === true) {
                            ListSources[0]["ExtraFilter"].push({ "Field": "" + criteria.DataProperty + "", "Value": "ISNULL" });
                        }
                        if (inactive === true) {
                            ListSources[0]["ExtraFilter"].push({ "Field": "" + criteria.DataProperty + "", "Value": "NOTNULL" });
                        }
                    }
                }
                else if (criteria.Type.toLowerCase() === "checkbox") {
                    // total cheboxes
                    var total = $(".Filter_" + criteria.DataProperty).length;
                    var selected = $(".Filter_" + criteria.DataProperty + ":checked").length;
                    var comparedValue = "|";
                    if (selected > 0 && selected < total) {
                        for (var x = 0; x < $(".Filter_" + criteria.DataProperty).length; x++) {
                            if ($("#Filter_" + criteria.DataProperty + "_" + (x + 1)).prop("checked") === true) {
                                comparedValue += (x + 1) + "|";
                            }
                        }

                        ListSources[0]["ExtraFilter"].push({ "Field": criteria.DataProperty, "Value": comparedValue, "Comparer": "INLIST" });
                    }
                }
                else if (criteria.Type.toLowerCase() === "customcheckbox") {
                    var value = null;
                    if (HasPropertyValue(criteria.GetValue)) {
                        var typeofValue = eval("typeof " + criteria.GetValue);
                        if (typeofValue === "function") {
                            value = window[criteria.GetValue]();
                        }
                    }

                    if (value !== null) {
                        ListSources[0]["ExtraFilter"].push({ "Field": criteria.Id, "Value": value, "Comparer": "BINARYCONTAINS" });
                    }
                }
            }
        }

        console.log("Extrafilter", ListSources[0]["ExtraFilter"]);

        SearchList();
    }

    this.ButtonAddLabel = function () {
        var res = this.ListDefinition.ButtonAddLabel;
        if (typeof res === "undefined" || res === null || res === "") {
            res = Dictionary.Common_Add + " " + this.ItemDefinition.Layout.Label.toLowerCase();
        }

        return res;
    }

    this.RenderPageList = function () {
        var res = "";
        this.CalculateWidths(this.ListDefinition, this.ItemDefinition);

        var Title = GetPropertyValue(this.ListDefinition.Title, this.ItemDefinition.Layout.LabelPlural);
        var ButtonAddLabel = this.ButtonAddLabel();

        res += "<div id=\"" + this.ComponentId + "_List\" class=\"ListContainer\">";
        res += "  <div class=\"hpanel hblue hpanel-table\" style=\"margin:0;\">";
        res += "    <div class=\"panel-heading hbuilt\">";
        res += "      <span id=\"" + this.ComponentId + "_ListTitle\">" + Title + "</span>";
        res += "      <div class=\"panel-tools\">";

        if (HasPropertyValue(this.ListDefinition.Export)) {
            var exportActions = this.ListDefinition.Export.split('|');
            var extraExport = [];
            for (var a = 0; a < exportActions.length; a++) {
                var exportAction = exportActions[a];

                if (exportAction === "PDF") {
                    res += "    <a data-action=\"add\" style=\"border:1px solid #00f; color:#fff;\" class=\"TableInFormActionBtn\" id=\"BtnExport_PDF\" onclick=\"List_Export('PDF','" + this.ItemName + "','" + this.ListId + "');\">";
                    res += "     <i class=\"ace-icon fal fa-file-pdf\"></i>&nbsp;" + Dictionary.Common_ListPdf;
                    res += "    </a>";
                    res += " | ";
                }
                else {
                    extraExport.push(exportAction);
                }
            }

            if (extraExport.length === 1) {
                res += "    <a data-action=\"add\" class=\"TableInFormAction\" id=\"BtnExport_" + extraExport[0] + "\" onclick=\"List_Export('" + extraExport[0] + "','" + this.ItemName + "','" + this.ListId + "');\">";
                res += "      Exportar " + extraExport[0];
                res += "    </a>";
                res += " | ";
            }
            else if (extraExport.length > 1) {
                res += "<a class=\"TableInFormAction dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\">";
                res += "    <i class=\"ace-icon fad fa-file-export\"></i>&nbsp;Exportar";
                res += "    <i class=\"ace-icon fa fa-chevron-down icon-on-right\"></i>";
                res += "</a>";
                res += "<ul class=\"dropdown-menu dropdown-blue dropdown-menu-right dropdown-caret dropdown-close\">";
                for (var x = 0; x < extraExport.length; x++) {
                    res += "    <li><a id=\"BtnExport_" + extraExport[x] + "\" onclick=\"List_Export('" + extraExport[x] + "','" + this.ItemName + "','" + this.ListId + "');\" style=\"cursor:pointer;\">Exportar " + extraExport[x] + "</a></li>";
                }
                res += "</ul>";
                res += " | ";
            }
        }

        if (HasArrayValues(this.ListDefinition.Actions)) {
            for (var a = 0; a < this.ListDefinition.Actions.length; a++) {
                var action = this.ListDefinition.Actions[a];
                res += "    <a data-action=\"add\" class=\"TableInFormAction\" id=\"" + action.Action + "_Btn\" onclick=\"" + action.Action + "();\">";
                res += "     <i class=\"ace-icon " + action.Icon + "\"></i>&nbsp;" + action.Label;
                res += "    </a>";
                res += " | ";
            }
        }

        res += "        <a id=\"" + this.ComponentId + "_AddBtn\" onclick=\"GoEncryptedView('" + this.ItemDefinition.ItemName + "', '" + this.ListId + "', -1,'" + this.ListDefinition.FormId + "', null)\"><i class=\"fa fa-plus\"></i>&nbsp;<span id=\"" + this.ComponentId + "_AddBtnLabel\">" + ButtonAddLabel + "</span></a>";
        res += "      </div>";
        res += "    </div>";

        res += this.RenderFilter();

        res += "    <div class=\"tableHead\">";
        res += "      <table cellpadding=\"1\" cellspacing=\"1\" class=\"table\">";
        res += "        <thead id=\"" + this.ComponentId + "_ListHead\">";
        res += "          <tr>";
        res += this.RenderHeader(this.ItemDefinition, this.ListDefinition, false, 0, this.Widths, 1);
        res += "          </tr>";
        res += "        </thead>";
        res += "      </table>";
        res += "    </div>";

        res += "    <div class=\"panel-body panel-body-list\">";
        res += "      <div class=\"table-responsive\">";
        res += "         <div class=\"table-body\" style=\"max-height: 100%; height: 100%\">";
        res += "           <table cellpadding=\"1\" cellspacing=\"1\" class=\"table\" style=\"max-height: 100%\">";
        res += "             <tbody style=\"max-height: 100%\" id=\"" + this.ComponentId + "_ListBody\"></tbody>";
        res += "           </table>";
        res += "         </div>";
        res += "      </div>";
        res += "    </div><!-- panel-body -->";

        res += "    <div class=\"panel-footer panel-footer-list\">";
        res += "      Nombre de registres: <strong id=\"" + this.ComponentId + "_ListCount\">&nbsp;-</strong>";
        res += "    </div><!-- panel-body -->";

        res += "  </div>";
        res += "</div>";


        $("#TableList").html(res);
        $(".filter .datepicker").localDatePicker();
        ResizeWorkArea();
    }

    this.Render = function (forcedHeight, tabId) {
        this.CalculateWidths(this.ListDefinition, this.ItemDefinition);
        var BtnAddLabel = this.ButtonAddLabel();

        if (this.ListDefinition.EditAction === "FormPage") {
            $("#" + this.ComponentId + "AddBtn").data("itemDefinitionId", this.ItemDefinition.Id);
            $("#" + this.ComponentId + "AddBtn").data("formId", this.ListDefinition.FormId);
            $("#" + this.ComponentId + "AddBtn").on("click", function () {
                console.log($(this).data("itemDefinitionId"));
                console.log($(this).data("formId"));
                GoEncryptedView($(this).data("itemDefinitionId"), -1, $(this).data("formId"));
            });
        }
        else if (this.ListDefinition.EditAction === "InLine") {
            $("#" + this.ComponentId + "AddBtn").data("itemDefinitionId", this.ItemDefinition.Id);
            $("#" + this.ComponentId + "AddBtn").data("formId", this.ListDefinition.FormId);
            $("#" + this.ComponentId + "AddBtn").on("click", function () {
                console.log($(this).data("itemDefinitionId"));
                console.log($(this).data("formId"));
                //GoEncryptedView($(this).data("itemDefinitionId"), -1, $(this).data("formId"));
            });
        }

        $("#" + this.ComponentId + "AddBtnLabel").html(BtnAddLabel);
        $("#" + this.ComponentId + "ListTitle").html(this.ItemDefinition.Layout.LabelPlural);

        if (HasPropertyValue(this.ListDefinition.PageSize) === false) {
            this.ListDefinition.PageSize = ListRenderTopFirst;
        }

        var height = 400; //weke ListSetHeight(this.ListDefinition, forcedHeight);
        var tableHeight = height - 78;

        if (tableHeight < 0) {
            tableHeight = 100;
        }

        var noDataMessage = HasPropertyValue(this.ListDefinition.NoDataMessage) ? this.ListDefinition.NoDataMessage : Dictionary.Common_NoData;
        if (HasPropertyValue(this.ListDefinition.NoDataMessage)) {
            noDataMessage = this.ListDefinition.NoDataMessage;
        }

        var res = "";

        // Generar cabecera de tabla con título y botones si no es un página ListItem
        // Si ListDefinition.EditAction = 3 se pueden añadir (dependiendo de grants usuario)
        if (PageType !== "PageList") {
            res += "  <div class=\"hpanel hblue hpanel-table\" style=\"margin:0;\" id=\"" + this.ComponentId + "_PanelBody\">";
            res += "    <div class=\"panel-heading hbuilt\">";


            
            var headerTable = "";
            var Title = GetPropertyValue(this.ListDefinition.Title, this.ItemDefinition.Layout.LabelPlural);
            // ItemLinked
            if (this.ListDefinition.EditAction === "ItemLink") {
                var itemLinked = GetItemDefinitionByName(this.ListDefinition.ItemLink);
                var itemLikedHostName = itemDefinition.ItemName;
                var listData = FK[this.ListDefinition.ItemLink].Data;
                headerTable += "<!-- List itemLinked header -->";
                headerTable += "<div class=\"row\" id=\"List_" + itemLinked.ItemName + "_Header\">";
                headerTable += "<div class=\"form-group col-sm-12\" id=\"List_" + itemLinked.ItemName +"_Body\">";
                headerTable += "    <label id=\"ItemLink" + itemLinked.ItemName + "Label\" class=\"col-sm-2\"> " + itemLinked.Layout.LabelPlural + "<span style=\"color:#ff0000;\">*</span ></label>";
                headerTable += "    <div class=\"col-sm-5\">";
                headerTable += "        <select class=\"form-control col-xs-12 col-sm-12 chosen-select\" id=\"ItemLink" + itemLinked.ItemName + "Id\" data-placeholder=\"\" onchange=\"\" >";
                headerTable += "            <option value=\"0\" > Seleccionar</option>";

                for (var x = 0; x < listData.length; x++) {
                    headerTable += "<option value=\"" + listData[x].Id + "\">" + listData[x].Description + "</option>";
                }

                headerTable += "        </select>";
                headerTable += "    </div>";
                headerTable += "<button data-listItem=\"" + this.ItemDefinition.ItemName + "\" data-itemBridge=\"" + this.ItemDefinition.ItemName + "\" data-listId=\"" + this.ListDefinition.Id + "\" data-linkedItem=\"" + itemLinked.ItemName + "\" data-linkedItemHost=\"" + itemLikedHostName + "\" class=\"btn btn-info\" type=\"button\" id=\"ItemLinkBtnAdd" + itemLinked.ItemName + "\" style=\"border:none;height:25px;padding-top:2px;\" onclick=\"$('#ItemLinkBtnAdd" + itemLinked.ItemName + "').attr('disabled', 'disabled');ItemLinkedSetLink(this);\"><i class=\"fa fa-plus\"></i> " + Dictionary.Common_Add + " " + itemLinked.Layout.Label + "</button>";
                headerTable += "&nbsp;<span style=\"color:#c00;\" id=\"" + itemLikedHostName + "_Add" + itemLinked.ItemName + "Error\"></span>";
                headerTable += "</div></div>";
                headerTable += "<!-- End list itemLinked header -->";
            }

            if (HasPropertyValue(this.ListDefinition.Title) || this.ListDefinition.EditAction === "EditableAdd" || this.ListDefinition.EditAction === "Popup") {
                headerTable += "<div class=\"row\" id=\"List_" + this.ItemDefinition.ItemName +"_Header\">";
                headerTable += "  <div class=\"col-xs-6 col-sm-8\" style=\"padding-top:12px;\">";
                if (HasPropertyValue(this.ListDefinition.Title)) {
                    headerTable += "    <div class=\"listTitle\">" + Title + "</div>";
                }

                headerTable += "  </div>";
                headerTable += "  <div class=\"listTitleButtons col-xs-6 col-sm-4\" style=\"text-align:right;padding-top:12px;\">";

                if (HasArrayValues(this.Actions)) {
                    headerTable += "    <a data-action=\"add\" class=\"TableInFormAction\" id=\"BtnAddItem" + this.ComponentId + "\" onclick=\"" + editAction + "\">";
                    headerTable += "     <i class=\"ace-icon fa fa-cog\"></i>&nbsp;hola";
                    headerTable += "    </a>";
                }

                if (this.ListDefinition.EditAction === "Popup" || this.ListDefinition.EditAction === "EditableAdd") {
                    var editAction = "PopupNew('" + this.ItemDefinition.ItemName + "', '" + this.ListDefinition.FormId + "')";
                    if (this.ListDefinition.EditAction === "Popup") {
                        editAction = "PopupItem({'ItemName': '" + this.ItemDefinition.ItemName + "','ListId': '" + this.ListDefinition.Id + "','ItemId': -1});";
                    }

                    headerTable += "    <a data-action=\"add\" class=\"TableInFormAction\" id=\"BtnAddItem" + this.ComponentId + "\" onclick=\"" + editAction + "\">";
                    headerTable += "     <i class=\"ace-icon fa fa-cog\"></i>&nbsp;" + Dictionary.Common_Add + " " + this.ItemDefinition.Layout.Label.toLowerCase();
                    headerTable += "    </a>";
                }

                headerTable += "  </div>";
                headerTable += "</div>";
            }

            if (HasPropertyValue(this.ListDefinition.Explanation)) {
                res += "<div class=\"col-xs-12\" style=\"margin-bottom:4px;\" id=\"List_" + this.ItemDefinition.ItemName +"_Explanation\">";
                res += RenderExplanation("info", this.ListDefinition.Explanation);
                res += "</div>";
            }

            res += headerTable;
        }
        else {
            // Si es listPage el botón ya existe sólo hay que cambiar el onclick.
            if (this.ListDefinition.EditAction === "Popup") {
                $("#BtnAddItem").removeAttr("onclick");
                $("#BtnAddItem").on("click", function () {
                    PopupItem(itemDefinition.ItemName, listDefinition.Id, -1);
                });
            }

            if (HasPropertyValue(this.ListDefinition.Explanation)) {
                res += "<div class=\"col-xs-12\" style=\"margin-bottom:4px;\" id=\"List_" + this.ItemDefinition.ItemName + "_Explanation\">";
                res += RenderExplanation("info", this.ListDefinition.Explanation);
                res += "</div>";
            }
        }

        // Cabecera de lista
        // ----------------------------------------
        res += "      <div class=\"tableHead\">";
        res += "        <table class=\"table\">";
        res += "          <thead id=\"" + this.ComponentId + "_ListHead\">";
        res += this.RenderHeader(this.ItemDefinition, this.ListDefinition, false, 0, this.Widths, 2);
        res += "          </thead>";
        res += "        </table>";
        res += "      </div>";
        // ----------------------------------------

        // Datos de lista
        // ----------------------------------------
        res += "      <div class=\"panel-body2 panel-body-list-inForm\" id=\"" + this.ComponentId + "_PanelBodyList\">";
        res += "        <div class=\"table-responsive\" style=\"max-height: 100%; height: 100%; overflow-y: scroll; overflow-x: hidden\">";
        res += "          <div class=\"table-body\" style=\"max-height: 100%; height: 100%\">";
        res += "            <table class=\"table\" style=\"max-height: 100%\" cellpadding=\"1\" cellspacing=\"1\">";
        res += "              <tbody id=\"" + this.ComponentId + "_ListBody\">";
        res += "              </tbody>";
        res += "            </table>";
        res += "          </div>";
        res += "        </div>";
        res += "      </div>";
        // ----------------------------------------


        // Pie de lista
        // ----------------------------------------
        res += "      <div class=\"panel-footer\">";
        res += "        Nº de registros: <strong id=\"" + this.ComponentId + "_ListCount\"></strong>";
        res += "      </div>";
        res += "  </div>";
        // ----------------------------------------

        $("#" + this.ComponentId + "_List").html(res);

        //$("#" + this.ComponentId + "_ListHead").html(this.RenderHeader(this.ItemDefinition, this.ListDefinition, false, 0, this.Widths, 2));
    };

    this.RenderFilter = function () {
        var res = "";
        if (HasArrayValues(this.ListDefinition.Filter)) {
            ListHeightDelta = 280;
            res += "      <div class=\"panel-heading filter\">";

            for (var x = 0; x < this.ListDefinition.Filter.length; x++) {
                var filter = this.ListDefinition.Filter[x];

                if (HasPropertyValue(filter.Type) === false) {
                    filter.Type = "";
                }

                if (HasPropertyValue(filter.DataProperty) === false) {
                    filter.DataProperty = "";
                }

                var field = GetFieldDefinition(filter.DataProperty, this.ItemDefinition);

                if (filter.Type.toLowerCase() === "customcheckbox") {
                    var label = "";
                    if (HasPropertyValue(filter.Label)) {
                        label = "<label>" + filter.Label +"</label>:&nbsp;";
                    }

                    var parts = filter.Options.split('|');

                    res += "<span style=\"margin-right:12px;\">";
                    res += label;

                    for (var p = 0; p < parts.length; p++) {
                        res += "<input type=\"checkbox\" class=\"Filter_" + filter.Id + "\" id=\"Filter_" + filter.Id + "_" + p + "\" style=\"margin:0!important;\" onclick=\"FilterList('" + this.ListId + "','" + this.ItemName + "');\">&nbsp;" + parts[p];
                        res += "&nbsp;";
                    }

                    res += "</span>";
                }
                else if (filter.Type.toLowerCase() === "customselect") {
                    var label = "";
                    if (HasPropertyValue(filter.Label)) {
                        label = "<label>" + filter.Label + "</label>:&nbsp;";
                    }

                    var parts = filter.Options.split('|');

                    res += "<span style=\"margin-right:12px;\">";
                    res += label;

                    res += " <select id=\"Filter_" + filter.If + "\" onchange=\"FilterList('" + this.ListId + "','" + this.ItemName + "');\">";
                    for (var o = 0; o < parts.length; o++) {                        
                        res += "<option value=\"" + o + "\">" + parts[o] + "</option>";
                    }

                    res += "</select>";

                    res += "</span>";
                }
                else if(filter.Type.toLowerCase() === "isnull") {
                    var label = field.Label;
                    var label1 = "Informat";
                    var label2 = "No informat";

                    if (HasPropertyValue(filter.Label)) {
                        label = filter.Label;
                    }

                    if (label.trim() !== "") {
                        label = "<label>" + label + "</label>:&nbsp;";
                    }

                    if (HasPropertyValue(filter.Options)) {
                        if (filter.Options.indexOf('|') !== -1) {
                            label1 = filter.Options.split('|')[0];
                            label2 = filter.Options.split('|')[1];
                        }
                    }

                    res += "<span style=\"margin-right:12px;\">";
                    res += label;
                    res += "<input type=\"checkbox\" id=\"" + field.Name + "_0\" style=\"margin:0!important;\" onclick=\"FilterList('" + this.ListId + "','" + this.ItemName + "');\">&nbsp;" + label1;
                    res += "&nbsp;";
                    res += "<input type=\"checkbox\" id=\"" + field.Name + "_1\" style=\"margin:0!important;\" onclick=\"FilterList('" + this.ListId + "','" + this.ItemName + "');\">&nbsp;" + label2;
                    res += "</span>";
                }
                else if (filter.Type.toLowerCase() === "daterange") {
                    var label = HasPropertyValue(filter.Label) ? filter.Label : field.Label;

                    res += "<span style=\"margin-right:12px;\">";
                    res += "<label style=\"float:left;\">" + label + ":&nbsp;</label>";
                    res += "<span class=\"input-group date\" style=\"width:112px;float:left;\">";
                    res += "  <input id=\"DateRealStart\" type=\"text\" class=\"form-control datepicker\" autocomplete=\"off\" style=\"width:85px;height:22px;\">";
                    res += "  <span id =\"DateRealStartBtnDatepicker\" class=\"input-group-addon\"><i class=\"fa fa-calendar\"></i></span>";
                    res += "</span>";
                    res += "<span class=\"input-group date\" style=\"width:112px;float:left;\">";
                    res += "  <input id=\"DateRealStart\" type=\"text\" class=\"form-control datepicker\" autocomplete=\"off\" style=\"width:85px;height:22px;\">";
                    res += "  <span id =\"DateRealStartBtnDatepicker\" class=\"input-group-addon\"><i class=\"fa fa-calendar\"></i></span>";
                    res += "</span>";
                    res += "</span>";
                }
                else {
                    if (field.Type == "FixedList") {
                        var list = FixedLists[field.FixedListName];
                        if (HasPropertyValue(filter.Type)) {
                            if (filter.Type.toLowerCase() === "checkbox") {
                                res += "<span style=\"margin-right:12px;\">";
                                res += "<label>" + field.Label + "</label>:&nbsp;";
                                for (var o = 1; o < list.length; o++) {
                                    res += "<input type=\"checkbox\" class=\"Filter_" + field.Name + "\" id=\"Filter_" + field.Name + "_" + o + "\" style=\"margin:0!important;\"  onclick=\"FilterList('" + this.ListId + "','" + this.ItemName + "');\" />&nbsp;" + list[o] + "&nbsp;";
                                }
                                res += "</span>";
                            }
                        }
                        else {
                            res += "<span><label>" + field.Label + "</label>:&nbsp;";
                            res += " <select id=\"Filter_" + filter.DataProperty + "\" onchange=\"FilterList('" + this.ListId + "','" + this.ItemName + "');\">";
                            for (var o = 0; o < list.length; o++) {
                                res += "<option value=\"" + o + "\">" + list[o] + "</option>";
                            }

                            res += "</select>";
                            res += "</span>";
                        }
                    }
                    else if (IsFK(this.ItemDefinition, field.Name)) {
                        res += "<span><label>" + field.Label + "</label>:&nbsp;";
                        res += "<select id=\"Filter_" + filter.DataProperty + "\" onchange=\"FilterList('" + this.ListId + "','" + this.ItemName + "');\">";
                        res += "<option value=\"-1\">" + Dictionary.Common_All + "</option>";
                        var list = FK[field.Name.substr(0, field.Name.length - 2)].Data;
                        for (var o = 0; o < list.length; o++) {
                            if (list[o].Active) {
                                res += "<option value=\"" + list[o].Id + "\">" + list[o].Description + "</option>";
                            }
                        }
                        res += "</select>";
                        res += "</span>";
                    }
                }                
            }
            
            res += "      </div>";
        }

        return res;
    }

    this.RenderHeader = function (itemDefinition, listDefinition, filtrable, tabId, widths, actionButtonsCount) {
        var res = "";
        if (filtrable !== true && HasPropertyValue(listDefinition.Label)) {
            var z = "";
        }
        else {
            if (HasPropertyValue(listDefinition.Label)) {
                res += "<tr>";
                res += "<th colspan=\"" + (listDefinition.Columns.length + 1) + "\" style=\"border-right:1px solid #ddd;\">" + listDefinition.Label + "</th>";
                res += "</tr>";
            }
        }

        res += "<tr>";

        var i = 0;
        if (FeatureEnabled("Unloadable", itemDefinition) && GrantByItem(itemDefinition.ItemName, "H")) {
            res += "<th id=\"th0\" style=\"width:30px;\">&nbsp;</th>";
            i = 1;
        }

        var searchColumns = [];
        for (var c = 0; c < listDefinition.Columns.length; c++) {
            var column = listDefinition.Columns[c];
            if (column.DataProperty === "Id") { continue; }

            if (typeof column.HiddenList !== "undefined" && column.HiddenList !== null && column.HiddenList === true) {
                continue;
            }

            var labelHeader = column.DataProperty;
            var field = GetFieldDefinition(column.DataProperty, itemDefinition);
            if (HasPropertyValue(column.Label)) {
                labelHeader = column.Label;
            }
            else {                
                if (field !== null) {
                    labelHeader = field.Label;
                }
            }

            var style = " style=\"width:" + widths[c] + "px;\"";
            var metaData = "";
            var action = "";
            var cssClass = " class=\"";
            if (HasPropertyEnabled(column.Orderable)) {
                cssClass += "sort";
                metaData += " data-sortfield=\"" + column.DataProperty + "\"";

                if (field !== null) {
                    metaData += " data-sortType=\"" + field.Type + "\"";
                }

                metaData += " data-tableId=\"" + this.ItemName + "_" + this.ListId + "\"";
                action += " onclick=\"List_Sort(this);\"";
            }

            if (HasPropertyValue(column.ReplacedBy)) {
                metaData += " data-replacedby=\"" + column.ReplacedBy + "\"";
            }

            if (HasPropertyEnabled(column.Search)) {
                cssClass += " search";
            }

            searchColumns.push(c);
            cssClass += "\"";

            res += "<th id=\"th" + i + "\"";
            res += action;
            res += style;
            res += cssClass;
            res += metaData;
            res += "> " + labelHeader + "</th > ";
            i++;
        }

        if (listDefinition.EditAction === "ItemLink" || listDefinition.EditAction === "EditableAdd" || listDefinition.EditAction === "Popup" || listDefinition.EditAction === "Editable" || listDefinition.Exportable === true) {
            // Si no tiene permisos de eliminacion
            if (GrantCanWriteByItem(itemDefinition.ItemName)){
                res += "<th class=\"THActions\" data-buttons=\"" + actionButtonsCount + "\" style=\"width:" + (widths[widths.length - 1] + 18) + "px;border-right:none;\">";
                if (listDefinition.Exportable === true) {
                    res += "<div class=\"btn-group\">";
                    res += "    <button data-toggle=\"dropdown\" class=\"dropdown-toggle blue\" style=\"background-color:transparent;border:none;\">";
                    res += "        <span class=\"ace-icon fa fa-download icon-on-right\"></span>";
                    res += "    </button>";
                    res += "    <ul class=\"dropdown-menu dropdown-default\">";
                    res += "         <li><a href=\"javascript:ListDownload('PDF','" + itemDefinition.ItemName + "','" + listDefinition.Id + "');\">" + Dictionary.Common_Export_PDF + "</a></li>";
                    res += "         <li><a href=\"javascript:ListDownload('CSV','" + itemDefinition.ItemName + "','" + listDefinition.Id + "');\">" + Dictionary.Common_Export_CSV + "</a></li>";
                    res += "    </ul>";
                    res += " </div>";
                }
                res += "</th > ";
            }
            else  {
                res += "<th style=\"paddin:0;width:" + (widths[widths.length - 1] + 20) + "px;border-right:none;\"></th>";
            }
        } else {
            res += "<th style=\"padding:0;width:" + (widths[widths.length - 1] + 20) + "px;border-right:none;\"></th>";
        }

        res += "</tr>";

        // weke
        /*if (searchColumns.length > 0) {
            if (pageType === "list") { tabId = "0"; }
            var searchList = ListSearchFind(itemDefinition.ItemName, listDefinition.Id);
            if (searchList === null) {
                ListSearchAddList(itemDefinition.ItemName, listDefinition.Id, tabId);
            }

            if (pageType === "list") {
                ListSearchAddColumnIndex(itemDefinition.ItemName, listDefinition.Id, tabId);
            }
            else {
                ListSearchAddColumnIndex(itemDefinition.ItemName, listDefinition.Id, tabId);
            }
        }*/

        return res;
    }
    // --------------------- END RENDERING

    // --------------------- FILL DATA
    this.FillData = function () {
        this.FilteredData = this.FilteredData.filter(function (v) { return v.Active === true; });
        var idFunction = null;
        if (HasPropertyValue(this.ListDefinition.Template)) {
            var tpl = this.ListDefinition.Template;
            idFunction = (eval("typeof " + tpl + "=== \"function\";"));
        }

        console.log("Tools fill data", this.ComponentId, 1);
        console.log("widths", this.Widths, 1);
        $("#" + this.ComponentId + "_ListBody").html("");
        var total = 0;
        var innerHTML = "<!--- data -->";
        var grant = GrantsByItem(this.ItemDefinition.ItemName);
        var dataToShow = [];

        // Determinar si aparecen los hidden
        // 1º Sólo si existe la feature
        if (FeatureEnabled("Hidden", this.ItemDefinition)) {
            // 2º Sólo si hace falta descriminar
            if (this.ShowHiddenData === false) {
                for (var d = 0; d < this.FilteredData.length; d++) {
                    if (this.FilteredData[d].Unloaded === true) { continue; }
                    dataToShow.push(this.FilteredData[d]);
                }
            }
            else {
                dataToShow = this.FilteredData;
            }
        } else {
            dataToShow = this.FilteredData;
        }

        if (dataToShow.length > 0) {
            $("#ListDataTable_" + this.ComponentId).html(this.RenderColumns(this.ListDefinition));
        }

        var init = this.ListDefinition.PageSize > 0 ? (this.ListDefinition.PageSize * this.PageIndex) : 0;
        if (init > dataToShow.length) {
            init = Math.floor(dataToShow.length / this.ListDefinition.PageSize);
        }

        var end = this.ListDefinition.PageSize > 0 ? (this.ListDefinition.PageSize * (this.PageIndex + 1)) : dataToShow.length;
        if (end > dataToShow.length) {
            end = dataToShow.length;
        }
        for (var x = init; x < end; x++) {
            /*if (this.ListDefinition.PageSize > 0) {
                if (x < (this.ListDefinition.PageSize - 1) * this.PageIndex) {
                    continue;
                } else {
                    if (x < (this.ListDefinition.PageSize + 1) * this.PageIndex) {
                        continue;
                    }
                }
            }*/

            if (total <= ListRenderTopFirst) {
                if (idFunction === true) {
                    innerHTML += eval(tpl + "(dataToShow[x]);");
                }
                else {
                    var rowHtml = this.RenderRow(dataToShow[x], grant, this.ListDefinition, this.ItemDefinition);
                    innerHTML += rowHtml;
                }
                $("#ListLoading_Alumno_NoImagen").hide();
                total++;
            }
        }

        if (dataToShow.length > ListRenderTopFirst && this.ListDefinition.PageSize === 0) {
            setTimeout(FillDataAfter, 1000, this.ItemDefinition.ItemName, this.ListDefinition.Id, dataToShow, grant, this.ShowHiddenData, idFunction);
        }
        else {
            //console.log(this.ComponentId, "List completed!!!");
        }

        if (this.ListDefinition.PageSize > 0) {
            $("#ListPager_" + this.ComponentId).html(ListRenderPager(this.ComponentId, this.PageIndex, this.ListDefinition.PageSize, dataToShow.length));
        }
        else {
            $("#ListPager_" + this.ComponentId).html("");
        }

        $("#ListLoading_" + this.ComponentId).hide();
        if (total > 0) {
            $("#" + this.ComponentId + "_ListBody").html(innerHTML);
            $("#" + this.ComponentId + "_ListBody").show();
            $("#ListTable_" + this.ComponentId + "_NoData").hide();
        }
        else {
            $("#" + this.ComponentId + "_ListBody").hide();
            $("#ListTable_" + this.ComponentId + "_NoData").show();
        }

        if (total === this.Total || this.Total === 0) {
            $("#" + this.ComponentId + "_ListCount").html(total);
        }
        else {
            $("#" + this.ComponentId + "_ListCount").html(dataToShow.length + " de " + this.Total);
        }

        //TableRendered();

        var TableAfterFill = this.ComponentId + "_AfterFill";
        var afterFillCallback = eval("typeof " + TableAfterFill);
        if (afterFillCallback === "function") {
            eval(TableAfterFill + "();");
        }
        //else {
        //    console.log("List no callback", TableAfterFill);
        //}
    };

    //--------------- Calcular el número de botones por fila ---------------
    this.CalculateNumberOfButtons = function () {
        this.ActionsButtonsCount = 0;

        //if (this.ListDefinition.EditAction === "ItemLink" || this.ListDefinition.EditAction === "Popup" || this.ListDefinition.EditAction === "EditableAdd" || this.ListDefinition.EditAction === "Editable")
        if(1 === 1 || this.ListDefinition.EditAction === "FormPage")
        {
            var buttonEdit = true; // weke GrantCanWriteByItem(this.ItemName) || GrantCanReadByItem(this.ItemName);

            // No se puede borrar desde la lista, hay que acceder al elemento
            var buttonDelete = false;

            // En listas ItemLink, sólo se puede eliminar, para añadir hay que usar el combo y el botón añadir
            if (this.ListDefinition.EditAction === "ItemLink" && GrantCanWriteByItem(this.ItemName)) {
                buttonDelete = true;
                buttonEdit = false;
            }

            if (buttonEdit) {
                this.ActionsButtonsCount++;
            }

            if (buttonDelete) {
                this.ActionsButtonsCount++;
            }

            if (FeatureEnabled("Geolocation", this.ItemDefinition)) {
                this.ActionsButtonsCount++;
            }

            if (HasArrayValues(this.ListDefinition.Buttons)) {
                this.ActionsButtonsCount += this.ListDefinition.Buttons.length;
            }
        }
    };

    this.CalculateWidths = function () {
        this.CalculateNumberOfButtons();
        var tableName = "ListDataDiv_" + this.ItemName + "_" + this.ListId;
        var itemDefinition = this.ItemDefinition;
        var listDefinition = this.ListDefinition;
        var width = 0;

        /* weke? if ($("#tabHeader").length !== 0) {
            width = $("#tabHeader").width() -17;
        }
        else {
            width = $("#MainContainer").width() - 7;
        }*/

        console.log(tableName, width, 1);
        var widths = [];
        var usedWidth = 0;
        var noWidth = 0;

        if (FeatureEnabled("Unloadable", itemDefinition)) {
            usedWidth += 30;
        }
        //------------------
        for (var c = 0; c < listDefinition.Columns.length; c++) {
            var column = listDefinition.Columns[c];
            if (column.DataProperty === "Id") { continue; }

            if (typeof column.HiddenList !== "undefined" && column.HiddenList !== null && column.HiddenList === true) {
                continue;
            }

            var itemField = GetFieldDefinition(column.DataProperty, itemDefinition);

            if (typeof column.Width !== "undefined" && column.Width !== null && column.Width > 0) {
                widths.push(column.Width);
                usedWidth += column.Width;
            } else {
                if (itemField !== null && itemField.Type === "datetime") {
                    widths.push(100);
                    usedWidth += 100;
                }
                else {
                    widths.push(-1);
                    noWidth++;
                }
            }
        }

        this.Widths = widths;

        // Añadir width de actions
        this.Widths.push((30 * this.ActionsButtonsCount));
        this.DefaultColumnWidth = (width - usedWidth - (47 * this.ActionsButtonsCount));

        for (var x = 0; x < this.Widths.length -1; x++) {
            if (this.Widths[x] < 1) {
                this.Widths[x] = this.DefaultColumnWidth;
            }
        }

        this.Widths[this.Widths.length - 1] = (25 * this.ActionsButtonsCount) + 16;
        //console.log("Widths", this.Widths);
    };

    this.RenderColumns = function (listDefinition) {
        var res = "<colgroup>";
        var resdebug = "<tr style=\"display:_;font-size:8px;background-color:#ccc;color:#333;\">";
        var i = 0;

        if (FeatureEnabled("Unloadable", this.ItemDefinition)) {
            i++;
            res += "<col id=\"tcol0\" style=\"width:30px;\" />";
            resdebug += "<td style=\"width:30px;\">30</td>";
        }

        for (var w = 0; w < this.Widths.length; w++) {
            var style = " style=\"";
            var colwidth = this.Widths[w];
            if (this.Widths[w] < 0) {
                style += "width:" + this.DefaultColumnWidth + "px;";
                colwidth = this.DefaultColumnWidth;
            }
            else {
                style += "width:" + this.Widths[w] + "px;";
            }

            style += "\"";

            res += "<col id=\"tcol" + i + "\"";
            res += style;
            res += " />";
            i++;

            resdebug += "<td" + style + ">" + colwidth + "</td>";
        }

        if (listDefinition.EditAction === 2 || listDefinition.EditAction === 3 || listDefinition.EditAction === 4 || listDefinition.Exportable === true) {
        //    res += "<col style=\"width:107px;\" />";
        //} else {
            res += "<col style=\"width:90px;\" />";
        }

        //------------------

        res += "</colgroup>";
        resdebug += "<tr>";
        return res;// + resdebug;
    };

    this.RenderRow = function (data, grant, listDefinition, itemDefinition) {
        var res = "<tr id=\"" + data.Id + "\"";
        if (HasPropertyValue(listDefinition.CssClass)) {
            var customClass = "";
            eval("customClass = " + listDefinition.CssClass + "(" + JSON.stringify(data) + ");");
            res += " " + customClass;
        }

        if (listDefinition.EditAction === 7) {
            res += " onclick=\"SelectMasterDetail(this);\"";
        }

        res += ">";
        var tdData = "";

        var c = 0;
        if (FeatureEnabled("Unloadable", itemDefinition)) {
            res += "<td style=\"width:30px;\"><i class=\"fa fa-circle ";
            if (data.Unloaded === true) { res += "red"; }
            else { res += "green"; }
            res += "\"></i></td>";
            c = 1;
        }

        for (var x = 0; x < listDefinition.Columns.length; x++) {
            var column = listDefinition.Columns[x];
            if (column.DataProperty !== "Id") {
                var field = GetFieldDefinition(column.DataProperty, itemDefinition);
                if (typeof column.HiddenList === "undefined" || column.HiddenList === null || column.HiddenList === false) {
                    tdData += RenderRowCell(column, field, data, itemDefinition, listDefinition.Id, listDefinition.FormId, listDefinition.EditAction, c, this.Widths[x]);
                }
                c++;
            }
        }

        var tdActionsButtons = "";
        if (listDefinition.EditAction === "InLine" || listDefinition.EditAction === "ItemLink" || listDefinition.EditAction === "Popup" || listDefinition.EditAction === "EditableAdd" || listDefinition.EditAction === "Editable" || listDefinition.EditAction === "OnlyView" || listDefinition.EditAction === "FormPage") {

            var buttonEdit = GrantCanReadByItem(itemDefinition.ItemName);
            var buttonDelete = GrantCanDeleteByItem(itemDefinition.ItemName);

            // En listas ItemLink, sólo se puede eliminar, para añadir hay que usar el combo y el botón añadir
            if (listDefinition.EditAction === "ItemLink" && GrantCanWriteByItem(itemDefinition.ItemName)) {
                buttonDelete = true;
                buttonEdit = false;
            } else if(listDefinition.EditAction === "InLine" && GrantCanWriteByItem(itemDefinition.ItemName)) {
                buttonDelete = true;
                buttonEdit = true;
            } else if (listDefinition.EditAction === "OnlyView") {
                buttonDelete = false;
                buttonEdit = true;
            }

            // Desde una lista principal no se eliminar, se obliga a entrar en la ficha
            if (PageType === "PageList") {
                buttonDelete = false;
            }

            //GoEncryptedView(itemName, listId, itemId, formId, params)
            var editAction = "GoEncryptedView('" + itemDefinition.ItemName + "', '" + this.ListDefinition.Id + "', " + data.Id + ",'" + this.ListDefinition.FormId + "', null)";
            if (listDefinition.EditAction === "Popup") {
                //editAction = "PopupItem('" + itemDefinition.Id + "', '" + listDefinition.Id + "', this.id);";
                editAction = "PopupItem({'ItemName': '" + this.ItemDefinition.ItemName + "','ListId': '" + this.ListDefinition.Id + "','ItemId': " + data.Id+"});"
            }

            if (buttonEdit) {
                if (grant.Grants.indexOf("W") !== -1) {
                    tdActionsButtons += ListButtonRow(data.Id, "fal fa-pencil-alt", "blue", editAction, Dictionary.Common_Edit);
                }
                else {
                    if (grant.Grants.indexOf("R") !== -1) {
                        tdActionsButtons += ListButtonRow(data.Id, "fal fa-eye", "blue", editAction);
                    }
                }
            }

            if (buttonDelete) {
                if (data.Active === false) {
                    var recycleAction = "ActivateItem('" + itemDefinition.Id + "','" + listDefinition.Id + "', this.id, 'list');";
                    tdActionsButtons += ListButtonRow(data.Id, "fal fa-recycle", "green", recycleAction);
                }
                else {
                    var deleteAction = "DeleteItem('" + itemDefinition.Id + "', '" + listDefinition.Id + "', this.id, 'list');";
                    tdActionsButtons += ListButtonRow(data.Id, "fal fa-times", "red", deleteAction, Dictionary.Common_Delete);
                }
            }

            if (FeatureEnabled("Geolocation", itemDefinition)) {
                //if (typeof data["Latitude"] !== "undefined" && typeof data["Longitude"] !== "undefined" && data["Latitude"] !== null && data["Longitude"] !== null) {
                var mapAction = "MapPopupShowFromList(this);";
                tdActionsButtons += ListButtonRow(itemDefinition.ItemName + "_" + listDefinition.Id + "_Map_" + data["Id"], "fas fa-map-marker-alt", "green", mapAction);
                //tdActionsButtons += "    <a class=\"green ace-icon fal fa-map-marker-alt bigger-120\" id =\"" + itemDefinition.ItemName + "_" + listDefinition.Id + "_Map_" + data["Id"] + "\" onclick=\"MapPopupShowFromList(this);\"></a>";
                //}
            }

            if (HasArrayValues(listDefinition.Buttons)) {
                for (var b = 0; b < listDefinition.Buttons.length; b++) {
                    var button = listDefinition.Buttons[b];
                    var icon = "fa fa-gear";
                    var color = "";

                    if (HasPropertyValue(button.Icon)) {
                        icon = button.Icon;
                    }

                    if (HasPropertyValue(button.Color)) {
                        color = " style=\"color:" + button.Color + ";\"";
                    }

                    tdActionsButtons += ListButtonRow(data.Id, icon, color, button.Function, "", button.Name);
                    //tdActionsButtons += "    <a id=\"BtnCustomAction-" + button.Name + "-" + data.Id + "\" class=\"customAction actionBtn-" + button.Name + " ace-icon " + icon + " bigger-120\" id =\"" + button.Function + "_" + data.Id + "\" onclick=\"" + button.Function + "(this);\"" + color + "></a>";
                    //tdActionsButtons += "    <a id=\"BtnCustomAction-" + button.Name + "-" + data.Id + "\" class=\"customAction actionBtn-" + button.Name + " ace-icon " + icon + " bigger-120\" id =\"" + button.Function + "_" + data.Id + "\" onclick=\"" + button.Function + "(this);\"" + color + "></a>";
                }
            }

            var tdActions = "<td class=\"action-buttons\" data-buttons=\"" + this.ActionsButtonsCount + "\" style=\"width:" + (this.ActionsButtonsCount * 25 + 16) + "px;white-space:nowrap;\">";
            tdActions += tdActionsButtons;
            tdActions += "</td>";
        }

        res += tdData;
        if (listDefinition.EditAction !== "Readonly") {
            res += tdActions;
        }

        res += "</tr>";
        return res;
    };

    function RenderRowCell(column, field, rowData, itemDefinition, listDefinitionId, targetForm, edition, columnIndex, columnWidth) {
        if (columnWidth < 1) {
            columnWidth = this.DefaultColumnWidth;
        }

        var res = "";
        if (field !== null) {
            switch (field.Type) {
                case "boolean":
                case "date":
                case "datetime":
                    column.Align = column.Align || "center";
                    if (columnWidth < 1) {
                        columnWidth = 90;
                    }

                    break;
                case "int":
                case "integer":
                case "decimal":
                case "money":
                    column.Align = column.Align || "right";
                    break;
            }
        }

        var style = " style=\"width:" + columnWidth + "px;";
        if (HasPropertyValue(column.Align)) { style += "text-align:" + column.Align + ";"; }
        style += "\"";

        var editAction = "GoEncryptedView('" + this.ItemDefinition.ItemName + "', '" + this.ListId + "', " + rowData.Id + ",'" + this.ListDefinition.FormId + "', null)\"";
        if (edition === "Popup") {
            editAction = "PopupItem('" + itemDefinition.ItemName + "', '" + listDefinitionId + "', this.id);";
        }

        var dataKeyName = column.DataProperty;
        if (HasPropertyValue(column.ReplacedBy)) { dataKeyName = column.ReplacedBy; }
        var cellData = rowData[dataKeyName];

        var searchData = "";
        var dataOrderData = "";
        var textData = cellData;

        if (typeof column.RenderData !== "undefined" && column.RenderData !== null && column.RenderData !== "") {
            var data = null;
            if (typeof rowData[dataKeyName] === "object") {
                data = JSON.stringify(rowData[dataKeyName]);
                searchData = rowData[dataKeyName] === null ? "" : rowData[dataKeyName].Value;
            } else if (typeof rowData[dataKeyName] === "string") {
                data = "'" + rowData[dataKeyName] + "'";
                searchData = rowData[dataKeyName] === null ? "" : rowData[dataKeyName];
            } else if (typeof rowData[dataKeyName] !== "undefined") {
                data = rowData[dataKeyName];
                searchData = rowData[dataKeyName] === null ? "" : rowData[dataKeyName];
            }

            textData = eval(column.RenderData + "(" + data + "," + JSON.stringify(rowData) + ");");
            searchData = textData;
        }

        if (typeof column.Linkable !== "undefined" && column.Linkable !== null && column.Linkable === true) {
            var itemName = "";

            if (field !== null && !IsFK(field.Name, itemDefinition)) {
                cellData = "<a id=\"" + rowData.Id + "\" onclick=\"" + editAction + "\" style=\"cursor:pointer;\">";
                cellData += textData;
                cellData += "</a>";
                //searchData = rowData[dataKeyName];
            }
            else if (field.Type === "FixedList") {
                cellData = rowData[dataKeyName];
                if (HasPropertyValue(field.FixedListName) === true) {
                    if (typeof rowData[dataKeyName] !== "undefined" && rowData[dataKeyName] !== null) {
                        cellData = FixedLists[field.FixedListName][rowData[dataKeyName]];
                    }
                }

                searchData = cellData;
            }
            else if (typeof rowData[dataKeyName] === "object") {
                itemName = column.DataProperty.substr(0, column.DataProperty.length - 2);
                if (HasPropertyValue(column.LinkableItem)) { itemName = column.LinkableItem; }

                if (rowData[dataKeyName] === null) {
                    cellData = "";
                    searchData = "";
                }
                else {
                    //searchData = rowData[dataKeyName];
                    searchData = textData;
                    if (GrantCanReadByItem(itemName) !== true) {
                        cellData = rowData[dataKeyName]["Value"];
                    }
                    else {
                        // Se tiene que hacer link a un item distinto
                        var editFKAction = "GoItemView('" + itemName + "', " + rowData[dataKeyName]["Id"] + ", 'Custom', [])";
                        cellData = "<a id=\"" + rowData.Id + "\" onclick =\"" + editFKAction + "\">";
                        cellData += rowData[dataKeyName]["Value"];
                        //cellData = textData;
                        cellData += "</a>";
                    }
                }
            }
            else {
                itemName = column.DataProperty.substr(0, column.DataProperty.length - 2);
                //searchData = rowData[dataKeyName];
                searchData = textData;
                if (GrantCanReadByItem(itemName) !== true) {
                    //cellData += rowData[dataKeyName]["Value"];
                    cellData = textData;
                }
                else {
                    cellData = "<a onclick=\"GoItemView('" + itemName + "', " + rowData[dataKeyName]["Id"] + ", '" + targetForm + "', [])\">";
                    //cellData += rowData[dataKeyName]["Value"];
                    cellData = textData;
                    cellData += "</a>";
                }
            }
        }
        else {
            if (typeof column.Render !== "undefined" && column.Render !== null && column.Render !== "") {
                if (typeof cellData === "object") {
                    cellData = eval(column.Render + "(" + JSON.stringify(rowData[dataKeyName]) + "," + rowData.Id + "," + JSON.stringify(rowData) + ");");
                }
                else {
                    cellData = eval(column.Render + "('" + rowData[dataKeyName] + "'," + rowData.Id + "," + JSON.stringify(rowData) + ");");
                }


                //searchData = rowData[dataKeyName];
                searchData = cellData;
            }
            else if (typeof column.RenderData !== "undefined" && column.RenderData !== null && column.RenderData !== "") {
                var data = null;
                if (typeof rowData[dataKeyName] === "object") {
                    data = JSON.stringify(rowData[dataKeyName]);
                    searchData = rowData[dataKeyName] === null ? "" : rowData[dataKeyName].Value;
                } else if (typeof rowData[dataKeyName] === "string") {
                    data = "'" + rowData[dataKeyName] + "'";
                    searchData = rowData[dataKeyName] === null ? "" : rowData[dataKeyName];
                } else if (typeof rowData[dataKeyName] !== "undefined") {
                    data = rowData[dataKeyName];
                    searchData = rowData[dataKeyName] === null ? "" : rowData[dataKeyName];
                }

                cellData = eval(column.RenderData + "(" + data + "," + JSON.stringify(rowData) + ");");
                searchData = eval(column.RenderData + "(" + data + "," + JSON.stringify(rowData) + ");");
            }
            else if (field === null) {
                if (typeof rowData[column.ReplacedBy] === 'object') {
                    cellData = rowData[column.ReplacedBy].Description;
                }
                else {
                    cellData = rowData[column.ReplacedBy];
                }
            }
            else if (field.Type.toLowerCase() === "fixedlist") {
                cellData = rowData[dataKeyName];
                if (HasPropertyValue(field.FixedListName) === true) {
                    if (typeof rowData[dataKeyName] !== "undefined" && rowData[dataKeyName] !== null) {
                        cellData = FixedLists[field.FixedListName][rowData[dataKeyName]];
                    }
                }

                searchData = cellData;
            }
            else if (field !== null && typeof field.DataFormat !== "undefined" && field.DataFormat !== null) {
                try {
                    if (field.Type === "datetime") {
                        cellData = eval(field.DataFormat.Name + "('" + rowData[dataKeyName] + "')");
                    }
                    else {
                        cellData = eval(field.DataFormat.Name + "(" + rowData[dataKeyName] + ")");
                    }
                    searchData = rowData[dataKeyName];
                }
                catch (e) {
                    cellData = rowData[dataKeyName];
                    searchData = rowData[dataKeyName];
                }
            } else if (field.Type.toLowerCase() === "money") {
                cellData = ToMoneyFormat(rowData[dataKeyName]);
            }
            else if (field.Type.toLowerCase() === "fixedlistmultiple") {
                var dataValue = rowData[dataKeyName];
                var listName = field.FixedListName;

                var binary = (dataValue >>> 0).toString(2);
                var test = "";
                var first = true;
                for (var b = binary.length - 1; b >= 0; b--) {
                    if (binary[b] === '1') {
                        test += first ? "" : ", ";
                        test += FixedLists[listName][binary.length - 1 - b];
                        first = false;
                    }
                }

                cellData = test;

            }
            else {
                if (typeof cellData === "object" && cellData !== null) {
                    if (rowData[column.DataProperty] === null) {
                        cellData = null;
                        searchData = null;
                    }
                    else {
                        cellData = rowData[dataKeyName]["Value"];
                        searchData = rowData[dataKeyName]["Value"];
                    }
                }
                else {
                    searchData = data;
                }
            }
        }

        if (cellData === null) { cellData = ""; }

        if (HasPropertyEnabled(column.Search) || HasPropertyEnabled(column.Filter)) {
            ListSearchAddItem(itemDefinition.ItemName, listDefinitionId, searchData);
        }

		var tdTitle = "";
		if(typeof searchData !== "undefined" && searchData !== null && searchData !== "") {
            tdTitle = "title=\"" + searchData.toString().split('<br />').join('\n');
            dataOrderData = " data-orderData=\"" + searchData + "\"";
		}

        res += "<td" + style + " data-order=\"" + columnIndex + "\"" + dataOrderData + "><div class=\"truncate\" style=\"width:" + (columnWidth - 20) + "px;\"" + tdTitle + "\">";
        res += cellData;
        res += "</div></td>";
        return res;
    }
    // --------------------- END FILL DATA

    // --------------------- GET DATA
    this.GetData = function () {
        ListSetLoading(this.ComponentId);
        if (HasPropertyValue(this.CustomAjaxSource)) {
            if (this.CustomAjaxSource === "NoData") {
                data = { "data": [], "ItemName": this.ItemName, "ListId": this.ListId };
                GetDataCallBack(data);
            } else {
                this.GetDataFromCustomAjaxSource();
            }
        }
        else {
            this.GetDataFromListDefinition();
        }
    };

    this.GetDataFromCustomAjaxSource = function () {
        console.log("GetDataFromCustomAjaxSource", this.CustomAjaxSource, 1);
        var SendParameters = [];
        if (HasArrayValues(this.Parameters)) {
            var value = "";
            for (var p = 0; p < this.Parameters.length; p++) {
                if (this.Parameters[p].Value.toLowerCase() === "#actualitemId#") {
                    SendParameters.push({ "Name": this.Parameters[p].Name, "Value": ItemData.ActualData.Id, "Type": "long" });
                }
                if (this.Parameters[p].Value.toLowerCase() === "#applicationuser#") {
                    SendParameters.push({ "Name": this.Parameters[p].Name, "Value": ApplicationUser.Id, "Type": "long" });
                }
                else if (this.Parameters[p].Value.charAt(0) === "@") {
                    value = $("#" + this.Parameters[p].Value.substr(1, this.Parameters[p].Value.length - 1)).val();
                    if (typeof value === "undefined") {
                        value = "null";
                    }

                    SendParameters.push({ "Name": this.Parameters[p].Name, "Value": value, "Type": this.Parameters[p].Type });
                }
                else {
                    SendParameters.push(this.Parameters[p]);
                }
            }
        }

        var data = {
            "itemName": this.ItemName,
            "listDefinitionId": this.ListId,
            "parametersList": JSON.stringify(SendParameters),
            "companyId": Company.Id,
            "instanceName": Instance.Name
        }

        $.ajax({
            "type": "POST",
            "url": "/Async/ItemService.asmx/GetListCustomAjaxSource",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (msg) {
                var data = null;
                eval("data = " + msg.d + ";");
                console.log("TableFillData", data);
                GetDataCallBack(data);
            },
            "error": function (msg) {
                PopupWarning(msg.responseText);
            }
        });

        /*var url = "/Instances/" + Instance.Name + "/Data/ItemDataBase.aspx?I=" + Instance.Name + "&C=" + Company.Id + "&Action=" + this.CustomAjaxSource + "&listId=" + this.ListDefinition.Id + "&ItemName=" + this.ItemDefinition.ItemName + params;
        $.getJSON(url,
            function (data) {
                //console.time("TableFillData" + data.ListId, data.ItemName + " --> " + data.ListId);
                GetDataCallBack(data);
                //console.timeEnd("TableFillData" + data.ListId, data.ItemName + " --> " + data.ListId);
            }).error(function (e) {
                if (e.readyState === 4 && e.status === 200) {
                    //console.log(eval(e.responseText));
                    var data = eval(e.responseText);
                    GetDataCallBack(data);
                }
            });*/
    };

    this.GetFinalParameters = function () {
        var finalParameters = "";
        if (this.ListDefinition !== null) {
            if (typeof this.ListDefinition.Parameters !== "undefined" && this.ListDefinition.Parameters !== null) {
                for (var x = 0; x < this.ListDefinition.Parameters.length; x++) {
                    var value = this.ListDefinition.Parameters[x].Value;
                    if (value === "#actualItemId#") {
                        value = ItemData.OriginalItemData.Id;
                    }

                    finalParameters += this.ListDefinition.Parameters[x].Name + "^" + value + "|";
                }
            }
        }

        return finalParameters;
    }

    this.GetDataFromListDefinition = function () {
		// console.log(this.ItemDefinition.ItemName, new Date());
        /*var finalParameters = "";
        if (this.ListDefinition !== null) {
            if (typeof this.ListDefinition.Parameters !== "undefined" && this.ListDefinition.Parameters !== null) {
                for (var x = 0; x < this.ListDefinition.Parameters.length; x++) {
                    var value = this.ListDefinition.Parameters[x].Value;
                    if (value === "#actualItemId#") {
                        value = itemData.Id;
                    }

                    finalParameters += this.ListDefinition.Parameters[x].Name + "^" + value + "|";
                }
            }
        }*/

        var data = {
            "itemName": this.ItemDefinition.ItemName,
            "listDefinitionId": this.ListDefinition.Id,
            "parametersList": this.GetFinalParameters(),
            "companyId": Company.Id,
            "instanceName": Instance.Name
        };

        GetDataFromListDefinitionStandalone(data);

        /*$.ajax({
            "type": "POST",
            "url": "/Async/ItemService.asmx/GetList",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(data, null, 2),
            "success": function (msg) {
                var data = null;
                eval("data = " + msg.d + ";");
                if(debugConsole === true) { console.time("TableFillData", data.ItemName + " --> " + data.ListId); }
                GetDataCallBack(data);
            },
            "error": function (msg) {
                PopupWarning(msg.responseText);
            }
        });*/
    };

    this.ScopeList = function (data) {
        // weke
        return data;

        var scope = GetScopeByItem(ApplicationUser, this.ItemDefinition.Id);

        var res = [];
        for (var s = 0; s < data.length; s++) {
            if (scope !== null) {
                if ($.inArray(data[s].Id, scope) !== -1) {
                    res.push(data[s]);
                }
            }
            else {
                res.push(data[s]);
            }
        }

        // Revisar scopes de FK
        if (typeof this.ItemDefinition.ForeignValues !== "undefined" && this.ItemDefinition.ForeignValues.length > 0) {
            var temp = [];
            for (var f = 0; f < this.ItemDefinition.ForeignValues.length; f++) {
                var fkItemName = this.ItemDefinition.ForeignValues[f].ItemName;
                var fkItemDefinition = GetItemDefinitionByName(fkItemName);
                if (FeatureEnabled("ScopeView", fkItemDefinition)) {
                    temp = ScopeByListItem(this.ListDefinition, res, ApplicationUser, fkItemDefinition.Id);
                }
                else {
                    temp = res;
                }
            }

            res = temp;
        }

        return res;
    };
    // --------------------- END GET DATA
}

function ScopeByListItem(listDefinition, listData, ApplicationUser, fkItemDefinitionId) {
    var temp = [];
    var scopeFK = GetScopeByItem(ApplicationUser, fkItemDefinitionId);
    var scopedItemDefinition = GetItemDefinitionById(fkItemDefinitionId);

    if (scopedItemDefinition === null) { return listData; }

    var scopedColumn = null;
    for (var c = 0; c < listDefinition.Columns.length; c++) {
        if (listDefinition.Columns[c].DataProperty === scopedItemDefinition.ItemName + "Id") {
            scopedColumn = listDefinition.Columns[c].DataProperty;
            if (HasPropertyValue(listDefinition.Columns[c].ReplacedBy) === true) {
                scopedColumn = listDefinition.Columns[c].ReplacedBy;
            }

            break;
        }
    }

    if (scopedColumn === null) { return listData; }

    for (var s2 = 0; s2 < listData.length; s2++) {
        var itemExamined = listData[s2];
        if (scopeFK !== null) {
            if (typeof itemExamined[scopedColumn] !== "undefined") {
                if ($.inArray(itemExamined[scopedColumn].Id, scopeFK) !== -1) {
                    temp.push(itemExamined);
                }
            }
        }
        else {
            temp.push(itemExamined);
        }
    }

    return temp;
}

function ListPageOnListContext(listId, itemName) {
    if (typeof listSources !== "undefined" && listSources !== null && listSources.length > 0) {
        for (var x = 0; x < listSources.length; x++) {
            if (HasPropertyValue(listSources[x].ItemDefinition) && HasPropertyValue(listSources[x].ListDefinition)) {
                if (listSources[x].ItemDefinition.ItemName === itemName && listSources[x].ListDefinition.Id === listId) {
                    return listSources[x];
                }
            }
        }
    }

    return null;
}

function ListPageOnListContextIndex(listId, itemName) {
    if (typeof ListSources !== "undefined" && ListSources !== null && ListSources.length > 0) {
        for (var x = 0; x < ListSources.length; x++) {
            if (HasPropertyValue(ListSources[x].ItemDefinition) && HasPropertyValue(ListSources[x].ListDefinition)) {
                if (ListSources[x].ItemDefinition.ItemName === itemName && ListSources[x].ListDefinition.Id === listId) {
                    return x;
                }
            }
        }
    }

    return null;
}

function ListById(listId, itemName) {
    for (var l = 0; l < ListSources.length; l++) {
        var list = ListSources[l];

        if (HasPropertyValue(itemName)) {
            if (list.ListId === listId && list.ItemName === itemName) {
                return ListSources[l];
            }
        }
        else {
            if (list.ListId === listId) {
                return ListSources[l];
            }
        }
    }

    return null;
}

function ListDataById(listId, itemName, itemId) {
    listId = listId.toLowerCase();
    itemName = itemName.toLowerCase();
    var id = itemId * 1;
    for (var l = 0; l < ListSources.length; l++) {
        var list = ListSources[l];
        if (list.ListId.toLowerCase() === listId && list.ItemDefinition.ItemName.toLowerCase() === itemName) {
            for (var d = 0; d < list.Data.length; d++) {
                if (list.Data[d].Id === id) {
                    return list.Data[d];
                }
            }

            return null;
        }
    }

    return null;
}

function RedrawList(sender, listId, itemName) {
    var list = ListById(listId, itemName);
    list.ShowHiddenData = sender.checked;
    if (list !== null) {
        list.FillData();
    }
}

function FillDataAfter(itemName, listId, data, grant, hiddenData, idFunction) {
    var innerHTML = "";
    var list = ListById(listId, itemName);
    var tpl = null;
    if (idFunction === true) {
        tpl = list.ListDefinition.Template;
    }

    for (var x = ListRenderTopFirst + 1; x < data.length; x++) {
        var show = true;
        if (show === true) {
            if (idFunction === true) {
                innerHTML += eval(tpl + "(data[x]);");
            }
            else {
                var rowHtml = list.RenderRow(data[x], grant, list.ListDefinition, list.ItemDefinition);
                innerHTML += rowHtml;
            }

            $("#ListLoading_Alumno_NoImagen").hide();
        }
    }

    $("#ListDataTable_" + list.ItemName + "_" + list.ListId).append(innerHTML);
}

function ListSort(sender) {
    var thId = sender.target.id;
    var tableDivName = sender.target.parentNode.parentNode.parentNode.parentNode.id;
    var itemName = tableDivName.split("_")[1];
    var itemDefinition = GetItemDefinitionByName(itemName);
    var listId = tableDivName.split("_")[2];
    //console.log(itemName, listId);

    var sortAscendant = sender.target.className.indexOf(" ASC") === -1;
    var dataProperty = $("#" + tableDivName + " #" + thId).data("sortfield");
    var replacedBy = $("#" + tableDivName + " #" + thId).data("replacedby");
    if (typeof replacedBy === "undefined") {
        replacedBy = dataProperty;
    }

    //if (debugConsole === true) { console.log(replacedBy); }

    var field = GetFieldDefinition(dataProperty, itemDefinition);
    if (debugConsole === true) {
        //console.log(thId, sortAscendant + " " + dataProperty);
        //console.log(field);
    }

    ListSetHeaderSorting(tableDivName, thId, sortAscendant);

    var sortingList = ListById(listId, itemName);

    if (field.FK === true) {
        sortingList.FilteredData.sort(sort_by(replacedBy, sortAscendant, function (a) {
            if (typeof a === "string") { return a.toLocaleUpperCase(); }
            if (typeof a === "number") { return a * 1; }
            //return (typeof a === "undefined" || a === null ? "" : a.Value).toUpperCase();
           return (typeof a === "undefined" || a === null ? "" : (isNaN(a.Value) ? a.Value.toUpperCase() : a.Value*1));
        }));
    }
    else if (field.Type === "datetime") {
        sortingList.FilteredData.sort(sort_by(replacedBy, sortAscendant, function (a) {
            var value = a === null ? "" : DateTextToYYYYMMDD(a, "/");
            if (debugConsole === true) { console.log(a, value); }
            return value;
        }));
    }
    else if (field.Type === "int" || field.Type === "long" || field.Type === "float" || field.Type === "decimal" || field.Type === "money") {
        sortingList.FilteredData.sort(sort_by(replacedBy, sortAscendant, function (a) {
            if (debugConsole === true) { console.log(a); }
            return (a === null ? 0 : a) * 1;
        }));
    }
    else {
        sortingList.FilteredData.sort(sort_by(replacedBy, sortAscendant, function (a) {
            if (debugConsole === true) { console.log(a); }
            return (a === null ? "" : a.toString()).toUpperCase();
        }));
    }

    if (debugConsole === true) { console.log(sortingList.FilteredData); }
    sortingList.FillData();
}

function GetDataCallBack(data) {
    data.data = data.data.filter(function (v) { return v.Active === true; });
    var index = ListPageOnListContextIndex(data.ListId, data.ItemName);
    ListSources[index].Data = ListSources[index].ScopeList(data.data);
    ListSources[index].FilteredData = ListSources[index].Data;
    ListSources[index].Total = ListSources[index].Data.length;
    if (PageType === "PageList") {
        ListSources[index].Data = data.data;
        ListSources[index].Total = data.data.length;
    }

    if (ListSources[index] !== null) {
        ListSources[index].FillData();
    }

    var customCallbackName = data.ItemName + "_" + data.ListId + "_Callback";
    console.log("List callback get", customCallbackName);
    if (eval("typeof " + customCallbackName) === "function") {
        eval(customCallbackName + "(data);");
    }

    List_SortGo("Sort_" + ListSources[index].ItemName + "_" + ListSources[index].ListId);
}

function SelectMasterDetail(sender) {
    $("tr").removeClass("rowMasterDetailSelected");
    $(sender).addClass("rowMasterDetailSelected");
    ItemDataJson_Context = {
        "ItemDefinition": itemDefinition,
        "ItemId": sender.id,
        "Callback": SelectMasterDetailCallback,
        "Data": null
    };

    GetItemDataJson();
}

function SelectMasterDetailCallback() {
    FillFormDetail(ItemDataJson_Context.ItemDefinition, ItemDataJson_Context.Data);
}

function SetFilterListSession(itemName, listId, configuration) {
    var data = {
        "itemName": itemName,
        "listId": listId,
        "configuration": configuration
    };
    $.ajax({
        "type": "POST",
        "url": "/Async/FilterServices.asmx/SetFilterList",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function () {
        },
        "error": function (msg) {
            console.log(msg.responseText);
        }
    });
}

function GetFilterListSession(itemName, listId) {
    var data = {
        "itemName": itemName,
        "listId": listId
    };
    $.ajax({
        "type": "POST",
        "url": "/Async/FilterServices.asmx/GetFilterList",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            //console.log(msg);
        },
        "error": function (msg) {
            console.log(msg.responseText);
        }
    });
}

function GetFilter(columns) {
    var res = [];
    for (var x = 0; x < columns.length; x++) {
        if (HasPropertyEnabled(columns[x].Search)) {
            if (HasPropertyValue(columns[x].ReplacedBy)) {
                res.push(columns[x].ReplacedBy);
            }
            else {
                res.push(columns[x].DataProperty);
            }
        }
    }

    return res;
}

function RowTemplate(list, grant) {
    var itemDefinition = list.ItemDefinition;
    var listDefinition = list.ListDefinition;

    var res = "<tr id=\"#Id#\"";
    if (HasPropertyValue(listDefinition.CssClass)) {
        var customClass = "";
        eval("customClass = " + listDefinition.CssClass + "();");
        res += " " + customClass;
    }

    if (listDefinition.EditAction === 7) {
        res += " onclick=\"SelectMasterDetail(this);\"";
    }

    res += ">";
    var tdData = "";

    var c = 0;
    if (FeatureEnabled("Unloadable", itemDefinition)) {
        res += "<td style=\"width:30px;\"><i class=\"fa fa-circle #Unloaded#\"></i></td>";
        c = 1;
    }

    for (var x = 0; x < listDefinition.Columns.length; x++) {
        var column = listDefinition.Columns[x];
        if (column.DataProperty !== "Id") {
            var field = GetFieldDefinition(column.DataProperty, itemDefinition);
            if (typeof column.HiddenList === "undefined" || column.HiddenList === null || column.HiddenList === false) {
                tdData += RowCellTemplate(column, field,  itemDefinition, listDefinition.Id, listDefinition.FormId, listDefinition.EditAction, c, list.Widths[x]);
            }

            c++;
        }
    }

    var tdActions = "<td class=\"action-buttons\" style=\"width:90px;white-space:nowrap;\">";
    if (listDefinition.EditAction === ListEditAction.Popup || listDefinition.EditAction === 3 || listDefinition.EditAction === 4) {

        var editAction = "GoItemView('" + itemDefinition.ItemName + "', this.id)";
        if (listDefinition.EditAction === ListEditAction.Popup || listDefinition.EditAction === 4) {
            editAction = "PopupItem('" + itemDefinition.ItemName + "', '" + listDefinition.Id + "', this.id);";
        }

        if (grant.Grants.indexOf("W") !== -1) {
            tdActions += "    <a class=\"blue ace-icon fal fa-pencil-alt bigger-120\" id=\"#Id#\" onclick=\"" + editAction + "\"></a>";
        }
        else {
            if (grant.Grants.indexOf("R") !== -1) {
                tdActions += "    <a class=\"blue ace-icon fal fa-eye bigger-120\" id=\"#Id#\" onclick=\"" + editAction + "\"></a>";
            }
        }

        /*if (grant.Grants.indexOf("D") !== -1 && data.Active === false) {
            tdActions += "    <a class=\"green ace-icon fal fa-recycle bigger-120\" id =\"#Id#\" onclick=\"ActivateItem('" + itemDefinition.ItemName + "','" + listDefinition.Id + "', this.id, 'list');\"></a>";
        }
        else if (grant.Grants.indexOf("D") !== -1) {
            tdActions += "    <a class=\"red ace-icon fal fa-times bigger-120\" id =\"#Id#\" onclick=\"DeleteItem('" + itemDefinition.ItemName + "','" + listDefinition.Id + "', this.id, 'list');\"></a>";
        }*/

        if (FeatureEnabled("Geolocation", itemDefinition)) {
            //if (typeof data["Latitude"] !== "undefined" && typeof data["Longitude"] !== "undefined" && data["Latitude"] !== null && data["Longitude"] !== null) {
            tdActions += "    <a class=\"green ace-icon fas fa-map-marker-alt bigger-120\" id =\"" + itemDefinition.ItemName + "_" + listDefinition.Id + "_Map_#Id#\" onclick=\"MapPopupShowFromList(this);\"></a>";
            //}
        }

        if (HasArrayValues(listDefinition.Buttons)) {
            for (var b = 0; b < listDefinition.Buttons.length; b++) {
                var button = listDefinition.Buttons[b];
                var icon = "fa fa-gear";
                var color = "";
                if (HasPropertyValue(button.Icon)) {
                    icon = button.Icon;
                }

                if (HasPropertyValue(button.Color)) {
                    color = " style=\"color:" + button.Color + ";\"";
                }

                tdActions += ListButtonRow(data.Id, icon, color, button.Function, "", button.Name);
                //tdActions += "    <a id=\"BtnCustomAction-" + button.Name + "-" + data.Id + "\" class=\"customAction actionBtn-" + button.Name + " ace-icon " + icon + " bigger-120\" id =\"" + button.Function + "_" + data.Id + "\" onclick=\"" + button.Function + "(this);\"" + color + "></a>";
               
            }
        }

        tdActions += "</td>";
    }

    res += tdData;
    if (listDefinition.EditAction !== 5 && listDefinition.EditAction !== ListEditAction.Readonly) {
        res += tdActions;
    }

    res += "</tr>";
    return res;
}

function RowCellTemplate(column, field, itemDefinition, listDefinitionId, targetForm, edition, columnIndex, columnWidth) {
    if (columnWidth < 1) {
        columnWidth = this.DefaultColumnWidth;
    }

    var res = "";
    if (field !== null) {
        switch (field.Type) {
            case "boolean":
            case "date":
            case "datetime":
                column.Align = column.Align || "center";
                if (columnWidth < 1) {
                    columnWidth = 90;
                }

                break;
            case "int":
            case "integer":
            case "decimal":
            case "money":
                column.Align = column.Align || "right";
                break;
        }
    }

    var style = " style=\"width:" + columnWidth + "px;";
    if (HasPropertyValue(column.Align)) { style += "text-align:" + column.Align + ";"; }
    style += "\"";

    var editAction = "GoItemView('" + itemDefinition.ItemName + "', #Id, '" + targetForm + "', [])";
    if (edition === ListEditAction.Popup || edition === 4) {
        editAction = "PopupItem('" + itemDefinition.ItemName + "', '" + listDefinitionId + "', this.id);";
    }

    res += "<td" + style + " data-order=\"" + columnIndex + "\"><div class=\"truncate\" style=\"width:" + (columnWidth - 20) + "px;\" title=\"#" + column.DataProperty + "Value#\">";
    res += "#" + column.DataProperty +"Value#";
    res += "</div></td>";
    return res;
}

function test() {
    var list = listSources[0];
    $("#ListDataTable_" + list.ComponentId).html("");
    var template = RowTemplate(list, { ItemId: 2, ItemName: "Alumno", Grants: "RWDFNIHGM" });

    for (var x = 0; x < list.FilteredData.length; x++) {
        var data = list.FilteredData[x];
        res = template.split("#Id#").join(data.Id);
        for (var key in data) {
            res = res.split("#" + key + "Value#").join(data[key]);
        }
        $("#ListDataTable_" + list.ComponentId).prepend(res);
    }

}

//(this.ComponentId, this.PageIndex, this.ListDefinition.PageSize, total)
function ListRenderPager(componentId, pageIndex, pageSize, total) {
    var buttons = "";
    var totalPages = 1;
    while (totalPages * pageSize < total) {
        totalPages++;
    }

    if (totalPages < 1) {
        return "";
    }

    pageIndex++;
    var x = pageIndex > 6 ? pageIndex - 5 : 1;
    var count = 0;

    while (x <= totalPages && count < 6) {
        buttons += "        <li class=\"paginate_button " + (x === pageIndex ? "active" : "") + "\" aria-controls=\"dynamic-table\" tabindex=\"0\">";
        buttons += "            <a onclick=\"ListPagerTo('" + componentId + "'," + x + ");\">" + x + "</a>";
        buttons += "        </li>";
        x++;
        count++;
    }

    var res = "<div class=\"dataTables_paginate paging_simple_numbers\" id=\"dynamic-table_paginate\">";
    res += "    <ul class=\"pagination\">";
    res += "        <li class=\"paginate_button previous"+ (pageIndex === 1  ?" disabled" : "") + "\" aria-controls=\"dynamic-table\" tabindex=\"0\" id=\"dynamic-table_previous\">";
    res += "            <a ";
    if (pageIndex > 1) {
        res += " onclick =\"ListPagerTo('" + componentId + "'," + (pageIndex - 1) + ");";
    }
    res += "\">" + Dictionary.Common_Previous + "</a>";
    res += "        </li>";
    res += buttons;
    res += "        <li class=\"paginate_button next" + (pageIndex === totalPages ? " disabled" : "") + "\" aria-controls=\"dynamic-table\" tabindex=\"0\" id=\"dynamic-table_next\">";
    res += "            <a ";
    if (pageIndex < totalPages-1) {
        res += " onclick =\"ListPagerTo('" + componentId + "'," + (pageIndex + 1) + ");";
    }
    res += "\">" + Dictionary.Common_Next + "</a>";
    res += "        </li>";
    res += "    </ul>";
    res += "</div>";

    return res;
}

function ListPagerTo(componentId, pageIndex) {
    var list = ListById(componentId.split('_')[1], componentId.split('_')[0]);
    if (list !== null) {
        list.PageIndex = --pageIndex;
        list.FillData();
    }
}

var ItemLinked_Context = {
    "ListId": null,
	"HostedItem": null,
    "AddButton": null,
    "DropDown": null
};

function ItemLinkedSetLink(sender) {
    // Extraer datos de configuración
    var listItem = $(sender).data().listitem;
    var listId = $(sender).data().listid;
    var itemLinked = $(sender).data().linkeditem;
    var itemLinkedHost = $(sender).data().linkeditemhost;
    var itemLinkedId = $("#ItemLink" + itemLinked + "Id").val() * 1;
    var btnId = "ItemLinkBtnAdd" + itemLinked;
    var errorLabelId = itemLinkedHost + "_Add" + itemLinked + "Error";
    var itemBridge = $(sender).data().itembridge;

    ItemLinked_Context = {
        "ListId": listId,
        "AddButton": btnId,
		"HostedItem": listItem,
        "DropDown": "#ItemLink" + itemLinked + "Id"
    };

    // Ocultar mensaje de error si lo hubiera
    $("#" + errorLabelId).html("");
    $("#" + errorLabelId).hide();

    // Comprobar que se ha seleccionado un elemento
    if (itemLinkedId === 0) {
        $("#" + errorLabelId).html(Dictionary.Common_Error_SelectNone);
        $("#" + errorLabelId).show();
        $("#" + btnId).enable();
        return false;
    }

    //console.log(itemLinkedHost + "(" + itemLinkedId + ")", itemLinked);

    // Comprobar si ya está asociado
    var listSource = null;
    for (var x = 0; x < listSources.length; x++) {
        if (listSources[x].ListDefinition.Id === listId && listSources[x].ItemName === listItem) {
            listSource = listSources[x];
            for (var a = 0; a < listSource.Data.length; a++) {
                if (listSource.Data[a][itemLinked + "Id"].Id * 1 === itemLinkedId) {
                    $("#" + errorLabelId).html(Dictionary.Common_AlreadyExists);
                    $("#" + errorLabelId).show();
                    $("#" + btnId).enable();
                    return false;
                }
            }
        }
    }

    var data = {
        "itemName": itemBridge,
        "linkedItem": itemLinked + "Id",
        "hostItem": itemLinkedHost + "Id",
        "hostId": itemData.Id,
        "linkedId": itemLinkedId,
        "companyId": CompanyId,
        "applicationUserId": ApplicationUser.Id
    };

    //console.log(data);
    $.ajax({
        "type": "POST",
        "url": "/Async/ItemService.asmx/SetItemLink",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            if (msg.d.Success === true) {
                var listSource = null;
                for (var x = 0; x < listSources.length; x++) {
                    if (listSources[x].ListDefinition.Id === ItemLinked_Context.ListId && listSources[x].ItemName === ItemLinked_Context.HostedItem) {
                        listSource = listSources[x];
                    }
                }

                if (listSource !== null) {
                    GetListData(listSource);
                }

                $("#" + ItemLinked_Context.AddButton).enable();
                $("#" + ItemLinked_Context.Dropdown).val(0);
                $("#" + ItemLinked_Context.Dropdown).trigger("chosen:updated");
            }
        },
        "error": function (msg) {
            console.log(msg.responseText);
            $("#" + ItemLinked_Context.AddButton).enable();
        }
    });
}

function PageListById(itemName, listId) {
    if (ListSources !== null) {
        for (var x = 0; x < ListSources.length; x++) {
            if (ListSources[x].ItemName === itemName && ListSources[x].ListId === listId) {
                return ListSources[x];
            }
        }
    }

    return null;
}

function RemoveListSourceById(itemName, listId) {
    var temp = [];
    if (ListSources !== null) {
        for (var x = 0; x < ListSources.length; x++) {
            if (ListSources[x].ItemName !== itemName || ListSources[x].ListId !== listId) {
                temp.push(ListSources[x]);
            }
        }
    }

    listSources = temp;
}

function ListSourceItemById(itemName, listId, itemId) {
    var data = PageListById("ResidenteIMC", "ByResidente").Data
    if (data !== null) {
        for (var x = 0; x < data.length; x++) {
            if (data[x].Id === itemId) {
                return data[x];
            }
        }
    }

    return null;
}

async function GetDataFromListDefinitionStandalone(data) {
    try {
        const res = await GetDataFromListDefinitionStandaloneGo(data);
        console.log("weke", res);
    } catch (err) {
        console.log(err);
    }
}

function GetDataFromListDefinitionStandaloneGo(data) {
    $.ajax({
        "type": "POST",
        "url": "/Async/ItemService.asmx/GetList",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            var data = null;
            eval("data = " + msg.d + ";");

            if (typeof data.data === "string") {
                Notify(data.data, "error");
                $("#" + data.ItemName + "_" + data.ListId + "_ListCount").html("Error: <i style=\"color:red;\">" + data.data + "</i>");
            }
            else {
                console.log("GetDataFromListDefinitionStandaloneGo", data.ItemName);
                if (debugConsole === true) { console.time("TableFillData", data.ItemName + " --> " + data.ListId); }
                GetDataCallBack(data);
            }
        },
        "error": function (msg) {
            console.log(msg.responseText);
        }
    });
}

function ListSetLoading(componentId) {
    console.log("TODO ListSetLoading", componentId);
}

function List_Sort(sender) {
    console.log($(sender).data());
    var listId = $(sender).data("tableid");
    var dataProperty = $(sender).data("sortfield");
    var sortType = $(sender).data("sorttype").toLowerCase();
    var actualOrder = $(sender).hasClass("ASC") ? "DESC" : "ASC";
    var config = { "actualOrder": actualOrder, "dataProperty": dataProperty, "sortType": sortType };
    console.clear();
    console.log("PRE",config);
    LocalStorageSetJson("Sort_" + listId, config );
    List_SortGo("Sort_" + listId);
}

function List_SortGo(configId) {
    var list = PageListById(configId.split('_')[1], configId.split('_')[2]);
    var config = LocalStorageGetJson(configId);
    if (config === null) {
        var x = ItemListById(list.ItemDefinition, list.ListId);
        console.log("Sorting", x.Sorting);
        var column = x.Columns[x.Sorting.Index - 1];
        var config = {
            "actualOrder": x.Sorting.SortingType.toUpperCase(),
            "dataProperty": column.DataProperty,
            "sortType": "text"
        };

        console.log("default config", config);

        LocalStorageSetJson(configId, config);

        return;
    }

    var data = list.Data;
    var dataProperty = config.dataProperty;
    var actualOrder = config.actualOrder;
    var listId = configId.split('_')[1] + "_" + configId.split('_')[2];
    console.log(data);
    $("#" + listId + "_ListHead .sort").removeClass("ASC");
    $("#" + listId + "_ListHead .sort").removeClass("DESC");

    $("#" + listId + "_ListHead [data-sortfield='" + config.dataProperty + "']").addClass(actualOrder);
    console.log("POST",config);

    switch (config.sortType) {
        case "money":
            data.sort(function (a, b) {
                return (a[dataProperty] < b[dataProperty] ? -1 : 1) * (actualOrder === "DESC" ? -1 : 1);
            });
            break;
        case "datetime":
            data.sort(function (a, b) {
                return (GetDate(a[dataProperty], "/", false) < GetDate(b[dataProperty], "/", false) ? -1 : 1) * (actualOrder === "DESC" ? -1 : 1);
            });
            break;
        default:
            data.sort(function (a, b) {

                return (a[dataProperty] < b[dataProperty] ? -1 : 1) * (actualOrder === "DESC" ? -1 : 1) ;
            });
            break;
    }

    list.Data = data;
    SearchList();


}

function DeleteItem(itemDefinitionId, listId, itemId, mode) {
    var itemDefinition = ItemDefinitionById(itemDefinitionId);
    var list = PageListById(itemDefinition.ItemName, listId);
    var item = GetItemById(list, itemId);
    console.log(item);

    PopupDeleteContext.ItemDefinition = itemDefinition;
    PopupDeleteContext.ItemId = itemId;
    PopupDeleteContext.ListId = listId;
    PopupDeleteContext.Message = ItemGetDescription(itemDefinition, item);
    PopupDeleteContext.Mode = mode;
    PopupRenderDelete();
    $("#LauncherPopupDelete").click();
}

function GetItemById(pageList, itemId) {
    itemId = itemId * 1;
    var res = pageList.Data.filter(function (i) { return i.Id === itemId });

    if (res.length > 0) {
        return res[0];
    }

    return null;
}

function FilterList(listId, itemName) {
    var list = PageListById(itemName, listId);
    if (list !== null) {
        list.Search();
    }
}

function List_Export(exportType, itemName, listId) {
    PopupPrinting("Exportant llista a " + exportType);
}