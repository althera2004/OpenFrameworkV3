// --------------------------------
// <copyright file="FixedListItem.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager
{
    using System.Globalization;
    using Newtonsoft.Json;

    /// <summary>Implements item of fixed list</summary>
    public sealed class FixedListItem
    {
        /// <summary>Gets or sets the item identifier</summary>
        [JsonProperty("Id")]
        public string Id { get; set; }

        /// <summary>Gets or sets the item description</summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>Gets a JSON structure of item</summary>
        [JsonIgnore]
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0}, ""Description"":""{1}""}}",
                    this.Id,
                    Tools.Json.JsonCompliant(this.Description));
            }
        }
    }
}