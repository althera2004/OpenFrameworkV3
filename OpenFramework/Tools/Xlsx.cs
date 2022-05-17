// --------------------------------
// <copyright file="Tools.Xlsx.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Tools
{
    using System;
    using System.Globalization;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    /// <summary>Tools to operate in an Excel document</summary>
    public static class Xlsx
    {
        /// <summary>Creates a new row</summary>
        /// <param name="sheet">Sheet to create row</param>
        /// <param name="rowIndex">Index of row</param>
        [CLSCompliant(false)]
        public static void CreateRow(XSSFSheet sheet, int rowIndex)
        {
            if (sheet == null)
            {
                return;
            }

            if (sheet.GetRow(rowIndex) == null)
            {
                sheet.CreateRow(rowIndex);
            }
        }

        /// <summary>Creates a new row</summary>
        /// <param name="sheet">Sheet to create row</param>
        /// <param name="rowIndex">Index of row</param>
        [CLSCompliant(false)]
        public static void CreateRow(HSSFSheet sheet, int rowIndex)
        {
            if (sheet == null)
            {
                return;
            }

            if (sheet.GetRow(rowIndex) == null)
            {
                sheet.CreateRow(rowIndex);
            }
        }

        /// <summary>Gets the string value of cell</summary>
        /// <param name="row">Row of cell</param>
        /// <param name="cellIndex">Index of cell in row</param>
        /// <returns>String value of cell</returns>
        [CLSCompliant(false)]
        public static string GetString(IRow row, int cellIndex)
        {
            if (row == null)
            {
                return string.Empty;
            }

            if (row.GetCell(cellIndex) == null)
            {
                return string.Empty;
            }

            return GetString(row.GetCell(cellIndex));
        }

        /// <summary>Gets value from a cell</summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="cell">Excel cell</param>
        /// <returns>Value of cell</returns>
        [CLSCompliant(false)]
        public static T GetValue<T>(ICell cell)
        {
            if (cell == null)
            {
                return (T)Convert.ChangeType(default(T), typeof(T), CultureInfo.InvariantCulture);
            }

            object cellValue = null;
            switch (typeof(T).FullName.ToUpperInvariant())
            {
                case "SYSTEM.DATETIME":
                case "SYSTEM.DATETIME?":
                    cellValue = GetDateTimeNullable(cell);
                    break;
                case "SYSTEM.BOOL":
                case "SYSTEM.BOOL?":
                    cellValue = GetBooleanValue(cell);
                    break;
                case "SYSTEM.DECIMAL":
                case "SYSTEM.DECIMAL?":
                    cellValue = GetDecimalValue(cell);
                    break;
                case "SYSTEM.STRING":
                default:
                    cellValue = GetString(cell);
                    break;
            }

            if (cellValue == null)
            {
                if (typeof(T).FullName.ToUpperInvariant().IndexOf("SYSTEM.DATETIME", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    cellValue = GetDateTimeNullable(cell);
                }

                if (typeof(T).FullName.ToUpperInvariant().IndexOf("SYSTEM.BOOL", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    cellValue = GetBooleanValue(cell);
                }

                if (typeof(T).FullName.ToUpperInvariant().IndexOf("SYSTEM.DECIMAL", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    cellValue = GetDecimalValue(cell);
                }
            }

            var valueType = typeof(T);
            valueType = Nullable.GetUnderlyingType(valueType) ?? valueType;
            return (cellValue == null || DBNull.Value.Equals(cellValue)) ? default(T) : (T)Convert.ChangeType(cellValue, valueType, CultureInfo.InvariantCulture);
        }

        /// <summary>Gets string value of a cell</summary>
        /// <param name="cell">Excel cell</param>
        /// <returns>String value of cell</returns>
        [CLSCompliant(false)]
        public static string GetString(ICell cell)
        {
            if (cell == null)
            {
                return string.Empty;
            }

            string textValue = string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    textValue = string.Empty;
                    break;
                case CellType.String:
                    textValue = cell.StringCellValue;
                    break;
                case CellType.Numeric:
                    textValue = cell.NumericCellValue.ToString();
                    break;
                case CellType.Boolean:
                    textValue = cell.BooleanCellValue ? ApplicationDictionary.Translate("Common_Yes") : ApplicationDictionary.Translate("Common_No");
                    break;
            }

            return textValue;
        }


        /// <summary>Gets string value of a cell</summary>
        /// <param name="row">Container row</param>
        /// <param name="cellIndex">Excel cell</param>
        /// <returns>Nullable datetime value of cell</returns>
        [CLSCompliant(false)]
        public static DateTime? GetDateTimeNullable(IRow row, int cellIndex)
        {
            if (row == null)
            {
                return null;
            }

            return GetDateTimeNullable(row.GetCell(cellIndex));
        }

        /// <summary>Gets date time value</summary>
        /// <param name="cell">Cell index</param>
        /// <returns>Date time value or null</returns>
        [CLSCompliant(false)]
        public static DateTime? GetDateTimeNullable(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }

            switch (cell.CellType)
            {
                case CellType.String:
                    return Tools.DateFormat.FromStringddMMyyy(cell.StringCellValue);
                case CellType.Blank:
                    return null;
                default: return cell.DateCellValue;
            }
        }

        [CLSCompliant(false)]
        public static decimal? GetDecimalValue(IRow row, int cellIndex)
        {
            if (row == null)
            {
                return null;
            }

            return GetDecimalValue(row.GetCell(cellIndex));
        }

        /// <summary>Gets decimal value</summary>
        /// <param name="cell">Cell index</param>
        /// <returns>Decimal value or null</returns>
        [CLSCompliant(false)]
        public static decimal? GetDecimalValue(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }

            switch (cell.CellType)
            {
                case CellType.Blank:
                    return null;
                case CellType.String:
                    decimal res = 0;
                    if (decimal.TryParse(cell.StringCellValue, out res))
                    {
                        return res;
                    }

                    return null;
                default: break;
            }

            if (cell.CellType == CellType.Numeric)
            {
                return Convert.ToDecimal(cell.NumericCellValue, CultureInfo.InvariantCulture);
            }

            return null;
        }

        /// <summary>Gets float value</summary>
        /// <param name="row">Container row</param>
        /// <param name="cellIndex">Cell index</param>
        /// <returns>Float value or null</returns>
        [CLSCompliant(false)]
        public static float? GetFloatValue(IRow row, int cellIndex)
        {
            if (row == null)
            {
                return null;
            }

            return GetFloatValue(row.GetCell(cellIndex));
        }

        /// <summary>Gets float value</summary>
        /// <param name="cell">Conatiner cell</param>
        /// <returns>Float value or null</returns>
        [CLSCompliant(false)]
        public static long? GetLongValue(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }

            switch (cell.CellType)
            {
                case CellType.Blank:
                    return null;
                case CellType.String:
                    long res = 0;
                    if (long.TryParse(cell.StringCellValue, out res))
                    {
                        return res;
                    }

                    return null;
                default: break;
            }

            if(cell.CellType == CellType.Numeric)
            {
                return Convert.ToInt64(cell.NumericCellValue, CultureInfo.InvariantCulture);
            }

            return null;
        }

        /// <summary>Gets float value</summary>
        /// <param name="cell">Container cell</param>
        /// <returns>Float value or null</returns>
        [CLSCompliant(false)]
        public static float? GetFloatValue(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }

            switch (cell.CellType)
            {
                case CellType.Blank:
                    return null;
                case CellType.String:
                    float res = 0;
                    if (float.TryParse(cell.StringCellValue, out res))
                    {
                        return res;
                    }

                    return null;
                default: break;
            }

            if (cell.CellType == CellType.Numeric)
            {
                return Convert.ToSingle(cell.NumericCellValue, CultureInfo.InvariantCulture);
            }

            return null;
        }

        /// <summary>Gets a boolean value from a cell</summary>
        /// <param name="cell">Cell to extract value</param>
        /// <returns>Boolean value or null</returns>
        [CLSCompliant(false)]
        public static bool? GetBooleanValue(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }

            bool? res = null;
            switch (cell.CellType)
            {
                case CellType.Boolean:
                    res = cell.BooleanCellValue;
                    break;
                case CellType.String:
                default:
                    string data = cell.StringCellValue.ToUpperInvariant();

                    if (data.Equals(Constant.JavaScriptTrue, StringComparison.OrdinalIgnoreCase))
                    {
                        res = Constant.True;
                    }

                    if (data.Equals(Constant.JavaScriptFalse, StringComparison.OrdinalIgnoreCase))
                    {
                        res = Constant.False;
                    }

                    res = null;
                    break;
            }

            return res;
        }
    }
}