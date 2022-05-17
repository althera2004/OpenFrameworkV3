// --------------------------------
// <copyright file="ColumnsByUserIdSimple.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.Security
{
    /// <summary>Implements ApplicationUser class.</summary>
    public partial class ApplicationUser
    {
        /// <summary>Index of columns from "Core_User_GetByIdSimple" stored procedure</summary>
        public static class ColumnsByUserIdSimple
        {
            /// <summary>Index column of "Id" column</summary>
            public readonly static int Id = 0;

            /// <summary>Index column of "Email" column</summary>
            public readonly static int Email = 1;

            /// <summary>Index column of "Locked" column</summary>
            public readonly static int Locked = 2;

            /// <summary>Index column of "Core" column</summary>
            public readonly static int Core = 3;

            /// <summary>Index column of "LanguageId" column</summary>
            public readonly static int LanguageId = 4;

            /// <summary>Index column of "LanguageName" column</summary>
            public readonly static int LanguageName = 5;

            /// <summary>Index column of "LaguageLocaleName" column</summary>
            public readonly static int LaguageLocaleName = 6;

            /// <summary>Index column of "LanguageISO" column</summary>
            public readonly static int LanguageISO = 7;

            /// <summary>Index column of "TechnicalUser" column</summary>
            public readonly static int TechnicalUser = 8;

            /// <summary>Index column of "PrimaryUser" column</summary>
            public readonly static int PrimaryUser = 9;

            /// <summary>Index column of "AdminUser" column</summary>
            public readonly static int AdminUser = 10;

            /// <summary>Index column of "Name" column</summary>
            public readonly static int Name = 11;

            /// <summary>Index column of "LastName" column</summary>
            public readonly static int LastName = 12;

            /// <summary>Index column of "LastName2" column</summary>
            public readonly static int LastName2 = 13;

            /// <summary>Index column of "Phone" column</summary>
            public readonly static int Phone = 14;

            /// <summary>Index column of "Mobile" column</summary>
            public readonly static int Mobile = 15;

            /// <summary>Index column of "CreatedById" column</summary>
            public readonly static int CreatedById = 16;

            /// <summary>Index column of "CreatedByName" column</summary>
            public readonly static int CreatedByName = 17;

            /// <summary>Index column of "CreatedByLastName" column</summary>
            public readonly static int CreatedByLastName = 18;

            /// <summary>Index column of "CreatedByLastName2" column</summary>
            public readonly static int CreatedByLastName2 = 19;

            /// <summary>Index column of "CreatedOn" column</summary>
            public readonly static int CreatedOn = 20;

            /// <summary>Index column of "ModifiedById" column</summary>
            public readonly static int ModifiedById = 21;

            /// <summary>Index column of "ModifiedByName" column</summary>
            public readonly static int ModifiedByName = 22;

            /// <summary>Index column of "ModifiedByLastName" column</summary>
            public readonly static int ModifiedByLastName = 23;

            /// <summary>Index column of "ModifiedByLastName2" column</summary>
            public readonly static int ModifiedByLastName2 = 24;

            /// <summary>Index column of "ModifiedOn" column</summary>
            public readonly static int ModifiedOn = 25;

            /// <summary>Index column of "Active" column</summary>
            public readonly static int Active = 26;

            /// <summary>Index column of "External" column</summary>
            public readonly static int External = 27;

            /// <summary>Index column of "ExternalUsers" column</summary>
            public readonly static int ExternalUsers = 28;

            /// <summary>Index column of "Corportative" column</summary>
            public readonly static int Corportative = 29;
        }
    }
}