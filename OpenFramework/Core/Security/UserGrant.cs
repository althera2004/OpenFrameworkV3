// --------------------------------
// <copyright file="UserGrant.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Security
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
    using OpenFrameworkV3.Core.Bindings;
    using OpenFrameworkV3.Core.DataAccess;

    /// <summary>Implments user grants</summary>
    public partial class UserGrant
    {
        /// <summary>Gets an empty user grant</summary>
        public static UserGrant Empty
        {
            get
            {
                return new UserGrant
                {
                    ItemId = Constant.DefaultId,
                    SecurityGroupId = Constant.DefaultId,
                    ApplicationUserId = Constant.DefaultId,
                    Grants = string.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    ItemName = string.Empty
                };
            }
        }

        /// <summary>Gets or sets the group idetifier</summary>
        public long SecurityGroupId { get; set; }

        /// <summary>Gets or sets the user idetifier</summary>
        public long ApplicationUserId { get; set; }

        /// <summary>Gets or sets the item that is affected by grant</summary>
        public long ItemId { get; set; }

        /// <summary>Gets or sets the item that is affected by grant</summary>
        public string ItemName { get; set; }

        /// <summary>Gets or sets the item grants by code</summary>
        public string Grants { get; set; }

        /// <summary>Gets or sets the usear that creates grant</summary>
        public ApplicationUser CreatedBy { get; set; }

        /// <summary>Gets or sets the date of grant creationd</summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>Gets or sets the usear that modifies grant</summary>
        public ApplicationUser ModifiedBy { get; set; }

        /// <summary>Gets or sets the date of last grant modification</summary>
        public DateTime ModifiedOn { get; set; }

        public static string CombineGrants(string grants1, string grants2)
        {
            return new string((grants1 + grants2).ToCharArray().Distinct().ToArray());
        }

        /// <summary>Gets JSON structure of an user grant</summary>
        public string Json
        {
            get
            {
                return string.Format(
                        CultureInfo.InvariantCulture,
                        @"            {{""ItemId"":{0},""ItemName"":""{1}"", ""Grants"": ""{2}""}}",
                        this.ItemId,
                        this.ItemName,
                        this.Grants);
            }
        }

        /// <summary>Obtains grants for user</summary>
        /// <param name="applicationUserId">User identifier</param>
        /// <param name="connectionString">String for database connection</param>
        /// <returns>List of user grants</returns>
        public static ReadOnlyCollection<UserGrant> ByUser(long applicationUserId, string connectionString)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "UserGrant::ByUser({0}, {1})", applicationUserId, connectionString);
            var res = new List<UserGrant>();
            using (var cmd = new SqlCommand("Core_User_GetGrants"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                using (var cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new UserGrant
                                {
                                    SecurityGroupId = rdr.GetInt64(ColumnsGrantGet.GroupId),
                                    ApplicationUserId = rdr.GetInt64(ColumnsGrantGet.ApplicationUserId),
                                    Grants = rdr.GetString(ColumnsGrantGet.Grants),
                                    ItemName = rdr.GetString(ColumnsGrantGet.ItemName),
                                    ItemId = rdr.GetInt64(ColumnsGrantGet.ItemId),
                                    CreatedBy = ApplicationUser.Empty,
                                    CreatedOn = DateTime.Now,
                                    ModifiedBy = ApplicationUser.Empty,
                                    ModifiedOn = DateTime.Now
                                });
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

            return new ReadOnlyCollection<UserGrant>(res);
        }

        public static ReadOnlyCollection<UserGrant> ForGroups(string connectionString, ReadOnlyCollection<Group> groups)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "UserGrant::ForGroups({0})", connectionString);
            var res = new List<UserGrant>();
            using (var cmd = new SqlCommand("Core_SecurityGroups_GetGrants"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                long securityGroupId = rdr.GetInt64(ColumnsGrantGet.GroupId);
                                long itemId = rdr.GetInt64(ColumnsGrantGet.ItemId);

                                if (groups.Any(gr => gr.Id == securityGroupId))
                                {

                                    var grant = new UserGrant
                                    {
                                        SecurityGroupId = securityGroupId,
                                        ApplicationUserId = rdr.GetInt64(ColumnsGrantGet.ApplicationUserId),
                                        Grants = rdr.GetString(ColumnsGrantGet.Grants),
                                        ItemName = rdr.GetString(ColumnsGrantGet.ItemName),
                                        ItemId = itemId,
                                        CreatedBy = ApplicationUser.Empty,
                                        CreatedOn = DateTime.Now,
                                        ModifiedBy = ApplicationUser.Empty,
                                        ModifiedOn = DateTime.Now
                                    };

                                    if (!res.Any(g => g.ItemId == itemId && g.SecurityGroupId == securityGroupId))
                                    {
                                        res.Add(grant);
                                    }
                                    else
                                    {
                                        foreach (var g in res)
                                        {
                                            if (g.ItemId == itemId && g.SecurityGroupId == securityGroupId)
                                            {
                                                g.Grants = UserGrant.CombineGrants(g.Grants, grant.Grants);
                                            }
                                        }
                                    }
                                }
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

            return new ReadOnlyCollection<UserGrant>(res);
        }

        /// <summary>Obtains grants for security group</summary>
        /// <param name="groupId">Security group identifier</param>
        /// <param name="connectionString">String for database connection</param>
        /// <returns>List of security group grants</returns>
        public static ReadOnlyCollection<UserGrant> ByGroup(long groupId, string connectionString)
        {
            /* CREATE PROCEDURE [dbo].[Core_SecurityGroup_GetGrants]
             *   @SecurityGroupId bigint */
            var source = string.Format(CultureInfo.InvariantCulture, "UserGrant::ByGroup({0}, {1})", groupId, connectionString);
            var res = new List<UserGrant>();
            using (var cmd = new SqlCommand("Core_SecurityGroup_GetGrants"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@SecurityGroupId", groupId));
                using (var cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new UserGrant
                                {
                                    SecurityGroupId = rdr.GetInt64(ColumnsGrantGet.GroupId),
                                    ApplicationUserId = rdr.GetInt64(ColumnsGrantGet.ApplicationUserId),
                                    Grants = rdr.GetString(ColumnsGrantGet.Grants),
                                    ItemName = rdr.GetString(ColumnsGrantGet.ItemName),
                                    ItemId = rdr.GetInt64(ColumnsGrantGet.ItemId),
                                    CreatedBy = ApplicationUser.Empty,
                                    CreatedOn = DateTime.Now,
                                    ModifiedBy = ApplicationUser.Empty,
                                    ModifiedOn = DateTime.Now
                                });
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

            return new ReadOnlyCollection<UserGrant>(res);
        }

        /// <summary>Gets a JSON list from a list of grants</summary>
        /// <param name="list">List of grants</param>
        /// <returns>JSON list from a list of grants</returns>
        public static string JsonList(ReadOnlyCollection<UserGrant> list)
        {
            if (list == null)
            {
                return Tools.Json.EmptyJsonList;
            }

            var result = new StringBuilder("[");
            bool first = true;
            foreach (var grant in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    result.Append(",");
                }

                result.Append(Environment.NewLine);
                result.Append("\t\t\t");
                result.Append(grant.Json);
            }

            result.Append(Environment.NewLine).Append("\t\t").Append("]");
            return result.ToString();
        }

        /// <summary>Determine if user has read grant</summary>
        /// <param name="grants">Collection of user grants</param>
        /// <param name="itemId">Grant to examine</param>
        /// <returns>Return if user has read grant</returns>
        public static bool HasReadGrant(ReadOnlyCollection<UserGrant> grants, long itemId)
        {
            if (grants == null)
            {
                return false;
            }

            foreach (var g in grants)
            {
                if (itemId == g.ItemId)
                {
                    return g.Grants.Contains("R");
                }
            }

            return false;
        }

        /// <summary>Determine if user has write grant</summary>
        /// <param name="grants">Collection of user grants</param>
        /// <param name="itemId">Grant to examine</param>
        /// <returns>Return if user has write grant</returns>
        public static bool HasWriteGrant(ReadOnlyCollection<UserGrant> grants, long itemId)
        {
            if (grants == null)
            {
                return false;
            }

            foreach (var g in grants)
            {
                if (itemId == g.ItemId)
                {
                    return g.Grants.Contains("W");
                }
            }

            return false;
        }

        /// <summary>Determine if user has delete grant</summary>
        /// <param name="grants">Collection of user grants</param>
        /// <param name="itemId">Grant to examine</param>
        /// <returns>Return if user has delete grant</returns>
        public static bool HasDeleteGrant(ReadOnlyCollection<UserGrant> grants, long itemId)
        {
            if (grants == null)
            {
                return false;
            }

            foreach (var g in grants)
            {
                if (itemId == g.ItemId)
                {
                    return g.Grants.Contains("D");
                }
            }

            return false;
        }

        /// <summary>Determine if user has FAQs grant</summary>
        /// <param name="grants">Collection of user grants</param>
        /// <param name="itemId">Grant to examine</param>
        /// <returns>Return if user has delete grant</returns>
        public static bool HasFAQSGrant(ReadOnlyCollection<UserGrant> grants, long itemId)
        {
            if (grants == null)
            {
                return false;
            }

            foreach (var g in grants)
            {
                if (itemId == g.ItemId)
                {
                    return g.Grants.Contains("F");
                }
            }

            return false;
        }

        /// <summary>Determine if user has News grant</summary>
        /// <param name="grants">Collection of user grants</param>
        /// <param name="itemId">Grant to examine</param>
        /// <returns>Return if user has delete grant</returns>
        public static bool HasNewsGrant(ReadOnlyCollection<UserGrant> grants, long itemId)
        {
            if (grants == null)
            {
                return false;
            }

            foreach (var g in grants)
            {
                if (itemId == g.ItemId)
                {
                    return g.Grants.Contains("N");
                }
            }

            return false;
        }

        /// <summary>Determine if user has mail grant</summary>
        /// <param name="grants">Collection of user grants</param>
        /// <param name="itemId">Grant to examine</param>
        /// <returns>Return if user has delete grant</returns>
        public static bool HasMailGrant(ReadOnlyCollection<UserGrant> grants, long itemId)
        {
            if (grants == null)
            {
                return false;
            }

            foreach (var g in grants)
            {
                if (itemId == g.ItemId)
                {
                    return g.Grants.Contains("M");
                }
            }

            return false;
        }

        /// <summary>Determine if user has Import grant</summary>
        /// <param name="grants">Collection of user grants</param>
        /// <param name="itemId">Grant to examine</param>
        /// <returns>Return if user has delete grant</returns>
        public static bool HasImportGrant(ReadOnlyCollection<UserGrant> grants, long itemId)
        {
            if (grants == null)
            {
                return false;
            }

            foreach (var g in grants)
            {
                if (itemId == g.ItemId)
                {
                    return g.Grants.Contains("I");
                }
            }

            return false;
        }

        /// <summary>Determine if user has geolocation grant</summary>
        /// <param name="grants">Collection of user grants</param>
        /// <param name="itemId">Grant to examine</param>
        /// <returns>Return if user has delete grant</returns>
        public static bool HasGeolocationGrant(ReadOnlyCollection<UserGrant> grants, long itemId)
        {
            if (grants == null)
            {
                return false;
            }

            foreach (var g in grants)
            {
                if (itemId == g.ItemId)
                {
                    return g.Grants.Contains("G");
                }
            }

            return false;
        }

        /// <summary>Determine if user has Disable grant</summary>
        /// <param name="grants">Collection of user grants</param>
        /// <param name="itemId">Grant to examine</param>
        /// <returns>Return if user has delete grant</returns>
        public static bool HasUnloadableGrant(ReadOnlyCollection<UserGrant> grants, long itemId)
        {
            if (grants == null)
            {
                return false;
            }

            foreach (var g in grants)
            {
                if (itemId == g.ItemId)
                {
                    return g.Grants.Contains("U");
                }
            }

            return false;
        }

        /// <summary>Saves user grant into database</summary>
        /// <param name="applicationUserId">Identifier of user that performs the action</param>
        /// <param name="connectionString">String for database connection</param>
        /// <returns>Result of action</returns>
        public ActionResult Save(long applicationUserId, string connectionString)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "UserGrant::Save({0},{1},{2})", this.Json, applicationUserId, connectionString);
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Core_UserGrant_Save"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@UserId", this.ApplicationUserId));
                cmd.Parameters.Add(DataParameter.Input("@SecurityGroupId", this.SecurityGroupId));
                cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                cmd.Parameters.Add(DataParameter.Input("@ItemName", this.ItemName, 50));
                cmd.Parameters.Add(DataParameter.Input("@Grants", this.Grants, 15));
                cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                using (var cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (NullReferenceException ex)

                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (FormatException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (NotSupportedException ex)
                    {
                        res.SetFail(ex);
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

            return res;
        }
    }
}