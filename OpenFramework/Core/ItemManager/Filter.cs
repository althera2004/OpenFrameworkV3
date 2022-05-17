// --------------------------------
// <copyright file="Filter.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager
{ 
    using System;
    using Newtonsoft.Json;

    [Serializable]
    public sealed class Filter
    {
        /// <summary>Name of field</summary>
        [JsonProperty("FieldName")]
        public string FieldName;

        /// <summary>Condition of filter</summary>
        [JsonProperty("Condition")]
        public Condition Condition { get; set; }
    }
}