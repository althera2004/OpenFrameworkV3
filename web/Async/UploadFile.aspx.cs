// --------------------------------
// <copyright file="UploadFile.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFramework.Web.Async
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Web.UI;
    using OpenFrameworkV3;
    using OpenFrameworkV3.Core;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;
    using OpenFrameworkV3.Feature;
    using OpenFrameworkV3.Tools;

    /// <summary>Implements upload action form attach files</summary>
    public partial class UploadFile : Page
    {
        /// <summary>Page's load event</summary>
        /// <param name="sender">Loaded page</param>
        /// <param name="e">Event's arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var debug = string.Empty;
            var res = ActionResult.NoAction;
            var cns = string.Empty;
            
            var file = this.Request.Files[0];

            string itemName = this.Request.Form["itemName"];
            string fieldName = this.Request.Form["fieldName"];
            var instanceName = this.Request.Form["instanceName"];            

            long itemId = Convert.ToInt64(this.Request.Form["itemId"]);
            long companyId = Convert.ToInt64(this.Request.Form["companyId"]);
            string description = this.Request.Form["description"];
            long applicationUserId = Convert.ToInt64(this.Request.Form["applicationUserId"]);

            bool signature = Convert.ToBoolean(this.Request.Form["signature"]);
            bool historial = Convert.ToBoolean(this.Request.Form["historial"]);
            bool gallery = Convert.ToBoolean(this.Request.Form["gallery"]);

            string mode = this.Request.Form["mode"];

            var path = this.Request.PhysicalApplicationPath;
            if (!path.EndsWith(@"\"))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\\", path);
            }

            string fileName = string.Empty;
            if (mode == "field")
            {
                debug += "|field";
                var folder = string.Format(CultureInfo.InvariantCulture, @"{0}Instances\{1}\Data", path, instanceName);
                Basics.VerifyFolder(folder);

                folder = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", folder, itemName);
                Basics.VerifyFolder(folder);

                folder = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", folder, itemId);
                Basics.VerifyFolder(folder);

                // si el documento es de un campo, el nombre es el del campo
                fileName = string.Format(
                    CultureInfo.InvariantCulture,
                    @"{0}\{1}{2}",
                    folder,
                    fieldName,
                    Path.GetExtension(description));

                // Si es un documento firmado, se eliminan los pendientes de firmar y se añade ".signed" al nombre
                if (signature)
                {
                    debug += "|signature";
                    var itemDefinition = Persistence.ItemDefinitionByName(itemName, instanceName);
                    ////DocumentsSign.Delete(itemDefinition.Id, fieldName, itemId, companyId);

                    fileName = string.Format(
                    CultureInfo.InvariantCulture,
                    @"{0}\{1}.signed{2}",
                    folder,
                    fieldName,
                    Path.GetExtension(description));
                    ////ActionLog.Trace("Upload documento firmado ==> " + fieldName, instanceName, itemDefinition.ItemName, itemId);
                }

                if (gallery)
                {
                    debug += "|gallery";
                    var itemDefinition = Persistence.ItemDefinitionByName(itemName, instanceName);

                    // En caso de galeria se mantiene el nombre original en el path de historicos
                    folder = string.Format(CultureInfo.InvariantCulture, @"{0}Instances\{1}\Data\History", path, instanceName);
                    Basics.VerifyFolder(folder);

                    folder = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", folder, itemDefinition.ItemName);
                    Basics.VerifyFolder(folder);

                    folder = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", folder, itemId);
                    Basics.VerifyFolder(folder);

                    var finalFileName = Path.GetFileNameWithoutExtension(file.FileName);

                    finalFileName = finalFileName.Replace("+", "_");
                    finalFileName = finalFileName.Replace("-", "_");
                    finalFileName = finalFileName.Replace("/", "_");
                    finalFileName = finalFileName.Replace("&", string.Empty);
                    finalFileName = finalFileName.Replace("  ", " ");

                    fileName = string.Format(
                     CultureInfo.InvariantCulture,
                     @"{0}\{1}{2}{3}",
                     folder,
                     finalFileName,
                     signature ? ".signed" : string.Empty,
                     Path.GetExtension(file.FileName));


                    var count = 1;
                    while (File.Exists(fileName))
                    {
                        fileName = string.Format(
                            CultureInfo.InvariantCulture,
                            @"{0}\{1}{2}({4}){3}",
                            folder,
                            finalFileName,
                            signature ? ".signed" : string.Empty,
                            Path.GetExtension(file.FileName),
                            count);
                        count++;
                    }

                    debug += "-->" + fileName;

                    file.SaveAs(fileName);

                    
                    using (var cmd = new SqlCommand("Core_DocumentHistory_Insert"))
                    {
                        using (var cnn = new SqlConnection(cns))
                        {

                            cmd.Connection = cnn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                            cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                            cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinition.Id));
                            cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                            cmd.Parameters.Add(DataParameter.Input("@FieldName", fieldName, 50));
                            cmd.Parameters.Add(DataParameter.Input("@FileName", Path.GetFileName(fileName), 100));
                            cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                            try
                            {
                                cmd.Connection.Open();
                                cmd.ExecuteNonQuery();
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

                    var query = string.Format(
                                CultureInfo.InvariantCulture,
                                "UPDATE Item_{0} SET {1} = '{2}', ModifiedBy = {4}, ModifiedOn = GETDATE() WHERE Id = {3}",
                                itemName,
                                fieldName,
                                Path.GetFileName(description),
                                itemId,
                                applicationUserId);
                    var res1 = new ExecuteQuery { QueryText = query, ConnectionString = cns }.ExecuteCommand;
                    ////ActionLog.Trace("Upload documento geleria ==> " + fieldName, instanceName, itemName, itemId);
                    //var documentHistory = new DocumentHistory
                    //{
                    //    CompanyId = companyId,
                    //    ItemDefinitionId = itemDefinition.Id,
                    //    ItemId = itemId,
                    //    FieldName = fieldName,
                    //    Undated = true,
                    //    FileName = description
                    //}.Insert(applicationUserId);
                }
                else if (historial)
                {
                    debug += "|historial";
                    var itemDefinition = Persistence.ItemDefinitionByName(itemName, instanceName);
                    var query = string.Format(
                                CultureInfo.InvariantCulture,
                                "UPDATE Item_{0} SET {1} = '{2}', ModifiedBy = {4}, ModifiedOn = GETDATE() WHERE Id = {3}",
                                itemName,
                                fieldName,
                                Path.GetFileName(fileName),
                                itemId,
                                applicationUserId);
                    var documentHistory = new DocumentHistory
                    {
                        CompanyId = companyId,
                        ItemDefinitionId = itemDefinition.Id,
                        ItemId = itemId,
                        FieldName = fieldName,
                        Undated = true,
                        FileName = fileName
                    }.InsertField(applicationUserId, instanceName);

                    // Insert(itemDefinition.Id, fieldName, itemId, applicationUserId, companyId);

                    var res1 = new ExecuteQuery { QueryText = query, ConnectionString = cns }.ExecuteCommand;
                }
                else
                {
                    debug += "|No gallery";
                    res = Save.UpdateDocumentWithoutPrevious(itemId, itemName, fieldName, Path.GetFileName(fileName), applicationUserId, instanceName);
                }

                // el fichero se guarda después de la copia del anterior al historial si no es galeria
                if (!gallery)
                {
                    file.SaveAs(fileName);
                }

                var trace = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{5},{{{5}{4}{4}""user"":""{0}"",{5}{4}{4}""date"": ""{1:dd/MM/yyyy hh:mm:ss}"",{5}{4}{4}""changes"":
                        [{{
                            ""Field"": ""{2}"",
                            ""Original"": ""Subir documento"",
                            ""Actual"": ""{3}""
                        }}]

                        }}",
                        ApplicationUser.ById(applicationUserId, instanceName).Profile.FullName,
                        DateTime.UtcNow,
                        fieldName,
                        Path.GetFileName(fileName),
                        '\t',
                        '\n');


                ActionLog.TraceItemData(instanceName, itemName, itemId, trace);

                res.SetSuccess(Path.GetFileName(fileName));
            }

            if (mode == "attach")
            {
                var folder = string.Format(CultureInfo.InvariantCulture, @"{0}Instances\{1}\Data\DocumentsGallery", path, instanceName);
                Basics.VerifyFolder(folder);

                folder = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", folder, itemName);
                Basics.VerifyFolder(folder);

                folder = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", folder, itemId);
                Basics.VerifyFolder(folder);

                fileName = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", folder, Path.GetFileName(description));
                file.SaveAs(fileName);
                ////ActionLog.Trace("Upload documento adjunto: " + Path.GetFileName(fileName), instanceName, itemName, itemId);
                res.SetSuccess();
            }

            if (res.Success)
            {
                res.ReturnValue = Path.GetFileName(fileName);// + debug;
            }

            this.Response.Clear();
            this.Response.ContentType = "application/json";
            this.Response.Write("\"" + (res.Success ? res.ReturnValue : res.MessageError) + "\"");
            this.Response.Flush();
        }

        public ActionResult Insert(long itemDefinitionId, string fieldName, long itemId, long applicationUserId, long companyId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_DocumentHistory_Insert
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @FieldName nvarchar(50),
             *   @ApplicationUserId bigint */
            var cns = string.Empty;

            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Core_DocumentHistory_Insert"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        var itemDefinition = Persistence.ItemDefinitionById(itemId, instanceName);
                        var dataOriginal = Read.GetFieldValue<string>(itemDefinition.ItemName, fieldName, itemId, instanceName);

                        if(!string.IsNullOrEmpty(dataOriginal))
                        {

                            // Path del fichero historico
                            var pathHistory = string.Format(
                                CultureInfo.InvariantCulture,
                                @"{0}History",
                                Instance.Path.Data(instanceName));

                            Basics.VerifyFolder(pathHistory);
                            pathHistory = string.Format(CultureInfo.InvariantCulture, @"{0}\\{1}", pathHistory, itemDefinition.ItemName);
                            Basics.VerifyFolder(pathHistory);
                            pathHistory = string.Format(CultureInfo.InvariantCulture, @"{0}\\{1}", pathHistory, itemId);
                            Basics.VerifyFolder(pathHistory);

                            var targetFile = string.Format(
                                "{0}_{1:yyyyMMdd_hhmmss}{2}",
                                Path.GetFileNameWithoutExtension(dataOriginal),
                                DateTime.Now,
                                Path.GetExtension(dataOriginal));

                            cmd.Connection = cnn;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                            cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                            cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinitionId));
                            cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                            cmd.Parameters.Add(DataParameter.Input("@FieldName", fieldName, 50));
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
                                    itemId,
                                    dataOriginal);

                                pathHistory = string.Format(CultureInfo.InvariantCulture, @"{0}\\{1}", pathHistory, targetFile);
                                pathHistory = pathHistory.Replace("\\\\", "\\");
                                pathOriginal = pathOriginal.Replace("\\\\", "\\");

                                // Copiar fichero
                                if(File.Exists(pathOriginal))
                                {
                                    cmd.Connection.Open();
                                    cmd.ExecuteNonQuery();
                                    long resultId = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                                    res.SetSuccess(resultId);
                                    File.Move(pathOriginal, pathHistory);
                                }
                            }
                            catch(Exception ex)
                            {
                                ExceptionManager.Trace(ex, "DocumentHistory-->Insert");
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
            }

            return res;
        }
    }
}