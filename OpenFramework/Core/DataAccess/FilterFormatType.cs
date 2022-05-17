// --------------------------------
// <copyright file="FilterFormatType.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.DataAccess
{
    using System;

    /// <summary>Enumeration of export file format</summary>
    [Serializable]
    public enum FilterFormatType
    {
        /// <summary>CSV - Comma separated value</summary>
        CSV = 0,

        /// <summary>Excel - Microsoft Excel</summary>
        Excel = 1,

        /// <summary>PDF - Portable document format</summary>
        PDF = 2
    }
}
