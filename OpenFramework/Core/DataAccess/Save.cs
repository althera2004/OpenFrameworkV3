// --------------------------------
// <copyright file="Save.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.DataAccess
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Enums;
    using OpenFrameworkV3.Core.ItemManager;
    using OpenFrameworkV3.Core.Security;
    using OpenFrameworkV3.Feature;

    /// <summary>Implements save actions for item into database</summary>
    public static class Save
    {
        /// <summary>Inserts item data into database</summary>
        /// <param name="itemBuilder">Item instance</param>
        /// <param name="instanceName">String connection to database</param>
        /// <param name="userId">User identifier</param> 
        /// <param name="companyId">Identifier of actual company</param> 
        /// <param name="fromImport">Indicates if insert is from import (not required)</param>
        /// <returns>Result of action</returns>
        public static ActionResult Insert(string itemDefinitionName, ItemData data, string instanceName, long userId, long companyId, bool fromImport)
        {
            var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(itemDefinitionName, StringComparison.OrdinalIgnoreCase));

            var res = ActionResult.NoAction;
            string query = SqlServerWrapper.InsertQuery(itemDefinition, data, userId, companyId);

            if (string.IsNullOrEmpty(query))
            {
                return ActionResult.NoAction;
            }

            using (var cmd = new SqlCommand(query))
            {
                var cns = Persistence.ConnectionString(instanceName);
                using (var cnn = new SqlConnection(cns))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();

                        // ExecuteScalar devuelve el Id del último insertado
                        var id = cmd.ExecuteScalar();

                        // PersistentData
                        if (data.ContainsKey("Id"))
                        {
                            data["Id"] = id;
                        }
                        else
                        {
                            data.Add("Id", id);
                        }

                        data["Active"] = true;
                        res.SetSuccess();
                        res.MessageError = itemDefinitionName;
                        res.ReturnValue = string.Format(CultureInfo.InvariantCulture, "INSERT|{0}", id);
                        //ActionLog.Trace(itemBuilder.ItemName + "_", itemBuilder.Id, itemBuilder.Json, itemBuilder.InstanceName, ApplicationUser.Actual);
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.Trace(ex as Exception,  "Insert:" + query);
                        res.SetFail(ex);
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

            return res;
        }

        /// <summary>Updates item data in database</summary>
        /// <param name="itemBuilder">Item instance</param>
        /// <param name="instanceName">Name of instance</param>
        /// <param name="userId">User identifier</param>
        /// <param name="fromImport">Indicates if update is from import (not required)</param>
        /// <returns>Result of action</returns>
        public static ActionResult Update(string itemDefinitionName, ItemData data, string instanceName, long userId, long companyId, bool fromImport)
        {
            var res = ActionResult.NoAction;
            if (string.IsNullOrEmpty(itemDefinitionName))
            {
                res.SetFail("No itemBuilder defined");
                return res;
            }

            if (string.IsNullOrEmpty(instanceName))
            {
                res.SetFail("No instance specified");
                return res;
            }

            var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(itemDefinitionName, StringComparison.OrdinalIgnoreCase));

            string query = SqlServerWrapper.UpdateQuery(itemDefinition, data, userId);
            using (var cmd = new SqlCommand(query))
            {
                try
                {
                    if (string.IsNullOrEmpty(query))
                    {
                        return ActionResult.NoAction;
                    }

                    var cns = Persistence.ConnectionString(instanceName);
                    if (!string.IsNullOrEmpty(cns))
                    {
                        using (var cnn = new SqlConnection(cns))
                        {
                            cmd.Connection = cnn;
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
                            if (res.Success)
                            {
                                var user = HttpContext.Current.Session["ApplicationUser"] as ApplicationUser;
                                res.ReturnValue = string.Format(CultureInfo.InvariantCulture, "UPDATE|{0}", Convert.ToInt64(data["Id"]));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.Trace(ex as Exception,  "Update:" + query);
                    res.SetFail(query + "<br />" + ex);
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return res;
        }

        /// <summary>Activate an item in database</summary>
        /// <param name="itemName">Item name</param>
        /// <param name="itemId">Item identifier</param>
        /// <param name="applicationUserId">Identifier of user that performs action</param>
        /// <returns>Result of action</returns>
        public static ActionResult Activate(string itemName, long itemId, long applicationUserId, string instanceName)
        {
            var query = new ExecuteQuery
            {
                ConnectionString = Persistence.ConnectionString(instanceName),
                QueryText = Query.Activate(itemName, itemId, applicationUserId)
            };

            return query.ExecuteCommand;
        }

        /// <summary>Inactivate an item in database</summary>
        /// <param name="itemName">Item name</param>
        /// <param name="itemId">Item identifier</param>
        /// <param name="applicationUserId">Identifier of user that performs action</param>
        /// <returns>Result of action</returns>
        public static ActionResult Inactivate(string itemName, long itemId, long companyId, long applicationUserId, string instanceName)
        {
            var query = new ExecuteQuery
            {
                ConnectionString = Persistence.ConnectionString(instanceName),
                QueryText = Query.Inactivate(itemName, itemId, applicationUserId)
            };

            return query.ExecuteCommand;
        }

        /// <summary>Unload an item in database</summary>
        /// <param name="itemName">Item name</param>
        /// <param name="itemId">Item identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult Unload(string itemName, long itemId, string instanceName)
        {
            var query = new ExecuteQuery
            {
                ConnectionString = Persistence.ConnectionString(instanceName),
                QueryText = Query.Unload(itemName, itemId)
            };

            return query.ExecuteCommand;
        }

        /// <summary>Reload an item in database</summary>
        /// <param name="itemName">Item name</param>
        /// <param name="itemId">Item identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult Reload(string itemName, long itemId, string instanceName)
        {            
            var query = new ExecuteQuery
            {
                ConnectionString = Persistence.ConnectionString(instanceName),
                QueryText = Query.Reload(itemName, itemId)
            };

            return query.ExecuteCommand;
        }

        /// <summary>Delete an item form database</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="userId">Identifier of user that performs action</param>
        /// <param name="applicationUser">User that realizes action</param>
        /// <param name="itemName">Item name</param>
        /// <param name="instanceName">Name of actual instance</param>
        /// <param name="connectionString">String connection to database</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(long itemId, long userId, string itemName, string instanceName, string connectionString)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                var query = SqlServerWrapper.DeleteQuery(itemId, userId, itemName, instanceName);
                using (var cmd = new SqlCommand(query))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(itemName);
                        }
                        catch (SqlException ex)
                        {
                            res.SetFail(ex);
                        }
                        catch (NullReferenceException ex)
                        {
                            res.SetFail(ex);
                        }
                        catch (Exception ex)
                        {
                            res.SetFail(ex);
                        }
                    }
                }
            }

            return res;
        }

        /// <summary>Inactivate or delete an item in database</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="itemName">Item name</param>
        /// <param name="userId">Identifier of user that performs action</param>
        /// <param name="applicationUser">User that realizes action</param>
        /// <param name="instanceName">Name of instance</param>
        /// <returns>Result of action</returns>
        public static ActionResult Inactive(long itemId, string itemName, long userId, long companyId, string instanceName)
        {
            if (string.IsNullOrEmpty(instanceName))
            {
                return ActionResult.NoAction;
            }

            using (var instance = Persistence.InstanceByName(instanceName))
            {
                if (instance.Config.DeleteAction == DeleteAction.Delete)
                {
                    return Delete(itemId, userId, itemName, instanceName, instance.Config.ConnectionString);
                }
                else
                {
                    return Inactivate(itemName, itemId,companyId, userId, instanceName );
                }
            }
        }

        /// <summary>Inactivate or delete an item in database</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="itemName">Item name</param>
        /// <param name="latitude">Latitude value</param>
        /// <param name="longitude">Longitude value</param>
        /// <param name="userId">Identifier of user that performs action</param>
        /// <param name="applicationUser">User that realizes action</param>
        /// <param name="instance">Actual instance</param>
        /// <returns>Result of action</returns>
        public static ActionResult SetLocation(long itemId, string itemName, decimal latitude, decimal longitude, long userId, ApplicationUser applicationUser, string instanceName)
        {
            var res = ActionResult.NoAction;
            var query = SqlServerWrapper.SetLocationQuery(itemId, latitude, longitude, userId, itemName, instanceName);
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand(query))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(itemName);
                        }
                        catch (SqlException ex)
                        {
                            res.SetFail(ex);
                        }
                        catch (NullReferenceException ex)
                        {
                            res.SetFail(ex);
                        }
                        catch (Exception ex)
                        {
                            res.SetFail(ex);
                        }
                    }
                }
            }

            return res;
        }

        public static ActionResult UpdateDocumentUndated(long itemId, string itemName, string fieldName, string fileName, long applicacionUserId, long companyId, string instanceName)
        {
            var itemDefinitionId = Persistence.ItemDefinitionIdByName(itemName, instanceName);
            var query = string.Format(
                        CultureInfo.InvariantCulture,
                        "UPDATE Item_{0} SET {1} = '{2}', ModifiedBy = {4}, ModifiedOn = GETDATE() WHERE Id = {3}",
                        itemName,
                        fieldName,
                        fileName,
                        itemId,
                        applicacionUserId);
            new DocumentHistory
            {
                CompanyId = companyId,
                ItemDefinitionId = itemDefinitionId,
                ItemId = itemId,
                FieldName = fieldName,
                Undated = true,
                FileName = fileName
            }.Insert(true, applicacionUserId, instanceName);
            return new ExecuteQuery { QueryText = query, ConnectionString = Persistence.ConnectionString(instanceName) }.ExecuteCommand;
        }

        public static ActionResult UpdateDocumentWithoutPrevious(long itemId, string itemName, string fieldName, string value, long applicacionUserId, string instanceName)
        {
            var query = string.Format(
                        CultureInfo.InvariantCulture,
                        "UPDATE Item_{0} SET {1} = '{2}', ModifiedBy = {4}, ModifiedOn = GETDATE() WHERE Id = {3}",
                        itemName,
                        fieldName,
                        value,
                        itemId,
                        applicacionUserId);
            return new ExecuteQuery { QueryText = query, ConnectionString = Persistence.ConnectionString(instanceName) }.ExecuteCommand;
        }

        public static ActionResult UpdateDocument(long itemId, string itemName, string fieldName, string value, long applicacionUserId, long companyId, string instanceName)
        {
            var itemDefinitionId = Persistence.ItemDefinitionIdByName(itemName, instanceName);
            var query = string.Format(
                        CultureInfo.InvariantCulture,
                        "UPDATE Item_{0} SET {1} = '{2}', ModifiedBy = {4}, ModifiedOn = GETDATE() WHERE Id = {3}",
                        itemName,
                        fieldName,
                        value,
                        itemId,
                        applicacionUserId);
            var res = new ExecuteQuery { QueryText = query, ConnectionString = Persistence.ConnectionString(instanceName) }.ExecuteCommand;
            return new DocumentHistory
            {
                CompanyId = companyId,
                ItemDefinitionId = itemDefinitionId,
                ItemId = itemId,
                FieldName = fieldName
            }.Insert(applicacionUserId, instanceName);
        }

        public static ActionResult UpdateField(long itemId, string itemName, string fieldName, string value, long applicacionUserId, string instanceName)
        {
            var query = string.Format(
                        CultureInfo.InvariantCulture,
                        "UPDATE Item_{0} SET {1} = '{2}', ModifiedBy = {4}, ModifiedOn = GETDATE() WHERE Id = {3}",
                        itemName,
                        fieldName,
                        value,
                        itemId,
                        applicacionUserId);
            return new ExecuteQuery { QueryText = query, ConnectionString = Persistence.ConnectionString(instanceName) }.ExecuteCommand;
        }

        public static ActionResult UpdateField(long itemId, string itemName, string fieldName, long value, long applicacionUserId, string instanceName)
        {
            var query = string.Format(
                        CultureInfo.InvariantCulture,
                        "UPDATE Item_{0} SET {1} = {2}, ModifiedBy = {4}, ModifiedOn = GETDATE() WHERE Id = {3}",
                        itemName,
                        fieldName,
                        value,
                        itemId,
                        applicacionUserId);
            return new ExecuteQuery { QueryText = query, ConnectionString = Persistence.ConnectionString(instanceName) }.ExecuteCommand;
        }

        public static ActionResult UpdateField(long itemId, string itemName, string fieldName, int value, long applicacionUserId, string instanceName)
        {
            var query = string.Format(
                        CultureInfo.InvariantCulture,
                        "UPDATE Item_{0} SET {1} = {2}, ModifiedBy = {4}, ModifiedOn = GETDATE() WHERE Id = {3}",
                        itemName,
                        fieldName,
                        value,
                        itemId,
                        applicacionUserId);
            return new ExecuteQuery { QueryText = query, ConnectionString = Persistence.ConnectionString(instanceName) }.ExecuteCommand;
        }

        public static ActionResult UpdateField(long itemId, string itemName, string fieldName, decimal value, long applicacionUserId, string instanceName)
        {
            var query = string.Format(
                        CultureInfo.InvariantCulture,
                        "UPDATE Item_{0} SET {1} = {2}, ModifiedBy = {4}, ModifiedOn = GETDATE() WHERE Id = {3}",
                        itemName,
                        fieldName,
                        value,
                        itemId,
                        applicacionUserId);
            return new ExecuteQuery { QueryText = query, ConnectionString = Persistence.ConnectionString(instanceName) }.ExecuteCommand;
        }
    }
}