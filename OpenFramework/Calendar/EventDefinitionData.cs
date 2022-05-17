// --------------------------------
// <copyright file="EventDefinitionData.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Calendar
{
    using Newtonsoft.Json;

    public class EventDefinitionData
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("Label")]
        public string Label { get; set; }
    }
}
