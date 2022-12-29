// --------------------------------
// <copyright file="SecurityConfiguration.cs" company="OpenFramework">
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

    public partial class ContactPerson
    {
        public const long ItemGrantId = 1010;
        public const string ItemGrantCode = "C";
        public const string ItemGrantName = "ContactPerson";

        [XmlElement(Type = typeof(long), ElementName = "Id")]
        public long Id { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "FirstName")]
        public string FirstName { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "LastName")]
        public string LastName { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Phone")]
        public string Phone { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Mobile")]
        public string Mobile { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Email")]
        public string Email { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Email2")]
        public string Email2 { get; set; }

        [XmlElement(Type = typeof(int), ElementName = "JobPosition")]
        public int JobPosition { get; set; }

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
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    JobPosition = 0,
                    Email = string.Empty,
                    Email2 = string.Empty,
                    Phone = string.Empty,
                    Mobile = string.Empty,
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
                        ""FirstName"":""{1}"",
                        ""LastName"":""{2}"",
                        ""FullName"":""{3}"",
                        ""JobPosition"":{4},
                        ""Phone"":""{5}"",
                        ""Mobile"":""{6}"",
                        ""Email"":""{7}"",
                        ""Email2"":""{8}"",
                        ""ItemDefinitionId"":{9},
                        ""ItemId"":{10},
                        ""Active"":{11}}}",
                    this.Id,
                    Tools.Json.JsonCompliant(this.FirstName),
                    Tools.Json.JsonCompliant(this.LastName),
                    Tools.Json.JsonCompliant(this.FullName),
                    Tools.Json.JsonCompliant(this.JobPosition),
                    Tools.Json.JsonCompliant(this.Phone),
                    Tools.Json.JsonCompliant(this.Mobile),
                    Tools.Json.JsonCompliant(this.Email),
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
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_ContactPerson_ById"))
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
                                    res.FirstName = rdr.GetString(ColumnsContactPersonGet.FirstName);
                                    res.LastName = rdr.GetString(ColumnsContactPersonGet.LastName);
                                    res.JobPosition = rdr.GetInt32(ColumnsContactPersonGet.JobPosition);
                                    res.Email = rdr.GetString(ColumnsContactPersonGet.Email);
                                    res.Email2 = rdr.GetString(ColumnsContactPersonGet.Email2);
                                    res.Phone = rdr.GetString(ColumnsContactPersonGet.Phone).Trim();
                                    res.Mobile = rdr.GetString(ColumnsContactPersonGet.Mobile).Trim();
                                    res.Active = rdr.GetBoolean(ColumnsContactPersonGet.Active);
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsContactPersonGet.CreatedBy),
                                        Profile = new UserProfile
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
                                        Profile = new UserProfile
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

        public static ReadOnlyCollection<ContactPerson> ByItemId(long itemDefinitionId, long itemId, string instanceName)
        {
            var res = new List<ContactPerson>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_ContactPerson_ByItemId"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinitionId));
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
                                        FirstName = rdr.GetString(ColumnsContactPersonGet.FirstName),
                                        LastName = rdr.GetString(ColumnsContactPersonGet.LastName),
                                        JobPosition = rdr.GetInt32(ColumnsContactPersonGet.JobPosition),
                                        Email = rdr.GetString(ColumnsContactPersonGet.Email),
                                        Email2 = rdr.GetString(ColumnsContactPersonGet.Email2),
                                        Phone = rdr.GetString(ColumnsContactPersonGet.Phone).Trim(),
                                        Mobile = rdr.GetString(ColumnsContactPersonGet.Mobile).Trim(),
                                        Active = rdr.GetBoolean(ColumnsContactPersonGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsContactPersonGet.CreatedBy),
                                            Profile = new UserProfile
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
                                            Profile = new UserProfile
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
                        cmd.Parameters.Add(DataParameter.Input("@FirstName", this.FirstName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@LastName", this.LastName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@JobPosition", this.JobPosition));
                        cmd.Parameters.Add(DataParameter.Input("@Email1", this.Email, 150));
                        cmd.Parameters.Add(DataParameter.Input("@Email2", this.Email2, 150));
                        cmd.Parameters.Add(DataParameter.Input("@Phone", this.Phone, 15));
                        cmd.Parameters.Add(DataParameter.Input("@Mobile", this.Mobile, 15));
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
                        cmd.Parameters.Add(DataParameter.Input("@FirstName", this.FirstName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@LastName", this.LastName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@JobPosition", this.JobPosition));
                        cmd.Parameters.Add(DataParameter.Input("@Email1", this.Email, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Email2", this.Email2, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Phone", this.Phone, 15));
                        cmd.Parameters.Add(DataParameter.Input("@Mobile", this.Mobile, 15));
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