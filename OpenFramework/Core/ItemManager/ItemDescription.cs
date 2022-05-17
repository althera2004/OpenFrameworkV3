// --------------------------------
// <copyright file="ItemDescription.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    /// <summary>Implements item description Class</summary>
    public sealed class ItemDescription
    {
        /// <summary>List of fields that composes description</summary>
        [JsonProperty("Fields")]
        private ItemDescriptionField[] fields;

        /// <summary>Gets or sets the pattern to build description</summary>
        [JsonProperty("Pattern")]
        public string Pattern { get; set; }

        /// <summary>Gets the list of fields that composes description</summary>
        [JsonIgnore]
        public ReadOnlyCollection<ItemDescriptionField> Fields
        {
            get
            {
                if (this.fields == null)
                {
                    return new ReadOnlyCollection<ItemDescriptionField>(new List<ItemDescriptionField>());
                }

                return new ReadOnlyCollection<ItemDescriptionField>(this.fields);
            }
        }

        public void AddField(string name)
        {
            if (this.fields == null)
            {
                this.fields = new List<ItemDescriptionField>().ToArray();
            }

            var fieldsTemp = new List<ItemDescriptionField>();
            bool exists = false;
            foreach(var field in this.fields)
            {
                fieldsTemp.Add(field);
                if (field.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
                {
                    exists = true;
                }
            }

            if (!exists)
            {
                fieldsTemp.Add(new ItemDescriptionField { Name = name });
            }

            this.fields = fieldsTemp.ToArray();
        }
    }
}
