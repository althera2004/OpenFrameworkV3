// --------------------------------
// <copyright file="Signature.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Reports
{
    using Newtonsoft.Json;

    public class Signature
    {
        [JsonProperty("EveryPage")]
        public bool EveryPage { get; set; }

        [JsonProperty("Page")]
        public int Page { get; set; }

        [JsonProperty("X")]
        public int X { get; set; }

        [JsonProperty("Y")]
        public int Y { get; set; }

        [JsonProperty("Width")]
        public float Width { get; set; }

        [JsonProperty("Height")]
        public float Height { get; set; }

        [JsonProperty("Instructions")]
        public string Instructions { get; set; }

        [JsonProperty("Reason")]
        public string Reason { get; set; }
    }
}
