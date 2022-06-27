// --------------------------------
// <copyright file="CompanyContactPerson.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Companies
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
    using OpenFrameworkV3.Core.Security;

    public class CompanyContactPerson
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public bool Main { get; set; }
        public bool ContractOwner { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastName2 { get; set; }
        public string NIF { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string EmergencyPhone { get; set; }
        public string Email1 { get; set; }
        public string Email2{ get; set; }
        public string AlternativeMail { get; set; }
        public string JobPosition { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }
        public long ApplicationUserId { get; set; }

        public string FullName
        {
            get{
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} {1} {2}",
                    this.FirstName,
                    this.LastName,
                    this.LastName2).Replace("  ", " ").Trim();
            }
        }

        public static CompanyContactPerson Empty
        {
            get
            {
                return new CompanyContactPerson
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    LastName2 = string.Empty,
                    NIF = string.Empty,
                    ContractOwner = false,
                    Main = false,
                    Phone1 = string.Empty,
                    Phone2 = string.Empty,
                    EmergencyPhone = string.Empty,
                    Email1 = string.Empty,
                    Email2 = string.Empty,
                    AlternativeMail = string.Empty,
                    JobPosition = string.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"": {0},
                        ""CompanyId"": {1},
                        ""FirstName"": ""{2}"",
                        ""LastName"": ""{3}"",
                        ""LastName2"": ""{4}"",
                        ""NIF"": ""{5}"",
                        ""Active"": {6}
                    }}",
                    this.Id,
                    this.CompanyId,
                    Tools.Json.JsonCompliant(this.FirstName),
                    Tools.Json.JsonCompliant(this.LastName),
                    Tools.Json.JsonCompliant(this.LastName2),
                    Tools.Json.JsonCompliant(this.NIF),
                    ConstantValue.Value(this.Active));
            }
        }

        public string Json
        {
            get
            {
                if(this.Id == Constant.DefaultId)
                {
                    return @"{""Id"":-1}";
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"": {0},
                        ""CompanyId"": {1},
                        ""ContractOwner"": {2},
                        ""Main"": {3},
                        ""FirstName"": ""{4}"",
                        ""LastName"": ""{5}"",
                        ""LastName2"": ""{6}"",
                        ""FullName"": ""{20}"",
                        ""NIF"": ""{18}"",
                        ""Phone1"": ""{7}"",
                        ""Phone2"": ""{8}"",
                        ""EmergencyPhone"": ""{9}"",
                        ""Email1"": ""{10}"",
                        ""Email2"": ""{19}"",
                        ""AlternativeEmail"": ""{11}"",
                        ""JobPosition"": ""{12}"",
                        ""CreatedBy"": {13},
                        ""CreatedOn"": ""{14:dd/MM/yyyy}"",
                        ""ModifiedBy"": {15},
                        ""ModifiedOn"": ""{16:dd/MM/yyyy}"",
                        ""Active"": {17},
                        ""ApplicationUserId"": {21}
                    }}",
                    this.Id,
                    this.CompanyId,
                    ConstantValue.Value(this.ContractOwner),
                    ConstantValue.Value(this.Main),
                    Tools.Json.JsonCompliant(this.FirstName),
                    Tools.Json.JsonCompliant(this.LastName),
                    Tools.Json.JsonCompliant(this.LastName2),
                    Tools.Json.JsonCompliant(this.Phone1),
                    Tools.Json.JsonCompliant(this.Phone2),
                    Tools.Json.JsonCompliant(this.EmergencyPhone),
                    Tools.Json.JsonCompliant(this.Email1),
                    Tools.Json.JsonCompliant(this.AlternativeMail),
                    Tools.Json.JsonCompliant(this.JobPosition),
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedBy,
                    ConstantValue.Value(this.Active),
                    Tools.Json.JsonCompliant(this.NIF),
                    Tools.Json.JsonCompliant(this.Email2),
                    Tools.Json.JsonCompliant(this.FullName),
                    this.ApplicationUserId);
            }
        }

        /// <summary>Creates a JSON reprensentation of an contact persons list</summary>
        /// <param name="list">List of contact persons</param>
        /// <returns>JSON reprensentation of an contact persons</returns>
        public static string JsonList(ReadOnlyCollection<CompanyContactPerson> list)
        {
            var res = new StringBuilder("[");

            if(list != null && list.Count > 0)
            {
                bool first = true;
                foreach(var contact in list)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(contact.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>Obtains contacte persons of company with signature options</summary>
        /// <param name="companyId">Company's identifier</param>
        /// <returns>Readonly collection of contact persons</returns>
        public static ReadOnlyCollection<CompanyContactPerson> FirmantesByCompany(long companyId, string instanceName)
        {
            return new ReadOnlyCollection<CompanyContactPerson>(ByCompany(companyId, instanceName).Where(pc => pc.ContractOwner == true).ToList());
        }

        /// <summary>Obtains contacte persons of company</summary>
        /// <param name="companyId">Company's identifier</param>
        /// <returns>Readonly collection of contact persons</returns>
        public static ReadOnlyCollection<CompanyContactPerson> ByCompany(long companyId, string instanceName)
        {
            var res = new List<CompanyContactPerson>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyContactPerson_ByCompanyId"))
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
                                    res.Add(new CompanyContactPerson
                                    {
                                        Id = rdr.GetInt64(ColumnsCompanyContactPersonGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsCompanyContactPersonGet.CompanyId),
                                        Main = rdr.GetBoolean(ColumnsCompanyContactPersonGet.Main),
                                        ContractOwner = rdr.GetBoolean(ColumnsCompanyContactPersonGet.ContractOwner),
                                        FirstName = rdr.GetString(ColumnsCompanyContactPersonGet.FirstName),
                                        LastName = rdr.GetString(ColumnsCompanyContactPersonGet.LastName),
                                        LastName2 = rdr.GetString(ColumnsCompanyContactPersonGet.LastName2),
                                        NIF = rdr.GetString(ColumnsCompanyContactPersonGet.Nif).Trim(),
                                        JobPosition = rdr.GetString(ColumnsCompanyContactPersonGet.JobPosition),
                                        Phone1 = rdr.GetString(ColumnsCompanyContactPersonGet.Phone1).Trim(),
                                        Phone2 = rdr.GetString(ColumnsCompanyContactPersonGet.Phone2).Trim(),
                                        EmergencyPhone = rdr.GetString(ColumnsCompanyContactPersonGet.EmergencyPhone),
                                        Email1 = rdr.GetString(ColumnsCompanyContactPersonGet.Email1),
                                        Email2 = rdr.GetString(ColumnsCompanyContactPersonGet.Email2),
                                        AlternativeMail = rdr.GetString(ColumnsCompanyContactPersonGet.AlternativeMail),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsCompanyContactPersonGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsCompanyContactPersonGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsCompanyContactPersonGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsCompanyContactPersonGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsCompanyContactPersonGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsCompanyContactPersonGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsCompanyContactPersonGet.ModifiedBy),
                                            Profile =
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsCompanyContactPersonGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnsCompanyContactPersonGet.ModifiedByName),
                                            LastName = rdr.GetString(ColumnsCompanyContactPersonGet.ModifiedByLastName),
                                            LastName2 = rdr.GetString(ColumnsCompanyContactPersonGet.ModifiedByLastName2)
                                        }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsCompanyContactPersonGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsCompanyContactPersonGet.Active),
                                        ApplicationUserId = rdr.FieldCount > 26 ? rdr.GetInt64(ColumnsCompanyContactPersonGet.ApplicationUserId) : -1
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

            return new ReadOnlyCollection<CompanyContactPerson>(res);
        }

        /// <summary>Obtains main contact person of a company</summary>
        /// <param name="companyId">Company's identifier</param>
        /// <returns>Main contact persona of company, empty if not exists</returns>
        public static CompanyContactPerson MainByCompany(long companyId, string instanceName)
        {
            var res = CompanyContactPerson.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyMainContactPerson_ByCompanyId"))
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
                                    if (rdr.GetBoolean(ColumnsCompanyContactPersonGet.Main))
                                    {
                                        res = new CompanyContactPerson
                                        {
                                            Id = rdr.GetInt64(ColumnsCompanyContactPersonGet.Id),
                                            CompanyId = rdr.GetInt64(ColumnsCompanyContactPersonGet.CompanyId),
                                            Main = rdr.GetBoolean(ColumnsCompanyContactPersonGet.Main),
                                            ContractOwner = rdr.GetBoolean(ColumnsCompanyContactPersonGet.ContractOwner),
                                            FirstName = rdr.GetString(ColumnsCompanyContactPersonGet.FirstName),
                                            LastName = rdr.GetString(ColumnsCompanyContactPersonGet.LastName),
                                            LastName2 = rdr.GetString(ColumnsCompanyContactPersonGet.LastName2),
                                            NIF = rdr.GetString(ColumnsCompanyContactPersonGet.Nif).Trim(),
                                            Phone1 = rdr.GetString(ColumnsCompanyContactPersonGet.Phone1).Trim(),
                                            Phone2 = rdr.GetString(ColumnsCompanyContactPersonGet.Phone2).Trim(),
                                            EmergencyPhone = rdr.GetString(ColumnsCompanyContactPersonGet.EmergencyPhone),
                                            JobPosition = rdr.GetString(ColumnsCompanyContactPersonGet.JobPosition),
                                            Email1 = rdr.GetString(ColumnsCompanyContactPersonGet.Email1),
                                            Email2 = rdr.GetString(ColumnsCompanyContactPersonGet.Email2),
                                            AlternativeMail = rdr.GetString(ColumnsCompanyContactPersonGet.AlternativeMail),
                                            CreatedBy = new ApplicationUser
                                            {
                                                Id = rdr.GetInt64(ColumnsCompanyContactPersonGet.CreatedBy),
                                                Profile = new Profile
                                                {
                                                    ApplicationUserId = rdr.GetInt64(ColumnsCompanyContactPersonGet.CreatedBy),
                                                    Name = rdr.GetString(ColumnsCompanyContactPersonGet.CreatedByName),
                                                    LastName = rdr.GetString(ColumnsCompanyContactPersonGet.CreatedByLastName),
                                                    LastName2 = rdr.GetString(ColumnsCompanyContactPersonGet.CreatedByLastName2)
                                                }
                                            },
                                            CreatedOn = rdr.GetDateTime(ColumnsCompanyContactPersonGet.CreatedOn),
                                            ModifiedBy = new ApplicationUser
                                            {
                                                Id = rdr.GetInt64(ColumnsCompanyContactPersonGet.ModifiedBy),
                                                Profile =
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsCompanyContactPersonGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsCompanyContactPersonGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsCompanyContactPersonGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsCompanyContactPersonGet.ModifiedByLastName2)
                                            }
                                            },
                                            ModifiedOn = rdr.GetDateTime(ColumnsCompanyContactPersonGet.ModifiedOn),
                                            Active = rdr.GetBoolean(ColumnsCompanyContactPersonGet.Active)
                                        };
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
            /* PROCEDURE [dbo].[Core_CompanyContactPerson_Insert]
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @Main bit,
             *   @ContractOwner bit,
             *   @FirstName nvarchar(50),
             *   @LastName nvarchar(50),
             *   @LastName2 nvarchar(50),
             *   @NIF nvarchar(15),
             *   @Phone1 nchar(30),
             *   @Phone2 nchar(30),
             *   @EmergencyPhone nchar(30),
             *   @Email nvarchar(150),
             *   @AlternativeMail nvarchar(150),
             *   @JobPosition nvarchar(50),
             *   @UserId bigint,
             *   @ApplicationUserId bigint */

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyContactPerson_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Main", this.Main));
                        cmd.Parameters.Add(DataParameter.Input("@ContractOwner", this.ContractOwner));
                        cmd.Parameters.Add(DataParameter.Input("@FirstName", this.FirstName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@LastName", this.LastName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@LastName2", this.LastName2, 50));
                        cmd.Parameters.Add(DataParameter.Input("@NIF", this.NIF, 15));
                        cmd.Parameters.Add(DataParameter.Input("@Phone1", this.Phone1, 30));
                        cmd.Parameters.Add(DataParameter.Input("@Phone2", this.Phone2, 30));
                        cmd.Parameters.Add(DataParameter.Input("@EmergencyPhone", this.EmergencyPhone, 30));
                        cmd.Parameters.Add(DataParameter.Input("@Email", this.Email1, 150));
                        cmd.Parameters.Add(DataParameter.Input("@AlternativeMail", this.Email2, 150));
                        cmd.Parameters.Add(DataParameter.Input("@JobPosition", this.JobPosition, 50));

                        if (this.ApplicationUserId > 0)
                        {
                            cmd.Parameters.Add(DataParameter.Input("@UserId", this.ApplicationUserId));
                        }
                        else
                        {
                            cmd.Parameters.Add(DataParameter.InputNull("@UserId"));
                        }

                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(this.Id);
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

        public ActionResult Update(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* PROCEDURE [dbo].[Core_CompanyContactPerson_Update]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @Main bit,
             *   @ContractOwner bit,
             *   @FirstName nvarchar(50),
             *   @LastName nvarchar(50),
             *   @LastName2 nvarchar(50),
             *   @NIF nvarchar(15),
             *   @Phone1 nchar(30),
             *   @Phone2 nchar(30),
             *   @EmergencyPhone nchar(30),
             *   @Email nvarchar(150),
             *   @AlternativeMail nvarchar(150),
             *   @JobPosition nvarchar(50),
             *   @ApplicationUserId bigint */

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyContactPerson_Update"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Main", this.Main));
                        cmd.Parameters.Add(DataParameter.Input("@ContractOwner", this.ContractOwner));
                        cmd.Parameters.Add(DataParameter.Input("@FirstName", this.FirstName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@LastName", this.LastName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@LastName2", this.LastName2, 50));
                        cmd.Parameters.Add(DataParameter.Input("@NIF", this.NIF, 15));
                        cmd.Parameters.Add(DataParameter.Input("@Phone1", this.Phone1, 30));
                        cmd.Parameters.Add(DataParameter.Input("@Phone2", this.Phone2, 30));
                        cmd.Parameters.Add(DataParameter.Input("@EmergencyPhone", this.EmergencyPhone, 30));
                        cmd.Parameters.Add(DataParameter.Input("@Email", this.Email1, 150));
                        cmd.Parameters.Add(DataParameter.Input("@AlternativeMail", this.Email2, 150));
                        cmd.Parameters.Add(DataParameter.Input("@JobPosition", this.JobPosition, 50));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(this.Id);
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

        public static CompanyContactPerson ById (long id, string instanceName)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "CompanyContactPerson::ById({0})", id);
            var res = CompanyContactPerson.Empty;
            /* CREATE PROCEDURE Core_CompanyContactPerson_Get
             *   @CompanyId bigint,
             *   @Id bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyContactPerson_Get"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.InputNull("@CompanyId"));
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    res.Id = rdr.GetInt64(ColumnsCompanyContactPersonGet.Id);
                                    res.CompanyId = rdr.GetInt64(ColumnsCompanyContactPersonGet.CompanyId);
                                    res.Main = rdr.GetBoolean(ColumnsCompanyContactPersonGet.Main);
                                    res.ContractOwner = rdr.GetBoolean(ColumnsCompanyContactPersonGet.ContractOwner);
                                    res.FirstName = rdr.GetString(ColumnsCompanyContactPersonGet.FirstName);
                                    res.LastName = rdr.GetString(ColumnsCompanyContactPersonGet.LastName);
                                    res.LastName2 = rdr.GetString(ColumnsCompanyContactPersonGet.LastName2);
                                    res.Phone1 = rdr.GetString(ColumnsCompanyContactPersonGet.Phone1);
                                    res.Phone2 = rdr.GetString(ColumnsCompanyContactPersonGet.Phone2);
                                    res.EmergencyPhone = rdr.GetString(ColumnsCompanyContactPersonGet.EmergencyPhone);
                                    res.Email1 = rdr.GetString(ColumnsCompanyContactPersonGet.Email1);
                                    res.AlternativeMail = rdr.GetString(ColumnsCompanyContactPersonGet.AlternativeMail);
                                    res.JobPosition = rdr.GetString(ColumnsCompanyContactPersonGet.JobPosition);
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsCompanyContactPersonGet.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsCompanyContactPersonGet.CreatedBy),
                                            Name = rdr.GetString(ColumnsCompanyContactPersonGet.CreatedByName),
                                            LastName = rdr.GetString(ColumnsCompanyContactPersonGet.CreatedByLastName),
                                            LastName2 = rdr.GetString(ColumnsCompanyContactPersonGet.CreatedByLastName2)
                                        }
                                    };
                                    res.CreatedOn = rdr.GetDateTime(ColumnsCompanyContactPersonGet.CreatedOn);
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsCompanyContactPersonGet.ModifiedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsCompanyContactPersonGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnsCompanyContactPersonGet.ModifiedByName),
                                            LastName = rdr.GetString(ColumnsCompanyContactPersonGet.LastName),
                                            LastName2 = rdr.GetString(ColumnsCompanyContactPersonGet.LastName2)
                                        }
                                    };
                                    res.ModifiedOn = rdr.GetDateTime(ColumnsCompanyContactPersonGet.ModifiedOn);
                                    res.Active = rdr.GetBoolean(ColumnsCompanyContactPersonGet.Active);
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex, source);
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex, source);
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.Trace(ex, source);
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

        public static ActionResult Activate(long id, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyContactPerson_Activate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
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

        public static ActionResult Inactivate(long id, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyContactPerson_Inactivate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
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

        public static ActionResult SetMain(long id, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyContactPerson_SetMain"))
                {
                    using (var cnn = new SqlConnection(cns))
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
                            res.SetSuccess();
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

        public static ActionResult SetContractOwner(long id, bool contractOwner, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyContactPerson_SetContractOwner"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@ContractOwner", contractOwner));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
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
    }
}