// --------------------------------
// <copyright file="ItemDataBase.aspx.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Web.Instances.Support
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using OpenFrameworkV3.Core;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Tools;

    public partial class ItemDataBase : Page
    {
        private string instanceName;

        private long companyId;

        /// <summary>Page load event</summary>
        /// <param name="sender">This page</param>
        /// <param name="e">Event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.instanceName = this.Request.QueryString["I"];
            this.companyId = Convert.ToInt64(this.Request.QueryString["C"]);

            if (this.Request.QueryString["Action"] != null)
            {
                this.GoAction();
            }

            if (this.Request.QueryString["GetScript"] != null)
            {
                this.GoScript();
            }
        }

        private string GetList()
        {
            var listId = this.Request.QueryString["ListId"];
            var itemName = this.Request.QueryString["ItemName"];
            var item = Persistence.ItemDefinitionByName(itemName, this.instanceName);
            var parameters = Request.QueryString.Keys.Cast<string>().ToDictionary(k => k, v => Request.QueryString[v]);
            parameters = parameters.Where(p => p.Key != "ApplicationUserId" && p.Key != "_" && p.Key != "Action" && p.Key != "ItemName" && p.Key != "ListId").ToDictionary(v => v.Key, v => v.Value);
            var query = Query.ByListId(item.ItemName, parameters, listId, this.companyId, this.instanceName);
            return SqlStream.ByQuery(query, Persistence.ConnectionString(this.instanceName));
        }

        /// <summary>Gets JavaScript variable</summary>
        private void GoScript()
        {
            string res = string.Empty;
            string action = this.Request.QueryString["GetScript"].Trim().ToUpperInvariant();
            string variable = this.Request.QueryString["var"].Trim();
            switch (action)
            {
                default:
                    res = Constant.JavaScriptNull;
                    break;
            }

            this.Response.Clear();
            this.Response.Write(string.Format(CultureInfo.InvariantCulture, "var {0} = {1}; ", variable, res));
            this.Response.Flush();
            this.Response.SuppressContent = true;
            this.ApplicationInstance.CompleteRequest();
        }

        /// <summary>Gets JSON array</summary>
        private void GoAction()
        {
            var d0 = DateTime.Now;
            string res = string.Empty;
            string action = this.Request.QueryString["Action"].Trim().ToUpperInvariant();

            switch (action)
            {
                case "GETLIST":
                    res = this.GetList();
                    break;
                default:
                    var connectionString = Persistence.ConnectionString(this.instanceName);
                    if (this.Request.QueryString["params"] == null)
                    {
                        res = SqlStream.GetSqlStreamNoParams(action, connectionString);
                    }
                    else
                    {
                        var parameters = QueryParameter.FromString(this.Request.QueryString["params"]);
                        res = SqlStream.GetSqlStream(action, new ReadOnlyCollection<SqlParameter>(parameters), connectionString);
                    }

                    break;
            }

            var d1 = DateTime.Now;
            this.Response.Clear();
            this.Response.Write(@"{""data"":");
            this.Response.Write(res.Replace("\n", "\\n"));
            var d2 = DateTime.Now;

            if (this.Request.QueryString["listId"] != null)
            {
                this.Response.Write(string.Format(CultureInfo.InvariantCulture, @", ""ListId"":""{0}""", this.Request.QueryString["listId"]));
            }

            if (this.Request.QueryString["itemName"] != null)
            {
                this.Response.Write(string.Format(CultureInfo.InvariantCulture, @", ""ItemName"":""{0}""", this.Request.QueryString["itemName"]));
            }

            this.Response.Write(string.Format(CultureInfo.GetCultureInfo("en-us"), @",""Time"":""{0:#,##0} - {1:#,##0}""}}", (d1 - d0).TotalMilliseconds, (d2 - d1).TotalMilliseconds));
            this.Response.Flush();
            this.Response.SuppressContent = true;
            this.ApplicationInstance.CompleteRequest();
        }
    }
}