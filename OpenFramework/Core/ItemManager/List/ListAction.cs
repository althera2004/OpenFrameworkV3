// --------------------------------
// <copyright file="ListAction.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager.ItemList
{
    using Newtonsoft.Json;

    public class ListAction
    {
        /// <summary>Form type</summary>
        [JsonProperty("Label")]
        public string Label { get; set; }

        /// <summary>Form type</summary>
        [JsonProperty("Action")]
        public string Action { get; set; }

        /// <summary>Form type</summary>
        [JsonProperty("Icon")]
        public string Icon { get; set; }

        /// <summary>Gets or sets a value indicating tab identifier when button is showed</summary>
        [JsonProperty("Group")]
        public string Group { get; set; }
    }
}
