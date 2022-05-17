// --------------------------------
// <copyright file="ColumnsReceiptLineGet.cs" company="OpenFramework">
//     Copyright (c) 2013 - ColumnsInvoiceLineGet. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Billing
{
    /// <summary>Implements receipt lines</summary>
    public partial class ReceiptLine
    {
        /// <summary>Index of columns from Billing_ReceiptLineGet stored procedures</summary>
        public static class ColumnsReceiptLineGet
        {
            /// <summary>Index column of "Id" column</summary>
            public const int Id = 0;

            /// <summary>Index column of "CompanyId" column</summary>
            public const int CompanyId = 1;

            /// <summary>Index column of "Concept" column</summary>
            public const int Concept = 2;

            /// <summary>Index column of "BasePrice" column</summary>
            public const int BasePrice = 3;

            /// <summary>Index column of "Quantity" column</summary>
            public const int Quantity = 4;

            /// <summary>Index column of "Discount" column</summary>
            public const int Discount = 5;

            /// <summary>Index column of "IVA" column</summary>
            public const int IVA = 6;

            /// <summary>Index column of "CreatedBy" column</summary>
            public const int CreatedBy = 7;

            /// <summary>Index column of "CreatedByName" column</summary>
            public const int CreatedByName = 8;

            /// <summary>Index column of "CreatedByLastName" column</summary>
            public const int CreatedByLastName = 9;

            /// <summary>Index column of "CreatedByLastName2" column</summary>
            public const int CreatedByLastName2 = 10;

            /// <summary>Index column of "CreatedOn" column</summary>
            public const int CreatedOn = 11;

            /// <summary>Index column of "ModifiedBy" column</summary>
            public const int ModifiedBy = 12;

            /// <summary>Index column of "ModifiedByName" column</summary>
            public const int ModifiedByName = 13;

            /// <summary>Index column of "CompanyId" column</summary>
            public const int ModifiedByLastName = 14;

            /// <summary>Index column of "ModifiedByLastName" column</summary>
            public const int ModifiedByLastName2 = 15;

            /// <summary>Index column of "ModifiedOn" column</summary>
            public const int ModifiedOn = 16;

            /// <summary>Index column of "Active" column</summary>
            public const int Active = 17;
        }
    }
}