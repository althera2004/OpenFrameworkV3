// --------------------------------
// <copyright file="Sorting.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager.ItemList
{
    using Newtonsoft.Json;

    /// <summary>Implements sorting configuration of column</summary>
    public sealed class Sorting
    {
        /// <summary>Gets or sets sorting type</summary>
        [JsonProperty("SortingType")]
        public string SortingType { get; set; }

        /// <summary>Gets or sets the field/column name</summary>
        [JsonProperty("Index")]
        public int Index { get; set; }
    }
}