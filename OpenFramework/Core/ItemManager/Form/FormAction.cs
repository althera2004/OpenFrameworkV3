// --------------------------------
// <copyright file="FormAction.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager.ItemForm
{
    using Newtonsoft.Json;

    public class FormAction
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

        /// <summary>Gets or sets a value indicating if option is considered importat</summary>
        [JsonProperty("Important")]
        public bool Important { get; set; }

        /// <summary>Gets or sets a value indicating tab identifier when button is showed</summary>
        [JsonProperty("Tab")]
        public string Tab { get; set; }

        /// <summary>Gets or sets a value indicating tab identifier when button is showed</summary>
        [JsonProperty("Group")]
        public string Group { get; set; }
    }
}