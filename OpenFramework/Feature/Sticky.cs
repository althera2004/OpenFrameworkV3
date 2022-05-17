// --------------------------------
// <copyright file="Sticky.cs" company="OpenFramework">
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
    using System.Xml.Serialization;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;

    [Serializable]
    public partial class Sticky
    {
        [XmlElement(Type = typeof(long), ElementName = "Id")]
        public long Id { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "CompanyId")]
        public long CompanyId { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "Author")]
        public long Author { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Target")]
        public string Target { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Text")]
        public string Text { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "ItemDefinitionId")]
        public long ItemDefinitionId { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "ItemId")]
        public long ItemId { get; set; }

        [XmlElement(Type = typeof(ApplicationUser), ElementName = "CreatedBy")]
        public ApplicationUser CreatedBy { get; set; }

        [XmlElement(Type = typeof(ApplicationUser), ElementName = "ModifiedBy")]
        public ApplicationUser ModifiedBy { get; set; }

        [XmlElement(Type = typeof(DateTime), ElementName = "CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [XmlElement(Type = typeof(DateTime), ElementName = "ModifiedOn")]
        public DateTime ModifiedOn { get; set; }

        [XmlElement(Type = typeof(bool), ElementName = "Active")]
        public bool Active { get; set; }

        public Sticky()
        {
        }

        public static Sticky Empty
        {
            get
            {
                return new Sticky
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    Author = Constant.DefaultId,
                    Text = string.Empty,
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

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"":{0},
                        ""CompanyId"":{1},
                        ""Author"":{2},
                        ""Target"":""{3}"",
                        ""Text"":""{4}"",
                        ""ItemDefinitionId"":{5},
                        ""ItemId"":{6},
                        ""CreatedBy"":{7},
                        ""CreatedOn"":""{8:dd/MM/yyyy hh:mm}"",
                        ""ModifiedBy"":{9},
                        ""ModifiedOn"":""{10:dd/MM/yyyy}"",
                        ""Active"":{11}
                    }}",
                    this.Id,
                    this.CompanyId,
                    this.Author,
                    Tools.Json.JsonCompliant(this.Target),
                    Tools.Json.JsonCompliant(this.Text),
                    this.ItemDefinitionId,
                    this.ItemId,
                    this.CreatedBy.JsonSimple,
                    this.CreatedOn,
                    this.ModifiedBy.JsonSimple,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static string JsonList(ReadOnlyCollection<Sticky> list)
        {
            var res = new StringBuilder("[");
            if (list != null && list.Count > 0)
            {
                bool first = true;
                foreach (var faq in list)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(faq.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<Sticky> All(string instanceName)
        {
            var res = new List<Sticky>();
            var user = ApplicationUser.Actual;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_Sticky_GetAll"))
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
                                    var newSticky = new Sticky
                                    {
                                        Id = rdr.GetInt64(ColumnStickyGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnStickyGet.CompanyId),
                                        Author = rdr.GetInt64(ColumnStickyGet.Author),
                                        ItemDefinitionId = Constant.DefaultId,
                                        ItemId = Constant.DefaultId,
                                        Text = rdr.GetString(ColumnStickyGet.Text),
                                        Active = rdr.GetBoolean(ColumnStickyGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnStickyGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnStickyGet.CreatedBy),
                                                Name = rdr.GetString(ColumnStickyGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnStickyGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnStickyGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnStickyGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnStickyGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnStickyGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnStickyGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnStickyGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnStickyGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnStickyGet.ModifiedOn)
                                    };

                                    if (!rdr.IsDBNull(ColumnStickyGet.ItemDefinitionId))
                                    {
                                        newSticky.ItemDefinitionId = rdr.GetInt64(ColumnStickyGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnStickyGet.ItemId))
                                    {
                                        newSticky.ItemId = rdr.GetInt64(ColumnStickyGet.ItemId);
                                    }

                                    res.Add(newSticky);
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

            return new ReadOnlyCollection<Sticky>(res);
        }

        public static ReadOnlyCollection<Sticky> ByItemId(long itemDefinitionId, long itemId, long companyId, string instanceName)
        {
            var res = new List<Sticky>();
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Feature_Sticky_GetByItemId"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                while(rdr.Read())
                                {
                                    var newSticky = new Sticky
                                    {
                                        Id = rdr.GetInt64(ColumnStickyGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnStickyGet.CompanyId),
                                        Author = rdr.GetInt64(ColumnStickyGet.Author),
                                        Target = rdr.GetString(ColumnStickyGet.Target),
                                        ItemDefinitionId = Constant.DefaultId,
                                        ItemId = Constant.DefaultId,
                                        Text = rdr.GetString(ColumnStickyGet.Text),
                                        Active = rdr.GetBoolean(ColumnStickyGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnStickyGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnStickyGet.CreatedBy),
                                                Name = rdr.GetString(ColumnStickyGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnStickyGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnStickyGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnStickyGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnStickyGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnStickyGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnStickyGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnStickyGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnStickyGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnStickyGet.ModifiedOn)
                                    };

                                    if(!rdr.IsDBNull(ColumnStickyGet.ItemDefinitionId))
                                    {
                                        newSticky.ItemDefinitionId = rdr.GetInt64(ColumnStickyGet.ItemDefinitionId);
                                    }

                                    if(!rdr.IsDBNull(ColumnStickyGet.ItemId))
                                    {
                                        newSticky.ItemId = rdr.GetInt64(ColumnStickyGet.ItemId);
                                    }

                                    res.Add(newSticky);
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

            return new ReadOnlyCollection<Sticky>(res);
        }

        public static ReadOnlyCollection<Sticky> ByItemDefinitionId(long itemDefinitionId, string instanceName)
        {
            var res = new List<Sticky>();
            var user = ApplicationUser.Actual;
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Feature_Sticky_GetByItemDefinitionId"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", user.CompanyId));
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                while(rdr.Read())
                                {
                                    var newSticky = new Sticky
                                    {
                                        Id = rdr.GetInt64(ColumnStickyGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnStickyGet.CompanyId),
                                        Author = rdr.GetInt64(ColumnStickyGet.Author),
                                        Target = rdr.GetString(ColumnStickyGet.Target),
                                        ItemDefinitionId = Constant.DefaultId,
                                        ItemId = Constant.DefaultId,
                                        Text = rdr.GetString(ColumnStickyGet.Text),
                                        Active = rdr.GetBoolean(ColumnStickyGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnStickyGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnStickyGet.CreatedBy),
                                                Name = rdr.GetString(ColumnStickyGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnStickyGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnStickyGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnStickyGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnStickyGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnStickyGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnStickyGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnStickyGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnStickyGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnStickyGet.ModifiedOn)
                                    };

                                    if(!rdr.IsDBNull(ColumnStickyGet.ItemDefinitionId))
                                    {
                                        newSticky.ItemDefinitionId = rdr.GetInt64(ColumnStickyGet.ItemDefinitionId);
                                    }

                                    if(!rdr.IsDBNull(ColumnStickyGet.ItemId))
                                    {
                                        newSticky.ItemId = rdr.GetInt64(ColumnStickyGet.ItemId);
                                    }

                                    res.Add(newSticky);
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

            return new ReadOnlyCollection<Sticky>(res);
        }

        public static Sticky ById(long id, string instanceName)
        {
            var res = Sticky.Empty;
            var user = ApplicationUser.Actual;
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Feature_Sticky_GetById"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", user.CompanyId));
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                while(rdr.Read())
                                {
                                    res.Id = rdr.GetInt64(ColumnStickyGet.Id);
                                    res.CompanyId = rdr.GetInt64(ColumnStickyGet.CompanyId);
                                    res.Author = rdr.GetInt64(ColumnStickyGet.Author);
                                    res.Target = rdr.GetString(ColumnStickyGet.Target);
                                    res.ItemDefinitionId = Constant.DefaultId;
                                    res.ItemId = Constant.DefaultId;
                                    res.Text = rdr.GetString(ColumnStickyGet.Text);
                                    res.Active = rdr.GetBoolean(ColumnStickyGet.Active);
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnStickyGet.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnStickyGet.CreatedBy),
                                            Name = rdr.GetString(ColumnStickyGet.CreatedByName),
                                            LastName = rdr.GetString(ColumnStickyGet.CreatedByLastName),
                                            LastName2 = rdr.GetString(ColumnStickyGet.CreatedByLastName2)
                                        }
                                    };
                                    res.CreatedOn = rdr.GetDateTime(ColumnStickyGet.CreatedOn);
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnStickyGet.ModifiedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnStickyGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnStickyGet.ModifiedByName),
                                            LastName = rdr.GetString(ColumnStickyGet.ModifiedByLastName),
                                            LastName2 = rdr.GetString(ColumnStickyGet.ModifiedByLastName2)
                                        }
                                    };
                                    res.ModifiedOn = rdr.GetDateTime(ColumnStickyGet.ModifiedOn);

                                    if(!rdr.IsDBNull(ColumnStickyGet.ItemDefinitionId))
                                    {
                                        res.ItemDefinitionId = rdr.GetInt64(ColumnStickyGet.ItemDefinitionId);
                                    }

                                    if(!rdr.IsDBNull(ColumnStickyGet.ItemId))
                                    {
                                        res.ItemId = rdr.GetInt64(ColumnStickyGet.ItemId);
                                    }
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

            return res;
        }

        public ActionResult Insert(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"Sticky::insert ==> {0}", this.Text);
            /* CREATE PROCEDURE [dbo].[Feature_Sticky_Insert]
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @Target nchar(100),
             *   @Text nvarchar(2000),
             *   @ItemDefintionId bigint,
             *   @ItemId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Feature_Sticky_Insert"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Text", this.Text, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@Target", this.Target, 100));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@CreatedOn", DateTime.Now.ToUniversalTime()));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(this.Id);
                        }
                        catch(FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch(SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch(NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch(NotImplementedException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
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

        public ActionResult Update(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"Sticky::update ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_Sticky_Update]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @Text nvarchar(2000),
             *   @ItemDefintionId bigint,
             *   @ItemId bigint,
             *   @ApplicationUserId bigint
             *   @ModifiedOn datetime */
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Feature_Sticky_Update"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Text", this.Text, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@ModifiedOn", DateTime.Now.ToUniversalTime()));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(this.Id);
                        }
                        catch(FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch(SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch(NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch(NotImplementedException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
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

        public ActionResult Activate(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"Sticky::Activate ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_Sticky_Activate]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint 
             *   @ModifiedOn nvarchar(20) */
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Feature_Sticky_Activate"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.InputDateNow("@MofiedOn"));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(this.Id);
                        }
                        catch(FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch(SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch(NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch(NotImplementedException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
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

        public static ActionResult Inactivate(long id,long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"Sticky::Inactivate ==> {0}", id);
            /* CREATE PROCEDURE [dbo].[Feature_Sticky_Inactivate]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint
             *   @ModifiedOn nvarchar(20) */
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Feature_Sticky_Inactivate"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(id);
                        }
                        catch(FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch(SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch(NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch(NotImplementedException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
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