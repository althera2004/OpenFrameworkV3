// --------------------------------
// <copyright file="AlertFieldPosition.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Feature
{
    using Newtonsoft.Json;

    /// <summary>Implements AlertFieldPosition class</summary>
    public class AlertFieldPosition
    {
        /// <summary>Gets or sets the position of filed</summary>
        [JsonProperty("Position")]
        public int Position { get; set; }

        /// <summary>Gets or sets the name of filed </summary>
        [JsonProperty("FieldName")]
        public string FieldName { get; set; }
    }
}