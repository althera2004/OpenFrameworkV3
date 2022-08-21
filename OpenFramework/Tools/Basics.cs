// --------------------------------
// <copyright file="Tools.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    public partial class Basics
    {
        private static RandomNumberGenerator rng = RandomNumberGenerator.Create();

        public static string RandomString(int size)
        {
            var salt = new byte[size];
            rng.GetBytes(salt);
            string result = new String(System.Text.Encoding.ASCII.GetString(salt).Where(Char.IsLetter).ToArray());

            return result;
        }

        private const bool OnlyFirstLetter = true;

        public static string MonthName(int month, string language, string instanceName)
        {
            return MonthNameBase0(month - 1, language, instanceName);
        }

        public static string MonthNameBase0(int month, string language, string instanceName)
        {
            switch (month)
            {
                case 0: return ApplicationDictionary.Translate("Common_MonthName_January", language, instanceName);
                case 1: return ApplicationDictionary.Translate("Common_MonthName_February", language, instanceName);
                case 2: return ApplicationDictionary.Translate("Common_MonthName_March", language, instanceName);
                case 3: return ApplicationDictionary.Translate("Common_MonthName_April", language, instanceName);
                case 4: return ApplicationDictionary.Translate("Common_MonthName_May", language, instanceName);
                case 5: return ApplicationDictionary.Translate("Common_MonthName_June", language, instanceName);
                case 6: return ApplicationDictionary.Translate("Common_MonthName_July", language, instanceName);
                case 7: return ApplicationDictionary.Translate("Common_MonthName_August", language, instanceName);
                case 8: return ApplicationDictionary.Translate("Common_MonthName_September", language, instanceName);
                case 9: return ApplicationDictionary.Translate("Common_MonthName_October", language, instanceName);
                case 10: return ApplicationDictionary.Translate("Common_MonthName_November", language, instanceName);
                case 11: return ApplicationDictionary.Translate("Common_MonthName_December", language, instanceName);
            }

            return string.Empty;
        }

        public static string GetIPAddress
        {
            get
            {
                var context = HttpContext.Current;
                string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');
                    if (addresses.Length != 0)
                    {
                        return addresses[0];
                    }
                }

                return context.Request.ServerVariables["REMOTE_ADDR"];
            }
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static void VerifyFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public static void VerifyFolder(string folder, bool complete)
        {
            if (!complete)
            {
                VerifyFolder(folder);
            }
            else
            {
                folder = folder.Replace("\\\\", "\\");
                var fileName = Path.GetFileName(folder);
                var parts = Path.GetFullPath(folder).Replace(fileName, string.Empty).Split('\\');
                var targetFolder = string.Empty;
                foreach (var part in parts)
                {
                    targetFolder += part + "\\";
                    VerifyFolder(targetFolder);
                }
            }
        }

        /// <summary>Gets a random string for anticache purposses</summary>
        public static string AntiCache
        {
            get
            {
                var ac = ConfigurationManager.AppSettings["FrameworkVersion"].ToString();
                if (HttpContext.Current.Session["AntiCache"] != null)
                {
                    if ((bool)HttpContext.Current.Session["AntiCache"])
                    {
                        return string.Format(CultureInfo.InvariantCulture, "ac={0}", Guid.NewGuid());
                    }
                }

                return string.Format(CultureInfo.InvariantCulture, "ac={0}", ac);
            }
        }

        /// <summary>Gets a random string for anticache purposses</summary>
        public static string AntiCacheVersion
        {
            get
            {
                var ac = ConfigurationManager.AppSettings["FrameworkVersion"].ToString();
                return string.Format(CultureInfo.InvariantCulture, "ac={0}", ac);
            }
        }

        /// <summary>Gets a random string for anticache purposses</summary>
        public static string AntiCacheForced
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }

        /// <summary>Gets a random string for anticache purposses</summary>
        public static bool AntiCacheEnabled
        {
            get
            {
                if (HttpContext.Current.Session["AntiCache"] != null)
                {
                    if ((bool)HttpContext.Current.Session["AntiCache"])
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public static string DebugTime(string label, DateTime fromTime)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{2}\t{0}:{1}",
                label,
                (DateTime.Now - fromTime).TotalMilliseconds,
                Environment.NewLine);
        }

        public static string DebugTime(string label, DateTime fromTime, DateTime toTime)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{2}\t{0}:{1}",
                label,
                (toTime - fromTime).TotalMilliseconds,
                Environment.NewLine);
        }

        public static string LabelToIdentifier(string label)
        {
            if (string.IsNullOrEmpty(label))
            {
                return string.Empty;
            }

            return label.Replace(" ", "_");
        }

        public static string Base64Decode(string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
            {
                return string.Empty;
            }

            if (base64EncodedData.EndsWith("%3d", StringComparison.Ordinal))
            {
                base64EncodedData = base64EncodedData.Replace("%3d", "=");
            }

            if (base64EncodedData.EndsWith("====", StringComparison.Ordinal))
            {
                base64EncodedData = base64EncodedData.Replace("====", "==");
            }

            if (base64EncodedData.EndsWith("===", StringComparison.Ordinal))
            {
                base64EncodedData = base64EncodedData.Replace("===", "==");
            }

            var res = string.Empty;
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                res = Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch
            {
                var base64EncodedBytes = Convert.FromBase64String(base64EncodedData.Replace("=", ""));
                res = Encoding.UTF8.GetString(base64EncodedBytes);
            }

            return res;
        }

        /// <summary>Creates resume for a text</summary>
        /// <param name="text">Text to resume</param>
        /// <param name="length">Maximum length</param>
        /// <returns>Resume of text</returns>
        public static string Resume(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= length)
            {
                return text;
            }

            text = text.Substring(0, length);
            int pos = text.LastIndexOf(' ');
            if (pos != -1)
            {
                text = text.Substring(0, pos);
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}...", text);
        }

        public static string LimitLength(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length < length)
            {
                return text;
            }

            return text.Substring(0, length);
        }

        public static string Capitalize(string value)
        {
            return Capitalize(value, OnlyFirstLetter);
        }

        public static string Capitalize(string value, bool onlyFirst)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = string.Empty;
            }

            value = value.Trim();
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (value.Length == 1)
            {
                return value.ToUpperInvariant();
            }

            value = value.ToLowerInvariant();

            if (onlyFirst)
            {
                return value.Substring(0, 1).ToUpperInvariant() + value.Substring(1).ToLowerInvariant();
            }

            var res = new StringBuilder();
            foreach (string word in value.Split(' '))
            {
                res.Append(Capitalize(word)).Append(" ");
            }

            return res.ToString().Trim();
        }

        /// <summary>Make a string with decimal format for pdf purposses</summary>
        /// <param name="value">Decimal value</param>
        /// <returns>String with decimal format</returns>
        public static string PdfMoneyFormat(decimal value)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", value).Replace(",", "*").Replace(".", ",").Replace("*", ".");
        }

        /// <summary>Make a string with decimal format</summary>
        /// <param name="value">Decimal value</param>
        /// <returns>String with decimal format</returns>
        public static string NumberFormat(decimal? value)
        {
            if (value.HasValue)
            {
                return NumberFormat(value.Value);
            }

            return string.Empty;
        }

        /// <summary>Make a string with decimal format</summary>
        /// <param name="value">Decimal value</param>
        /// <returns>String with decimal format</returns>
        public static string NumberFormat(decimal value)
        {
            string res = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.000000}", value);
            while (res.EndsWith("0"))
            {
                res = res.Substring(0, res.Length - 1);
            }

            if (res.EndsWith("."))
            {
                res = res.Substring(0, res.Length - 1);
            }

            return res.Replace(".", ",");
        }

        /// <summary>Prepare a texto to be HTML compliant</summary>
        /// <param name="text">Text to Htmlize</param>
        /// <returns>Html compliant text</returns>
        public static string SetHtml(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            string res = text.Replace("\"", "&quot;");
            res = res.Replace("&", "&amp;");
            return res;
        }

        /// <summary>Prepares a text to be a tooltip</summary>
        /// <param name="text">Texto to prepare</param>
        /// <returns>Text ready for to be a tooltip</returns>
        public static string SetTooltip(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text.Replace("\"", "˝");
        }

        /// <summary>Replace quote for literal quote</summary>
        /// <param name="text">Original text</param>
        /// <returns>Text with literal quotes</returns>
        public static string LiteralQuote(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text.Replace('\'', '´');
        }

        /// <summary>Gets trasnlated month name</summary>
        /// <param name="monthNumber">Month number</param>
        /// <param name="dictionary">Dictionary for month names</param>
        /// <returns>Translated month name</returns>
        public static string TranslatedMonth(int monthNumber, Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return string.Empty;
            }

            switch (monthNumber)
            {
                case 1:
                    return dictionary["Common_MonthName_January"];
                case 2:
                    return dictionary["Common_MonthName_February"];
                case 3:
                    return dictionary["Common_MonthName_March"];
                case 4:
                    return dictionary["Common_MonthName_April"];
                case 5:
                    return dictionary["Common_MonthName_May"];
                case 6:
                    return dictionary["Common_MonthName_June"];
                case 7:
                    return dictionary["Common_MonthName_July"];
                case 8:
                    return dictionary["Common_MonthName_August"];
                case 9:
                    return dictionary["Common_MonthName_September"];
                case 10:
                    return dictionary["Common_MonthName_October"];
                case 11:
                    return dictionary["Common_MonthName_November"];
                case 12:
                    return dictionary["Common_MonthName_December"];
                default:
                    return string.Empty;
            }
        }

        /// <summary>Search the text on dictionary</summary>
        /// <param name="text">Dictionary key</param>
        /// <returns>Translated text by dictionary, or original text if not found</returns>
        public static string Translate(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            if (dictionary.ContainsKey(text))
            {
                return dictionary[text];
            }

            return text;
        }

        public static string NormalizeFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return string.Empty;
            }

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    fileName = fileName.Replace(c, '_');
                }
            }

            return fileName.Replace('#', '_').Replace('%', '_');
        }

        /// <summary>Converts degree to radian</summary>
        /// <param name="angle">Angle on degrees</param>
        /// <returns>Angle on radians</returns>
        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        /// <summary>Converts radian to degree</summary>
        /// <param name="angle">Angle on radians</param>
        /// <returns>Angle on degrees</returns>
        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        ///// <summary>Gets name of month</summary>
        ///// <param name="index">Index of month</param>
        ///// <param name="languageCode">Code on laguange to translate</param>
        ///// <returns>Name of month</returns>
        //public static string MonthName(int index, string language, string instanceName)
        //{
        //    switch (index)
        //    {
        //        case Constant.Month.January + 1: return ApplicationDictionary.Translate("Common_MonthName_January", language, instanceName);
        //        case Constant.Month.February + 1: return ApplicationDictionary.Translate("Common_MonthName_February", language, instanceName);
        //        case Constant.Month.March + 1: return ApplicationDictionary.Translate("Common_MonthName_March", language, instanceName);
        //        case Constant.Month.April + 1: return ApplicationDictionary.Translate("Common_MonthName_April", language, instanceName);
        //        case Constant.Month.May + 1: return ApplicationDictionary.Translate("Common_MonthName_May", language, instanceName);
        //        case Constant.Month.June + 1: return ApplicationDictionary.Translate("Common_MonthName_June", language, instanceName);
        //        case Constant.Month.July + 1: return ApplicationDictionary.Translate("Common_MonthName_July", language, instanceName);
        //        case Constant.Month.August + 1: return ApplicationDictionary.Translate("Common_MonthName_August", language, instanceName);
        //        case Constant.Month.September + 1: return ApplicationDictionary.Translate("Common_MonthName_September", language, instanceName);
        //        case Constant.Month.October + 1: return ApplicationDictionary.Translate("Common_MonthName_October", language, instanceName);
        //        case Constant.Month.November + 1: return ApplicationDictionary.Translate("Common_MonthName_November", language, instanceName);
        //        case Constant.Month.December + 1: return ApplicationDictionary.Translate("Common_MonthName_December", language, instanceName);
        //        default: return string.Empty;
        //    }
        //}

        public static string ServerName(string request)
        {
            var uri = new Uri(request);
            return uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
        }

        public static bool ValidateDate(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            var data = value.Split('/');
            if (data.Length != 3)
            {
                return false;
            }

            try
            {
                int day = Convert.ToInt32(data[0], CultureInfo.InvariantCulture);
                int month = Convert.ToInt32(data[1], CultureInfo.InvariantCulture);
                int year = Convert.ToInt32(data[2], CultureInfo.InvariantCulture);
                var probeDate = new DateTime(year, month, day);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>Validates email format</summary>
        /// <param name="emailAddress">Text to validate</param>
        /// <returns>Boolean indicating if text if a valid email</returns>
        public static bool EmailIsValid(string emailAddress)
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase).IsMatch(emailAddress);
        }

        /// <summary>Clone an object</summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="source">Object to be cloned</param>
        /// <returns>Copy of source object</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", source.GetType().ToString());
            }

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>Extract characters from right of text</summary>
        /// <param name="text">Text where extract</param>
        /// <param name="length">Number of characters to extract</param>
        /// <returns>String value</returns>
        public static string Right(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= length)
            {
                return text;
            }

            return text.Substring(text.Length - length, length);
        }

        /// <summary>Extract characters from left of text</summary>
        /// <param name="text">Text where extract</param>
        /// <param name="length">Number of characters to extract</param>
        /// <returns>String value</returns>
        public static string Left(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= length)
            {
                return text;
            }

            return text.Substring(0, length);
        }

        /// <summary>Calculate seconds in a string time formatted</summary>
        /// <param name="time">Time formatted</param>
        /// <returns>Number of seconds represented in time formatted</returns>
        public static int? ParseTime(string time)
        {
            if (string.IsNullOrEmpty(time))
            {
                return null;
            }

            // Si no se especifica unidad se asumen que son segundos
            if (int.TryParse(time, out int value))
            {
                return value;
            }

            string timeUnit = Right(time, 1);
            string timeValue = time.Substring(0, time.Length - 1);
            if (!int.TryParse(timeValue, out value))
            {
                return null;
            }

            switch (timeUnit.ToUpperInvariant())
            {
                case "S":
                    return value;
                case "M":
                    return value * 60;
                case "H":
                    return value * 3600;
                default:
                    return null;
            }
        }

        /// <summary>Calculates month name from a date</summary>
        /// <param name="date">Date to get month name from</param>
        /// <param name="languageCode">Code of laguage to translate month names</param>
        /// <returns>Name of month</returns>
        public static string MonthDate(DateTime date, string language, string instanceName)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), "{0}/{1}", MonthName(date.Month, language, instanceName), date.Year);
        }

        /// <summary>Calculates if two dates are in the same month of the same year</summary>
        /// <param name="firstDate">First date on comparison</param>
        /// <param name="secondDate">Second date on comparison</param>
        /// <returns>Indicates if two dates are in the same month of the same year</returns>
        public static bool SameMonth(DateTime firstDate, DateTime secondDate)
        {
            if (firstDate.Month != secondDate.Month)
            {
                return false;
            }

            if (firstDate.Year != secondDate.Year)
            {
                return false;
            }

            return true;
        }

        /// <summary>Gets a limited length string</summary>
        /// <param name="text">Original string</param>
        /// <param name="length">Maximum length</param>
        /// <returns>A limited length string</returns>
        public static string LimitedText(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= length)
            {
                return text;
            }

            return text.Substring(0, length);
        }

        /// <summary>Gets a limited length string</summary>
        /// <param name="text">Original string</param>
        /// <param name="length">Maximum length</param>
        /// <param name="ellipsis">Shows ellipsis on reduce</param>
        /// <returns>A limited length string</returns>
        public static string LimitedText(string text, int length, bool ellipsis)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= length)
            {
                return text;
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", text.Substring(0, length), ellipsis ? "..." : string.Empty);
        }

        public static string SpanishMoney(decimal value)
        {
            return string.Format(CultureInfo.GetCultureInfo("es-es"), "{0:#,##0.00}", value);
        }

        public static string Ident(int length)
        {
            string res = string.Empty;
            length = length * 4;
            for (int x = 0; x < length; x++)
            {
                res += " ";
            }

            return res;
        }

        public static string HtmlLabel(string label)
        {
            if (string.IsNullOrEmpty(label))
            {
                return string.Empty;
            }

            string res = label.Replace("Á", "&Aacute;");
            res = res.Replace("É", "&Eacute;");
            res = res.Replace("Í", "&Iacute;");
            res = res.Replace("Ó", "&Oacute;");
            res = res.Replace("Ú", "&Uacute;");
            res = res.Replace("á", "&aacute;");
            res = res.Replace("é", "&eacute;");
            res = res.Replace("í", "&iacute;");
            res = res.Replace("ó", "&oacute;");
            res = res.Replace("ú", "&uacute;");
            res = res.Replace("À", "&Agrave;");
            res = res.Replace("È", "&Egrave;");
            res = res.Replace("Ì", "&Igrave;");
            res = res.Replace("Ò", "&Ograve;");
            res = res.Replace("Ù", "&Ugrave;");
            res = res.Replace("à", "&agrave;");
            res = res.Replace("è", "&egrave;");
            res = res.Replace("ì", "&igrave;");
            res = res.Replace("ò", "&ograve;");
            res = res.Replace("ù", "&ugrave;");
            res = res.Replace("ñ", "&ntilde;");
            res = res.Replace("Ñ", "&Ntilde;");
            res = res.Replace("ç", "&ccedil;");
            res = res.Replace("Ç", "&Ccedil;");
            return res;
        }

        public static string Base64Encode(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
            {
                return string.Empty;
            }

            var plainTextBytes = Encoding.UTF8.GetBytes(plaintext);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string PathToUrl(string path)
        {
            return path.ToLowerInvariant().Replace(HttpContext.Current.Request.PhysicalApplicationPath.ToLowerInvariant(), "/").Replace("\\", "/").Replace("//", "/");
        }

        public static string PathToUrl(string path, bool antiCache)
        {
            return path.ToLowerInvariant().Replace(HttpContext.Current.Request.PhysicalApplicationPath.ToLowerInvariant(), "/").Replace("\\", "/").Replace("//", "/") + "?" + Guid.NewGuid().ToString();
        }

        public static string TimeAgo(DateTime date)
        {
            var span = DateTime.Now - date;
            if (span.Days > 365)
            {
                int years = span.Days / 365;
                if (span.Days % 365 != 0)
                {
                    years += 1;
                }

                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    "about {0} {1} ago",
                    years,
                    years == 1 ? "year" : "years");
            }

            if (span.Days > 30)
            {
                int months = span.Days / 30;
                if (span.Days % 31 != 0)
                {
                    months += 1;
                }

                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    "about {0} {1} ago",
                    months,
                    months == 1 ? "month" : "months");
            }

            if (span.Days > 0)
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    "about {0} {1} ago",
                    span.Days,
                    span.Days == 1 ? "day" : "days");
            }

            if (span.Hours > 0)
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    "about {0} {1} ago",
                    span.Hours,
                    span.Hours == 1 ? "hour" : "hours");
            }

            if (span.Minutes > 0)
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    "about {0} {1} ago",
                    span.Minutes,
                    span.Minutes == 1 ? "minute" : "minutes");
            }

            if (span.Seconds > 5)
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), "about {0} seconds ago", span.Seconds);
            }

            if (span.Seconds <= 5)
            {
                return "just now";
            }

            return string.Empty;
        }

        public static string CapitalizePhrase(string text)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            // Return char and concat substring.
            return char.ToUpper(text[0], CultureInfo.InvariantCulture) + text.Substring(1);
        }

        public static string NumeroALetras(decimal numberAsString)
        {
            string dec = string.Empty;
            var entero = Convert.ToInt64(Math.Truncate(numberAsString));

            var decimales = Convert.ToInt32(Math.Round((numberAsString - entero) * 100, 2));
            if (decimales > 0)
            {
                dec = NumeroALetrasDecimal(Convert.ToDecimal(decimales));
            }
            else
            {
                dec = NumeroALetrasDecimal(Convert.ToDecimal(decimales));
            }

            var res = NumeroALetras(Convert.ToDouble(entero)) + " con " + dec;
            return res;
        }

        /// <summary>Generates text for decimal part of value</summary>
        /// <param name="numberAsString">Decimal part of number</param>
        /// <returns></returns>
        public static string NumeroALetrasDecimal(decimal numberAsString)
        {
            string dec = string.Empty;
            var entero = Convert.ToInt64(Math.Truncate(numberAsString));
            if (entero == 0)
            {
                return string.Empty;
            }

            var res = NumeroALetras(Convert.ToDouble(entero));
            return res;
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private static string NumeroALetras(double value)
        {
            string num2Text;
            value = Math.Truncate(value);
            if (value == 0) { num2Text = "CERO"; }
            else if (value == 1) { num2Text = "UNO"; }
            else if (value == 2) { num2Text = "DOS"; }
            else if (value == 3) { num2Text = "TRES"; }
            else if (value == 4) { num2Text = "CUATRO"; }
            else if (value == 5) { num2Text = "CINCO"; }
            else if (value == 6) { num2Text = "SEIS"; }
            else if (value == 7) { num2Text = "SIETE"; }
            else if (value == 8) { num2Text = "OCHO"; }
            else if (value == 9) { num2Text = "NUEVE"; }
            else if (value == 10) { num2Text = "DIEZ"; }
            else if (value == 11) { num2Text = "ONCE"; }
            else if (value == 12) { num2Text = "DOCE"; }
            else if (value == 13) { num2Text = "TRECE"; }
            else if (value == 14) { num2Text = "CATORCE"; }
            else if (value == 15) { num2Text = "QUINCE"; }
            else if (value < 20) { num2Text = "DIECI" + NumeroALetras(value - 10); }
            else if (value == 20) { num2Text = "VEINTE"; }
            else if (value < 30) { num2Text = "VEINTI" + NumeroALetras(value - 20); }
            else if (value == 30) { num2Text = "TREINTA"; }
            else if (value == 40) { num2Text = "CUARENTA"; }
            else if (value == 50) { num2Text = "CINCUENTA"; }
            else if (value == 60) { num2Text = "SESENTA"; }
            else if (value == 70) { num2Text = "SETENTA"; }
            else if (value == 80) { num2Text = "OCHENTA"; }
            else if (value == 90) { num2Text = "NOVENTA"; }
            else if (value < 100) { num2Text = NumeroALetras(Math.Truncate(value / 10) * 10) + " Y " + NumeroALetras(value % 10); }
            else if (value == 100) { num2Text = "CIEN"; }
            else if (value < 200) { num2Text = "CIENTO " + NumeroALetras(value - 100); }
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) { num2Text = NumeroALetras(Math.Truncate(value / 100)) + "CIENTOS"; }
            else if (value == 500) { num2Text = "QUINIENTOS"; }
            else if (value == 700) { num2Text = "SETECIENTOS"; }
            else if (value == 900) { num2Text = "NOVECIENTOS"; }
            else if (value < 1000) num2Text = NumeroALetras(Math.Truncate(value / 100) * 100) + " " + NumeroALetras(value % 100);
            else if (value == 1000) { num2Text = "MIL"; }
            else if (value < 2000) { num2Text = "MIL " + NumeroALetras(value % 1000); }
            else if (value < 1000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value % 1000);
                }
            }
            else if (value == 1000000)
            {
                num2Text = "UN MILLON";
            }
            else if (value < 2000000)
            {
                num2Text = "UN MILLON " + NumeroALetras(value % 1000000);
            }
            else if (value < 1000000000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000) * 1000000);
                }
            }
            else if (value == 1000000000000) { num2Text = "UN BILLON"; }
            else if (value < 2000000000000) { num2Text = "UN BILLON " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000); }
            else
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
                }
            }

            return num2Text;
        }
    }
}