// --------------------------------
// <copyright file="StoredParameter.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Reports
{
    using Newtonsoft.Json;

    public class StoredParameter
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("DataType")]
        public string DataType { get; set; }
    }
}
