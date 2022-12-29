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
    using System.IO;
    using System.Web;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;
    using OpenFrameworkV3.Tools;

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

        public static ActionResult DeleteFieldDocument(string itemName, string fieldName,string fileName, long itemId, long applicationUserId, long companyId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                var query = Query.DeleteFieldDocument(itemName, fieldName, itemId, applicationUserId);

                res = new ExecuteQuery()
                {
                    ConnectionString = cns,
                    QueryText = query
                }.ExecuteCommand;

                if (res.Success)
                {
                    var path = HttpContext.Current.Request.PhysicalApplicationPath;
                    if (!path.EndsWith(@"\"))
                    {
                        path = string.Format(CultureInfo.InvariantCulture, @"{0}\\", path);
                    }

                    var folder = string.Format(CultureInfo.InvariantCulture, @"{0}Instances\{1}\Data", path, instanceName);
                    Basics.VerifyFolder(folder);

                    folder = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", folder, itemName);
                    Basics.VerifyFolder(folder);

                    folder = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", folder, itemId);
                    Basics.VerifyFolder(folder);

                    // si el documento es de un campo, el nombre es el del campo
                    fileName = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{0}\{1}",
                        folder,
                        fileName);

                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    var trace = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{5},{{{5}{4}{4}""user"":""{0}"",{5}{4}{4}""date"": ""{1:dd/MM/yyyy hh:mm:ss}"",{5}{4}{4}""changes"":
                        [{{
                            ""Field"": ""{2}"",
                            ""Original"": ""Eliminar documento"",
                            ""Actual"": ""{3}""
                        }}]

                        }}",
                        ApplicationUser.ById(applicationUserId, instanceName).Profile.FullName,
                        DateTime.UtcNow,
                        fieldName,
                        Path.GetFileName(fileName),
                        '\t',
                        '\n');


                    ActionLog.TraceItemData(instanceName, itemName, itemId, trace);
                }
            }


            return res;
        }
    }
}