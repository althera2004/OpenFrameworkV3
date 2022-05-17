// --------------------------------
// <copyright file="SqlConnector.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Dapper;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Configuration;
    using OpenFrameworkV3.Core.ItemManager;

    public class SqlConnector
    {
        public static T ById<T>(long id, ItemDefinition definition, string instanceName)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"SqlConnector::ById({0},{1},{2}", id, definition.ItemName, instanceName);
            string query = string.Empty;
            if (!string.IsNullOrEmpty(definition.DataAdapter.GetAll.StoredName))
            {
                query = string.Format(CultureInfo.InvariantCulture, @"EXEC {0}", definition.DataAdapter.GetAll.StoredName);
            }
            else
            {
                var instance = Persistence.InstanceByName(instanceName);
                var q = new StringBuilder("SELECT Item.Id,Item.CompanyId,");
                foreach (var field in definition.Fields.Where(f => !f.TypeName.Equals("ImageGallery", StringComparison.OrdinalIgnoreCase)))
                {
                    if (!definition.ForeignValues.Any(l => !string.IsNullOrEmpty(l.LinkField) && l.LocalName == field.Name))
                    {
                        q.AppendFormat(CultureInfo.InvariantCulture, @"Item.[{0}],", field.Name);
                    }
                }

                string modifiedBy = "ISNULL(CONCAT(ISNULL(MB.[Name], ''), ' ', ISNULL(MB.LastName, '')), '')";
                if (instance.Config.Profile.NameFormat == ProfileConfiguration.NameFormatComplete)
                {
                    modifiedBy = "ISNULL(MB.[Name], '')";
                }
                else if (instance.Config.Profile.NameFormat == ProfileConfiguration.NameFormatFirstName2LastName)
                {
                    modifiedBy = "ISNULL(CONCAT(ISNULL(MB.[Name], ''), ' ', ISNULL(MB.LastName, ''), ' ', ISNULL(MB.LastName2,'')), '')";
                }

                q.AppendFormat(@"
                        {2} AS ModifiedBy,
                        ISNULL(CONVERT(VARCHAR(20), Item.ModifiedOn, 103),'') AS ModifiedOn,
                        Item.Active
                    FROM ITEM_{0} Item WITH(NOLOCK) 
	                LEFT JOIN Core_Profile MB WITH(NOLOCK)
	                ON	MB.ApplicationUserId = Item.ModifiedBy
                    WHERE Item.Id = {1}",
                    definition.ItemName,
                    id,
                    modifiedBy);
                query = q.ToString();
            }

            var list = new List<ItemData>();
            using (var db = new SqlConnection(Persistence.ConnectionString(instanceName)))
            {
                try
                {
                    var result = db.Query(query, true);
                    var dataDB = new ReadOnlyCollection<dynamic>(result.Select(r => (dynamic)r).ToList());
                    foreach (dynamic itemRead in dataDB)
                    {
                        var itembuilder = ItemDefinition.FromDynamic(instanceName, definition.ItemName, itemRead);
                        list.Add(itembuilder);
                    }
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (IndexOutOfRangeException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (IOException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (NotSupportedException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
            }

            if(list != null && list.Count > 0){
                var res = list.First();
                switch (typeof(T).Name.ToUpperInvariant())
                {
                    case "STRING":
                        return (T)Convert.ChangeType(res.Json, typeof(T), CultureInfo.InvariantCulture);
                    default:
                        return (T)Convert.ChangeType(res, typeof(T), CultureInfo.InvariantCulture);
                }
            }
            else
            {
                switch (typeof(T).Name.ToUpperInvariant())
                {
                    case "STRING":
                        return (T)Convert.ChangeType("{\"Id\":-1}", typeof(T), CultureInfo.InvariantCulture);
                    default:
                        return (T)Convert.ChangeType(new ItemData(), typeof(T), CultureInfo.InvariantCulture);
                }
            }
        }

        public static T ByFieldValue<T>(string fieldName, string  fieldValue, ItemDefinition definition, string instanceName)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"SqlConnector::ByFieldValue({0},{1},{2},{3}", fieldName, fieldValue, definition.ItemName, instanceName);
            string query = string.Empty;
            if (!string.IsNullOrEmpty(definition.DataAdapter.GetAll.StoredName))
            {
                query = string.Format(CultureInfo.InvariantCulture, @"EXEC {0}", definition.DataAdapter.GetAll.StoredName);
            }
            else
            {
                var q = new StringBuilder("SELECT Item.Id,Item.CompanyId,");
                foreach (var field in definition.Fields.Where(f => !f.TypeName.Equals("ImageGallery", StringComparison.OrdinalIgnoreCase)))
                {
                    if (!definition.ForeignValues.Any(l => !string.IsNullOrEmpty(l.LinkField) && l.LocalName == field.Name))
                    {
                        q.AppendFormat(CultureInfo.InvariantCulture, @"Item.[{0}],", field.Name);
                    }
                }

                string modifiedBy = "ISNULL(CONCAT(ISNULL(MB.[Name], ''), ' ', ISNULL(MB.LastName, '')), '')";
                using (var instance = Persistence.InstanceByName(instanceName))
                {
                    if (instance.Config.Profile.NameFormat == ProfileConfiguration.NameFormatComplete)
                    {
                        modifiedBy = "ISNULL(MB.[Name], '')";
                    }
                    else if (instance.Config.Profile.NameFormat == ProfileConfiguration.NameFormatFirstName2LastName)
                    {
                        modifiedBy = "ISNULL(CONCAT(ISNULL(MB.[Name], ''), ' ', ISNULL(MB.LastName, ''), ' ', ISNULL(MB.LastName2,'')), '')";
                    }
                }

                q.AppendFormat(@"
                        {3} AS ModifiedBy,
                        ISNULL(CONVERT(VARCHAR(20), Item.ModifiedOn, 103),'') AS ModifiedOn,
                        Item.Active
                    FROM ITEM_{0} Item WITH(NOLOCK) 
	                LEFT JOIN Core_Profile MB WITH(NOLOCK)
	                ON	MB.ApplicationUserId = Item.ModifiedBy
                    WHERE Item.{1} = '{2}'",
                    definition.ItemName,
                    fieldName,
                    fieldValue,
                    modifiedBy);
                query = q.ToString();
            }

            var list = new List<ItemData>();
            using (var db = new SqlConnection(Persistence.ConnectionString(instanceName)))
            {
                try
                {
                    var result = db.Query(query, true);
                    var dataDB = new ReadOnlyCollection<dynamic>(result.Select(r => (dynamic)r).ToList());
                    foreach (dynamic itemRead in dataDB)
                    {
                        var itembuilder = ItemDefinition.FromDynamic(instanceName, definition.ItemName, itemRead);
                        list.Add(itembuilder);
                    }
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (IndexOutOfRangeException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (IOException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (NotSupportedException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
            }

            if (list != null && list.Count > 0)
            {
                var res = list.First();
                switch (typeof(T).Name.ToUpperInvariant())
                {
                    case "STRING":
                        return (T)Convert.ChangeType(res.Json, typeof(T), CultureInfo.InvariantCulture);
                    default:
                        return (T)Convert.ChangeType(res, typeof(T), CultureInfo.InvariantCulture);
                }
            }
            else
            {
                switch (typeof(T).Name.ToUpperInvariant())
                {
                    case "STRING":
                        return (T)Convert.ChangeType("{\"Id\":-1}", typeof(T), CultureInfo.InvariantCulture);
                    default:
                        return (T)Convert.ChangeType(new ItemData(), typeof(T), CultureInfo.InvariantCulture);
                }
            }
        }


        public static T All<T>(ItemDefinition definition, string instanceName)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"Read::ById({0},{1}", definition.ItemName, instanceName);
            string query = string.Empty;
            if (!string.IsNullOrEmpty(definition.DataAdapter.GetAll.StoredName))
            {
                query = string.Format(CultureInfo.InvariantCulture, @"EXEC {0}", definition.DataAdapter.GetAll.StoredName);
            }
            else
            {
                var q = new StringBuilder("SELECT Id,CompanyId,");
                foreach (var field in definition.Fields)
                {
                    if (!definition.ForeignValues.Any(l => !string.IsNullOrEmpty(l.LinkField) && l.LocalName == field.Name))
                    {
                        q.AppendFormat(CultureInfo.InvariantCulture, @"[{0}],", field.Name);
                    }
                }

                q.AppendFormat("Active FROM ITEM_{0} WITH(NOLOCK)", definition.ItemName);
                query = q.ToString();
            }

            var list = new List<ItemData>();
            using (var db = new SqlConnection(Persistence.ConnectionString(instanceName)))
            {
                try
                {
                    var result = db.Query(query, true);
                    var dataDB = new ReadOnlyCollection<dynamic>(result.Select(r => (dynamic)r).ToList());
                    foreach (dynamic itemRead in dataDB)
                    {
                        var itembuilder = ItemDefinition.FromDynamic(instanceName, definition.ItemName, itemRead);
                        list.Add(itembuilder);
                    }
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (IndexOutOfRangeException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (IOException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (NotSupportedException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
            }

            switch (typeof(T).Name.ToUpperInvariant())
            {
                case "STRING":
                    return (T)Convert.ChangeType(AllString(definition, instanceName), typeof(T), CultureInfo.InvariantCulture);
                default:
                    return (T)Convert.ChangeType(AllItemBuilder(definition, instanceName), typeof(T), CultureInfo.InvariantCulture);
            }
        }

        public static string AllString(ItemDefinition definition, string instanceName)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"Read::ById({0},{1}", definition.ItemName, instanceName);
            string query = string.Empty;
            if (!string.IsNullOrEmpty(definition.DataAdapter.GetAll.StoredName))
            {
                query = string.Format(CultureInfo.InvariantCulture, @"EXEC {0}", definition.DataAdapter.GetAll.StoredName);
            }
            else
            {
                var q = new StringBuilder("SELECT Id,CompanyId,");
                foreach (var field in definition.Fields)
                {
                    if (!definition.ForeignValues.Any(l => !string.IsNullOrEmpty(l.LinkField) && l.LocalName == field.Name))
                    {
                        q.AppendFormat(CultureInfo.InvariantCulture, @"[{0}],", field.Name);
                    }
                }

                q.AppendFormat("Active FROM ITEM_{0} WITH(NOLOCK)", definition.ItemName);
                query = q.ToString();
            }

            var list = new StringBuilder("[");
            using (var db = new SqlConnection(Persistence.ConnectionString(instanceName)))
            {
                try
                {
                    var result = db.Query(query, true);
                    var dataDB = new ReadOnlyCollection<dynamic>(result.Select(r => (dynamic)r).ToList());
                    bool first = true;
                    foreach (dynamic itemRead in dataDB)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            list.Append(",");
                        }

                        var itembuilder = ItemDefinition.FromDynamic(instanceName, definition.ItemName, itemRead);
                        list.Append(itembuilder.Json);
                    }
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (IndexOutOfRangeException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (IOException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (NotSupportedException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
            }

            list.Append("]");
            return list.ToString();
        }

        public static ReadOnlyCollection<ItemData> AllItemBuilder(ItemDefinition definition, string instanceName)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"Read::ById({0},{1}", definition.ItemName, instanceName);
            string query = string.Empty;
            if (!string.IsNullOrEmpty(definition.DataAdapter.GetAll.StoredName))
            {
                query = string.Format(CultureInfo.InvariantCulture, @"EXEC {0}", definition.DataAdapter.GetAll.StoredName);
            }
            else
            {
                var q = new StringBuilder("SELECT Id,CompanyId,");
                foreach (var field in definition.Fields)
                {
                    if (!definition.ForeignValues.Any(l => !string.IsNullOrEmpty(l.LinkField) && l.LocalName == field.Name))
                    {
                        q.AppendFormat(CultureInfo.InvariantCulture, @"[{0}],", field.Name);
                    }
                }

                q.AppendFormat("Active FROM ITEM_{0} WITH(NOLOCK)", definition.ItemName);
                query = q.ToString();
            }

            var list = new List<ItemData>();
            using (var db = new SqlConnection(Persistence.ConnectionString(instanceName)))
            {
                try
                {
                    var result = db.Query(query, true);
                    var dataDB = new ReadOnlyCollection<dynamic>(result.Select(r => (dynamic)r).ToList());
                    foreach (dynamic itemRead in dataDB)
                    {
                        var itembuilder = ItemDefinition.FromDynamic(instanceName, definition.ItemName, itemRead);
                        list.Add(itembuilder);
                    }
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (IndexOutOfRangeException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (IOException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (NotSupportedException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
            }

            return new ReadOnlyCollection<ItemData>(list);
        }
    }
}