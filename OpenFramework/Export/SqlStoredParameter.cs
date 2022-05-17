// --------------------------------
// <copyright file="SqlStoredParameter.cs" company="OpenFramework">
//     Copyright (c) 2019 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Export
{
    using System;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core.ItemManager;

    /// <summary>Implements StoredParameter class</summary>
    [Serializable]
    public sealed class SqlStoredParameter
    {
        /// <summary>Parameter name</summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>Parameter value</summary>
        [JsonProperty("Value")]
        public string Value { get; set; }

        /// <summary>Parameter type</summary>
        [JsonProperty("Type")]
        private readonly string valueType;

        /// <summary>Parameter type</summary>
        [JsonProperty("Length")]
        private readonly int length;

        [JsonIgnore]
        public FieldDataType Type
        {
            get
            {
                switch (this.valueType.ToUpperInvariant())
                {
                    case "INT":
                        return FieldDataType.Integer;
                    case "BOOL":
                        return FieldDataType.Boolean;
                    case "LONG":
                        return FieldDataType.Long;
                    case "DECIMAL":
                        return FieldDataType.Decimal;
                    case "DATE":
                        return FieldDataType.DateTime;
                    case "GUID":
                        return FieldDataType.Guid;
                    case "STRING":
                    default:
                        return FieldDataType.Text;
                }
            }
        }

        [JsonIgnore]
        public int Length
        {
            get
            {
                switch (this.valueType.ToUpperInvariant())
                {
                    case "INT":
                        return 0;
                    case "BOOL":
                        return 0;
                    case "LONG":
                        return 0;
                    case "DATE":
                        return 0;
                    case "GUID":
                        return 32;
                    case "STRING":
                    default:
                        return this.length == 0 ? 50 : this.length;
                }
            }
        }
    }
}