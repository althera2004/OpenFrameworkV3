namespace OpenFrameworkV3.Core.ItemManager.ItemForm
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>Implements form tab definition</summary>
    public sealed class FormTab
    {
        /// <summary>Gets or sets the index of field positions</summary>
        [JsonProperty("Groups")]
        private int[] groups { get; set; }

        /// <summary>Definition of the rows that will be contained inside of the tab</summary>
        [JsonProperty("Rows")]
        private readonly List<FormRow> rows;

        /// <summary>Gets tabs identifier</summary>
        [JsonProperty("Id")]
        public string Id { get; private set; }

        /// <summary>Gets a value indicating whether tab is selected by default</summary>
        [JsonProperty("DefaultSelected")]
        public bool DefaultSelected { get; private set; }

        /// <summary>Gets a value indicating whether if tab fields are ever visible</summary>
        [JsonProperty("Persistent")]
        public bool Persitent { get; private set; }

        /// <summary>Gets a value indicating whether if the current tab requires an item instance to make it selectable</summary>
        [JsonProperty("ItemRequired")]
        public bool ItemRequired { get; private set; }

        /// <summary>Gets a value indicating whether if item must exists in order to show tab</summary>
        [JsonProperty("ItemMustExists")]
        public bool ItemMustExists { get; private set; }

        /// <summary>Gets a value indicating whether if tab contains a list</summary>
        [JsonProperty("Listable")]
        public bool Listable { get; private set; }

        /// <summary>Gets the tab label</summary>
        [JsonProperty("Label")]
        public string Label { get; private set; }

        /// <summary>Gets the tab Explanation</summary>
        [JsonProperty("Explanation")]
        public string Explanation { get; private set; }

        /// <summary>Gets the rows of form</summary>
        [JsonIgnore]
        public ReadOnlyCollection<FormRow> Rows
        {
            get
            {
                if(this.rows == null || this.rows.Count == 0)
                {
                    return new ReadOnlyCollection<FormRow>(new List<FormRow>());
                }

                return new ReadOnlyCollection<FormRow>(this.rows);
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<long> Groups
        {
            get
            {
                var res = new List<long>();
                if(this.groups != null && this.groups.Count() > 0)
                {
                    foreach(var id in this.groups)
                    {
                        res.Add(Convert.ToInt64(id));
                    }
                }


                return new ReadOnlyCollection<long>(res);
            }
        }

        [JsonIgnore]
        public string GroupsJson
        {
            get
            {
                if(this.groups != null && this.groups.Count() > 0)
                {
                    var res = new StringBuilder("[");
                    bool first = true;
                    foreach(var id in this.groups)
                    {
                        res.AppendFormat(
                            CultureInfo.InvariantCulture,
                            "{0}{1}",
                            first ? string.Empty : ",",
                            id);
                        first = false;
                    }

                    res.Append("]");
                    return res.ToString();
                }


                return Constant.JavaScriptNull;
            }
        }
    }
}