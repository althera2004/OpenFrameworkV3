// --------------------------------
// <copyright file="Json.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Tools
{
    using System;
    using System.Globalization;
    using OpenFrameworkV3.Core.ItemManager;

    public static class Json
    {
        /// <summary>Text for JavaScript empty list value</summary>
        public const string EmptyJsonList = "[]";

        /// <summary>Text for JavaScript empty json object value</summary>
        public const string EmptyJsonObject = "{}";

        public static string JsonCompliant(float value, int decimals)
        {
            var pattenr = "{0:#0";
            if (decimals > 0)
            {
                pattenr += ".";
                for (var x = 0; x < decimals; x++)
                {
                    pattenr += "0";
                }
            }

            pattenr += "}";
            return string.Format(CultureInfo.InvariantCulture, pattenr, value);
        }

        public static string JsonCompliant(double value, int decimals)
        {
            var pattenr = "{0:#0";
            if (decimals > 0)
            {
                pattenr += ".";
                for (var x = 0; x < decimals; x++)
                {
                    pattenr += "0";
                }
            }

            pattenr += "}";
            return string.Format(CultureInfo.InvariantCulture, pattenr, value);
        }

        public static string JsonCompliant(decimal value, int decimals)
        {
            var pattenr = "{0:#0";
            if (decimals > 0)
            {
                pattenr += ".";
                for (var x = 0; x < decimals; x++)
                {
                    pattenr += "0";
                }
            }

            pattenr += "}";
            return string.Format(CultureInfo.InvariantCulture, pattenr, value);
        }

        /// <summary>Create a JSON compliant text</summary>
        /// <param name="text">Original text</param>
        /// <returns>JSON compliant text</returns>
        public static string JsonCompliant(string text)
        {
            if (text == null)
            {
                return Constant.JavaScriptNull;
            }

            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            text = text.Replace("\n", "\\n");
            text = text.Replace("\\r", string.Empty);
            return text.Replace("\"", "\\\"");
        }

        /// <summary>Create a JSON compliant text</summary>
        /// <param name="value">Original object to normalize</param>
        /// <returns>JSON compliant text</returns>
        public static string JsonCompliant(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            string text = value.ToString();
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text.Replace("\n", "\\n").Replace("\"", "\\\"");
        }

        /// <summary>Create a JSON key with a null value</summary>
        /// <param name="key">Name of the key</param>
        /// <returns>JSON compliant text</returns>
        public static string JsonNull(string key)
        {
            return string.Format(CultureInfo.InvariantCulture, @"""{0}"": null", key);
        }

        /// <summary>Create a JSON pair key/value from a BaseItem value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">BaseItem value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, BaseItem value)
        {
            if (value == null)
            {
                return "\"" + key + "\":null";
            }

            return "\"" + key + "\":" + value.JsonKeyValue;
        }

        /// <summary>Create a JSON pair key/value from a string value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">String value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, string value)
        {
            return string.Format(CultureInfo.InvariantCulture, @"""{0}"":""{1}""", key, JsonCompliant(value));
        }

        /// <summary>Create a JSON pair key/value from a string value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">String value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, bool value)
        {
            return string.Format(CultureInfo.InvariantCulture, @"""{0}"":{1}", key, ConstantValue.Value(value));
        }

        /// <summary>Create a JSON pair key/value from a DateTime value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">DateTime value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, DateTime value)
        {
            return string.Format(CultureInfo.InvariantCulture, @"""{0}"":{1:yyyyMMdd}", key, value);
        }

        /// <summary>Create a JSON pair key/value from a integer value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Integer value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, int value)
        {
            return string.Format(CultureInfo.InvariantCulture, @"""{0}"":{1}", key, value);
        }

        /// <summary>Create a JSON pair key/value from a long value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Long value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, long value)
        {
            return string.Format(CultureInfo.InvariantCulture, @"""{0}"":{1}", key, value);
        }

        /// <summary>Create a JSON pair key/value from a decimal value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Decimal value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, decimal value)
        {
            return string.Format(CultureInfo.InvariantCulture, @"""{0}"":{1:#0.00}", key, value);
        }

        /// <summary>Create a JSON pair key/value from a decimal value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Decimal value</param>
        /// <param name="decimals">Number of decimals</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, decimal value, int decimals)
        {
            string pattern = @"""{0}"":{1:#0.";
            for (int x = 0; x < decimals; x++)
            {
                pattern += "0";
            }

            pattern += "}";
            return string.Format(CultureInfo.InvariantCulture, pattern, key, value);
        }

        /// <summary>Create a JSON pair key/value from a decimal nullable value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Decimal value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, decimal? value)
        {
            if (value.HasValue)
            {
                return string.Format(CultureInfo.InvariantCulture, @"""{0}"":{1:#0.00}", key, value);
            }

            return string.Format(CultureInfo.InvariantCulture, @"""{0}"":null", key);
        }

        /// <summary>Create a JSON pair key/value from a decimal nullable value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Decimal value</param>
        /// <param name="decimals">Number of decimals</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, decimal? value, int decimals)
        {
            if (value.HasValue)
            {
                string pattern = @"""{0}"":{1:#0.";
                for (int x = 0; x < decimals; x++)
                {
                    pattern += "0";
                }

                pattern += "}";
                return string.Format(CultureInfo.InvariantCulture, pattern, key, value);
            }

            return string.Format(CultureInfo.InvariantCulture, @"""{0}"":null", key);
        }

        /// <summary>Create a JSON pair key/value from a Double value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Double value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, double value)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"":{1:#0.00}", key, value);
        }

        /// <summary>Create a JSON pair key/value from a string value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">String value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, DateTime? value)
        {
            if (value == null)
            {
                return string.Format(CultureInfo.InvariantCulture, @"""{0}"":null", key);
            }

            return string.Format(CultureInfo.InvariantCulture, @"""{0}"":{1:yyyyMMdd}", key, value);
        }
    }
}