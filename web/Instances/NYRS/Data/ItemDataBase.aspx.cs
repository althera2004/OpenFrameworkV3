// --------------------------------
// <copyright file="ItemDataBase.aspx.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.Instance.ViuLleure
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using System.Web.UI;
    using OpenFrameworkV3;
    using OpenFrameworkV3.Core;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Companies;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Tools;

    /// <summary>Database actions for ViuLleure instance</summary>
    [WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ScriptService]
	public partial class ItemDataBase : Page
	{
		private const string InstanceName = "nyrs";

		/// <summary>ViuLleure instance</summary>
		private Instance instance;

		/// <summary>Page load event</summary>
		/// <param name="sender">This page</param>
		/// <param name="e">Event arguments</param>
		protected void Page_Load(object sender, EventArgs e)
		{
            this.instance = Persistence.InstanceByName("nyrs");

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
			var res = new StringBuilder();
			var listId = this.Request.QueryString["ListId"];
			var itemName = this.Request.QueryString["ItemName"];
			var parameters = Request.QueryString.Keys.Cast<string>().ToDictionary(k => k, v => Request.QueryString[v]);
			parameters = parameters.Where(p => p.Key != "ApplicationUserId" && p.Key != "_" && p.Key != "Action" && p.Key != "ItemName" && p.Key != "ListId").ToDictionary(v => v.Key, v => v.Value);
			var company = HttpContext.Current.Session["Company"] as Company;
			var query = Query.ByListId(itemName, parameters, listId, company.Id, InstanceName);
			return SqlStream.ByQuery(query, Persistence.ConnectionString(InstanceName));
		}

		/// <summary>Gets JavaScript variable</summary>
		private void GoScript()
		{
			string res = string.Empty;
			string action = this.Request.QueryString["GetScript"].Trim().ToUpperInvariant();
			string variable = this.Request.QueryString["var"].Trim();
			switch (action)
			{
			}

			this.Response.Clear();
			this.Response.Write("var " + variable + "=");
			this.Response.Write(res);
			this.Response.Write(";");
			this.Response.Flush();
			this.Response.SuppressContent = true;
			//this.ApplicationInstance.CompleteRequest();
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
					if (this.Request.QueryString["params"] == null)
					{
						res = SqlStream.GetSqlStreamNoParams(action, this.instance.Config.ConnectionString);
					}
					else
					{
						var parameters = QueryParameter.FromString(this.Request.QueryString["params"]);
						res = SqlStream.GetSqlStream(action, new ReadOnlyCollection<SqlParameter>(parameters), this.instance.Config.ConnectionString);
					}

					break;
			}

			var d1 = DateTime.Now;
			this.Response.Clear();
			this.Response.Write(@"{""data"":");
			this.Response.Write(res);
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