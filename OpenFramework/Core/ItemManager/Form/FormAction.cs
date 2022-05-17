﻿namespace OpenFrameworkV3.Core.ItemManager.ItemForm
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

        /// <summary>Gets or sets a value inditcaitng if option is considered importat</summary>
        [JsonProperty("Important")]
        public bool Important { get; set; }

        /// <summary>Gets or sets a value inditcaitng tab identifier when button is showed</summary>
        [JsonProperty("Tab")]
        public string Tab { get; set; }
    }
}