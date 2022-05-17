// --------------------------------
// <copyright file="ListPosition.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.ItemManager.List
{
    using System;

    /// <summary>Enumeration of possible list placement</summary>
    [FlagsAttribute]
    public enum ListPosition
    {
        /// <summary>Is embedded</summary>
        Embedded = 0,

        /// <summary>In a widget</summary>
        Widget = 1
    }
}