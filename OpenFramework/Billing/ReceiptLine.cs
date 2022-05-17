// --------------------------------
// <copyright file="ReceiptLine.cs" company="OpenFramework">
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
    using OpenFrameworkV3;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;

    public partial class ReceiptLine
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long ReceiptId { get; set; }
        public string ConceptText { get; set; }
        public decimal BaseImport { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal IVA { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public decimal TotalIVA
        {
            get
            {
                return this.BaseImport * this.Quantity * (this.IVA / 100) * (1 - this.Discount);
            }
        }

        public decimal Total
        {
            get
            {
                return this.BaseImport * this.Quantity * (1 + this.IVA / 100) * (1 - this.Discount);
            }
        }

        public static ReceiptLine Empty
        {
            get
            {
                return new ReceiptLine
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    ReceiptId = Constant.DefaultId,
                    ConceptText = string.Empty,
                    BaseImport = 0,
                    Quantity = 0,
                    Discount = 0,
                    IVA = 0,
                    CreatedBy = ApplicationUser.Empty,
                    ModifiedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public static string JsonList(ReadOnlyCollection<ReceiptLine> list)
        {
            if (list == null)
            {
                return Tools.Json.EmptyJsonList;
            }

            var res = new StringBuilder("[");
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

            res.Append("]");
            return res.ToString();
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
                        ""ConceptText"":""{2}"",
                        ""BaseImport"":{3},
                        ""Quantity"":{4},
                        ""Discount"":{5},
                        ""IVA"":{6},
                        ""TotalIVA"":{7},
                        ""Total"":{8},
                        ""CreatedBy"":{9},
                        ""CreatedOn"":""{10:dd/MM/yyyy}"",
                        ""ModifiedBy"":{11},
                        ""ModifiedOn"":""{12:dd/MM/yyyy}"",
                        ""Active"":{13}
                    }}",
                    this.Id,
                    this.CompanyId,
                    Tools.Json.JsonCompliant(this.ConceptText),
                    Tools.Json.JsonCompliant(this.BaseImport, 2),
                    Tools.Json.JsonCompliant(this.Quantity, 2),
                    Tools.Json.JsonCompliant(this.Discount, 2),
                    Tools.Json.JsonCompliant(this.IVA, 2),
                    Tools.Json.JsonCompliant(this.TotalIVA, 2),
                    Tools.Json.JsonCompliant(this.Total, 2),
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static ActionResult Activate(long id, long receiptId, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_ReceiptLine_Activate"))
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
                            Receipt.UpdateAmounts(receiptId, instanceName);
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

        public static ActionResult Inactivate(long id, long receiptId, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_ReceiptLine_Inactivate"))
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
                            Receipt.UpdateAmounts(receiptId, instanceName);
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

        public static ReadOnlyCollection<ReceiptLine> ByReceipt(long receiptId, long companyId, string instanceName)
        {
            var res = new List<ReceiptLine>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_ReceiptLine_ByReceipt"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ReceiptId", receiptId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new ReceiptLine
                                    {
                                        Id = rdr.GetInt64(ColumnsReceiptLineGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsReceiptLineGet.CompanyId),
                                        ReceiptId = receiptId,
                                        ConceptText = rdr.GetString(ColumnsReceiptLineGet.Concept),
                                        BaseImport = rdr.GetDecimal(ColumnsReceiptLineGet.BasePrice),
                                        Quantity = rdr.GetDecimal(ColumnsReceiptLineGet.Quantity),
                                        IVA = rdr.GetDecimal(ColumnsReceiptLineGet.IVA),
                                        Discount = rdr.GetDecimal(ColumnsReceiptLineGet.Discount),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsReceiptLineGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsReceiptLineGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsReceiptLineGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsReceiptLineGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsReceiptLineGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsReceiptLineGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsReceiptLineGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsReceiptLineGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsReceiptLineGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsReceiptLineGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsReceiptLineGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsReceiptLineGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsReceiptLineGet.Active)
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

            return new ReadOnlyCollection<ReceiptLine>(res);
        }

        public ActionResult Insert(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* PROCEDURE [dbo].[Billing_ReceiptLine_Insert]
             *   @CompanyId bigint,
             *   @ReceiptId bigint,
             *   @Concept nvarchar(100),
             *   @Quantity decimal(6,3),
             *   @BasePrice decimal (18,3),
             *   @IVA decimal(6,3),
             *   @Discount decimal(6,3),
             *   @TotalIVA decimal(18,3),
             *   @Total decimal(18,3),
             *   @ApplicationUserId bigint */

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_ReceiptLine_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ReceiptId", this.ReceiptId));
                        cmd.Parameters.Add(DataParameter.Input("@Concept", this.ConceptText, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Quantity", this.Quantity));
                        cmd.Parameters.Add(DataParameter.Input("@BasePrice", this.BaseImport));
                        cmd.Parameters.Add(DataParameter.Input("@IVA", this.IVA));
                        cmd.Parameters.Add(DataParameter.Input("@TotalIVA", this.TotalIVA));
                        cmd.Parameters.Add(DataParameter.Input("@Total", this.Total));
                        cmd.Parameters.Add(DataParameter.Input("@Discount", this.Discount));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            Receipt.UpdateAmounts(this.ReceiptId, instanceName);
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
            /* PROCEDURE [dbo].[Billing_ReceiptLine_Update]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @Concept nvarchar(100),
             *   @Quantity decimal(6,3),
             *   @BasePrice decimal(18,3),
             *   @IVA decimal(6,3),
             *   @Discount decimal(6,3),
             *   @ApplicationUserId bigint */

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_ReceiptLine_Update"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Concept", this.ConceptText, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Quantity", this.Quantity));
                        cmd.Parameters.Add(DataParameter.Input("@BasePrice", this.BaseImport));
                        cmd.Parameters.Add(DataParameter.Input("@IVA", this.IVA));
                        cmd.Parameters.Add(DataParameter.Input("@TotalIVA", this.TotalIVA));
                        cmd.Parameters.Add(DataParameter.Input("@Total", this.Total));
                        cmd.Parameters.Add(DataParameter.Input("@Discount", this.Discount));
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
    }
}