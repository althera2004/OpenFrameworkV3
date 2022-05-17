// --------------------------------
// <copyright file="DocumentsSign.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Feature
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
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Reports;

    public class DocumentsSign
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long ItemDefinitionId { get; set; }
        public long ItemId { get; set; }
        public string FieldName { get; set; }
        public int Count { get; set; }

        public static DocumentsSign Empty
        {
            get
            {
                return new DocumentsSign
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    ItemDefinitionId = Constant.DefaultId,
                    ItemId = Constant.DefaultId,
                    FieldName = string.Empty,
                    Count = 0
                };
            }
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""CompanyId"":{1},""ItemDefinitionId"":{2},""ItemId"":{3},""FieldName"":""{4}"",""Count"":{5}}}",
                    this.Id,
                    this.CompanyId,
                    this.ItemDefinitionId,
                    this.ItemId,
                    Tools.Json.JsonCompliant(this.FieldName),
                    this.Count);
            }
        }

        public static string JsonList(ReadOnlyCollection<DocumentsSign> list)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach(var item in list)
            {
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "{0}{1}",
                    first ? string.Empty : ",",
                    item.Json);

                first = false;
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<DocumentsSign> ByItemId(long companyId, long itemDefintionId, long itemId, string instanceName)
        {
            /* CREATE PROCEDURE Core_DocumentSigntaures_ByItemId
             *   @CompanyId bigint,
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint */
            var res = new List<DocumentsSign>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_DocumentSigntaures_ByItemId"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefintionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new DocumentsSign
                                    {
                                        Id = rdr.GetInt64(0),
                                        CompanyId = rdr.GetInt64(1),
                                        ItemDefinitionId = rdr.GetInt64(2),
                                        ItemId = rdr.GetInt64(3),
                                        FieldName = rdr.GetString(4),
                                        Count = rdr.GetInt32(5)
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

            return new ReadOnlyCollection<DocumentsSign>(res);
        }

        public static DocumentsSign ByFieldName(long companyId, long itemDefintionId, long itemId, string fieldName, string instanceName)
        {
            /* CREATE PROCEDURE Core_DocumentSigntaures_ByFieldName
             *   @CompanyId bigint,
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @FieldName nvarchar(50) */
            var res = DocumentsSign.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_DocumentSigntaures_ByFieldName"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefintionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        cmd.Parameters.Add(DataParameter.Input("@FieldName", fieldName, 50));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    res.Id = rdr.GetInt64(0);
                                    res.CompanyId = rdr.GetInt64(1);
                                    res.ItemDefinitionId = rdr.GetInt64(2);
                                    res.ItemId = rdr.GetInt64(3);
                                    res.FieldName = rdr.GetString(4);
                                    res.Count = rdr.GetInt32(5);
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
            /* CREATE PROCEDURE Core_DocumentSigntaures_Insert
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @FieldName nvarchar(50) */
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_DocumentSigntaures_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@FieldName", this.FieldName, 50));
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

        public static ActionResult Delete(long itemDefinitionId, string fieldName, long itemId, long companyId, string instanceName)
        {
            /* CREATE PROCEDURE Core_DocumentSigntaures_Delete
             *   @CompanyId bigint,
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @FieldName nvarchar(50) */
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_DocumentSigntaures_Delete"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        cmd.Parameters.Add(DataParameter.Input("@FieldName", fieldName, 50));
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

        public ActionResult Reset(long applicationUserId, string instanceName)
        {
            /* CREATE PROCEDURE Core_DocumentSigntaures_Reset
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @FieldName nvarchar(50) */
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_DocumentSigntaures_Reset"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@FieldName", this.FieldName, 50));
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

        public ActionResult Add(long applicationUserId, string instanceName)
        {
            /* CREATE PROCEDURE Core_DocumentSigntaures_Add
             * @Id bigint output,
             * @CompanyId bigint,
             * @ItemDefinitionId bigint,
             * @ItemId bigint,
             * @FieldName nvarchar(50) */
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_DocumentSigntaures_Add"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@FieldName", this.FieldName, 50));
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

        public static string FieldsByForm(string itemName, string formId, long itemId, long companyId, string instanceName)
        {
            var res = new StringBuilder("[");
            var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            var form = itemDefinition.Forms.First(f => f.Id.Equals(formId, StringComparison.OrdinalIgnoreCase));
            var singStatus = ByItemId(companyId, itemDefinition.Id, itemId, instanceName);
            var templates = PdfReportTemplate.All(instanceName);
            var first = true;
            foreach(var field in form.FieldsDocumentSign)
            {
                var status = DocumentsSign.Empty;
                var template = new List<PdfReportTemplate>();
                if(singStatus.Any(s=>s.FieldName.Equals(field.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    status = singStatus.First(s => s.FieldName.Equals(field.Name, StringComparison.OrdinalIgnoreCase));
                }

                if (templates.Any(t => t.Destination != null && string.IsNullOrEmpty(t.Destination.ItemField) == false && t.Destination.ItemName.Equals(itemName,StringComparison.OrdinalIgnoreCase) && t.Destination.ItemField.Equals(field.Name,StringComparison.OrdinalIgnoreCase)))
                {
                    template = templates.Where(
                        t => t.Destination != null &&
                        string.IsNullOrEmpty(t.Destination.ItemField) == false &&
                        t.Destination.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase) &&
                        t.Destination.ItemField.Equals(field.Name, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"{0}{{""FieldName"": ""{1}"", ""Count"":{2}, ""Signatures"":[",
                    first ? string.Empty : ", ",
                    field.Name,
                    status.Count);

                var firstTemplate = true;
                foreach (var tpx in template)
                {
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"{0}{{""Template"":""{1}"",""Signatures"":[",
                        firstTemplate ? string.Empty : ", ",
                        tpx.Name);

                    var firstSignature = true;
                    foreach (var signature in tpx.Signatures)
                    {
                        res.AppendFormat(
                            CultureInfo.InvariantCulture,
                            @"{0}{{""Instructions"":""{1}"", ""Reason"":""{2}""}}",
                            firstSignature ? string.Empty : ", ",
                            Tools.Json.JsonCompliant(signature.Instructions),
                            Tools.Json.JsonCompliant(signature.Reason));
                        firstSignature = false;
                    }

                    res.Append("]}");
                    firstTemplate = false;
                }

                res.Append("]}");
                first = false;
            }


            res.Append("]");
            return res.ToString();
        }
    }
}