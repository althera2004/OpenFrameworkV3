// --------------------------------
// <copyright file="ColumnNotesGet.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Feature
{
    public partial class Note
    {
        /// <summary>Index of columns from "Core_Access_All" stored procedure</summary>
        public class ColumnNotesGet
        {
            /// <summary>Index column of "Id" column</summary>
            public readonly static int Id = 0;

            /// <summary>Index column of "CompanyId" column</summary>
            public readonly static int CompanyId = 1;

            /// <summary>Index column of "ItemDefinitionId" column</summary>
            public readonly static int ItemDefinitionId = 2;

            /// <summary>Index column of "ItemId" column</summary>
            public readonly static int ItemId = 3;

            /// <summary>Index column of "Author" column</summary>
            public readonly static int Author = 4;

            /// <summary>Index column of "Text" column</summary>
            public readonly static int Text = 5;

            /// <summary>Index column of "CreatedBy" column</summary>
            public readonly static int CreatedBy = 6;

            /// <summary>Index column of "CreatedByName" column</summary>
            public readonly static int CreatedByName = 7;

            /// <summary>Index column of "CreatedByLastName" column</summary>
            public readonly static int CreatedByLastName = 8;

            /// <summary>Index column of "CreatedByLastName2" column</summary>
            public readonly static int CreatedByLastName2 = 9;

            /// <summary>Index column of "CreatedOn" column</summary>
            public readonly static int CreatedOn = 10;

            /// <summary>Index column of "column" column</summary>
            public readonly static int ModifiedBy = 11;

            /// <summary>Index column of "ModifiedByName" column</summary>
            public readonly static int ModifiedByName = 12;

            /// <summary>Index column of "ModifiedByLastName" column</summary>
            public readonly static int ModifiedByLastName = 13;

            /// <summary>Index column of "ModifiedByLastName2" column</summary>
            public readonly static int ModifiedByLastName2 = 14;

            /// <summary>Index column of "ModifiedOn" column</summary>
            public readonly static int ModifiedOn = 15;

            /// <summary>Index column of "Active" column</summary>
            public readonly static int Active = 16;
        }
    }
}