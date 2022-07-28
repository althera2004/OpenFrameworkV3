// --------------------------------
// <copyright file="SecurityConfiguration.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager.ItemList
{
    using Newtonsoft.Json;

    /// <summary>Implements parameters for list</summary>
    public struct ListParameter
    {
        /// <summary>Gets or sets parameter name</summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>Gets or sets parameter value</summary>
        [JsonProperty("Value")]
        public string Value { get; set; }

        /// <summary>Gets or sets parameter value</summary>
        [JsonProperty("Type")]
        public string Type { get; set; }
    }
}