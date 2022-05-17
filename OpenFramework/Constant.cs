// --------------------------------
// <copyright file="Constant.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3
{
    using System;
    using System.Globalization;

    public partial class Constant
    {
        public const long BillingItemType = 10000;

        public const int FiscalMonthDays = 30;

        public const long CompanyAdminGroup = 4;

        public const string DefaultItem = @"{""Id"":-1}";

        public const int DefaultTimeout = 900;

        public const string FakeCssId = "id=\"main-ace-style\"";

        public const string SqlNull = "NULL";

        public const long DefaultId = -1;

        public const bool OnlyActive = true;

        public const bool ActiveAndInactive = false;

        public const bool Disposable = true;

        public const bool True = true;

        public const bool False = false;

        /// <summary>Indicates the last item of breadcrumb</summary>
        public const bool BreadCrumbLeaf = true;

        /// <summary>Text for JavaScript true value</summary>
        public const int SmtpPort = 25;

        /// <summary>Text for JavaScript true value</summary>
        public const string JavaScriptTrue = "true";

        /// <summary>Text for JavaScript false value</summary>
        public const string JavaScriptFalse = "false";

        /// <summary>Text for JavaScript null value</summary>
        public const string JavaScriptNull = "null";

        /// <summary>Value for a two span division on bootstrap grid</summary>
        public const int ColumnSpan2 = 2;

        /// <summary>Value for a three span division on bootstrap grid</summary>
        public const int ColumnSpan3 = 3;

        /// <summary>Value for a four span division on bootstrap grid</summary>
        public const int ColumnSpan4 = 4;

        /// <summary>Value for a six span division on bootstrap grid</summary>
        public const int ColumnSpan6 = 6;

        /// <summary>Value for a eight span division on bootstrap grid</summary>
        public const int ColumnSpan8 = 8;

        /// <summary>Value for a twelve span division on bootstrap grid</summary>
        public const int ColumnSpan12 = 12;

        /// <summary>Maximum length for textarea fields in database</summary>
        public const int MaximumTextAreaLength = 2000;

        /// <summary>Default length for nvarchar fields in SQL table</summary>
        public const int DefaultDatabaseVarChar = 50;

        /// <summary>Menu option is not leaft</summary>
        public const bool NotLeaft = false;

        /// <summary>Page execution has end response</summary>
        public const bool EndResponse = true;

        /// <summary>Actual date time</summary>
        public static DateTime Now
        {
            get
            {
                return DateTime.Now.ToLocalTime();
            }
        }

        /// <summary>Formatted text for actual date time</summary>
        public static string NowText
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("es-es"), "{0:dd/MM/yyyy}", Now);
            }
        }
    }
}