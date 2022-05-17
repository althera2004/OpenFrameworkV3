// --------------------------------
// <copyright file="Scope.cs" company="OpenFramework">
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
    using System.Text;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Bindings;
    using OpenFrameworkV3.Core.DataAccess;

    /// <summary>Implements scope actions for users</summary>
    public partial class Scope
    {
        public long Id { get; set; }
        public long ApplicationUserId { get; set; }
        public long ScopeGroupId { get; set; }
        public long ItemDefinitionId { get; set; }
        public long ItemId { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active;

        public static Scope Empty
        {
            get
            {
                return new Scope
                {
                    Id = Constant.DefaultId,
                    ApplicationUserId = Constant.DefaultId,
                    ScopeGroupId = Constant.DefaultId,
                    ItemDefinitionId = Constant.DefaultId,
                    ItemId = Constant.DefaultId,
                    CreatedBy = ApplicationUser.Empty,
                    ModifiedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public static string JsonListSimple(ReadOnlyCollection<Scope> list)
        {
            return JsonList(list, Constant.True);
        }

        public static string JsonList(ReadOnlyCollection<Scope> list)
        {
            return JsonList(list, Constant.False);
        }

        public static string JsonList(ReadOnlyCollection<Scope> list, bool simple)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var scope in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(simple ? scope.JsonKeyValue : scope.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""ApplicationUserId"":{1},""ScopeGroupId"":{2},""ItemDefinitionId"":{3},""ItemId"":{4},""Active"":{5}}}",
                    this.Id,
                    this.ApplicationUserId,
                    this.ScopeGroupId,
                    this.ItemDefinitionId,
                    this.ItemId,
                    ConstantValue.Value(this.Active));
            }
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""ApplicationUserId"":{1},""ScopeGroupId"":{2},""ItemDefinitionId"":{3},""ItemId"":{4},""CreatedBy"":{5},""CreatedOn"":""{6:dd/MM/yyyy}"",""ModifiedBy"":{7},""ModifiedOn"":""{8:dd/MM/yyyy}"",""Active"":{9}}}",
                    this.Id,
                    this.ApplicationUserId,
                    this.ScopeGroupId,
                    this.ItemDefinitionId,
                    this.ItemId,
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static ReadOnlyCollection<Scope> ByUser(long userId, string instanceName)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "Scope::ByUser({0})", userId);
            var cns = Persistence.ConnectionString(instanceName);
            var res = new List<Scope>();

            if (!string.IsNullOrEmpty(cns))
            {
                /* CREATE PROCEDURE [dbo].[Core_ScopeUser_GetScopeView]
                 *   @ScopeUserId bigint */
                using (var cmd = new SqlCommand("Core_ScopeUser_GetScopeView"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@ScopeUserId", userId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new Scope
                                    {
                                        Id = rdr.GetInt64(ColumnsScopeGet.Id),
                                        ApplicationUserId = rdr.GetInt64(ColumnsScopeGet.ApplicationUserId),
                                        ScopeGroupId = Constant.DefaultId,
                                        ItemDefinitionId = rdr.GetInt64(ColumnsScopeGet.ItemDefinitionId),
                                        ItemId = rdr.GetInt64(ColumnsScopeGet.ItemId),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsScopeGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsScopeGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsScopeGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsScopeGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsScopeGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsScopeGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsScopeGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsScopeGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsScopeGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsScopeGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsScopeGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsScopeGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsScopeGet.Active)
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
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

            return new ReadOnlyCollection<Scope>(res);
        }

        public static ReadOnlyCollection<Scope> ByGroup(long groupId, string instanceName)
        {
            var res = new List<Scope>();
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Core_Scope_GetByGroup"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", groupId));
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                while(rdr.Read())
                                {
                                    res.Add(new Scope
                                    {
                                        Id = rdr.GetInt64(ColumnsScopeGet.Id),
                                        ApplicationUserId = Constant.DefaultId,
                                        ScopeGroupId = rdr.GetInt64(ColumnsScopeGet.ScopeGroupId),
                                        ItemDefinitionId = rdr.GetInt64(ColumnsScopeGet.ItemDefinitionId),
                                        ItemId = rdr.GetInt64(ColumnsScopeGet.ItemId),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsScopeGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsScopeGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsScopeGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsScopeGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsScopeGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsScopeGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsScopeGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsScopeGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsScopeGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsScopeGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsScopeGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsScopeGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsScopeGet.Active)
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
            }

            return new ReadOnlyCollection<Scope>(res);
        }

        public ActionResult Save(long applicationUserId, string instanceName)
        {
            var source = string.Format(CultureInfo.InvariantCulture, @"Scope::Save({0},{1})", this.Json, applicationUserId);
            var res = ActionResult.NoAction;

            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                /* CREATE PROCEDURE Core_Scope_Save
                 *   @UserId bigint,
                 *   @ScopeGroupId bigint,
                 *   @ItemDefinitionId bigint,
                 *   @ItemId bigint,
                 *   @ApplicationUserId bigint */
                using(var cmd = new SqlCommand("Core_Scope_Save"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@UserId", this.ApplicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@ScopeGroupId", this.ScopeGroupId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitonId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
                        }
                        catch(NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch(FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch(SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch(NotSupportedException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
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
            }

            return res;
        }
    }
}