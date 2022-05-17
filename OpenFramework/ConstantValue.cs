// --------------------------------
// <copyright file="ConstantValue.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3
{
    /// <summary>Implements constant values representation</summary>
    public static class ConstantValue
    {
        /// <summary>Origin native</summary>
        public const string OriginNative = "NATIVE";

        /// <summary>Origin NAVISION</summary>
        public const string OriginNavision = "NAVISION";

        /// <summary>Origin CRM</summary>
        public const string OriginCRM = "CRM";

        /// <summary>Origin SAP</summary>
        public const string OriginSAP = "SAP";

        /// <summary>Extension for Microsoft Excel files</summary>
        public static readonly string ExcelExtension = "xls";

        /// <summary>Extension for Microsoft Excel 2007 files</summary>
        public static readonly string Excel2007Extension = "xlsx";

        /// <summary>Extension for PDF files</summary>
        public static readonly string PdfExtension = "pdf";

        /// <summary>Extension for CSV files</summary>
        public static readonly string CommaSeparatedValueExtension = "csv";

        /// <summary>Name of temporal folder</summary>
        public static readonly string TemporalFolder = "Temp";

        /// <summary>JavaScript representation of null value</summary>
        public static readonly string Null = "null";

        /// <summary>SQL representation of null value</summary>
        public static readonly string SqlNull = "NULL";

        /// <summary>JavaScript representation of true value</summary>
        public static readonly string True = "true";

        /// <summary>JavaScript representation of false value</summary>
        public static readonly string False = "false";

        /// <summary>Gets JavaScript representation of boolean value</summary>
        /// <param name="booleanValue">Boolean value</param>
        /// <returns>JavaScript text</returns>
        public static string Value(bool booleanValue)
        {
            return booleanValue ? Constant.JavaScriptTrue : Constant.JavaScriptFalse;
        }

        /// <summary>Gets JavaScript representation of boolean value</summary>
        /// <param name="booleanValue">Boolean value</param>
        /// <returns>JavaScript text</returns>
        public static string Value(bool? booleanValue)
        {
            return booleanValue.HasValue ? Value(booleanValue.Value) : Constant.JavaScriptNull;
        }
    }
}