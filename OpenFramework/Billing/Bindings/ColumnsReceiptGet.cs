// --------------------------------
// <copyright file="ColumnsReceiptGet.cs" company="OpenFramework">
//     Copyright (c) 2013 - ColumnsInvoiceLineGet. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Billing
{
    /// <summary>Implements receipt</summary>
    public partial class Receipt
    {
        /// <summary>Index of columns from Billing_ReceiptGet stored procedures</summary>
        public static class ColumnsReceiptGet
        {
            /// <summary>Index column of "Id" column</summary>
            public const int Id = 0;

            /// <summary>Index column of "CompanyId" column</summary>
            public const int CompanyId = 1;

            /// <summary>Index column of "Number" column</summary>
            public const int Number = 2;

            /// <summary>Index column of "Subject" column</summary>
            public const int Subject = 3;

            /// <summary>Index column of "ItemDefinitionId" column</summary>
            public const int ItemDefinitionId = 4;

            /// <summary>Index column of "ItemId" column</summary>
            public const int ItemId = 5;

            /// <summary>Index column of "Date" column</summary>
            public const int Date = 6;

            /// <summary>Index column of "PayerName" column</summary>
            public const int PayerName = 7;

            /// <summary>Index column of "PayerCIF" column</summary>
            public const int PayerCIF = 8;

            /// <summary>Index column of "ChargerName" column</summary>
            public const int ChargerName = 9;

            /// <summary>Index column of "ChargerCIF" column</summary>
            public const int ChargerCIF = 10;

            /// <summary>Index column of "Status" column</summary>
            public const int Status = 11;

            /// <summary>Index column of "BaseAmount" column</summary>
            public const int BaseAmount = 12;

            /// <summary>Index column of "TotalIVA" column</summary>
            public const int TotalIVA = 13;

            /// <summary>Index column of "Total" column</summary>
            public const int Total = 14;

            /// <summary>Index column of "CreatedBy" column</summary>
            public const int CreatedBy = 15;

            /// <summary>Index column of "CreatedByName" column</summary>
            public const int CreatedByName = 16;

            /// <summary>Index column of "CreatedByLastName" column</summary>
            public const int CreatedByLastName = 17;

            /// <summary>Index column of "CreatedByLastName2" column</summary>
            public const int CreatedByLastName2 = 18;

            /// <summary>Index column of "CreatedOn" column</summary>
            public const int CreatedOn = 19;

            /// <summary>Index column of "ModifiedBy" column</summary>
            public const int ModifiedBy = 20;

            /// <summary>Index column of "ModifiedByName" column</summary>
            public const int ModifiedByName = 21;

            /// <summary>Index column of "ModifiedByLastName" column</summary>
            public const int ModifiedByLastName = 22;

            /// <summary>Index column of "ModifiedByLastName2" column</summary>
            public const int ModifiedByLastName2 = 23;

            /// <summary>Index column of "ModifiedOn" column</summary>
            public const int ModifiedOn = 24;

            /// <summary>Index column of "Active" column</summary>
            public const int Active = 25;

            /// <summary>Index column of "DateConfirmed" column</summary>
            public const int DateConfirmed = 26;

            /// <summary>Index column of "DateSend" column</summary>
            public const int DateSend = 27;

            /// <summary>Index column of "DatePay" column</summary>
            public const int DatePay = 28;

            /// <summary>Index column of "Notes" column</summary>
            public const int Notes = 29;

            /// <summary>Index column of "Type" column</summary>
            public const int Type = 30;
        }
    }
}