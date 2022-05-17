// --------------------------------
// <copyright file="AlertDefinition.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Feature
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.ItemManager;
    using OpenFrameworkV3.Core.Security;
    using OpenFrameworkV3.Tools;

    /// <summary>Implements alerts definition</summary>
    public class AlertDefinition
    {
        /// <summary>Custom menu for this alert</summary>
        [JsonProperty("CustomMenu")]
        private string customMenu;

        /// <summary>Gets alert definition from disk</summary>
        /// <returns>Alert definition structure</returns>
        public static ReadOnlyCollection<AlertDefinition> GetFromDisk(string instancename, long applicationUserId)
        {
            var user = ApplicationUser.ById(applicationUserId, instancename);
            var res = new List<AlertDefinition>();
            var path = Instance.Path.Alerts(instancename);
            Basics.VerifyFolder(path);
            var myFiles = Directory.GetFiles(path, "*.alert", SearchOption.TopDirectoryOnly).ToList();

            foreach (string fileName in myFiles)
            {
                var definition = GetDefinitionByFile(user.CompanyId, fileName, user.Id);

                if (user.AdminUser)
                {
                    res.Add(definition);
                }
                else
                {
                    if (definition.Groups == null || definition.Groups.Count() == 0 || definition.Owner)
                    {
                        res.Add(definition);
                        continue;
                    }

                    foreach (var g in user.Groups)
                    {
                        if (definition.Groups.Any(gr => gr == g))
                        {
                            res.Add(definition);
                            break;
                        }
                    }
                }
            }

            return new ReadOnlyCollection<AlertDefinition>(res);
        }

        /// <summary>Gets or sets de company identifier</summary>
        [JsonProperty("CompanyId")]
        public int CompanyId { get; set; }

        /// <summary>Gets or sets de item type</summary>
        [JsonProperty("ItemType")]
        public long ItemType { get; set; }

        /// <summary>Gets or sets icon</summary>
        [JsonProperty("Icon")]
        public string Icon { get; set; }

        /// <summary>Gets or sets the alert short description</summary>
        [JsonProperty("AlertDescription")]
        public string AlertDescription { get; set; }

        /// <summary>Gets or sets the alert large explanation</summary>
        [JsonProperty("AlertExplanation")]
        public string AlertExplanation { get; set; }

        /// <summary>Gets or sets a custom category</summary>
        [JsonProperty("Category")]
        public string Category { get; set; }

        /// <summary>Gets or sets the importance of alert</summary>
        [JsonProperty("Importance")]
        public int Importance { get; set; }

        /// <summary>Gets or sets the query to extract alert occurrences</summary>
        [JsonProperty("Query")]
        public string Query { get; set; }

        /// <summary>Gets or sets the HTML template for alert menu tag</summary>
        [JsonProperty("Tag")]
        public string Tag { get; set; }

        /// <summary>Gets or sets tab of form target</summary>
        [JsonProperty("Tab")]
        public string Tab { get; set; }

        /// <summary>Gets or sets the HTML template for alert list row</summary>
        [JsonProperty("Row")]
        public string Row { get; set; }

        /// <summary>Gets or sets the url of affected item</summary>
        [JsonProperty("ItemUrl")]
        public string ItemUrl { get; set; }

        /// <summary>Gets or sets the index of field positions</summary>
        [JsonProperty("Index")]
        private AlertFieldPosition[] Index { get; set; }

        /// <summary>Gets or sets the index of field positions</summary>
        [JsonProperty("Groups")]
        private int[] groups { get; set; }

        /// <summary>Gets or sets a value indicating whether owner can see alert/summary>
        [JsonProperty("Owner")]
        private bool owner { get; set; }

        /// <summary>Gets or sets the index of field positions</summary>
        [JsonProperty("AfterLogin")]
        private bool AfterLogin { get; set; }

        /// <summary>Gets or sets action to apply</summary>
        [JsonProperty("CustomAction")]
        private string CustomAction { get; set; }

        /// <summary>Gets or sets identifier of blocked action</summary>
        [JsonProperty("BlockAction")]
        public int BlockAction { get; set; }

        [JsonIgnore]
        public bool Owner
        {
            get
            {
                if (this.owner)
                {
                    return true;
                }

                return false;
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<long> Groups
        {
            get
            {
                var res = new List<long>();
                if (this.groups != null && this.groups.Length > 0)
                {
                    foreach (var id in this.groups)
                    {
                        res.Add(Convert.ToInt64(id));
                    }
                }


                return new ReadOnlyCollection<long>(res);
            }
        }

        /// <summary>Gets the custom menu for this alert</summary>
        [JsonIgnore]
        public string CustomMenu
        {
            get
            {
                if (string.IsNullOrEmpty(this.customMenu))
                {
                    return string.Empty;
                }

                return this.customMenu.ToLowerInvariant();
            }
        }

        /// <summary>Read alert definition from file</summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="fileName">File name of alert</param>
        /// <param name="userId">Identifier of actual user</param>
        /// <returns>AlertDefinition structure</returns>
        public static AlertDefinition GetDefinitionByFile(long companyId, string fileName, long userId)
        {
            var alert = new AlertDefinition();
            if (File.Exists(fileName))
            {
                using (var input = new StreamReader(fileName))
                {
                    alert = JsonConvert.DeserializeObject<AlertDefinition>(input.ReadToEnd());
                    alert.Query = alert.Query.Replace("#CompanyId#", companyId.ToString(CultureInfo.InvariantCulture));
                    alert.Query = alert.Query.Replace("#ActualUser#", userId.ToString());
                    if (alert.CompanyId != 0 && alert.CompanyId != companyId)
                    {
                        return new AlertDefinition();
                    }
                }
            }

            return alert;
        }

        /// <summary>Creates a HTML code for a alert row of alerts page</summary>
        /// <param name="dictionary">Dictionary for fixed label</param>
        /// <returns>HTML code for a alert row of alerts page</returns>
        public ReadOnlyCollection<string> RenderRow(Dictionary<string, string> dictionary, string instanceName, bool remindAlerts)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            var res = new List<string>();
            var columns = new List<string>();
            foreach (var position in this.Index.OrderBy(x => x.Position))
            {
                columns.Add(position.FieldName);
            }

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand(this.Query))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.Text;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    var data = new List<string>();
                                    foreach (string columnName in columns)
                                    {
                                        data.Add(rdr[columnName].ToString());
                                    }

                                    var shortDescription = this.AlertDescription;
                                    if (this.AlertDescription != null)
                                    {
                                        if (this.AlertDescription.StartsWith("Alert_Short_", StringComparison.OrdinalIgnoreCase))
                                        {
                                            if (dictionary.ContainsKey(this.AlertDescription))
                                            {
                                                shortDescription = dictionary[this.AlertDescription];
                                            }
                                        }
                                    }

                                    var largeExplanation = this.AlertExplanation;
                                    if (this.AlertExplanation != null)
                                    {
                                        if (this.AlertExplanation.StartsWith("Alert_Large_", StringComparison.OrdinalIgnoreCase))
                                        {
                                            if (dictionary.ContainsKey(this.AlertExplanation))
                                            {
                                                largeExplanation = dictionary[this.AlertExplanation];
                                            }
                                        }
                                    }

                                    // Comporbar si están vacías
                                    if (string.IsNullOrEmpty(largeExplanation))
                                    {
                                        largeExplanation = shortDescription;
                                    }

                                    if (string.IsNullOrEmpty(shortDescription))
                                    {
                                        shortDescription = largeExplanation;
                                    }

                                    var remindAlertCell = string.Empty;
                                    if (remindAlerts)
                                    {
                                        remindAlertCell = string.Format(
                                            CultureInfo.InvariantCulture,
                                            @"<td style=""text-align:center;width:50px;"">
                                             <i class=""fa fa-comment red"" title=""{0}"" onclick=""FEATURE_Alerts_Remind(this);""></i>
                                             <i class=""fa fa-lock blank"" title=""{1}"" onclick=""FEATURE_Alerts_BlockAction(this, true,{2});""></i>
                                         </td>",
                                            ApplicationDictionary.Translate("Feature_Messagin_IconTitle"),
                                            ApplicationDictionary.Translate("Feature_BlockAction_IconTitle"),
                                            this.BlockAction);

                                        if (this.groups == null || this.groups.Length == 0 || this.BlockAction < 1)
                                        {
                                            remindAlertCell = string.Format(
                                            CultureInfo.InvariantCulture,
                                            @"<td style=""text-align:center;width:50px;"">
                                             <i class=""fa fa-comment red"" title=""{0}"" onclick=""FEATURE_Alerts_Remind(this);""></i>
                                         </td>",
                                            ApplicationDictionary.Translate("Feature_Messagin_IconTitle"));
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(this.Row))
                                    {
                                        res.Add(string.Format(
                                            CultureInfo.InvariantCulture,
                                            this.Row.Replace("#AlertDescription#", largeExplanation),
                                            data.ToArray()));
                                    }
                                    else
                                    {
                                        try
                                        {
                                            var groups = string.Empty;
                                            if (this.groups != null && this.groups.Count() > 0)
                                            {
                                                var first = true;
                                                foreach (var group in this.groups)
                                                {
                                                    groups += string.Format(CultureInfo.InvariantCulture, "{0}{1}",
                                                        first ? string.Empty : ",",
                                                        group);
                                                    first = false;
                                                }
                                            }

                                            var itemDefinition = ItemDefinition.Empty;
                                            var itemLabel = string.Empty;
                                            if (this.ItemType > 0)
                                            {
                                                if (this.ItemType == 10000)
                                                {
                                                    itemLabel = ApplicationDictionary.Translate("Billing_Invoice");
                                                    itemDefinition.ItemName = "Billing_Invoice";
                                                }
                                                else if (this.ItemType == 10005)
                                                {
                                                    itemLabel = ApplicationDictionary.Translate("Billing_Receipt");
                                                    itemDefinition.ItemName = "Billing_Receipt";
                                                }
                                                else
                                                {
                                                    itemDefinition = Persistence.ItemDefinitionById(this.ItemType, instanceName);
                                                    itemLabel = itemDefinition.Layout.Label;
                                                }
                                            }

                                            var category = this.Category ?? "NoCategory";
                                            var paramGo = string.IsNullOrEmpty(Tab) ? string.Empty : ",['T=" + Tab + "']";

                                            var onclick = string.Format(
                                                CultureInfo.InvariantCulture,
                                                "GoItemView('{0}',{1},'Custom'{2});",
                                                itemDefinition.ItemName,
                                                data[0],
                                                paramGo);

                                            if (string.IsNullOrEmpty(itemDefinition.ItemName))
                                            {
                                                onclick = "return false;";
                                            }

                                            if (!string.IsNullOrEmpty(CustomAction))
                                            {
                                                onclick = string.Format(
                                                CultureInfo.InvariantCulture,
                                                "Feature_AfterLogin_Action('{0}','{1}',{2});",
                                                CustomAction,
                                                itemDefinition.ItemName,
                                                data[0]);
                                            }

                                            res.Add(string.Format(
                                                CultureInfo.InvariantCulture,
                                                @"
                                                <tr class=""TRAlert"" data-itemDefinitionId=""{8}"" data-itemId=""{2}"" data-groups=""{9}"">
                                                    <td class=""AlertItemDesc"" style=""width:200px;"" onclick=""{10}"">{4}</td>
                                                    <td class=""AlertItemDesc"" onclick=""{10}('{3}',{2},'Custom'{6});"">{0}</td>
                                                    <td style=""width:120px;"" onclick=""{10};"">{5}</td>
                                                    <td style=""width:350px;"" data-ItemDefinitionId=""{8}"" data-itemId=""{2}"" class=""AlertItemDesc"" onclick=""{10}"">{1}
                                                </td>{7}
                                                </tr>",
                                                largeExplanation,
                                                data[1],
                                                data[0],
                                                itemDefinition.ItemName,
                                                category,
                                                itemLabel,
                                                paramGo,
                                                remindAlertCell,
                                                itemDefinition.Id,
                                                groups,
                                                onclick));
                                        }
                                        catch (Exception ex)
                                        {
                                            ExceptionManager.Trace(ex, "RenderAlert");
                                        }
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if (cmd.Connection.State != ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return new ReadOnlyCollection<string>(res);
        }

        /// <summary>Extract alert ocurrences</summary>
        /// <returns>A list of alert ocurrences</returns>
        public ReadOnlyCollection<string> Extract(string instanceName)
        {
            var res = new List<string>();
            try
            {
                var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
                var columns = new List<string>();
                foreach (var position in this.Index.OrderBy(x => x.Position))
                {
                    columns.Add(position.FieldName);
                }

                var cns = Persistence.ConnectionString(instanceName);
                if (!string.IsNullOrEmpty(cns))
                {
                    using (var cmd = new SqlCommand(this.Query))
                    {
                        using (var cnn = new SqlConnection(cns))
                        {
                            cmd.Connection = cnn;
                            cmd.CommandType = CommandType.Text;
                            try
                            {
                                cmd.Connection.Open();
                                using (var rdr = cmd.ExecuteReader())
                                {
                                    while (rdr.Read())
                                    {
                                        var data = new List<string>();
                                        foreach (var columnName in columns)
                                        {
                                            data.Add(rdr[columnName].ToString());
                                        }

                                        var shortDescription = this.AlertDescription;
                                        if (this.AlertDescription != null)
                                        {
                                            if (this.AlertDescription.StartsWith("Alert_Short_", StringComparison.OrdinalIgnoreCase))
                                            {
                                                if (dictionary.ContainsKey(this.AlertDescription))
                                                {
                                                    shortDescription = dictionary[this.AlertDescription];
                                                }
                                            }
                                        }

                                        var largeExplanation = this.AlertExplanation;
                                        if (this.AlertExplanation != null)
                                        {
                                            if (this.AlertExplanation.StartsWith("Alert_Large_", StringComparison.OrdinalIgnoreCase))
                                            {
                                                if (dictionary.ContainsKey(this.AlertExplanation))
                                                {
                                                    largeExplanation = dictionary[this.AlertExplanation];
                                                }
                                            }
                                        }

                                        // Comporbar si están vacías
                                        if (string.IsNullOrEmpty(largeExplanation))
                                        {
                                            largeExplanation = shortDescription;
                                        }

                                        if (string.IsNullOrEmpty(shortDescription))
                                        {
                                            shortDescription = largeExplanation;
                                        }

                                        var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.Id == this.ItemType);
                                        this.Icon = itemDefinition.Layout.Icon;
                                        var icon = this.Icon ?? "fa-exclamation-triangle";

                                        data.Add(shortDescription);
                                        data.Add(itemDefinition.ItemName);
                                        data.Add(icon);


                                        var pattern = @"
                                        <li onclick=""GoItemView('{3}',{0},'custom');"">
                                          <a href=""#"">
                                            <div class=""MenuAlertTitle"">{2}</div>
                                            <div class=""clearfix"">
                                              <span class=""pull-left""><i class=""red fa {4}""></i>&nbsp;{1}</span>
                                            </div>
                                          </a>
                                        </li>";

                                        res.Add(string.Format(CultureInfo.InvariantCulture, pattern, data.ToArray()));
                                    }
                                }
                            }
                            finally
                            {
                                if (cmd.Connection.State != ConnectionState.Closed)
                                {
                                    cmd.Connection.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Trace(ex, "AlertDefinition::Extract();");
                res = new List<string>();
            }

            return new ReadOnlyCollection<string>(res);
        }

        /// <summary>Extract alert ocurrences</summary>
        /// <returns>A list of alert ocurrences in JSON format</returns>
        public ReadOnlyCollection<string> ExtractJson(string instanceName)
        {
            var res = new List<string>();
            try
            {
                var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
                var columns = new List<string>();
                foreach (var position in this.Index.OrderBy(x => x.Position))
                {
                    columns.Add(position.FieldName);
                }

                var cns = Persistence.ConnectionString(instanceName);
                if (!string.IsNullOrEmpty(cns))
                {
                    using (var cmd = new SqlCommand(this.Query))
                    {
                        using (var cnn = new SqlConnection(cns))
                        {
                            cmd.Connection = cnn;
                            cmd.CommandType = CommandType.Text;
                            try
                            {
                                cmd.Connection.Open();
                                using (var rdr = cmd.ExecuteReader())
                                {
                                    while (rdr.Read())
                                    {
                                        var data = new List<string>();
                                        foreach (var columnName in columns)
                                        {
                                            data.Add(Tools.Json.JsonCompliant(rdr[columnName].ToString()));
                                        }

                                        var shortDescription = this.AlertDescription;
                                        if (this.AlertDescription != null)
                                        {
                                            if (this.AlertDescription.StartsWith("Alert_Short_", StringComparison.OrdinalIgnoreCase))
                                            {
                                                if (dictionary.ContainsKey(this.AlertDescription))
                                                {
                                                    shortDescription = dictionary[this.AlertDescription];
                                                }
                                            }
                                        }

                                        var largeExplanation = this.AlertExplanation;
                                        if (this.AlertExplanation != null)
                                        {
                                            if (this.AlertExplanation.StartsWith("Alert_Large_", StringComparison.OrdinalIgnoreCase))
                                            {
                                                if (dictionary.ContainsKey(this.AlertExplanation))
                                                {
                                                    largeExplanation = dictionary[this.AlertExplanation];
                                                }
                                            }
                                        }

                                        // Comporbar si están vacías
                                        if (string.IsNullOrEmpty(largeExplanation))
                                        {
                                            largeExplanation = shortDescription;
                                        }

                                        if (string.IsNullOrEmpty(shortDescription))
                                        {
                                            shortDescription = largeExplanation;
                                        }

                                        var itemDefinition = Persistence.ItemDefinitionById(this.ItemType, instanceName);
                                        this.Icon = itemDefinition.Layout.Icon;
                                        var icon = this.Icon ?? "fa-exclamation-triangle";

                                        data.Add(shortDescription);
                                        data.Add(itemDefinition.ItemName);
                                        data.Add(icon);
                                        data.Add(this.Importance.ToString());
                                        data.Add(this.customMenu ?? "Default");


                                        /*var pattern = @"
                                            <li onclick=""GoEncryptedView('{3}',{0},'custom');"">
                                              <a href=""#"">
                                                <div class=""MenuAlertTitle"">{2}</div>
                                                <div class=""clearfix"">
                                                  <span class=""pull-left""><i class=""red fa {4}""></i>&nbsp;{1}</span>
                                                </div>
                                              </a>
                                            </li>";*/

                                        var pattern = @"
                                        {{
                                            ""ItemId"":""{0}"",
                                            ""ItemDescription"":""{1}"",
                                            ""Text"":""{2}"",
                                            ""ItemName"":""{3}"",
                                            ""Icon"":""{4}"",
                                            ""Importance"":{5},
                                            ""CustomMenu"":""{6}""
                                        }}";

                                        res.Add(string.Format(CultureInfo.InvariantCulture, pattern, data.ToArray()));
                                    }
                                }
                            }
                            finally
                            {
                                if (cmd.Connection.State != ConnectionState.Closed)
                                {
                                    cmd.Connection.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Trace(ex, "AlertDefinition::Extract();");
                res = new List<string>();
            }

            return new ReadOnlyCollection<string>(res);
        }

        public static string GetJson(string instanceName)
        {
            var res = new StringBuilder();
            var instance = Persistence.InstanceByName(instanceName);

            if (HttpContext.Current.Session["AlertFastQuery"] == null)
            {
                var user = ApplicationUser.Actual;
                if (!instance.Config.ActiveAlerts)
                {
                    return Json.EmptyJsonList;
                }

                var show = HttpContext.Current.Session["AlertsDefinition"] as ReadOnlyCollection<AlertDefinition>;
                var alertsTags = new List<string>();

                var query = string.Empty;
                bool firstQuery = true;

                if (show == null || show.Count == 0)
                {
                    return string.Empty;
                }

                int showed = 0;
                foreach (var alertDefinition in show)
                {
                    if (!user.HasGrantToRead(alertDefinition.ItemType) && !user.AdminUser)
                    {
                        continue;
                    }

                    query += firstQuery ? string.Empty : "\n\nUNION\n\n";
                    query += "SELECT '" + alertDefinition.AlertDescription.Replace('\'', '´') + "' AS Label,";
                    query += alertDefinition.Importance.ToString() + " AS Importance,";
                    query += "'" + alertDefinition.Category + "' AS Category,";
                    query += "'" + (alertDefinition.Tab ?? string.Empty) + "' AS Tab,";
                    query += "'" + (string.IsNullOrEmpty(alertDefinition.Icon) ? "fa fa-exclamation-triangle" : alertDefinition.Icon) + "' AS Icon,";
                    query += "'" + (string.IsNullOrEmpty(alertDefinition.CustomMenu) ? "Default" : alertDefinition.CustomMenu) + "' AS CustomMenu,";
                    query += "'" + (string.IsNullOrEmpty(alertDefinition.CustomAction) ? string.Empty : alertDefinition.CustomAction) + "' AS CustomAction,";

                    if (alertDefinition.Groups.Count > 0)
                    {
                        query += "'";
                        bool fg = true;
                        foreach (var g in alertDefinition.Groups)
                        {
                            query += fg ? string.Empty : ",";
                            query += g.ToString();
                            fg = false;
                        }

                        query += "' AS Groups,";
                    }
                    else
                    {
                        query += "'' AS Groups,";
                    }

                    if (alertDefinition.ItemType == Constant.BillingItemType)
                    {
                        query += "'Billing' AS ItemName,";
                    }
                    else if (alertDefinition.ItemType == 0)
                    {
                        query += "'None' AS ItemName,";
                    }
                    else
                    {
                        query += "'" + Persistence.ItemDefinitions(instanceName).First(d => d.Id == alertDefinition.ItemType).ItemName + "' AS ItemName,";
                    }

                    query += "D.Id, REPLACE(D.Nombre, CHAR(13), '') AS Nombre FROM (" + alertDefinition.Query + ") AS D";


                    firstQuery = false;
                    showed++;
                }

                query = @"SELECT
    '{""Label"":""' + X.Label + '"",""Importance"":' + CAST(X.Importance AS nvarchar(6)) + ',""Category"":""' + X.Category + '"",' +
    '""Menu"":""' + X.CustomMenu + '"",""Groups"":[' + X.Groups + '],' +
    '""Tab"":""' + X.Tab + '"",' + 
    '""Acion"":""' + X.CustomAction + '"",' + 
    '""Nombre"":""' + X.Nombre + '"",' + 
    '""Icon"":""' + X.Icon + '"",' +
	'""ItemId"":' + CAST(x.Id as nvarchar(10)) + ',' +
    '""ItemName"":""' + X.ItemName + '""' +
    '}'

FROM
(" + query + ") as X ORDER BY X.CustomMenu, X.Importance DESC";

                if (showed > 0)
                {
                    HttpContext.Current.Session["AlertFastQuery"] = query;
                }
            }

            if (HttpContext.Current.Session["AlertFastQuery"] != null)
            {
                using (var cmd = new SqlCommand(HttpContext.Current.Session["AlertFastQuery"] as string))
                {
                    using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.Text;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                bool first = true;
                                while (rdr.Read())
                                {
                                    res.AppendFormat(
                                        CultureInfo.InvariantCulture,
                                        @"{2}{0}    {1}",
                                        Environment.NewLine,
                                        rdr.GetString(0),
                                        first ? string.Empty : ",");

                                    first = false;

                                }
                            }
                        }
                        finally
                        {
                            if (cmd.Connection.State != ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
                return res.ToString();
            }

            return Json.EmptyJsonList;
        }

        public static string GetJsonAfterLogin(string instanceName)
        {
            var instance = Persistence.InstanceByName(instanceName);
            if (!instance.Config.ActiveAlerts)
            {
                return string.Empty;
            }

            var show = HttpContext.Current.Session["AlertsDefinition"] as ReadOnlyCollection<AlertDefinition>;

            if (show == null)
            {
                return string.Empty;
            }

            var showFinal = show.Where(ad => ad.AfterLogin == true).ToList();

            if (showFinal.Count == 0)
            {
                return string.Empty;
            }

            var res = new StringBuilder();
            var query = string.Empty;
            var user = ApplicationUser.Actual;
            var alertsTags = new List<string>();

            bool firstQuery = true;
            foreach (var alertDefinition in showFinal)
            {
                if (!user.HasGrantToRead(alertDefinition.ItemType))
                {
                    continue;
                }

                query += firstQuery ? string.Empty : "\n\nUNION\n\n";
                query += "SELECT '" + alertDefinition.AlertDescription.Replace('\'', '´') + "' AS Label,";
                query += alertDefinition.Importance.ToString() + " AS Importance,";
                query += "'" + alertDefinition.Category + "' AS Category,";
                query += "'" + (string.IsNullOrEmpty(alertDefinition.Icon) ? "fa fa-exclamation-triangle" : alertDefinition.Icon) + "' AS Icon,";
                query += "'" + (string.IsNullOrEmpty(alertDefinition.CustomMenu) ? "Default" : alertDefinition.CustomMenu) + "' AS CustomMenu,";
                query += "'" + (string.IsNullOrEmpty(alertDefinition.CustomAction) ? string.Empty : alertDefinition.CustomAction) + "' AS CustomAction,";

                if (alertDefinition.Groups.Count > 0)
                {
                    query += "'";
                    bool fg = true;
                    foreach (var g in alertDefinition.Groups)
                    {
                        query += fg ? string.Empty : ",";
                        query += g.ToString();
                        fg = false;
                    }

                    query += "' AS Groups,";
                }
                else
                {
                    query += "'' AS Groups,";
                }

                query += "'" + instance.ItemDefinitions.First(d => d.Id == alertDefinition.ItemType).ItemName + "' AS ItemName,";
                query += "D.Id, D.Nombre FROM (" + alertDefinition.Query + ") AS D";
                firstQuery = false;
            }

            if (!string.IsNullOrEmpty(query))
            {
                query = @"SELECT
    '{""Label"":""' + X.Label + '"",""Importance"":' + CAST(X.Importance AS nvarchar(6)) + ',""Category"":""' + X.Category + '"",' +
    '""Menu"":""' + X.CustomMenu + '"",""Groups"":[' + X.Groups + '],' +
    '""CustomAction"":""' + X.CustomAction + '"",' + 
    '""Nombre"":""' + X.Nombre + '"",' + 
    '""Icon"":""' + X.Icon + '"",' +
	'""ItemId"":' + CAST(x.Id as nvarchar(10)) + ',' +
    '""ItemName"":""' + X.ItemName + '""' +
    '}'

FROM
(" + query + ") as X ORDER BY X.CustomMenu, X.Importance DESC";

                using (var cmd = new SqlCommand(query))
                {
                    using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.Text;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                bool first = true;
                                while (rdr.Read())
                                {
                                    res.AppendFormat(
                                        CultureInfo.InvariantCulture,
                                        @"{2}{0}    {1}",
                                        Environment.NewLine,
                                        rdr.GetString(0),
                                        first ? string.Empty : ",");

                                    first = false;

                                }
                            }
                        }
                        finally
                        {
                            if (cmd.Connection.State != ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return res.ToString();
        }
    }
}