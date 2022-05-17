// --------------------------------
// <copyright file="ItemGeolocation.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager
{
    using System;
    using Newtonsoft.Json;

    [Serializable]
    public sealed class ItemGeolocation
    {
        /// <summary>Gets or sets way type field</summary>
        [JsonProperty("WayType")]
        public string WayType { get; private set; }

        /// <summary>Gets or sets address field</summary>
        [JsonProperty("Address")]
        public string Address { get; private set; }

        /// <summary>Gets or sets address complement field</summary>
        [JsonProperty("AddressComplement")]
        public string AddressComplement { get; private set; }

        /// <summary>Gets or sets postal code field</summary>
        [JsonProperty("PostalCode")]
        public string PostalCode { get; private set; }

        /// <summary>Gets or sets city field</summary>
        [JsonProperty("City")]
        public string City { get; private set; }

        /// <summary>Gets or sets province field</summary>
        [JsonProperty("Province")]
        public string Province { get; private set; }

        /// <summary>Gets or sets country field</summary>
        [JsonProperty("Country")]
        public string Country { get; private set; }

        /// <summary>Gets or sets latitude field</summary>
        [JsonProperty("Latitude")]
        public string Latitude { get; private set; }

        /// <summary>Gets or sets longitude field</summary>
        [JsonProperty("Longitude")]
        public string Longitude { get; private set; }
    }
}