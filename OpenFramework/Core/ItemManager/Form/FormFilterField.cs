// --------------------------------
// <copyright file="SecurityConfiguration.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager.ItemForm
{
    using System;
    using Newtonsoft.Json;

    /// <summary>Implements definition for form fields</summary>
    [Serializable]
    public sealed class FormFilterField
    {
        /// <summary>Field value</summary>
        [JsonProperty("Value")]
        private readonly string value;

        /// <summary>Local field value</summary>
        [JsonProperty("LocalValue")]
        private readonly string localValue;

        /// <summary>Gets the name of field</summary>
        [JsonProperty("Field")]
        public string Field { get; private set; }

        /// <summary>Gets field value</summary>
        [JsonIgnore]
        public string Value
        {
            get
            {
                if(string.IsNullOrEmpty(this.value))
                {
                    return string.Empty;
                }

                return this.value;
            }
        }

        /// <summary>Gets local field value</summary>
        [JsonIgnore]
        public string LocalValue
        {
            get
            {
                if(string.IsNullOrEmpty(this.localValue))
                {
                    return string.Empty;
                }

                return this.localValue;
            }
        }
    }
}