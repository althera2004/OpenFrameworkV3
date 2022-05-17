// --------------------------------
// <copyright file="ItemDataBase.aspx.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV2.Web.ViuLleure
{
    using System;
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
    using OpenFrameworkV2;
    using OpenFrameworkV2.Core.DataAccess;
    using OpenFrameworkV2.Tools;
    using OpenFrameworkV2.Core.ItemManager.ItemList;
    using OpenFrameworkV2.Core;
    using OpenFrameworkV2.Core.ItemManager;

    /// <summary>Database actions for ViuLleure instance</summary>
    [WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ScriptService]
	public partial class ItemDataBase : Page
	{
		/// <summary>ViuLleure instance</summary>
		private Instance instance;

		/// <summary>Page load event</summary>
		/// <param name="sender">This page</param>
		/// <param name="e">Event arguments</param>
		protected void Page_Load(object sender, EventArgs e)
		{
            Instance.CheckPersistence();
			if (this.Request.QueryString["Action"] != null)
			{
				this.GoAction();
			}

			if (this.Request.QueryString["GetScript"] != null)
			{
				this.GoScript();
			}
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
				default:
					if (this.Request.QueryString["params"] == null)
					{
						res = SqlStream.GetSqlStreamNoParams(action, this.instance.Config.ConnectionString);
					}
					else
					{
						//var parameters = QueryParameter.FromString(this.Request.QueryString["params"]);
						//res = SqlStream.GetSqlStream(action, new ReadOnlyCollection<SqlParameter>(parameters), this.instance.Config.ConnectionString);
					}

					break;
			}

			var d1 = DateTime.Now;
			this.Response.Clear();
			this.Response.Write(@"{""data"":");
			this.Response.Write(res);
			var d2 = DateTime.Now;
			this.Response.Write(string.Format(CultureInfo.GetCultureInfo("en-us"), @",""Time"":""{0:#,##0} - {1:#,##0}""}}", (d1 - d0).TotalMilliseconds, (d2 - d1).TotalMilliseconds));
			this.Response.Flush();
			this.Response.SuppressContent = true;
			this.ApplicationInstance.CompleteRequest();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ActionResult CasalGrupo_SetAsignacion(long grupoCasalId, string asistentes, long companyId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Item_CasalGrupo_SetAsignacion 
             *   @CasalGrupoId bigint,
             *   @Data nchar(200),
             *   @CompanyId bigint */
            using (var cmd = new SqlCommand("Item_CasalGrupo_SetAsignacion"))
            {
                using (var cnn = new SqlConnection(Persistence.ConnectionString(Instance.InstanceName)))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CasalGrupoId", grupoCasalId));
                    cmd.Parameters.Add(DataParameter.Input("@Data", asistentes.Replace("A_", string.Empty), 200));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
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

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ActionResult INSCRIPCION_CambiarActividad(long inscripcionId, long casalId, long applicationUserId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Item_CasalGrupo_SetAsignacion 
             *   @CasalGrupoId bigint,
             *   @Data nchar(200),
             *   @CompanyId bigint */
            using(var cmd = new SqlCommand("Item_Inscripcion_CambiarActividad"))
            {
                using(var cnn = new SqlConnection(Persistence.ConnectionString(Instance.InstanceName)))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@InscripcionId", inscripcionId));
                    cmd.Parameters.Add(DataParameter.Input("@CasalId", casalId));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch(Exception ex)
                    {
                        res.SetFail(ex);
                    }
                    finally
                    {
                        if(cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return res;
        }
    }
}	