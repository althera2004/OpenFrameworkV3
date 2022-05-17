// --------------------------------
// <copyright file="ExtendSelect.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------

namespace OpenFramework.ItemManager.Form
{
    using System;

    /// <summary>Enumeration of representations of select control in HTML page</summary>
    [FlagsAttribute]
    public enum ExtendSelect
    {
        /// <summary>None - 0</summary>
        None = 0,

        /// <summary>Select2 - 0</summary>
        Select2 = 1,

        /// <summary>Select2Add - 0</summary>
        Select2Add = 2,

        /// <summary>ButtonBAR - 0</summary>
        ButtonBAR = 3
    }
}