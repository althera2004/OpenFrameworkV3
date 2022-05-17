namespace OpenFrameworkV3.Core.ItemManager
{
    using System.Globalization;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>Definition for item layout</summary>
    public sealed class ItemDefinitionLayout
    {
        /// <summary>Gets an empty instance of an ItemDefinitionLayout class</summary>
        [JsonIgnore]
        public static ItemDefinitionLayout Empty
        {
            get
            {
                return new ItemDefinitionLayout()
                {
                    Icon = string.Empty,
                    Label = string.Empty,
                    LabelPlural = string.Empty
                };
            }
        }

        /// <summary>Gets or sets the icon that represents the item</summary>
        [JsonProperty("Icon")]
        public string Icon { get; set; }

        /// <summary>Gets or sets the text form item label</summary>
        [JsonProperty("Label")]
        public string Label { get; set; }

        /// <summary>Gets or sets the text for item plural label</summary>
        [JsonProperty("LabelPlural")]
        public string LabelPlural { get; set; }

        /// <summary>Gets or sets the descriptions that represents the item</summary>
        [JsonProperty("Description")]
        public ItemLayoutDescription Description { get; set; }

        [JsonIgnore]
        public string Json
        {
            get
            {
                var fields = new StringBuilder();
                bool first = true;
                foreach (var field in this.Description.Fields)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        fields.Append(", ");
                    }

                    fields.AppendFormat(CultureInfo.InvariantCulture, @" {{ ""Name"": ""{0}"" }}", field.Name);
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"
    ""Layout"": {{
        ""Icon"": ""{2}"",
        ""Label"": ""{0}"",
        ""LabelPlural"": ""{1}"",
        ""Description"": {{
            ""Pattern"": ""{3}"",
            ""Fields"": [{4} ]
        }},
    }}",
                    this.Label,
                    this.LabelPlural ?? this.Label,
                    string.IsNullOrEmpty(this.Icon) ? "fa fa-cog" : this.Icon,
                    this.Description.Pattern,
                    fields);
            }
        }
    }
}