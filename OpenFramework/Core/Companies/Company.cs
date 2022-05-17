// --------------------------------
// <copyright file="Company.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Companies
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using OpenFrameworkV3.Billing;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Bindings;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;

    /// <summary>Implements Company class</summary>
    public class Company
    {
        private List<CompanyPostalAddress> adresses;
        private List<CompanyContactPerson> contactPersons;
        private Dictionary<string, object> customFields;

        public ReadOnlyDictionary<string, object> CustomFiels
        {
            get
            {
                if(this.customFields == null)
                {
                    this.customFields = new Dictionary<string, object>();
                }

                return new ReadOnlyDictionary<string, object>(this.customFields);
            }
        }

        public Invoice.InvoicePerson PayerData
        {
            get
            {
                var bankAccount = CompanyBankAccount.MainByCompany(this.Id, this.InstanceName);
                return new Invoice.InvoicePerson
                {
                    Name = this.Name.Trim(),
                    CIF = (this.Cif ?? string.Empty).Trim(),
                    IBAN = (bankAccount.IBAN ?? string.Empty).Trim(),
                    SWIFT = (bankAccount.Swift ?? string.Empty).Trim(),
                    Address = this.BillingAddress.FullStreetAddress.Trim(),
                    PostalCode = (this.BillingAddress.PostalCode ?? string.Empty).Trim(),
                    City = (this.BillingAddress.City ?? string.Empty).Trim(),
                    Province = (this.BillingAddress.Province ?? string.Empty).Trim(),
                    Country = (this.BillingAddress.Country ?? string.Empty).Trim(),
                    Email = (this.Email ?? string.Empty).Trim(),
                    Phone = (this.ContactPerson.Phone1 ?? string.Empty).Trim()
                };
            }
        }

        /// <summary>Gets an empty company</summary>
        public static Company Empty
        {
            get
            {
                return new Company
                {
                    Id = Constant.DefaultId,
                    Code = string.Empty,
                    Name = string.Empty,
                    LOPD = false,
                    SubscriptionStart = DateTime.Now,
                    DefaultLanguage = Language.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Web = string.Empty,
                    Fax = string.Empty,
                    Email = string.Empty,
                    Cif = string.Empty,
                    NombreEmpresa = string.Empty,
                    Phone = string.Empty,
                    RazonSocial = string.Empty,
                    Active = false
                };
            }
        }

        public static Company Default(string instanceName)
        {
            return new Company
            {
                Id = 1,
                Name = instanceName,
                Code = instanceName,
                Active = true,
                LOPD = true,
                DefaultLanguage = Language.Empty,
                CreatedBy = ApplicationUser.OpenFramework,
                ModifiedBy = ApplicationUser.OpenFramework,
                CreatedOn = DateTime.Today,
                ModifiedOn = DateTime.Today,
                SubscriptionEnd = DateTime.Today,
                SubscriptionStart = DateTime.Today
            };
        }

        public string InstanceName { get; set; }

        /// <summary>Gets or sets a value indicating whether agreement document is accepted</summary>
        public bool LOPD { get; set; }

        /// <summary>Gets a JSON key/value stucture of the company</summary>
        public string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""Value"":""{1}"",""Active"":{2}}}",
                    this.Id,
                    Tools.Json.JsonCompliant(this.Name),
                    ConstantValue.Value(this.Active));
            }
        }

        /// <summary>Gets or sets the company identifier</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets the code of company</summary>
        public string Code { get; set; }

        /// <summary>Gets or sets the name of company</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the CIF of company</summary>
        public string Cif { get; set; }

        public string NombreEmpresa { get; set; }

        public string RazonSocial { get; set; }

        public string Email { get; set; }

        public string Web { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        /// <summary>Gets or sets default language of company</summary>
        public Language DefaultLanguage { get; set; }

        /// <summary>Gets or sets the date of starting subscription</summary>
        public DateTime SubscriptionStart { get; set; }

        /// <summary>Gets or sets the date of finishing subscription</summary>
        public DateTime? SubscriptionEnd { get; set; }

        /// <summary>Gets or sets contact person</summary>
        public CompanyContactPerson ContactPerson
        {
            get
            {
                if(this.contactPersons != null)
                {
                    if(this.contactPersons.Any(cp=>cp.Main == true))
                    {
                        return this.contactPersons.First(cp => cp.Main);
                    }
                }

                return CompanyContactPerson.Empty;
            }
        }

        /// <summary>Gets or sets users that creates company</summary>
        public ApplicationUser CreatedBy { get; set; }

        /// <summary>Gets or sets user of last company modification</summary>
        public ApplicationUser ModifiedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool Active { get; set; }

        /// <summary>Gets path of logo image</summary>
        public string LogoPath
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"{0}\CompanyData\{1}\Logo.png", Instance.Path.Data(this.InstanceName), this.Id);
            }
        }

        /// <summary>Gets path of logo for pdfs purposses</summary>
        public string LogoPathPdf
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"{0}\CompanyData\{1}\PDFLogo.png", Instance.Path.Data(this.InstanceName), this.Id);
            }
        }

        /// <summary>Gets path of logo for pdfs purposses</summary>
        public string LogoPathInvoice
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"{0}\CompanyData\{1}\InvoiceLogo.png", Instance.Path.Data(this.InstanceName), this.Id);
            }
        }

        /// <summary>Get url of logo for show in web pages</summary>
        public string LogoUrl
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"/Instances/{0}/Data/CompanyData/{1}/Logo.png", Instance.Path.Data(this.InstanceName), this.Id);
            }
        }

        /// <summary>Gets company's addresses</summary>
        public ReadOnlyCollection<CompanyPostalAddress> Addresses
        {
            get
            {
                if (this.adresses == null)
                {
                    this.adresses = new List<CompanyPostalAddress>();
                }

                return new ReadOnlyCollection<CompanyPostalAddress>(this.adresses);
            }
        }

        /// <summary>Get main address of company</summary>
        public CompanyPostalAddress MainAddress
        {
            get
            {
                var res = CompanyPostalAddress.Empty;
                if(this.adresses != null)
                {
                    if(this.adresses.Any(a=>a.Main == true))
                    {
                        res = this.adresses.First(a => a.Main == true);
                    }
                }

                return res;
            }
        }

        /// <summary>Gets billing address of company</summary>
        public CompanyPostalAddress BillingAddress
        {
            get
            {
                var res = CompanyPostalAddress.Empty;
                if (this.adresses != null)
                {
                    if (this.adresses.Any(a => a.Billing == true))
                    {
                        res = this.adresses.First(a => a.Billing == true);
                    }
                }

                return res;
            }
        }

        private string JsonCustomFields
        {
            get
            {
                if(this.customFields == null)
                {
                    return Tools.Json.EmptyJsonList;
                }

                var res = new StringBuilder("[");
                var first = true;
                foreach(var field in this.customFields)
                {
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"{2}{{""Key"":""{0}"",""Value"":""{1}""}}",
                        field.Key.Trim(),
                        field.Value,
                        first ? string.Empty : ",");
                    first = false;
                }

                res.Append("]");
                return res.ToString();
            }
        }

        /// <summary>Gets a JSON structure of company</summary>
        /// <returns>JSON structure</returns>
        public string Json
        {
            get
            {
                if (this == null)
                {
                    return Tools.Json.EmptyJsonObject;
                }

                var endDate = Constant.JavaScriptNull;
                if (this.SubscriptionEnd.HasValue)
                {
                    endDate = string.Format(CultureInfo.InvariantCulture, @"""{0:dd/MM/yyyy}""", this.SubscriptionEnd);
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},
                        ""Name"":""{1}"",
                        ""RazonSocial"":""{14}"",
                        ""Code"":""{2}"",
                        ""LOPD"":{3},
                        ""SubscriptionStart"":""{4:dd/MM/yyyy}"",
                        ""SubscriptionEnd"":{5},
                        ""ContactPerson"":{6},
                        ""Language"": {7},
                        ""MainAddress"": {13},
                        ""CustomFields"":{15},
                        ""CreatedBy"":{8},
                        ""CreatedOn"":""{9:dd/MM/yyyy}"",
                        ""ModifiedBy"":{10},
                        ""ModifiedOn"":""{11:dd/MM/yyyy}"",
                        ""Active"":{12}}}",
                    this.Id,
                    Tools.Json.JsonCompliant(this.Name),
                    Tools.Json.JsonCompliant(this.Code),
                    ConstantValue.Value(this.LOPD),
                    this.SubscriptionStart,
                    endDate,
                    this.ContactPerson.Json,
                    this.DefaultLanguage.Json,
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active),
                    this.MainAddress.Json,
                    Tools.Json.JsonCompliant(this.RazonSocial),
                    this.JsonCustomFields);
            }
        }

        /// <summary>Obtains companies where user has access</summary>
        /// <param name="applicationUserId">Application user identifier</param>
        /// <returns>List of companies</returns>
        public static ReadOnlyCollection<Company> ByUser(long applicationUserId, string instanceName)
        {
            var res = new List<Company>();
            var cns = Persistence.ConnectionString(instanceName);
            string source = string.Format(CultureInfo.InvariantCulture, "Company::Company_ByUserId({0})", applicationUserId);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Company_ByUserId"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    var newCompany = new Company
                                    {
                                        Id = rdr.GetInt64(ColumnsCompanyGet.Id),
                                        Name = rdr.GetString(ColumnsCompanyGet.Name),
                                        Code = rdr.GetString(ColumnsCompanyGet.Code),
                                        SubscriptionStart = DateTime.Now,
                                        LOPD = false,
                                        CreatedBy = ApplicationUser.Empty,
                                        CreatedOn = DateTime.Today,
                                        ModifiedBy = ApplicationUser.Empty,
                                        ModifiedOn = DateTime.Today,
                                        Active = true
                                    };

                                    res.Add(newCompany);
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (ArgumentNullException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (ArgumentException ex)
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

            return new ReadOnlyCollection<Company>(res);
        }

        /// <summary>Gets all companies</summary>
        public static ReadOnlyCollection<Company> All(string instanceName)
        {
            var res = new List<Company>();
            var cns = Persistence.ConnectionString(instanceName);
            string source = string.Format(CultureInfo.InvariantCulture, "Company::All({0})", instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Company_All"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    var newCompany = new Company
                                    {
                                        Id = rdr.GetInt64(ColumnsCompanyGet.Id),
                                        Name = rdr.GetString(ColumnsCompanyGet.Name),
                                        Code = rdr.GetString(ColumnsCompanyGet.Code),
                                        SubscriptionStart = rdr.GetDateTime(ColumnsCompanyGet.SubscriptionStart),
                                        LOPD = rdr.GetBoolean(ColumnsCompanyGet.LOPD),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsCompanyGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsCompanyGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsCompanyGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsCompanyGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsCompanyGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsCompanyGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsCompanyGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsCompanyGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsCompanyGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsCompanyGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsCompanyGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsCompanyGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsCompanyGet.Active)
                                    };

                                    if (!rdr.IsDBNull(ColumnsCompanyGet.DefaultLanguage))
                                    {
                                        newCompany.DefaultLanguage = Language.ById(rdr.GetInt64(ColumnsCompanyGet.DefaultLanguage), instanceName);
                                    }
                                    else
                                    {
                                        newCompany.DefaultLanguage = Language.Default;
                                    }

                                    if (!rdr.IsDBNull(ColumnsCompanyGet.SubscriptionEnd))
                                    {
                                        newCompany.SubscriptionEnd = rdr.GetDateTime(ColumnsCompanyGet.SubscriptionEnd);
                                    }

                                    res.Add(newCompany);
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (ArgumentNullException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (ArgumentException ex)
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
            return new ReadOnlyCollection<Company>(res);
        }

        /// <summary>Gets company from database based on identifier</summary>
        /// <param name="companyId">Company indentifier</param>
        /// <param name="instanceName">Name of instance</param>
        /// <returns>Company by identifier</returns>
        public static Company ById(long companyId, string instanceName)
        {
            return ById(companyId, instanceName, false);
        }

        /// <summary>Initializes a new instance of the Company class.
        /// Company data is searched on database based in company identifier</summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="instanceName">Name of instance</param>
        /// <param name="customConfig">Indicates if custom config is available</param>
        /// <returns>Company object</returns>
        public static Company ById(long companyId, string instanceName, bool customConfig)
        {
            var res = Company.Empty;
            string source = string.Format(CultureInfo.InvariantCulture, "cto::Company({0})", companyId);

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Company_ById"))
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
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    res.Id = rdr.GetInt64(ColumnsCompanyGet.Id);
                                    res.Name = rdr.GetString(ColumnsCompanyGet.Name);
                                    res.RazonSocial = rdr.GetString(ColumnsCompanyGet.RazonSocial);
                                    res.Code = rdr.GetString(ColumnsCompanyGet.Code);
                                    res.SubscriptionStart = rdr.GetDateTime(ColumnsCompanyGet.SubscriptionStart);

                                    if (!rdr.IsDBNull(ColumnsCompanyGet.SubscriptionEnd))
                                    {
                                        res.SubscriptionEnd = rdr.GetDateTime(ColumnsCompanyGet.SubscriptionEnd);
                                    }

                                    res.Cif = rdr.GetString(ColumnsCompanyGet.CIF).ToUpperInvariant().Trim();
                                    res.LOPD = rdr.GetBoolean(ColumnsCompanyGet.LOPD);

                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsCompanyGet.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsCompanyGet.CreatedBy),
                                            Name = rdr.GetString(ColumnsCompanyGet.CreatedByName),
                                            LastName = rdr.GetString(ColumnsCompanyGet.CreatedByLastName),
                                            LastName2 = rdr.GetString(ColumnsCompanyGet.CreatedByLastName2)
                                        }
                                    };

                                    res.CreatedOn = rdr.GetDateTime(ColumnsCompanyGet.CreatedOn);

                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsCompanyGet.ModifiedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsCompanyGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnsCompanyGet.ModifiedByName),
                                            LastName = rdr.GetString(ColumnsCompanyGet.ModifiedByLastName),
                                            LastName2 = rdr.GetString(ColumnsCompanyGet.ModifiedByLastName2)
                                        }
                                    };

                                    res.ModifiedOn = rdr.GetDateTime(ColumnsCompanyGet.ModifiedOn);
                                    res.Active = rdr.GetBoolean(ColumnsCompanyGet.Active);

                                    if (rdr.FieldCount > 30)
                                    {
                                        res.Email = rdr.GetString(ColumnsCompanyGet.Email).ToLowerInvariant();
                                    }

                                    if (rdr.FieldCount > 31)
                                    {
                                        res.Web = rdr.GetString(ColumnsCompanyGet.Web).ToLowerInvariant();
                                        res.Phone = rdr.GetString(ColumnsCompanyGet.Phone).Trim();
                                        res.Fax = rdr.GetString(ColumnsCompanyGet.Fax).Trim();
                                    }

                                    res.adresses = CompanyPostalAddress.ByCompany(res.Id, instanceName).ToList();
                                    res.contactPersons = CompanyContactPerson.ByCompany(res.Id, instanceName).ToList();
                                }
                            }

                            if (customConfig)
                            {
                                res.ObtainCustomFiels();
                            }
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InstalledUICulture, "cto::Company({0})", companyId));
                        }
                        catch (FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InstalledUICulture, "cto::Company({0})", companyId));
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InstalledUICulture, "cto::Company({0})", companyId));
                        }
                        catch (ArgumentNullException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "cto::Company({0})", companyId));
                        }
                        catch (ArgumentException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "cto::Company({0})", companyId));
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

        /// <summary>Gets log of company</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Filename of company's logo</returns>
        public static string GetLogoFileName(int companyId)
        {
            string res = "NoImage.jpg";
            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith("\\"))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}\images\Logos\", path);
            string pattern = string.Format(CultureInfo.InvariantCulture, "{0}.*", companyId);
            var last = new DateTime(1900, 1, 1);
            var files = Directory.GetFiles(path, pattern);
            foreach (string file in files)
            {
                var info = new FileInfo(file);
                var created = info.LastWriteTime;
                if (created > last)
                {
                    last = created;
                    res = file;
                }
            }

            res = Path.GetFileName(res);
            return res;
        }

        /// <summary>Update company in data base</summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(this.InstanceName);
            /* CREATE PROCEDURE [dbo].[Core_Company_Update]
             *   @Id bigint,
             *   @Name nvarchar(50),
             *   @Code nvarchar(15),
             *   @LOPD bit,
             *   @SubscriptionStart datetime,
             *   @SubscriptionEnd datetime,
             *   @ContactPerson bigint,
             *   @ApplicationUserId bigint */
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Company_Update"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        try
                        {
                            cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.Id));
                            cmd.Parameters.Add(DataParameter.Input("@Name", this.Name, Constant.DefaultDatabaseVarChar));
                            cmd.Parameters.Add(DataParameter.Input("@Code", this.Code, 15));
                            cmd.Parameters.Add(DataParameter.Input("@LOPD", this.LOPD));
                            cmd.Parameters.Add(DataParameter.Input("@SubscriptionStart", this.SubscriptionStart));
                            cmd.Parameters.Add(DataParameter.Input("@SubscriptionEnd", this.SubscriptionEnd));
                            cmd.Parameters.Add(DataParameter.Input("@ContactPerson", this.ContactPerson.Id));
                            cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.Success = true;
                            res.MessageError = string.Empty;
                        }
                        catch (SqlException ex)
                        {
                            res.SetFail(ex);
                        }
                        catch (FormatException ex)
                        {
                            res.SetFail(ex);
                        }
                        catch (NullReferenceException ex)
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

        public static ActionResult SetBillingLastMonth(long companyId, DateTime lastBillingMonth, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Core_Company_SetLastBillingMonth"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@LastBillingMonth", lastBillingMonth));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
                        }
                        catch(Exception ex)
                        {
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

        public static ActionResult GetBillingLastMonth(long companyId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Company_GetLastBillingMonth"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    if (!rdr.IsDBNull(0))
                                    {
                                        res.SetSuccess(rdr.GetDateTime(0));
                                    }
                                    else
                                    {
                                        res.SetSuccess();
                                    }
                                }
                            }
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

        public ActionResult UpdateMainContactPerson(string name, string nif, string tel1, string tel2, string telU, string email, string emailA, string cargo, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* ALTER PROCEDURE[dbo].[Core_CompanyContactPerson_Update]
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
            var cns = Persistence.ConnectionString(this.InstanceName);
            var contact = CompanyContactPerson.MainByCompany(this.Id, instanceName);
            using (var cmd = new SqlCommand("Core_CompanyContactPerson_Update"))
            {
                using (var cnn = new SqlConnection(cns))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", contact.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", contact.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Main", contact.Main));
                    cmd.Parameters.Add(DataParameter.Input("@ContractOwner", contact.ContractOwner));
                    cmd.Parameters.Add(DataParameter.Input("@FirstName", name, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Nif", nif.ToUpperInvariant(), 15));
                    cmd.Parameters.Add(DataParameter.Input("@LastName", string.Empty, 50));
                    cmd.Parameters.Add(DataParameter.Input("@LastName2", string.Empty, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Phone1", tel1, 30));
                    cmd.Parameters.Add(DataParameter.Input("@Phone2", tel2, 30));
                    cmd.Parameters.Add(DataParameter.Input("@EmergencyPhone", telU, 30));
                    cmd.Parameters.Add(DataParameter.Input("@Email", email.ToLowerInvariant(), 150));
                    cmd.Parameters.Add(DataParameter.Input("@AlternativeMail", emailA.ToLowerInvariant(), 150));
                    cmd.Parameters.Add(DataParameter.Input("@Jobposition", cargo, 50));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        HttpContext.Current.Session["Company"] = Company.ById(this.Id, this.InstanceName);
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
            return res;
        }

        public ActionResult Update(long applicationUserId)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(this.InstanceName);
            using(var cmd = new SqlCommand("Core_Company_UpdateFromProfile"))
            {
                using (var cnn = new SqlConnection(cns))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@Name", this.Name, 50));
                    cmd.Parameters.Add(DataParameter.Input("@CIF", this.Cif, 15));
                    cmd.Parameters.Add(DataParameter.Input("@RazonSocial", this.RazonSocial, 150));
                    cmd.Parameters.Add(DataParameter.Input("@Email", this.Email, 150));
                    cmd.Parameters.Add(DataParameter.Input("@Web", this.Web, 150));
                    cmd.Parameters.Add(DataParameter.Input("@Phone", this.Phone, 30));
                    cmd.Parameters.Add(DataParameter.Input("@Fax", this.Fax, 30));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        HttpContext.Current.Session["Company"] = Company.ById(this.Id, this.InstanceName);
                        res.SetSuccess();
                    }
                    catch(Exception ex)
                    {
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

            return res;
        }

        private void AddCustomField(string key, string value)
        {
            if (this.customFields == null)
            {
                this.customFields = new Dictionary<string, object>();
            }

            if (this.customFields.ContainsKey(key))
            {
                this.customFields[key] = value;
            }
            else
            {
                this.customFields.Add(key, value);
            }
        }

        public static ActionResult SetConfig(string key, string value, long companyId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyConfig_Set"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Key", key, 20));
                        cmd.Parameters.Add(DataParameter.Input("@Value", value, 50));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
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

        public void ObtainCustomFiels()
        {
            var cns = Persistence.ConnectionString(this.InstanceName);
            /* CREATE PROCEDURE [dbo].[Core_CompanyConfigAll]
             *   @CompanyId bigint */

            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyConfigAll"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.Id));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    var value = rdr[2].ToString().Trim();
                                    this.AddCustomField(rdr.GetString(1).Trim(), value);
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
    }
}