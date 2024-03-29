﻿// --------------------------------
// <copyright file="ColumnsGrantGet.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFramework.Security
{
    /// <summary>Implments user grants</summary>
    public partial class UserGrant
    {
        /// <summary>Index of columns from "Core_Group_GetGrants" and "Core_User_GetGrants" stored procedures</summary>
        public static class ColumnsGrantGet
        {
            /// <summary>Index column of "GroupId" column</summary>
            public readonly static int GroupId = 0;

            /// <summary>Index column of "ApplicationUserId" column</summary>
            public readonly static int ApplicationUserId = 1;

            /// <summary>Index column of "ItemId" column</summary>
            public readonly static int ItemId = 2;

            /// <summary>Index column of "ItemName" column</summary>
            public readonly static int ItemName = 3;

            /// <summary>Index column of "Grants" column</summary>
            public readonly static int Grants = 4;
        }
    }
}