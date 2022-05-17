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

    public partial class ContactPerson
    {
        public const long ItemGrantId = 1010;
        public const string ItemGrantCode = "C";
        public const string ItemGrantName = "ContactPerson";

        [XmlElement(Type = typeof(long), ElementName = "Id")]
        public long Id { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "CompanyId")]
        public long CompanyId { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "FirstName")]
        public string FirstName { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "LastName")]
        public string LastName { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "LastName2")]
        public string LastName2 { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "PhoneJob")]
        public string PhoneJob { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "PhonePersonal")]
        public string PhonePersonal { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "PhoneEmergency")]
        public string PhoneEmergency { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Email1")]
        public string Email1 { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Email2")]
        public string Email2 { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "JobPosition")]
        public string JobPosition { get; set; }

        [XmlElement(Type = typeof(bool), ElementName = "Principal")]
        public bool Principal { get; set; }

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

        [XmlElement(Type = typeof(string), ElementName = "FullName")]
        public string FullName
        {
            get
            {
                var res = string.Empty;

                if (!string.IsNullOrEmpty(this.FirstName))
                {
                    res = this.FirstName;
                }

                if (!string.IsNullOrEmpty(this.LastName))
                {
                    if (!string.IsNullOrEmpty(res))
                    {
                        res += " ";
                    }

                    res += this.LastName;
                }

                if (!string.IsNullOrEmpty(this.LastName2))
                {
                    if (!string.IsNullOrEmpty(res))
                    {
                        res += " ";
                    }

                    res += this.LastName2;
                }

                return res;
            }
        }

        public ContactPerson()
        {
        }

        public static ContactPerson Empty
        {
            get
            {
                return new ContactPerson
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    LastName2 = string.Empty,
                    JobPosition = string.Empty,
                    Email1 = string.Empty,
                    Email2 = string.Empty,
                    PhoneJob = string.Empty,
                    PhonePersonal = string.Empty,
                    PhoneEmergency = string.Empty,
                    Principal = false,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
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
                        ""CompanyId"": {1},
                        ""FirstName"":""{2}"",
                        ""LastName"":""{3}"",
                        ""LastName2"":""{4}"",
                        ""FullName"":""{5}"",
                        ""Principal"":{6},
                        ""JobPosition"":""{7}"",
                        ""PhoneJob"":""{8}"",
                        ""PhonePersonal"":""{9}"",
                        ""PhoneEmergency"":""{10}"",
                        ""Email1"":""{11}"",
                        ""Email2"":""{12}"",
                        ""ItemDefinitionId"":{13},
                        ""ItemId"":{14},
                        ""Active"":{15}}}",
                    this.Id,
                    this.CompanyId,
                    Tools.Json.JsonCompliant(this.FirstName),
                    Tools.Json.JsonCompliant(this.LastName),
                    Tools.Json.JsonCompliant(this.LastName2),
                    Tools.Json.JsonCompliant(this.FullName),
                    ConstantValue.Value(this.Principal),
                    Tools.Json.JsonCompliant(this.JobPosition),
                    Tools.Json.JsonCompliant(this.PhoneJob),
                    Tools.Json.JsonCompliant(this.PhonePersonal),
                    Tools.Json.JsonCompliant(this.PhoneEmergency),
                    Tools.Json.JsonCompliant(this.Email1),
                    Tools.Json.JsonCompliant(this.Email2),
                    this.ItemDefinitionId,
                    this.ItemId,
                    ConstantValue.Value(this.Active));
            }
        }

        public static string JsonList(ReadOnlyCollection<ContactPerson> list)
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

        public static ContactPerson ById(long id, long itemId, string instanceName)
        {
            var res = ContactPerson.Empty;
            var user = ApplicationUser.Actual;
            var cns = Persistence.ConnectionString(instanceName);
            /* CREATE PROCEDURE Feature_ContactPerson_GetById
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_ContactPerson_GetById"))
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
                                    res.Id = rdr.GetInt64(ColumnsContactPersonGet.Id);
                                    res.CompanyId = rdr.GetInt64(ColumnsContactPersonGet.CompanyId);
                                    res.FirstName = rdr.GetString(ColumnsContactPersonGet.FirstName);
                                    res.LastName = rdr.GetString(ColumnsContactPersonGet.LastName);
                                    res.LastName2 = rdr.GetString(ColumnsContactPersonGet.LastName2);
                                    res.JobPosition = rdr.GetString(ColumnsContactPersonGet.JobPosition);
                                    res.Email1 = rdr.GetString(ColumnsContactPersonGet.Email1);
                                    res.Email2 = rdr.GetString(ColumnsContactPersonGet.Email2);
                                    res.PhoneJob = rdr.GetString(ColumnsContactPersonGet.PhoneJob);
                                    res.PhonePersonal = rdr.GetString(ColumnsContactPersonGet.PhonePersonal);
                                    res.PhoneEmergency = rdr.GetString(ColumnsContactPersonGet.PhoneEmergency);
                                    res.Principal = rdr.GetBoolean(ColumnsContactPersonGet.Principal);
                                    res.Active = rdr.GetBoolean(ColumnsContactPersonGet.Active);
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsContactPersonGet.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsContactPersonGet.CreatedBy),
                                            Name = rdr.GetString(ColumnsContactPersonGet.CreatedByName),
                                            LastName = rdr.GetString(ColumnsContactPersonGet.CreatedByLastName),
                                            LastName2 = rdr.GetString(ColumnsContactPersonGet.CreatedByLastName2)
                                        }
                                    };
                                    res.CreatedOn = rdr.GetDateTime(ColumnsContactPersonGet.CreatedOn);
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsContactPersonGet.ModifiedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsContactPersonGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnsContactPersonGet.ModifiedByName),
                                            LastName = rdr.GetString(ColumnsContactPersonGet.ModifiedByLastName),
                                            LastName2 = rdr.GetString(ColumnsContactPersonGet.ModifiedByLastName2)
                                        }
                                    };
                                    res.ModifiedOn = rdr.GetDateTime(ColumnsContactPersonGet.ModifiedOn);
                                    res.ItemDefinitionId = rdr.GetInt64(ColumnsContactPersonGet.ItemDefinitionId);
                                    res.ItemId = rdr.GetInt64(ColumnsContactPersonGet.ItemId);
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

        public static ReadOnlyCollection<ContactPerson> ByItemId(long itemDefinitionId, long companyId, long itemId, string instanceName)
        {
            var res = new List<ContactPerson>();
            var cns = Persistence.ConnectionString(instanceName);
            /* CREATE PROCEDURE Feature_ContactPerson_GetByItemId
             *   @ItemId bigint,
             *   @ItemDefinitionId bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_ContactPerson_GetByItemId"))
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
                                    res.Add(new ContactPerson
                                    {
                                        Id = rdr.GetInt64(ColumnsContactPersonGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsContactPersonGet.CompanyId),
                                        FirstName = rdr.GetString(ColumnsContactPersonGet.FirstName),
                                        LastName = rdr.GetString(ColumnsContactPersonGet.LastName),
                                        LastName2 = rdr.GetString(ColumnsContactPersonGet.LastName2),
                                        JobPosition = rdr.GetString(ColumnsContactPersonGet.JobPosition),
                                        Email1 = rdr.GetString(ColumnsContactPersonGet.Email1),
                                        Email2 = rdr.GetString(ColumnsContactPersonGet.Email2),
                                        PhoneJob = rdr.GetString(ColumnsContactPersonGet.PhoneJob),
                                        PhonePersonal = rdr.GetString(ColumnsContactPersonGet.PhonePersonal),
                                        PhoneEmergency = rdr.GetString(ColumnsContactPersonGet.PhoneEmergency),
                                        Principal = rdr.GetBoolean(ColumnsContactPersonGet.Principal),
                                        Active = rdr.GetBoolean(ColumnsContactPersonGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsContactPersonGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsContactPersonGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsContactPersonGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsContactPersonGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsContactPersonGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsContactPersonGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsContactPersonGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsContactPersonGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsContactPersonGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsContactPersonGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsContactPersonGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsContactPersonGet.ModifiedOn),
                                        ItemDefinitionId = rdr.GetInt64(ColumnsContactPersonGet.ItemDefinitionId),
                                        ItemId = rdr.GetInt64(ColumnsContactPersonGet.ItemId)
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

            return new ReadOnlyCollection<ContactPerson>(res);
        }

        public ActionResult Save(long applicationUserId, string instanceName)
        {
            if (this.Id > 0)
            {
                return Update(applicationUserId, instanceName);
            }
            else
            {
                return Insert(applicationUserId, instanceName);
            }
        }

        public ActionResult Insert(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            /* CREATE PROCEDURE Feature_ContactPerson_Insert
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @FirstName nvarchar(100),
             *   @LastName nvarchar(100),
             *   @LastName2 nvarchar(100),
             *   @JobPosition nvarchar(100),
             *   @Email1 nvarchar(150),
             *   @Email2 nvarchar(150),
             *   @PhoneJob nchar(20),
             *   @PhonePersonal nchar(20),
             *   @PhoneEmergency nchar(20),
             *   @Principal bit,
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @ApplicationUserId bigint */
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_ContactPerson_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@FirstName", this.FirstName, 100));
                        cmd.Parameters.Add(DataParameter.Input("@LastName", this.LastName, 100));
                        cmd.Parameters.Add(DataParameter.Input("@LastName2", this.LastName2, 100));
                        cmd.Parameters.Add(DataParameter.Input("@JobPosition", this.JobPosition, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Email1", this.Email1, 150));
                        cmd.Parameters.Add(DataParameter.Input("@Email2", this.Email2, 150));
                        cmd.Parameters.Add(DataParameter.Input("@PhoneJob", this.PhoneJob, 20));
                        cmd.Parameters.Add(DataParameter.Input("@PhonePersonal", this.PhonePersonal, 20));
                        cmd.Parameters.Add(DataParameter.Input("@PhoneEmergency", this.PhoneEmergency, 20));
                        cmd.Parameters.Add(DataParameter.Input("@Principal", this.Principal));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value.ToString());
                            res.SetSuccess(this.Id);
                        }
                        catch (Exception ex)
                        {
                            res.SetFail(ex);
                            ExceptionManager.Trace(ex, "Feature_ContactPerson(INSERT)");
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
            var cns = Persistence.ConnectionString(instanceName);
            /* CREATE PROCEDURE Feature_ContactPerson_Update
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @FirstName nvarchar(100),
             *   @LastName nvarchar(100),
             *   @LastName2 nvarchar(100),
             *   @JobPosition nvarchar(100),
             *   @Email1 nvarchar(150),
             *   @Email2 nvarchar(150),
             *   @PhoneJob nchar(20),
             *   @PhonePersonal nchar(20),
             *   @PhoneEmergency nchar(20),
             *   @Principal bit,
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @ApplicationUserId bigint */
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_ContactPerson_Update"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@FirstName", this.FirstName, 100));
                        cmd.Parameters.Add(DataParameter.Input("@LastName", this.LastName, 100));
                        cmd.Parameters.Add(DataParameter.Input("@LastName2", this.LastName2, 100));
                        cmd.Parameters.Add(DataParameter.Input("@JobPosition", this.JobPosition, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Email1", this.Email1, 150));
                        cmd.Parameters.Add(DataParameter.Input("@Email2", this.Email2, 150));
                        cmd.Parameters.Add(DataParameter.Input("@PhoneJob", this.PhoneJob, 20));
                        cmd.Parameters.Add(DataParameter.Input("@PhonePersonal", this.PhonePersonal, 20));
                        cmd.Parameters.Add(DataParameter.Input("@PhoneEmergency", this.PhoneEmergency, 20));
                        cmd.Parameters.Add(DataParameter.Input("@Principal", this.Principal));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value.ToString());
                            res.SetSuccess(this.Id);
                        }
                        catch (Exception ex)
                        {
                            res.SetFail(ex);
                            ExceptionManager.Trace(ex, "Feature_ContactPerson(INSERT)");
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

        public static ActionResult Activate(long contactPersonId, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"Feature_ContactPerson::Activate ==> {0}", contactPersonId);
            /* CREATE PROCEDURE [dbo].[Feature_ContactPerson_Activate]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_ContactPerson_Activate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", contactPersonId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(contactPersonId);
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

        public static  ActionResult Inactivate(long contactPersonId, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"Feature_ContactPerson::Inactivate ==> {0}", contactPersonId);
            /* CREATE PROCEDURE [dbo].[Feature_ContactPerson_Inactivate]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_ContactPerson_Inactivate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", contactPersonId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(contactPersonId);
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