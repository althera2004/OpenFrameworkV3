// --------------------------------
// <copyright file="ForeignList.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager
{
    using System;
    using Newtonsoft.Json;

    /// <summary>Implements foreign list definition</summary>
    [Serializable]
    public sealed class ForeignList
    {
        /// <summary>Name of field referenced for import purposses</summary>
        [JsonProperty("ImportReference")]
        private readonly string importReference;

        /// <summary>Name of local field</summary>
        [JsonProperty("LocalField")]
        private string localField;

        /// <summary>Name of foreign field</summary>
        [JsonProperty("ForeignField")]
        private string foreignField;

        /// <summary>Name of field retrieved</summary>
        [JsonProperty("FieldRetrieved")]
        private string fieldRetrieved;

        /// <summary>Gets a empty foreign list</summary>
        [JsonIgnore]
        public static ForeignList Empty
        {
            get
            {
                return new ForeignList
                {
                    ItemName = string.Empty,
                    fieldRetrieved = string.Empty,
                    foreignField = string.Empty,
                    localField = string.Empty
                };
            }
        }

        /// <summary>Gets or sets the name of item</summary>
        [JsonProperty("ItemName")]
        public string ItemName { get; set; }

        /// <summary>Gets the name of local field</summary>
        [JsonIgnore]
        public string LocalName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.localField))
                {
                    return this.localField;
                }

                return this.ItemName + "Id";
            }
        }

        /// <summary>Gets the name of foreign field</summary>
        [JsonIgnore]
        public string ForeignField
        {
            get
            {
                if (string.IsNullOrEmpty(this.foreignField))
                {
                    return "Id";
                }

                return this.foreignField;
            }
        }

        /// <summary>Gets the name of retrieved field</summary>
        [JsonIgnore]
        public string FieldRetrieved
        {
            get
            {
                if (string.IsNullOrEmpty(this.fieldRetrieved))
                {
                    return "Description";
                }

                return this.fieldRetrieved;
            }
        }

        /// <summary>Gets the name of retrieved field</summary>
        [JsonIgnore]
        public string ImportReference
        {
            get
            {
                if (string.IsNullOrEmpty(this.importReference))
                {
                    return this.FieldRetrieved;
                }

                return this.importReference;
            }
        }

        /// <summary>Gets the name of linked field</summary>
        [JsonProperty("LinkField")]
        public string LinkField { get; private set; }

        /// <summary>Gets the name of foreing field</summary>
        [JsonProperty("TargetField")]
        public string TargetField { get; private set; }

        /// <summary>Gets the name of foreing item</summary>
        [JsonProperty("RemoteItem")]
        public string RemoteItem { get; private set; }

        /// <summary>Gets the name of remote field retrieved</summary>
        [JsonProperty("RemoteRetrievedField")]
        public string RemoteRetrievedField { get; private set; }

        /// <summary>Gets the name of linked combo</summary>
        [JsonProperty("LinkedCombo")]
        public string LinkedCombo { get; private set; }

        /// <summary>Gets a value indicating whether if item is static</summary>
        [JsonProperty("IsStaticItem")]
        public bool IsStaticItem { get; private set; }
    }
}