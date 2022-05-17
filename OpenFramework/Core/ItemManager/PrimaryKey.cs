// --------------------------------
// <copyright file="PrimaryKey.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>Implements primary keys of an item for unique identification</summary>
    [Serializable]
    public sealed class PrimaryKey
    {
        /// <summary>Field names of primary keys</summary>
        [JsonProperty("Values")]
        private string[] itemFields;

        /// <summary>Gets an empty primary key</summary>
        [JsonIgnore]
        public static PrimaryKey Empty
        {
            get
            {
                return new PrimaryKey
                {
                    Id = string.Empty,
                    itemFields = new List<string>().ToArray()
                };
            }
        }

        /// <summary>Gets or sets primary key identifier</summary>
        [JsonProperty("Id")]
        public string Id { get; set; }

        /// <summary>Gets field names of primary keys</summary>
        [JsonIgnore]
        public ReadOnlyCollection<string> ItemFields
        {
            get
            {
                if (this.itemFields == null)
                {
                    return new ReadOnlyCollection<string>(new List<string>());
                }

                return new ReadOnlyCollection<string>(this.itemFields.ToList());
            }
        }
    }
}