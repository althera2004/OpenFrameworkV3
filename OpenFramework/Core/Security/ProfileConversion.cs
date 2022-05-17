// -----------------------------------------------------------------------
// <copyright file="ProfileConversion.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace OpenFrameworkV3.Security
{
    using Newtonsoft.Json;
    using System.Globalization;

    public class ProfileConversion
    {
        [JsonProperty("FieldFrom")]
        public string FieldFrom { get; set; }

        [JsonProperty("FieldProfile")]
        public string FieldProfile { get; set; }

        [JsonIgnore]
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""FieldForm"":""{0}"", ""FieldProfile"":""{1}""}}",
                    this.FieldFrom,
                    this.FieldProfile);
            }
        }
    }
}