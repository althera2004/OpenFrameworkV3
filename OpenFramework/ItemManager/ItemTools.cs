// --------------------------------
// <copyright file="ItemTools.cs" company="Althera2004">
//     Copyright (c) Althera2004. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace AltheraFramework.ItemManager
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public static class ItemTools
    {
        /*public static string GetPathScripts(ItemDefinition definition)
        {
            if (definition == null)
            {
                return string.Empty;
            }

            return GetPathScripts(definition.Instance.Name, definition.ItemName);
        }*/

        /*public static string GetPathScriptsList(ItemDefinition definition)
        {
            if (definition == null)
            {
                return string.Empty;
            }

            return GetPathScriptsList(definition.Instance.Name, definition.ItemName);
        }*/

        /*public static string GetPathScripts(string instanceName, string itemName)
        {
            if (string.IsNullOrEmpty(instanceName) || string.IsNullOrEmpty(itemName))
            {
                return string.Empty;
            }

            string path = string.Format(
                CultureInfo.InvariantCulture,
                ConfigurationManager.AppSettings["ItemsDefinitionPath"].Replace("ItemDefinition", "Scripts"),
                instanceName);

            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}.js", path, itemName);
            }
            else
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}{1}.js", path, itemName);
            }

            return path;
        }*/

        /*public static string GetPathScriptsList(string instanceName, string itemName)
        {
            if (string.IsNullOrEmpty(instanceName) || string.IsNullOrEmpty(itemName))
            {
                return string.Empty;
            }

            string path = string.Format(
                CultureInfo.InvariantCulture,
                ConfigurationManager.AppSettings["ItemsDefinitionPath"].Replace("ItemDefinition", "Scripts"),
                instanceName);

            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}List.js", path, itemName);
            }
            else
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}{1}List.js", path, itemName);
            }

            return path;
        }*/

        /*public static string SelectQueryBasic(ItemDefinition definition)
        {
            if (definition == null)
            {
                return string.Empty;
            }

            var query = new StringBuilder("SELECT Id,CompanyId,");
            foreach (ItemField field in definition.Fields.Where(f => f.DataType != FieldDataType.Image))
            {
                if (!definition.ForeignValues.Any(l => !string.IsNullOrEmpty(l.LinkField) && l.LocalName == field.Name))
                {
                    query.AppendFormat(CultureInfo.InvariantCulture, @"[{0}],", field.Name);
                }
            }

            query.AppendFormat("Active FROM ITEM_{0} WITH(NOLOCK)", definition.ItemName);
            return query.ToString();
        }*/

        /*public static string SelectQueryAdapter(ItemDefinition definition)
        {
            if (definition == null)
            {
                return string.Empty;
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"EXEC {0}",
                definition.DataAdapter.GetAll.StoredName);
        }*/

        /*public static string SelectQuery(ItemDefinition definition)
        {
            if (definition == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(definition.DataAdapter.GetAll.StoredName))
            {
                return SelectQueryAdapter(definition);
            }
            else
            {
                return SelectQueryBasic(definition);
            }
        }*/

        /*public static ReadOnlyCollection<ItemField> FieldsDescription(ItemDefinition definition)
        {
            var res = new List<ItemField>();
            if (definition.Layout.Description != null)
            {
                foreach (var field in definition.Layout.Description.Fields)
                {
                    foreach(var fieldData in definition.Fields)
                    {
                        if(fieldData.Name == field.Name)
                        {
                            res.Add(fieldData);
                            break;
                        }
                    }
                }
            }

            return new ReadOnlyCollection<ItemField>(res);
        }*/

        /*public static ReadOnlyCollection<ItemField> FieldForeingLines(ItemDefinition definition)
        {
            var fields = definition.Fields.Where(i => definition.FKList.Fields.Contains(i.Name)).ToList();
            return new ReadOnlyCollection<ItemField>(fields.Where(f => FieldsDescription(definition).Select(a => a.Name).ToList().Contains(f.Name) == false).ToList());
        }*/
    }
}