// --------------------------------
// <copyright file="ConditionType.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager
{
    using System;

    /// <summary>List of types of condition</summary>
    [FlagsAttribute]
    public enum ConditionType
    {
        /// <summary>Condition refers to a field value</summary>
        Field = 0,

        /// <summary>Condition refers to a global value</summary>
        Global = 1
    }
}