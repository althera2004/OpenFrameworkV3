// --------------------------------
// <copyright file="CodedQuery.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Globalization;
    using OpenFrameworkV3.Tools;

    /// <summary>Implements handling of coded querystring</summary>
    public class CodedQuery
    {
        /// <summary>Encrypted data</summary>
        private string stringQuery;

        /// <summary>Encrypted data</summary>
        private NameValueCollection query;

        /// <summary>Decrypted data</summary>
        private List<CodedQueryItem> data;

        /// <summary>Indicates if it is parsed</summary>
        private bool parsed;

        /// <summary>Gets or sets a value indicating whether querystring is cleaned</summary>
        public bool CodedQueryClean { get; set; }

        /// <summary>Gets a value indicating whether querystring has values</summary>
        public bool HasData
        {
            get
            {
                if (this.query != null)
                {
                    return this.query.Count > 0;
                }

                return false;
            }
        }

        /// <summary>Gets a clean CodedQuery data</summary>
        public string UncodedQuery
        {
            get
            {
                if (!string.IsNullOrEmpty(this.stringQuery))
                {
                    return Basics.Base64Decode(this.stringQuery);
                }

                return Basics.Base64Decode(this.query.ToString());
            }
        }

        public CodedQuery(NameValueCollection queryParameters)
        {
            this.SetQuery(queryParameters);
            this.Parse();
        }

        /// <summary>Gets all data from coded query</summary>
        public ReadOnlyDictionary<string, string> All
        {
            get
            {
                var res = new Dictionary<string, string>();

                if (this.CodedQueryClean)
                {

                }
                else
                {
                    if (!this.parsed)
                    {
                        this.Parse();
                    }

                    if (this.data != null && this.data.Count > 0)
                    {
                        foreach (var pair in this.data)
                        {
                            res.Add(pair.Key, pair.Value as string);
                        }
                    }
                }

                return new ReadOnlyDictionary<string, string>(res);
            }
        }

        /// <summary>Gets list parameters from query</summary>
        public ReadOnlyDictionary<string, string> ListParameters
        {
            get
            {
                var res = new Dictionary<string, string>();
                if (!this.parsed)
                {
                    this.Parse();
                }

                if (this.data != null && this.data.Count > 0)
                {
                    foreach (var pair in this.data)
                    {
                        if (pair.Key.StartsWith("plist", StringComparison.OrdinalIgnoreCase))
                        {
                            res.Add(pair.Key, pair.Value as string);
                        }
                    }
                }

                return new ReadOnlyDictionary<string, string>(res);
            }
        }

        /// <summary>Gets the collection of decrypted values</summary>
        public ReadOnlyCollection<CodedQueryItem> Data
        {
            get
            {
                if (!this.parsed)
                {
                    this.Parse();
                    this.parsed = true;
                }

                return new ReadOnlyCollection<CodedQueryItem>(this.data);
            }
        }

        /// <summary>Set the query</summary>
        /// <param name="queryParameters">Query data</param>
        public void SetQuery(NameValueCollection queryParameters)
        {
            this.query = queryParameters;
        }

        /// <summary>Set the query</summary>
        /// <param name="queryParameters">Query data</param>
        public void SetQuery(string queryParameters)
        {
            this.stringQuery = queryParameters + "==";
        }

        /// <summary>Get value of pair by key</summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="key">Key to resolve</param>
        /// <returns>An object of type "T"</returns>
        public T GetByKey<T>(string key)
        {
            try
            {
                if (this.CodedQueryClean)
                {
                    var query = this.query["d"] as string;
                    if (!string.IsNullOrEmpty(query))
                    {
                        switch (key.ToUpperInvariant())
                        {
                            case "ITEMTYPEID":
                                return (T)Convert.ChangeType(Convert.ToInt64(query.Split('.')[0]), typeof(T), CultureInfo.InvariantCulture);
                            case "LISTID":
                            case "FORMID":
                                return (T)Convert.ChangeType(query.Split('.')[1] as string, typeof(T), CultureInfo.InvariantCulture);
                            case "OPTIONID":
                                return (T)Convert.ChangeType(Convert.ToInt64(query.Split('.')[2]), typeof(T), CultureInfo.InvariantCulture);
                            case "ITEMID":
                                return (T)Convert.ChangeType(Convert.ToInt64(query.Split('.')[3]), typeof(T), CultureInfo.InvariantCulture);
                        }
                    }

                    query = this.query["up"] as string;
                    if (!string.IsNullOrEmpty(query))
                    {
                        switch (key.ToUpperInvariant())
                        {
                            case "TAB":
                                return (T)Convert.ChangeType(query.Split('.')[1] as string, typeof(T), CultureInfo.InvariantCulture);
                        }
                    }

                    return (T)Convert.ChangeType(default(T), typeof(T), CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {

            }

            if (!this.parsed)
            {
                this.Parse();
            }

            if (this.data == null || this.data.Count == 0)
            {
                return (T)Convert.ChangeType(default(T), typeof(T), CultureInfo.InvariantCulture);
            }

            foreach (var pair in this.data)
            {
                if (pair.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    return (T)Convert.ChangeType(pair.Value, typeof(T), CultureInfo.InvariantCulture);
                }
            }

            return (T)Convert.ChangeType(default(T), typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>Parse encrypted data to decrypted KeyValuePair collection</summary>
        private void Parse()
        {
            if (this.data == null)
            {
                var res = new List<CodedQueryItem>();
                var parts = this.UncodedQuery.Split('&');
                foreach (string part in parts)
                {
                    if (!string.IsNullOrEmpty(part))
                    {
                        if (part.IndexOf('=') != -1)
                        {
                            res.Add(new CodedQueryItem { Key = part.Split('=')[0], Value = part.Split('=')[1] });
                        }
                    }
                }

                this.data = res;
                this.parsed = true;
            }
        }

        public static string CreateListQuery(string listId, long optionId, long itemDefinitionId, bool clean)
        {
            var pattern = clean ? "d={0}.{1}.{2}.{3}" : "itemTypeId={0}&listid={1}&optionId={2}&ac={3}";
            var query = string.Format(
               CultureInfo.InvariantCulture,
               pattern,
               itemDefinitionId, string.IsNullOrEmpty(listId) ? "custom" : listId,
               optionId,
               Guid.NewGuid());

            if (!clean)
            {
                query = Basics.Base64Encode(query);
            }

            return string.Format(CultureInfo.InvariantCulture, "/ItemList.aspx?{0}", query);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CodedQuery))
            {
                return false;
            }

            var query = (CodedQuery)obj;
            return stringQuery == query.stringQuery &&
                   EqualityComparer<NameValueCollection>.Default.Equals(this.query, query.query) &&
                   EqualityComparer<List<CodedQueryItem>>.Default.Equals(data, query.data) &&
                   parsed == query.parsed &&
                   UncodedQuery == query.UncodedQuery &&
                   EqualityComparer<ReadOnlyDictionary<string, string>>.Default.Equals(All, query.All) &&
                   EqualityComparer<ReadOnlyDictionary<string, string>>.Default.Equals(ListParameters, query.ListParameters) &&
                   EqualityComparer<ReadOnlyCollection<CodedQueryItem>>.Default.Equals(Data, query.Data);
        }

        public override int GetHashCode()
        {
            var hashCode = -775695124;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(stringQuery);
            hashCode = hashCode * -1521134295 + EqualityComparer<NameValueCollection>.Default.GetHashCode(query);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<CodedQueryItem>>.Default.GetHashCode(data);
            hashCode = hashCode * -1521134295 + parsed.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UncodedQuery);
            hashCode = hashCode * -1521134295 + EqualityComparer<ReadOnlyDictionary<string, string>>.Default.GetHashCode(All);
            hashCode = hashCode * -1521134295 + EqualityComparer<ReadOnlyDictionary<string, string>>.Default.GetHashCode(ListParameters);
            hashCode = hashCode * -1521134295 + EqualityComparer<ReadOnlyCollection<CodedQueryItem>>.Default.GetHashCode(Data);
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
