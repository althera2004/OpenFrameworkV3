// --------------------------------
// <copyright file="DeleteAction.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Enums
{
    using System;

    /// <summary>Enumeration of delete actions available</summary>
    [FlagsAttribute]
    public enum DeleteAction
    {
        /// <summary>Inactive item in data base</summary>
        Inactive = 0,

        /// <summary>Delete item from data base</summary>
        Delete = 1
    }
}