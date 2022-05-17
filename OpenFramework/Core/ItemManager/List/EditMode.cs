// --------------------------------
// <copyright file="EditMode.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager.ItemList
{
    using System;

    /// <summary>Enumeration of available button actions</summary>
    [FlagsAttribute]
    public enum EditMode
    {
        /// <summary>Not defined</summary>
        None = 0,

        /// <summary>Edition of row inline</summary>
        Inline = 1,

        /// <summary>Show form edition in popup</summary>
        Popup = 2,

        /// <summary>Show content data in custom form on another page</summary>
        Custom = 3,

        /// <summary>Show content data as a part of another ItemBuilder that contains it</summary>
        Inform = 4,

        /// <summary>Button for capture existent data</summary>
        Capture = 5,

        /// <summary>Read only table</summary>
        ReadOnly = 6,

        /// <summary>Master and detail table</summary>
        MasterDetail = 7,

        /// <summary>MultiView edition</summary>
        MultiView = 8,

        /// <summary>ItemLink edition</summary>
        ItemLink = 9,

        /// <summary>MapView edition</summary>
        MapView = 10
    }
}