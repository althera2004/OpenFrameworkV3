// --------------------------------
// <copyright file="WhatYouMissedData.cs" company="OpenFramework">
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
    using System.Linq;
    using System.Text;
    using System.Web;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;

    public class WhatYouMissedData
    {
        public string ItemName { get; set; }
        public long ItemDefinitionId { get; set; }
        public long ItemId { get; set; }
        public string ItemDescripcion { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Data { get; set; }
        public string Groups { get; set; }
        public bool Notify { get; set; }
        public string Category { get; set; }

        public string JsonList(ReadOnlyCollection<WhatYouMissedData> list)
        {
            var res = new StringBuilder("[");
            var first = false;
            foreach (var item in list)
            {
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "{0}{1}",
                    first ? string.Empty : ",",
                    item.Json);

                first = false;
            }

            res.Append("]");
            return res.ToString();
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""ItemName"":""{0}"", ""ItemId"":{1}, ""ItemDescription"":""{2}"", ""Type"":""{3}"", ""Date"":""{4:dd/MM/yyyy}"", ""Data"":""{5}""}}",
                    this.ItemName,
                    this.ItemId,
                    Tools.Json.JsonCompliant(this.ItemDescripcion),
                    this.Type,
                    this.Date,
                    Tools.Json.JsonCompliant(this.Data));
            }
        }

        public static ReadOnlyCollection<WhatYouMissedData> ByUserId(long userId, long companyId, string instanceName)
        {
            var res = new List<WhatYouMissedData>();
            using (var instance = Persistence.InstanceByName(instanceName))
            {
                if (instance.Config.WhatYouMissed)
                {
                    if (HttpContext.Current.Session["WhatYouMissedDefinition"] != null && HttpContext.Current.Session["WhatYouMissedQuery"] == null)
                    {
                        ReadOnlyCollection<WhayYouMissedDeninition> definitions = HttpContext.Current.Session["WhatYouMissedDefinition"] as ReadOnlyCollection<WhayYouMissedDeninition>;
                        if (definitions != null && definitions.Count > 0)
                        {
                            var query = new StringBuilder();
                            bool first = true;
                            foreach (var definition in definitions)
                            {

                                if (first)
                                {
                                    first = false;
                                }
                                else
                                {
                                    query.AppendFormat(CultureInfo.InvariantCulture, "{0}{0} UNION {0}{0}", Environment.NewLine);
                                }

                                var definitionQuery = "SELECT " + (definition.Notify ? "1" : "0") + " AS Notify,";
                                definitionQuery += "'" + definition.Category + "' AS Category,";

                                var itemDefinitionName = "weke";
                                if (definition.ItemDefinitionId == 10003)
                                {
                                    itemDefinitionName = "S.C.";
                                }
                                else if (instance.ItemDefinitions.Any(d => d.Id == definition.ItemDefinitionId))
                                {
                                    itemDefinitionName = instance.ItemDefinitions.First(d => d.Id == definition.ItemDefinitionId).Layout.Label;
                                }

                                definitionQuery += "'" + itemDefinitionName + "' AS ItemName,";
                                definitionQuery += definition.ItemDefinitionId + " AS ItemDefinitionId,";

                                if (definition.Groups.Count > 0)
                                {
                                    definitionQuery += "'";
                                    bool firstGroup = true;
                                    foreach (var group in definition.Groups)
                                    {
                                        if (firstGroup)
                                        {
                                            firstGroup = false;
                                        }
                                        else
                                        {
                                            definitionQuery += ",";
                                        }

                                        definitionQuery += group.ToString();
                                    }
                                    definitionQuery += "' AS Groups,";
                                }
                                else
                                {
                                    definitionQuery += "null AS Groups,";
                                }

                                definitionQuery += definition.Query.Replace("SELECT", string.Empty);

                                query.Append(definitionQuery);
                            }

                            HttpContext.Current.Session["WhatYouMissedQuery"] = query.ToString();
                        }
                    }

                    var notShow = new List<NotShowIndex>();
                    using (var cmd2 = new SqlCommand("Core_WhatYouMissed_NotShow"))
                    {
                        using (var cnn2 = new SqlConnection(instance.Config.ConnectionString))
                        {
                            cmd2.Connection = cnn2;
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.Add(DataParameter.Input("@UserId", userId));
                            cmd2.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                            try
                            {
                                cmd2.Connection.Open();
                                using (var rdr = cmd2.ExecuteReader())
                                {
                                    while (rdr.Read())
                                    {
                                        notShow.Add(new NotShowIndex { ItemId = rdr.GetInt64(0), ItemDefinitonId = rdr.GetInt64(1) });
                                    }
                                }
                            }
                            finally
                            {
                                if (cmd2.Connection.State != ConnectionState.Closed)
                                {
                                    cmd2.Connection.Close();
                                }
                            }
                        }

                        var query = HttpContext.Current.Session["WhatYouMissedQuery"] as string;
                        if (!string.IsNullOrEmpty(query))
                        {
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
                                            while (rdr.Read())
                                            {
                                                var itemDefinitionId = Convert.ToInt64(rdr.GetInt32(3));
                                                var itemId = rdr.GetInt64(5);

                                                var show = true;
                                                foreach (var notShowitem in notShow)
                                                {
                                                    if (notShowitem.ItemDefinitonId == itemDefinitionId && notShowitem.ItemId == itemId)
                                                    {
                                                        show = false;
                                                        break;
                                                    }
                                                }

                                                if (show)
                                                {
                                                    var groups = string.Empty;
                                                    if (!rdr.IsDBNull(4))
                                                    {
                                                        groups = rdr.GetString(4);
                                                    }

                                                    if (rdr.IsDBNull(8))
                                                    {
                                                        continue;
                                                    }
                                                    var newWhatYouMissedData = new WhatYouMissedData
                                                    {
                                                        Notify = rdr.GetInt32(0) == 1,
                                                        Category = rdr.GetString(1),
                                                        ItemName = rdr.GetString(2),
                                                        ItemDefinitionId = itemDefinitionId,
                                                        Groups = groups,
                                                        ItemId = itemId,
                                                        ItemDescripcion = rdr.GetString(7),
                                                        Date = rdr.GetDateTime(8),
                                                        Data = string.Empty
                                                    };

                                                    if (!rdr.IsDBNull(9))
                                                    {
                                                        newWhatYouMissedData.Data = rdr.GetString(9);
                                                    }

                                                    res.Add(newWhatYouMissedData);
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
                    }

                    /* CREATE PROCEDURE Feature_WhatYouMissed_ByUser
                     *   @UserId bigint,
                     *   @CompanyId bigint */
                    /*using (var cmd = new SqlCommand("Feature_WhatYouMissed_ByUser"))
                    {
                        using (var cnn = new SqlConnection(instance.Config.ConnectionString))
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
                                    while (rdr.Read())
                                    {
                                        var itemId = rdr.GetInt64(1);
                                        res.Add(new WhatYouMissedData
                                        {
                                            ItemName = rdr.GetString(0),
                                            ItemId = itemId,
                                            ItemDescripcion = rdr.GetString(2),
                                            Type = rdr.GetString(3),
                                            Date = rdr.GetDateTime(4),
                                            Data = rdr.GetString(5)
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
                    }*/
                }
            }

            return new ReadOnlyCollection<WhatYouMissedData>(res);
        }

        public static ActionResult NotShow(long itemDefintionId, long itemId, long companyId, string instanceName)
        {
            var user = ApplicationUser.Actual;
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_WhatYouMissed_SetNotShow"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@UserId", user.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefintionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));

                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(string.Format(CultureInfo.InvariantCulture, "{0}_{1}", itemDefintionId, itemId));
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

        private struct NotShowIndex
        {
            public long ItemDefinitonId { get; set; }
            public long ItemId { get; set; }
        }
    }
}