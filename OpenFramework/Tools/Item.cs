// --------------------------------
// <copyright file="Tools.Item.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web;
    using OpenFrameworkV3.Core.Enums;
    using OpenFrameworkV3.Core.ItemManager;
    using OpenFrameworkV3.Core.ItemManager.ItemForm;
    using OpenFrameworkV3.Core.ItemManager.ItemList;
    using OpenFrameworkV3.Security;

    public static class Item
    {
        public static string UserConvertible(long itemDefinitionId)
        {
            if (HttpContext.Current.Session["UserConversions"] == null)
            {
                return Constant.JavaScriptNull;
            }

            var userConversions = HttpContext.Current.Session["UserConversions"] as List<UserConversion>;
            if (userConversions.Any(uc => uc.ItemDefinitionId == itemDefinitionId))
            {
                return userConversions.First(uc => uc.ItemDefinitionId == itemDefinitionId).Json;
            }

            return Constant.JavaScriptNull;
        }

        public static bool UserConvertibleAvailable(long itemDefinitionId)
        {
            if (HttpContext.Current.Session["UserConversions"] == null)
            {
                return false;
            }

            var userConversions = HttpContext.Current.Session["UserConversions"] as List<UserConversion>;
            if (userConversions.Any(uc => uc.ItemDefinitionId == itemDefinitionId))
            {
                return true;
            }

            return false;
        }

        private const string DefinitionPattern =
                @"{{
    ""Id"": {0},
    ""ItemName"": ""{1}"",
    ""Description"": ""{2}"",
    ""DataOrigin"": {3},{4}{5},
    {6},
    {7},{12}{13}
    ""CoreValues"": {11},
    ""Fields"": [{8}
    ],
    ""Lists"": [{9}
    ],
    ""Forms"": [{10}
    ]
}}";

        private const string FormPattern =
                @"
        {{
            ""Id"": ""{0}"",
            ""FormType"": ""{1}"",{2}
            ""Tabs"": [{3}
            ]{4}
        }}";

        private const string TabPattern =
                @"
                {{
                    ""Id"": ""{0}"",
                    ""Label"": ""{1}"",
                    ""Groups"":{7},{2}{3}{4}{6}{8}
                    ""Rows"": [{5}
                    ]
                }}";

        private const string ListPattern =
                @"
        {{
            ""Id"": ""{0}"",
            ""FormId"": ""{1}"",
            {2}{12}{3}{4}{5}{6}{7}{8}{9}{10}{11}{15}{16}{18}{19}{20}
            ""Columns"": [{13}
            ], 
            ""Sorting"": [ {{ ""Index"":1, ""SortingType"": ""asc"" }} ]{14}{17}
        }}";

        public static string Geolocation(ItemGeolocation definition)
        {
            if (definition == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder("\n\t\tGeolocation: {");
            res.Append(PropertyJson("WayType", definition.WayType, 4));
            res.Append(PropertyJson("Address", definition.Address, 4));
            res.Append(PropertyJson("AddressComplement", definition.AddressComplement, 4));
            res.Append(PropertyJson("PostalCode", definition.PostalCode, 4));
            res.Append(PropertyJson("City", definition.City, 4));
            res.Append(PropertyJson("Province", definition.Province, 4));
            res.Append(PropertyJson("Country", definition.Country, 4));
            res.Append(PropertyJson("Latitude", definition.Latitude, 4));
            res.Append(PropertyJson("Longitude", definition.Longitude, 4));
            res.Append("},");
            return res.ToString().Replace(",}", "}");
        }

        public static string Json(ItemDefinition definition)
        {
            var res = string.Format(
                CultureInfo.InvariantCulture,
                DefinitionPattern,
                definition.Id,
                definition.ItemName,
                definition.Description ?? string.Empty,
                definition.DataOrigin,
                DefinitionFeatures(definition.Features, 1),
                definition.Layout.Json,
                ForeignValues(definition),
                PrimaryKeys(definition),
                Fields(definition, 2),
                Lists(definition, 2),
                Forms(definition, 2),
                ConstantValue.Value(definition.CoreValues),
                "null", // weke Geolocation(definition.Geolocation),
                PropertyJson("CustomFK", definition.CustomFK));
            return res;
        }

        private static string Lists(ItemDefinition definition, int fill)
        {
            var res = new StringBuilder();
            var firstList = true;
            foreach (var list in definition.Lists)
            {
                if (firstList)
                {
                    firstList = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(ListJson(list, fill + 1));
            }

            return res.ToString();
        }

        private static string Forms(ItemDefinition definition, int fill)
        {
            var res = new StringBuilder();
            var first = true;
            foreach (var form in definition.Forms)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(FormJson(form, fill + 1));
            }

            return res.ToString();
        }

        private static string Fields(ItemDefinition definition, int fill)
        {
            var res = new StringBuilder();
            var first = true;
            foreach (var field in definition.Fields)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(Environment.NewLine);
                res.Append(string.Empty.PadRight((fill + 1) * 4));
                res.Append(FieldJson(field, fill + 1));
            }

            return res.ToString();
        }

        private static string FieldJson(ItemField field, int fill)
        {
            string dataFormat = string.Empty;
            if (field.ColumnDataFormat != null)
            {
                dataFormat = string.Format(CultureInfo.InvariantCulture, @", ""DataFormat"": {{ ""Name"": ""{0}"" }}", field.ColumnDataFormat.Name);
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"{{ ""Name"": ""{0}"", ""Label"": ""{1}"", ""Type"": ""{2}""{3}{4}{5}{6}{7}{8}{9}{10}{11}{12} }}",
                field.Name,
                string.IsNullOrEmpty(field.Label) ? field.Label : Tools.Json.JsonCompliant(field.Label),
                field.DataTypePropertyValue.ToLowerInvariant(),
                field.Length.HasValue ? string.Format(CultureInfo.InvariantCulture, @" ,""Length"": {0}", field.Length) : string.Empty,
                field.FK ? " ,\"FK\": true" : string.Empty,
                field.Required ? " ,\"Required\": true" : string.Empty,
                dataFormat,
                string.IsNullOrEmpty(field.CodeSequence) ? string.Empty : string.Format(CultureInfo.InvariantCulture, @" ,""CodeSequence"":""{0}""", field.CodeSequence),
                string.IsNullOrEmpty(field.FKItem) ? string.Empty : string.Format(CultureInfo.InvariantCulture, @" ,""FKItem"":""{0}""", field.FKItem),
                field.MaintainOriginalName ? " ,\"MaintainOriginalName\": true" : string.Empty,
                field.Linkable ? " ,\"Linkable\": true" : string.Empty,
                field.Geolocalization.HasValue ? string.Format(CultureInfo.InvariantCulture, @", ""Geolocation"": {0}", field.Geolocalization.Value) : string.Empty,
                field.Internal ? " ,\"Internal\": true" : string.Empty);
        }

        private static string DefinitionFeatures(ItemFeatures definition, int fill)
        {
            var features = new StringBuilder(string.Format(CultureInfo.InvariantCulture, @"{0}{1}""Features"": {{ ", Environment.NewLine, string.Empty.PadRight(fill * 4)));
            features.Append(PropertyJson("Attachs", definition.Attachs).Trim());
            features.Append(PropertyJson("BankAccount", definition.BankAccount).Trim());
            features.Append(PropertyJson("Invoices", definition.Invoices).Trim());
            features.Append(PropertyJson("Follow", definition.Following).Trim());
            features.Append(PropertyJson("FAQs", definition.FAQs).Trim());
            features.Append(PropertyJson("Firmafy", definition.Firmafy).Trim());
            features.Append(PropertyJson("Geolocation", definition.Geolocation).Trim());
            features.Append(PropertyJson("AdminRestriction", definition.AdminRestriction).Trim());
            features.Append(PropertyJson("Persistence", definition.Persistence).Trim());
            features.Append(PropertyJson("Traces", definition.Traces).Trim());
            features.Append(PropertyJson("ScopeView", definition.ScopeView).Trim());
            features.Append(PropertyJson("EventCalendar", definition.EventCalendar).Trim());
            features.Append(PropertyJson("Unloadable", definition.Unloadable).Trim());
            features.Append(PropertyJson("Importable", definition.Importable).Trim());
            features.Append(PropertyJson("News", definition.News).Trim());
            features.Append(PropertyJson("ShowOnPortal", definition.ShowOnPortal).Trim());
            features.Append(PropertyJson("Tags", definition.Tags).Trim());
            features.Append(PropertyJson("Mailable", definition.Mailable).Trim());
            features.Append(PropertyJson("MailLink", definition.MailLink).Trim());
            features.Append(PropertyJson("DocumentSign", definition.DocumentSign).Trim());
            features.Append(PropertyJson("DocumentHistory", definition.DocumentHistory).Trim());
            features.Append(PropertyJson("ContactPerson", definition.ContactPerson).Trim());
            features.Append(PropertyJson("Schedule", definition.Schedule).Trim());
            features.Append(PropertyJson("Sticky", definition.Sticky).Trim());
            var res = features.ToString().Substring(0, features.Length - 1);
            return string.Format(CultureInfo.InvariantCulture, @"{1}{0} }},", string.Empty.PadRight(fill * 4), res);
        }

        private static string ForeignValues(ItemDefinition definition)
        {
            var res = new StringBuilder("\"ForeignValues\": [");
            var first = true;
            foreach (var fv in definition.ForeignValues)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(", ");
                }

                var localName = fv.LocalName;
                if (localName.Equals(fv.ItemName + "id", StringComparison.OrdinalIgnoreCase))
                {
                    localName = string.Empty;
                }

                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @" {{ ""ItemName"": ""{0}""{1} }}",
                    fv.ItemName,
                    string.IsNullOrEmpty(localName) ? string.Empty : ", \"LocalName\": \"" + localName + "\"");
            }

            res.Append(" ]");
            return res.ToString();
        }

        private static string PrimaryKeys(ItemDefinition definition)
        {
            var res = new StringBuilder("\"PrimaryKeys\": [");
            var first = true;
            foreach (var pk in definition.PrimaryKeys)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(", ");
                }

                bool firstValue = true;
                foreach (var value in pk)
                {
                    if (firstValue)
                    {
                        firstValue = false;
                    }
                    else
                    {
                        res.Append(", ");
                    }

                    res.AppendFormat(CultureInfo.InvariantCulture, @"""{0}""", value);
                }
            }

            res.Append(" ]");
            return res.ToString();
        }

        private static string PropertyJson(string label, bool? value)
        {
            if (value.HasValue)
            {
                return PropertyJson(label, value.Value, 0);
            }

            return string.Empty;
        }

        private static string PropertyJson(string label, bool value)
        {
            return PropertyJson(label, value, 0);
        }

        private static string PropertyJson(string label, bool value, int fill)
        {
            if (fill == 0)
            {
                return value ? string.Format(CultureInfo.InvariantCulture, @"""{0}"": true,", label) : string.Empty;
            }
            else
            {
                return value ? string.Format(CultureInfo.InvariantCulture, @"{1}{2}""{0}"": true,", label, Environment.NewLine, string.Empty.PadRight(fill * 4)) : string.Empty;
            }
        }

        private static string PropertyJson(string label, string value)
        {
            return PropertyJson(label, value, 0);
        }

        private static string PropertyJson(string label, string value, int fill)
        {
            if (fill == 0)
            {
                return string.IsNullOrEmpty(value) ? string.Empty : string.Format(CultureInfo.InvariantCulture, @"""{0}"": ""{1}"",", label, Tools.Json.JsonCompliant(value));
            }
            else
            {
                return string.IsNullOrEmpty(value) ? string.Empty : string.Format(CultureInfo.InvariantCulture, @"{2}{3}""{0}"": ""{1}"",", label, Tools.Json.JsonCompliant(value), Environment.NewLine, string.Empty.PadRight(4 * fill));
            }
        }

        private static string PropertyJson(string label, int? value)
        {
            return PropertyJson(label, value, 0);
        }

        private static string PropertyJson(string label, int? value, int fill)
        {
            if (fill == 0)
            {
                return value > 0 ? string.Format(CultureInfo.InvariantCulture, @"""{0}"": {1},", label, Tools.Json.JsonCompliant(value)) : string.Empty;
            }
            else
            {
                return value > 0 ? string.Format(CultureInfo.InvariantCulture, @"{2}{3}""{0}"": {1},", label, Tools.Json.JsonCompliant(value), Environment.NewLine, string.Empty.PadRight(4 * fill)) : string.Empty;
            }
        }

        public static string FormJson(Form form, int fill)
        {
            var tabs = new StringBuilder();
            var firstTab = true;
            foreach (var tab in form.Tabs)
            {
                if (firstTab)
                {
                    firstTab = false;
                }
                else
                {
                    tabs.Append(",");
                }

                tabs.Append(TabJson(tab, fill + 2));
            }

            var actions = new StringBuilder();
            if (form.Actions.Count > 0)
            {
                actions.Append(Environment.NewLine);
                actions.Append(",\"Actions\": [");
                var firstAction = true;
                foreach (var action in form.Actions)
                {
                    if (firstAction)
                    {
                        firstAction = false;
                    }
                    else
                    {
                        actions.Append(",");
                    }

                    actions.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"{{""Label"":""{0}"", ""Action"": ""{1}"", ""Icon"": ""{2}"",{3}{4}}}",
                        action.Label,
                        action.Action,
                        action.Icon,
                        PropertyJson("Important", action.Important),
                        PropertyJson("Tab", action.Tab));
                }

                actions.Append("]");
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                FormPattern,
                form.Id,
                form.FormTypeName ?? "Custom",
                PropertyJson("Label", form.Label, fill + 1),
                tabs,
                actions);
        }

        private static string TabJson(FormTab tab, int fill)
        {
            if (tab == null)
            {
                return Constant.JavaScriptNull;
            }

            var rowsJson = new StringBuilder();
            if (tab.Rows.Count > 0)
            {
                bool firstRow = true;
                foreach (var row in tab.Rows)
                {
                    if (firstRow)
                    {
                        firstRow = false;
                    }
                    else
                    {
                        rowsJson.Append(",");
                    }

                    rowsJson.Append(Environment.NewLine);
                    rowsJson.Append(string.Empty.PadRight((fill + 1) * 4));
                    rowsJson.Append(FormRowJson(row, fill + 1));
                }
            }

            rowsJson.Append(" ");

            return string.Format(
                CultureInfo.InvariantCulture,
                TabPattern,
                tab.Id,
                tab.Label ?? tab.Id,
                PropertyJson("Explanation", tab.Explanation, fill),
                PropertyJson("Listable", tab.Listable, fill),
                PropertyJson("ItemMustExists", tab.ItemMustExists, fill),
                rowsJson,
                PropertyJson("Persistent", tab.Persitent, fill),
                tab.GroupsJson,
                PropertyJson("Hidden", tab.Hidden, fill));
        }

        public static string ListJson(List list, int fill)
        {
            if (list == null)
            {
                return Constant.JavaScriptNull;
            }

            var columns = new StringBuilder();
            var firstColumn = true;
            foreach (var column in list.Columns)
            {
                if (firstColumn)
                {
                    firstColumn = false;
                }
                else
                {
                    columns.Append(",");
                }

                columns.Append(Environment.NewLine);
                columns.Append(string.Empty.PadRight((fill + 1) * 4));
                columns.Append(ListColumn(column, fill + 1));
            }

            var parameters = new StringBuilder();
            if (list.Parameters.Count > 0)
            {
                parameters = new StringBuilder(Environment.NewLine).Append(",            \"Parameters\": [");
                bool firstParameter = true;
                foreach (var parameter in list.Parameters)
                {
                    if (firstParameter)
                    {
                        firstParameter = false;
                    }
                    else
                    {
                        parameters.Append(", ");
                    }

                    parameters.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"{{ ""Name"":""{0}"", ""Value"":""{1}"" }}",
                        parameter.Name,
                        parameter.Value);
                }


                parameters.Append("]");
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                ListPattern,
                list.Id,
                list.FormId ?? "Custom",
                PropertyJson("Layout", list.Layout, fill),
                PropertyJson("EditAction", list.EditAction, fill),
                PropertyJson("RowClass", list.RowClass, fill),
                PropertyJson("Exportable", list.Exportable, fill),
                PropertyJson("Importable", list.Importable, fill),
                PropertyJson("Filtrable", list.Filtrable, fill),
                PropertyJson("CustomAjaxSource", list.CustomAjaxSource, fill),
                PropertyJson("Label", list.Label, fill),
                PropertyJson("Title", list.Title, fill),
                PropertyJson("ReadOnly", list.ReadOnly, fill),
                PropertyJson("MinHeight", list.MinHeight, fill),
                columns,
                parameters,
                PropertyJson("Template", list.Template),
                PropertyJson("PageSize", list.PageSize, fill),
                ListButtonsJson(list.Buttons),
                PropertyJson("ForceHeight", list.ForceHeight, fill),
                PropertyJson("ItemLink", list.ItemLink, fill),
                PropertyJson("Explanation", list.Explanation, fill),
                PropertyJson("NoDataMessage", list.NoDataMessage, fill));
        }

        private static string ListButtonsJson(ReadOnlyCollection<ListButton> buttons)
        {
            if (buttons == null || buttons.Count == 0)
            {
                return string.Empty;
            }

            var res = new StringBuilder(Environment.NewLine).Append(@"            ,""Buttons"":[");
            var first = true;
            foreach (var button in buttons)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"{5}                {{ ""Name"":""{0}"", ""Label"":""{1}"", ""Icon"":""{2}"", ""Function"":""{3}"", ""Color"": ""{4}"" }}",
                    button.Name,
                    button.Label,
                    button.Icon,
                    button.Function,
                    button.Color,
                    Environment.NewLine);
            }

            res.Append("            ]");
            return res.ToString();
        }

        private static string FormRowJson(FormRow row, int fill)
        {
            var fieldsJson = new StringBuilder();
            fieldsJson.AppendFormat(CultureInfo.InvariantCulture, @"{0}{1}""Fields"": [ ", Environment.NewLine, string.Empty.PadRight((fill + 1) * 4));
            if (row.Fields.Count > 0)
            {
                var first = true;
                foreach (var field in row.Fields)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        fieldsJson.Append(",");
                    }

                    fieldsJson.Append(Environment.NewLine);
                    fieldsJson.Append(string.Empty.PadRight((fill + 2) * 4));
                    fieldsJson.Append(FormFieldJson(field));
                }

                fieldsJson.AppendFormat(CultureInfo.InvariantCulture, @"{0}{1}", Environment.NewLine, string.Empty.PadRight((fill + 1) * 4));
            }

            fieldsJson.Append("]");

            return string.Format(
                CultureInfo.InvariantCulture,
                @"{{{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}}}",
                PropertyJson("Id", row.Id, fill + 1),
                PropertyJson("Label", row.Label, fill + 1),
                PropertyJson("ListId", row.ListId, fill + 1),
                PropertyJson("ItemList", row.ItemList, fill + 1),
                PropertyJson("Hidden", row.Hidden, fill + 1),
                PropertyJson("Expandible", row.Expandible, fill + 1),
                PropertyJson("ExpandibleGroup", row.ExpandibleGroup, fill + 1),
                PropertyJson("ExpandibleCollapsed", row.ExpandibleCollapsed, fill + 1),
                fieldsJson,
                Environment.NewLine,
                string.Empty.PadRight((fill) * 4));
        }

        private static string FormFieldJson(FormField field)
        {
            var res = string.Format(
                CultureInfo.InvariantCulture,
                @"{{ {0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}",
                PropertyJson("Name", field.Name),
                PropertyJson("Label", field.Label),
                PropertyJson("ReadOnly", field.ReadOnly),
                PropertyJson("Wysiwyg", field.Wysiwyg),
                PropertyJson("Type", field.Type),
                PropertyJson("Rows", field.Rows),
                PropertyJson("ColSpan", field.ColSpan),
                PropertyJson("Function", field.Function),
                PropertyJson("Hidden", field.Hidden),
                PropertyJson("Icon", field.Icon),
                PropertyJson("Style", field.Style),
                PropertyJson("Extend", ((ExtendSelect)field.ExtendSelect).ToString()),
                PropertyJson("BAR", field.BAR),
                PropertyJson("Config", field.Config),
                PropertyJson("LabelAlign", field.LabelAlign),
                PropertyJson("Width", field.Width),
                PropertyJson("Compact", field.Compact));
            return res.Substring(0, res.Length - 1).Replace(",", ", ") + " }";
        }

        private static string ListColumn(Column column, int fill)
        {
            var others = new StringBuilder();
            others.Append(PropertyJson("Label", column.Label));
            others.Append(PropertyJson("ReplacedBy", column.ReplacedBy));
            others.Append(PropertyJson("Render", column.Render));
            others.Append(PropertyJson("RenderData", column.Render));
            others.Append(PropertyJson("Align", column.Align));
            others.Append(PropertyJson("Width", column.Width));
            others.Append(PropertyJson("Search", column.Search));
            others.Append(PropertyJson("Orderable", column.Orderable));
            others.Append(PropertyJson("Linkable", column.Linkable));
            others.Append(PropertyJson("LinkableItem", column.LinkableItem));
            others.Append(PropertyJson("HiddenList", column.HiddenList));
            others.Append(PropertyJson("HiddenMobile", column.HiddenMobile));

            var othersText = others.ToString();
            if (!string.IsNullOrEmpty(othersText))
            {
                othersText = othersText.ToString().Substring(0, others.Length - 1);
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"{{ ""DataProperty"": ""{0}"",{1} }}",
                column.DataProperty,
                othersText).Replace(",", ", ");
        }
    }
}