// --------------------------------
// <copyright file="Persistence.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using OpenFrameworkV3.Mail;
    using OpenFrameworkV3.Core;
    using OpenFrameworkV3.Core.ItemManager;
    using System.Globalization;

    public static class Persistence
    {
        private static Dictionary<string, Instance> instances;
        private static Dictionary<string, string> connectionsString;

        private static Dictionary<string, Dictionary<string,string>> dictionary;

        private static string DictionaryKey(string language, string instanceName)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}|{1}", instanceName, language).ToUpperInvariant();
        }

        public static void DictionaryAdd(string language, string instanceName)
        {
            var key = DictionaryKey(language, instanceName);
            var corpus = ApplicationDictionary.Load(language, instanceName);

            if (dictionary == null)
            {
                dictionary = new Dictionary<string, Dictionary<string, string>>
                {
                    { key, corpus }
                };
            }
            else
            {
                if (dictionary.ContainsKey(key))
                {
                    dictionary.Remove(key);
                }

                dictionary.Add(key, corpus);
            }
        }

        public static ReadOnlyDictionary<string, string> Dictionary(string language, string instanceName)
        {
            if(DictionaryExists(language, instanceName))
            {
                return new ReadOnlyDictionary<string, string>(dictionary[DictionaryKey(language, instanceName)]);
            }

            return new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
        }

        public static bool DictionaryExists(string language, string instanceName)
        {
            if (dictionary != null)
            {
                var key = DictionaryKey(language, instanceName);
                if (dictionary.ContainsKey(key))
                {
                    return true;
                }
            }

            return false;
        }

        public static void DictionaryCheck(string language, string instanceName)
        {
            instanceName = instanceName.ToUpperInvariant();
            language = language.ToUpperInvariant();
            if(!DictionaryExists(language, instanceName))
            {
                DictionaryAdd(language, instanceName);
            }
        }       

        public static ReadOnlyCollection<ItemDefinition> ItemDefinitions(string instanceName)
        {
            return InstanceByName(instanceName).ItemDefinitions;
        }

        public static ItemDefinition ItemDefinitionByName(string itemName, string instanceName)
        {
            var res = ItemDefinition.Empty;

            var instance = InstanceByName(instanceName);
            if(instance.ItemDefinitions.Any(d=>d.ItemName.Equals(itemName, System.StringComparison.OrdinalIgnoreCase))) {
                res = instance.ItemDefinitions.First(d => d.ItemName.Equals(itemName, System.StringComparison.OrdinalIgnoreCase));
            }

            return res;
        }

        public static ItemDefinition ItemDefinitionById(long definitionId, string instanceName)
        {
            var res = ItemDefinition.Empty;

            var instance = InstanceByName(instanceName);
            if (instance.ItemDefinitions.Any(d => d.Id == definitionId))
            {
                res = instance.ItemDefinitions.First(d => d.Id == definitionId);
            }

            return res;
        }

        public static long ItemDefinitionIdByName(string itemName, string instanceName)
        {
            var res = Constant.DefaultId;

            var instance = InstanceByName(instanceName);
            if (instance.ItemDefinitions.Any(d => d.ItemName.Equals(itemName, System.StringComparison.OrdinalIgnoreCase)))
            {
                res = instance.ItemDefinitions.First(d => d.ItemName.Equals(itemName, System.StringComparison.OrdinalIgnoreCase)).Id;
            }

            return res;
        }

        public static void AddConnectionString(string instanceName, string connectionString)
        {
            instanceName = instanceName.ToUpperInvariant();
            if (connectionsString == null)
            {
                connectionsString = new Dictionary<string, string>
                {
                    { instanceName, connectionString }
                };

                return;
            }

            instanceName = instanceName.ToUpperInvariant();
            if (!connectionsString.ContainsKey(instanceName))
            {
                connectionsString.Add(instanceName, connectionString);
            }
            else
            {
                connectionsString[instanceName] = connectionString;
            }
        }

        /// <summary>
        /// Gets connecction string to dababase of instance
        /// </summary>
        /// <param name="instanceName">Instance Name</param>
        /// <returns>Connecction string to dababase of instance</returns>
        public static string ConnectionString(string instanceName)
        {
            if (connectionsString == null)
            {
                return string.Empty;
            }

            instanceName = instanceName.ToUpperInvariant();
            if (connectionsString.ContainsKey(instanceName))
            {
                return connectionsString[instanceName];
            }

            return string.Empty;
        }

        /// <summary>Gets instance by name</summary>
        /// <param name="instanceName">Name of instance</param>
        /// <returns>Instance object</returns>
        public static Instance InstanceByName(string instanceName)
        {
            if (string.IsNullOrEmpty(instanceName))
            {
                return Instance.Empty;
            }

            if (instances != null && instances.ContainsKey(instanceName.ToUpperInvariant()))
            {
                return instances[instanceName.ToUpperInvariant()];
            }
            else
            {
                var instance = Instance.LoadDefinition(instanceName);
                Persistence.AddInstance(instance);
                return instance;
            }
        }

        /// <summary>Check is exists instance</summary>
        /// <param name="instanceName">Name of instance</param>
        /// <returns>A value indicating if instance exists</returns>
        public static bool InstanceExists(string instanceName)
        {
            instanceName = instanceName.ToUpperInvariant();

            if (instances == null)
            {
                return false;
            }

            return instances.ContainsKey(instanceName);
        }

        /// <summary>Adds instance to persistence</summary>
        /// <param name="instance">Instance object</param>
        public static void AddInstance(Instance instance)
        {
            instance.Name = instance.Name.ToUpperInvariant();
            if (instances == null)
            {
                instances = new Dictionary<string, Instance>
                {
                    { instance.Name, instance }
                };

                AddConnectionString(instance.Name, instance.Config.ConnectionString);
                return;
            }

            bool exists = false;
            foreach (var i in instances)
            {
                if (i.Value.Id == instance.Id)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                instances.Add(instance.Name.ToUpperInvariant(), instance);
            }
            else
            {
                instances.Remove(instance.Name);
                instances.Add(instance.Name, instance);
            }

            AddConnectionString(instance.Name, instance.Config.ConnectionString);
        }

        /// <summary>Gets the list of names of instances</summary>
        /// <returns>List of names of instances</returns>
        public static ReadOnlyCollection<string> ListOfInstances
        {
            get
            {
                var res = new List<string>();
                if (instances != null)
                {
                    foreach (var instance in instances)
                    {
                        res.Add(instance.Key);
                    }
                }

                return new ReadOnlyCollection<string>(res);
            }
        }
    }
}