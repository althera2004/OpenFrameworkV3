// --------------------------------
// <copyright file="DocumentHistory.cs" company="OpenFramework">
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
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using OpenFrameworkV3.Core;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;
    using OpenFrameworkV3.Tools;

    public class DocumentHistory
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long ItemDefinitionId { get; set; }
        public long ItemId { get; set; }
        public string FieldName { get; set; }
        public string FileName { get; set; }
        public ApplicationUser OriginalCreatedBy { get; set; }
        public ApplicationUser OriginalModifiedBy { get; set; }
        public DateTime OriginalCreatedOn { get; set; }
        public DateTime OriginalModiedOn { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }
        public bool Undated { get; set; }

        public static DocumentHistory Empty
        {
            get
            {
                return new DocumentHistory
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    ItemDefinitionId = Constant.DefaultId,
                    ItemId = Constant.DefaultId,
                    FieldName = string.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    ModifiedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Active = true
                };
            }
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"
                    {{
                        ""Id"": {0},
                        ""CompanyId"": {1},
                        ""ItemDefinitionId"": {2},
                        ""ItemId"": {3},
                        ""FieldName"": ""{4}"",
                        ""FileName"": ""{5}"",
                        ""CreatedBy"": {6},
                        ""CreatedOn"": ""{7:dd/MM/yyyy}"",
                        ""Active"": {8}
                    }}",
                    this.Id,
                    this.CompanyId,
                    this.ItemDefinitionId,
                    this.ItemId,
                    this.FieldName,
                    this.FileName,
                    this.CreatedBy.JsonSimple,
                    this.CreatedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static string JsonList(ReadOnlyCollection<DocumentHistory> list)
        {
            var res = new StringBuilder("[");
            var first = true;
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

        public ActionResult Insert(long applicationUserId, string instanceName)
        {
            return Insert(false, applicationUserId, instanceName);
        }

        public ActionResult Insert(bool gallery, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_DocumentHistory_Insert
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @FieldName nvarchar(50),
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_DocumentHistory_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.Id == ItemDefinitionId);
                        var dataOriginal = Read.GetFieldValue<string>(itemDefinition.ItemName, this.FieldName, this.ItemId, instanceName);
                        if (!string.IsNullOrEmpty(dataOriginal) || gallery)
                        {

                            // Path del fichero historico
                            var pathHistory = string.Format(
                                CultureInfo.InvariantCulture,
                                @"{0}{1}History",
                                Instance.Path.Data(instanceName),
                                Instance.Path.Data(instanceName).EndsWith("\\", StringComparison.OrdinalIgnoreCase) ? string.Empty : "\\");

                            Basics.VerifyFolder(pathHistory);
                            pathHistory = string.Format(CultureInfo.InvariantCulture, @"{0}\\{1}", pathHistory, itemDefinition.ItemName);
                            Basics.VerifyFolder(pathHistory);
                            pathHistory = string.Format(CultureInfo.InvariantCulture, @"{0}\\{1}", pathHistory, ItemId);
                            Basics.VerifyFolder(pathHistory);

                            var dated = string.Empty;
                            if (!gallery)
                            {
                                dated = string.Format(CultureInfo.InvariantCulture, "{0:yyyyMMdd_hhmmss}", DateTime.Now.ToUniversalTime());
                            }

                            var targetFile = string.Format(
                                "{0}{3}{1:yyyyMMdd_hhmmss}{2}",
                                Path.GetFileNameWithoutExtension(gallery ? this.FileName : dataOriginal),
                                dated,
                                Path.GetExtension(gallery ? this.FileName : dataOriginal),
                                gallery ? string.Empty : "_");

                            if (this.Undated && !gallery)
                            {
                                targetFile = string.Format(
                                "{0}{1}",
                                Path.GetFileNameWithoutExtension(dataOriginal),
                                Path.GetExtension(dataOriginal));
                            }

                            cmd.Connection = cnn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                            cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                            cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                            cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                            cmd.Parameters.Add(DataParameter.Input("@FieldName", this.FieldName, 50));
                            cmd.Parameters.Add(DataParameter.Input("@FileName", targetFile, 100));
                            cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                            try
                            {

                                // Path del fichero original
                                var pathOriginal = string.Format(
                                    CultureInfo.InvariantCulture,
                                    @"{0}{1}{2}\\{3}\\{4}",
                                    Instance.Path.Data(instanceName),
                                    Instance.Path.Data(instanceName).EndsWith("\\", StringComparison.OrdinalIgnoreCase) ? string.Empty : "\\",
                                    itemDefinition.ItemName,
                                    this.ItemId,
                                    gallery ? this.FileName : dataOriginal);

                                pathHistory = string.Format(CultureInfo.InvariantCulture, @"{0}\\{1}", pathHistory, targetFile);
                                pathHistory = pathHistory.Replace("\\\\", "\\");
                                pathOriginal = pathOriginal.Replace("\\\\", "\\");

                                // Copiar fichero
                                if (File.Exists(pathOriginal))
                                {
                                    cmd.Connection.Open();
                                    cmd.ExecuteNonQuery();
                                    this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                                    res.SetSuccess(this.Id);
                                    File.Move(pathOriginal, pathHistory);
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionManager.Trace(ex, "DocumentHistory-->Insert");
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
            }

            return res;
        }
        
        public ActionResult InsertField(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_DocumentHistory_Insert
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @FieldName nvarchar(50),
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            { 
            using (var cmd = new SqlCommand("Core_DocumentHistory_Insert"))
            {
                using (var cnn = new SqlConnection(cns))
                {
                    var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.Id == this.ItemDefinitionId);
                    var dataOriginal = Read.GetFieldValue<string>(itemDefinition.ItemName, this.FieldName, this.ItemId, instanceName);

                        if (!string.IsNullOrEmpty(dataOriginal))
                        {
                            // Path del fichero historico
                            var pathData = Instance.Path.Data(instanceName);
                            var pathHistory = string.Format(
                                CultureInfo.InvariantCulture,
                                @"{0}{1}History",
                                pathData,
                                pathData.EndsWith("\\", StringComparison.OrdinalIgnoreCase) ? string.Empty : "\\");

                            Basics.VerifyFolder(pathHistory);
                            pathHistory = string.Format(CultureInfo.InvariantCulture, @"{0}\\{1}", pathHistory, itemDefinition.ItemName);
                            Basics.VerifyFolder(pathHistory);
                            pathHistory = string.Format(CultureInfo.InvariantCulture, @"{0}\\{1}", pathHistory, this.ItemId);
                            Basics.VerifyFolder(pathHistory);

                            var targetFile = string.Format(
                                "{0}_{1:yyyyMMdd_hhmmss}{2}",
                                Path.GetFileNameWithoutExtension(dataOriginal),
                                DateTime.Now,
                                Path.GetExtension(dataOriginal));

                            cmd.Connection = cnn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                            cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                            cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                            cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                            cmd.Parameters.Add(DataParameter.Input("@FieldName", this.FieldName, 50));
                            cmd.Parameters.Add(DataParameter.Input("@FileName", targetFile, 100));
                            cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                            try
                            {
                                // Path del fichero original
                                var pathOriginal = string.Format(
                                    CultureInfo.InvariantCulture,
                                    @"{0}{1}{2}\\{3}\\{4}",
                                    pathData,
                                    pathData.EndsWith("\\", StringComparison.OrdinalIgnoreCase) ? string.Empty : "\\",
                                    itemDefinition.ItemName,
                                    this.ItemId,
                                    dataOriginal);

                                pathHistory = string.Format(CultureInfo.InvariantCulture, @"{0}\\{1}", pathHistory, targetFile);
                                pathHistory = pathHistory.Replace("\\\\", "\\");
                                pathOriginal = pathOriginal.Replace("\\\\", "\\");

                                // Copiar fichero
                                if (File.Exists(pathOriginal))
                                {
                                    cmd.Connection.Open();
                                    cmd.ExecuteNonQuery();
                                    long resultId = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                                    res.SetSuccess(resultId);
                                    File.Move(pathOriginal, pathHistory);
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionManager.Trace(ex, "DocumentHistory-->Insert");
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
            }

            return res;
        }

        public ActionResult Activate(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_DocumentHistory_Activate 
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_DocumentHistory_Activate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
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
                            ExceptionManager.Trace(ex, "DocumentHistory-->Activate");
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

        public ActionResult Inactivate(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_DocumentHistory_Inactivate 
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns)) {
                using (var cmd = new SqlCommand("Core_DocumentHistory_Inactivate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(this.Id);

                            if (Persistence.ItemDefinitions(instanceName).Any(d => d.Id == this.ItemDefinitionId))
                            {
                                var definition = Persistence.ItemDefinitions(instanceName).First(d => d.Id == this.ItemDefinitionId);
                                ActionLog.Trace("DELETE DOCUMENT HISTORY", string.Format(CultureInfo.InvariantCulture, "Item:{0}({1}),{2}", definition.ItemName, this.ItemId, this.Id), instanceName, ApplicationUser.Actual);
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.Trace(ex, "DocumentHistory-->Inactivate");
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

        public static ReadOnlyCollection<DocumentHistory> ByItemField (long itemDefinitionId, long itemId, string fieldName,long companyId, string instanceName)
        {
            var res = new List<DocumentHistory>();
            /* CREATE PROCEDURE Core_DocumentHistory_ByItemId
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @FieldName nvarchar(50),
             *   @CompanydId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_DocumentHistory_ByItemId"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        cmd.Parameters.Add(DataParameter.Input("@FieldName", fieldName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new DocumentHistory
                                    {
                                        Id = rdr.GetInt64(ColumnsDocumentHistoryGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsDocumentHistoryGet.CompanyId),
                                        ItemDefinitionId = rdr.GetInt64(ColumnsDocumentHistoryGet.ItemDefinitionId),
                                        ItemId = rdr.GetInt64(ColumnsDocumentHistoryGet.ItemId),
                                        FieldName = rdr.GetString(ColumnsDocumentHistoryGet.FieldName),
                                        FileName = rdr.GetString(ColumnsDocumentHistoryGet.FileName),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsDocumentHistoryGet.CreatedBy),
                                            Profile = new UserProfile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsDocumentHistoryGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsDocumentHistoryGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsDocumentHistoryGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsDocumentHistoryGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsDocumentHistoryGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsDocumentHistoryGet.ModifiedBy),
                                            Profile = new UserProfile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsDocumentHistoryGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsDocumentHistoryGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsDocumentHistoryGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsDocumentHistoryGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsDocumentHistoryGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsDocumentHistoryGet.Active)
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

            return new ReadOnlyCollection<DocumentHistory>(res);
        }
    }
}