// --------------------------------
// <copyright file="WhatYouMissed.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Feature
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;

    public class WhatYouMissed
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long WYMId { get; set; }
        public long ItemDefintionId { get; set; }
        public long ItemId { get; set; }
        public DateTime WhenHappened { get; set; }
        public string Subject { get; set; }

        public ActionResult SetLastView(long userId, long companyId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_WhatYouMissed_SetLastView
             *   @UserId bigint,
             *   @CompanyId bigint,
             *   @LastViewDate datetime */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_WhatYouMissed_SetLastView"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@LastDateView", DateTime.Now.Date));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
                        }
                        catch (SqlException ex)
                        {
                            res.SetFail(ex);
                        }
                        catch (NullReferenceException ex)
                        {
                            res.SetFail(ex);
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
            }

            return res;
        }

        public static DateTime LastView(long userId, long companyId, string instanceName)
        {
            var res = DateTime.MinValue;
            /* CREATE PROCEDURE Core_WhatYouMissed_ByUserCompany
             *   @UserId bigint,
             *   @CompanyId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_WhatYouMissed_ByUserCompany"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    res = rdr.GetDateTime(0);
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

            return res;
        }

        public ActionResult Insert(string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Feature_WhatYouMissed_Insert
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @WYMId bigint,
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @WhenHappened datetime,
             *   @Subject nvarchar(4000) */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_WhatYouMissed_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@WYMId", this.WYMId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefintionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@WhenHappened", this.WhenHappened));
                        cmd.Parameters.Add(DataParameter.Input("@Subject", this.Subject, 4000));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(this.Id);
                        }
                        catch (Exception ex)
                        {
                            res.SetFail(ex);
                            ExceptionManager.Trace(ex, "WhatYouMissed::Insert");
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

            return res;
        }
    }
}
