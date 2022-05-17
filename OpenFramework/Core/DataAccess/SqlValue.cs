// --------------------------------
// <copyright file="SqlValue.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.DataAccess
{
    using System;
    using System.Globalization;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Enums;
    using OpenFrameworkV3.Core.ItemManager;

    /// <summary>Implements SQLValue class</summary>
    public static class SqlValue
    {
        /// <summary>Gets the SQL value representation of field</summary>
        /// <param name="field">Field to extract value</param>
        /// <param name="data">Value of field</param>
        /// <returns>The SQL value representation of field</returns>
        public static string Value(ItemField field, object data)
        {
            if (field == null || data == null)
            {
                return Constant.SqlNull;
            }

            try
            {
                if (string.IsNullOrEmpty(data.ToString()) || data.ToString() == ConstantValue.Null)
                {
                    return Constant.SqlNull;
                }

                switch (field.DataType)
                {
                    case ItemFieldDataType.Boolean:
                    case ItemFieldDataType.NullableBoolean:
                        return Value(data.ToString().ToUpperInvariant() == ConstantValue.True.ToUpperInvariant());
                    case ItemFieldDataType.Integer:
                    case ItemFieldDataType.NullableInteger:
                    case ItemFieldDataType.ImageGallery:
                    case ItemFieldDataType.Long:
                    case ItemFieldDataType.NullableLong:
                        return Value(Convert.ToInt64(data, CultureInfo.GetCultureInfo("en-us")));
                    case ItemFieldDataType.Money:
                    case ItemFieldDataType.Decimal:
                        if(data.GetType().Name.Equals("decimal", StringComparison.OrdinalIgnoreCase) ||
                            data.GetType().Name.Equals("int32", StringComparison.OrdinalIgnoreCase))
                        {
                            return Value(Convert.ToDecimal(data.ToString()));
                        }

                        if (string.IsNullOrEmpty(data as string))
                        {
                            return "0";
                        }

                        return Value(Convert.ToDecimal(data, CultureInfo.GetCultureInfo("en-us")));
                    case ItemFieldDataType.NullableMoney:
                    case ItemFieldDataType.NullableDecimal:
                        if (string.IsNullOrEmpty(data.ToString()) || data.ToString() == ConstantValue.Null)
                        {
                            return ConstantValue.SqlNull;
                        }

                        if (data.GetType().Name.ToUpperInvariant() == "STRING")
                        {
                            string dataText = data as string;
                            if (dataText.StartsWith("NaN,0", StringComparison.OrdinalIgnoreCase))
                            {
                                return ConstantValue.SqlNull;
                            }

                            data = data.ToString().Replace("%", string.Empty).Trim();
                        }

                        return Value(Convert.ToDecimal(data, CultureInfo.GetCultureInfo("en-us")));
                    case ItemFieldDataType.DateTime:
                    case ItemFieldDataType.Time:
                    case ItemFieldDataType.NullableDateTime:
                    case ItemFieldDataType.NullableTime:

                        if (data.GetType().Name.Equals("DateTime", StringComparison.OrdinalIgnoreCase))
                        {
                            var date = (DateTime)data;
                            date = date.AddHours(1);
                            return string.Format(CultureInfo.GetCultureInfo("en-us"), "'{0:yyyy/MM/dd}'", date);
                        }

                        return Value(Convert.ToDateTime(data.ToString(), CultureInfo.GetCultureInfo("es-es")));
                    case ItemFieldDataType.Text:
                    default:
                        string finalValue = data.ToString();
                        finalValue = finalValue.Replace("'", "''");
                        return string.Format(CultureInfo.InvariantCulture, @"N'{0}'", finalValue);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Trace(ex as Exception, "OpenFramework.Core.SqlValue.Value(SQL VALUE:" + field.Name + " -->" + data.ToString());
                throw new Exception("SQL VALUE:" + field.Name + " -->" + data.ToString(), ex);
            }
        }

        /// <summary>Gets the SQL representation</summary>
        /// <param name="data">Value to parse</param>
        /// <returns>The SQL representation</returns>
        public static string Value(string data)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"'{0}'", data);
        }

        /// <summary>Gets the SQL representation</summary>
        /// <param name="data">Value to parse</param>
        /// <returns>The SQL representation</returns>
        public static string Value(bool data)
        {
            return data ? "1" : "0";
        }

        /// <summary>Gets the SQL representation</summary>
        /// <param name="data">Value to parse</param>
        /// <returns>The SQL representation</returns>
        public static string Value(bool? data)
        {
            if (data == null)
            {
                return ConstantValue.Null;
            }

            return data.Value ? "1" : "0";
        }

        /// <summary>Gets the SQL representation</summary>
        /// <param name="data">Value to parse</param>
        /// <returns>The SQL representation</returns>
        public static string Value(int data)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:0}", data);
        }

        /// <summary>Gets the SQL representation</summary>
        /// <param name="data">Value to parse</param>
        /// <returns>The SQL representation</returns>
        public static string Value(int? data)
        {
            if (data == null)
            {
                return ConstantValue.Null;
            }

            return string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:0}", data.Value);
        }

        /// <summary>Gets the SQL representation</summary>
        /// <param name="data">Value to parse</param>
        /// <returns>The SQL representation</returns>
        public static string Value(long data)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:0}", data);
        }

        /// <summary>Gets the SQL representation</summary>
        /// <param name="data">Value to parse</param>
        /// <returns>The SQL representation</returns>
        public static string Value(long? data)
        {
            if (data == null)
            {
                return ConstantValue.Null;
            }

            return string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:0}", data.Value);
        }

        /// <summary>Gets the SQL representation</summary>
        /// <param name="data">Value to parse</param>
        /// <returns>The SQL representation</returns>
        public static string Value(float data)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:0}", data);
        }

        /// <summary>Gets the SQL representation</summary>
        /// <param name="data">Value to parse</param>
        /// <returns>The SQL representation</returns>
        public static string Value(float? data)
        {
            if (data == null)
            {
                return ConstantValue.Null;
            }

            return string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:0.000}", data.Value);
        }

        /// <summary>Gets the SQL representation</summary>
        /// <param name="data">Value to parse</param>
        /// <returns>The SQL representation</returns>
        public static string Value(decimal data)
        {
            int decimalCount = BitConverter.GetBytes(decimal.GetBits(data)[3])[2];
            string decimals = string.Empty;
            for (int i = 0; i < decimalCount; i++)
            {
                decimals += "0";
            }

            string res = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{0:0.{0}}}", decimals);
            return string.Format(CultureInfo.GetCultureInfo("en-us"), res, data);
        }

        /// <summary>Gets the SQL representation</summary>
        /// <param name="data">Value to parse</param>
        /// <returns>The SQL representation</returns>
        public static string Value(decimal? data)
        {
            if (data == null)
            {
                return ConstantValue.Null;
            }

            int decimalCount = BitConverter.GetBytes(decimal.GetBits(data.Value)[3])[2];
            string decimals = string.Empty;
            for (int i = 0; i < decimalCount; i++)
            {
                decimals += "0";
            }

            string res = string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{0:0.{0}}}", decimals);
            return string.Format(CultureInfo.GetCultureInfo("en-us"), res, data.Value);
        }

        /// <summary>Gets the SQL representation</summary>
        /// <param name="data">Value to parse</param>
        /// <returns>The SQL representation</returns>
        public static string Value(byte data)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:0}", data);
        }

        /// <summary>Gets the SQL representation</summary>
        /// <param name="data">Value to parse</param>
        /// <returns>The SQL representation</returns>
        public static string Value(byte? data)
        {
            if (data == null)
            {
                return ConstantValue.Null;
            }

            return string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:0}", data.Value);
        }

        /// <summary>Gets the SQL representation</summary>
        /// <param name="data">Value to parse</param>
        /// <returns>The SQL representation</returns>
        public static string Value(DateTime data)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), "'{0:yyyy/MM/dd}'", data);
        }

        /// <summary>Gets the SQL representation</summary>
        /// <param name="data">Value to parse</param>
        /// <returns>The SQL representation</returns>
        public static string Value(DateTime? data)
        {
            if (data == null)
            {
                return ConstantValue.Null;
            }

            return string.Format(CultureInfo.GetCultureInfo("en-us"), "'{0:yyyy/MM/dd}'", data.Value);
        }
    }
}