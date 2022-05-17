// --------------------------------
// <copyright file="JsonConnector.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using OpenFrameworkV3.Core.ItemManager;

    public class JsonConnector
    {
        public static T ById<T>(long id, ItemDefinition definition, string instanceName)
        {
            var res = new ItemData(); // weke new Item(definition.ItemName, definition, instanceName);
            var path = Instance.Path.DataJson(instanceName);
            var fileName = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}_{2}.datax", path, definition.ItemName, id);
            var json = string.Empty;
            if (File.Exists(fileName))
            {
                using (var input = new StreamReader(fileName))
                {
                    json = input.ReadToEnd();
                }

               //weke  res = Item.FromJsonString(definition.ItemName, json, instanceName);
            }

            switch (typeof(T).Name.ToUpperInvariant())
            {
                case "STRING":
                    return (T)Convert.ChangeType(json, typeof(T), CultureInfo.InvariantCulture);
                default:
                    return (T)Convert.ChangeType(res, typeof(T), CultureInfo.InvariantCulture);
            }
        }

        public static T All<T>(ItemDefinition definition, string instanceName)
        {
            var list = new List<ItemData>();
            var path = Instance.Path.DataJson(instanceName);
            var pattern = string.Format(CultureInfo.InvariantCulture, @"{0}_*.datax", definition.ItemName);
            var myFiles = Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly);
            foreach (var file in myFiles)
            {
                var json = string.Empty;
                using (var input = new StreamReader(file))
                {
                    json = input.ReadToEnd();
                }

                // weke list.Add(ItemDefinition.FromJsonString(definition.ItemName, json, instanceName));
            }

            switch (typeof(T).Name.ToUpperInvariant())
            {
                case "STRING":
                    return (T)Convert.ChangeType(AllString(definition, instanceName), typeof(T), CultureInfo.InvariantCulture);
                default:
                    return (T)Convert.ChangeType(AllItemBuilder(definition, instanceName), typeof(T), CultureInfo.InvariantCulture);
            }
        }

        private static ReadOnlyCollection<ItemData> AllItemBuilder(ItemDefinition definition, string instanceName)
        {
            var list = new List<ItemData>();
            var path = Instance.Path.DataJson(instanceName);
            var pattern = string.Format(CultureInfo.InvariantCulture, @"{0}_*.datax", definition.ItemName);
            var myFiles = Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly);
            foreach (var file in myFiles)
            {
                var json = string.Empty;
                using (var input = new StreamReader(file))
                {
                    json = input.ReadToEnd();
                }

                if (!string.IsNullOrEmpty(json))
                {
                    // weke list.Add(Item.FromJsonString(definition.ItemName, json, instanceName));
                }
            }

            return new ReadOnlyCollection<ItemData>(list);
        }

        private static string AllString(ItemDefinition definition, string instanceName)
        {
            var list = new StringBuilder("[");
            var path = Instance.Path.DataJson(instanceName);
            var pattern = string.Format(CultureInfo.InvariantCulture, @"{0}_*.datax", definition.ItemName);
            var myFiles = Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly);
            bool first = true;
            foreach (var file in myFiles)
            {
                var json = string.Empty;
                using (var input = new StreamReader(file))
                {
                    json = input.ReadToEnd();
                }

                if (first)
                {
                    first = false;
                }
                else
                {
                    list.Append(",");
                }

                if (!string.IsNullOrEmpty(json))
                {
                    list.Append(json);
                }
            }

            list.Append("]");
            return list.ToString();
        }

        public static string JsonActive(string itemName, string instanceName)
        {
            var res = new StringBuilder("[");
            var definition = Persistence.ItemDefinitionByName(itemName, instanceName);
            var data = All<ReadOnlyCollection<ItemData>>(definition, instanceName).Where(d => Convert.ToBoolean(d["Active"]) == true);
            bool first = true;
            foreach (var item in data)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(item.Json);
            }

            res.Append("]");
            return res.ToString();
        }
    }
}