// --------------------------------
// <copyright file="InvoiceLine.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
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

    public partial class InvoiceLine
    {
        public long Id { get; set; } 
        public long CompanyId { get; set; }
        public long InvoiceId { get; set; }
        public string ConceptText { get; set; }
        public decimal BaseImport { get; set; }
        public decimal BaseImponible { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal IVA { get; set; }
        public decimal TotalIVA { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Unbilling { get; set; }
        public bool ServeiComplementari { get; set; }
        public bool Active { get; set; }

        public decimal TotalCalculated
        {
            get
            {
                return this.BaseImport * this.Quantity * (1 + this.IVA / 100);
            }
        }

        public decimal TotalIVACalculated
        {
            get
            {
                return this.BaseImport * this.Quantity * (this.IVA / 100);
            }
        }

        public decimal Total
        {
            get
            {
                return this.BaseImponible + this.TotalIVA;
            }
        }

        public static InvoiceLine Empty
        {
            get
            {
                return new InvoiceLine
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    InvoiceId = Constant.DefaultId,
                    ConceptText = string.Empty,
                    BaseImport = 0,
                    BaseImponible = 0,
                    Quantity = 0,
                    Discount = 0,
                    IVA = 0,
                    Unbilling = false,
                    ServeiComplementari = false,
                    CreatedBy = ApplicationUser.Empty,
                    ModifiedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public static string JsonList(ReadOnlyCollection<InvoiceLine> list)
        {
            if(list == null)
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

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""CompanyId"":{1},""ConceptText"":""{2}"",""BaseImport"":{3},""Quantity"":{4},""Discount"":{5},""BaseImponible"":{6},""IVA"":{7},""TotalIVA"":{8},""Total"":{9},""Unbilling"":{10},""ServeiComplementari"":{11},""CreatedBy"":{12},""CreatedOn"":""{13:dd/MM/yyyy}"",""ModifiedBy"":{14},""ModifiedOn"":""{15:dd/MM/yyyy}"",""Active"":{16}}}",
                    this.Id,
                    this.CompanyId,
                    Tools.Json.JsonCompliant(this.ConceptText),
                    Tools.Json.JsonCompliant(this.BaseImport, 2),
                    Tools.Json.JsonCompliant(this.Quantity, 2),
                    Tools.Json.JsonCompliant(this.Discount, 2),
                    Tools.Json.JsonCompliant(this.BaseImponible,2),
                    Tools.Json.JsonCompliant(this.IVA, 2),
                    Tools.Json.JsonCompliant(this.TotalIVA,2),
                    Tools.Json.JsonCompliant(this.Total, 2),
                    ConstantValue.Value(this.Unbilling),
                    ConstantValue.Value(this.ServeiComplementari),
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static ActionResult Activate(long id, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_InvoiceLine_Activate"))
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
                using (var cmd = new SqlCommand("Billing_InvoiceLine_Inactivate"))
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

        public static ReadOnlyCollection<InvoiceLine> ByInvoice(long invoiceId, long companyId, string instanceName)
        {
            var res = new List<InvoiceLine>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_InvoiceLine_ByInvoice"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@InvoiceId", invoiceId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                while(rdr.Read())
                                {
                                    res.Add(new InvoiceLine
                                    {
                                        Id = rdr.GetInt64(ColumnsInvoiceLineGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsInvoiceLineGet.CompanyId),
                                        InvoiceId = invoiceId,
                                        ConceptText = rdr.GetString(ColumnsInvoiceLineGet.Concept),
                                        BaseImport = rdr.GetDecimal(ColumnsInvoiceLineGet.BasePrice),
                                        Quantity = rdr.GetDecimal(ColumnsInvoiceLineGet.Quantity),
                                        IVA = rdr.GetDecimal(ColumnsInvoiceLineGet.IVA),
                                        Discount = rdr.GetDecimal(ColumnsInvoiceLineGet.Discount),
                                        Unbilling = rdr.GetBoolean(ColumnsInvoiceLineGet.Unbilling),
                                        ServeiComplementari = rdr.GetBoolean(ColumnsInvoiceLineGet.ServeiComplementari),
                                        BaseImponible = rdr.GetDecimal(ColumnsInvoiceLineGet.BaseImponible),
                                        TotalIVA = rdr.GetDecimal(ColumnsInvoiceLineGet.TotalIVA),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsInvoiceLineGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsInvoiceLineGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsInvoiceLineGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsInvoiceLineGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsInvoiceLineGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsInvoiceLineGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsInvoiceLineGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsInvoiceLineGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsInvoiceLineGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsInvoiceLineGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsInvoiceLineGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsInvoiceLineGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsInvoiceLineGet.Active)
                                    });
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

            return new ReadOnlyCollection<InvoiceLine>(res);
        }

        public ActionResult InsertFull(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                /* PROCEDURE [dbo].[Billing_InvoiceLine_InsertFull]
                 *   @Id bigint output,
                 *   @CompanyId bigint,
                 *   @InvoiceId bigint,
                 *   @Concept nvarchar(100),
                 *   @Quantity decimal(6,3),
                 *   @BasePrice decimal(18,3),
                 *   @IVA decimal(6,3),
                 *   @TotalIVA decimal(18,3),
                 *   @Total decimal(18,3),
                 *   @Discount decimal(6,3),
                 *   @Unbilling bit,
                 *   @ServeiComplementari bit,
                 *   @ApplicationUserId bigint */

                using (var cmd = new SqlCommand("Billing_InvoiceLine_InsertFull"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@InvoiceId", this.InvoiceId));
                        cmd.Parameters.Add(DataParameter.Input("@Concept", this.ConceptText, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Quantity", this.Quantity));
                        cmd.Parameters.Add(DataParameter.Input("@BasePrice", Math.Round(this.BaseImport, 2)));
                        cmd.Parameters.Add(DataParameter.Input("@IVA", this.IVA));
                        cmd.Parameters.Add(DataParameter.Input("@TotalIVA", this.TotalIVA));
                        cmd.Parameters.Add(DataParameter.Input("@Total", this.TotalIVA + Math.Round(this.BaseImport, 2) * this.Quantity));
                        cmd.Parameters.Add(DataParameter.Input("@Discount", this.Discount));
                        cmd.Parameters.Add(DataParameter.Input("@Unbilling", this.Unbilling));
                        cmd.Parameters.Add(DataParameter.Input("@ServeiComplementari", this.ServeiComplementari));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(this.Id);
                        }
                        catch(SqlException ex)
                        {
                            res.SetFail(ex);
                        }
                        catch(NullReferenceException ex)
                        {
                            res.SetFail(ex);
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

        public ActionResult Insert(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                /* PROCEDURE [dbo].[Billing_InvoiceLine_Insert]
                 *   @Id bigint output,
                 *   @CompanyId bigint,
                 *   @InvoiceId bigint,
                 *   @Concept nvarchar(100),
                 *   @Quantity decimal(6,3),
                 *   @BasePrice decimal(18,3),
                 *   @IVA decimal(6,3),
                 *   @Discount decimal(6,3),
                 *   @Unbilling bit,
                 *   @ServeiComplementari bit,
                 *   @ApplicationUserId bigint */

                using (var cmd = new SqlCommand("Billing_InvoiceLine_Insert"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@InvoiceId", this.InvoiceId));
                        cmd.Parameters.Add(DataParameter.Input("@Concept", this.ConceptText, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Quantity", this.Quantity));
                        cmd.Parameters.Add(DataParameter.Input("@BasePrice", Math.Round(this.BaseImport, 2)));
                        cmd.Parameters.Add(DataParameter.Input("@IVA", this.IVA));
                        cmd.Parameters.Add(DataParameter.Input("@Discount", this.Discount));
                        cmd.Parameters.Add(DataParameter.Input("@Unbilling", this.Unbilling));
                        cmd.Parameters.Add(DataParameter.Input("@ServeiComplementari", this.ServeiComplementari));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(this.Id);
                        }
                        catch(SqlException ex)
                        {
                            res.SetFail(ex);
                        }
                        catch(NullReferenceException ex)
                        {
                            res.SetFail(ex);
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

        public ActionResult Update(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                /* PROCEDURE [dbo].[Billing_InvoiceLine_Update]
                 *   @Id bigint,
                 *   @CompanyId bigint,
                 *   @Concept nvarchar(100),
                 *   @Quantity decimal(6,3),
                 *   @BasePrice decimal(18,3),
                 *   @IVA decimal(6,3),
                 *   @Discount decimal(6,3),
                 *   @ApplicationUserId bigint */

                using (var cmd = new SqlCommand("Billing_InvoiceLine_Update"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Concept", this.ConceptText, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Quantity", this.Quantity));
                        cmd.Parameters.Add(DataParameter.Input("@BasePrice", Math.Round(this.BaseImport, 2)));
                        cmd.Parameters.Add(DataParameter.Input("@IVA", this.IVA));
                        cmd.Parameters.Add(DataParameter.Input("@Discount", this.Discount));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(this.Id);
                        }
                        catch(SqlException ex)
                        {
                            res.SetFail(ex);
                        }
                        catch(NullReferenceException ex)
                        {
                            res.SetFail(ex);
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
    }
}