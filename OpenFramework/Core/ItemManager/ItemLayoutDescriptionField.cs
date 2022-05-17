namespace OpenFrameworkV3.Core.ItemManager
{
    using Newtonsoft.Json;

    /// <summary>Description field of item</summary>
    public sealed class ItemLayoutDescriptionField
    {
        /// <summary>Gets or sets the name of item description field</summary>
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}