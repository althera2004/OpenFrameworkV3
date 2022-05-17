// --------------------------------
// <copyright file="GenerateFromTemplate.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Export
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using OpenFrameworkV3;
    using OpenFrameworkV3.Core;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Companies;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Feature;
    using OpenFrameworkV3.Core.Security;
    using OpenFrameworkV3.Reports;
    using OpenFrameworkV3.Tools;

    /// <summary>Generation of pdf document based on template</summary>
    public class GenerateFromTemplate
    {
        public static string Go(string templateName, ReadOnlyDictionary<string, string> parameters, long companyId, string instanceName)
        {
            var template = PdfReportTemplate.Load(templateName, instanceName);

            if(template != null)
            {
                if(template.Template.Equals("flow", StringComparison.OrdinalIgnoreCase))
                {
                    return GoFlow(template, parameters, companyId, instanceName);
                }
                else
                {
                    return GoPdfEditable(template, parameters, companyId, instanceName);
                }
            }

            return "NoTemplate";

        }
        public static string GoFlow(PdfReportTemplate template, ReadOnlyDictionary<string, string> parameters,long companyId, string instanceName)
        {
            var user = ApplicationUser.Actual;
            var company = Company.ById(companyId, instanceName);
            var data = new Dictionary<int, string>();
            long itemId = Constant.DefaultId;
            itemId = Convert.ToInt64(parameters[template.Destination.ItemIdParameter]);
            long signatureId = -1;

            int fontSizeFooter = 8;

            // Si se genera un pdf firmable se descartar el pendiente de firmar
            // ----------------------------------------------------------------------
            if(template.Signatures != null)
            {
                if(template.Signatures.Count > 0)
                {
                    var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(template.Destination.ItemName, StringComparison.OrdinalIgnoreCase));
                    DocumentsSign.Delete(itemDefinition.Id, template.Destination.ItemField, itemId, companyId, instanceName);
                }
            }
            // ----------------------------------------------------------------------

            // Se hace la llamada al stored para recoger los datos


            var signerUser = ApplicationUser.Empty;
            var signerUserApply = false;
            if(template.Fields.Any(tf => tf.DataType.StartsWith("Signer", StringComparison.OrdinalIgnoreCase)))
            {
                signerUserApply = true;
            }

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand(template.StoredProcedure))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (var parameter in template.StoredParameters)
                        {
                            switch (parameter.DataType)
                            {
                                case "long":
                                    long valueLong = 0;
                                    if (parameter.Value.Equals("FromQuery", StringComparison.OrdinalIgnoreCase))
                                    {
                                        valueLong = Convert.ToInt64(parameters[parameter.Name]);
                                    }
                                    else if (parameter.Value.Equals("FromSession", StringComparison.OrdinalIgnoreCase))
                                    {
                                        valueLong = Convert.ToInt64(HttpContext.Current.Session[parameter.Name]);
                                    }
                                    else
                                    {
                                        valueLong = Convert.ToInt64(parameter.Value);
                                    }

                                    cmd.Parameters.Add(DataParameter.Input(parameter.Name, valueLong));
                                    break;
                                case "int":
                                    int valueInt = 0;
                                    if (parameter.Value.Equals("FromQuery", StringComparison.OrdinalIgnoreCase))
                                    {
                                        valueInt = Convert.ToInt32(parameters[parameter.Name]);
                                    }
                                    else if (parameter.Value.Equals("FromSession", StringComparison.OrdinalIgnoreCase))
                                    {
                                        valueInt = Convert.ToInt32(HttpContext.Current.Session[parameter.Name]);
                                    }
                                    else
                                    {
                                        valueInt = Convert.ToInt32(parameter.Value);
                                    }

                                    cmd.Parameters.Add(DataParameter.Input(parameter.Name, valueInt));
                                    break;
                                case "text":
                                    string textValue = string.Empty;
                                    if (parameter.Value.Equals("FromQuery", StringComparison.OrdinalIgnoreCase))
                                    {
                                        textValue = parameters[parameter.Name] as string;
                                    }
                                    else if (parameter.Value.Equals("FromSession", StringComparison.OrdinalIgnoreCase))
                                    {
                                        textValue = HttpContext.Current.Session[parameter.Name] as string;
                                    }
                                    else
                                    {
                                        textValue = parameter.Value;
                                    }

                                    cmd.Parameters.Add(DataParameter.Input(parameter.Name, textValue));
                                    break;
                                default:
                                    cmd.Parameters.Add(DataParameter.Input(parameter.Name, parameter.Value));
                                    break;
                            }
                        }

                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                rdr.Read();
                                for (var d = 0; d < rdr.FieldCount; d++)
                                {
                                    data.Add(d, rdr[d].ToString());
                                }

                                if (signerUserApply)
                                {
                                    var signerUserIdText = rdr["SignerUser"].ToString();
                                    if (!string.IsNullOrEmpty(signerUserIdText))
                                    {
                                        var signerUserId = Convert.ToInt64(signerUserIdText);
                                        signerUser = ApplicationUser.ById(signerUserId, instanceName);
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

            var fileNameNew = string.Empty;

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
                if(!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                {
                    path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
                }
               
                // Este el el nombre del documento generado
                var targetFolder = ConfigurationManager.AppSettings["ExportFolder"] as string;
                if(template.Destination != null)
                {
                    if(!string.IsNullOrEmpty(template.Destination.ItemName))
                    {
                        targetFolder = string.Format(CultureInfo.InvariantCulture, "{0}Instances\\{1}\\Data\\{2}", HttpContext.Current.Request.PhysicalApplicationPath, instanceName, template.Destination.ItemName);
                        Basics.VerifyFolder(targetFolder);
                        targetFolder = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", targetFolder, parameters[template.Destination.ItemIdParameter]);
                        Basics.VerifyFolder(targetFolder);
                    }
                }

                if(template.Signatures.Count > 0)
                {
                    targetFolder = string.Format(CultureInfo.InvariantCulture, "{0}\\SignaturesTemporal", targetFolder);
                    Basics.VerifyFolder(targetFolder);
                }

                // Este el el path final dónde se genera el pdf
                fileNameNew = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", targetFolder, template.ResultFile);

                if(template.Destination != null && string.IsNullOrEmpty(template.Destination.ItemField))
                {
                    targetFolder = string.Format(CultureInfo.InvariantCulture, "{0}Instances\\{1}\\Data\\DocumentsGallery\\{2}", HttpContext.Current.Request.PhysicalApplicationPath, instanceName, template.Destination.ItemName);
                    Basics.VerifyFolder(targetFolder);
                    targetFolder = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", targetFolder, itemId);
                    Basics.VerifyFolder(targetFolder);
                    fileNameNew = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}_{2:yyyy-MM-dd-hhmmss}{3}", targetFolder, Path.GetFileNameWithoutExtension(template.ResultFile), DateTime.Now, Path.GetExtension(template.ResultFile));
                }

                // Si es un fichero de campo pero no es firmable
                var existsPrevious = false;
                var ficheroCopiaantiMachaca = fileNameNew.Replace(".pdf", ".copia.pdf");
                if(template.Destination != null && !string.IsNullOrEmpty(template.Destination.ItemField) && template.Signatures.Count == 0)
                {
                    if(File.Exists(fileNameNew))
                    {
                        existsPrevious = true;
                        if(File.Exists(ficheroCopiaantiMachaca))
                        {
                            File.Delete(ficheroCopiaantiMachaca);
                        }

                        File.Copy(fileNameNew, ficheroCopiaantiMachaca);
                    }
                }

                using(var pdfDoc = new Document(PageSize.A4, 40, 40, 80, 40))
                {
                    var writer = PdfWriter.GetInstance(pdfDoc, new FileStream(fileNameNew, FileMode.Create));
                    Pdf.SetMetadata(pdfDoc, "Informe fisio", "OpenFramework - " + instanceName);
                    Pdf.SuperCompression(writer);

                    pdfDoc.Open();

                    foreach(var block in template.Fields)
                    {
                        if(!string.IsNullOrEmpty(block.DataType))
                        {
                            if(block.DataType.Equals("CompanyLocationAndLargeDate", StringComparison.OrdinalIgnoreCase))
                            {
                                var fechaDocumento = DateTime.Now;
                                string mes = MonthName(fechaDocumento.Month, company.DefaultLanguage.Iso);
                                string fechaLarga = string.Format(CultureInfo.InvariantCulture, "{0} a {1} de {2} de {3}", company.MainAddress.City, fechaDocumento.Day, mes, fechaDocumento.Year);
                                pdfDoc.Add(new Paragraph(new Chunk(fechaLarga, new Font(baseFont, 10))));
                            }
                            else if(block.DataType.Equals("Literal", StringComparison.OrdinalIgnoreCase))
                            {
                                var finalText = block.Text.Replace("**", "^").Split('^');
                                var paragraph = new Paragraph();

                                paragraph.Add(new Chunk(finalText[0], new Font(baseFont, 10)));

                                var first = true;
                                foreach(var part in finalText)
                                {
                                    if(!first)
                                    {
                                        paragraph.Add(new Chunk("\n", new Font(baseFont, 10)));
                                        paragraph.Add(new Chunk("a", new Font(baseFontCheckBox, 10)));
                                        paragraph.Add(new Chunk(part, new Font(baseFont, 10)));
                                    }

                                    first = false;
                                }

                                pdfDoc.Add(paragraph);

                            }
                        }
                        else
                        {
                            pdfDoc.Add(new Phrase(block.FieldName));
                        }
                    }

                    pdfDoc.CloseDocument();
                }


                var itemDefinitionId = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(template.Destination.ItemName, StringComparison.OrdinalIgnoreCase)).Id;

                if(template.Destination != null && !string.IsNullOrEmpty(template.Destination.ItemField))
                {
                    if(template.Signatures.Count > 0)
                    {
                        var itemDefinition = Persistence.ItemDefinitions(instanceName).First(i => i.ItemName.Equals(template.Destination.ItemName, StringComparison.OrdinalIgnoreCase));
                        var signStatus = DocumentsSign.ByFieldName(companyId, itemDefinition.Id, itemId, template.Destination.ItemField, instanceName);
                        var documentSingRes = new DocumentsSign
                        {
                            Id = Constant.DefaultId,
                            CompanyId = companyId,
                            ItemDefinitionId = itemDefinitionId,
                            ItemId = itemId,
                            FieldName = template.Destination.ItemField
                        }.Insert(user.Id, instanceName);

                        if(documentSingRes.Success)
                        {
                            signatureId = Convert.ToInt64(documentSingRes.ReturnValue);
                        }

                        ActionLog.Trace(template.Destination.ItemField + ": Document To Sign", instanceName, itemDefinition.ItemName, itemId, companyId);
                    }
                    else
                    {
                        if(existsPrevious)
                        {
                            Save.UpdateDocument(itemId, template.Destination.ItemName, template.Destination.ItemField, Path.GetFileName(ficheroCopiaantiMachaca), user.Id, companyId, instanceName);
                            ActionLog.Trace(template.Destination.ItemField + ": Document created", instanceName, template.Destination.ItemName, itemId, companyId);
                        }
                        else
                        {
                            Save.UpdateDocumentWithoutPrevious(itemId, template.Destination.ItemName, template.Destination.ItemField, Path.GetFileName(fileNameNew), user.Id, instanceName);
                            ActionLog.Trace(template.Destination.ItemField + ": Document created with historial", instanceName, template.Destination.ItemName, itemId, companyId);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.InvariantCulture, @"GenerateFromTemplate({0}, {1})", template.Template, itemId));
            }

            // Determinar si el documento está asociado a algún campo para enviarlo como resultado
            var res = new StringBuilder();
            if(template.Destination != null && !string.IsNullOrEmpty(template.Destination.ItemField))
            {
                res.Append("{\"FileUrl\":\"");
                if(template.Signatures.Count > 0)
                {
                    res.Append("SignaturesTemporal/");
                }
                res.Append(template.ResultFile + "\"");
                res.AppendFormat(CultureInfo.InvariantCulture, @", ""FieldName"":""{0}""", template.Destination.ItemField);
                if(signatureId > 0)
                {
                    res.AppendFormat(CultureInfo.InvariantCulture, @", ""Id"":{0}", signatureId);
                }
                res.Append("}");
            }
            else
            {
                res.Append("{\"FileUrl\":\"/Instances/" + instanceName + "/Data/DocumentsGallery/" + template.Destination.ItemName + "/" + itemId + "/" + Path.GetFileName(fileNameNew) + "\"}");
            }

            return res.ToString();
        }

        public static string GoPdfEditable(PdfReportTemplate template, ReadOnlyDictionary<string, string> parameters, long companyId, string instanceName)
        {
            var instance = Persistence.InstanceByName(instanceName);
            var user = ApplicationUser.Actual;
            var company = Company.ById(companyId, instanceName);
            var data = new Dictionary<int, string>();
            long itemId = Constant.DefaultId;
            itemId = Convert.ToInt64(parameters[template.Destination.ItemIdParameter]);
            long signatureId = -1;

            int fontSizeFooter = 8;

            // Si se genera un pdf firmable se descartar el pendiente de firmar
            // ----------------------------------------------------------------------
            if(template.Signatures != null)
            {
                if(template.Signatures.Count > 0)
                {
                    var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(template.Destination.ItemName, StringComparison.OrdinalIgnoreCase));
                    DocumentsSign.Delete(itemDefinition.Id, template.Destination.ItemField, itemId, company.Id, instanceName);
                }
            }
            // ----------------------------------------------------------------------

            // Se hace la llamada al stored para recoger los datos


            var signerUser = ApplicationUser.Empty;
            var signerUserApply = false;
            if(template.Fields.Any(tf => tf.DataType.StartsWith("Signer", StringComparison.OrdinalIgnoreCase)))
            {
                signerUserApply = true;
            }

            using(var cmd = new SqlCommand(template.StoredProcedure))
            {
                using(var cnn = new SqlConnection(instance.Config.ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach(var parameter in template.StoredParameters)
                    {
                        switch(parameter.DataType)
                        {
                            case "long":
                                long valueLong = 0;
                                if(parameter.Value.Equals("FromQuery", StringComparison.OrdinalIgnoreCase))
                                {
                                    valueLong = Convert.ToInt64(parameters[parameter.Name]);
                                }
                                else if(parameter.Value.Equals("FromSession", StringComparison.OrdinalIgnoreCase))
                                {
                                    valueLong = Convert.ToInt64(HttpContext.Current.Session[parameter.Name]);
                                }
                                else
                                {
                                    valueLong = Convert.ToInt64(parameter.Value);
                                }

                                cmd.Parameters.Add(DataParameter.Input(parameter.Name, valueLong));
                                break;
                            case "int":
                                int valueInt = 0;
                                if(parameter.Value.Equals("FromQuery", StringComparison.OrdinalIgnoreCase))
                                {
                                    valueInt = Convert.ToInt32(parameters[parameter.Name]);
                                }
                                else if(parameter.Value.Equals("FromSession", StringComparison.OrdinalIgnoreCase))
                                {
                                    valueInt = Convert.ToInt32(HttpContext.Current.Session[parameter.Name]);
                                }
                                else
                                {
                                    valueInt = Convert.ToInt32(parameter.Value);
                                }

                                cmd.Parameters.Add(DataParameter.Input(parameter.Name, valueInt));
                                break;
                            case "text":
                                string textValue = string.Empty;
                                if(parameter.Value.Equals("FromQuery", StringComparison.OrdinalIgnoreCase))
                                {
                                    textValue = parameters[parameter.Name] as string;
                                }
                                else if(parameter.Value.Equals("FromSession", StringComparison.OrdinalIgnoreCase))
                                {
                                    textValue = HttpContext.Current.Session[parameter.Name] as string;
                                }
                                else
                                {
                                    textValue = parameter.Value;
                                }

                                cmd.Parameters.Add(DataParameter.Input(parameter.Name, textValue));
                                break;
                            case "datetime":
                                if(parameter.Value.Equals("FromQuery", StringComparison.OrdinalIgnoreCase))
                                {
                                    cmd.Parameters.Add(DataParameter.Input(parameter.Name, DateFormat.FromStringddMMyyy(parameters[parameter.Name])));
                                }
                                else if(parameter.Value.Equals("FromSession", StringComparison.OrdinalIgnoreCase))
                                {
                                    cmd.Parameters.Add(DataParameter.Input(parameter.Name, HttpContext.Current.Session[parameter.Name] as DateTime?));
                                }
                                else
                                {
                                    cmd.Parameters.Add(DataParameter.Input(parameter.Name, DateFormat.FormatDateDDMMYYYY(parameter.Value)));
                                }
                                
                                break;
                            default:
                                cmd.Parameters.Add(DataParameter.Input(parameter.Name, parameter.Value));
                                break;
                        }
                    }

                    try
                    {
                        cmd.Connection.Open();
                        using(var rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            for(var d = 0; d < rdr.FieldCount; d++)
                            {
                                data.Add(d, rdr[d].ToString());
                            }

                            if(signerUserApply)
                            {
                                var signerUserIdText = rdr["SignerUser"].ToString();
                                if(!string.IsNullOrEmpty(signerUserIdText))
                                {
                                    var signerUserId = Convert.ToInt64(signerUserIdText);
                                    signerUser = ApplicationUser.ById(signerUserId, instanceName);
                                }
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

            var fileNameNew = string.Empty;

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
                if(!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                {
                    path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
                }

                var fileNameTemplate = string.Format(CultureInfo.InvariantCulture, @"{0}{2}{1}", Instance.Path.PdfTemplates(instanceName), template.Template, Instance.Path.PdfTemplates(instanceName).EndsWith("\\") ? string.Empty : "\\");

                // Este el el nombre de la plantilla
                var targetFolder = ConfigurationManager.AppSettings["ExportFolder"] as string;
                if(template.Destination != null)
                {
                    if(!string.IsNullOrEmpty(template.Destination.ItemName))
                    {
                        targetFolder = string.Format(CultureInfo.InvariantCulture, "{0}Instances\\{1}\\Data\\{2}", HttpContext.Current.Request.PhysicalApplicationPath, instance.Name, template.Destination.ItemName);
                        Basics.VerifyFolder(targetFolder);
                        targetFolder = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", targetFolder, parameters[template.Destination.ItemIdParameter]);
                        Basics.VerifyFolder(targetFolder);
                    }
                }

                if(template.Signatures.Count > 0)
                {
                    targetFolder = string.Format(CultureInfo.InvariantCulture, "{0}\\SignaturesTemporal", targetFolder);
                    Basics.VerifyFolder(targetFolder);
                }

                // Este el el path final dónde se genera el pdf
                fileNameNew = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", targetFolder, template.ResultFile);

                if(template.Destination != null && string.IsNullOrEmpty(template.Destination.ItemField))
                {
                    targetFolder = string.Format(CultureInfo.InvariantCulture, "{0}Instances\\{1}\\Data\\DocumentsGallery\\{2}", HttpContext.Current.Request.PhysicalApplicationPath, instance.Name, template.Destination.ItemName);
                    Basics.VerifyFolder(targetFolder);
                    targetFolder = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", targetFolder, itemId);
                    Basics.VerifyFolder(targetFolder);
                    fileNameNew = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}_{2:yyyy-MM-dd-hhmmss}{3}", targetFolder, Path.GetFileNameWithoutExtension(template.ResultFile), DateTime.Now, Path.GetExtension(template.ResultFile));
                }

                // Si es un fichero de campo pero no es firmable
                var existsPrevious = false;
                var ficheroCopiaantiMachaca = fileNameNew.Replace(".pdf", ".copia.pdf");
                if(template.Destination != null && !string.IsNullOrEmpty(template.Destination.ItemField) && template.Signatures.Count == 0)
                {
                    if(File.Exists(fileNameNew))
                    {
                        existsPrevious = true;
                        if(File.Exists(ficheroCopiaantiMachaca))
                        {
                            File.Delete(ficheroCopiaantiMachaca);
                        }

                        File.Copy(fileNameNew, ficheroCopiaantiMachaca);
                    }
                }

                // ---------------------------------------
                using(var existingFileStream = new FileStream(fileNameTemplate, FileMode.Open))
                {
                    using(var newFileStream = new FileStream(fileNameNew, FileMode.Create))
                    {
                        var pdfReader = new PdfReader(existingFileStream);
                        var stamper = new PdfStamper(pdfReader, newFileStream);
                        stamper.SetFullCompression();

                        var form = stamper.AcroFields;
                        var fieldKeys = form.Fields.Keys;
                        Pdf.SetMetadata(pdfReader, stamper, template.Title, template.Subject, "OpenFramework - " + instance.Name);

                        // Se recorren todos los campos del pdf y a cada uno se pone el contenido
                        foreach(string fieldKey in fieldKeys)
                        {
                            var filled = false;
                            foreach(var dataField in template.Fields.Where(f => f.FieldName.Equals(fieldKey, StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(f.Value) == true))
                            {
                                if(dataField.FieldName.Equals(fieldKey, StringComparison.OrdinalIgnoreCase))
                                {
                                    if(dataField.DataType.Equals("title", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, data[dataField.Data[0].Index], 14, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("field", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, data[dataField.Data[0].Index], 11, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("checkbox", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, data[dataField.Data[0].Index], 16, baseFontCheckBox);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("composite", StringComparison.OrdinalIgnoreCase))
                                    {
                                        var compositeData = new List<string>();
                                        foreach(var cd in dataField.Data)
                                        {
                                            if(cd.Index > -1)
                                            {
                                                if(cd.DataType.StartsWith("money", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    var moneyValue = Convert.ToDecimal(data.First(d => d.Key == cd.Index).Value.Trim());
                                                    if(cd.DataType.Equals("money", StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        compositeData.Add(Basics.PdfMoneyFormat(moneyValue));
                                                    }
                                                    else if(cd.DataType.Equals("moneytext", StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        compositeData.Add(Basics.NumeroALetras(moneyValue));
                                                    }
                                                }
                                                else
                                                {
                                                    compositeData.Add(data.First(d => d.Key == cd.Index).Value.Trim());
                                                }
                                            }
                                            else
                                            {
                                                var dataValue = cd.DataType;
                                                switch(cd.DataType.ToUpperInvariant())
                                                {
                                                    case "COMPANYNAME": dataValue = company.Name; break;
                                                    case "COMPANYCIF": dataValue = company.Cif; break;
                                                    case "COMPANYMAINPHONE": dataValue = company.ContactPerson.Phone1; break;
                                                    case "COMPANYMAINEMAIL": dataValue = company.ContactPerson.Email1; break;
                                                    case "COMPANYADDRESS": dataValue = company.MainAddress.FullAddress; break;
                                                    case "COMPANYMAINSIGNATURENAME": dataValue = company.ContactPerson.FullName; break;
                                                    case "COMPANYMAINSIGNATURENIF": dataValue = company.ContactPerson.NIF; break;
                                                }

                                                compositeData.Add(dataValue);
                                            }
                                        }

                                        Pdf.ReplaceField(stamper, fieldKey, dataField.Pattern, compositeData.ToArray(), baseFont, baseFontBold, 11);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("SignerUserName", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, signerUser.Profile.FullName, 11, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("SignerDataText1", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, signerUser.Profile.DataText1, 11, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("SignerDataText2", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, signerUser.Profile.DataText2, 11, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("UserName", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, user.Profile.FullName, 11, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("UserNameMainPhone", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, user.Profile.Phone, 11, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("UserName", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, user.Profile.FullName, 11, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("UserNameMainPhone", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, user.Profile.Phone, 11, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("UserByGroup", StringComparison.OrdinalIgnoreCase))
                                    {
                                        var userByGroup = ApplicationUser.ChoseByGroup(dataField.GroupId, company.Id, instanceName);
                                        var value = string.Empty;

                                        switch(dataField.Value.ToUpperInvariant())
                                        {
                                            case "NAME":
                                                value = userByGroup.Profile.FullName;
                                                break;
                                            case "PHONE":
                                                value = userByGroup.Profile.Phone;
                                                break;
                                            case "DATATEXT1":
                                                value = userByGroup.Profile.DataText1;
                                                break;
                                            case "DATATEXT2":
                                                value = userByGroup.Profile.DataText2;
                                                break;
                                            case "DATATEXT3":
                                                value = userByGroup.Profile.DataText3;
                                                break;
                                            case "DATATEXT4":
                                                value = userByGroup.Profile.DataText4;
                                                break;
                                        }

                                        if(value != null)
                                        {
                                            Pdf.FillField(form, fieldKey, value as string, 11, baseFontBold);
                                            filled = true;
                                        }
                                    }
                                    else if(dataField.DataType.Equals("CompanyLocationAndLargeDate", StringComparison.OrdinalIgnoreCase))
                                    {
                                        var fechaDocumento = DateTime.Now;
                                        string mes = MonthName(fechaDocumento.Month, company.DefaultLanguage.Iso);
                                        string fechaLarga = string.Format(CultureInfo.InvariantCulture, "{0} a {1} de {2} de {3}", company.MainAddress.City, fechaDocumento.Day, mes, fechaDocumento.Year);
                                        Pdf.FillField(form, fieldKey, fechaLarga, 11, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, company.Name, 11, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("CompanyCIF", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, company.Cif, 11, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("CompanyMainPhone", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, company.Phone, 11, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("CompanyMainEmail", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, company.Email, 11, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("CompanyAddress", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, company.MainAddress.FullAddress, 11, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("CompanyMainSignatureName", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, company.ContactPerson.FullName, 11, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("CompanyMainSignatureNIF", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Pdf.FillField(form, fieldKey, company.ContactPerson.NIF, 11, baseFontBold);
                                        filled = true;
                                    }
                                    else if(dataField.DataType.Equals("ImageField", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if(!string.IsNullOrEmpty(data[dataField.Data[0].Index])) {
                                            var imagePath = string.Format(CultureInfo.InvariantCulture,
                                                @"{0}\Images\{1}\{2}",
                                                Instance.Path.Data(instanceName),
                                                template.Destination.ItemName,
                                                data[dataField.Data[0].Index]);
                                            if(File.Exists(imagePath))
                                            {
                                                var position = stamper.AcroFields.GetFieldPositions(fieldKey)[0];
                                                var cb = stamper.GetOverContent(position.page);
                                                var rectangle = new Rectangle(position.position.Left, position.position.Top, position.position.Right, position.position.Bottom, 0);
                                                cb.Rectangle(rectangle);

                                                var img = iTextSharp.text.Image.GetInstance(imagePath);
                                                var canvas = stamper.GetOverContent(position.page);
                                                img.ScaleAbsolute(position.position.Right - position.position.Left, position.position.Top - position.position.Bottom);
                                                img.SetAbsolutePosition(position.position.Left, position.position.Bottom);
                                                canvas.AddImage(img);
                                            }

                                            filled = true;
                                        }
                                    }
                                    else
                                    {
                                        Pdf.FillField(form, fieldKey, "*", 11, baseFontBold);
                                        filled = true;
                                    }

                                    break;
                                }
                            }

                            // Los datos que vienen en la query de la llamada y no han sido ya rellenados
                            if(!filled)
                            {
                                foreach(var dataField in template.Fields.Where(f => f.FieldName.Equals(fieldKey, StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(f.Value) == false))
                                {
                                    if(dataField.FieldName.Equals(fieldKey, StringComparison.OrdinalIgnoreCase))
                                    {
                                        if(dataField.DataType.Equals("FromQuery", StringComparison.OrdinalIgnoreCase))
                                        {
                                            var valueText = HttpContext.Current.Request.QueryString[dataField.Value] as string;
                                            if(!string.IsNullOrEmpty(valueText))
                                            {
                                                Pdf.FillField(form, fieldKey, valueText, 11, baseFontBold);
                                                filled = true;
                                            }
                                        }
                                        else if(dataField.DataType.Equals("UserByGroup", StringComparison.OrdinalIgnoreCase))
                                        {
                                            var userByGroup = ApplicationUser.ChoseByGroup(dataField.GroupId, company.Id, instanceName);
                                            var value = string.Empty;

                                            switch(dataField.Value.ToUpperInvariant())
                                            {
                                                case "NAME":
                                                    value = userByGroup.Profile.FullName;
                                                    break;
                                                case "PHONE":
                                                    value = userByGroup.Profile.Phone;
                                                    break;
                                                case "DATATEXT1":
                                                    value = userByGroup.Profile.DataText1;
                                                    break;
                                                case "DATATEXT2":
                                                    value = userByGroup.Profile.DataText2;
                                                    break;
                                                case "DATATEXT3":
                                                    value = userByGroup.Profile.DataText3;
                                                    break;
                                                case "DATATEXT4":
                                                    value = userByGroup.Profile.DataText4;
                                                    break;
                                            }

                                            if(value != null)
                                            {
                                                Pdf.FillField(form, fieldKey, value as string, 11, baseFontBold);
                                                filled = true;
                                            }
                                        }
                                        else if(dataField.DataType.Equals("UserData", StringComparison.OrdinalIgnoreCase))
                                        {
                                            var value = user.Profile.GetType().GetProperty(dataField.Value).GetValue(user.Profile, null);
                                            if(value != null)
                                            {
                                                Pdf.FillField(form, fieldKey, value as string, 11, baseFontBold);
                                                filled = true;
                                            }
                                        }
                                        else
                                        {
                                            if(dataField.DataType.Equals("checkbox", StringComparison.OrdinalIgnoreCase))
                                            {
                                                var param = dataField.FieldName;
                                                var paramValue = parameters[param] as string;
                                                Pdf.FillField(form, fieldKey, paramValue, 16, baseFontCheckBox);
                                                filled = true;
                                            }
                                            else
                                            {
                                                var param = dataField.FieldName;
                                                var paramValue = parameters[param] as string;
                                                Pdf.FillField(form, fieldKey, paramValue, 11, baseFontBold);
                                                filled = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Piés de página
                        if(template.Footers != null && template.Footers.Count > 0)
                        {
                            foreach(var dataField in template.Footers)
                            {
                                if(fieldKeys.Any(f => f.StartsWith(dataField.FieldName, StringComparison.OrdinalIgnoreCase)))
                                {
                                    var candidates = fieldKeys.Where(f => f.StartsWith(dataField.FieldName, StringComparison.OrdinalIgnoreCase));
                                    foreach(var fieldKey in candidates)
                                    {
                                        switch(dataField.DataType.ToUpperInvariant())
                                        {
                                            case "FROMQUERY":
                                                var valueText = HttpContext.Current.Request.QueryString[dataField.Value] as string;
                                                if(!string.IsNullOrEmpty(valueText))
                                                {
                                                    Pdf.FillField(form, fieldKey, valueText, fontSizeFooter, baseFont);
                                                }
                                                break;
                                            case "USERDATA":
                                                var value = user.Profile.GetType().GetProperty(dataField.Value).GetValue(user.Profile, null);
                                                if(value != null)
                                                {
                                                    Pdf.FillField(form, fieldKey, value as string, fontSizeFooter, baseFont);
                                                }
                                                break;
                                            case "COMPOSITE":
                                                var compositeData = new List<string>();
                                                foreach(var cd in dataField.Data)
                                                {
                                                    switch(cd.DataType.ToUpperInvariant())
                                                    {
                                                        case "COMPANYNAME":
                                                            compositeData.Add(company.Name);
                                                            break;
                                                        case "COMPANYADDRESS":
                                                            compositeData.Add(company.MainAddress.FullStreetAddress);
                                                            break;
                                                        case "COMPANYPHONE":
                                                            compositeData.Add(company.ContactPerson.Phone1);
                                                            break;
                                                        case "COMPANYPHONE2":
                                                            compositeData.Add(company.ContactPerson.Phone2);
                                                            break;
                                                        case "COMPANYWEB":
                                                            compositeData.Add(company.Web);
                                                            break;
                                                        case "COMPANYMAIL":
                                                            compositeData.Add(company.Email);
                                                            break;
                                                        default:
                                                            compositeData.Add(data.First(d => d.Key == cd.Index).Value.Trim());
                                                            break;
                                                    }
                                                }

                                                var finalText = string.Empty;
                                                var parts = dataField.Pattern.Split('|');
                                                int cont = 0;
                                                foreach(string dataPart in compositeData.ToArray())
                                                {
                                                    if(!string.IsNullOrEmpty(parts[cont].Trim()))
                                                    {
                                                        finalText += parts[cont];
                                                    }

                                                    finalText += (dataPart ?? string.Empty).Trim();
                                                    cont++;
                                                }

                                                if(parts.Length == cont + 1)
                                                {
                                                    finalText += parts[cont];
                                                }

                                                //Pdf.ReplaceField(stamper, fieldKey, dataField.Pattern, compositeData.ToArray(), baseFont, baseFont, fontSizeFooter);
                                                Pdf.FillField(form, fieldKey, finalText, fontSizeFooter, baseFont);
                                                break;
                                            default:
                                                var param = dataField.FieldName;
                                                var paramValue = parameters[param] as string;
                                                Pdf.FillField(form, fieldKey, paramValue, fontSizeFooter, baseFont);
                                                break;
                                        }
                                    }
                                }
                            }
                        }

                        // Se acaba el rellenado del formulario
                        stamper.FormFlattening = true;

                        if(template.Images != null)
                        {
                            foreach(var image in template.Images)
                            {
                                var fileImage = string.Empty;
                                switch(image.File.ToUpperInvariant())
                                {
                                    case "#COMPANYLOGO":
                                        fileImage = company.LogoPathPdf;

                                        if(!File.Exists(fileImage))
                                        {
                                            fileImage = company.LogoPath;
                                        }

                                        break;
                                    case "#COMPANYMAINSIGNATURE":
                                        fileImage = string.Format(
                                            CultureInfo.InvariantCulture,
                                            @"{0}{2}CompanyData\{1}\MainSignature.png",
                                            Instance.Path.Data(instanceName),
                                            company.Id,
                                            Instance.Path.Data(instanceName).EndsWith("\\") ? string.Empty : "\\");
                                        image.SetDimensions(image.Width, image.Height);
                                        break;
                                    case "#SIGNERUSERSIGNATURE":
                                        fileImage = HttpContext.Current.Request.PhysicalApplicationPath + signerUser.Profile.Signature(instanceName).Substring(1).Replace("/", "\\");
                                        image.SetDimensions(150, 75);
                                        break;
                                    case "#USERSIGNATURE":
                                        fileImage = HttpContext.Current.Request.PhysicalApplicationPath + user.Profile.Signature(instanceName).Substring(1).Replace("/", "\\");
                                        image.SetDimensions(150, 75);
                                        break;
                                    default:
                                        if(image.File.StartsWith("#USERBYGROUP_"))
                                        {
                                            var groupId = Convert.ToInt64(image.File.Split('_')[1].Replace("#", string.Empty));
                                            var userSignature = ApplicationUser.ChoseByGroup(groupId, companyId, instanceName);
                                            fileImage = userSignature.Profile.SignaturePath(instanceName);
                                            image.SetDimensions(150, 75);
                                        }
                                        else if(image.File.StartsWith("#COMPANYCONFIG_"))
                                        {
                                            var userId = company.CustomFiels.First(c => c.Key.Equals("MedicoTitular", StringComparison.OrdinalIgnoreCase)).Value;
                                            var userSignature = ApplicationUser.ById(Convert.ToInt64(userId), instanceName);
                                            fileImage = userSignature.Profile.SignaturePath(instanceName);
                                            image.SetDimensions(150, 75);
                                        }
                                        else
                                        {
                                            fileImage = image.File;
                                        }
                                        break;
                                }

                                // Evitar que falle el pdf si los usuarios no tienen firma
                                if(File.Exists(fileImage))
                                {
                                    var img = iTextSharp.text.Image.GetInstance(fileImage);
                                    var canvas = stamper.GetOverContent(image.Page);
                                    img.ScaleAbsolute(image.Width, image.Height);
                                    img.SetAbsolutePosition(image.X, image.Y);
                                    canvas.AddImage(img);
                                }
                            }
                        }

                        try
                        {
                            stamper.Close();
                            pdfReader.Close();
                        }
                        catch(Exception ex)
                        {
                            ExceptionManager.Trace(ex, template.Name);
                            fileNameNew = ex.Message;
                        }
                    }
                }
                // ---------------------------------------

                var itemDefinitionId = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(template.Destination.ItemName, StringComparison.OrdinalIgnoreCase)).Id;

                if(template.Destination != null && !string.IsNullOrEmpty(template.Destination.ItemField))
                {
                    if(template.Signatures.Count > 0)
                    {
                        var itemDefinition = Persistence.ItemDefinitions(instanceName).First(i => i.ItemName.Equals(template.Destination.ItemName, StringComparison.OrdinalIgnoreCase));
                        var signStatus = DocumentsSign.ByFieldName(company.Id, itemDefinition.Id, itemId, template.Destination.ItemField, instanceName);
                        var documentSingRes = new DocumentsSign
                        {
                            Id = Constant.DefaultId,
                            CompanyId = company.Id,
                            ItemDefinitionId = itemDefinitionId,
                            ItemId = itemId,
                            FieldName = template.Destination.ItemField
                        }.Insert(user.Id, instanceName);

                        if(documentSingRes.Success)
                        {
                            signatureId = Convert.ToInt64(documentSingRes.ReturnValue);
                        }

                        ActionLog.Trace(template.Destination.ItemField + ": Document To Sign", instance.Name, itemDefinition.ItemName, itemId, companyId);
                    }
                    else
                    {
                        if(existsPrevious)
                        {
                            Save.UpdateDocument(itemId, template.Destination.ItemName, template.Destination.ItemField, Path.GetFileName(ficheroCopiaantiMachaca), user.Id, company.Id, instanceName);
                            ActionLog.Trace(template.Destination.ItemField + ": Document created", instance.Name, template.Destination.ItemName, itemId, companyId);
                        }
                        else
                        {
                            Save.UpdateDocumentWithoutPrevious(itemId, template.Destination.ItemName, template.Destination.ItemField, Path.GetFileName(fileNameNew), user.Id, instanceName);
                            ActionLog.Trace(template.Destination.ItemField + ": Document created with historial", instance.Name, template.Destination.ItemName, itemId, companyId);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.InvariantCulture, @"GenerateFromTemplate({0}, {1})", template.Template, itemId));
            }

            // Determinar si el documento está asociado a algún campo para enviarlo como resultado
            var res = new StringBuilder();
            if(template.Destination != null && !string.IsNullOrEmpty(template.Destination.ItemField))
            {
                res.Append("{\"FileUrl\":\"");
                if(template.Signatures.Count > 0)
                {
                    res.Append("SignaturesTemporal/");
                }
                res.Append(template.ResultFile + "\"");
                res.AppendFormat(CultureInfo.InvariantCulture, @", ""FieldName"":""{0}""", template.Destination.ItemField);
                if(signatureId > 0)
                {
                    res.AppendFormat(CultureInfo.InvariantCulture, @", ""Id"":{0}", signatureId);
                }
                res.Append("}");
            }
            else
            {
                res.Append("{\"FileUrl\":\"/Instances/" + instance.Name + "/Data/DocumentsGallery/" + template.Destination.ItemName + "/" + itemId + "/" + Path.GetFileName(fileNameNew) + "\"}");
            }

            return res.ToString();
        }

        /// <summary>Gets name of month</summary>
        /// <param name="index">Index of month</param>
        /// <param name="languageCode">Code on laguange to translate</param>
        /// <returns>Name of month</returns>
        private static string MonthName(int index, string languageCode)
        {
            switch(index)
            {
                case Constant.Month.January + 1: return "Gener"; //ApplicationDictionary.Translate("Common_MonthName_January");
                case Constant.Month.February + 1: return "Febrer"; // ApplicationDictionary.Translate("Common_MonthName_February");
                case Constant.Month.March + 1: return "Març";  //ApplicationDictionary.Translate("Common_MonthName_March");
                case Constant.Month.April + 1: return "Abril"; // ApplicationDictionary.Translate("Common_MonthName_April");
                case Constant.Month.May + 1: return "Maig"; // ApplicationDictionary.Translate("Common_MonthName_May");
                case Constant.Month.June + 1: return "Juny"; // ApplicationDictionary.Translate("Common_MonthName_June");
                case Constant.Month.July + 1: return "Juliol"; // ApplicationDictionary.Translate("Common_MonthName_July");
                case Constant.Month.August + 1: return "Agost"; // ApplicationDictionary.Translate("Common_MonthName_August");
                case Constant.Month.September + 1: return "Setembre"; // ApplicationDictionary.Translate("Common_MonthName_September");
                case Constant.Month.October + 1: return "Octubre"; // ApplicationDictionary.Translate("Common_MonthName_October");
                case Constant.Month.November + 1: return "Novembre"; // ApplicationDictionary.Translate("Common_MonthName_November");
                case Constant.Month.December + 1: return "Desembre"; // ApplicationDictionary.Translate("Common_MonthName_December");
                default: return string.Empty;
            }
        }
    }
}