// --------------------------------
// <copyright file="SecurityConfiguration.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager.ItemForm
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>Implements form wor definition</summary>
    public sealed class FormRow
    {
        /// <summary>Checks if the row is collapsible</summary>
        [JsonProperty("Id")]
        private readonly string id;

        /// <summary>Checks if the row is collapsible</summary>
        [JsonProperty("Collapsible")]
        public bool Collapsible { get; set; }

        /// <summary>Gets a value indicating whether row is hidden</summary>
        [JsonProperty("Hidden")]
        public bool Hidden { get; set; }

        /// <summary>Name of the item to render a list (if current row is a list)</summary>
        [JsonProperty("ItemList")]
        private readonly string itemList;

        /// <summary>Name of the item of the form to render (if the current row is a form inform)</summary>
        [JsonProperty("ItemForm")]
        private readonly string itemForm;

        /// <summary>Label of row</summary>
        [JsonProperty("Label")]
        private readonly string label;

        /// <summary>List of fields contained in this row</summary>
        [JsonProperty("Fields")]
        private List<FormField> fields;

        /// <summary>List of filters for the list (if current row is a list)</summary>
        [JsonProperty("FilterFields")]
        private readonly List<FormFilterField> filterFields;

        /// <summary>Gets row fields</summary>
        [JsonIgnore]
        public ReadOnlyCollection<FormField> Fields
        {
            get
            {
                if(this.fields == null)
                {
                    this.fields = new List<FormField>();
                }

                return new ReadOnlyCollection<FormField>(this.fields);
            }
        }

        /// <summary>Gets row label</summary>
        [JsonIgnore]
        public string Id
        {
            get
            {
                if(string.IsNullOrEmpty(this.id))
                {
                    return string.Empty;
                }

                return this.id;
            }
        }

        /// <summary>Gets label of row</summary>
        [JsonIgnore]
        public string Label
        {
            get
            {
                if(string.IsNullOrEmpty(this.label))
                {
                    return string.Empty;
                }

                return this.label;
            }
        }

        /// <summary>Gets list identifier</summary>
        [JsonProperty("ListId")]
        public string ListId { get; private set; }

        /// <summary>Gets custom AJAX source for the list (if the current row is a list)</summary>
        [JsonProperty("CustomAjaxSource")]
        public string CustomAjaxSource { get; private set; }

        /// <summary>Gets or sets table alias in order to be called by JavaScript</summary>
        [JsonProperty("TableAlias")]
        public string TableAlias { get; set; }

        /// <summary>Gets or sets explandible type of row</summary>
        [JsonProperty("Expandible")]
        public string Expandible { get; set; }

        /// <summary>Gets or sets a value indicatig if expandible is collapsed by default</summary>
        [JsonProperty("ExpandibleCollapsed")]
        public bool ExpandibleCollapsed { get; set; }

        /// <summary>Gets or sets group identifier for expandible purposses</summary>
        [JsonProperty("ExpandibleGroup")]
        public string ExpandibleGroup { get; set; }

        /// <summary>Gets filter fields</summary>
        [JsonIgnore]
        public ReadOnlyCollection<FormFilterField> FilterFields
        {
            get
            {
                if(this.filterFields == null)
                {
                    return new ReadOnlyCollection<FormFilterField>(new List<FormFilterField>());
                }

                return new ReadOnlyCollection<FormFilterField>(this.filterFields);
            }
        }

        /// <summary>Gets a value indicating the name of the dependencies to populate list</summary>
        [JsonProperty("LocalData")]
        public string LocalData { get; private set; }

        /// <summary>Gets item builder of list</summary>
        [JsonIgnore]
        public string ItemList
        {
            get
            {
                if(string.IsNullOrEmpty(this.itemList))
                {
                    return string.Empty;
                }

                return this.itemList;
            }
        }

        /// <summary>Gets a value indicating whether the current row is a list</summary>
        [JsonIgnore]
        public bool IsItemList
        {
            get
            {
                return !string.IsNullOrEmpty(this.itemList);
            }
        }

        /// <summary>Gets itemForm</summary>
        [JsonIgnore]
        public string ItemForm
        {
            get
            {
                if(string.IsNullOrEmpty(this.itemForm))
                {
                    return string.Empty;
                }

                return this.itemForm;
            }
        }

        /// <summary>Gets a value indicating whether the current row is a form inform</summary>
        [JsonIgnore]
        public bool IsItemForm
        {
            get
            {
                if(string.IsNullOrEmpty(this.itemForm))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>Function that returns the ItemField objects corresponding to the fields of this row</summary>
        /// <param name="itemFields">List of fields (of the item)</param>
        /// <returns>Dictionary of fields definition></returns>
        public ReadOnlyDictionary<ItemField, FormField> ObtainFields(ReadOnlyCollection<ItemField> itemFields)
        {
            if(this.fields == null || itemFields == null)
            {
                return new ReadOnlyDictionary<ItemField, FormField>(new Dictionary<ItemField, FormField>());
            }

            var res = new Dictionary<ItemField, FormField>();

            // For all fields in the row
            foreach(var fieldDefinition in this.fields)
            {
                // If any of the ItemFields matches the current field...
                if(itemFields.Any(f => f.Name == fieldDefinition.Name))
                {
                    // Add the field to the list
                    res.Add(itemFields.First(f => f.Name == fieldDefinition.Name), fieldDefinition);
                }
                else
                {
                    // Create an empty field
                    var field = ItemField.Empty;

                    // Add the name and the label from the definition
                    if(!string.IsNullOrEmpty(fieldDefinition.Name) && !string.IsNullOrEmpty(fieldDefinition.Label))
                    {
                        field.Name = fieldDefinition.Name;
                        field.Label = fieldDefinition.Label;
                    }

                    // Add the new field
                    res.Add(field, fieldDefinition);
                }
            }

            return new ReadOnlyDictionary<ItemField, FormField>(res);
        }
    }
}