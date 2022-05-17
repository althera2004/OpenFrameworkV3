// --------------------------------
// <copyright file="ListLayout.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.ItemManager.List
{
    using System;

    /// <summary>Enumeration of list layouts</summary>
    [FlagsAttribute]
    public enum ListLayout
    {
        /// <summary>ReadOnly - 0</summary>
        ReadOnly = 0,

        /// <summary>Editable - 1</summary>
        Editable = 1,

        /// <summary>Linkable = 2</summary>
        Linkable = 2,

        /// <summary>NoButton = 3</summary>
        NoButton = 3
    }
}