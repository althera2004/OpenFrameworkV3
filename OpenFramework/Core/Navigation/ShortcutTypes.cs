// --------------------------------
// <copyright file="ShortcutTypes.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Navigation
{
    using System;

    /// <summary>Indentification of four menu shortcuts</summary>
    [FlagsAttribute]
    public enum ShortcutTypes
    {
        /// <summary>None - 0</summary>
        None = 0,

        /// <summary>Red - 1</summary>
        Red = 1,

        /// <summary>Blue - 2</summary>
        Blue = 2,

        /// <summary>Green - 3</summary>
        Green = 3,

        /// <summary>Yellow - 4</summary>
        Yellow = 4
    }
}