// --------------------------------
// <copyright file="Field.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Reports
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>Defines the <see cref="Field" />.</summary>
    public class Field
    {
        /// <summary>Defines the data.</summary>
        [JsonProperty("Data")]
        private readonly FieldData[] data;

        /// <summary>Gets or sets the FieldName.</summary>
        [JsonProperty("FieldName")]
        public string FieldName { get; set; }

        /// <summary>Gets or sets the DataType.</summary>
        [JsonProperty("DataType")]
        public string DataType { get; set; }

        /// <summary>Gets or sets the Pattern.</summary>
        [JsonProperty("Pattern")]
        public string Pattern { get; set; }

        /// <summary>Gets or sets the Value.</summary>
        [JsonProperty("Value")]
        public string Value { get; set; }

        /// <summary>Gets or sets literal text</summary>
        [JsonProperty("Text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the GroupId.
        /// </summary>
        [JsonProperty("GroupId")]
        public long GroupId { get; set; }

        /// <summary>Gets the Data.</summary>
        [JsonIgnore]
        public ReadOnlyCollection<FieldData> Data
        {
            get
            {
                if (this.data == null)
                {
                    return new ReadOnlyCollection<FieldData>(new List<FieldData>());
                }

                return new ReadOnlyCollection<FieldData>(this.data.ToList());
            }
        }
    }
}
