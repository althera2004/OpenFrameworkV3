// --------------------------------
// <copyright file="SecurityConfiguration.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    /// <summary>Implements item description Class</summary>
    public sealed class ItemLayoutDescription
    {
        /// <summary>List of fields that composes description</summary>
        [JsonProperty("Fields")]
        private ItemLayoutDescriptionField[] fields;

        /// <summary>Gets or sets the pattern to build description</summary>
        [JsonProperty("Pattern")]
        public string Pattern { get; set; }

        /// <summary>Gets the list of fields that composes description</summary>
        [JsonIgnore]
        public ReadOnlyCollection<ItemLayoutDescriptionField> Fields
        {
            get
            {
                if (this.fields == null)
                {
                    return new ReadOnlyCollection<ItemLayoutDescriptionField>(new List<ItemLayoutDescriptionField>());
                }

                return new ReadOnlyCollection<ItemLayoutDescriptionField>(this.fields);
            }
        }

        public void AddField(string name)
        {
            if (this.fields == null)
            {
                this.fields = new List<ItemLayoutDescriptionField>().ToArray();
            }

            var fieldsTemp = new List<ItemLayoutDescriptionField>();
            bool exists = false;
            foreach (var field in this.fields)
            {
                fieldsTemp.Add(field);
                if (field.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
                {
                    exists = true;
                }
            }

            if (!exists)
            {
                fieldsTemp.Add(new ItemLayoutDescriptionField { Name = name });
            }

            this.fields = fieldsTemp.ToArray();
        }
    }
}