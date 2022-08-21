// --------------------------------
// <copyright file="ApplicationDictionary.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using OpenFrameworkV3.Core;
    using OpenFrameworkV3.Core.Activity;

    /// <summary>Implements dictionary class</summary>
    public static class ApplicationDictionary
    {
        /// <summary>Loads the new language.</summary>
        /// <param name="language">Language code</param>
        /// <returns>A dictionary class with text for fixed labels</returns>
        /// <param name="instanceName">Name of instance</param>
        public static Dictionary<string, string> LoadNewLanguage(string language, string instanceName)
        {
            var instance = Persistence.InstanceByName(instanceName);
            var dictionary = LoadValuesFromFile("Core", language);

            var featuresDictionary = LoadValuesFromFile("Features", language);
            foreach (var value in featuresDictionary)
            {
                if (dictionary.ContainsKey(value.Key))
                {
                    dictionary[value.Key] = value.Value;
                }
                else
                {
                    dictionary.Add(value.Key, value.Value);
                }
            }

            if (instance.Config.Billing != null)
            {
                var billingDictionary = LoadValuesFromFile("Billing", language);
                foreach(var value in billingDictionary)
                {
                    if (dictionary.ContainsKey(value.Key))
                    {
                        dictionary[value.Key] = value.Value;
                    }
                    else
                    {
                        dictionary.Add(value.Key, value.Value);
                    }
                }
            }

            var instanceDictionaryFile = string.Format(CultureInfo.InvariantCulture, @"{0}\core_{1}.dicc", Instance.Path.Dictionary(instanceName), language);
            if(File.Exists(instanceDictionaryFile))
            {
                var instanceDictionary = LoadValuesFromFile(instanceDictionaryFile);
                foreach (var value in instanceDictionary)
                {
                    if (dictionary.ContainsKey(value.Key))
                    {
                        dictionary[value.Key] = value.Value;
                    }
                    else
                    {
                        dictionary.Add(value.Key, value.Value);
                    }
                }
            }

            var items = from pair in dictionary
                        orderby pair.Key ascending
                        select pair;
            var dictionaryFinal = new Dictionary<string, string>();
            foreach (var item in items)
            {
                dictionaryFinal.Add(item.Key, item.Value);
            }

            HttpContext.Current.Session["Dictionary"] = dictionaryFinal;
            return dictionaryFinal;
        }

        /// <summary>Load a dictionary</summary>
        /// <param name="language">Code of language</param>
        /// <param name="instanceName">Name of instance</param>
        /// <returns>A dictionary class with text for fixed labels</returns>
        public static Dictionary<string, string> Load(string language, string instanceName)
        {
            var instance = Persistence.InstanceByName(instanceName);
            var dictionary = LoadValuesFromFile("Core", language);

            var featuresDictionary = LoadValuesFromFile("Features", language);
            foreach (var value in featuresDictionary)
            {
                if (dictionary.ContainsKey(value.Key))
                {
                    dictionary[value.Key] = value.Value;
                }
                else
                {
                    dictionary.Add(value.Key, value.Value);
                }
            }

            if (instance.Config.Billing != null)
            {
                var billingDictionary = LoadValuesFromFile("Billing", language);
                foreach (var value in billingDictionary)
                {
                    if (dictionary.ContainsKey(value.Key))
                    {
                        dictionary[value.Key] = value.Value;
                    }
                    else
                    {
                        dictionary.Add(value.Key, value.Value);
                    }
                }
            }

            var instanceDictionaryFile = string.Format(CultureInfo.InvariantCulture, @"{0}\core_{1}.dicc", Instance.Path.Dictionary(instanceName), language);
            if (File.Exists(instanceDictionaryFile))
            {
                var instanceDictionary = LoadValuesFromFile(instanceDictionaryFile);
                foreach (var value in instanceDictionary)
                {
                    if (dictionary.ContainsKey(value.Key))
                    {
                        dictionary[value.Key] = value.Value;
                    }
                    else
                    {
                        dictionary.Add(value.Key, value.Value);
                    }
                }
            }

            foreach(var itemDefinition in instance.ItemDefinitions)
            {
                var itemName = itemDefinition.ItemName;
                var itemDictionaryFile = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}_{2}.dicc", Instance.Path.Dictionary(instanceName), itemName, language);
                var itemDictionary = LoadValuesFromFile(itemDictionaryFile);
                foreach (var value in itemDictionary)
                {
                    var finalKey = itemName + "_" + value.Key;
                    if (dictionary.ContainsKey(finalKey))
                    {
                        dictionary[finalKey] = value.Value;
                    }
                    else
                    {
                        dictionary.Add(finalKey, value.Value);
                    }
                }
            }

            var items = from pair in dictionary
                        orderby pair.Key ascending
                        select pair;
            var dictionaryFinal = new Dictionary<string, string>();
            foreach (var item in items)
            {
                dictionaryFinal.Add(item.Key, item.Value);
            }

            //HttpContext.Current.Session["Dictionary"] = dictionaryFinal;
            return dictionaryFinal;
        }

        /// <summary>Load a dictionary</summary>
        /// <param name="language">Code of language</param>
        /// <param name="instanceName">Name of instance</param>
        /// <returns>A dictionary class with text for fixed labels</returns>
        public static string GetCorpus(string language, string instanceName)
        {
            var instance = Persistence.InstanceByName(instanceName);
            var dictionary = LoadValuesFromFile("Core", language);

            var featuresDictionary = LoadValuesFromFile("Features", language);
            foreach (var value in featuresDictionary)
            {
                if (dictionary.ContainsKey(value.Key))
                {
                    dictionary[value.Key] = value.Value;
                }
                else
                {
                    dictionary.Add(value.Key, value.Value);
                }
            }

            if (instance.Config.Billing != null)
            {
                var billingDictionary = LoadValuesFromFile("Billing", language);
                foreach (var value in billingDictionary)
                {
                    if (dictionary.ContainsKey(value.Key))
                    {
                        dictionary[value.Key] = value.Value;
                    }
                    else
                    {
                        dictionary.Add(value.Key, value.Value);
                    }
                }
            }

            var instanceDictionaryFile = string.Format(CultureInfo.InvariantCulture, @"{0}\core_{1}.dicc", Instance.Path.Dictionary(instanceName), language);
            if (File.Exists(instanceDictionaryFile))
            {
                var instanceDictionary = LoadValuesFromFile(instanceDictionaryFile);
                foreach (var value in instanceDictionary)
                {
                    if (dictionary.ContainsKey(value.Key))
                    {
                        dictionary[value.Key] = value.Value;
                    }
                    else
                    {
                        dictionary.Add(value.Key, value.Value);
                    }
                }
            }

            foreach (var itemDefinition in instance.ItemDefinitions)
            {
                var itemName = itemDefinition.ItemName;
                var itemDictionaryFile = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}_{2}.dicc", Instance.Path.Dictionary(instanceName), itemName, language);
                var itemDictionary = LoadValuesFromFile(itemDictionaryFile);
                foreach (var value in itemDictionary)
                {
                    var finalKey = itemName + "_" + value.Key;
                    if (dictionary.ContainsKey(finalKey))
                    {
                        dictionary[finalKey] = value.Value;
                    }
                    else
                    {
                        dictionary.Add(finalKey, value.Value);
                    }
                }
            }

            var items = from pair in dictionary
                        orderby pair.Key ascending
                        select pair;

            var res = new StringBuilder("[");
            var first = true;
            foreach (var item in items)
            {
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"{2}{{""Key"":""{0}"", ""Value"":""{1}""}}",
                    item.Key,
                    Tools.Json.JsonCompliant(item.Value),
                    first ? string.Empty : ",");
                first = false;
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>Try to get a label from dictionary</summary>
        /// <param name="key">Key of dictionary</param>
        /// <returns>Label based on key or key notation</returns>
        public static string Translate(string key, string language, string instanceName)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            var dictionary = Persistence.Dictionary(language, instanceName);

            if (dictionary != null && dictionary.ContainsKey(key))
            {
                return dictionary[key].Trim();
            }

            return key;
        }

        /// <summary>Indicates if dictionay has a key</summary>
        /// <param name="key">Key to search</param>
        /// <returns></returns>
        public static bool ContainsKey(string key)
        {
            if (!(HttpContext.Current.Session["Dictionary"] is Dictionary<string, string> dictionary))
            {
                return false;
            }

            return dictionary.ContainsKey(key);
        }

        /// <summary>Gets a entry for dictionary JSON</summary>
        /// <param name="key">Item key</param>
        /// <param name="value">Item value</param>
        /// <returns>JSON Code</returns>
        public static string DictionaryItem(string key, string value)
        {
            return string.Format(CultureInfo.InvariantCulture, @"    ""{0}"": ""{1}"",{2}", key, value, Environment.NewLine);
        }

        /// <summary>Creates javascript dictionary of language</summary>
        /// <param name="language">ISO code of language</param>
        /// <param name="instanceName">Name of instance</param>
        public static void CreateJavascriptFile(string language, string instanceName)
        {
            var dictionary = Persistence.Dictionary(language, instanceName);
            var content = new StringBuilder("// " + DateTime.Now.ToShortDateString());
            content.Append(Environment.NewLine);
            content.Append("var Dictionary =" + Environment.NewLine);
            content.Append("{" + Environment.NewLine);

            foreach (var item in dictionary)
            {
                if (!item.Key.StartsWith("Help_") || true)
                {
                    string finalValue = item.Value.Trim().Replace("\"", "\\\"");
                    content.Append(ApplicationDictionary.DictionaryItem(item.Key.Replace(' ', '_'), finalValue));
                }
            }

            content.Append("    \"-\": \"-\"" + Environment.NewLine);
            content.Append("};");

            try
            {
                using (var output = new StreamWriter(DictionaryJavascriptFile(language), false))
                {
                    output.Write(content);
                }
            }
            catch (IOException ex)
            {
                ExceptionManager.Trace(ex, "ApplicationDictionary.CreateJavascriptFile(" + language + ")");
            }
            catch (NullReferenceException ex)
            {
                ExceptionManager.Trace(ex, "ApplicationDictionary.CreateJavascriptFile(" + language + ")");
            }
            catch (Exception ex)
            {
                ExceptionManager.Trace(ex, "ApplicationDictionary.CreateJavascriptFile(" + language + ")");
            }
        }

        /// <summary>Gets the path of javascript dictionary of language</summary>
        /// <param name="language">ISO code of language</param>
        /// <returns>Path of javascript dictionary of language</returns>
        public static string DictionaryJavascriptFile(string language)
        {
            var path = HttpContext.Current.Request.PhysicalApplicationPath;

            if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\Dicc", path);
            }
            else
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}Dicc", path);
            }

            return string.Format(CultureInfo.InvariantCulture, @"{0}\{1}.js", path, language);
        }

        private static Dictionary<string, string> LoadValuesFromFile(string dictionaryName, string language)
        {
            string fileName = string.Format(
                CultureInfo.InvariantCulture,
                "{0}{4}dicc\\{1}{3}{2}.dicc",
                HttpContext.Current.Request.PhysicalApplicationPath,
                dictionaryName,
                language,
                string.IsNullOrEmpty(dictionaryName) ? string.Empty : "_",
                HttpContext.Current.Request.PhysicalApplicationPath.EndsWith("\\", StringComparison.OrdinalIgnoreCase) ? string.Empty : "\\");
            return LoadValuesFromFile(fileName);
        }

        private static Dictionary<string, string> LoadValuesFromFile(string dictionaryFile)
        {
            var res = new Dictionary<string, string>();
            if (File.Exists(dictionaryFile))
            {
                using (var input = new StreamReader(dictionaryFile))
                {
                    string linea = input.ReadLine();
                    while (!string.IsNullOrEmpty(linea))
                    {
                        if (linea.IndexOf("::", StringComparison.Ordinal) != -1)
                        {
                            linea = linea.Replace("::", "^");
                            string key = linea.Split('^')[0];
                            string value = linea.Split('^')[1];

                            if (string.IsNullOrEmpty(value))
                            {
                                value = key;
                            }

                            if (!res.ContainsKey(key))
                            {
                                res.Add(key, value.Replace('\'', '´'));
                            }
                            else
                            {
                                res[key] = value.Replace('\'', '´');
                            }
                        }

                        linea = input.ReadLine();
                    }
                }
            }

            return res;
        }
    }
}