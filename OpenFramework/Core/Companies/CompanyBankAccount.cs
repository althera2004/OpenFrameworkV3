// --------------------------------
// <copyright file="CompanyBankAccount.cs" company="OpenFramework">
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

    public partial class CompanyBankAccount
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string IBAN { get; set; }
        public string Swift { get; set; }
        public string BankName { get; set; }
        public string ContractId { get; set; }
        public string PaymentType { get; set; }
        public bool Main { get; set; }
        public string Alias { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public static CompanyBankAccount Empty
        {
            get
            {
                return new CompanyBankAccount
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    IBAN = string.Empty,
                    Swift = string.Empty,
                    BankName = string.Empty,
                    ContractId = string.Empty,
                    PaymentType = string.Empty,         
                    Main = false,
                    Alias = string.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    ModifiedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public static string JsonList(ReadOnlyCollection<CompanyBankAccount> list)
        {
            var res = new StringBuilder("[");
            if (list != null)
            {
                bool first = true;
                foreach (var item in list)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(item.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"
                    {{
                        ""Id"":{0},
                        ""IBAN"":""{1}"",
                        ""Swift"":""{2}"",
                        ""BankName"":""{3}"",
                        ""Main"":{4},
                        ""Alias"":""{5}"",
                        ""ContractId"":""{6}"",
                        ""PaymentType"":""{7}"",
                        ""CreatedBy"":{8},
                        ""CreatedOn"":""{9:dd/MM/yyyy}"",
                        ""ModifiedBy"":{10},
                        ""ModifiedOn"":""{11:dd/MM/yyyy}"",
                        ""Active"":{12}
                    }}",
                    this.Id,
                    this.IBAN,
                    this.Swift,
                    Tools.Json.JsonCompliant(this.BankName),
                    ConstantValue.Value(this.Main),
                    Tools.Json.JsonCompliant(this.Alias),
                    Tools.Json.JsonCompliant(this.ContractId),
                    Tools.Json.JsonCompliant(this.PaymentType),
                    this.CreatedBy.JsonSimple,
                    this.CreatedOn,
                    this.ModifiedBy.JsonSimple,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static ActionResult SetMain(long id, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyBankAccount_SetMain"))
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
                            res.SetSuccess(id);
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

        public static ActionResult Activate(long id, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyBankAccount_Activate"))
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
                            res.SetSuccess(id);
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

        public static ActionResult Inactivate(long id, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyBankAccount_Inactivate"))
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
                            res.SetSuccess(id);
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

        public static CompanyBankAccount ById(long id, long companyId, string instanceName)
        {
            var res = CompanyBankAccount.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyBankAccount_ById"))
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
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    res = new CompanyBankAccount
                                    {
                                        Id = rdr.GetInt64(ColumnsCompanyBankAccountGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsCompanyBankAccountGet.CompanyId),
                                        IBAN = rdr.GetString(ColumnsCompanyBankAccountGet.Iban).Trim(),
                                        Swift = rdr.GetString(ColumnsCompanyBankAccountGet.Swift).Trim(),
                                        BankName = rdr.GetString(ColumnsCompanyBankAccountGet.BankName),
                                        Alias = rdr.GetString(ColumnsCompanyBankAccountGet.Alias),
                                        Main = rdr.GetBoolean(ColumnsCompanyBankAccountGet.Main),
                                        ContractId = rdr.GetString(ColumnsCompanyBankAccountGet.ContractId),
                                        PaymentType = rdr.GetString(ColumnsCompanyBankAccountGet.PaymentType).Trim().ToUpperInvariant(),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsCompanyBankAccountGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsCompanyBankAccountGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsCompanyBankAccountGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsCompanyBankAccountGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsCompanyBankAccountGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsCompanyBankAccountGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsCompanyBankAccountGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsCompanyBankAccountGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsCompanyBankAccountGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsCompanyBankAccountGet.ModifiedLastName),
                                                LastName2 = rdr.GetString(ColumnsCompanyBankAccountGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsCompanyBankAccountGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsCompanyBankAccountGet.Active)
                                    };
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

        public static CompanyBankAccount MainByCompany(long companyId, string instanceName)
        {
            var res = CompanyBankAccount.Empty;
            var accounts = ByCompany(companyId, instanceName);
            if(accounts.Any(a=>a.Main == true))
            {
                res = accounts.First(a => a.Main == true);
            }

            return res;
        }

        public static ReadOnlyCollection<CompanyBankAccount> ByCompany(long companyId, string instanceName)
        {
            var res = new List<CompanyBankAccount>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyBankAccount_ByCompanyId"))
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
                                    res.Add(new CompanyBankAccount
                                    {
                                        Id = rdr.GetInt64(ColumnsCompanyBankAccountGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsCompanyBankAccountGet.CompanyId),
                                        IBAN = rdr.GetString(ColumnsCompanyBankAccountGet.Iban).Trim(),
                                        Swift = rdr.GetString(ColumnsCompanyBankAccountGet.Swift).Trim(),
                                        BankName = rdr.GetString(ColumnsCompanyBankAccountGet.BankName),
                                        Alias = rdr.GetString(ColumnsCompanyBankAccountGet.Alias),
                                        Main = rdr.GetBoolean(ColumnsCompanyBankAccountGet.Main),
                                        ContractId = rdr.GetString(ColumnsCompanyBankAccountGet.ContractId),
                                        PaymentType = rdr.GetString(ColumnsCompanyBankAccountGet.PaymentType).Trim().ToUpperInvariant(),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsCompanyBankAccountGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsCompanyBankAccountGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsCompanyBankAccountGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsCompanyBankAccountGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsCompanyBankAccountGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsCompanyBankAccountGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsCompanyBankAccountGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsCompanyBankAccountGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsCompanyBankAccountGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsCompanyBankAccountGet.ModifiedLastName),
                                                LastName2 = rdr.GetString(ColumnsCompanyBankAccountGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsCompanyBankAccountGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsCompanyBankAccountGet.Active)
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

            return new ReadOnlyCollection<CompanyBankAccount>(res);
        }

        public ActionResult Save(long applicationUserId, string instanceName)
        {
            if(this.Id > 0)
            {
                return Update(applicationUserId, instanceName);
            }

            return Insert(applicationUserId, instanceName);
        }

        public ActionResult Insert(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            /* CREATE PROCEDURE [dbo].[Core_CompanyBankAccount_Insert]
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @IBAN nchar(40),
             *   @Swift nchar(40),
             *   @BankName nvarchar(50),
             *   @Alias nvarchar(50),
             *   @ContractId nvarchar(50),
             *   @PaymentType nchar(4),
             *   @Main bit,
             *   @ApplicationUserId bigint */

            if(!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyBankAccount_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@IBAN", this.IBAN, 40));
                        cmd.Parameters.Add(DataParameter.Input("@Swift", this.Swift, 40));
                        cmd.Parameters.Add(DataParameter.Input("@BankName", this.BankName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Alias", this.Alias, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Main", this.Main));
                        cmd.Parameters.Add(DataParameter.Input("@ContractId", this.ContractId, 50));
                        cmd.Parameters.Add(DataParameter.Input("@PaymentType", this.PaymentType, 4));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(this.Id);
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

        public ActionResult Update(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            /* CREATE PROCEDURE [dbo].[Core_CompanyBankAccount_Update]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @IBAN nchar(40),
             *   @Swift nchar(40),
             *   @BankName nvarchar(50),
             *   @Alias nvarchar(50),
             *   @ContractId nvarchar(50),
             *   @PaymentType nchar(4),
             *   @Main bit,
             *   @ApplicationUserId bigint */
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyBankAccount_Update"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@IBAN", this.IBAN, 40));
                        cmd.Parameters.Add(DataParameter.Input("@Swift", this.Swift, 40));
                        cmd.Parameters.Add(DataParameter.Input("@BankName", this.BankName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Alias", this.Alias, 50));
                        cmd.Parameters.Add(DataParameter.Input("@ContractId", this.ContractId, 50));
                        cmd.Parameters.Add(DataParameter.Input("@PaymentType", this.PaymentType, 4));
                        cmd.Parameters.Add(DataParameter.Input("@Main", this.Main));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(this.Id);
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
    }
}