// --------------------------------
// <copyright file="FormFieldType.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFramework.ItemManager.Form
{
    using System;

    /// <summary>Enumeration of type of fields in form</summary>
    [FlagsAttribute]
    public enum FormFieldType
    {
        /// <summary>Input control</summary>
        Input = 0,

        /// <summary>Button to do action</summary>
        Button = 1,

        /// <summary>Empty space to fill row</summary>
        Break = 2,

        /// <summary>List (filtering purpose)</summary>
        List = 3,

        /// <summary>Place holder for custom controls and messages</summary>
        PlaceHolder = 4,

        /// <summary>Foreign keys in radio buttons format</summary>
        Radiobutton = 5
    }
}
