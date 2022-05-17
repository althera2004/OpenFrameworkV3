// --------------------------------
// <copyright file="TwoColumnHeaderFooter.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Reports
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    /// <summary>Implements handler of begin and end pages to set header of footer in document</summary>
    public class TwoColumnHeaderFooter : PdfPageEventHelper
    {
        /// <summary>This is the content byte object of the writer</summary> 
        private PdfContentByte contentByte;

        /// <summary>This is the BaseFont we are going to use for the header / footer</summary> 
        private BaseFont baseFont = null;

        // This keeps track of the creation time
        private DateTime printTime = DateTime.Now;

        #region Properties
        public string Title { get; set; }
        public string HeaderLeft { get; set; }
        public string HeaderRight { get; set; }

        [CLSCompliant(false)]
        public Font HeaderFont { get; set; }

        [CLSCompliant(false)]
        public Font FooterFont { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Date { get; set; }
        public string CreatedBy { get; set; }

        [CLSCompliant(false)]
        private PdfTemplate template;

        public string CompanyLogo { get; set; }

        [CLSCompliant(false)]
        public BaseColor TitleBackground { get; set; }

        public string Link { get; set; }
        #endregion
        // we override the onOpenDocument method
        [CLSCompliant(false)]
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            if (writer == null || document == null)
            {
                return;
            }

            try
            {
                // ------------ FONTS 
                string path = HttpContext.Current.Request.PhysicalApplicationPath;
                string pathFonts = HttpContext.Current.Request.PhysicalApplicationPath;
                if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                {
                    pathFonts = string.Format(CultureInfo.InstalledUICulture, @"{0}\", pathFonts);
                }

                this.printTime = DateTime.Now;
                this.baseFont = BaseFont.CreateFont(string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", pathFonts), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                this.contentByte = writer.DirectContent;
                this.template = this.contentByte.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
            }
            catch (IOException ioe)
            {
            }
        }

        [CLSCompliant(false)]
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            if (writer == null || document == null)
            {
                return;
            }

            base.OnStartPage(writer, document);
            Rectangle pageSize = document.PageSize;

            if (this.TitleBackground != null)
            {
                this.contentByte.SetColorFill(this.TitleBackground);
            }

            this.contentByte.Rectangle(39f, document.PageSize.Height - 60f, document.PageSize.Width - 78f, 30f);
            this.contentByte.FillStroke();

            // Lineas
            this.contentByte.SetLineWidth(0.5f);
            this.contentByte.MoveTo(40f, document.PageSize.Height - 30f);
            this.contentByte.LineTo(document.PageSize.Width - 40f, document.PageSize.Height - 30f);
            this.contentByte.Stroke();
            this.contentByte.MoveTo(40f, document.PageSize.Height - 60f);
            this.contentByte.LineTo(document.PageSize.Width - 40f, document.PageSize.Height - 60f);
            this.contentByte.Stroke();
            this.contentByte.SetColorFill(BaseColor.WHITE);

            // Titulo
            this.contentByte.BeginText();
            this.contentByte.SetFontAndSize(this.baseFont, 14);
            this.contentByte.ShowTextAligned(
                PdfContentByte.ALIGN_CENTER,
                this.Title,
                pageSize.GetRight(document.PageSize.Width / 2),
                pageSize.GetTop(50),
                0);
            this.contentByte.EndText();

            // Empresa
            /*if (string.IsNullOrEmpty(this.CompanyLogo))
            {
                this.contentByte.BeginText();
                this.contentByte.SetFontAndSize(this.baseFont, 10);
                this.contentByte.ShowTextAligned(
                    PdfContentByte.ALIGN_LEFT,
                    this.CompanyName,
                    pageSize.GetLeft(40),
                    pageSize.GetTop(48),
                    0);
                this.contentByte.EndText();
            }
            else
            {
                var tif = Image.GetInstance(this.CompanyLogo);
                tif.ScalePercent(33f);
                tif.SetAbsolutePosition(40f, document.PageSize.Height - 58f);
                this.contentByte.AddImage(tif);
            }

            var logoConstraula = Image.GetInstance(this.InstanceLogo);
            logoConstraula.ScalePercent(33f);
            logoConstraula.SetAbsolutePosition(document.PageSize.Width - 150f, document.PageSize.Height - 54f);
            this.contentByte.AddImage(logoConstraula);*/
        }

        [CLSCompliant(false)]
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            if (writer == null || document == null)
            {
                return;
            }

            base.OnEndPage(writer, document);
            int pageN = writer.PageNumber;
            string text = string.Format(CultureInfo.InvariantCulture, "{0} de ", pageN);
            float len = this.baseFont.GetWidthPoint(text, 8);
            var pageSize = document.PageSize;

            // Numero de pagina
            // Add a unique (empty) template for each page here
            this.contentByte.BeginText();
            this.contentByte.SetFontAndSize(this.baseFont, 8);
            this.contentByte.SetTextMatrix((pageSize.Width / 2) - len, pageSize.GetBottom(30));
            this.contentByte.ShowText(text);
            this.contentByte.EndText();
            this.contentByte.AddTemplate(this.template, pageSize.Width / 2, pageSize.GetBottom(30));

            // Fecha
            this.contentByte.BeginText();
            this.contentByte.SetFontAndSize(this.baseFont, 8);
            this.contentByte.ShowTextAligned(
                PdfContentByte.ALIGN_RIGHT,
                this.Date,
                pageSize.GetRight(40),
                pageSize.GetBottom(32),
                0);
            this.contentByte.EndText();

            // Generado
            this.contentByte.BeginText();
            this.contentByte.SetFontAndSize(this.baseFont, 8);
            this.contentByte.ShowTextAligned(
                PdfContentByte.ALIGN_LEFT,
                this.CompanyName,
                pageSize.GetLeft(40),
                pageSize.GetBottom(32),
                0);
            this.contentByte.EndText();

            if (!string.IsNullOrEmpty(this.Link))
            {
                this.contentByte.BeginText();
                this.contentByte.SetFontAndSize(this.baseFont, 8);
                this.contentByte.ShowTextAligned(
                    PdfContentByte.ALIGN_LEFT,
                    this.Link,
                    pageSize.GetLeft(40),
                    pageSize.GetBottom(30),
                    0);
                this.contentByte.EndText();
            }
        }

        [CLSCompliant(false)]
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            if (writer == null || document == null)
            {
                return;
            }

            base.OnCloseDocument(writer, document);
            this.template.BeginText();
            this.template.SetFontAndSize(this.baseFont, 8);
            this.template.SetTextMatrix(0, 0);
            this.template.ShowText(string.Empty + writer.PageNumber);
            this.template.EndText();
        }
    }
}