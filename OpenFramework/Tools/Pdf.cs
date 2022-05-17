// --------------------------------
// <copyright file="Pdf.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    /// <summary>Implements tools for PDF documents creation</summary>
    public static partial class Pdf
    {
        /// <summary>Cell with all borders</summary>
        public const int BorderAll = Rectangle.RIGHT_BORDER + Rectangle.TOP_BORDER + Rectangle.LEFT_BORDER + Rectangle.BOTTOM_BORDER;

        /// <summary>Padding for table cells</summary>
        public const float PaddingTableCell = 8;

        /// <summary>Top padding for table cells</summary>
        public const float PaddingTopTableCell = 6;

        /// <summary>Top padding for criteria table cells</summary>
        public const float PaddingTopCriteriaCell = 4;

        /// <summary>Background color for summary cells</summary>
        [CLSCompliant(false)]
        public static readonly BaseColor SummaryBackgroundColor = BaseColor.LIGHT_GRAY;

        /// <summary>Background color for data cells</summary>
        [CLSCompliant(false)]
        public static readonly BaseColor LineBackgroundColor = BaseColor.WHITE;

        /// <summary>Background color for header cells</summary>
        [CLSCompliant(false)]
        public static readonly BaseColor HeaderBackgroundColor = BaseColor.LIGHT_GRAY;

        /// <summary>Gets paths of font files</summary>
        public static string FontPath
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"{0}fonts\ARIAL.TTF", HttpContext.Current.Request.PhysicalApplicationPath);
            }
        }

        public static void PutImage(PdfContentByte contentByte, string imagePath, float width, float height, float positionX, float positionY)
        {
            if (contentByte == null)
            {
                return;
            }

            if (File.Exists(imagePath))
            {
                var logo = Image.GetInstance(imagePath);
                logo.ScaleAbsolute(width, height);
                logo.SetAbsolutePosition(positionX, positionY);
                contentByte.AddImage(logo);
            }
        }

        public static void PutImageFit(PdfContentByte contentByte, string imagePath, float width, float height, float positionX, float positionY)
        {
            if (contentByte == null)
            {
                return;
            }

            if (File.Exists(imagePath))
            {
                var logo = Image.GetInstance(imagePath);
                logo.ScaleAbsolute(width, height);
                logo.SetAbsolutePosition(positionX, positionY);
                contentByte.AddImage(logo);
            }
        }

        public static void PutText(PdfContentByte contentByte, string text, BaseFont baseFont, int fontSize, int aligment, float positionX, float positionY)
        {
            if (contentByte == null || baseFont == null)
            {
                return;
            }

            contentByte.BeginText();
            contentByte.SetFontAndSize(baseFont, fontSize);
            contentByte.ShowTextAligned(
                aligment,
                text,
                positionX,
                positionY,
                0);
            contentByte.EndText();
        }

        public static void PutTextAndLabel(PdfContentByte contentByte, string label, string value, BaseFont fontLabel, BaseFont fontValue, int fontSize, float positionX, float positionY, float separation)
        {
            if(contentByte == null || fontLabel == null || fontValue == null)
            {
                return;
            }

            contentByte.BeginText();
            contentByte.SetFontAndSize(fontLabel, 11);
            contentByte.ShowTextAligned(
                PdfContentByte.ALIGN_RIGHT,
                label,
               positionX,
                positionY,
                0);
            contentByte.EndText();

            contentByte.BeginText();
            contentByte.SetFontAndSize(fontValue, 11);
            contentByte.ShowTextAligned(
                PdfContentByte.ALIGN_LEFT,
                value,
                positionX + separation,
                positionY,
                0);
            contentByte.EndText();
        }

        /// <summary>Creates de cell for list header</summary>
        /// <param name="label">Label to show</param>
        /// <returns>Cell for list header</returns>
        [CLSCompliant(false)]
        public static PdfPCell HeaderCell(string label)
        {
            var finalLabel = string.Empty;
            if (!string.IsNullOrEmpty(label))
            {
                finalLabel = label;
            }

            return new PdfPCell(new Phrase(finalLabel.ToUpperInvariant(), Fonts.Times))
            {
                Border = Border.All,
                BackgroundColor = HeaderBackgroundColor,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = PaddingTableCell,
                PaddingTop = PaddingTopTableCell
            };
        }

        /// <summary>Sets metadata on file</summary>
        /// <param name="reader">Document reader</param>
        /// <param name="stamper">Stamper to put info</param>
        /// <param name="title">Document's title</param>
        [CLSCompliant(false)]
        public static void SetMetadata(PdfReader reader, PdfStamper stamper, string title)
        {
            if(reader == null || stamper == null)
            {
                return;
            }

            Dictionary<string, string> info = reader.Info;
            if (!string.IsNullOrEmpty(title))
            {
                info["Title"] = title;
                info["Subject"] = title;
            }

            info["Keywords"] = string.Empty;
            info["Creator"] = "OpenFramework";
            info["Author"] = "OpendFramework";
            stamper.MoreInfo = info;
        }

        /// <summary>Sets metadata on file</summary>
        /// <param name="reader">Document reader</param>
        /// <param name="stamper">Stamper to put info</param>
        /// <param name="title">Document's title</param>
        /// <param name="author">Document's author</param>
        [CLSCompliant(false)]
        public static void SetMetadata(PdfReader reader, PdfStamper stamper, string title, string author)
        {
            if (reader == null || stamper == null)
            {
                return;
            }

            Dictionary<string, string> info = reader.Info;
            if (!string.IsNullOrEmpty(title))
            {
                info["Title"] = title;
                info["Subject"] = title;
            }

            info["Keywords"] = string.Empty;
            info["Creator"] = "OpenFramework";
            info["Author"] = author;
            stamper.MoreInfo = info;
        }

        /// <summary>Sets metadata on file</summary>
        /// <param name="reader">Document reader</param>
        /// <param name="stamper">Stamper to put info</param>
        /// <param name="title">Document's title</param>
        /// <param name="subject">Document's subject</param>
        /// <param name="author">Document's author</param>
        [CLSCompliant(false)]
        public static void SetMetadata(PdfReader reader, PdfStamper stamper, string title, string subject, string author)
        {
            if (reader == null || stamper == null)
            {
                return;
            }

            Dictionary<string, string> info = reader.Info;
            if (!string.IsNullOrEmpty(title))
            {
                info["Title"] = title;
            }

            if (!string.IsNullOrEmpty(subject))
            {
                info["Subject"] = subject;
            }

            info["Keywords"] = string.Empty;
            info["Creator"] = "OpenFramework";
            info["Author"] = author;
            stamper.MoreInfo = info;
        }

        /// <summary>Sets metadata on file</summary>
        /// <param name="document">Document to set metadata</param>
        /// <param name="title">Document's title</param>
        /// <param name="author">Document's author</param>
        [CLSCompliant(false)]
        public static void SetMetadata(Document document, string title, string author)
        {
            if (document == null)
            {
                return;
            }

            SetMetadata(document, title, title, author);
        }

        public static void SuperCompression(PdfWriter writer)
        {
            writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
            writer.CompressionLevel = PdfStream.BEST_COMPRESSION;
        }

        /// <summary>Sets metadata on file</summary>
        /// <param name="document">Document to set metadata</param>
        /// <param name="title">Document's title</param>
        /// <param name="subject">Document's subject</param>
        /// <param name="author">Document's author</param>
        [CLSCompliant(false)]
        public static void SetMetadata(Document document, string title, string subject, string author)
        {
            if (document == null)
            {
                return;
            }

            document.AddTitle(title);
            document.AddAuthor(author);
            document.AddSubject(subject);
            document.AddCreator("OpenFramework");
        }

        /// <summary>Fill interactive field of document</summary>
        /// <param name="form">Form of field</param>
        /// <param name="fieldName">Name of field</param>
        /// <param name="value">Value to fill</param>
        /// <param name="fontSize">Size of font</param>
        /// <param name="font">Font of text</param>
        [CLSCompliant(false)]
        public static void FillField(AcroFields form, string fieldName, string value, float fontSize, BaseFont font)
        {
            if (form == null)
            {
                return;
            }

            form.SetField(fieldName, value);
            form.SetFieldProperty(fieldName, "textsize", fontSize, null);
            form.SetFieldProperty(fieldName, "textfont", font, null);
            form.RegenerateField(fieldName);
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell CellTable(string value)
        {
            return CellTable(value, Fonts.Times);
        }

        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell CellTable(string value, Font font)
        {
            var finalValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

            return new PdfPCell(new Phrase(finalValue, font))
            {
                Border = Rectangle.TOP_BORDER,
                BackgroundColor = LineBackgroundColor,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = 2
            };
        }

        /// <summary>Creates a cell table with content alignment to right</summary>
        /// <param name="value">Value to show</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCellRight(string value)
        {
            return DataCell(value, Fonts.Times, Rectangle.ALIGN_RIGHT);
        }

        /// <summary>Creates a cell table with content alignment to right</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCellRight(string value, Font font)
        {
            return DataCell(value, font, Rectangle.ALIGN_RIGHT);
        }

        /// <summary>Creates a cell table with content alignment to right</summary>
        /// <param name="value">Integer value to show</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCellRight(int value)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), Fonts.Times, Rectangle.ALIGN_RIGHT);
        }

        /// <summary>Creates a cell table with content alignment to right</summary>
        /// <param name="value">Integer value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCellRight(int value, Font font)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), font, Rectangle.ALIGN_RIGHT);
        }

        /// <summary>Creates a cell table with content alignment to right</summary>
        /// <param name="value">Long value to show</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCellRight(long value)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), Fonts.Times, Rectangle.ALIGN_RIGHT);
        }

        /// <summary>Creates a cell table with content alignment to right</summary>
        /// <param name="value">Long value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCellRight(long value, Font font)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), font, Rectangle.ALIGN_RIGHT);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Integer value to show</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCell(int value)
        {
            return DataCell(value.ToString(), Fonts.Times, Rectangle.ALIGN_LEFT);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Integer value to show</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCell(decimal value)
        {
            return DataCell(value.ToString(), Fonts.Times, Rectangle.ALIGN_RIGHT);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Long value to show</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCell(long value)
        {
            return DataCell(value.ToString(), Fonts.Times, Rectangle.ALIGN_LEFT);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Text value to show</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCell(string value)
        {
            return DataCell(value, Fonts.Times, Rectangle.ALIGN_LEFT);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Text value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCell(string value, Font font)
        {
            return DataCell(value, font, Rectangle.ALIGN_LEFT);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Long value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCell(long value, Font font)
        {
            return DataCell(value.ToString(CultureInfo.InvariantCulture), font, Rectangle.ALIGN_LEFT);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Long value to show</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCell(DateTime? value)
        {
            if (value == null)
            {
                return DataCell(string.Empty, Fonts.Times, Rectangle.ALIGN_CENTER);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value.Value), Fonts.Times, Rectangle.ALIGN_LEFT);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Decimal value to show in money format</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCellMoney(decimal? value)
        {
            string valueText = string.Empty;
            if (value.HasValue)
            {
                valueText = Basics.PdfMoneyFormat(value.Value);
            }

            return DataCellRight(valueText, Fonts.Times);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Decimal value to show in money format</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCellMoney(decimal? value, Font font)
        {
            string valueText = string.Empty;
            if (value.HasValue)
            {
                valueText = Basics.PdfMoneyFormat(value.Value);
            }

            return DataCellRight(valueText, font);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Decimal value to show </param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCellMoney(decimal value, Font font)
        {
            string valueText = Basics.PdfMoneyFormat(value);
            return DataCellRight(valueText, font);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">DateTime value to show </param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCell(DateTime value)
        {
            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), Fonts.Times, Rectangle.ALIGN_CENTER);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">DateTime value to show </param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCell(DateTime value, Font font)
        {
            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font, Rectangle.ALIGN_CENTER);
        }

        /// <summary>Creates a cell table with content with alignment</summary>
        /// <param name="value">DateTime value to show </param>
        /// <param name="font">Font for content</param>
        /// <param name="alignment">Content alignment</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCell(DateTime value, Font font, int alignment)
        {
            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font, alignment);
        }

        /// <summary>Creates a cell table with content alignment to center</summary>
        /// <param name="value">Text value to show</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCellCenter(string value)
        {
            return DataCell(value, Fonts.Times, Rectangle.ALIGN_CENTER);
        }

        /// <summary>Creates a cell table with content alignment to center</summary>
        /// <param name="value">Text value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCellCenter(string value, Font font)
        {
            return DataCell(value, font, Rectangle.ALIGN_CENTER);
        }

        /// <summary>Creates a cell table with content alignment to center</summary>
        /// <param name="value">DateTime value to show</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCellCenter(DateTime? value)
        {
            if (value == null)
            {
                return DataCell(string.Empty, Fonts.Times, Rectangle.ALIGN_CENTER);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), Fonts.Times, Rectangle.ALIGN_CENTER);
        }

        /// <summary>Creates a cell table with content alignment to center</summary>
        /// <param name="value">DateTime value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCellCenter(DateTime? value, Font font)
        {
            if (value == null)
            {
                return DataCell(string.Empty, font, Rectangle.ALIGN_CENTER);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font, Rectangle.ALIGN_CENTER);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">DateTime value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCell(DateTime? value, Font font)
        {
            if (value == null)
            {
                return DataCell(string.Empty, font, Rectangle.ALIGN_LEFT);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font);
        }

        /// <summary>Creates a cell table with content alignment</summary>
        /// <param name="value">DateTime value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="alignment">Content alignment</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCell(DateTime? value, Font font, int alignment)
        {
            if (value == null)
            {
                return DataCell(string.Empty, font, Rectangle.ALIGN_LEFT);
            }

            return DataCell(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", value), font, alignment);
        }

        /// <summary>Creates a cell table with content</summary>
        /// <param name="value">Text value to show</param>
        /// <param name="font">Font for content</param>
        /// <param name="alignment">Content alignment</param>
        /// <returns>Cell table</returns>
        [CLSCompliant(false)]
        public static PdfPCell DataCell(string value, Font font, int alignment)
        {
            var finalValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

            return new PdfPCell(new Phrase(finalValue, font))
            {
                Border = 0,
                BackgroundColor = BaseColor.WHITE,
                Padding = 6f,
                PaddingTop = 4f,
                HorizontalAlignment = alignment
            };
        }

        /// <summary>Replaces a field with a formatted text</summary>
        /// <param name="stamper">Stamper of document</param>
        /// <param name="fieldName">Name of field</param>
        /// <param name="pattenr">Pattern of formatted text</param>
        /// <param name="data">Data to fill into formatted text</param>
        /// <param name="font">Font of formatted text</param>
        /// <param name="fontSize">Font size of formatted text</param>
        [CLSCompliant(false)]
        public static void ReplaceField(PdfStamper stamper, string fieldName, string pattenr, string[] data, BaseFont font, int fontSize)
        {
            ReplaceField(stamper, fieldName, pattenr, data, font, font, fontSize);
        }

        /// <summary>Replaces a field with a formatted text</summary>
        /// <param name="stamper">Stamper of document</param>
        /// <param name="fieldName">Name of field</param>
        /// <param name="pattenr">Pattern of formatted text</param>
        /// <param name="data">Data to fill into formatted text</param>
        /// <param name="fontPattern">Font of static part</param>
        /// <param name="fontData">Font for dynamic part</param>
        /// <param name="fontSize">Font size of formatted text</param>
        [CLSCompliant(false)]
        public static void ReplaceField(PdfStamper stamper, string fieldName, string pattenr, string[] data, BaseFont fontPattern, BaseFont fontData, int fontSize)
        {
            if(stamper == null)
            {
                return;
            }

            var position = stamper.AcroFields.GetFieldPositions(fieldName)[0];
            var cb = stamper.GetOverContent(position.page);
            var rectangle = new Rectangle(position.position.Left, position.position.Top, position.position.Right, position.position.Bottom, 0);
            cb.Rectangle(rectangle);
            var ct = new ColumnText(cb);
            ct.SetSimpleColumn(rectangle);

            var phrase = new Phrase();
            var parts = pattenr.Split('|');

            int cont = 0;
            foreach (string dataPart in data)
            {
                if (!string.IsNullOrEmpty(parts[cont].Trim()))
                {
                    phrase.Add(new Chunk(parts[cont], new Font(fontPattern, fontSize)));
                }

                phrase.Add(new Chunk(dataPart, new Font(fontData, fontSize)));
                cont++;
            }

            if (parts.Length == cont + 1)
            {
                phrase.Add(new Chunk(parts[cont], new Font(fontPattern, fontSize)));
            }

            phrase.SetLeading(1, 1);
            ct.AddElement(phrase);
            ct.Go();
        }

        /// <summary>Creates a phrase based on pattern text</summary>
        /// <param name="pattenr">Pattern of formatted text</param>
        /// <param name="data">Data to fill into formatted text</param>
        /// <param name="fontPattern">Font of static part</param>
        /// <param name="fontData">Font for dynamic part</param>
        /// <param name="fontSize">Font size of formatted text</param>
        [CLSCompliant(false)]
        public static Paragraph FlowParagraph(string pattenr, string[] data, BaseFont fontPattern, BaseFont fontData, int fontSize)
        {
            if(string.IsNullOrEmpty(pattenr))
            {
                return new Paragraph();
            }

            var phrase = new Paragraph();
            var parts = pattenr.Split('|');

            int cont = 0;
            if(data != null)
            {
                foreach(string dataPart in data)
                {
                    if(!string.IsNullOrEmpty(parts[cont].Trim()))
                    {
                        var chunck = new Chunk(parts[cont], new Font(fontPattern, fontSize));
                        phrase.Add(chunck);
                    }

                    phrase.Add(new Chunk(dataPart, new Font(fontData, fontSize)));
                    cont++;
                }
            }

            if(parts.Length == cont + 1)
            {
                phrase.Add(new Chunk(parts[cont], new Font(fontPattern, fontSize)));
            }

            return phrase;
        }

        /// <summary>Creates a phrase based on pattern text</summary>
        /// <param name="pattenr">Pattern of formatted text</param>
        /// <param name="data">Data to fill into formatted text</param>
        /// <param name="fontPattern">Font of static part</param>
        /// <param name="fontData">Font for dynamic part</param>
        /// <param name="fontSize">Font size of formatted text</param>
        [CLSCompliant(false)]
        public static Phrase CompositeField(string pattenr, string[] data, BaseFont fontPattern, BaseFont fontData, int fontSize)
        {
            if (string.IsNullOrEmpty(pattenr))
            {
                return new Phrase();
            }

            var phrase = new Phrase();
            var parts = pattenr.Split('|');

            int cont = 0;
            if (data != null)
            {
                foreach (string dataPart in data)
                {
                    if (!string.IsNullOrEmpty(parts[cont].Trim()))
                    {
                        var chunck = new Chunk(parts[cont], new Font(fontPattern, fontSize));
                        phrase.Add(chunck);
                    }

                    phrase.Add(new Chunk(dataPart, new Font(fontData, fontSize)));
                    cont++;
                }
            }

            if (parts.Length == cont + 1)
            {
                phrase.Add(new Chunk(parts[cont], new Font(fontPattern, fontSize)));
            }

            return phrase;
        }

        /// <summary>Replaces a field with a formatted text</summary>
        /// <param name="stamper">Stamper of document</param>
        /// <param name="fieldName">Name of field</param>
        /// <param name="data">Data to fill into formatted text</param>
        /// <param name="fontData">Font for dynamic part</param>
        /// <param name="fontSize">Font size of formatted text</param>
        [CLSCompliant(false)]
        public static void ReplaceField(PdfStamper stamper, string fieldName, string data, BaseFont fontData, int fontSize)
        {
            if (stamper == null)
            {
                return;
            }

            var position = stamper.AcroFields.GetFieldPositions(fieldName)[0];
            var cb = stamper.GetOverContent(position.page);
            var rectangle = new Rectangle(position.position.Left, position.position.Top, position.position.Right, position.position.Bottom, 0);
            cb.Rectangle(rectangle);
            var ct = new ColumnText(cb);
            ct.SetSimpleColumn(rectangle);

            var phrase = new Phrase
            {
                new Chunk(data, new Font(fontData, fontSize))
            };

            ct.AddElement(phrase);
            ct.Go();
        }

        /// <summary>Replaces a field with a formatted text</summary>
        /// <param name="stamper">Stamper of document</param>
        /// <param name="value">Value to show</param>
        /// <param name="page">Page to put</param>
        /// <param name="positionX">Horizontal position</param>
        /// <param name="positionY">Vertical position</param>
        /// <param name="font">Font of text</param>
        /// <param name="fontSize">Size of font</param>
        [CLSCompliant(false)]
        public static void AbsolutePosition(PdfStamper stamper, string value, int page, float positionX, float positionY, BaseFont font, int fontSize)
        {
            if (stamper == null)
            {
                return;
            }

            var cb = stamper.GetOverContent(page);
            var rectangle = new Rectangle(positionX, positionY + 20, positionX + 20, positionY, 0);
            cb.Rectangle(rectangle);
            var ct = new ColumnText(cb);
            ct.SetSimpleColumn(rectangle);

            var phrase = new Phrase(value, new Font(font, fontSize));
            ct.AddElement(phrase);
            ct.Go();
        }

        /// <summary>Replaces a field with a formatted text</summary>
        /// <param name="stamper">Stamper of document</param>
        /// <param name="value">Value to show</param>
        /// <param name="page">Page to put</param>
        /// <param name="positionX">Horizontal position</param>
        /// <param name="positionY">Vertical position</param>
        /// <param name="width">width of rectangle</param>
        /// <param name="font">Font of text</param>
        /// <param name="fontSize">Size of font</param>
        [CLSCompliant(false)]
        public static void AbsolutePosition(PdfStamper stamper, string value, int page, float positionX, float positionY, float width, BaseFont font, int fontSize)
        {
            if (stamper == null)
            {
                return;
            }

            var cb = stamper.GetOverContent(page);
            var rectangle = new Rectangle(positionX, positionY + 20, positionX + width, positionY, 0);
            cb.Rectangle(rectangle);
            var ct = new ColumnText(cb);
            ct.SetSimpleColumn(rectangle);

            var phrase = new Phrase(value, new Font(font, fontSize));
            ct.AddElement(phrase);
            ct.Go();
        }

        /// <summary>Replaces a field with a formatted text</summary>
        /// <param name="stamper">Stamper of document</param>
        /// <param name="value">Value to show</param>
        /// <param name="page">Page to put</param>
        /// <param name="positionX">Horizontal position</param>
        /// <param name="positionY">Vertical position</param>
        /// <param name="width">width of rectangle</param>
        /// <param name="font">Font of text</param>
        /// <param name="fontSize">Size of font</param>
        [CLSCompliant(false)]
        public static void AbsolutePositionCenter(PdfStamper stamper, string value, int page, float positionX, float positionY, float width, BaseFont font, int fontSize)
        {
            if(stamper == null)
            {
                return;
            }

            var cb = stamper.GetOverContent(page);
            var rectangle = new Rectangle(positionX, positionY + 20, positionX + width, positionY, 0);
            cb.Rectangle(rectangle);
            var ct = new ColumnText(cb);
            ct.SetSimpleColumn(rectangle);

            var content = new Paragraph(value, new Font(font, fontSize));
            content.Alignment = Rectangle.ALIGN_CENTER;
            ct.Alignment = Rectangle.ALIGN_CENTER;
            ct.AddElement(content);
            ct.Go();
        }

        /// <summary>Replaces a field with a formatted text</summary>
        /// <param name="stamper">Stamper of document</param>
        /// <param name="value">Value to show</param>
        /// <param name="page">Page to put</param>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <param name="width">width of rectangle</param>
        /// <param name="font">Font of text</param>
        /// <param name="fontSize">Size of font</param>
        [CLSCompliant(false)]
        public static void AbsolutePositionRight(PdfStamper stamper, string value, int page, float x, float y, float width, BaseFont font, int fontSize)
        {
            if(stamper == null)
            {
                return;
            }

            var cb = stamper.GetOverContent(page);
            var rectangle = new Rectangle(x, y + 20, x + width, y, 0);
            cb.Rectangle(rectangle);
            var ct = new ColumnText(cb);
            ct.SetSimpleColumn(rectangle);

            var content = new Paragraph(value, new Font(font, fontSize));
            content.Alignment = Rectangle.ALIGN_RIGHT;
            ct.Alignment = Rectangle.ALIGN_RIGHT;
            ct.AddElement(content);
            ct.Go();
        }

        /// <summary>Replaces a field with a formatted text</summary>
        /// <param name="stamper">Stamper of document</param>
        /// <param name="value">Value to show</param>
        /// <param name="page">Page to put</param>
        /// <param name="x">Horizontal position</param>
        /// <param name="y">Vertical position</param>
        /// <param name="width">width of rectangle</param>
        /// <param name="font">Font of text</param>
        /// <param name="fontSize">Size of font</param>
        [CLSCompliant(false)]
        public static void AbsolutePositionLargeText(PdfStamper stamper, string value, int page, float x, float y, float width, BaseFont font, int fontSize)
        {
            if(stamper == null)
            {
                return;
            }

            var cb = stamper.GetOverContent(page);
            var rectangle = new Rectangle(x, y + 20, x + width, y - 480, 0);
            cb.Rectangle(rectangle);
            var ct = new ColumnText(cb);
            ct.SetSimpleColumn(rectangle);

            var phrase = new Phrase(value, new Font(font, fontSize));
            ct.AddElement(phrase);
            ct.Go();
        }

        /// <summary>Replaces a field with a formatted text</summary>
        /// <param name="stamper">Stamper of document</param>
        /// <param name="phrase">Text to show</param>
        /// <param name="page">Page to put</param>
        /// <param name="horizontalPosition">Horizontal position</param>
        /// <param name="verticalPosition">Vertical position</param>
        /// <param name="width">width of rectangle</param>
        /// <param name="font">Font of text</param>
        /// <param name="fontSize">Size of font</param>
        [CLSCompliant(false)]
        public static void AbsolutePositionLargeText(PdfStamper stamper, Phrase phrase, int page, float horizontalPosition, float verticalPosition, float width, BaseFont font, int fontSize)
        {
            var cb = stamper.GetOverContent(page);
            var rectangle = new Rectangle(horizontalPosition, verticalPosition + 20, horizontalPosition + width, verticalPosition - 480, 0);
            cb.Rectangle(rectangle);
            var ct = new ColumnText(cb);
            ct.SetSimpleColumn(rectangle);
            ct.AddElement(phrase);
            ct.Go();
        }

        /// <summary>Replaces a field with a formatted text</summary>
        /// <param name="stamper">Stamper of document</param>
        /// <param name="value">Value to show</param>
        /// <param name="page">Page to put</param>
        /// <param name="horizontalPosition">Horizontal position</param>
        /// <param name="verticalPosition">Vertical position</param>
        /// <param name="width">width of rectangle</param>
        /// <param name="fontSize">Size of font</param>
        [CLSCompliant(false)]
        public static void AbsolutePositionCheckBox(PdfStamper stamper, bool value, int page, float horizontalPosition, float verticalPosition, float width, int fontSize)
        {
            var fontNameCheckbox = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "webdings.ttf");
            var baseFont = BaseFont.CreateFont(fontNameCheckbox, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);


            var fontNameCheckbox2 = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "fontawesome-webfont.ttf");
            var baseFont2 = BaseFont.CreateFont(fontNameCheckbox2, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Pdf.AbsolutePosition(stamper, "c", page, horizontalPosition, verticalPosition, width, baseFont, fontSize);
            if (value)
            {
                Pdf.AbsolutePosition(stamper, "", page, horizontalPosition, verticalPosition, width, baseFont2, fontSize);
            }
        }
    }
}