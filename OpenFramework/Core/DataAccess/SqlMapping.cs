// --------------------------------
// <copyright file="SqlMapping.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.DataAccess
{
    using System;
    using Newtonsoft.Json;

    /// <summary>Mapping for item filed to table field</summary>
    [Serializable]
    public sealed class SqlMapping
    {
        /// <summary>The name of table field</summary>
        [JsonProperty("TableField")]
        private readonly string tableField;

        /// <summary>Gets or sets the name of item field</summary>
        [JsonProperty("ItemField")]
        public string ItemField { get; set; }

        /// <summary>Gets or sets the default value for field</summary>
        [JsonProperty("DefaultValue")]
        public string DefaultValue { get; set; }

        /// <summary>Gets the name of table field</summary>
        [JsonIgnore]
        public string TableField
        {
            get
            {
                if (string.IsNullOrEmpty(this.tableField))
                {
                    return this.ItemField;
                }

                return this.tableField;
            }
        }
    }
}