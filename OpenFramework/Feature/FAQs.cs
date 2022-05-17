// --------------------------------
// <copyright file="FAQs.cs" company="OpenFramework">
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
    public partial class FAQs
    {
        public const long ItemGrantId = 1000;
        public const string ItemGrantCode = "F";
        public const string ItemGrantName = "CoreFAQs";

        [XmlElement(Type = typeof(long), ElementName = "Id")]
        public long Id { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "CompanyId")]
        public long CompanyId { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Question")]
        public string Question { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Answer")]
        public string Answer { get; set; }

        [XmlElement(Type = typeof(bool), ElementName = "Derivated")]
        public bool Derivated { get; set; }

        [XmlElement(Type = typeof(bool), ElementName = "Published")]
        public bool Published { get; set; }

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

        public FAQs()
        {
        }

        public static FAQs Empty
        {
            get
            {
                return new FAQs
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    Question = string.Empty,
                    Answer = string.Empty,
                    ItemDefinitionId = Constant.DefaultId,
                    ItemId = Constant.DefaultId,
                    Derivated = false,
                    Published = false,
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
                        ""Question"":""{2}"",
                        ""Answer"":""{3}"",
                        ""ItemDefinitionId"":{4},
                        ""ItemId"":{5},
                        ""Derivated"":{6},
                        ""Published"":{7},
                        ""CreatedBy"":{8},
                        ""CreatedOn"":""{9:dd/MM/yyyy}"",
                        ""ModifiedBy"":{10},
                        ""ModifiedOn"":""{11:dd/MM/yyyy}"",
                        ""Active"":{12}
                    }}",
                    this.Id,
                    this.CompanyId,
                    Tools.Json.JsonCompliant(this.Question),
                    Tools.Json.JsonCompliant(this.Answer),
                    this.ItemDefinitionId,
                    this.ItemId,
                    ConstantValue.Value(this.Derivated),
                    ConstantValue.Value(this.Published),
                    this.CreatedBy.JsonSimple,
                    this.CreatedOn,
                    this.ModifiedBy.JsonSimple,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static string JsonList(ReadOnlyCollection<FAQs> list)
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

        public static ReadOnlyCollection<FAQs> All(string instanceName)
        {
            var res = new List<FAQs>();
            var user = ApplicationUser.Actual;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_FAQ_GetAll"))
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
                                    var newFAQs = new FAQs
                                    {
                                        Id = rdr.GetInt64(ColumnsFAQsGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsFAQsGet.CompanyId),
                                        ItemDefinitionId = Constant.DefaultId,
                                        ItemId = Constant.DefaultId,
                                        Question = rdr.GetString(ColumnsFAQsGet.Question),
                                        Answer = rdr.GetString(ColumnsFAQsGet.Answer),
                                        Published = rdr.GetBoolean(ColumnsFAQsGet.Published),
                                        Derivated = rdr.GetBoolean(ColumnsFAQsGet.Derivated),
                                        Active = rdr.GetBoolean(ColumnsFAQsGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsFAQsGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsFAQsGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsFAQsGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsFAQsGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsFAQsGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsFAQsGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsFAQsGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsFAQsGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsFAQsGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsFAQsGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsFAQsGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsFAQsGet.ModifiedOn)
                                    };

                                    if (!rdr.IsDBNull(ColumnsFAQsGet.ItemDefinitionId))
                                    {
                                        newFAQs.ItemDefinitionId = rdr.GetInt64(ColumnsFAQsGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsFAQsGet.ItemId))
                                    {
                                        newFAQs.ItemId = rdr.GetInt64(ColumnsFAQsGet.ItemId);
                                    }

                                    res.Add(newFAQs);
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

            return new ReadOnlyCollection<FAQs>(res);
        }

        public static ReadOnlyCollection<FAQs> ByItemId(long itemDefinitionId, long itemId, string instanceName)
        {
            var res = new List<FAQs>();
            var user = ApplicationUser.Actual;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_FAQ_GetByItemId"))
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
                                while (rdr.Read())
                                {
                                    var newFAQs = new FAQs
                                    {
                                        Id = rdr.GetInt64(ColumnsFAQsGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsFAQsGet.CompanyId),
                                        ItemDefinitionId = Constant.DefaultId,
                                        ItemId = Constant.DefaultId,
                                        Question = rdr.GetString(ColumnsFAQsGet.Question),
                                        Answer = rdr.GetString(ColumnsFAQsGet.Answer),
                                        Published = rdr.GetBoolean(ColumnsFAQsGet.Published),
                                        Derivated = rdr.GetBoolean(ColumnsFAQsGet.Derivated),
                                        Active = rdr.GetBoolean(ColumnsFAQsGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsFAQsGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsFAQsGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsFAQsGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsFAQsGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsFAQsGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsFAQsGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsFAQsGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsFAQsGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsFAQsGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsFAQsGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsFAQsGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsFAQsGet.ModifiedOn)
                                    };

                                    if (!rdr.IsDBNull(ColumnsFAQsGet.ItemDefinitionId))
                                    {
                                        newFAQs.ItemDefinitionId = rdr.GetInt64(ColumnsFAQsGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsFAQsGet.ItemId))
                                    {
                                        newFAQs.ItemId = rdr.GetInt64(ColumnsFAQsGet.ItemId);
                                    }

                                    res.Add(newFAQs);
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

            return new ReadOnlyCollection<FAQs>(res);
        }

        public static ReadOnlyCollection<FAQs> ByItemDefinitionId(long itemDefinitionId, string instanceName)
        {
            var res = new List<FAQs>();
            var user = ApplicationUser.Actual;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_FAQ_GetbyItemDefinitionId"))
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
                                    var newFAQs = new FAQs
                                    {
                                        Id = rdr.GetInt64(ColumnsFAQsGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsFAQsGet.CompanyId),
                                        ItemDefinitionId = Constant.DefaultId,
                                        ItemId = Constant.DefaultId,
                                        Question = rdr.GetString(ColumnsFAQsGet.Question),
                                        Answer = rdr.GetString(ColumnsFAQsGet.Answer),
                                        Published = rdr.GetBoolean(ColumnsFAQsGet.Published),
                                        Derivated = rdr.GetBoolean(ColumnsFAQsGet.Derivated),
                                        Active = rdr.GetBoolean(ColumnsFAQsGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsFAQsGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsFAQsGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsFAQsGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsFAQsGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsFAQsGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsFAQsGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsFAQsGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsFAQsGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsFAQsGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsFAQsGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsFAQsGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsFAQsGet.ModifiedOn)
                                    };

                                    if (!rdr.IsDBNull(ColumnsFAQsGet.ItemDefinitionId))
                                    {
                                        newFAQs.ItemDefinitionId = rdr.GetInt64(ColumnsFAQsGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsFAQsGet.ItemId))
                                    {
                                        newFAQs.ItemId = rdr.GetInt64(ColumnsFAQsGet.ItemId);
                                    }

                                    res.Add(newFAQs);
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

            return new ReadOnlyCollection<FAQs>(res);
        }

        public static FAQs ById(long id, long companyId, string instanceName)
        {
            var res = FAQs.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_FAQ_GetById"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Id = rdr.GetInt64(ColumnsFAQsGet.Id);
                                    res.CompanyId = rdr.GetInt64(ColumnsFAQsGet.CompanyId);
                                    res.ItemDefinitionId = Constant.DefaultId;
                                    res.ItemId = Constant.DefaultId;
                                    res.Question = rdr.GetString(ColumnsFAQsGet.Question);
                                    res.Answer = rdr.GetString(ColumnsFAQsGet.Answer);
                                    res.Derivated = rdr.GetBoolean(ColumnsFAQsGet.Derivated);
                                    res.Published = rdr.GetBoolean(ColumnsFAQsGet.Published);
                                    res.Active = rdr.GetBoolean(ColumnsFAQsGet.Active);
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsFAQsGet.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsFAQsGet.CreatedBy),
                                            Name = rdr.GetString(ColumnsFAQsGet.CreatedByName),
                                            LastName = rdr.GetString(ColumnsFAQsGet.CreatedByLastName),
                                            LastName2 = rdr.GetString(ColumnsFAQsGet.CreatedByLastName2)
                                        }
                                    };
                                    res.CreatedOn = rdr.GetDateTime(ColumnsFAQsGet.CreatedOn);
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsFAQsGet.ModifiedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsFAQsGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnsFAQsGet.ModifiedByName),
                                            LastName = rdr.GetString(ColumnsFAQsGet.ModifiedByLastName),
                                            LastName2 = rdr.GetString(ColumnsFAQsGet.ModifiedByLastName2)
                                        }
                                    };
                                    res.ModifiedOn = rdr.GetDateTime(ColumnsFAQsGet.ModifiedOn);

                                    if (!rdr.IsDBNull(ColumnsFAQsGet.ItemDefinitionId))
                                    {
                                        res.ItemDefinitionId = rdr.GetInt64(ColumnsFAQsGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsFAQsGet.ItemId))
                                    {
                                        res.ItemId = rdr.GetInt64(ColumnsFAQsGet.ItemId);
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
            var source = string.Format(CultureInfo.InvariantCulture, @"FAQ::insert ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_FAQ_Insert]
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @Question nvarchar(500),
             *   @Answer nvarchar(1000),
             *   @ItemDefintionId bigint,
             *   @ItemId bigint,
             *   @Derivated bit,
             *   @Published bit,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_FAQ_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Question", this.Question, 500));
                        cmd.Parameters.Add(DataParameter.Input("@Answer", this.Answer, 1000));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@Derivated", this.Derivated));
                        cmd.Parameters.Add(DataParameter.Input("@Published", this.Published));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
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

        public ActionResult Update(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"FAQ::update ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_FAQ_Update]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @Question nvarchar(500),
             *   @Answer nvarchar(1000),
             *   @ItemDefintionId bigint,
             *   @ItemId bigint,
             *   @Derivated bit,
             *   @Published bit,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_FAQ_Update"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Question", this.Question, 500));
                        cmd.Parameters.Add(DataParameter.Input("@Answer", this.Answer, 1000));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@Derivated", this.Derivated));
                        cmd.Parameters.Add(DataParameter.Input("@Published", this.Published));
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

        public ActionResult Publish(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"FAQ::publish ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_FAQ_Publish]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_FAQ_Publish"))
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

        public ActionResult Unpublish(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"FAQ::Unpublish ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_FAQ_Unpublish]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_FAQ_Unpublish"))
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

        public ActionResult Activate(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"FAQ::Activate ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_FAQ_Activate]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_FAQ_Activate"))
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
            var source = string.Format(CultureInfo.InvariantCulture, @"FAQ::Inactivate ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_FAQ_Inactivate]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_FAQ_Inactivate"))
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