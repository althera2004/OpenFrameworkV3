﻿namespace OpenFrameworkV3.Core.ItemManager.ItemList
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>Implements parameters for list</summary>
    public sealed class ListParameter
    {
        /// <summary>Gets or sets parameter name</summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>Gets or sets parameter value</summary>
        [JsonProperty("Value")]
        public string Value { get; set; }

        public static Dictionary<string, string> DictionaryFromListParameter(ReadOnlyCollection<ListParameter> parameters)
        {
            var res = new Dictionary<string, string>();
            foreach(var param in parameters)
            {
                res.Add(param.Name, param.Value);
            }

            return res;
        }
    }
}