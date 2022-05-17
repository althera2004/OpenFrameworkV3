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
    public partial class Grant
    {
        /// <summary>Gets an empty user grant</summary>
        public static Grant Empty
        {
            get
            {
                return new Grant
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
        /// <param name="companyId">Company identifier</param>
        /// <param name="instanceName">Name of instance</param>
        /// <returns>List of user grants</returns>
        public static ReadOnlyCollection<Grant> ByUser(long applicationUserId, long companyId, string instanceName)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "UserGrant::ByUser({0}, {1})", applicationUserId, instanceName);
            var res = new List<Grant>();

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {

                using (var cmd = new SqlCommand("Core_User_GetGrants"))
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
                                    res.Add(new Grant
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
            }

            return new ReadOnlyCollection<Grant>(res);
        }

        /// <summary>Obtains grants for security group</summary>
        /// <param name="groupId">Security group identifier</param>
        /// <param name="instanceName">String for database connection</param>
        /// <returns>List of security group grants</returns>
        public static ReadOnlyCollection<Grant> ByGroup(long groupId, long companyId, string instanceName)
        {
            /* CREATE PROCEDURE [dbo].[Core_SecurityGroup_GetGrants]
             *   @SecurityGroupId bigint */
            var source = string.Format(CultureInfo.InvariantCulture, "UserGrant::ByGroup({0}, {1})", groupId, instanceName);
            var res = new List<Grant>();

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_SecurityGroup_GetGrants"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@SecurityGroupId", groupId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new Grant
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
            }

            return new ReadOnlyCollection<Grant>(res);
        }

        /// <summary>Gets a JSON list from a list of grants</summary>
        /// <param name="list">List of grants</param>
        /// <returns>JSON list from a list of grants</returns>
        public static string JsonList(ReadOnlyCollection<Grant> list)
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
        public static bool HasReadGrant(ReadOnlyCollection<Grant> grants, long itemId)
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
        public static bool HasWriteGrant(ReadOnlyCollection<Grant> grants, long itemId)
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
        public static bool HasDeleteGrant(ReadOnlyCollection<Grant> grants, long itemId)
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
        public static bool HasFAQSGrant(ReadOnlyCollection<Grant> grants, long itemId)
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
        public static bool HasNewsGrant(ReadOnlyCollection<Grant> grants, long itemId)
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
        public static bool HasMailGrant(ReadOnlyCollection<Grant> grants, long itemId)
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
        public static bool HasImportGrant(ReadOnlyCollection<Grant> grants, long itemId)
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
        public static bool HasGeolocationGrant(ReadOnlyCollection<Grant> grants, long itemId)
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
        public static bool HasUnloadableGrant(ReadOnlyCollection<Grant> grants, long itemId)
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
        public ActionResult Save(long applicationUserId, string instanceName)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "UserGrant::Save({0},{1},{2})", this.Json, applicationUserId, instanceName);
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_UserGrant_Save"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@UserId", this.ApplicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@SecurityGroupId", this.SecurityGroupId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemName", this.ItemName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Grants", this.Grants, 15));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
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
            }

            return res;
        }
    }
}