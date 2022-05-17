// --------------------------------
// <copyright file="ExportCasalPeriodo.aspx.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace Instances.Viulleure.Export
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using System.Web.UI;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using OpenFrameworkV2;
    using OpenFrameworkV2.Core.Activity;
    using OpenFrameworkV2.Core.Companies;
    using OpenFrameworkV2.Core.DataAccess;
    using OpenFrameworkV2.Tools;

    /// <summary>Implements "Medicación usuario" report generation</summary>
    public partial class ExportCasalPeriodo : Page
    {
        /// <summary>Generates "Medicación usuario" report</summary>
        /// <param name="itemId">Item identifier</param>
        /// <returns>Url of generated report</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Generate(long casalPeriodoId, long companyId)
        {
            var instance = Persistence.InstanceByName("viulleure");
            var company = Company.ById(companyId, instance.Name);
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

                var fileNameNew = Pdf.Paths.Temporal(instance.Name, "CasalPeriodo.pdf");
                fileNameNew = fileNameNew.Replace(".pdf", Guid.NewGuid() + ".pdf");

                var CasalNombre = string.Empty;
                var CasalPeriodo = string.Empty;
                long actualGrupo = -1;
                var grupoName = string.Empty;
                var monitorName = string.Empty;

                #region Table
                using (var pdfDoc = new Document(PageSize.A4.Rotate(), 40, 40, 80, 40))
                {
                    var writer = PdfWriter.GetInstance(pdfDoc, new FileStream(fileNameNew, FileMode.Create));
                    using (var cmd = new SqlCommand("Export_CasalGrupos"))
                    {
                        using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                        {
                            cmd.Connection = cnn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DataParameter.Input("@CasalPeriodoId", casalPeriodoId));
                            try
                            {
                                cmd.Connection.Open();
                                using (var rdrm = cmd.ExecuteReader())
                                {
                                    if (rdrm.HasRows)
                                    {
                                        rdrm.Read();
                                        CasalNombre = rdrm.GetString(0);
                                        CasalPeriodo = rdrm.GetString(1);
                                        grupoName = rdrm.GetString(3);
                                        monitorName = rdrm.GetString(4);
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

                    writer.PageEvent = new ExportCasalGrupoHeaderFooter
                    {
                        Nif = "",
                        Cip = "",
                        FechaNacimiento = DateTime.Now,
                        CasalNombre = CasalNombre,
                        CasalPeriodo = CasalPeriodo,
                        Telefono = "weke",
                        CompanyName = company.Name,
                        LogoPath = company.LogoPathPdf
                    };
                    pdfDoc.Open();

                    var table = new PdfPTable(10)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 1,
                        SpacingBefore = 20f,
                        SpacingAfter = 0f,
                        HeaderRows = 1
                    };

                    table.SetWidths(new float[] { 15f, 4f, 10f, 25f, 15f, 4f, 4f, 4f, 4f, 4f });

                    table.AddCell(new PdfPCell(new Phrase("Grup:", new Font(basefontBold, 12, Font.NORMAL, BaseColor.BLACK)))
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Padding = 6,
                        BackgroundColor = new BaseColor(255, 255, 255)
                    });
                    table.AddCell(new PdfPCell(new Phrase(grupoName, new Font(basefont, 12, Font.NORMAL, BaseColor.BLACK)))
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Padding = 6,
                        BackgroundColor = new BaseColor(255, 255, 255),
                        Colspan = 9
                    });
                    table.AddCell(new PdfPCell(new Phrase("Monitor:", new Font(basefontBold, 12, Font.NORMAL, BaseColor.BLACK)))
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Padding = 6,
                        BackgroundColor = new BaseColor(255, 255, 255)
                    });
                    table.AddCell(new PdfPCell(new Phrase(monitorName, new Font(basefont, 12, Font.NORMAL, BaseColor.BLACK)))
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Padding = 6,
                        BackgroundColor = new BaseColor(255, 255, 255),
                        Colspan = 9
                    });

                    table.AddCell(HeaderCellLeft("Nen/a", basefont));
                    table.AddCell(HeaderCellLeft("Curs", basefont));
                    table.AddCell(HeaderCell("Tel", basefont));
                    table.AddCell(HeaderCell("Observacions", basefont));
                    table.AddCell(HeaderCell("Alèrgies", basefont));
                    table.AddCell(HeaderCell("dl", basefont));
                    table.AddCell(HeaderCell("dm", basefont));
                    table.AddCell(HeaderCell("dc", basefont));
                    table.AddCell(HeaderCell("dj", basefont));
                    table.AddCell(HeaderCell("dv", basefont));

                    using (var cmd = new SqlCommand("Export_CasalGrupos"))
                    {
                        using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                        {
                            cmd.Connection = cnn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DataParameter.Input("@CasalPeriodoId", casalPeriodoId));
                            try
                            {
                                cmd.Connection.Open();
                                var odd = false;
                                using (var rdrm = cmd.ExecuteReader())
                                {
                                    while (rdrm.Read())
                                    {
                                        grupoName = rdrm.GetString(3);
                                        monitorName = rdrm.GetString(4);
                                        if (actualGrupo != rdrm.GetInt64(2) && actualGrupo > 0)
                                        {
                                            pdfDoc.Add(table);
                                            table = new PdfPTable(10)
                                            {
                                                WidthPercentage = 100,
                                                HorizontalAlignment = 1,
                                                SpacingBefore = 20f,
                                                SpacingAfter = 0f,
                                                HeaderRows = 1
                                            };

                                            table.SetWidths(new float[] { 15f, 4f, 10f, 25f, 15f, 4f, 4f, 4f, 4f, 4f });

                                            table.AddCell(new PdfPCell(new Phrase("Grup:", new Font(basefontBold, 12, Font.NORMAL, BaseColor.BLACK)))
                                            {
                                                Border = Rectangle.NO_BORDER,
                                                HorizontalAlignment = Element.ALIGN_LEFT,
                                                Padding = 6,
                                                BackgroundColor = new BaseColor(255, 255, 255)
                                            });
                                            table.AddCell(new PdfPCell(new Phrase(grupoName, new Font(basefont, 12, Font.NORMAL, BaseColor.BLACK)))
                                            {
                                                Border = Rectangle.NO_BORDER,
                                                HorizontalAlignment = Element.ALIGN_LEFT,
                                                Padding = 6,
                                                BackgroundColor = new BaseColor(255, 255, 255),
                                                Colspan = 9
                                            });
                                            table.AddCell(new PdfPCell(new Phrase("Monitor:", new Font(basefontBold, 12, Font.NORMAL, BaseColor.BLACK)))
                                            {
                                                Border = Rectangle.NO_BORDER,
                                                HorizontalAlignment = Element.ALIGN_LEFT,
                                                Padding = 6,
                                                BackgroundColor = new BaseColor(255, 255, 255)
                                            });
                                            table.AddCell(new PdfPCell(new Phrase(monitorName, new Font(basefont, 12, Font.NORMAL, BaseColor.BLACK)))
                                            {
                                                Border = Rectangle.NO_BORDER,
                                                HorizontalAlignment = Element.ALIGN_LEFT,
                                                Padding = 6,
                                                BackgroundColor = new BaseColor(255, 255, 255),
                                                Colspan = 9
                                            });

                                            table.AddCell(HeaderCellLeft("Nen/a", basefont));
                                            table.AddCell(HeaderCellLeft("Curs", basefont));
                                            table.AddCell(HeaderCell("Tel", basefont));
                                            table.AddCell(HeaderCell("Observacions", basefont));
                                            table.AddCell(HeaderCell("Alèrgies", basefont));
                                            table.AddCell(HeaderCell("dl", basefont));
                                            table.AddCell(HeaderCell("dm", basefont));
                                            table.AddCell(HeaderCell("dc", basefont));
                                            table.AddCell(HeaderCell("dj", basefont));
                                            table.AddCell(HeaderCell("dv", basefont));
                                            pdfDoc.NewPage();
                                        }

                                        table.AddCell(CellTable(rdrm.GetString(5), basefont, odd, 14));
                                        table.AddCell(CellTable(rdrm.GetString(6), basefont, odd, 14));
                                        table.AddCell(CellTable(rdrm.GetString(7), basefont, odd, 10));
                                        table.AddCell(CellTable(rdrm.GetString(8), basefont, odd, 10));
                                        table.AddCell(CellTable(rdrm.GetString(9), basefont, odd, 10));
                                        table.AddCell(CellTable(string.Empty, basefont, odd, 10));
                                        table.AddCell(CellTable(string.Empty, basefont, odd, 10));
                                        table.AddCell(CellTable(string.Empty, basefont, odd, 10));
                                        table.AddCell(CellTable(string.Empty, basefont, odd, 10));
                                        table.AddCell(CellTable(string.Empty, basefont, odd, 10));
                                        odd = !odd;
                                        actualGrupo = rdrm.GetInt64(2);
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

                    if (actualGrupo > 0)
                    {
                        pdfDoc.Add(table);
                    }

                    pdfDoc.CloseDocument();
                }
                #endregion

                res = fileNameNew;
            }
            catch (Exception ex)
            {
                ExceptionManager.Trace(ex, "Export casal periodo");
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
    }

    /// <summary>Header/footer for "Medicación usuario" report</summary>
    public class ExportCasalGrupoHeaderFooter : PdfPageEventHelper
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