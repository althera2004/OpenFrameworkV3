﻿// --------------------------------
// <copyright file="BasicsDateFormat.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Tools
{
    using System;
    using System.Globalization;
    using System.Linq;

    public static class DateFormat
    {
        public static DateTime? FromString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if(value.IndexOf('Z') != -1)
            {
                return FromStringZulu(value);
            }
            else if(value.IndexOf('/') != -1)
            {
                return FromStringddMMyyyy(value);
            }

            var res = FromStringyyyyMMdd(value);

            if(res == null)
            {
                res = FromStringddMMyyyy(value);
            }

            return res;
        }
        public static DateTime? FromStringZulu(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var year = Convert.ToInt32(value.Substring(0, 4));
            var month = Convert.ToInt32(value.Substring(5, 2));
            var day = Convert.ToInt32(value.Substring(8, 2));

            return new DateTime(year, month, day);

        }

        public static DateTime? FromStringddMMyyyy(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var data = value.Split('/');
            if (data.Count() != 3)
            {
                return null;
            }

            int year = Convert.ToInt32(data[2], CultureInfo.InvariantCulture);
            int month = Convert.ToInt32(data[1], CultureInfo.InvariantCulture);
            int day = Convert.ToInt32(data[0], CultureInfo.InvariantCulture);
            return new DateTime(year, month, day);
        }

        public static DateTime? FromStringyyyyMMdd(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            value = value.Replace("-", string.Empty);

            int year = Convert.ToInt32(value.Substring(0, 4), CultureInfo.InvariantCulture);
            int month = Convert.ToInt32(value.Substring(4, 2), CultureInfo.InvariantCulture);
            int day = Convert.ToInt32(value.Substring(6, 2), CultureInfo.InvariantCulture);
            return new DateTime(year, month, day);
        }

        // Cuts the date retrieved from the DB to the format DD/MM/YYYY
        public static string FormatDateDDMMYYYY(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return string.Empty;
            }

            if (date.Length > 9)
            {
                return date.Substring(0, 10);
            }

            return string.Empty;
        }

        public static string FormatDateDDMMYYYY(DateTime date)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0:ddMMyyyy}", date);
        }

        public static string FormatDateDDMMYYYY(DateTime? date)
        {
            if (date == null)
            {
                return string.Empty;
            }

            return FormatDateDDMMYYYY(date.Value);
        }

        public static string FormatDayMonth(DateTime date, string language, string instanceName)
        {
            string res = string.Format(CultureInfo.InvariantCulture, "{0}", date.Day) + " de ";
            switch (date.Month)
            {
                case Constant.Month.January:
                    res += ApplicationDictionary.Translate("Common_Month_January", language, instanceName);
                    break;
                case Constant.Month.February:
                    res += ApplicationDictionary.Translate("Common_Month_February", language, instanceName);
                    break;
                case Constant.Month.March:
                    res += ApplicationDictionary.Translate("Common_Month_March", language, instanceName);
                    break;
                case Constant.Month.April:
                    res += ApplicationDictionary.Translate("Common_Month_April", language, instanceName);
                    break;
                case Constant.Month.May:
                    res += ApplicationDictionary.Translate("Common_Month_May", language, instanceName);
                    break;
                case Constant.Month.June:
                    res += ApplicationDictionary.Translate("Common_Month_June", language, instanceName);
                    break;
                case Constant.Month.July:
                    res += ApplicationDictionary.Translate("Common_Month_July", language, instanceName);
                    break;
                case Constant.Month.August:
                    res += ApplicationDictionary.Translate("Common_Month_August", language, instanceName);
                    break;
                case Constant.Month.September:
                    res += ApplicationDictionary.Translate("Common_Month_September", language, instanceName);
                    break;
                case Constant.Month.October:
                    res += ApplicationDictionary.Translate("Common_Month_October", language, instanceName);
                    break;
                case Constant.Month.November:
                    res += ApplicationDictionary.Translate("Common_Month_November", language, instanceName);
                    break;
                case Constant.Month.December:
                    res += ApplicationDictionary.Translate("Common_Month_December", language, instanceName);
                    break;
            }

            return res;
        }

        public static string FormatMonthShortYear(DateTime date, string language, string instanceName)
        {
            string res = string.Empty;
            switch (date.Month - 1)
            {
                case Constant.Month.January:
                    res += ApplicationDictionary.Translate("Common_MonthNameShort_January", language, instanceName);
                    break;
                case Constant.Month.February:
                    res += ApplicationDictionary.Translate("Common_MonthNameShort_February", language, instanceName);
                    break;
                case Constant.Month.March:
                    res += ApplicationDictionary.Translate("Common_MonthNameShort_March", language, instanceName);
                    break;
                case Constant.Month.April:
                    res += ApplicationDictionary.Translate("Common_MonthNameShort_April", language, instanceName);
                    break;
                case Constant.Month.May:
                    res += ApplicationDictionary.Translate("Common_MonthNameShort_May", language, instanceName);
                    break;
                case Constant.Month.June:
                    res += ApplicationDictionary.Translate("Common_MonthNameShort_June", language, instanceName);
                    break;
                case Constant.Month.July:
                    res += ApplicationDictionary.Translate("Common_MonthNameShort_July", language, instanceName);
                    break;
                case Constant.Month.August:
                    res += ApplicationDictionary.Translate("Common_MonthNameShort_August", language, instanceName);
                    break;
                case Constant.Month.September:
                    res += ApplicationDictionary.Translate("Common_MonthNameShort_September", language, instanceName);
                    break;
                case Constant.Month.October:
                    res += ApplicationDictionary.Translate("Common_MonthNameShort_October", language, instanceName);
                    break;
                case Constant.Month.November:
                    res += ApplicationDictionary.Translate("Common_MonthNameShort_November", language, instanceName);
                    break;
                case Constant.Month.December:
                    res += ApplicationDictionary.Translate("Common_MonthNameShort_December", language, instanceName);
                    break;
            }

            res += "/" + date.Year.ToString();

            return res;
        }

        public static string FormatMonthYear(DateTime date, string language, string instanceName)
        {
            string res = string.Empty;
            switch (date.Month - 1)
            {
                case Constant.Month.January:
                    res += ApplicationDictionary.Translate("Common_MonthName_January", language, instanceName);
                    break;
                case Constant.Month.February:
                    res += ApplicationDictionary.Translate("Common_MonthName_February", language, instanceName);
                    break;
                case Constant.Month.March:
                    res += ApplicationDictionary.Translate("Common_MonthName_March", language, instanceName);
                    break;
                case Constant.Month.April:
                    res += ApplicationDictionary.Translate("Common_MonthName_April", language, instanceName);
                    break;
                case Constant.Month.May:
                    res += ApplicationDictionary.Translate("Common_MonthName_May", language, instanceName);
                    break;
                case Constant.Month.June:
                    res += ApplicationDictionary.Translate("Common_MonthName_June", language, instanceName);
                    break;
                case Constant.Month.July:
                    res += ApplicationDictionary.Translate("Common_MonthName_July", language, instanceName);
                    break;
                case Constant.Month.August:
                    res += ApplicationDictionary.Translate("Common_MonthName_August", language, instanceName);
                    break;
                case Constant.Month.September:
                    res += ApplicationDictionary.Translate("Common_MonthName_September", language, instanceName);
                    break;
                case Constant.Month.October:
                    res += ApplicationDictionary.Translate("Common_MonthName_October", language, instanceName);
                    break;
                case Constant.Month.November:
                    res += ApplicationDictionary.Translate("Common_MonthName_November", language, instanceName);
                    break;
                case Constant.Month.December:
                    res += ApplicationDictionary.Translate("Common_MonthName_December", language, instanceName);
                    break;
            }

            res += "/" + date.Year.ToString();

            return res;
        }

        /// <summary>Converts string value on date for DatePicker control</summary>
        /// <param name="date">Text to convert</param>
        /// <returns>String value on date for DatePicker control</returns>
        public static string FormatDatePicker(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return string.Empty;
            }
            else
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"{0:00}/{1:00}/{2:0000}",
                    Convert.ToInt32(date.Substring(0, 2), CultureInfo.GetCultureInfo("en-us")),
                    Convert.ToInt32(date.Substring(2, 2), CultureInfo.GetCultureInfo("en-us")),
                    Convert.ToInt32(date.Substring(6, 4), CultureInfo.GetCultureInfo("en-us")));
            }
        }

        public static string FormatDatePicker(DateTime date)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0:dd/MM/yyyy}", date);
        }

        /// <summary>Generates date value on text for DatePicker control</summary>
        /// <param name="date">Date to convert</param>
        /// <returns>Formatted text for DatePicker control</returns>
        public static string FormatDatePicker(DateTime? date)
        {
            if (date == null)
            {
                return string.Empty;
            }

            return FormatDatePicker(date.Value);
        }

        public static string FormatTimePicker(DateTime date)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0:hh:MM}", date);
        }

        /// <summary>Generates date value on text for DatePicker control</summary>
        /// <param name="date">Date to convert</param>
        /// <returns>Formatted text for DatePicker control</returns>
        public static string FormatTimePicker(DateTime? date)
        {
            if (date == null)
            {
                return string.Empty;
            }

            return FormatTimePicker(date.Value);
        }

        public static DateTime? TextToDate(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            if (text.IndexOf("T") != -1)
            {
                var partsT = text.Split('T')[0].Replace('-', '/').Split('/');
                return DateTime.ParseExact(partsT[2] + "/" + partsT[1] + "/" + partsT[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            var parts = text.Split('/');
            if (parts.Length == 3)
            {
                return DateTime.ParseExact(text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            return null;
        }
    }
}