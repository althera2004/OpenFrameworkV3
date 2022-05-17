namespace OpenFrameworkV3.Communications
{
    using System;
    using Newtonsoft.Json;

    /// <summary>Available actions for a job position item</summary>
    [FlagsAttribute]
    public enum SourceType
    {
        /// <summary>Source by text - 0</summary>
        None = 0,

        /// <summary>Source by item field - 1</summary>
        Field = 1,

        /// <summary>Source by item - 2</summary>
        Item = 2
    }

    [Serializable]
    public class Source
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("ItemName")]
        public string ItemName { get; set; }

        [JsonProperty("BridgeItemName")]
        public string BridgeItemName { get; set; }

        [JsonProperty("ItemFieldName")]
        public string ItemFieldName { get; set; }

        [JsonProperty("Type")]
        private readonly int type;

        [JsonIgnore]
        public SourceType Type
        {
            get
            {
                switch (this.type)
                {
                    case 1: return SourceType.Field;
                    case 2: return SourceType.Item;
                    default:return SourceType.None;
                }
            }
        }
    }
}
