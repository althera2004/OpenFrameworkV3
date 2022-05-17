// --------------------------------
// <copyright file="SecurityConfiguration.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Feature
{
    using Newtonsoft.Json;

    public class CustomListUpdatePrecondition
    {
        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("FieldName")]
        public string FieldName { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}