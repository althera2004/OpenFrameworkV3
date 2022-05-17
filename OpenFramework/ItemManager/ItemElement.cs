// --------------------------------
// <copyright file="ItemElement.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using OpenFramework.Activity;
    using OpenFramework.DataAccess;
    using OpenFramework.InstanceManager;

    /// <summary>Implements ItemElement in order to handle item definition as form data</summary>
    public class ItemElement
    {
        /// <summary>Gets item definition identifier</summary>
        public long Id { get; private set; }

        /// <summary>Gets item defintion name</summary>
        public string Name { get; private set; }

        /// <summary>Gets item definition icon</summary>
        public string Icon { get; private set; }

        /// <summary>Gets ava lue indicating whether item has FAQ enabled</summary>
        public bool FAQs { get; private set; }

        /// <summary>Gets a Json structure of Item</summary>
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""Name"":""{1}"",""Icon"":""{2}"",""FAQs"":{3}}}",
                    this.Id,
                    this.Name,
                    this.Icon,
                    ConstantValue.Value(this.FAQs));
            }
        }

        /// <summary>Gets a Json list of all items</summary>
        public static string AllJson
        {
            get
            {
                var res = new StringBuilder("[");
                bool first = true;
                foreach(var item in All.Where(i=>i.FAQs == true))
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
        }

        /// <summary>Gets all items from database</summary>
        public static ReadOnlyCollection<ItemElement> All
        {
            get
            {
                var res = new List<ItemElement>();
                var instance = CustomerFramework.Actual;
                using(var cmd = new SqlCommand("Core_Item_GetAll"))
                {
                    using(var cnn  = new SqlConnection(instance.Config.ConnectionString))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new ItemElement
                                    {
                                        Id = rdr.GetInt64(0),
                                        Name = rdr.GetString(1),
                                        Icon = rdr.GetString(2),
                                        FAQs = rdr.GetBoolean(3)
                                    });
                                }
                            }
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

                return new ReadOnlyCollection<ItemElement>(res);
            }
        }

        public static ActionResult UpdateDataBase()
        {
            var res = ActionResult.NoAction;
            var instance = CustomerFramework.Actual;
            using (var cmd = new SqlCommand("Core_Item_Save"))
            {
                using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        foreach (var definition in instance.Definitions)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(DataParameter.Input("@Id", definition.Id));
                            cmd.Parameters.Add(DataParameter.Input("@Name", definition.ItemName, 50));
                            cmd.Parameters.Add(DataParameter.Input("@Icon", definition.Layout.Icon, 50));
                            cmd.Parameters.Add(DataParameter.Input("@EnableFAQ", definition.Features.FAQs));
                            cmd.ExecuteNonQuery();
                        }
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

            return res;
        }
    }
}
