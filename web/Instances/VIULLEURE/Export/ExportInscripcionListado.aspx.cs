// --------------------------------
// <copyright file="ExportInscripcionListado.aspx.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV2.Web.Viulleure.Export
{
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using OpenFrameworkV2.Core.Activity;
    using OpenFrameworkV2.Core.Companies;
    using OpenFrameworkV2.Core.DataAccess;
    using OpenFrameworkV2.Tools;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using System.Web.UI;

    public partial class ExportInscripcionListado : Page
    {
        /// <summary>Generates "Medicación usuario" report</summary>
        /// <param name="itemId">Item identifier</param>
        /// <returns>Url of generated report</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Generate(long casalId, string ff, string fp, string fc, long companyId)
        {
            var instance = Persistence.InstanceActual();
            var company = Company.ById(companyId, instance.Name);

            var periodos = new List<Periodo>();
            using (var cmd = new SqlCommand("Item_CasalPeriodo_ByCasal"))
            {
                using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CasalId", casalId));
                    try
                    {
                        cmd.Connection.Open();
                        using(var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                periodos.Add(new Periodo
                                {
                                    Id = rdr.GetInt64(0),
                                    Code = rdr.GetString(1)
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

            var res = "0";
            try
            {
                // Se preparan los objetos para el PDF
                var fontName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "calibri.ttf");
                var fontNameBold = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "calibrib.ttf");
                var basefont = BaseFont.CreateFont(fontName, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                var basefontBold = BaseFont.CreateFont(fontNameBold, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                string path = HttpContext.Current.Request.PhysicalApplicationPath;
                if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                {
                    path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
                }

                var fileNameNew = Pdf.Paths.Temporal(instance.Name, "CasalListado.pdf");
                fileNameNew = fileNameNew.Replace(".pdf", Guid.NewGuid() + ".pdf");

                var CasalNombre = string.Empty;

                #region Table
                using (var pdfDoc = new Document(PageSize.A4.Rotate(), 40, 40, 80, 40))
                {
                    var writer = PdfWriter.GetInstance(pdfDoc, new FileStream(fileNameNew, FileMode.Create));
                    using (var cmd = new SqlCommand("Export_InscripcionListado"))
                    {
                        using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                        {
                            cmd.Connection = cnn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DataParameter.Input("@CasalId", casalId));
                            cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                            try
                            {
                                cmd.Connection.Open();
                                using (var rdrm = cmd.ExecuteReader())
                                {
                                    if (rdrm.HasRows)
                                    {
                                        rdrm.Read();
                                        CasalNombre = rdrm.GetString(13);
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

                    writer.PageEvent = new ExportInscripcionListadoHeaderFooter
                    {
                        Nif = "",
                        Cip = "",
                        FechaNacimiento = DateTime.Now,
                        CasalNombre = CasalNombre,
                        CasalPeriodo = "",
                        Telefono = "weke",
                        CompanyName = company.Name,
                        LogoPath = company.LogoPathPdf
                    };
                    pdfDoc.Open();

                    var widths = new List<float>();
                    widths.Add(20);

                    if (ff.IndexOf("|1|") != -1)
                    {
                        widths.Add(5f);
                    }

                    if (ff.IndexOf("|2|") != -1)
                    {
                        widths.Add(10f);
                        widths.Add(5f);
                        widths.Add(10f);
                    }

                    if(ff.IndexOf("|11|") != -1)
                    {
                        widths.Add(15f);
                    }

                    if(ff.IndexOf("|14|") != -1)
                    {
                        widths.Add(15f);
                    }

                    if (ff.IndexOf("|3|") != -1)
                    {
                        widths.Add(15f);
                    }

                    if (ff.IndexOf("|4|") != -1)
                    {
                        widths.Add(20f);
                    }

                    if (ff.IndexOf("|5|") != -1)
                    {
                        widths.Add(15f);
                    }

                    if (ff.IndexOf("|6|") != -1)
                    {
                        widths.Add(15f);
                    }

                    if (ff.IndexOf("|7|") != -1)
                    {
                        widths.Add(15f);
                    }

                    if (ff.IndexOf("|8|") != -1)
                    {
                        widths.Add(5f);
                    }

                    if (ff.IndexOf("|9|") != -1)
                    {
                        widths.Add(15f);
                    }

                    if(ff.IndexOf("|10|") != -1)
                    {
                        widths.Add(15f);
                    }

                    if(ff.IndexOf("|13|") != -1 && widths.Count < 3)
                    {
                        widths.Add(45f);
                    }

                    if(ff.IndexOf("|14|") != -1 && widths.Count < 3)
                    {
                        widths.Add(45f);
                    }

                    var table = new PdfPTable(widths.Count)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 1,
                        SpacingBefore = 20f,
                        SpacingAfter = 0f,
                        HeaderRows = 1
                    };

                    if(!string.IsNullOrEmpty(fc))
                    {
                        var criteriaText = "Criteris: ";
                        var firstCriteria = true;
                        if(fc.IndexOf("1") != -1)
                        {
                            criteriaText += "amb alergies";
                            firstCriteria = false;
                        }

                        if(fc.IndexOf("2") != -1)
                        {
                            criteriaText += firstCriteria ? string.Empty : ", ";
                            criteriaText += "amb medicació";
                            firstCriteria = false;
                        }

                        if(fc.IndexOf("3") != -1)
                        {
                            criteriaText += firstCriteria ? string.Empty : ", ";
                            criteriaText += "amb dieta";
                            firstCriteria = false;
                        }

                        if(fc.IndexOf("4") != -1)
                        {
                            criteriaText += firstCriteria ? string.Empty : ", ";
                            criteriaText += "amb cansanci";
                            firstCriteria = false;
                        }

                        if(fc.IndexOf("5") != -1)
                        {
                            criteriaText += firstCriteria ? string.Empty : ", ";
                            criteriaText += "amb insomni";
                            firstCriteria = false;
                        }

                        if(fc.IndexOf("6") != -1)
                        {
                            criteriaText += firstCriteria ? string.Empty : ", ";
                            criteriaText += "amb enuresi";
                            firstCriteria = false;
                        }

                        if(fc.IndexOf("7") != -1)
                        {
                            criteriaText += firstCriteria ? string.Empty : ", ";
                            criteriaText += "amb necessitats especials";
                            firstCriteria = false;
                        }

                        pdfDoc.Add(new Paragraph(criteriaText));
                    }

                    table.SetWidths(widths.ToArray());

                    table.AddCell(HeaderCellLeft("Nen/a", basefont));

                    if (ff.IndexOf("|1|") != -1)
                    {
                        table.AddCell(HeaderCellLeft("Curs", basefont));
                    }

                    if (ff.IndexOf("|2|") != -1)
                    {
                        table.AddCell(HeaderCell("Tutor", basefont));
                        table.AddCell(HeaderCell("Tel", basefont));
                        table.AddCell(HeaderCell("Email", basefont));
                    }

                    if (ff.IndexOf("|11|") != -1)
                    {
                        table.AddCell(HeaderCell("Adreça", basefont));
                    }

                    if(ff.IndexOf("|14|") != -1)
                    {
                        table.AddCell(HeaderCell("Municipi", basefont));
                    }

                    if (ff.IndexOf("|3|") != -1)
                    {
                        table.AddCell(HeaderCell("Periodes", basefont));
                    }

                    if (ff.IndexOf("|4|") != -1)
                    {
                        table.AddCell(HeaderCell("Observacions", basefont));
                    }

                    if (ff.IndexOf("|5|") != -1)
                    {
                        table.AddCell(HeaderCell("ALèrgies", basefont));
                    }

                    if (ff.IndexOf("|6|") != -1)
                    {
                        table.AddCell(HeaderCell("Medicaments", basefont));
                    }

                    if (ff.IndexOf("|7|") != -1)
                    {
                        table.AddCell(HeaderCell("Dieta", basefont));
                    }

                    if (ff.IndexOf("|8|") != -1)
                    {
                        table.AddCell(HeaderCell("Cansanci", basefont));
                    }

                    if (ff.IndexOf("|9|") != -1)
                    {
                        table.AddCell(HeaderCell("Insomni", basefont));
                    }

                    if(ff.IndexOf("|10|") != -1)
                    {
                        table.AddCell(HeaderCell("Enuresi", basefont));
                    }

                    if(ff.IndexOf("|13|") != -1 && widths.Count < 4)
                    {
                        table.AddCell(HeaderCell("Necessitats especials", basefont));
                    }

                    //table.AddCell(HeaderCell("Observacions", basefont));
                    //table.AddCell(HeaderCell("Alergies", basefont));
                    //table.AddCell(HeaderCell("Medicaments", basefont));
                    //table.AddCell(HeaderCell("Dieta", basefont));
                    //table.AddCell(HeaderCell("Cansanci", basefont));
                    //table.AddCell(HeaderCell("Insomni", basefont));
                    //table.AddCell(HeaderCell("Enuresi", basefont));

                    using (var cmd = new SqlCommand("Export_InscripcionListado"))
                    {
                        using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                        {
                            cmd.Connection = cnn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DataParameter.Input("@CasalId", casalId));
                            cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                            try
                            {
                                cmd.Connection.Open();
                                var odd = false;
                                using (var rdrm = cmd.ExecuteReader())
                                {
                                    while (rdrm.Read())
                                    {
                                        var show = true;

                                        #region Aplicar criterios
                                        if(fc.IndexOf("1") != -1 && string.IsNullOrEmpty(rdrm.GetString(7)))
                                        {
                                            show = false;
                                        }

                                        if(fc.IndexOf("2") != -1 && string.IsNullOrEmpty(rdrm.GetString(8)))
                                        {
                                            show = false;
                                        }

                                        if(fc.IndexOf("3") != -1 && string.IsNullOrEmpty(rdrm.GetString(9)))
                                        {
                                            show = false;
                                        }

                                        if(fc.IndexOf("4") != -1 && string.IsNullOrEmpty(rdrm.GetString(10)))
                                        {
                                            show = false;
                                        }

                                        if(fc.IndexOf("5") != -1 && string.IsNullOrEmpty(rdrm.GetString(11)))
                                        {
                                            show = false;
                                        }

                                        if(fc.IndexOf("6") != -1 && string.IsNullOrEmpty(rdrm.GetString(12)))
                                        {
                                            show = false;
                                        }

                                        if(fc.IndexOf("7") != -1 && string.IsNullOrEmpty(rdrm.GetString(19)))
                                        {
                                            show = false;
                                        }
                                        #endregion

                                        if(show)
                                        {
                                            table.AddCell(CellTable(rdrm.GetString(0), basefont, odd, 14));
                                            if(ff.IndexOf("|1|") != -1)
                                            {
                                                table.AddCell(CellTable(rdrm.GetString(1), basefont, odd, 14));
                                            }

                                            if(ff.IndexOf("|2|") != -1)
                                            {
                                                table.AddCell(CellTable(rdrm.GetString(2), basefont, odd, 10));
                                                table.AddCell(CellTable(rdrm.GetString(3), basefont, odd, 10));
                                                table.AddCell(CellTable(rdrm.GetString(4), basefont, odd, 10));
                                            }

                                            if(ff.IndexOf("|11|") != -1)
                                            {
                                                table.AddCell(CellTable(rdrm.GetString(14), basefont, odd, 10));
                                            }

                                            if(ff.IndexOf("|14|") != -1)
                                            {
                                                table.AddCell(CellTable(rdrm.GetString(20), basefont, odd, 10));
                                            }

                                            if(ff.IndexOf("|3|") != -1)
                                            {
                                                var periodesIds = rdrm.GetString(5);
                                                var periodesText = string.Empty;
                                                var parts = periodesIds.Split('|');
                                                var firstPeriode = true;
                                                foreach(var part in parts)
                                                {
                                                    if(string.IsNullOrEmpty(part))
                                                    {
                                                        continue;
                                                    }

                                                    if(periodos.Any(p => p.Id == Convert.ToInt64(part)))
                                                    {
                                                        if(firstPeriode)
                                                        {
                                                            firstPeriode = false;
                                                        }
                                                        else
                                                        {
                                                            periodesText += ", ";
                                                        }

                                                        periodesText += periodos.First(p => p.Id == Convert.ToInt64(part)).Code;
                                                    }
                                                }

                                                table.AddCell(CellTable(periodesText, basefont, odd, 14));
                                            }
                                            if(ff.IndexOf("|4|") != -1)
                                            {
                                                table.AddCell(CellTable(rdrm.GetString(6), basefont, odd, 10));
                                            }
                                            if(ff.IndexOf("|5|") != -1)
                                            {
                                                table.AddCell(CellTable(rdrm.GetString(7), basefont, odd, 10));
                                            }
                                            if(ff.IndexOf("|6|") != -1)
                                            {
                                                table.AddCell(CellTable(rdrm.GetString(8), basefont, odd, 10));
                                            }
                                            if(ff.IndexOf("|7|") != -1)
                                            {
                                                table.AddCell(CellTable(rdrm.GetString(9), basefont, odd, 10));
                                            }
                                            if(ff.IndexOf("|8|") != -1)
                                            {
                                                table.AddCell(CellTable(rdrm.GetString(10), basefont, odd, 10));
                                            }
                                            if(ff.IndexOf("|9|") != -1)
                                            {
                                                table.AddCell(CellTable(rdrm.GetString(11), basefont, odd, 10));
                                            }
                                            if(ff.IndexOf("|10|") != -1)
                                            {
                                                table.AddCell(CellTable(rdrm.GetString(12), basefont, odd, 10));
                                            }

                                            if(ff.IndexOf("|13|") != -1)
                                            {
                                                if(widths.Count < 4)
                                                {
                                                    table.AddCell(CellTable(rdrm.GetString(19), basefont, odd, 10));
                                                }
                                                else
                                                {
                                                    if(!string.IsNullOrEmpty(rdrm.GetString(19)))
                                                    {
                                                        table.AddCell(CellTableRight("Necessitats especials:", basefont, odd, 10));
                                                        table.AddCell(CellTableSpan(rdrm.GetString(19), basefont, odd, 10, widths.Count - 1));
                                                    }
                                                }
                                            }

                                            odd = !odd;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                var x = ex.Message;
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

                    pdfDoc.Add(table);
                    pdfDoc.CloseDocument();
                }
                #endregion

                res = fileNameNew;
            }
            catch (Exception ex)
            {
                ExceptionManager.Trace(ex, "Export casal listado inscripciones");
            }

            return Basics.PathToUrl(res, true);
        }

        /// <summary>Gets name of month</summary>
        /// <param name="index">Index of month</param>
        /// <param name="languageCode">Code for language to translate</param>
        /// <returns>Name of month</returns>
        public static string MonthName(int index, string languageCode)
        {
            switch (index)
            {
                case Constant.Month.January + 1: return "Gener";
                case Constant.Month.February + 1: return "Febrer";
                case Constant.Month.March + 1: return "Març";
                case Constant.Month.April + 1: return "Abril";
                case Constant.Month.May + 1: return "Maig";
                case Constant.Month.June + 1: return "Juny";
                case Constant.Month.July + 1: return "Juliol";
                case Constant.Month.August + 1: return "Agost";
                case Constant.Month.September + 1: return "Setembre";
                case Constant.Month.October + 1: return "Octubre";
                case Constant.Month.November + 1: return "Novembre";
                case Constant.Month.December + 1: return "Desembre";
                default: return string.Empty;
            }
        }

        /// <summary>Creates de cell for list header</summary>
        /// <param name="label">Label to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell for list header</returns>
        public static PdfPCell HeaderCellLeft(string label, BaseFont font)
        {
            var finalLabel = string.Empty;
            if (!string.IsNullOrEmpty(label))
            {
                finalLabel = label;
            }

            return new PdfPCell(new Phrase(finalLabel.ToUpperInvariant(), new Font(font, 9, Font.NORMAL, BaseColor.BLACK)))
            {
                Border = Rectangle.BOTTOM_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 4
            };
        }

        /// <summary>Creates de cell for list header</summary>
        /// <param name="label">Label to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell for list header</returns>
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
        /// <param name="odd">Indicates if is an odd row for background color</param>
        /// <param name="border">Cell border type</param>
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
        /// <param name="odd">Indicates if is an odd row for background color</param>
        /// <param name="border">Cell border type</param>
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
        /// <param name="odd">Indicates if is an odd row for background color</param>
        /// <param name="border">Cell border type</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTableRight(string value, BaseFont font, bool odd, int border)
        {
            var finalValue = string.Empty;
            if(!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

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
        /// <param name="odd">Indicates if is an odd row for background color</param>
        /// <param name="border">Cell border type</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTableSpan(string value, BaseFont font, bool odd, int border, int span)
        {
            var finalValue = string.Empty;
            if(!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

            return new PdfPCell(new Phrase(finalValue, new Font(font, 9, Font.NORMAL, BaseColor.BLACK)))
            {
                Border = border,
                Padding = 6f,
                PaddingTop = 6f,
                BackgroundColor = odd ? BaseColor.WHITE : new BaseColor(240, 240, 240),
                Colspan = span
            };
        }

        /// <summary>Creates a blank cell table</summary>
        /// <param name="odd">Indicates if is an odd row for background color</param>
        /// <param name="font">Font for content</param>
        /// <param name="border">Cell border type</param>
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

        /// <summary>Page's load event</summary>
        /// <param name="sender">Page loaded</param>
        /// <param name="e">Arguments of event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public struct Periodo
        {
            public long Id { get; set; }
            public string Code { get; set; }
        }
    }

    /// <summary>Header/footer for "Medicación usuario" report</summary>
    public class ExportInscripcionListadoHeaderFooter : PdfPageEventHelper
    {
        /// <summary>Gets or sets name of resident</summary>
        public string CasalNombre { get; set; }

        /// <summary>Gets or sets name of principal contact</summary>
        public string CasalPeriodo { get; set; }

        /// <summary>Gets or sets birthday of resident</summary>
        public DateTime FechaNacimiento { get; set; }

        /// <summary>Gets or sets NIF of resident</summary>
        public string Nif { get; set; }

        /// <summary>Gets or sets CIP of resident</summary>
        public string Cip { get; set; }

        /// <summary>Gets or sets phone of resident</summary>
        public string Telefono { get; set; }

        /// <summary>Gets or sets name of company</summary>
        public string CompanyName { get; set; }

        /// <summary>Gets or sets path for logo image</summary>
        public string LogoPath { get; set; }

        /// <summary>Gets or sets name of professional</summary>
        public string MedicoNombre { get; set; }

        /// <summary>Gets or sets acreditation number of professional</summary>
        public string NumColegiado { get; set; }

        /// <summary>Gets or sets filename of professional signature</summary>
        public string SignatureFileName { get; set; }

        /// <summary>Event on start page</summary>
        /// <param name="writer">Document writer</param>
        /// <param name="pdfDoc">Document of event</param>
        [CLSCompliant(false)]
        public override void OnStartPage(PdfWriter writer, Document pdfDoc)
        {
            string fontName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "calibri.ttf");
            string fontNameBold = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "calibrib.ttf");
            BaseFont basefont = BaseFont.CreateFont(fontName, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            BaseFont basefontBold = BaseFont.CreateFont(fontNameBold, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            base.OnStartPage(writer, pdfDoc);
            var contentByte = writer.DirectContent;
            Rectangle pageSize = pdfDoc.PageSize;

            if (File.Exists(this.LogoPath))
            {
                var logo = Image.GetInstance(this.LogoPath);
                logo.ScaleAbsolute(150f, 75f);
                logo.SetAbsolutePosition(pageSize.GetLeft(30), pageSize.GetTop(75));
                contentByte.AddImage(logo);
            }

            contentByte.BeginText();
            contentByte.SetFontAndSize(basefontBold, 24);
            contentByte.ShowTextAligned(
                PdfContentByte.ALIGN_CENTER,
                this.CasalNombre,
                pageSize.GetRight(pdfDoc.PageSize.Width / 2),
                pageSize.GetTop(30),
                0);
            contentByte.EndText();

            contentByte.BeginText();
            contentByte.SetFontAndSize(basefontBold, 14);
            contentByte.ShowTextAligned(
                PdfContentByte.ALIGN_CENTER,
                this.CasalPeriodo,
                pageSize.GetRight(pdfDoc.PageSize.Width / 2),
                pageSize.GetTop(50),
                0);
            contentByte.EndText();
        }

        /// <summary>Event on end page</summary>
        /// <param name="writer">Document writer</param>
        /// <param name="pdfDoc">Document of event</param>
        [CLSCompliant(false)]
        public override void OnEndPage(PdfWriter writer, Document pdfDoc)
        {
            string fontName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "calibri.ttf");
            string fontNameBold = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "calibrib.ttf");
            BaseFont basefont = BaseFont.CreateFont(fontName, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            BaseFont baseffontBold = BaseFont.CreateFont(fontNameBold, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            base.OnEndPage(writer, pdfDoc);
            Rectangle pageSize = pdfDoc.PageSize;
            var contentByte = writer.DirectContent;

            if (File.Exists(this.SignatureFileName))
            {
                var logo = Image.GetInstance(this.SignatureFileName);
                logo.ScaleAbsolute(150f, 75f);
                logo.SetAbsolutePosition(pageSize.GetLeft(120), pageSize.GetBottom(5));
                contentByte.AddImage(logo);
            }

            // Numero de pagina
            // Add a unique (empty) template for each page here
            contentByte.BeginText();
            contentByte.SetFontAndSize(basefont, 8);
            contentByte.ShowTextAligned(
                PdfContentByte.ALIGN_CENTER,
                "Pàgina " + writer.PageNumber.ToString(),
                pageSize.GetRight(pageSize.Width / 2f),
                pageSize.GetBottom(32),
                0);
            contentByte.EndText();

            // Fecha
            contentByte.BeginText();
            contentByte.SetFontAndSize(basefont, 8);
            contentByte.ShowTextAligned(
                PdfContentByte.ALIGN_RIGHT,
                string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", DateTime.Now),
                pageSize.GetRight(40),
                pageSize.GetBottom(32),
                0);
            contentByte.EndText();
        }
    }
}