// --------------------------------
// <copyright file="ColumnsConceptoGet.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Billing
{
    /// <summary>Implements concept of facturation concepts</summary>
    public partial class Concept
    {
        /// <summary>Index of columns from Fact_Concepto get stored procedures</summary>
        public static class ColumnsConceptoGet
        {
            /// <summary>Index column of "Id" column</summary>
            public const int Id = 0;

            /// <summary>Index column of "CompanyId" column</summary>
            public const int CompanyId = 1;

            /// <summary>Index column of "Description" column</summary>
            public const int Description = 2;

            /// <summary>Index column of "Amount" column</summary>
            public const int Amount = 3;

            /// <summary>Index column of "IVA" column</summary>
            public const int IVA = 4;

            /// <summary>Index column of "CategoryId" column</summary>
            public const int CategoryId = 5;

            /// <summary>Index column of "CategoryDescription" column</summary>
            public const int CategoryDescription = 6;

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

            /// <summary>Index column of "ModifiedByLastName" column</summary>
            public const int ModifiedByLastName = 14;

            /// <summary>Index column of "ModifiedByLastName2" column</summary>
            public const int ModifiedByLastName2 = 15;

            /// <summary>Index column of "ModifiedOn" column</summary>
            public const int ModifiedOn = 16;

            /// <summary>Index column of "Active" column</summary>
            public const int Active = 17;

            /// <summary>Index column of "Type" column</summary>
            public const int Type = 18;
        }
    }
}