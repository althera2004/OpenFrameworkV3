// --------------------------------
// <copyright file="StoredParameterQuery.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.DataAccess
{
    using Newtonsoft.Json;

    public class StoredParameterQuery
    {
        /// <summary>Parameter name</summary>
        [JsonProperty("Name")]
        private readonly string name;

        /// <summary>Name of field</summary>
        [JsonProperty("Value")]
        private readonly string value;

        /// <summary>Name of field</summary>
        [JsonProperty("Type")]
        private readonly string type;

        /// <summary>Gets the name of parameter</summary>
        [JsonIgnore]
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(this.name))
                {
                    return string.Empty;
                }

                return this.name;
            }
        }

        /// <summary>Gets the value of parameter</summary>
        [JsonIgnore]
        public string Value
        {
            get
            {
                if (string.IsNullOrEmpty(this.value))
                {
                    return string.Empty;
                }

                return this.value;
            }
        }

        /// <summary>Gets the type of parameter</summary>
        [JsonIgnore]
        public string Type
        {
            get
            {
                if (string.IsNullOrEmpty(this.type))
                {
                    return "nvarchar";
                }

                return this.type;
            }
        }
    }
}
