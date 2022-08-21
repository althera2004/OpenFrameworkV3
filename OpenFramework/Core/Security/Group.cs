// --------------------------------
// <copyright file="SecurityGroup.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Security
{
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Bindings;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.ItemManager;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>Implements groups for security purposses</summary>
    public partial class Group
    {
        /// <summary>Identifier of users</summary>
        private List<long> membership;

        /// <summary>Collections of security group grants</summary>
        private List<Grant> grants;

        /// <summary>Gets an empty group</summary>
        public static Group Empty
        {
            get
            {
                return new Group
                {
                    Id = Constant.DefaultId,
                    Name = string.Empty,
                    Description = string.Empty,
                    Deletable = true,
                    Core = false,
                    RemindAlert = false,
                    BillingAccess = false,
                    CreatedBy = ApplicationUser.Empty,
                    ModifiedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Active = false,
                    membership = new List<long>()
                };
            }
        }

        public static string MemberShipJsonList(long companyId, string instanceName)
        {
            var cns = Persistence.ConnectionString(instanceName);

            if (string.IsNullOrEmpty(cns))
            {
                return Tools.Json.EmptyJsonList;
            }

            /* CREATE PROCEDURE [dbo].[Core_CompanyMemberShip_GetByCompanyId]
             *  	@CompanyId bigint */
            var res = new StringBuilder("[");
            var first = true;

            using (var cmd = new SqlCommand("Core_SecurityGroup_GetAllMembersByCompany"))
            {
                using (var cnn = new SqlConnection(cns))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.AppendFormat(
                                    CultureInfo.InvariantCulture,
                                    @"{3}{{""GroupId"":{0}, ""UserId"":{1}, ""Active"":{2}}}",
                                    rdr.GetInt64(0),
                                    rdr.GetInt64(1),
                                    ConstantValue.Value(rdr.GetBoolean(2)),
                                    first ? string.Empty : ",");
                                first = false;
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

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<Group> ByUserId(long userId, long companyId, string instanceName)
        {
            var cns = Persistence.ConnectionString(instanceName);
            var res = new List<Group>();

            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Group_ByUserId"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("CompanyId", companyId));

                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new Group
                                    {
                                        Id = rdr.GetInt64(ColumnsGroupGet.Id),
                                        Name = rdr.GetString(ColumnsGroupGet.Name),
                                        Description = rdr.GetString(ColumnsGroupGet.Description),
                                        Deletable = rdr.GetBoolean(ColumnsGroupGet.Deletable),
                                        Core = rdr.GetBoolean(ColumnsGroupGet.Core),
                                        BillingAccess = rdr.GetBoolean(ColumnsGroupGet.BillingAccess),
                                        MainUserId = rdr.GetInt64(ColumnsGroupGet.MainUserId),
                                        CreatedOn = rdr.GetDateTime(ColumnsGroupGet.CreatedOn),
                                        ModifiedOn = rdr.GetDateTime(ColumnsGroupGet.ModifiedOn),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsGroupGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsGroupGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsGroupGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsGroupGet.CreatedByLastName)
                                            }
                                        },
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsGroupGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsGroupGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsGroupGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsGroupGet.ModifiedByLastName)
                                            }
                                        },
                                        Active = rdr.GetBoolean(ColumnsGroupGet.Active)
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

            return new ReadOnlyCollection<Group>(res);
        }

        /// <summary>Gets all groups</summary>
        public static ReadOnlyCollection<Group> All(long companyId, string instanceName)
        {
            var cns = Persistence.ConnectionString(instanceName);
            var res = new List<Group>();

            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Group_GetAll"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("CompanyId", companyId));

                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new Group
                                    {
                                        Id = rdr.GetInt64(ColumnsGroupGet.Id),
                                        Name = rdr.GetString(ColumnsGroupGet.Name),
                                        Description = rdr.GetString(ColumnsGroupGet.Description),
                                        Deletable = rdr.GetBoolean(ColumnsGroupGet.Deletable),
                                        Core = rdr.GetBoolean(ColumnsGroupGet.Core),
                                        CreatedOn = rdr.GetDateTime(ColumnsGroupGet.CreatedOn),
                                        ModifiedOn = rdr.GetDateTime(ColumnsGroupGet.ModifiedOn),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsGroupGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsGroupGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsGroupGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsGroupGet.CreatedByLastName)
                                            }
                                        },
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsGroupGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsGroupGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsGroupGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsGroupGet.ModifiedByLastName)
                                            }
                                        },
                                        Active = rdr.GetBoolean(ColumnsGroupGet.Active)
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

            return new ReadOnlyCollection<Group>(res);
        }

        public static ActionResult CleanGrants(long groupId, long companyId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);

            if (!string.IsNullOrEmpty(cns))
            {
                /* CREATE PROCEDURE [dbo].[Core_SecurityGroup_CleanGrants]
                 * @SecurityGroupId bigint */
                using (var cmd = new SqlCommand("Core_SecurityGroup_CleanGrants"))
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

        public static ActionResult SetGrants(long groupId, ReadOnlyCollection<string> grants, long applicationUserId, long companyId, string instanceName)
        {
            var res = CleanGrants(groupId, companyId, instanceName);
            if (res.Success)
            {
                var cns = Persistence.ConnectionString(instanceName);
                var definitions = Persistence.InstanceByName(instanceName).ItemDefinitions;

                if (!string.IsNullOrEmpty(cns))
                {
                    var itemGrants = new List<Grant>();
                    foreach (string grant in grants.Where(g => !string.IsNullOrEmpty(g)))
                    {
                        var grantType = grant.Substring(0, 1);
                        var grantItemId = Convert.ToInt64(grant.Substring(1));

                        if (itemGrants.Any(g => g.ItemId == grantItemId))
                        {
                            itemGrants.First(g => g.ItemId == grantItemId).Grants += grantType;
                        }
                        else
                        {
                            itemGrants.Add(new Grant
                            {
                                ItemId = grantItemId,
                                ItemName = definitions.First(d => d.Id == grantItemId).ItemName,
                                Grants = grantType,
                                SecurityGroupId = Constant.DefaultId
                            });
                        }
                    }

                    /* CREATE PROCEDURE Core_SecurityGroup_SetGrants
                     *   @SecurityGroupId bigint,
                     *   @ItemId bigint,
                     *   @ItemName nvarchar(50),
                     *   @Grants nvarchar(15) */
                    using (var cmd = new SqlCommand("Core_SecurityGroup_SetGrants"))
                    {
                        using (var cnn = new SqlConnection(cns))
                        {
                            cmd.Connection = cnn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            try
                            {
                                cmd.Connection.Open();
                                foreach (var grant in itemGrants)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add(DataParameter.Input("@SecurityGroupId", groupId));
                                    cmd.Parameters.Add(DataParameter.Input("@ItemId", grant.ItemId));
                                    cmd.Parameters.Add(DataParameter.Input("@ItemName", grant.ItemName, 50));
                                    cmd.Parameters.Add(DataParameter.Input("@Grants", grant.Grants, 15));
                                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                                    cmd.ExecuteNonQuery();
                                }

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
            }

            return res;
        }

        public ReadOnlyCollection<Grant> Grants
        {
            get
            {
                if (this.grants == null)
                {
                    this.grants = new List<Grant>();
                }

                return new ReadOnlyCollection<Grant>(this.grants);
            }
        }

        /// <summary>Gets or sets identifier</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets name</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets description</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets a value indicating whether if group is deletable</summary>
        public bool Deletable { get; set; }

        /// <summary>Gets or sets a value indicating whether if group is core</summary>
        public bool Core { get; set; }

        /// <summary>Gets or sets a value indicating whether if group members can remind alert to others users</summary>
        public bool RemindAlert { get; set; }

        /// <summary>Gets or sets a value indicating whether if group members can access to billing data</summary>
        public bool BillingAccess { get; set; }

        /// <summary>Gets or sets identifier of main user</summary>
        public long MainUserId { get; set; }

        /// <summary>Gets or sets the user that creates group</summary>
        public ApplicationUser CreatedBy { get; set; }

        /// <summary>Gets or sets the date and time of creation of group</summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>Gets or sets the user of last modification of group</summary>
        public ApplicationUser ModifiedBy { get; set; }

        /// <summary>Gets or sets the date and time of last modification of group</summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>Gets or sets a value indicating whether if group is active</summary>
        public bool Active { get; set; }

        /// <summary>Gets the identifiers of users</summary>
        public ReadOnlyCollection<long> Membership
        {
            get
            {
                if (this.membership == null)
                {
                    this.membership = new List<long>();
                }

                return new ReadOnlyCollection<long>(this.membership);
            }
        }

        /// <summary>Gets a json jey/value structure of security group</summary>
        public string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0}, ""Name"":""{1}"",  ""Core"":{2}, ""Active"":{3}}}",
                    this.Id,
                    Tools.Json.JsonCompliant(this.Name),
                    ConstantValue.Value(this.Core),
                    ConstantValue.Value(this.Active));
            }
        }

        /// <summary>Gets a simple json structure of security group</summary>
        public string JsonSimple
        {
            get
            {
                if (this.grants == null)
                {
                    this.grants = new List<Grant>();
                }

                string pattern = @"{{""Id"":{0},
                        ""Name"":""{1}"",
                        ""Description"":""{2}"",
                        ""Deletable"":{3},
                        ""Core"":{4}}}";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Id,
                    Tools.Json.JsonCompliant(this.Name),
                    Tools.Json.JsonCompliant(this.Description),
                    ConstantValue.Value(this.Deletable),
                    ConstantValue.Value(this.Core));
            }
        }

        /// <summary>Gets a json structure of security group</summary>
        public string Json
        {
            get
            {
                if (this.grants == null)
                {
                    this.grants = new List<Grant>();
                }

                string pattern = @"{{""Id"":{0},
                        ""Name"":""{1}"",
                        ""Description"":""{2}"",
                        ""Deletable"":{3},
                        ""Core"":{4},
                        ""RemindAlert"": {5},
                        ""BillingAccess"": {6},
                        ""Active"":{7},
                        ""CreatedBy"":{8},
                        ""CreatedOn"":""{9:dd/MM/yyyy}"",
                        ""ModifiedBy"":{10},
                        ""ModifiedOn"":""{11:dd/MM/yyyy}"",
                        ""Grants"":{12}}}";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Id,
                    Tools.Json.JsonCompliant(this.Name),
                    Tools.Json.JsonCompliant(this.Description),
                    ConstantValue.Value(this.Deletable),
                    ConstantValue.Value(this.Core),
                    ConstantValue.Value(this.RemindAlert),
                    ConstantValue.Value(this.BillingAccess),
                    ConstantValue.Value(this.Active),
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    Grant.JsonList(new ReadOnlyCollection<Grant>(this.grants)));
            }
        }

        /// <summary>Sets membership of group into database</summary>
        /// <param name="securityGroupId">Security group identifier</param>
        /// <param name="membership">List of user identifiers</param>
        /// <param name="applicationUserId">Identifer of user that perfoms the action</param>
        /// <returns>Result of action</returns>
        public static ActionResult SetMembership(long securityGroupId, ReadOnlyCollection<long> membership, long applicationUserId, long companyId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[Core_SecurityGroup_SetMembershipv2]
             *   @SecurityGroupId bigint,
             *   @Membership nvarchar(max),
             *   @ApplicationUserId bigint */

            var membershipText = new StringBuilder();
            bool first = true;
            foreach (long id in membership)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    membershipText.Append(",");
                }

                membershipText.Append(id);
            }

            if (!first)
            {
                var cns = Persistence.ConnectionString(instanceName);

                if (!string.IsNullOrEmpty(cns))
                {
                    using (var cmd = new SqlCommand("Core_SecurityGroup_SetMembershipv2"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var cnn = new SqlConnection(cns))
                        {
                            cmd.Connection = cnn;
                            cmd.Parameters.Add(DataParameter.Input("@SecurityGroupId", securityGroupId));
                            cmd.Parameters.Add(DataParameter.Input("@Membership", membershipText.ToString(), 4000));
                            cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                            cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
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
            }

            return res;
        }

        /// <summary>Gets security group from data base by identifier</summary>
        /// <param name="securityGroupId">Security group identifier</param>
        /// <param name="companyId">Comnpany identifier</param>
        /// <param name="instanceName">Name of intance</param>
        /// <returns>Security group</returns>
        public static Group ById(long securityGroupId, long companyId, string instanceName)
        {
            var res = Group.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_SecurityGroup_GetById"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@SecurityGroupId", securityGroupId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Id = rdr.GetInt64(ColumnsGroupGet.Id);
                                    res.Name = rdr.GetString(ColumnsGroupGet.Name);
                                    res.Description = rdr.GetString(ColumnsGroupGet.Description);
                                    res.Deletable = rdr.GetBoolean(ColumnsGroupGet.Deletable);
                                    res.Core = rdr.GetBoolean(ColumnsGroupGet.Core);
                                    res.RemindAlert = rdr.GetBoolean(ColumnsGroupGet.RemindAlert);
                                    res.BillingAccess = rdr.GetBoolean(ColumnsGroupGet.BillingAccess);
                                    res.Active = rdr.GetBoolean(ColumnsGroupGet.Active);
                                    res.CreatedOn = rdr.GetDateTime(ColumnsGroupGet.CreatedOn);
                                    res.ModifiedOn = rdr.GetDateTime(ColumnsGroupGet.ModifiedOn);
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsGroupGet.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsGroupGet.CreatedBy),
                                            Name = rdr.GetString(ColumnsGroupGet.CreatedByName),
                                            LastName = rdr.GetString(ColumnsGroupGet.CreatedByLastName)
                                        }
                                    };
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsGroupGet.ModifiedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsGroupGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnsGroupGet.ModifiedByName),
                                            LastName = rdr.GetString(ColumnsGroupGet.ModifiedByLastName)
                                        }
                                    };

                                    res.ObtainMemberShip(companyId, instanceName);
                                    res.ObtainGrants(companyId, instanceName);
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

        /// <summary>Gets a list of groups in JSON format</summary>
        /// <param name="list">Groups list</param>
        /// <returns>A list of groups in JSON format</returns>
        public static string JsonListSimple(ReadOnlyCollection<Group> list)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var group in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(group.JsonSimple);
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>Gets a list of groups in JSON format</summary>
        /// <param name="list">Groups list</param>
        /// <returns>A list of groups in JSON format</returns>
        public static string JsonList(ReadOnlyCollection<Group> list)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var group in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(group.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>Obtain membership from database</summary>
        public void ObtainMemberShip(long companyId, string instanceName)
        {
            if (this.membership == null)
            {
                this.membership = new List<long>();
            }

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_SecurityGroup_GetMembersByCompany"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@SecurityGroupId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    this.membership.Add(rdr.GetInt64(1));
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

        /// <summary>Obtain user grants from database</summary>
        public void ObtainGrants(long companyId, string instanceName)
        {
            this.grants = new List<Grant>();
            var cns = Persistence.ConnectionString(instanceName);
            var definitions = Persistence.InstanceByName(instanceName).ItemDefinitions;

            if (!string.IsNullOrEmpty(cns))
            {
                // Initialize all grants for each item, if is admin or primery user set all grant to true
                foreach (var itemName in definitions)
                {
                    var newGrant = new Grant
                    {
                        ApplicationUserId = this.Id,
                        ItemId = itemName.Id,
                        ItemName = itemName.ItemName,
                        Grants = string.Empty
                    };

                    newGrant.Grants += this.Core ? "R" : string.Empty;
                    newGrant.Grants += this.Core ? "W" : string.Empty;
                    newGrant.Grants += this.Core ? "D" : string.Empty;
                    newGrant.Grants += this.Core ? "F" : string.Empty;
                    newGrant.Grants += this.Core ? "N" : string.Empty;
                    newGrant.Grants += this.Core ? "I" : string.Empty;
                    newGrant.Grants += this.Core ? "H" : string.Empty;
                    newGrant.Grants += this.Core ? "G" : string.Empty;
                    this.grants.Add(newGrant);
                }

                if (!this.Core)
                {
                    foreach (var grant in Grant.ByGroup(this.Id,companyId, instanceName))
                    {
                        if (this.grants.Any(g => g.ItemId == grant.ItemId))
                        {
                            var grantActual = this.grants.First(g => g.ItemId == grant.ItemId);
                            this.grants.Remove(grantActual);
                            this.grants.Add(grant);
                        }
                    }
                }

                this.grants = this.grants.OrderBy(g => g.ItemName).ToList();
            }
        }

        /// <summary>Adds multiple membership</summary>
        /// <param name="memberShip">List of application user identifiers</param>
        public void BulkMemberShip(ReadOnlyCollection<long> memberShip)
        {
            foreach (long id in memberShip)
            {
                this.AddMemberShip(id);
            }
        }

        /// <summary>Adds multiple membership</summary>
        /// <param name="memberShip">List of application user identifiers</param>
        public void BulkMemberShip(ReadOnlyCollection<string> memberShip)
        {
            foreach (string id in memberShip)
            {
                if (long.TryParse(id, out long realId))
                {
                    this.AddMemberShip(Convert.ToInt64(realId));
                }
                else
                {
                    this.AddMemberShip(Constant.DefaultId);
                }
            }
        }

        /// <summary>Adds membership</summary>
        /// <param name="userId">Application user identifier</param>
        public void AddMemberShip(long userId)
        {
            if (this.membership == null)
            {
                this.membership = new List<long>();
            }

            if (!this.membership.Contains(userId) && userId > 0)
            {
                this.membership.Add(userId);
            }
        }

        /// <summary>Inserts security group info into database</summary>
        /// <param name="applicationUserId">Identifier of user that perfoms the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(long applicationUserId,long companyId, string instanceName)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "SecurityGroup::Insert({0},{1}", this.Id, applicationUserId);
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);

            if (!string.IsNullOrEmpty(cns))
            {
                /* CREATE PROCEDURE [dbo].[Core_SecurityGroup_Insert]
                 *   @Id bigint output,
                 *   @Name nvarchar(100),
                 *   @Description nvarchar(150),
                 *   RemindAlert bit,
                 *   BillingAccess bit,
                 *   @ApplicationUserId bigint */
                using (var cmd = new SqlCommand("Core_SecurityGroup_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@Name", this.Name, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 150));
                        cmd.Parameters.Add(DataParameter.Input("@RemindAlert", this.RemindAlert));
                        cmd.Parameters.Add(DataParameter.Input("@BillingAccess", this.BillingAccess));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.ReturnValue = this.Id;
                            res.SetSuccess(this.Id);

                            var resMembership = SetMembership(applicationUserId, companyId, instanceName);
                            if (!resMembership.Success)
                            {
                                res.SetFail(resMembership.MessageError);
                            }
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NotSupportedException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
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

        /// <summary>Updates security group info into database</summary>
        /// <param name="applicationUserId">Identifier of user that perfoms the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(long applicationUserId,long companyId, string instanceName)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "SecurityGroup::Update({0},{1}", this.Id, applicationUserId);
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);

            if (!string.IsNullOrEmpty(cns))
            {
                /* CREATE PROCEDURE [dbo].[Core_SecurityGroup_Update]
                 *   @Id bigint,
                 *   @Name nvarchar(100),
                 *   @Description nvarchar(150),
                 *   RemindAlert bit,
                 *   BillingAccess bit,
                 *   @ApplicationUserId bigint */
                using (var cmd = new SqlCommand("Core_SecurityGroup_Update"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Name", this.Name, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 150));
                        cmd.Parameters.Add(DataParameter.Input("@RemindAlert", this.RemindAlert));
                        cmd.Parameters.Add(DataParameter.Input("@BillingAccess", this.BillingAccess));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.ReturnValue = this.Id;
                            res.SetSuccess(this.Id);

                            var resMembership = SetMembership(applicationUserId, companyId, instanceName);
                            if (!resMembership.Success)
                            {
                                res.SetFail(resMembership.MessageError);
                            }
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NotSupportedException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
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

        /// <summary>Sets membership of group into database</summary>
        /// <param name="applicationUserId">Identifer of user that perfoms the action</param>
        /// <returns>Result of action</returns>
        private ActionResult SetMembership(long applicationUserId, long companyId, string instanceName)
        {
            return SetMembership(this.Id, this.Membership, applicationUserId, companyId, instanceName);
        }

        public static string FKList(long companyId,string language, string instanceName)
        {
            var res = new StringBuilder("[");
            var groups = All(companyId, instanceName);
            var first = true;
            foreach (var item in groups)
            {
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"{4}{{""Id"":{0}, ""Description"":""{1}"", ""Active"":{2}, ""Core"": {3}}}",
                    item.Id,
                    Tools.Json.JsonCompliant(ApplicationDictionary.Translate("Core_SecurityGroup_Name_" + item.Id.ToString(), language, instanceName)),
                    ConstantValue.Value(item.Active),
                    ConstantValue.Value(item.Core),
                    first ? string.Empty : ",");
                first = false;
            }

            res.Append("]");
            return res.ToString();
        }

        public static long MainUser(long companyId, long GroupId, string instanceName)
        {
            long res = 0;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("SELECT UserId FROM Core_GroupUserMain WHERE CompanyId = " + companyId.ToString() + " AND GroupId = " + GroupId.ToString()))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.Text;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    res = rdr.GetInt64(0);
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
    }
}