// --------------------------------
// <copyright file="ColumnsCategoryGet.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Billing
{
    /// <summary>Implements category of billing concepts</summary>
    public partial class ConceptCategory
    {
        /// <summary>Index of columns from Billing_Category get stored procedures</summary>
        public static class ColumnsCategoryGet
        {
            /// <summary>Index column of "Id" column</summary>
            public const int Id = 0;

            /// <summary>Index column of "CompanyId" column</summary>
            public const int CompanyId = 1;

            /// <summary>Index column of "Description" column</summary>
            public const int Description = 2;

            /// <summary>Index column of "CreatedBy" column</summary>
            public const int CreatedBy = 3;

            /// <summary>Index column of "CreatedByName" column</summary>
            public const int CreatedByName = 4;

            /// <summary>Index column of "CreatedByLastName" column</summary>
            public const int CreatedByLastName = 5;

            /// <summary>Index column of "CreatedByLastName2" column</summary>
            public const int CreatedByLastName2 = 6;

            /// <summary>Index column of "CreatedOn" column</summary>
            public const int CreatedOn = 7;

            /// <summary>Index column of "ModifiedBy" column</summary>
            public const int ModifiedBy = 8;

            /// <summary>Index column of "ModifiedByName" column</summary>
            public const int ModifiedByName = 9;

            /// <summary>Index column of "ModifiedByLastName" column</summary>
            public const int ModifiedByLastName = 10;

            /// <summary>Index column of "ModifiedByLastName2" column</summary>
            public const int ModifiedByLastName2 = 11;

            /// <summary>Index column of "ModifiedOn" column</summary>
            public const int ModifiedOn = 12;

            /// <summary>Index column of "Active" column</summary>
            public const int Active = 13;
        }
    }
}