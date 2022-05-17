// --------------------------------
// <copyright file="ReportDestination.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------

namespace OpenFrameworkV3.Reports
{
    using Newtonsoft.Json;

    /// <summary>Defines the <see cref="ReportDestination" />.</summary>
    public class ReportDestination
    {
        /// <summary>Gets or sets the ItemName.</summary>
        [JsonProperty("ItemName")]
        public string ItemName { get; set; }

        /// <summary>Gets or sets the ItemField.</summary>
        [JsonProperty("ItemField")]
        public string ItemField { get; set; }

        /// <summary>Gets or sets the ItemIdParameter.</summary>
        [JsonProperty("ItemIdParameter")]
        public string ItemIdParameter { get; set; }
    }
}
