// -----------------------------------------------------------------------
// <copyright file="SecurityConfiguration.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace OpenFrameworkV3.Core.InstanceManager
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    [Serializable]
    public class Features
    {
        /// <summary>Wacom signature activation</summary>
        [JsonProperty("Wacom")]
        public bool Wacom { get; set; }

        /// <summary>Firmafy wrapper activatin</summary>
        [JsonProperty("Firmafy")]
        public bool Firmafy { get; set; }

        /// <summary>Firmafy wrapper activatin</summary>
        [JsonProperty("QR")]
        public bool QR { get; set; }

        /// <summary>Firmafy wrapper activatin</summary>
        [JsonProperty("CreditCard")]
        public bool CreditCard { get; set; }

        /// <summary>Gets an empty instance of Features class</summary>
        [JsonIgnore]
        public static Features Empty
        {
            get
            {
                return new Features
                {
                    Wacom = false,
                    Firmafy = false,
                    QR = false,
                    CreditCard = false
                };
            }
        }/// <summary>Gets a JSON definition of mail configuration</summary>
        [JsonIgnore]
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"""Features"": {{
        ""Wacom"": {0},
        ""Firmafy"": {1},
        ""QR"": {2},
        ""CreditCard"": {3}
    }},",
                    ConstantValue.Value(this.Wacom),
                    ConstantValue.Value(this.Firmafy),
                    ConstantValue.Value(this.QR),
                    ConstantValue.Value(this.CreditCard));
            }
        }
    }
}
