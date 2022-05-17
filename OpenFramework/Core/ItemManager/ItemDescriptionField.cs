// --------------------------------
// <copyright file="ItemDescriptionField.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFramework.ItemManager
{    
    using Newtonsoft.Json;

    /// <summary>Description field of item</summary>
    public sealed class ItemDescriptionField
    {
        /// <summary>Gets or sets the name of item description field</summary>
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}