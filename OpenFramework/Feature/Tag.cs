// --------------------------------
// <copyright file="Tag.cs" company="OpenFramework">
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
    using System.Text;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;

    public partial class Tag
    {
        public const long ItemGrantCode = 1010;
        public const string ItemGrantName = "CoreTags";
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Tags { get; set; }
        public long? ItemDefinitionId { get; set; }
        public long? ItemId { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public static Tag Empty
        {
            get
            {
                return new Tag
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    Tags = string.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    ModifiedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public string Json
        {
            get
            {
                var itemDefinitionId = this.ItemDefinitionId.HasValue ? this.ItemDefinitionId.ToString() : Constant.JavaScriptNull;
                var itemId = this.ItemId.HasValue ? this.ItemId.ToString() : Constant.JavaScriptNull;
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"":{0},
                        ""CompanyId"":{1},
                        ""Tags"":""{2}"",
                        ""ItemDefinitionId"":{3},
                        ""ItemId"":{4},
                        ""CreatedBy"":{5},
                        ""CreatedOn"":""{6:dd/MM/yyyy}"",
                        ""ModifiedBy"":{7},
                        ""ModifiedOn"":""{8:dd/MM/yyyy}"",
                        ""Active"":{9}
                    }}",
                    this.Id,
                    this.CompanyId,
                    Tools.Json.JsonCompliant(this.Tags),
                    itemDefinitionId,
                    itemId,
                    this.CreatedBy.JsonSimple,
                    this.CreatedOn,
                    this.ModifiedBy.JsonSimple,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public string JsonSimple
        {
            get
            {
                var itemDefinitionId = this.ItemDefinitionId.HasValue ? this.ItemDefinitionId.ToString() : Constant.JavaScriptNull;
                var itemId = this.ItemId.HasValue ? this.ItemId.ToString() : Constant.JavaScriptNull;
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"":{0},
                        ""CompanyId"":{1},
                        ""Tags"":""{2}"",
                        ""ItemDefinitionId"":{3},
                        ""ItemId"":{4}
                    }}",
                    this.Id,
                    this.CompanyId,
                    Tools.Json.JsonCompliant(this.Tags),
                    itemDefinitionId,
                    itemId);
            }
        }

        public static string JsonList(ReadOnlyCollection<Tag> list)
        {
            var res = new StringBuilder("[");
            if (list != null && list.Count > 0)
            {
                bool first = true;
                foreach (var tag in list)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(tag.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<Tag> All(string instanceName)
        {
            var res = new List<Tag>();
            var user = ApplicationUser.Actual;
            var cns = Persistence.ConnectionString(instanceName);
            if (string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_Tag_GetAll"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", user.CompanyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    var newTag = new Tag
                                    {
                                        Id = rdr.GetInt64(ColumnsTagsGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsTagsGet.CompanyId),
                                        Tags = rdr.GetString(ColumnsTagsGet.Tags),
                                        Active = rdr.GetBoolean(ColumnsTagsGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsTagsGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsTagsGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsTagsGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsTagsGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsTagsGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsTagsGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsTagsGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsTagsGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsTagsGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsTagsGet.ModifiedLastName),
                                                LastName2 = rdr.GetString(ColumnsTagsGet.ModifiedLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsTagsGet.ModifiedOn)
                                    };

                                    if (!rdr.IsDBNull(ColumnsTagsGet.ItemDefinitionId))
                                    {
                                        newTag.ItemDefinitionId = rdr.GetInt64(ColumnsTagsGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsTagsGet.ItemId))
                                    {
                                        newTag.ItemId = rdr.GetInt64(ColumnsTagsGet.ItemId);
                                    }

                                    res.Add(newTag);
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

            return new ReadOnlyCollection<Tag>(res);
        }

        public static Tag ByItemId(long itemId, long itemDefinitionId, long companyId, string instanceName)
        {
            var res = Tag.Empty;
            var user = ApplicationUser.Actual;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_Tag_GetByItemId"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", user.CompanyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                bool first = true;
                                while (rdr.Read())
                                {
                                    if (first)
                                    {
                                        first = false;
                                        res.Id = rdr.GetInt64(ColumnsTagsGet.Id);
                                        res.CompanyId = rdr.GetInt64(ColumnsTagsGet.CompanyId);
                                        res.Tags = rdr.GetString(ColumnsTagsGet.Tags);
                                        res.Active = rdr.GetBoolean(ColumnsTagsGet.Active);
                                        res.CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsTagsGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsTagsGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsTagsGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsTagsGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsTagsGet.CreatedByLastName2)
                                            }
                                        };
                                        res.CreatedOn = rdr.GetDateTime(ColumnsTagsGet.CreatedOn);
                                        res.ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsTagsGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsTagsGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsTagsGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsTagsGet.ModifiedLastName),
                                                LastName2 = rdr.GetString(ColumnsTagsGet.ModifiedLastName2)
                                            }
                                        };
                                        res.ModifiedOn = rdr.GetDateTime(ColumnsTagsGet.ModifiedOn);

                                        if (!rdr.IsDBNull(ColumnsTagsGet.ItemDefinitionId))
                                        {
                                            res.ItemDefinitionId = rdr.GetInt64(ColumnsTagsGet.ItemDefinitionId);
                                        }

                                        if (!rdr.IsDBNull(ColumnsTagsGet.ItemId))
                                        {
                                            res.ItemId = rdr.GetInt64(ColumnsTagsGet.ItemId);
                                        }
                                    }
                                    else
                                    {
                                        res.Tags += ", ";
                                        res.Tags += rdr.GetString(ColumnsTagsGet.Tags);
                                        res.ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsTagsGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsTagsGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsTagsGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsTagsGet.ModifiedLastName),
                                                LastName2 = rdr.GetString(ColumnsTagsGet.ModifiedLastName2)
                                            }
                                        };
                                        res.ModifiedOn = rdr.GetDateTime(ColumnsTagsGet.ModifiedOn);
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

        public static Tag ById(long id, string instanceName)
        {
            var res = Tag.Empty;
            var user = ApplicationUser.Actual;
            var cns = Persistence.ConnectionString(instanceName);

            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_Tag_GetById"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", user.CompanyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Id = rdr.GetInt64(ColumnsTagsGet.Id);
                                    res.CompanyId = rdr.GetInt64(ColumnsTagsGet.CompanyId);
                                    res.Tags = rdr.GetString(ColumnsTagsGet.Tags);
                                    res.Active = rdr.GetBoolean(ColumnsTagsGet.Active);
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsTagsGet.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsTagsGet.CreatedBy),
                                            Name = rdr.GetString(ColumnsTagsGet.CreatedByName),
                                            LastName = rdr.GetString(ColumnsTagsGet.CreatedByLastName),
                                            LastName2 = rdr.GetString(ColumnsTagsGet.CreatedByLastName2)
                                        }
                                    };
                                    res.CreatedOn = rdr.GetDateTime(ColumnsTagsGet.CreatedOn);
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsTagsGet.ModifiedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsTagsGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnsTagsGet.ModifiedByName),
                                            LastName = rdr.GetString(ColumnsTagsGet.ModifiedLastName),
                                            LastName2 = rdr.GetString(ColumnsTagsGet.ModifiedLastName2)
                                        }
                                    };
                                    res.ModifiedOn = rdr.GetDateTime(ColumnsTagsGet.ModifiedOn);

                                    if (!rdr.IsDBNull(ColumnsTagsGet.ItemDefinitionId))
                                    {
                                        res.ItemDefinitionId = rdr.GetInt64(ColumnsTagsGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsTagsGet.ItemId))
                                    {
                                        res.ItemId = rdr.GetInt64(ColumnsTagsGet.ItemId);
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

        public ActionResult Save(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"Tag::insert ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_Tag_Save]
             *   @CompanyId bigint,
             *   @Tags nvarchar(500),
             *   @ItemDefintionId bigint,
             *   @ItemId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_Tag_Save"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Tags", this.Tags, 500));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
                        }
                        catch (FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NotImplementedException ex)
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

        public ActionResult Activate(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"Tag::Activate ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_Tag_Activate]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_Tag_Activate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(this.Id);
                        }
                        catch (FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NotImplementedException ex)
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

        public ActionResult Inactivate(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"Tag::Inactivate ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_Tag_Inactivate]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_Tag_Inactivate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(this.Id);
                        }
                        catch (FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NotImplementedException ex)
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
    }
}