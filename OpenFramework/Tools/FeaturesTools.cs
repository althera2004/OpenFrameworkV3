// --------------------------------
// <copyright file="FeaturesTools.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.Tools
{
    using System.Data;
    using System.Data.SqlClient;
    using OpenFrameworkV3.Core.DataAccess;

    /// <summary>Framework tools</summary>
    public static class FeaturesTools
    {
        public static bool ItemIsUnloaded(string itemName, long itemId, string connectionString)
        {
            /* CREATE PROCEDURE Feature_Unloaded
             *   @ItemId bigint,
             *   @ItemName nvarchar(50) */
            bool res = false;
            using (var cmd = new SqlCommand("Feature_Unloaded"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@ItemName", itemName));
                cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                using (var cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            res = rdr.HasRows;
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

            return res;
        }

        public static bool ItemIsFollowed(long applicationUserId, long itemTypeId, long itemId, string connectionString)
        {
            bool res = false;
            using (var cmd = new SqlCommand("Core_User_ItemIsFollowed"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                cmd.Parameters.Add(DataParameter.Input("@ItemTypeId", itemTypeId));
                cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                using (var cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            res = rdr.HasRows;
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

            return res;
        }
    }
}