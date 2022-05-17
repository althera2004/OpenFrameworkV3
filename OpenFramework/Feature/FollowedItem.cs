// --------------------------------
// <copyright file="FollowedItem.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
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
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.ItemManager;
    using OpenFrameworkV3.Tools;

    public class FollowedItem
    {
        public long ItemTypeId { get; set; }
        public long ItemId { get; set; }
        public string Link { get; private set; }
        public string ItemName { get; private set; }
        public string Icon { get; private set; }
        public string Description { get; private set; }

        public string InstanceName { get; private set; }

        public static FollowedItem FromDynamic(string instanceName, string itemName, dynamic data)
        {
            var res = new FollowedItem();
            if (data != null)
            {
                var values = (IDictionary<string, object>)data;
                res.Description = data["Description"];
            }

            return res;
        }

        /// <summary>Obtains item description based on identifier</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="definition">Item definition</param>
        /// <param name="instanceName">Database connection string</param>
        /// <returns>Gets item description</returns>
        public static string DescriptionById(long itemId, string itemName, string instanceName)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"Read::ById({0},{1},{2}", itemId, itemName, instanceName);
            var query = new StringBuilder("SELECT Id,CompanyId,CONCAT(");
            bool first = true;

            var definition = Persistence.ItemDefinitionByName(itemName, instanceName);
            foreach (var field in definition.Layout.Description.Fields)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    query.Append(",");
                }

                if (!definition.ForeignValues.Any(l => !string.IsNullOrEmpty(l.LinkField) && l.LocalName == field.Name))
                {
                    query.AppendFormat(CultureInfo.InvariantCulture, @"[{0}],' '", field.Name);
                }
            }

            query.AppendFormat("), Active FROM ITEM_{0} WITH(NOLOCK) WHERE Id = {1}", definition.ItemName, itemId);

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand(query.ToString()))
                {
                    cmd.CommandType = CommandType.Text;
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    return rdr.GetString(2).Trim();
                                }
                            }
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (NotSupportedException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
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

            return string.Empty;
        }

        public void FullData()
        {

            var definition = Persistence.ItemDefinitionById(this.ItemTypeId, this.InstanceName);
            if (definition != null)
            {
                this.Description = DescriptionById(this.ItemId, definition.ItemName, Persistence.ConnectionString(this.InstanceName));
                this.ItemName = definition.ItemName;
                this.Icon = definition.Layout.Icon;
                var query = Basics.Base64Encode(string.Format(CultureInfo.InvariantCulture, "&itemTypeId={0}&itemId={1}&formId=Custom", this.ItemName, this.ItemId));
                var link = string.Format(CultureInfo.InvariantCulture, "/PageView.aspx?{0}", query);
                this.Link = link;
            }
        }

        /// <summary>Gets a JSON structure of item definition followed</summary>
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""ItemTypeId"":{0},""ItemName"":""{1}"",""ItemId"":{2},""Description"":""{3}"",""Link"":""{4}"",""Icon"":""{5}""}}",
                    this.ItemTypeId,
                    this.ItemName,
                    this.ItemId,
                    Tools.Json.JsonCompliant(this.Description),
                    Tools.Json.JsonCompliant(this.Link),
                    this.Icon);
            }
        }

        /// <summary>Gets a JSON list of item defnition followed</summary>
        /// <param name="list">List of item defintions</param>
        /// <returns>A JSON list of item defnition followed</returns>
        public static string ListJson(ReadOnlyCollection<FollowedItem> list)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach(var item in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(item.Json);
            }

            res.Append("]");
            return res.ToString();
        }
        
        /// <summary>Followed items by application user id</summary>
        /// <param name="applicationUserId"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<FollowedItem> ByApplicationUser(long applicationUserId, string instanceName)
        {
            var res = new List<FollowedItem>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_User_FollowedItems"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new FollowedItem
                                    {
                                        ItemTypeId = rdr.GetInt64(0),
                                        ItemId = rdr.GetInt64(1)
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

            foreach(var items in res)
            {
                items.FullData();
            }

            return new ReadOnlyCollection<FollowedItem>(res);
        }
    }
}