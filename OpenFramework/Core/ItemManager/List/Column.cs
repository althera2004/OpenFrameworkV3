namespace OpenFrameworkV3.Core.ItemManager.ItemList
{
    using Newtonsoft.Json;

    /// <summary>Implements Column class that represents a DataTable column for show ItemBuilder data in lists pages</summary>
    [JsonObject("Columns")]
    public sealed class Column
    {
        /// <summary>Gets or sets de CSS class of column content</summary>
        [JsonProperty("ClassName")]
        public string ClassName { get; set; }

        /// <summary>Gets or sets a value indicating whether if column is orderable</summary>
        [JsonProperty("Orderable")]
        public bool Orderable { get; set; }

        /// <summary>Gets or sets a value indicating whether if column is linkable</summary>
        [JsonProperty("Linkable")]
        public bool Linkable { get; set; }

        /// <summary>Gets or sets item for linkable data</summary>
        [JsonProperty("LinkableItem")]
        public string LinkableItem { get; set; }

        /// <summary>Gets or sets a value indicating whether if column is hidden on mobile devices</summary>
        [JsonProperty("HiddenMobile")]
        public bool HiddenMobile { get; set; }

        /// <summary>Gets or sets the width of column</summary>
        [JsonProperty("Width")]
        public int Width { get; set; }

        /// <summary>Gets or sets a value indicating whether if column is showed on list</summary>
        [JsonProperty("HiddenList")]
        public bool HiddenList { get; set; }

        /// <summary>Gets or sets de field identifier of source data to extract column content</summary>
        [JsonProperty("DataProperty")]
        public string DataProperty { get; set; }

        /// <summary>Gets or sets a function to render column content</summary>
        [JsonProperty("Render")]
        public string Render { get; set; }

        /// <summary>Gets or sets a field to replace actual</summary>
        [JsonProperty("ReplacedBy")]
        public string ReplacedBy { get; set; }

        /// <summary>Gets or sets the width of column</summary>
        [JsonProperty("Align")]
        public string Align { get; set; }

        /// <summary>Gets or sets a value indicating whether if column is searchable</summary>
        [JsonProperty("Search")]
        public bool Search { get; set; }

        /// <summary>Gets or sets column label</summary>
        [JsonProperty("Label")]
        public string Label { get; set; }
    }
}