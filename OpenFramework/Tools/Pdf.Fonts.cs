// --------------------------------
// <copyright file="ToolsPdf.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Tools
{
    using System;
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    /// <summary>Implements tools for PDF documents creation</summary>
    public static partial class Pdf
    {
        /// <summary>Available fonts for pdf documents</summary>
        public static class Fonts
        {
            /// <summary>Base font for headers</summary>
            [CLSCompliant(false)]
            public static readonly BaseFont HeaderFont = BaseFont.CreateFont(FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            /// <summary>Base font for type Arial</summary>
            [CLSCompliant(false)]
            public static readonly BaseFont Arial = BaseFont.CreateFont(FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            /// <summary>Base font for type Awesome (icons)</summary>
            [CLSCompliant(false)]
            public static readonly BaseFont AwesomeFont = BaseFont.CreateFont(FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            /// <summary>Base font for data</summary>
            [CLSCompliant(false)]
            public static readonly BaseFont DataFont = BaseFont.CreateFont(FontPath, BaseFont.CP1250, BaseFont.EMBEDDED);

            /// <summary>Gets Times bold font</summary>
            [CLSCompliant(false)]
            public static readonly Font TimesBold = new Font(Arial, 8, Font.BOLD, BaseColor.BLACK);

            /// <summary>Gets Times font</summary>
            [CLSCompliant(false)] 
            public static readonly Font Times = new Font(DataFont, 10, Font.NORMAL, BaseColor.BLACK);

            /// <summary>Gets Times font for titles</summary>
            [CLSCompliant(false)]
            public static readonly Font TitleFont = new Font(Arial, 18, Font.BOLD, BaseColor.BLACK);
        }
    }
}