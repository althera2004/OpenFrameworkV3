// --------------------------------
// <copyright file="BlockAction.cs" company="OpenFramework">
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
    using System.Text;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;

    public partial class BlockAction
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long ApplicationUserId { get; set; }
        public long ItemDefinitionId { get; set; }
        public long ItemId { get; set; }
        public long ActionId { get; set; }
        public string Reason { get; set; }
        public string Comments { get; set; }

        public static BlockAction Empty
        {
            get
            {
                return new BlockAction
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    ApplicationUserId = Constant.DefaultId,
                    ItemDefinitionId = Constant.DefaultId,
                    ItemId = Constant.DefaultId,
                    ActionId = Constant.DefaultId,
                    Reason = string.Empty,
                    Comments = string.Empty
                };
            }
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""CompanyId"":{1},""ApplicationUserId"":{2},""ItemDefinitionId"":{3},""ItemId"":{4},""ActionId"":{5},""Reason"":""{6}"",""Comments"":""{7}""}}",
                    this.Id,
                    this.CompanyId,
                    this.ApplicationUserId,
                    this.ItemDefinitionId,
                    this.ItemId,
                    this.ActionId,
                    Tools.Json.JsonCompliant(this.Reason),
                    Tools.Json.JsonCompliant(this.Comments));
            }
        }

        public string JsonList(ReadOnlyCollection<BlockAction> list)
        {
            var res = new StringBuilder("[");
            if(list != null)
            {
                if(list.Count > 0)
                {
                    var first = true;
                    foreach(var item in list)
                    {
                        res.AppendFormat(
                            CultureInfo.InvariantCulture,
                            "{0}{1}{2}",
                            first ? string.Empty : ",",
                            Environment.NewLine,
                            item.Json);


                        first = false;
                    }
                }
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<BlockAction> ByApplicationUser(long applicationUserId, long companyId, string instanceName)
        {
            var res = new List<BlockAction>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_BlockActions_ByUserId"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new BlockAction
                                    {
                                        Id = rdr.GetInt64(ColumnBlockActionGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnBlockActionGet.CompanyId),
                                        ApplicationUserId = rdr.GetInt64(ColumnBlockActionGet.ApplicationUserId),
                                        ItemDefinitionId = rdr.GetInt64(ColumnBlockActionGet.ItemDefinitionId),
                                        ItemId = rdr.GetInt64(ColumnBlockActionGet.ItemId),
                                        ActionId = rdr.GetInt64(ColumnBlockActionGet.ActionId),
                                        Reason = rdr.GetString(ColumnBlockActionGet.Reason),
                                        Comments = rdr.GetString(ColumnBlockActionGet.Comments)
                                    });
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

            return new ReadOnlyCollection<BlockAction>(res);
        }

        public static bool UserIsBlocked(long applicationUserId, long companyId, string instanceName)
        {
            var res = false;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_BlockActions_UserIsBlocked"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    if (rdr.GetInt32(0) > 0)
                                    {
                                        res = true;
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

            return res;
        }

        public static ActionResult Inacivate(long applicationUserId, long companyId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_BlockActions_Inactivate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
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
            }

            return res;
        }

        public ActionResult Insert(string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_BlockActions_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", this.ApplicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@ActionId", this.ActionId));
                        cmd.Parameters.Add(DataParameter.Input("@Reason", this.Reason, 500));
                        cmd.Parameters.Add(DataParameter.Input("@Comments", this.Comments, 500));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.ItemId = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(this.Id);
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
    }
}