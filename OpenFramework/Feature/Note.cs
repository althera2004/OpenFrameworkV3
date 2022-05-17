// --------------------------------
// <copyright file="Note.cs" company="OpenFramework">
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
    using System.Text;
    using System.Xml.Serialization;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;

    [Serializable]
    public partial class Note
    {
        public const long ItemGrantId = 1007;
        public const string ItemGrantCode = "N";
        public const string ItemGrantName = "CoreNotes";

        [XmlElement(Type = typeof(long), ElementName = "Id")]
        public long Id { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "CompanyId")]
        public long CompanyId { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Author")]
        public string Author { get; set; }

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

        public Note()
        {
        }

        public static Note Empty
        {
            get
            {
                return new Note
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    Author = string.Empty,
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
                        ""Author"":""{2}"",
                        ""Text"":""{3}"",
                        ""ItemDefinitionId"":{4},
                        ""ItemId"":{5},
                        ""CreatedBy"":{6},
                        ""CreatedOn"":""{7:dd/MM/yyyy hh:mm}"",
                        ""ModifiedBy"":{8},
                        ""ModifiedOn"":""{9:dd/MM/yyyy}"",
                        ""Active"":{10}
                    }}",
                    this.Id,
                    this.CompanyId,
                    Tools.Json.JsonCompliant(this.Author),
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

        public static string JsonList(ReadOnlyCollection<Note> list)
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

        public static ReadOnlyCollection<Note> All(string instanceName)
        {
            var res = new List<Note>();
            var user = ApplicationUser.Actual;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_Note_GetAll"))
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
                                    var newNote = new Note
                                    {
                                        Id = rdr.GetInt64(ColumnNotesGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnNotesGet.CompanyId),
                                        Author = rdr.GetString(ColumnNotesGet.Author),
                                        ItemDefinitionId = Constant.DefaultId,
                                        ItemId = Constant.DefaultId,
                                        Text = rdr.GetString(ColumnNotesGet.Text),
                                        Active = rdr.GetBoolean(ColumnNotesGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnNotesGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnNotesGet.CreatedBy),
                                                Name = rdr.GetString(ColumnNotesGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnNotesGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnNotesGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnNotesGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnNotesGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnNotesGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnNotesGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnNotesGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnNotesGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnNotesGet.ModifiedOn)
                                    };

                                    if (!rdr.IsDBNull(ColumnNotesGet.ItemDefinitionId))
                                    {
                                        newNote.ItemDefinitionId = rdr.GetInt64(ColumnNotesGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnNotesGet.ItemId))
                                    {
                                        newNote.ItemId = rdr.GetInt64(ColumnNotesGet.ItemId);
                                    }

                                    res.Add(newNote);
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

            return new ReadOnlyCollection<Note>(res);
        }

        public static ReadOnlyCollection<Note> ByItemId(long itemDefinitionId, long itemId, long companyId, string instanceName)
        {
            var res = new List<Note>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_Note_GetByItemId"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    var newNote = new Note
                                    {
                                        Id = rdr.GetInt64(ColumnNotesGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnNotesGet.CompanyId),
                                        Author = rdr.GetString(ColumnNotesGet.Author),
                                        ItemDefinitionId = Constant.DefaultId,
                                        ItemId = Constant.DefaultId,
                                        Text = rdr.GetString(ColumnNotesGet.Text),
                                        Active = rdr.GetBoolean(ColumnNotesGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnNotesGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnNotesGet.CreatedBy),
                                                Name = rdr.GetString(ColumnNotesGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnNotesGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnNotesGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnNotesGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnNotesGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnNotesGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnNotesGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnNotesGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnNotesGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnNotesGet.ModifiedOn)
                                    };

                                    if (!rdr.IsDBNull(ColumnNotesGet.ItemDefinitionId))
                                    {
                                        newNote.ItemDefinitionId = rdr.GetInt64(ColumnNotesGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnNotesGet.ItemId))
                                    {
                                        newNote.ItemId = rdr.GetInt64(ColumnNotesGet.ItemId);
                                    }

                                    res.Add(newNote);
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

            return new ReadOnlyCollection<Note>(res);
        }

        public static ReadOnlyCollection<Note> ByItemDefinitionId(long itemDefinitionId, string instanceName)
        {
            var res = new List<Note>();
            var user = ApplicationUser.Actual;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_Note_GetByItemDefinitionId"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", user.CompanyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    var newNote = new Note
                                    {
                                        Id = rdr.GetInt64(ColumnNotesGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnNotesGet.CompanyId),
                                        Author = rdr.GetString(ColumnNotesGet.Author),
                                        ItemDefinitionId = Constant.DefaultId,
                                        ItemId = Constant.DefaultId,
                                        Text = rdr.GetString(ColumnNotesGet.Text),
                                        Active = rdr.GetBoolean(ColumnNotesGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnNotesGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnNotesGet.CreatedBy),
                                                Name = rdr.GetString(ColumnNotesGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnNotesGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnNotesGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnNotesGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnNotesGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnNotesGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnNotesGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnNotesGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnNotesGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnNotesGet.ModifiedOn)
                                    };

                                    if (!rdr.IsDBNull(ColumnNotesGet.ItemDefinitionId))
                                    {
                                        newNote.ItemDefinitionId = rdr.GetInt64(ColumnNotesGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnNotesGet.ItemId))
                                    {
                                        newNote.ItemId = rdr.GetInt64(ColumnNotesGet.ItemId);
                                    }

                                    res.Add(newNote);
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

            return new ReadOnlyCollection<Note>(res);
        }

        public static Note ById(long id, string instanceName)
        {
            var res = Note.Empty;
            var user = ApplicationUser.Actual;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_Note_GetById"))
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
                                    res.Id = rdr.GetInt64(ColumnNotesGet.Id);
                                    res.CompanyId = rdr.GetInt64(ColumnNotesGet.CompanyId);
                                    res.ItemDefinitionId = Constant.DefaultId;
                                    res.ItemId = Constant.DefaultId;
                                    res.Text = rdr.GetString(ColumnNotesGet.Text);
                                    res.Active = rdr.GetBoolean(ColumnNotesGet.Active);
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnNotesGet.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnNotesGet.CreatedBy),
                                            Name = rdr.GetString(ColumnNotesGet.CreatedByName),
                                            LastName = rdr.GetString(ColumnNotesGet.CreatedByLastName),
                                            LastName2 = rdr.GetString(ColumnNotesGet.CreatedByLastName2)
                                        }
                                    };
                                    res.CreatedOn = rdr.GetDateTime(ColumnNotesGet.CreatedOn);
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnNotesGet.ModifiedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnNotesGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnNotesGet.ModifiedByName),
                                            LastName = rdr.GetString(ColumnNotesGet.ModifiedByLastName),
                                            LastName2 = rdr.GetString(ColumnNotesGet.ModifiedByLastName2)
                                        }
                                    };
                                    res.ModifiedOn = rdr.GetDateTime(ColumnNotesGet.ModifiedOn);

                                    if (!rdr.IsDBNull(ColumnNotesGet.ItemDefinitionId))
                                    {
                                        res.ItemDefinitionId = rdr.GetInt64(ColumnNotesGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnNotesGet.ItemId))
                                    {
                                        res.ItemId = rdr.GetInt64(ColumnNotesGet.ItemId);
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

        public ActionResult Insert(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"Note::insert ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_Note_Insert]
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @Author nvarchar(150),
             *   @Text nvarchar(2000),
             *   @ItemDefintionId bigint,
             *   @ItemId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_Note_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Author", this.Author, 150));
                        cmd.Parameters.Add(DataParameter.Input("@Text", this.Text, 2000));
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

        public ActionResult Update(long applicationUserId,string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"Note::update ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_Note_Update]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @Text nvarchar(2000),
             *   @ItemDefintionId bigint,
             *   @ItemId bigint,
             *   @ApplicationUserId bigint
             *   @ModifiedOn datetime */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_Note_Update"))
                {
                    using (var cnn = new SqlConnection(cns))
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
            var source = string.Format(CultureInfo.InvariantCulture, @"Note::Activate ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_Note_Activate]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint 
             *   @ModifiedOn nvarchar(20) */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_Note_Activate"))
                {
                    using (var cnn = new SqlConnection(cns))
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
            var source = string.Format(CultureInfo.InvariantCulture, @"Note::Inactivate ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_Note_Inactivate]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint
             *   @ModifiedOn nvarchar(20) */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_Note_Inactivate"))
                {
                    using (var cnn = new SqlConnection(cns))
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