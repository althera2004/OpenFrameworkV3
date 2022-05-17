// --------------------------------
// <copyright file="Concept.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Billing
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

    public partial class Concept
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal IVA { get; set; }
        public int Type { get; set; }
        public ConceptCategory Category { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public static Concept Empty
        {
            get
            {
                return new Concept
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    Description = string.Empty,
                    Amount = 0,
                    IVA = 0,
                    Type = 0,
                    Category = ConceptCategory.Empty,
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
                    @"{{""Id"":{0},""CompanyId"":{1},""Description"":""{2}"",""Amount"":{3},""IVA"":{4},""Type"":{5},""Category"":{6},""CreatedBy"":{7},""ModifiedBy"":{8},""CreateOn"": ""{9:dd/MM/yyyy}"",""ModifiedOn"": ""{10:dd/MM/yyyy}"",""Active"":{11}}}",
                    this.Id,
                    this.CompanyId,
                    Tools.Json.JsonCompliant(this.Description),
                    this.Amount,
                    this.IVA,
                    this.Type,
                    this.Category.JsonJeyValue,
                    this.CreatedBy.JsonKeyValue,
                    this.ModifiedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static string JsonList(ReadOnlyCollection<Concept> list)
        {
            if(list == null)
            {
                return Tools.Json.EmptyJsonList;
            }

            if(list.Count == 0)
            {
                return Tools.Json.EmptyJsonList;
            }

            var res = new StringBuilder("[");
            bool first = true;
            foreach(var item in list)
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

            res.Append("]");
            return res.ToString();
        }

        public static ActionResult Activate(long id,long companyId, long applicactionUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_Concept_Activate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicactionUserId));
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
                            if (cmd.Connection.State == ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return res;
        }

        public static ActionResult Inactivate(long id,long companyId, long applicactionUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_Concept_Inactivate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicactionUserId));
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
                            if (cmd.Connection.State == ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return res;
        }

        public static Concept ById(long id, long companyId, string instanceName)
        {
            var res = Concept.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_Concept_ById"))
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
                                    res.Id = rdr.GetInt64(ColumnsConceptoGet.Id);
                                    res.CompanyId = rdr.GetInt64(ColumnsConceptoGet.CompanyId);
                                    res.Description = rdr.GetString(ColumnsConceptoGet.Description);
                                    res.Amount = rdr.GetDecimal(ColumnsConceptoGet.Amount);
                                    res.IVA = rdr.GetDecimal(ColumnsConceptoGet.IVA);
                                    res.Type = rdr.GetInt32(ColumnsConceptoGet.Type);
                                    res.Category = new ConceptCategory
                                    {
                                        Id = rdr.GetInt64(ColumnsConceptoGet.CategoryId),
                                        Description = rdr.GetString(ColumnsConceptoGet.CategoryDescription)
                                    };
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsConceptoGet.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsConceptoGet.CreatedBy),
                                            Name = rdr.GetString(ColumnsConceptoGet.CreatedByName),
                                            LastName = rdr.GetString(ColumnsConceptoGet.CreatedByLastName),
                                            LastName2 = rdr.GetString(ColumnsConceptoGet.CreatedByLastName2)
                                        }
                                    };
                                    res.CreatedOn = rdr.GetDateTime(ColumnsConceptoGet.CreatedOn);
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsConceptoGet.ModifiedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsConceptoGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnsConceptoGet.ModifiedByName),
                                            LastName = rdr.GetString(ColumnsConceptoGet.ModifiedByLastName),
                                            LastName2 = rdr.GetString(ColumnsConceptoGet.ModifiedByLastName2)
                                        }
                                    };
                                    res.ModifiedOn = rdr.GetDateTime(ColumnsConceptoGet.ModifiedOn);
                                    res.Active = rdr.GetBoolean(ColumnsConceptoGet.Active);
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

        public static ReadOnlyCollection<Concept> ByCompany(long companyId, string instanceName)
        {
            var res = new List<Concept>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_Concept_ByCompanyId"))
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
                                    res.Add(new Concept
                                    {
                                        Id = rdr.GetInt64(ColumnsConceptoGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsConceptoGet.CompanyId),
                                        Description = rdr.GetString(ColumnsConceptoGet.Description),
                                        Amount = rdr.GetDecimal(ColumnsConceptoGet.Amount),
                                        IVA = rdr.GetDecimal(ColumnsConceptoGet.IVA),
                                        Type = rdr.GetInt32(ColumnsConceptoGet.Type),
                                        Category = new ConceptCategory
                                        {
                                            Id = rdr.GetInt64(ColumnsConceptoGet.CategoryId),
                                            Description = rdr.GetString(ColumnsConceptoGet.CategoryDescription)
                                        },
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsConceptoGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsConceptoGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsConceptoGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsConceptoGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsConceptoGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsConceptoGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsConceptoGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsConceptoGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsConceptoGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsConceptoGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsConceptoGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsConceptoGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsConceptoGet.Active)
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

            return new ReadOnlyCollection<Concept>(res);
        }

        public static ReadOnlyCollection<Concept> ByCategory(long categoryId, long companyId, string instanceName)
        {
            var res = new List<Concept>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_Concept_ByCategoryId"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CategoryId", categoryId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new Concept
                                    {
                                        Id = rdr.GetInt64(ColumnsConceptoGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsConceptoGet.CompanyId),
                                        Description = rdr.GetString(ColumnsConceptoGet.Description),
                                        Amount = rdr.GetDecimal(ColumnsConceptoGet.Amount),
                                        IVA = rdr.GetDecimal(ColumnsConceptoGet.IVA),
                                        Type = rdr.GetInt32(ColumnsConceptoGet.Type),
                                        Category = new ConceptCategory
                                        {
                                            Id = rdr.GetInt64(ColumnsConceptoGet.CategoryId),
                                            Description = rdr.GetString(ColumnsConceptoGet.CategoryDescription)
                                        },
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsConceptoGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsConceptoGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsConceptoGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsConceptoGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsConceptoGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsConceptoGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsConceptoGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsConceptoGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsConceptoGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsConceptoGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsConceptoGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsConceptoGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsConceptoGet.Active)
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

            return new ReadOnlyCollection<Concept>(res);
        }

        public ActionResult Insert(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Billing_Concept
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @Description nvarchar(100),
             *   @Amount decimal(18,3),
             *   @IVA decimal(6,3),
             *   @CategoryId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_Concept_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Amount", this.Amount));
                        cmd.Parameters.Add(DataParameter.Input("@IVA", this.IVA));
                        cmd.Parameters.Add(DataParameter.Input("@Type", this.Type));
                        cmd.Parameters.Add(DataParameter.Input("@CategoryId", this.Category.Id));
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
                            if (cmd.Connection.State == ConnectionState.Closed)
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
            /* CREATE PROCEDURE Billing_Concept_Update
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @Description nvarchar(100),
             *   @Amount decimal(18,3),
             *   @IVA decimal(6,3),
             *   @CategoryId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_Concept_Update"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Amount", this.Amount));
                        cmd.Parameters.Add(DataParameter.Input("@IVA", this.IVA));
                        cmd.Parameters.Add(DataParameter.Input("@Type", this.Type));
                        cmd.Parameters.Add(DataParameter.Input("@CategoryId", this.Category.Id));
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
                            if (cmd.Connection.State == ConnectionState.Closed)
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