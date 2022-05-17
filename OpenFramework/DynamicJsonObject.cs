// --------------------------------
// <copyright file="DynamicJsonObject.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Dynamic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>Implements DynamicJSONObject class</summary>
    public sealed class DynamicJsonObject : DynamicObject
    {
        /// <summary>Data dictionary</summary>
        private readonly IDictionary<string, object> dictionary;

        /// <summary>Initializes a new instance of the DynamicJsonObject class</summary>
        /// <param name="dictionary">Dictionary of data</param>
        public DynamicJsonObject(IDictionary<string, object> dictionary)
        {
            this.dictionary = dictionary ?? throw new ArgumentNullException("dictionary");
        }

        /// <summary>Gets an string representation of object</summary>
        /// <returns>String representation of object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("{");
            this.ToString(sb);
            return sb.ToString();
        }

        /// <summary>Try to get a member of object</summary>
        /// <param name="binder">Binder to members</param>
        /// <param name="result">Result to host value</param>
        /// <returns>A value indicating if actions is successful finished</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder == null)
            {
                result = null;
                return false;
            }

            if (!this.dictionary.TryGetValue(binder.Name, out result))
            {
                // return null to avoid exception.  caller can check for null this way...
                result = null;
                return true;
            }

            result = WrapResultObject(result);
            return true;
        }

        /// <summary>Try to get a member of object</summary>
        /// <param name="binder">Binder to members</param>
        /// <param name="indexes">Indexes of members</param>
        /// <param name="result">Result to host value</param>
        /// <returns>A value indicating if actions is successful finished</returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (binder == null || indexes == null)
            {
                result = null;
                return false;
            }

            if (indexes.Length == 1 && indexes[0] != null)
            {
                if (!this.dictionary.TryGetValue(indexes[0].ToString(), out result))
                {
                    // return null to avoid exception.  caller can check for null this way...
                    result = null;
                    return true;
                }

                result = WrapResultObject(result);
                return true;
            }

            return base.TryGetIndex(binder, indexes, out result);
        }

        /// <summary>Creates a wrap for result</summary>
        /// <param name="result">Result to wrap</param>
        /// <returns>Complete object</returns>
        private static object WrapResultObject(object result)
        {
            if (result is IDictionary<string, object> dictionary)
            {
                return new DynamicJsonObject(dictionary);
            }

            if (result is ArrayList arrayList && arrayList.Count > 0)
            {
                if (arrayList[0] is IDictionary<string, object>)
                {
                    return new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new DynamicJsonObject(x)));
                }

                return new List<object>(arrayList.Cast<object>());
            }

            return result;
        }

        public ReadOnlyCollection<string> Keys
        {
            get
            {
                var res = new List<string>();
                foreach (var pair in this.dictionary)
                {
                    res.Add(pair.Key);
                }

                return new ReadOnlyCollection<string>(res);
            }
        }

        /// <summary>Get value based on key</summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="key">Key to search</param>
        /// <returns>Value of pair Key/Value, default type value if key not exists</returns>
        public T GetValue<T>(string key)
        {
            foreach (var pair in this.dictionary)
            {
                if (pair.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    switch (typeof(T).Name.ToUpperInvariant())
                    {
                        case "STRING":
                            return (T)Convert.ChangeType(pair.Value, typeof(T), CultureInfo.InvariantCulture);
                        default:
                            return (T)Convert.ChangeType(pair.Value, typeof(T), CultureInfo.InvariantCulture);
                    }
                }
            }

            return default(T);
        }

        /// <summary>Creates a string representations of object</summary>
        /// <param name="sb">StringBuilder object that contains data</param>
        private void ToString(StringBuilder sb)
        {
            var firstInDictionary = true;
            foreach (var pair in this.dictionary)
            {
                if (!firstInDictionary)
                {
                    sb.Append(",");
                }

                firstInDictionary = false;
                var value = pair.Value;
                var name = pair.Key;
                if (value is string)
                {
                    sb.AppendFormat("{0}:\"{1}\"", name, value);
                }
                else if (value is IDictionary<string, object>)
                {
                    new DynamicJsonObject((IDictionary<string, object>)value).ToString(sb);
                }
                else if (value is ArrayList)
                {
                    sb.Append(name + ":[");
                    var firstInArray = true;
                    foreach (var arrayValue in value as ArrayList)
                    {
                        if (!firstInArray)
                        {
                            sb.Append(",");
                        }

                        firstInArray = false;
                        if (arrayValue is IDictionary<string, object>)
                        {
                            new DynamicJsonObject((IDictionary<string, object>)arrayValue).ToString(sb);
                        }
                        else if (arrayValue is string)
                        {
                            sb.AppendFormat("\"{0}\"", arrayValue);
                        }
                        else
                        {
                            sb.AppendFormat("{0}", arrayValue);
                        }
                    }

                    sb.Append("]");
                }
                else
                {
                    sb.AppendFormat("{0}:{1}", name, value);
                }
            }

            sb.Append("}");
        }
    }
}