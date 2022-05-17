// --------------------------------
// <copyright file="ColumnsAccessGet.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.Security
{
    /// <summary>Implements Access control to application</summary>
    public partial class Access
    {
        /// <summary>Index of columns from "Core_Access_All" stored procedure</summary>
        public class ColumnsAccessGet
        {
            /// <summary>Index column of "Id" column</summary>
            public readonly static int Id = 0;

            /// <summary>Index column of "CompanyId" column</summary>
            public readonly static int CompanyId = 1;

            /// <summary>Index column of "Description" column</summary>
            public readonly static int Description = 2;

            /// <summary>Index column of "IP" column</summary>
            public readonly static int IP = 3;

            /// <summary>Index column of "IMEI" column</summary>
            public readonly static int IMEI = 4;

            /// <summary>Index column of "UserId" column</summary>
            public readonly static int UserId = 5;

            /// <summary>Index column of "UsuarioName" column</summary>
            public readonly static int UsuarioName = 6;

            /// <summary>Index column of "UsuarioLastName" column</summary>
            public readonly static int UsuarioLastName = 7;

            /// <summary>Index column of "UsuarioLastName2" column</summary>
            public readonly static int UsuarioLastName2 = 8;

            /// <summary>Index column of "CreatedBy" column</summary>
            public readonly static int CreatedBy = 9;

            /// <summary>Index column of "CreatedByName" column</summary>
            public readonly static int CreatedByName = 10;

            /// <summary>Index column of "CreatedByLastName" column</summary>
            public readonly static int CreatedByLastName = 11;

            /// <summary>Index column of "CreatedByLastName2" column</summary>
            public readonly static int CreatedByLastName2 = 12;

            /// <summary>Index column of "CreatedOn" column</summary>
            public readonly static int CreatedOn = 13;

            /// <summary>Index column of "ModifiedBy" column</summary>
            public readonly static int ModifiedBy = 14;

            /// <summary>Index column of "ModifiedByName" column</summary>
            public readonly static int ModifiedByName = 15;

            /// <summary>Index column of "ModifiedByLastName" column</summary>
            public readonly static int ModifiedByLastName = 16;

            /// <summary>Index column of "ModifiedByLastName2" column</summary>
            public readonly static int ModifiedByLastName2 = 17;

            /// <summary>Index column of "ModifiedOn" column</summary>
            public readonly static int ModifiedOn = 18;

            /// <summary>Index column of "Active" column</summary>
            public readonly static int Active = 19;
        }
    }
}