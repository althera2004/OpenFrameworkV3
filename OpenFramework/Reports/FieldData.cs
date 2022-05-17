// --------------------------------
// <copyright file="FieldData.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Reports
{
    using Newtonsoft.Json;

    public class FieldData
    {
        [JsonProperty("Index")]
        public int Index { get; set; }

        [JsonProperty("DataType")]
        public string DataType { get; set; }
    }
}