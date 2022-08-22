// --------------------------------
// <copyright file="Read.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.Script.Serialization;
    using Dapper;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.ItemManager;
    using OpenFrameworkV3.Core.Security;
    using OpenFrameworkV3.Tools;

    public static class Read
    {
        public static T ById<T>(long id, string itemName, string instanceName)
        {
            return ById<T>(id, Persistence.ItemDefinitionByName(itemName, instanceName), instanceName);
        }
        public static T ById<T>(long id, ItemDefinition definition, string instanceName)
        {
            return ById<T>(id, definition, instanceName);
        }

        public static T All<T>(string itemName, string instanceName)
        {
            return Read.All<T>(itemName, instanceName);
        }

        public static ReadOnlyCollection<ItemData> Active(string itemName, long applicationUserId, long companyId, string instanceName)
        {
            return new ReadOnlyCollection<ItemData>(new JavaScriptSerializer
            {
                MaxJsonLength = 500000000
            }.Deserialize<List<ItemData>>(SqlStream.GetFKStream(itemName, null,string.Empty,string.Empty, applicationUserId, companyId, Persistence.ConnectionString(instanceName))));
        }

        public static Dictionary<long, string> ActiveDescription(string itemDefinitionName, string instanceName)
        {
            var data = PrimaryKeys(itemDefinitionName, instanceName);
            var res = new Dictionary<long, string>();
            foreach(var item in data)
            {
                res.Add(Convert.ToInt64(item["Id"]), item["Description"] as string);
            }

            return res;
        }

        public static ReadOnlyCollection<ItemData> ForeingKeys(string definitionName, string instanceName)
        {
            if (string.IsNullOrEmpty(instanceName) || string.IsNullOrEmpty(definitionName))
            {
                return new ReadOnlyCollection<ItemData>(new List<ItemData>());
            }

            var source = string.Format(CultureInfo.InvariantCulture, "Read:All({0}, {1})", definitionName, instanceName);
            var res = new List<ItemData>();
            using (var dataBaseAccess = new SqlConnection(Persistence.ConnectionString(instanceName)))
            {
                try
                {
                    var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(definitionName, StringComparison.OrdinalIgnoreCase));
                    string query = ForeignKey.Query(itemDefinition);
                    var list = new List<ItemData>();
                    var result = dataBaseAccess.Query(query, true);
                    var dataDB = new ReadOnlyCollection<dynamic>(result.Select(r => (dynamic)r).ToList());
                    foreach (dynamic itemRead in dataDB)
                    {
                        var data = ItemDefinition.FromDynamic(instanceName, definitionName, itemRead);
                        if (itemDefinition.Fields.Any(f => !string.IsNullOrEmpty(f.VinculatedTo)))
                        {
                            foreach (var field in itemDefinition.Fields.Where(f => !string.IsNullOrEmpty(f.VinculatedTo)).ToList())
                            {
                                if (data.ContainsKey(field.VinculatedTo))
                                {
                                    data[field.Name] = data[field.VinculatedTo];
                                }
                                else
                                {
                                    data[field.Name] = null;
                                }
                            }
                        }

                        list.Add(data);
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
                catch (OverflowException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (OutOfMemoryException ex)
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
                catch (FieldAccessException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
                catch (NotSupportedException ex)
                {
                    ExceptionManager.Trace(ex as Exception, source);
                }
            }

            return new ReadOnlyCollection<ItemData>(res);
        }

        /// <summary>Gets primery keys of item definition data</summary>
        /// <param name="definition">Item definition</param>
        /// <param name="instance">Item instance</param>
        /// <returns>Itembuilder list</returns>
        public static ReadOnlyCollection<ItemData> PrimaryKeys(string definitionName, string instanceName)
        {
            if (string.IsNullOrEmpty(instanceName) || string.IsNullOrEmpty(definitionName))
            {
                return new ReadOnlyCollection<ItemData>(new List<ItemData>());
            }

            var res = new List<ItemData>();
            var definition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(definitionName, StringComparison.OrdinalIgnoreCase));
            definition.InstanceName = instanceName;
            string source = string.Format(CultureInfo.InvariantCulture, @"Read::PrimaryKeys({0},{1}", definitionName, instanceName);
            string query = string.Empty;
            var q = new StringBuilder("SELECT Id,CompanyId,");
            if (definition.PrimaryKeys != null && definition.PrimaryKeys.Count > 0)
            {
                foreach (var pk in definition.PrimaryKeys)
                {
                    q.AppendFormat(CultureInfo.InvariantCulture, @"[{0}],", pk);
                }
            }

            q.AppendFormat("Active FROM ITEM_{0} WITH(NOLOCK)", definition.ItemName);
            query = q.ToString();

            var list = new List<ItemData>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var db = new SqlConnection(cns))
                {
                    try
                    {
                        var result = db.Query(query, true);
                        var dataDB = new ReadOnlyCollection<dynamic>(result.Select(r => (dynamic)r).ToList());
                        foreach (dynamic itemRead in dataDB)
                        {
                            var itembuilder = ItemDefinition.FromDynamic(instanceName, definition.ItemName, itemRead);
                            res.Add(itembuilder);
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
            }

            return new ReadOnlyCollection<ItemData>(res);
        }

        /// <summary>Gets identifier/description of item definition data</summary>
        /// <param name="definition">Item definition</param>
        /// <param name="instance">Item instance</param>
        /// <param name="companyId">Actual company identifier</param>
        /// <returns>Itembuilder list</returns>
        public static ReadOnlyCollection<ItemData> KeyValue(ItemDefinition definition, string instanceName, long companyId)
        {
            if (string.IsNullOrEmpty(instanceName) || definition == null)
            {
                return new ReadOnlyCollection<ItemData>(new List<ItemData>());
            }

            var res = new List<ItemData>();
            definition.InstanceName = instanceName;
            string source = string.Format(CultureInfo.InvariantCulture, @"Read::KeyValue({0},{1},{2}", definition.ItemName, instanceName, companyId);
            string query = string.Empty;
            var q = new StringBuilder("SELECT Id,");
            if (definition.Layout.Description.Fields != null && definition.Layout.Description.Fields.Count > 0)
            {
                foreach (var pk in definition.Layout.Description.Fields)
                {
                    q.AppendFormat(CultureInfo.InvariantCulture, @"[{0}],", pk.Name);
                }
            }

            q.AppendFormat("Active FROM ITEM_{0} WITH(NOLOCK) Where CompanyId = {1} or CompanyId = 0", definition.ItemName, companyId);
            query = q.ToString();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var db = new SqlConnection(cns))
                {
                    try
                    {
                        var result = db.Query(query, true);
                        var dataDB = new ReadOnlyCollection<dynamic>(result.Select(r => (dynamic)r).ToList());
                        foreach (dynamic itemRead in dataDB)
                        {
                            var itembuilder = ItemDefinition.FromDynamic(instanceName, definition.ItemName, itemRead);
                            res.Add(itembuilder);
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
            }

            return new ReadOnlyCollection<ItemData>(res);
        }

        /// <summary>Gets value of field</summary>
        /// <param name="itemName">Item Name</param>
        /// <param name="fieldName">Fiel Name</param>
        /// <param name="itemId">Identifier of item</param>
        /// <returns>Value of field</returns>
        public static T GetFieldValue<T>(string itemName, string fieldName, long itemId, string instanceName)
        {
            var res = new object();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand())
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.Text;
                        var query = string.Format(
                            CultureInfo.InvariantCulture,
                            "SELECT {0} FROM Item_{1} WHERE Id = {2}",
                            fieldName,
                            itemName,
                            itemId);

                        try
                        {
                            cmd.Connection.Open();
                            cmd.CommandText = query;
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    res = rdr[0];
                                }
                                else
                                {
                                    return (T)Convert.ChangeType(default(T), typeof(T), CultureInfo.InvariantCulture);
                                }
                            }
                        }
                        finally
                        {
                            if (cmd.Connection.State != ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return (T)Convert.ChangeType(res, typeof(T), CultureInfo.InvariantCulture);
        }

        public static string JsonById(long id, ItemDefinition definition, string instanceName)
        {
            if (string.IsNullOrEmpty(instanceName) || definition == null)
            {
                return Json.EmptyJsonObject;
            }

            switch (definition.DataOrigin)
            {
                case DataOrigin.JsonFile:
                    return JsonConnector.ById<string>(id, definition, instanceName);
                default:
                    return SqlConnector.ById<string>(id, definition, instanceName);
            }
        }

        public static string JsonByFieldValue(string fieldName, string fieldValue, ItemDefinition definition, string instanceName)
        {
            return SqlConnector.ByFieldValue<string>(fieldName, fieldValue, definition, instanceName);
        }

        public static string JsonActive(string itemName, long companyId, string instanceName)
        {
            var definition = Persistence.ItemDefinitionByName(itemName, instanceName);
            switch (definition.DataOrigin)
            {
                case DataOrigin.JsonFile: return JsonConnector.JsonActive(itemName, instanceName);
                default: return JsonActiveSQL(itemName, companyId, companyId, instanceName);
            }
        }

        public static string JsonActiveSQL(string itemName,long applicationUserId, long companyId, string instanceName)
        {
            if (string.IsNullOrEmpty(instanceName))
            {
                return Json.EmptyJsonList;
            }

            var scopedField = string.Empty;
            var scopedItem = string.Empty;
            using (var instance = Persistence.InstanceByName(instanceName))
            {
                if (instance.ScopeView.Count > 0)
                {
                    var actualUser = ApplicationUser.ById(applicationUserId, instanceName);
                    if (actualUser.ScopeView.Count > 0)
                    {
                        var itemDefinitionId = Persistence.ItemDefinitionIdByName(itemName, instanceName);
                        if (actualUser.ScopeView.Any(s => s.ItemDefinitionId == itemDefinitionId))
                        {
                            scopedField = "Id";
                        }
                        else
                        {
                            var definition = Persistence.ItemDefinitionByName(itemName, instanceName);
                            foreach (var scope in actualUser.ScopeView)
                            {
                                var definitions = Persistence.InstanceByName(instanceName).ItemDefinitions;
                                if (definitions.Any(d => d.Id == scope.ItemDefinitionId))
                                {
                                    var scopeDefinition = definitions.First(d => d.Id == scope.ItemDefinitionId);
                                    if (definition.Fields.Any(f => f.Name.Equals(scopeDefinition.ItemName + "Id")))
                                    {
                                        scopedField = scopeDefinition.ItemName + "Id";
                                        scopedItem = scopeDefinition.ItemName;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                
                return SqlStream.GetFKStream(itemName, null, scopedField, scopedItem, applicationUserId, companyId, instanceName);
            }
        }

        public static string JsonAll(string itemName, string instanceName)
        {
            var definition = Persistence.ItemDefinitionByName(itemName, instanceName);
            switch (definition.DataOrigin)
            {
                case DataOrigin.JsonFile: return JsonConnector.All<string>(definition, instanceName);
                default: return SqlConnector.All<string>(definition, instanceName);
            }
        }
        public static string GetCustomFK(string itemName, string instanceName)
        {
            return GetCustomFK(Persistence.ItemDefinitionByName(itemName, instanceName), instanceName);
        }

        public static string GetCustomFK(ItemDefinition definition, string instanceName)
        {
            if(string.IsNullOrEmpty(instanceName))
            {
                return Json.EmptyJsonList;
            }

            var res = Json.EmptyJsonList;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand(definition.CustomFK))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = cnn;

                        try
                        {
                            res = SqlStream.SQLJSONStream(cmd);
                        }
                        catch (Exception ex)
                        {
                            res = Json.EmptyJsonList;
                            ExceptionManager.Trace(ex, string.Format(CultureInfo.InvariantCulture, "ItemDefinition::CreatePersistenceScript({0})", definition.ItemName));
                        }
                        finally
                        {
                            if (cmd.Connection.State != ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return res;
        }
    }
}