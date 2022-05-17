// --------------------------------
// <copyright file="SqlServerWrapper.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.DataAccess
{
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using OpenFrameworkV3.Core.ItemManager;

    /// <summary>Generates SQL sentences for insert and update items into database</summary>
    public static class SqlServerWrapper
    {
        /// <summary>Set latitude and longitude to item</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="latitude">Latitude value</param>
        /// <param name="longitude">Longitude value</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <param name="itemName">Item name</param>
        /// <param name="instanceName">Name of actual instance</param>
        /// <returns>Deactivate query for item</returns>
        public static string SetLocationQuery(long itemId, decimal latitude, decimal longitude, long applicationUserId, string itemName, string instanceName)
        {
            return string.Format(
                 CultureInfo.GetCultureInfo("en-us"),
                 "UPDATE Item_{0} SET Latitude = {3}, Longitude ={4}, ModifiedBy = {1}, ModifiedOn = GETDATE() WHERE Id = {2}",
                 itemName,
                 applicationUserId,
                 itemId,
                 latitude,
                 longitude);
        }

        /// <summary>Deactivates an item</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <param name="itemName">Item name</param>
        /// <param name="instanceName">Name of actual instance</param>
        /// <returns>Deactivate query for item</returns>
        public static string InactiveQuery(long itemId, long applicationUserId, string itemName, string instanceName)
        {
            string res = string.Empty;
            var definition = ItemDefinition.Load(itemName, instanceName);
            if (!string.IsNullOrEmpty(definition.DataAdapter.Inactive.StoredName))
            {
                res = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} @Id = {1}, @ModifiedBy = {2}", 
                    definition.DataAdapter.Inactive.StoredName, 
                    itemId, 
                    applicationUserId);
            } 
            else
            {
                res = DefaultInactiveQuery(itemId, applicationUserId, itemName);
            }

            return res;
        }

        /// <summary>Delete an item</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <param name="itemName">Item name</param>
        /// <param name="instanceName">Name of actual instance</param>
        /// <returns>Deactivate query for item</returns>
        public static string DeleteQuery(long itemId, long applicationUserId, string itemName, string instanceName)
        {
            string res = string.Empty;
            var definition = ItemDefinition.Load(itemName, instanceName);
            if (!string.IsNullOrEmpty(definition.DataAdapter.Inactive.StoredName))
            {
                res = string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    "{0} @Id = {1}, @ModifiedBy = {2}",
                    definition.DataAdapter.Inactive.StoredName,
                    itemId,
                    applicationUserId);
            }
            else
            {
                res = DefaultDeleteQuery(itemId, itemName);
            }

            return res;
        }

        /// <summary>Creates update query for item</summary>
        /// <param name="item">Item instance</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <returns>Update query for item</returns>
        public static string UpdateQuery(ItemDefinition item, ItemData data, long applicationUserId)
        {
            if (item == null)
            {
                return string.Empty;
            }

            string res = string.Empty;
            if (!string.IsNullOrEmpty(item.DataAdapter.Update.StoredName))
            {
                res = DataAdapterUpdateQuery(item, data, applicationUserId);
            }
            else
            {
                res = DefaultUpdateQuery(item, data, applicationUserId);
            }

            return res;
        }

        /// <summary>Creates insert query for item</summary>
        /// <param name="definition">Item instance</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <returns>Insert query for item</returns>
        public static string InsertQuery(ItemDefinition definition, ItemData data, long applicationUserId, long companyId)
        {
            if (definition == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(definition.DataAdapter.Insert.StoredName))
            {
                return DataAdapterInsertQuery(definition, data, applicationUserId, companyId);
            }
            else
            {
                return DefaultInsertQuery(definition, data, applicationUserId, companyId);
            }
        }

        /// <summary>Creates activate for item</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <param name="itemName">Item Name</param>
        /// <param name="instanceName">Name of actual instance</param>
        /// <returns>Activate query for item</returns>
        public static string ActiveQuery(long itemId, long applicationUserId, string itemName, string instanceName)
        {
            var definition = ItemDefinition.Load(itemName, instanceName);
            if (!string.IsNullOrEmpty(definition.DataAdapter.Active.StoredName))
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    "{0} @Id = {1}",
                    definition.DataAdapter.Active.StoredName,
                    itemId);
            }
            else
            {
                return DefaultActiveQuery(itemId, applicationUserId, itemName);
            }
        }

        /// <summary>Creates default (all fields) insert query for item</summary>
        /// <param name="definition">Item instance</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <returns>Insert query for item</returns>
        private static string DefaultInsertQuery(ItemDefinition definition, ItemData data, long applicationUserId, long companyId)
        {
            var sequences = new StringBuilder();
            var fields = new StringBuilder();
            var values = new StringBuilder();
            bool first = true;

            foreach (var field in definition.Fields.Where(f => f.Referencial != true && !f.TypeName.Equals("imagegallery", System.StringComparison.OrdinalIgnoreCase)))
            {
                if (field.Name == "Id" || !data.Keys.Contains(field.Name))
                {
                    continue;
                }

                string realField = string.Empty;
                // weke
                //if (definition.SqlMappings.Any(m => m.ItemField == field.Name))
                //{
                //    var mapping = definition.SqlMappings.First(m => m.ItemField == field.Name);
                //    realField = mapping.TableField;
                //}
                //else
                //{
                    realField = field.Name;
                //}

                if (first)
                {
                    first = false;
                }
                else
                {
                    fields.Append(",");
                    values.Append(",");
                }

                fields.AppendFormat(@"[{0}]", realField);

                if (!string.IsNullOrEmpty(field.CodeSequence))
                {
                    string queryPattern = @"DECLARE	@{0} nvarchar(10)

EXEC	[dbo].[Feature_CodeSequence_GetNext]
		@Description = N'{0}',
		@Code = @{0} OUTPUT

                        ";
                    values.AppendFormat(@"@{0}", field.CodeSequence);
                    sequences.AppendFormat(
                        CultureInfo.InvariantCulture,
                        queryPattern,
                        field.CodeSequence);
                }
                else
                {
                    values.AppendFormat(@"{0}", SqlValue.Value(field, data[field.Name]));
                }
            }

            fields.Append(InsertQueryFieldsEnd());
            values.Append(InsertQueryValuesEnd(applicationUserId, companyId));

            var res = new StringBuilder(sequences.ToString());                
            res.Append("set dateformat ymd;INSERT INTO Item_").Append(definition.ItemName);
            res.Append("(");
            res.Append(fields);
            res.Append(") VALUES(");
            res.Append(values);
            res.Append("); SELECT SCOPE_IDENTITY();");
            return res.ToString().Replace("\"\"", string.Empty);
        }

        /// <summary>Creates custom (defined in item) insert query for item</summary>
        /// <param name="itemDefinition">Item instance</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <param name="companyId">Identifier of company</param>
        /// <returns>Insert query for item</returns>
        private static string DataAdapterInsertQuery(ItemDefinition itemDefinition, ItemData data, long applicationUserId, long companyId)
        {
            if (itemDefinition.DataAdapter.Insert.StoredName == "#_insert")
            {
                var fields = new StringBuilder();
                var values = new StringBuilder();
                bool first = true;

                foreach (var param in itemDefinition.DataAdapter.Insert.Parameters)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        fields.Append(","); 
                        values.Append(",");
                    }

                    fields.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "[{0}]", param.Parameter);
                    values.Append(SqlValue.Value(itemDefinition.Fields.First(f => f.Name == param.Field), data[param.Field]));
                }

                fields.Append(InsertQueryFieldsEnd());
                values.Append(InsertQueryValuesEnd(applicationUserId, companyId));
                var res = new StringBuilder().AppendFormat("INSERT INTO Item_{0} ({1}) VALUES({2})", itemDefinition.ItemName, fields.ToString(), values.ToString());
                return res.ToString();
            }
            else
            {
                var values = new StringBuilder();
                foreach (var param in itemDefinition.DataAdapter.Insert.Parameters)
                {
                    values.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "@{0} = {1},", param.Parameter, SqlValue.Value(itemDefinition.Fields.First(f => f.Name == param.Field), data[param.Field]));
                }

                values.AppendFormat("@CreatedBy = {0}, @ModifiedBy = {0}", applicationUserId);
                var res = new StringBuilder();
                res.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "EXEC [{0}] {1}", itemDefinition.DataAdapter.Insert.StoredName, values);
                return res.ToString();
            }
        }

        /// <summary>Creates default (all fields) update query for item</summary>
        /// <param name="item">Item definition</param>
        /// <param name="data">Item data</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <returns>Update query for item</returns>
        private static string DefaultUpdateQuery(ItemDefinition item, ItemData data, long applicationUserId)
        {
            var values = new StringBuilder();
            bool first = true;
            foreach (var field in item.Fields.Where(f => !f.TypeName.Equals("imagegallery", System.StringComparison.OrdinalIgnoreCase)))
            {
                if (field.Name != "Id" && data.Keys.Contains(field.Name))
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        values.Append(",");
                    }

                    values.AppendFormat(@"[{0}] = {1}", field.Name, SqlValue.Value(field, data[field.Name]));
                }
            }

            values.Append(UpdateQueryValuesEnd(applicationUserId));

            var res = new StringBuilder("set dateformat ymd;UPDATE Item_").Append(item.ItemName);
            res.Append(" SET ");
            res.Append(values);
            res.AppendFormat(" WHERE Id = {0}", data["Id"]);
            return res.ToString().Replace("\"\"", string.Empty);
        }

        /// <summary>Creates custom (defined in item) update query for item</summary>
        /// <param name="item">Item instance</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <returns>Update query for item</returns>
        private static string DataAdapterUpdateQuery(ItemDefinition item, ItemData data, long applicationUserId)
        {
            if (item.DataAdapter.Update.StoredName == "#_update")
            {
                var values = new StringBuilder();
                bool first = true;
                foreach (var param in item.DataAdapter.Update.Parameters)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        values.Append(",");
                    }

                    values.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "[{0}] = {1}", param.Parameter, SqlValue.Value(item.Fields.First(f => f.Name == param.Field), data[param.Field]));
                }

                values.Append(UpdateQueryValuesEnd(applicationUserId));
                var res = new StringBuilder();
                res.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "UPDATE Item_{0} SET{1} WHERE Id = {2}", item.ItemName, values, data["Id"]);
                return res.ToString();
            }
            else
            {
                var values = new StringBuilder();
                bool first = true;
                foreach (var param in item.DataAdapter.Update.Parameters)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        values.Append(",");
                    }

                    values.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "@{0} = {1}", param.Parameter, SqlValue.Value(item.Fields.First(f => f.Name == param.Field), data[param.Field]));
                }

                values.AppendFormat(
                    CultureInfo.InvariantCulture,
                    ", @ModifiedBy = {0}",
                    applicationUserId);
                var res = new StringBuilder();
                res.AppendFormat(CultureInfo.GetCultureInfo("en-us"), "EXEC [{0}] {1}", item.DataAdapter.Update.StoredName, values);
                return res.ToString();
            }
        }

        /// <summary>Creates default deactivation for item</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <param name="itemName">Item name</param>
        /// <returns>Deactivate query for item</returns>
        private static string DefaultInactiveQuery(long itemId, long applicationUserId, string itemName)
        {
            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                "UPDATE Item_{0} SET Active = 0, ModifiedBy = {1}, ModifiedOn = GETDATE() WHERE Id = {2}", 
                itemName, 
                applicationUserId, 
                itemId);
        }

        /// <summary>Creates default deletion for item</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="itemName">Item name</param>
        /// <returns>Deactivate query for item</returns>
        private static string DefaultDeleteQuery(long itemId, string itemName)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "DELETE FROM Item_{0} WHERE Id = {1}",
                itemName, 
                itemId);
        }

        /// <summary>Creates default activation for item</summary>
        /// <param name="itemId">Item Identifier</param>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <param name="itemName">Item name</param>
        /// <returns>Activate query for item</returns>
        private static string DefaultActiveQuery(long itemId, long applicationUserId, string itemName)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "UPDATE Item_{0} SET Active = 1, ModifiedBy = {1} WHERE Id = {2}",
                itemName,
                applicationUserId,
                itemId);
        }

        /// <summary>Creates last fields for insert query</summary>
        /// <returns>String with last fields for insert query</returns>
        private static string InsertQueryFieldsEnd()
        {
            return ",[CompanyId],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn],[Active]";
        }

        /// <summary>Creates last values for insert query</summary>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <param name="companyId">Identifier of actual company</param>
        /// <returns>String with the last values for insert query</returns>
        private static string InsertQueryValuesEnd(long applicationUserId, long companyId)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                ",{0},{1},GETDATE(),{1},GETDATE(),1",
                companyId,
                applicationUserId);
        }

        /// <summary>Creates last fields for update query</summary>
        /// <param name="applicationUserId">Identifier of logged user</param>
        /// <returns>String with the last values for update query</returns>
        private static string UpdateQueryValuesEnd(long applicationUserId)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                ",[ModifiedBy] = {0}, [ModifiedOn] = GETDATE()",
                applicationUserId);
        }
    }
}