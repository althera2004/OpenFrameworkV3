// --------------------------------
// <copyright file="SecurityConfiguration.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;

    public class ItemData : IDictionary<string, object>, ICollection
    {
        private Dictionary<string, object> values;

        public object GetValue(string key)
        {
            if (this.values == null)
            {
                return null;
            }

            if (!this.values.ContainsKey(key))
            {
                return null;
            }

            return this.values[key];
        }

        public T GetValue<T>(string key)
        {
            return (T)Convert.ChangeType(GetValue(key), typeof(T), CultureInfo.InvariantCulture);
        }

        public static ItemData Empty
        {
            get
            {
                return new ItemData();
            }
        }

        public object this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ICollection<string> Keys
        {
            get
            {
                return this.values.Keys;
            }
        }

        public ICollection<object> Values
        {
            get
            {
                return this.values.Values;
            }
        }
        public int Count
        {
            get
            {
                return this.values.Keys.Count;
            }
        }

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(string key, object value)
        {
            if (this.values == null)
            {
                this.values = new Dictionary<string, object>();
            }

            if (!this.values.ContainsKey(key))
            {
                this.values.Add(key, value);
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            if (this.values == null)
            {
                this.values = new Dictionary<string, object>();
            }

            if (!this.values.ContainsKey(item.Key))
            {
                this.values.Add(item.Key, item.Value);
            }
        }

        public void Clear()
        {
            if (this.values == null)
            {
                this.values = new Dictionary<string, object>();
            }

            this.values.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            if (this.values == null)
            {
                return false;
            }

            if (!this.values.ContainsKey(item.Key))
            {
                return false;
            }

            return this.values[item.Key] == values;
        }
        public bool ContainsKey(string key)
        {
            if (this.values == null)
            {
                return false;
            }

            return this.values.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        public bool Remove(string key)
        {
            if (this.values == null)
            {
                return false;
            }

            return this.values.Remove(key);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            if (this.values == null)
            {
                return false;
            }

            return this.values.Remove(item.Key);
        }

        public bool TryGetValue(string key, out object value)
        {
            value = GetValue(key);
            return value != null;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public string Json
        {
            get
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        public object SyncRoot => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();
    }
}