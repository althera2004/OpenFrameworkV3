// --------------------------------
// <copyright file="Receipt.cs" company="OpenFramework">
//     Copyright (c) 2013 - ColumnsInvoiceLineGet. All rights reserved.
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
    using System.IO;
    using System.Text;
    using System.Web;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using OpenFrameworkV3;
    using OpenFrameworkV3.Core;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Companies;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;
    using OpenFrameworkV3.Tools;

    public partial class Receipt
    {
        private List<ReceiptLine> lines;

        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long Number { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public long ItemDefinitionId { get; set; }
        public long ItemId { get; set; }
        public string PayerName { get; set; }
        public string PayerCIF { get; set; }
        public string ChargerName { get; set; }
        public string ChargerCIF { get; set; }
        public int Status { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal TotalIVA { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public ApplicationUser  CreatedBy { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active;

        public string InstanceName { get; set; }

        public DateTime? DateConfirmed { get; set; }
        public DateTime? DateSend { get; set; }
        public DateTime? DatePay { get; set; }
        public DateTime? DateVto { get; set; }
        public DateTime? Quote { get; set; }
        public bool Devolucion { get; set; }
        public int Type { get; set; }

        public ReadOnlyCollection<ReceiptLine> Lines
        {
            get
            {
                if (this.lines == null)
                {
                    this.lines = new List<ReceiptLine>();
                }

                return new ReadOnlyCollection<ReceiptLine>(this.lines);
            }
        }

        public void AddLine(ReceiptLine line, bool computeTotal)
        {
            if (this.lines == null)
            {
                this.lines = new List<ReceiptLine>();
            }

            if (computeTotal)
            {
                this.Total += line.Total;
            }

            this.lines.Add(line);
        }

        public void GetLines()
        {
            var items = ReceiptLine.ByReceipt(this.Id, this.CompanyId, this.InstanceName);

            foreach (var item in items)
            {
                this.AddLine(item, false);
            }
        }

        public static Receipt Empty
        {
            get
            {
                return new Receipt
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    Number = Constant.DefaultId,
                    ItemDefinitionId = Constant.DefaultId,
                    ItemId = Constant.DefaultId,
                    PayerName = string.Empty,
                    PayerCIF = string.Empty,
                    ChargerName = string.Empty,
                    ChargerCIF = string.Empty,
                    Date = DateTime.Now,
                    Status = 0,
                    BaseAmount = 0,
                    TotalIVA = 0,
                    Total = 0,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false,
                    lines = new List<ReceiptLine>(),
                    Subject = string.Empty
                };
            }
        }

        public static string JsonList(ReadOnlyCollection<Receipt> list)
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

                res.Append(item.JsonSimple);
            }

            res.Append("]");
            return res.ToString();
        }

        public string JsonSimple
        {
            get
            {
                var datePayText = Constant.JavaScriptNull;
                if(this.DatePay != null)
                {
                    datePayText = string.Format(CultureInfo.InvariantCulture, @"""{0:dd/MM/yyyy}""", this.DatePay);
                }
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"
                    {{
                        ""Id"":{0},
                        ""CompanyId"":{1},
                        ""Number"":{2},
                        ""Subject"":""{3}"",
                        ""PayerName"":""{4}"",
                        ""PayerCIF"":""{5}"",
                        ""Date"":""{6:dd/MM/yyyy}"",
                        ""DatePay"":{13},
                        ""Status"":{7},
                        ""BaseAmount"":{8:#0.00},
                        ""TotalIVA"":{9:#0.00},
                        ""Total"":{10:#0.00},
                        ""Type"":{11},""Active"":{12}
                    }}",
                    this.Id,
                    this.CompanyId,
                    this.Number,
                    Tools.Json.JsonCompliant(this.Subject),
                    Tools.Json.JsonCompliant(this.PayerName),
                    Tools.Json.JsonCompliant(this.PayerCIF),
                    this.Date,
                    this.Status,
                    this.BaseAmount,
                    this.TotalIVA,
                    this.Total,
                    this.Type,
                    ConstantValue.Value(this.Active),
                    datePayText);
            }
        }

        public string Json
        {
            get
            {
                var datePayText = Constant.JavaScriptNull;
                if(this.DatePay != null)
                {
                    datePayText = string.Format(CultureInfo.InvariantCulture, @"""{0:dd/MM/yyyy}""", this.DatePay);
                }


                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"":{0},
                        ""CompanyId"":{1},
                        ""Number"":{2},
                        ""ItemDefinitionId"":{3},
                        ""ItemId"":{4},
                        ""PayerName"":""{5}"",
                        ""PayerCIF"":""{6}"",
                        ""ChargerName"":""{7}"",
                        ""ChargerCIF"":""{8}"",
                        ""Date"":""{9:dd/MM/yyyy}"",
                        ""DatePay"":{22},
                        ""Lines"":{10},
                        ""Status"":{11},
                        ""BaseAmount"":{12:#0.00},
                        ""TotalIVA"":{13:#0.00},
                        ""Total"":{14:#0.00},
                        ""Type"":{15},
                        ""Devolucion"":{16},
                        ""CreatedBy"":{17},
                        ""CreatedOn"":""{18:dd/MM/yyyy}"",
                        ""ModifiedBy"":{19},
                        ""ModifiedOn"":""{20:dd/MM/yyyy}"",
                        ""Active"":{21}
                    }}",
                    this.Id,
                    this.CompanyId,
                    this.Number,
                    this.ItemDefinitionId,
                    this.ItemId,
                    Tools.Json.JsonCompliant(this.PayerName),
                    Tools.Json.JsonCompliant(this.PayerCIF),
                    Tools.Json.JsonCompliant(this.ChargerName),
                    Tools.Json.JsonCompliant(this.ChargerCIF),
                    this.Date,
                    ReceiptLine.JsonList(this.Lines),
                    this.Status,
                    this.BaseAmount,
                    this.TotalIVA,
                    this.Total,
                    this.Type,
                    ConstantValue.Value(this.Devolucion),
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active),
                    datePayText);
            }
        }

        public ActionResult Insert(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                /* CREATE PROCEDURE Billing_Receipt_Insert
                 *   @Id bigint output,
                 *   @CompanyId bigint,
                 *   @Number bigint,
                 *   @Subject nvarchar(100),
                 *   @ItemDefinitionId bigint,
                 *   @ItemId bigint,
                 *   @PayerName nvarchar(100),
                 *   @PayerCIF nvarchar(15),
                 *   @ChargerName nvarchar(100),
                 *   @ChargerCIF nvarchar(15),
                 *   @BaseAmount decimal(18, 3),
                 *   @TotalIVA decimal(18, 3),
                 *   @Total decimal(18, 3),
                 *   @Date datetime,
                 *   @Status int,
                 *   @Notes nvarchar(500),
                 *   @DateConfirmed datetime,
                 *   @DateSend datetime,
                 *   @DatePay datetime,
                 *   @Quote datetime,
                 *   @Devolucion bit,
                 *   @Type int,
                 *   @ApplicationUserId bigint */
                using (var cmd = new SqlCommand("Billing_Receipt_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Number", this.Number));
                        cmd.Parameters.Add(DataParameter.Input("@Subject", this.Subject, 100));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@PayerName", this.PayerName, 100));
                        cmd.Parameters.Add(DataParameter.Input("@PayerCIF", this.PayerCIF, 15));
                        cmd.Parameters.Add(DataParameter.Input("@ChargerName", this.ChargerName, 100));
                        cmd.Parameters.Add(DataParameter.Input("@ChargerCIF ", this.ChargerCIF, 15));
                        cmd.Parameters.Add(DataParameter.Input("@BaseAmount ", this.BaseAmount));
                        cmd.Parameters.Add(DataParameter.Input("@TotalIVA ", this.TotalIVA));
                        cmd.Parameters.Add(DataParameter.Input("@Total ", this.Total));
                        cmd.Parameters.Add(DataParameter.Input("@Date ", this.Date));
                        cmd.Parameters.Add(DataParameter.Input("@Status", this.Status));
                        cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, 500));
                        cmd.Parameters.Add(DataParameter.Input("@DateConfirmed", this.DateConfirmed));
                        cmd.Parameters.Add(DataParameter.Input("@DateSend", this.DateSend));
                        cmd.Parameters.Add(DataParameter.Input("@DatePay", this.DatePay));
                        cmd.Parameters.Add(DataParameter.Input("@Quote", this.Quote));
                        cmd.Parameters.Add(DataParameter.Input("@Devolucion", this.Devolucion));
                        cmd.Parameters.Add(DataParameter.Input("@Type", this.Type));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(Id);
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

        public static ReadOnlyCollection<Receipt> Filter(string from, string to, bool pendiente, bool cobrado, long companyId, string instanceName)
        {
            var res = new List<Receipt>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_Receipt_Filter"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        var dateFrom = Tools.DateFormat.FromStringddMMyyy(from);
                        var dateTo = Tools.DateFormat.FromStringddMMyyy(to);
                        cmd.Parameters.Add(DataParameter.Input("@DateFrom", dateFrom));
                        cmd.Parameters.Add(DataParameter.Input("@DateTo", dateTo));
                        cmd.Parameters.Add(DataParameter.Input("@Pendiente", pendiente));
                        cmd.Parameters.Add(DataParameter.Input("@Cobrado", cobrado));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    var newReceipt = new Receipt
                                    {
                                        Id = rdr.GetInt64(ColumnsReceiptGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsReceiptGet.CompanyId),
                                        Number = rdr.GetInt64(ColumnsReceiptGet.Number),
                                        Subject = rdr.GetString(ColumnsReceiptGet.Subject),
                                        Date = rdr.GetDateTime(ColumnsReceiptGet.Date),
                                        BaseAmount = rdr.GetDecimal(ColumnsReceiptGet.BaseAmount),
                                        TotalIVA = rdr.GetDecimal(ColumnsReceiptGet.TotalIVA),
                                        Total = rdr.GetDecimal(ColumnsReceiptGet.Total),
                                        Type = rdr.GetInt32(ColumnsReceiptGet.Type),
                                        PayerName = rdr.GetString(ColumnsReceiptGet.PayerName),
                                        PayerCIF = rdr.GetString(ColumnsReceiptGet.PayerCIF),
                                        ChargerName = rdr.GetString(ColumnsReceiptGet.ChargerName),
                                        ChargerCIF = rdr.GetString(ColumnsReceiptGet.ChargerCIF),
                                        ItemDefinitionId = rdr.GetInt64(ColumnsReceiptGet.ItemDefinitionId),
                                        ItemId = rdr.GetInt64(ColumnsReceiptGet.ItemId),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsReceiptGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsReceiptGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsReceiptGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsReceiptGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsReceiptGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsReceiptGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsReceiptGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsReceiptGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsReceiptGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsReceiptGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsReceiptGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsReceiptGet.ModifiedOn),
                                        Status = rdr.GetInt32(ColumnsReceiptGet.Status),
                                        Active = rdr.GetBoolean(ColumnsReceiptGet.Active)
                                    };

                                    if (!rdr.IsDBNull(ColumnsReceiptGet.DateConfirmed))
                                    {
                                        newReceipt.DateConfirmed = rdr.GetDateTime(ColumnsReceiptGet.DateConfirmed);
                                    }

                                    if (!rdr.IsDBNull(ColumnsReceiptGet.DateSend))
                                    {
                                        newReceipt.DateSend = rdr.GetDateTime(ColumnsReceiptGet.DateSend);
                                    }

                                    if (!rdr.IsDBNull(ColumnsReceiptGet.DatePay))
                                    {
                                        newReceipt.DatePay = rdr.GetDateTime(ColumnsReceiptGet.DatePay);
                                    }

                                    res.Add(newReceipt);
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

            return new ReadOnlyCollection<Receipt>(res);
        }

        public static Receipt ById(long id, long companyId, string instanceName)
        {
            var res = Receipt.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_Receipt_ById"))
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
                                    res.Id = rdr.GetInt64(ColumnsReceiptGet.Id);
                                    res.CompanyId = rdr.GetInt64(ColumnsReceiptGet.CompanyId);
                                    res.Number = rdr.GetInt64(ColumnsReceiptGet.Number);
                                    res.Subject = rdr.GetString(ColumnsReceiptGet.Subject);
                                    res.Date = rdr.GetDateTime(ColumnsReceiptGet.Date);
                                    res.BaseAmount = rdr.GetDecimal(ColumnsReceiptGet.BaseAmount);
                                    res.TotalIVA = rdr.GetDecimal(ColumnsReceiptGet.TotalIVA);
                                    res.Total = rdr.GetDecimal(ColumnsReceiptGet.Total);
                                    res.Type = rdr.GetInt32(ColumnsReceiptGet.Type);
                                    res.PayerName = rdr.GetString(ColumnsReceiptGet.PayerName);
                                    res.PayerCIF = rdr.GetString(ColumnsReceiptGet.PayerCIF);
                                    res.ChargerName = rdr.GetString(ColumnsReceiptGet.ChargerName);
                                    res.ChargerCIF = rdr.GetString(ColumnsReceiptGet.ChargerCIF);
                                    res.ItemDefinitionId = rdr.GetInt64(ColumnsReceiptGet.ItemDefinitionId);
                                    res.ItemId = rdr.GetInt64(ColumnsReceiptGet.ItemId);
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsReceiptGet.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsReceiptGet.CreatedBy),
                                            Name = rdr.GetString(ColumnsReceiptGet.CreatedByName),
                                            LastName = rdr.GetString(ColumnsReceiptGet.CreatedByLastName),
                                            LastName2 = rdr.GetString(ColumnsReceiptGet.CreatedByLastName2)
                                        }
                                    };
                                    res.CreatedOn = rdr.GetDateTime(ColumnsReceiptGet.CreatedOn);
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsReceiptGet.ModifiedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsReceiptGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnsReceiptGet.ModifiedByName),
                                            LastName = rdr.GetString(ColumnsReceiptGet.ModifiedByLastName),
                                            LastName2 = rdr.GetString(ColumnsReceiptGet.ModifiedByLastName2)
                                        }
                                    };
                                    res.ModifiedOn = rdr.GetDateTime(ColumnsReceiptGet.ModifiedOn);
                                    res.Status = rdr.GetInt32(ColumnsReceiptGet.Status);
                                    res.Active = rdr.GetBoolean(ColumnsReceiptGet.Active);

                                    if (!rdr.IsDBNull(ColumnsReceiptGet.DatePay))
                                    {
                                        res.DatePay = rdr.GetDateTime(ColumnsReceiptGet.DatePay);
                                    }

                                    res.GetLines();
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

        public static ActionResult UpdateAmounts(long id, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_Receipt_UpdateAmounts"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ReceiptId", id));

                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(id);
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

        public static ActionResult SetSent(long id, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_Receipt_SetSent"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ReceiptId", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            RenderPdf(id, companyId, instanceName);
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

        public static ActionResult SetPayed(long id,long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_Receipt_SetPayed"))
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
                            RenderPdf(id, companyId, instanceName);
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
                using (var cmd = new SqlCommand("Billing_Receipt_Inactivate"))
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

        public static ActionResult Activate(long id, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Billing_Receipt_Activate"))
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


        public static void RenderPdf(long receiptId, long companyId, string instanceName)
        {
            var receipt = Receipt.ById(receiptId, companyId, instanceName);
            var instance = Persistence.InstanceByName(instanceName);
            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            try
            {
                // Se preparan los objetos para el PDF
                var fontNameCheckbox = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "webdings.ttf");
                var fontName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "calibri.ttf");
                var fontNameBold = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "calibrib.ttf");
                var baseFontCheckBox = BaseFont.CreateFont(fontNameCheckbox, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                var baseFont = BaseFont.CreateFont(fontName, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                var baseFontBold = BaseFont.CreateFont(fontNameBold, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                var path = HttpContext.Current.Request.PhysicalApplicationPath;
                if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                {
                    path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
                }

                var fileNameTemplate = string.Format(CultureInfo.InvariantCulture, @"{0}{1}ReceiptTemplate.pdf", Instance.Path.PdfTemplates(instanceName), Instance.Path.PdfTemplates(instanceName).EndsWith("\\") ? string.Empty : "\\");
                /*if (instance.Config.Billing.CustomInvoiceTemplate)
                {
                    fileNameTemplate = string.Format(CultureInfo.InvariantCulture, @"{0}{1}InvoiceTemplate.pdf", instance.PathData, instance.PathPdfTemplates.EndsWith("\\") ? string.Empty : "\\");
                }*/

                if (!File.Exists(fileNameTemplate))
                {
                    fileNameTemplate = string.Format(CultureInfo.InvariantCulture, @"{0}{1}\Billing\ReceiptTemplate.pdf", HttpContext.Current.Request.PhysicalApplicationPath, HttpContext.Current.Request.PhysicalApplicationPath.EndsWith("\\") ? string.Empty : "\\");
                }

                // Este el el nombre de la plantilla
                var targetFolder = string.Format(CultureInfo.InvariantCulture, @"{0}{1}\InvoicesPrint", HttpContext.Current.Request.PhysicalApplicationPath, HttpContext.Current.Request.PhysicalApplicationPath.EndsWith("\\") ? string.Empty : "\\");

                // Este el el path final dónde se genera el pdf
                var filePathNew = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", targetFolder, companyId);
                Basics.VerifyFolder(filePathNew);
                var fileNameNew = string.Format(CultureInfo.InvariantCulture, @"{0}\Receipt_{1}.pdf", filePathNew, receiptId);

                // ---------------------------------------
                using (var existingFileStream = new FileStream(fileNameTemplate, FileMode.Open))
                {
                    using (var newFileStream = new FileStream(fileNameNew, FileMode.Create))
                    {
                        var pdfReader = new PdfReader(existingFileStream);
                        var stamper = new PdfStamper(pdfReader, newFileStream);
                        var form = stamper.AcroFields;
                        var fieldKeys = form.Fields.Keys;
                        Pdf.SetMetadata(pdfReader, stamper, ApplicationDictionary.Translate("Billing_Receipt"), string.Empty, "OpenFramework - " + instance.Name);
                        var company = Company.ById(companyId, instanceName);

                        #region lineas
                        decimal baseImponible = 0;
                        decimal iva = 0;
                        decimal iva2 = 0;
                        decimal iva3 = 0;
                        decimal iva4 = 0;
                        decimal total = 0;

                        var table = new PdfPTable(6)
                        {
                            WidthPercentage = 100,
                            HorizontalAlignment = 1,
                            SpacingBefore = 0f,
                            SpacingAfter = 0f
                        };
                        table.SetWidths(new float[] { 75f, 50f, 25f, 30f, 30f, 40f });
                        table.AddCell(HeaderCellLeft(ApplicationDictionary.Translate("Billing_Receipt_HeaderPDF_Description"), baseFontBold, 3));
                        table.AddCell(HeaderCellRight(ApplicationDictionary.Translate("Billing_Receipt_HeaderPDF_Quantity"), baseFontBold));
                        table.AddCell(HeaderCellRight(ApplicationDictionary.Translate("Billing_Receipt_HeaderPDF_Amount"), baseFontBold));
                        //table.AddCell(HeaderCellRight(ApplicationDictionary.Translate("Billing_Receipt_HeaderPDF_IVA"), baseFontBold));
                        table.AddCell(HeaderCellRight(ApplicationDictionary.Translate("Billing_Receipt_HeaderPDF_Total"), baseFontBold));

                        int cont = 1;
                        var odd = true;
                        foreach (var line in receipt.Lines)
                        {
                            table.AddCell(CellTable(line.ConceptText, baseFont, odd, 0, 3));
                            table.AddCell(CellTableMoney(line.Quantity, baseFont, odd, 0));
                            table.AddCell(CellTableMoney(line.BaseImport, baseFont, odd, 0));
                            //table.AddCell(CellTablePercent(line.IVA, 0, baseFont, odd, 0));
                            table.AddCell(CellTableMoney(line.Quantity * line.BaseImport, baseFont, odd, 0));
                            cont++;
                            odd = !odd;

                            baseImponible += line.BaseImport;
                            var ivaLine = line.BaseImport * line.IVA / 100;
                            iva += ivaLine;
                            total += line.BaseImport * line.Quantity + ivaLine;

                            switch (line.IVA)
                            {
                                case 4: iva2 += ivaLine; break;
                                case 10: iva3 += ivaLine; break;
                                case 21: iva4 += ivaLine; break;
                            }
                        }

                        odd = true;
                        table.AddCell(CellTableRightResume(ApplicationDictionary.Translate("Billing_Receipt_HeaderPDF_Total"), baseFont, odd, 0, 5));
                        table.AddCell(CellTableMoneyResume(total, baseFontBold, odd, 0));

                        odd = false;
                        if (!string.IsNullOrEmpty(receipt.Notes))
                        {
                            table.AddCell(CellTableLeftResume(ApplicationDictionary.Translate("Common_Notes"), baseFont, odd, Rectangle.BOTTOM_BORDER, 5));
                            table.AddCell(new PdfPCell(new Phrase(receipt.Notes, new Font(baseFont, 9, Font.NORMAL, BaseColor.BLACK)))
                            {
                                Border = Rectangle.NO_BORDER,
                                Colspan = 7,
                                Padding = 6f,
                                PaddingTop = 6f,
                                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240)
                            });
                        }

                        var cb1 = stamper.GetOverContent(1);
                        table.TotalWidth = 470;
                        table.WriteSelectedRows(0, -1, 65, 555, cb1);
                        #endregion

                        // Se recorren todos los campos del pdf y a cada uno se pone el contenido
                        foreach (string fieldKey in fieldKeys)
                        {
                            switch (fieldKey)
                            {
                                case "ReciboSubject":
                                    form.SetField(fieldKey, receipt.Subject);
                                    form.SetFieldProperty(fieldKey, "textsize", (float)12, null);
                                    form.SetFieldProperty(fieldKey, "textfont", baseFontBold, null);
                                    form.RegenerateField(fieldKey);
                                    break;
                                case "FechaRecibo":
                                    form.SetField(fieldKey, "Data: " + string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", receipt.Date));
                                    form.SetFieldProperty(fieldKey, "textsize", (float)12, null);
                                    form.SetFieldProperty(fieldKey, "textfont", baseFontBold, null);
                                    form.RegenerateField(fieldKey);
                                    break;
                                case "NumeroRecibo":
                                    form.SetField(fieldKey, string.Format(CultureInfo.InvariantCulture, "Número rebut: {0:0000}", receipt.Number));
                                    form.SetFieldProperty(fieldKey, "textsize", (float)12, null);
                                    form.SetFieldProperty(fieldKey, "textfont", baseFontBold, null);
                                    form.RegenerateField(fieldKey);
                                    break;


                                case "PayerName":
                                    form.SetField(fieldKey, receipt.PayerName);
                                    form.SetFieldProperty(fieldKey, "textsize", (float)12, null);
                                    form.SetFieldProperty(fieldKey, "textfont", baseFontBold, null);
                                    form.RegenerateField(fieldKey);
                                    break;
                                case "PayerNIF":
                                    form.SetField(fieldKey, "NIF: " + receipt.PayerCIF);
                                    form.SetFieldProperty(fieldKey, "textsize", (float)10, null);
                                    form.SetFieldProperty(fieldKey, "textfont", baseFont, null);
                                    form.RegenerateField(fieldKey);
                                    break;


                                case "ChargerName":
                                    form.SetField(fieldKey, receipt.ChargerName);
                                    form.SetFieldProperty(fieldKey, "textsize", (float)12, null);
                                    form.SetFieldProperty(fieldKey, "textfont", baseFontBold, null);
                                    form.RegenerateField(fieldKey);
                                    break;
                                case "ChargerCIF":
                                    form.SetField(fieldKey, "CIF: " + receipt.ChargerCIF);
                                    form.SetFieldProperty(fieldKey, "textsize", (float)10, null);
                                    form.SetFieldProperty(fieldKey, "textfont", baseFont, null);
                                    form.RegenerateField(fieldKey);
                                    break;
                            }
                        }

                        var logoPath = Instance.Path.Data(instanceName) + "\\CompanyData\\" + companyId.ToString() + "\\PDFLogo.png";
                        if (!File.Exists(logoPath))
                        {
                            logoPath = Instance.Path.Data(instanceName) + "\\CompanyData\\" + companyId.ToString() + "\\PDFLogo.png";
                            if (!File.Exists(logoPath))
                            {
                                logoPath = Instance.Path.Data(instanceName) + "\\CompanyData\\" + companyId.ToString() + "\\Logo.png";
                            }
                        }

                        var logo = Image.GetInstance(logoPath);
                        logo.SetAbsolutePosition(65, 720);
                        var canvas1 = stamper.GetOverContent(1);
                        canvas1.AddImage(logo);
                        // Se acaba el rellenado del formulario
                        stamper.FormFlattening = true;

                        try
                        {
                            stamper.Close();
                            pdfReader.Close();
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.Trace(ex, "Print receipt");
                            fileNameNew = ex.Message;
                        }
                    }
                }
                // ---------------------------------------
            }
            catch (Exception ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.InvariantCulture, @"Invice({0}, {1})", instance.Name, receiptId));
            }
        }

        #region cells


        /// <summary>Creates de cell for list header</summary>
        /// <param name="label">Label to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cel for list header</returns>
        public static PdfPCell HeaderCellLeft(string label, BaseFont font)
        {
            var finalLabel = string.Empty;
            if (!string.IsNullOrEmpty(label))
            {
                finalLabel = label;
            }

            return new PdfPCell(new Phrase(finalLabel, new Font(font, 9, Font.NORMAL, BaseColor.BLACK)))
            {
                Border = Rectangle.BOTTOM_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 4
            };
        }

        /// <summary>Creates de cell for list header</summary>
        /// <param name="label">Label to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cel for list header</returns>
        public static PdfPCell HeaderCellLeft(string label, BaseFont font, int span)
        {
            var finalLabel = string.Empty;
            if (!string.IsNullOrEmpty(label))
            {
                finalLabel = label;
            }

            return new PdfPCell(new Phrase(finalLabel, new Font(font, 9, Font.NORMAL, BaseColor.BLACK)))
            {
                Border = Rectangle.BOTTOM_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 4,
                Colspan = span
            };
        }

        /// <summary>Creates de cell for list header</summary>
        /// <param name="label">Label to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cel for list header</returns>
        public static PdfPCell HeaderCellRight(string label, BaseFont font)
        {
            var finalLabel = string.Empty;
            if (!string.IsNullOrEmpty(label))
            {
                finalLabel = label;
            }

            return new PdfPCell(new Phrase(finalLabel, new Font(font, 9, Font.NORMAL, BaseColor.BLACK)))
            {
                Border = Rectangle.BOTTOM_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 4
            };
        }

        /// <summary>Creates de cell for list header</summary>
        /// <param name="label">Label to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cel for list header</returns>
        public static PdfPCell HeaderCell(string label, BaseFont font)
        {
            var finalLabel = string.Empty;
            if (!string.IsNullOrEmpty(label))
            {
                finalLabel = label;
            }

            return new PdfPCell(new Phrase(finalLabel.ToUpperInvariant(), new Font(font, 9, Font.NORMAL, BaseColor.BLACK)))
            {
                Border = Rectangle.BOTTOM_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 4
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="odd">Indicates if row is odd</param>
        /// <param name="border">Type of border</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTableCenter(string value, BaseFont font, bool odd, int border)
        {
            var finalValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

            return new PdfPCell(new Phrase(finalValue, new Font(font, 9, Font.NORMAL, BaseColor.BLACK)))
            {
                HorizontalAlignment = Rectangle.ALIGN_CENTER,
                Border = border,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240)
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="odd">Indicates if row is odd</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTableCenter(string value, BaseFont font, bool odd)
        {
            var finalValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

            return new PdfPCell(new Phrase(finalValue, new Font(font, 9, Font.NORMAL, BaseColor.BLACK)))
            {
                HorizontalAlignment = Rectangle.ALIGN_CENTER,
                Border = Rectangle.NO_BORDER,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240)
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="odd">Indicates if row is odd</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTableRight(string value, BaseFont font, bool odd)
        {
            var finalValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

            return new PdfPCell(new Phrase(finalValue, new Font(font, 9, Font.NORMAL, BaseColor.BLACK)))
            {
                HorizontalAlignment = Rectangle.ALIGN_RIGHT,
                Border = Rectangle.NO_BORDER,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240)
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="odd">Indicates if row is odd</param>
        /// <param name="border">Type of border</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTable(string value, BaseFont font, bool odd, int border)
        {
            var finalValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

            return new PdfPCell(new Phrase(finalValue, new Font(font, 9, Font.NORMAL, BaseColor.BLACK)))
            {
                Border = border,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240)
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="odd">Indicates if row is odd</param>
        /// <param name="border">Type of border</param>
        /// <param name="span">Column span</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTable(string value, BaseFont font, bool odd, int border, int span)
        {
            var finalValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

            return new PdfPCell(new Phrase(finalValue, new Font(font, 9, Font.NORMAL, BaseColor.BLACK)))
            {
                Colspan = span,
                Border = border,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240)
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="odd">Indicates if row is odd</param>
        /// <param name="border">Type of border</param>
        /// <param name="span">Column span</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTableLeftResume(string value, BaseFont font, bool odd, int border, int span)
        {
            var finalValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

            return new PdfPCell(new Phrase(finalValue, new Font(font, 13, Font.NORMAL, BaseColor.BLACK)))
            {
                Colspan = span,
                Border = border,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240)
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="odd">Indicates if row is odd</param>
        /// <param name="border">Type of border</param>
        /// <param name="span">Column span</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTableRightResumeSmall(string value, BaseFont font, bool odd, int border, int span)
        {
            var finalValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

            return new PdfPCell(new Phrase(finalValue, new Font(font, 12, Font.NORMAL, BaseColor.BLACK)))
            {
                HorizontalAlignment = Rectangle.ALIGN_RIGHT,
                Colspan = span,
                Border = border,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240)
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="odd">Indicates if row is odd</param>
        /// <param name="border">Type of border</param>
        /// <param name="span">Column span</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTableRightResume(string value, BaseFont font, bool odd, int border, int span)
        {
            var finalValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

            return new PdfPCell(new Phrase(finalValue, new Font(font, 13, Font.NORMAL, BaseColor.BLACK)))
            {
                HorizontalAlignment = Rectangle.ALIGN_RIGHT,
                Colspan = span,
                Border = border,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240)
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="odd">Indicates if row is odd</param>
        /// <param name="border">Type of border</param>
        /// <param name="span">Column span</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTableInfo(string value, BaseFont font, bool odd, int border, int span)
        {
            var finalValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

            return new PdfPCell(new Phrase(finalValue, new Font(font, 10, Font.NORMAL, BaseColor.BLACK)))
            {
                HorizontalAlignment = Rectangle.ALIGN_LEFT,
                Colspan = span,
                Border = border,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240)
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="decimals">Number or decimals</param>
        /// <param name="font">Font for content</param>
        /// <param name="odd">Indicates if row is odd</param>
        /// <param name="border">Type of border</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTablePercent(decimal value, int decimals, BaseFont font, bool odd, int border)
        {
            var pattern = "{0:#0";
            if (decimals > 0)
            {
                pattern += ".";
                for (var x = 0; x < decimals; x++)
                {
                    pattern += "0";
                }
            }

            pattern += "} %";

            var finalValue = string.Format(CultureInfo.InvariantCulture, pattern, value);

            return new PdfPCell(new Phrase(finalValue, new Font(font, 9, Font.NORMAL, BaseColor.BLACK)))
            {
                HorizontalAlignment = Rectangle.ALIGN_RIGHT,
                Border = border,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240)
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="odd">Indicates if row is odd</param>
        /// <param name="border">Type of border</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTableMoneyResume(decimal value, BaseFont font, bool odd, int border)
        {
            var finalValue = Basics.SpanishMoney(value);

            return new PdfPCell(new Phrase(finalValue, new Font(font, 13, Font.NORMAL, BaseColor.BLACK)))
            {
                HorizontalAlignment = Rectangle.ALIGN_RIGHT,
                Border = border,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240)
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="odd">Indicates if row is odd</param>
        /// <param name="border">Type of border</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTableMoneyInfo(decimal value, BaseFont font, bool odd, int border)
        {
            var finalValue = Basics.SpanishMoney(value);

            return new PdfPCell(new Phrase(finalValue, new Font(font, 10, Font.NORMAL, BaseColor.BLACK)))
            {
                HorizontalAlignment = Rectangle.ALIGN_RIGHT,
                Border = border,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240)
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="odd">Indicates if row is odd</param>
        /// <param name="border">Type of border</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTableMoneyResumeSmall(decimal value, BaseFont font, bool odd, int border)
        {
            var finalValue = Basics.SpanishMoney(value);

            return new PdfPCell(new Phrase(finalValue, new Font(font, 12, Font.NORMAL, BaseColor.BLACK)))
            {
                HorizontalAlignment = Rectangle.ALIGN_RIGHT,
                Border = border,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240)
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="odd">Indicates if row is odd</param>
        /// <param name="border">Type of border</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTableMoney(decimal value, BaseFont font, bool odd, int border)
        {
            var finalValue = Basics.SpanishMoney(value);

            return new PdfPCell(new Phrase(finalValue, new Font(font, 9, Font.NORMAL, BaseColor.BLACK)))
            {
                HorizontalAlignment = Rectangle.ALIGN_RIGHT,
                Border = border,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240)
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="odd">Indicates if row is odd</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTable(string value, BaseFont font, bool odd)
        {
            var finalValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

            return new PdfPCell(new Phrase(finalValue, new Font(font, 9, Font.NORMAL, BaseColor.BLACK)))
            {
                Border = Rectangle.NO_BORDER,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240)
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="odd">Indicates if row is odd</param>
        /// <param name="font">Font for content</param>
        /// <param name="border">Type fo border</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTableBlank(bool odd, BaseFont font, int border)
        {
            var color = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240);
            return new PdfPCell(new Phrase("͏-", new Font(font, 9, Font.NORMAL, color)))
            {
                Border = border,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = color
            };
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="odd">Indicates if row is odd</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTableBlank(bool odd, BaseFont font)
        {
            var color = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240);
            return new PdfPCell(new Phrase("͏-", new Font(font, 9, Font.NORMAL, color)))
            {
                Border = Rectangle.NO_BORDER,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = color
            };
        }
        #endregion
    }
}