// --------------------------------
// <copyright file="Query.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web;
    using OpenFrameworkV3.Core.Enums;
    using OpenFrameworkV3.Core.ItemManager;
    using OpenFrameworkV3.Core.ItemManager.ItemList;
    using OpenFrameworkV3.Core.Security;

    public static class Query
    {
        /// <summary>Generates query in order to get item's descriptions from database</summary>
        /// <param name="itemName">Name of item</param>
        /// <returns>SQL query in order to get item's descriptions</returns>
        public static string Descriptions(string itemName,long companyId, string instanceName)
        {
            var res = new StringBuilder();
            var itemDefinition = Persistence.ItemDefinitionByName(itemName, instanceName);
            foreach (var fieldName in itemDefinition.Layout.Description.Fields)
            {
                var field = itemDefinition.Fields.First(f => f.Name.Equals(fieldName.Name, StringComparison.OrdinalIgnoreCase));
                res.Append(field.SqlFieldExtractor);
                res.Append(" +");
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT 
		              '{{""Id"":' +  CAST(Item.Id AS  nvarchar(20)) + ',' +
                      {1}
                      '""Active"":' + CASE WHEN Item.Active = 0 THEN 'false' ELSE 'true' END + '}}' AS Data
                      FROM Item_{0} Item WITH(NOLOCK)
                      WHERE
                          Company = 0
                      OR  Compnay = {2}",
                itemDefinition.ItemName,
                res,
                companyId);
        }

        public static string PrimaryKeysList(ItemDefinition itemDefinition)
        {
            var res = new StringBuilder();
            if (itemDefinition.PrimaryKeys != null)
            {
                var primaryKeys = itemDefinition.PrimaryKeys.First();
                foreach (var primaryKey in primaryKeys.Value)
                {
                    var field = itemDefinition.Fields.First(f => f.Name.Equals(primaryKey, StringComparison.OrdinalIgnoreCase));
                    res.Append(field.SqlFieldExtractor);
                    res.Append(" +");
                }
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT 
		              '{{""Id"":' +  CAST(Item.Id AS  nvarchar(20)) + ',' +
                      {1}
                      '""Active"":' + CASE WHEN Item.Active = 0 THEN 'false' ELSE 'true' END + '}}' AS Data
                      FROM Item_{0} Item WITH(NOLOCK)",
                itemDefinition.ItemName,
                res);
        }

        public static string FKList(string itemDefinitionName, Dictionary<string, string> parameters, string scopedField, string scopedItemd, long applicationUserId, long companyId, string instanceName)
        {
            var descriptionFields = FieldsDescription(itemDefinitionName, instanceName);
            var fieldLines = FieldForeingLines(itemDefinitionName, instanceName);
            var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(itemDefinitionName, StringComparison.OrdinalIgnoreCase));
            var res = new StringBuilder();

            if (itemDefinition.Fields != null)
            {
                foreach (var field in fieldLines)
                {
                    res.Append("                      ");
                    res.Append(field.SqlFieldExtractor);
                    res.Append(" +");
                }
            }

            var descriptionFieldLine = itemDefinition.Layout.Description.Pattern;
            var descriptionFieldList = new List<string>();
            foreach (var field in descriptionFields)
            {
                descriptionFieldList.Add(field.SqlFieldExtractorValue);
            }

            var nfield = itemDefinition.Layout.Description.Pattern.ToCharArray().Count(c => c.Equals('{'));
            for (var x = 0; x < nfield; x++)
            {
                descriptionFieldLine = descriptionFieldLine.Replace("{" + x + "}", " #" + x + " ");
            }

            var parts = descriptionFieldLine.Split(' ');
            descriptionFieldLine = string.Empty;
            if (descriptionFieldList.Count > 0)
            {
                foreach (var part in parts)
                {
                    if (part.StartsWith("#"))
                    {
                        descriptionFieldLine += " + " + part;
                    }
                    else
                    {
                        descriptionFieldLine += " + '" + part + "'";
                    }
                }

                for (var x = 0; x < nfield; x++)
                {
                    descriptionFieldLine = descriptionFieldLine.Replace("#" + x, descriptionFieldList[x]);
                }
            }

            var additionalWhere = new StringBuilder(string.Format(CultureInfo.InvariantCulture, " WHERE (Item.CompanyId = {0} OR Item.CompanyId = 0)", companyId));
            if (parameters != null)
            {
                if (parameters.Count > 0)
                {
                    foreach (var parameter in parameters)
                    {
                        additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} ='{1}", parameter.Key, parameter.Value);
                    }
                }
            }

            if (!string.IsNullOrEmpty(scopedField))// && string.IsNullOrEmpty(scopedItemd))
            {
                var definition = itemDefinition;
                if (!string.IsNullOrEmpty(scopedItemd))
                {
                    definition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(scopedItemd, StringComparison.OrdinalIgnoreCase));
                }


                additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} IN({1})", scopedField, string.Empty); // weke ApplicationUser.Actual.ScopeViewIdsSqlWhereData(definition.Id));
            }

            var FKFields = new StringBuilder();
            foreach (var field in itemDefinition.Fields.Where(f => f.FK == true))
            {
                res.Append(field.SqlFieldExtractor);
                res.Append(" +");
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT 
		              '{{""Id"":' +  CAST(Item.Id AS  nvarchar(20)) + ',' +
                      '""Description"":""' {1} + '"",' + 
                      {2}
                      {4}
                      '""Active"":' + CASE WHEN Item.Active = 0 THEN 'false' ELSE 'true' END + '}}' AS Data
                      FROM Item_{0} Item WITH(NOLOCK){3}",
                itemDefinition.ItemName,
                descriptionFieldLine,
                res,
                additionalWhere,
                FKFields);
        }

        public static string KeyValueById(string itemDefinitionName, long id, string instanceName)
        {

            var descriptionFields = FieldsDescription(itemDefinitionName, instanceName);
            var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(itemDefinitionName, StringComparison.OrdinalIgnoreCase));
            var fieldLines = FieldForeingLines(itemDefinitionName, instanceName);
            var res = new StringBuilder("");

            if (itemDefinition.Fields != null)
            {
                foreach (var field in fieldLines)
                {
                    res.Append(field.SqlFieldExtractor);
                    res.Append(" +");
                }
            }

            var descriptionFieldLine = itemDefinition.Layout.Description.Pattern;
            var descriptionFieldList = new List<string>();
            foreach (var field in descriptionFields)
            {
                descriptionFieldList.Add(field.SqlFieldExtractorValue);
            }

            var nfield = itemDefinition.Layout.Description.Pattern.ToCharArray().Count(c => c.Equals('{'));
            for (var x = 0; x < nfield; x++)
            {
                descriptionFieldLine = descriptionFieldLine.Replace("{" + x + "}", "#" + x + " ");
            }

            var parts = descriptionFieldLine.Split(' ');
            descriptionFieldLine = string.Empty;
            foreach (var part in parts)
            {
                if (part.StartsWith("#"))
                {
                    descriptionFieldLine += " + " + part;
                }
                else
                {
                    descriptionFieldLine += " + '" + part + "'";
                }
            }

            for (var x = 0; x < nfield; x++)
            {
                descriptionFieldLine = descriptionFieldLine.Replace("#" + x, descriptionFieldList[x]);
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT 
		              '{{""Id"":' +  CAST(Item.Id AS  nvarchar(20)) + ',' +
                      '""Description"":""' {1} + '"",' + 
                      '""Active"":' + CASE WHEN Item.Active = 0 THEN 'false' ELSE 'true' END + '}}' AS Data
                      FROM Item_{0} Item WITH(NOLOCK){2}
                      WHERE Id =  {3}",
                itemDefinition.ItemName,
                descriptionFieldLine,
                res,
                id);
        }

        public static string ByList(ItemDefinition itemDefinition, ReadOnlyCollection<FilterCriteria> parameters, List listDefinition)
        {
            var res = new StringBuilder("");
            var innerJoin = new StringBuilder("");
            var joinCount = 1;

            foreach (var column in listDefinition.Columns)
            {
                res.Append(",");
                var field = itemDefinition.Fields.First(f => f.Name.Equals(column.DataProperty, StringComparison.OrdinalIgnoreCase));
                var referedField = field.GetReferedField(itemDefinition);

                if (referedField != null)
                {
                    foreach (var f in referedField)
                    {
                        var joinClause = string.Format(
                            CultureInfo.InvariantCulture,
                            @"LEFT JOIN Item_{0} J{1} WITH(NOLOCK) ON J{1}.Id = Item.{0}Id{2}",
                            f.ItemName,
                            joinCount,
                            Environment.NewLine);
                        innerJoin.Append(joinClause);

                        res.AppendFormat(
                            CultureInfo.InvariantCulture,
                            "ISNULL(J{0}.{1},'') AS {2}",
                            joinCount,
                            f.Name,
                            field.Name);

                        joinCount++;
                    }
                }
                else
                {
                    if (field.DataType == ItemFieldDataType.ItemDescription)
                    {
                        res.AppendFormat(
                            CultureInfo.InvariantCulture,
                            @"ISNULL(CAST(Item.ItemId as nvarchar(5)), '') AS ItemDescription",
                            field.Name);
                    }
                    else if (field.DataType == ItemFieldDataType.ItemDefinition)
                    {
                        var joinClause = string.Format(
                        CultureInfo.InvariantCulture,
                        @"LEFT JOIN Core_Item CI WITH(NOLOCK) ON CI.Id = Item.{1}{0}",
                        field.Name,
                        Environment.NewLine);
                        innerJoin.Append(joinClause);
                        res.AppendFormat(
                            CultureInfo.InvariantCulture,
                            @"ISNULL(CI.[Name], '') AS {0}",
                            field.Name);
                    }
                    else if (field.DataType == ItemFieldDataType.ApplicationUser)
                    {
                        var joinClause = string.Format(
                            CultureInfo.InvariantCulture,
                            @"LEFT JOIN Core_Profile CP WITH(NOLOCK) ON CP.ApplicationUserId = Item.{0}{1}",
                            field.Name,
                            Environment.NewLine);
                        innerJoin.Append(joinClause);
                        res.AppendFormat(
                            CultureInfo.InvariantCulture,
                            @" ISNULL(CP.[Name], '') AS {0}Name,ISNULL(CP.[LastName], '') AS {0}LastName1,ISNULL(CP.[LastName2], '') AS {0}LastName2,",
                            field.Name);
                    }
                    else if (field.DataType == ItemFieldDataType.SecurityGroup)
                    {
                        var joinClause = string.Format(
                            CultureInfo.InvariantCulture,
                            @"LEFT JOIN Core_Group G WITH(NOLOCK) ON G.Id = Item.{0}{1}",
                            field.Name,
                            Environment.NewLine);
                        innerJoin.Append(joinClause);
                        res.AppendFormat(
                            CultureInfo.InvariantCulture,
                            @" ISNULL(G.[Name], '') AS {0}",
                            field.Name);
                    }
                    else
                    {
                        res.AppendFormat(CultureInfo.InvariantCulture, "Item.{0}", field.Name);
                    }
                }

                res.Append(Environment.NewLine);
            }

            var additionalWhere = new StringBuilder();

            if (listDefinition.Parameters != null && listDefinition.Parameters.Count > 0)
            {
                foreach (var definitionParameter in listDefinition.Parameters)
                {
                    if (definitionParameter.Value.Equals("NULL", StringComparison.OrdinalIgnoreCase))
                    {
                        additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} IS NULL", definitionParameter.Name);
                    }
                    else if (definitionParameter.Value.Equals("defaultId", StringComparison.OrdinalIgnoreCase))
                    {
                        additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} = '{1}'", definitionParameter.Name, Constant.DefaultId);
                    }
                    else
                    {
                        additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} = '{1}'", definitionParameter.Name, definitionParameter.Value);
                    }
                }
            }

            if (parameters.Count > 0)
            {
                foreach (var parameter in parameters.Where(p => string.IsNullOrEmpty(p.Criteria) == false))
                {
                    switch (parameter.Criteria.ToUpperInvariant())
                    {
                        case "BOOL":
                            if (!string.IsNullOrEmpty(parameter.Value))
                            {
                                additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} = {1}", parameter.FieldName, (parameter.Value.Equals("True", StringComparison.OrdinalIgnoreCase) ? "1" : "0"));
                            }
                            break;
                        case "NULL":
                            additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} IS NULL", parameter.FieldName);
                            break;
                        case "NOTNULL":
                            additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} IS NOT NULL", parameter.FieldName);
                            break;
                        case "EQ":
                            additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} = '{1}'", parameter.FieldName, parameter.Value);
                            break;
                        case "NOTEQ":
                            additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} <> '{1}'", parameter.FieldName, parameter.Value);
                            break;
                        case "IN":
                            additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} IN ({1})", parameter.FieldName, parameter.Value);
                            break;
                        case "NOTIN":
                            additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} NOT IN ({1})", parameter.FieldName, parameter.Value);
                            break;
                        case "GT":
                            additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} > '{1}'", parameter.FieldName, parameter.Value);
                            break;
                        case "GTEQ":
                            additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} >= '{1}'", parameter.FieldName, parameter.Value);
                            break;
                        case "LT":
                            additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} < '{1}'", parameter.FieldName, parameter.Value);
                            break;
                        case "LTEQ":
                            additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} <= '{1}'", parameter.FieldName, parameter.Value);
                            break;
                        case "STARTS":
                            additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} LIKE '{1}%'", parameter.FieldName, parameter.Value);
                            break;
                        case "ENDS":
                            additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} LIKE '%{1}'", parameter.FieldName, parameter.Value);
                            break;
                        default:
                            additionalWhere.Append(string.Empty);
                            break;
                    }
                }
            }

            var user = ApplicationUser.Actual;
            var scope = user.ScopeViewIdsSqlWhereData(itemDefinition.Id);
            if (!string.IsNullOrEmpty(scope))
            {
                additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.Id IN ({0})", scope);
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT 
		              Item.Id
		              {1}
                      FROM Item_{0} Item WITH(NOLOCK){2}
                      WHERE 1 = 1 {3}{4}",
                itemDefinition.ItemName,
                res,
                innerJoin,
                additionalWhere,
                user.AdminUser ? string.Empty : " AND Item.Active = 1");
        }

        public static string ByList(ItemDefinition itemDefinition, Dictionary<string, string> parameters, List listDefinition, long companyId)
        {
            var res = new StringBuilder("");
            var innerJoin = new StringBuilder("");
            var joinCount = 1;

            foreach (var column in listDefinition.Columns)
            {
                var field = itemDefinition.Fields.First(f => f.Name.Equals(column.DataProperty, StringComparison.OrdinalIgnoreCase));
                var referedField = field.GetReferedField(itemDefinition);

                var queryField = string.Empty;
                if (referedField != null)
                {
                    var joinItemns = referedField.GroupBy(x => x.ItemName).Select(x => x.FirstOrDefault());

                    foreach (var itemName in joinItemns)
                    {
                        var joinClause = string.Format(
                            CultureInfo.InvariantCulture,
                            @"LEFT JOIN Item_{0} J{1} WITH(NOLOCK) ON J{1}.Id = Item.{0}Id{2}",
                            itemName.ItemName,
                            joinCount,
                            Environment.NewLine);
                        innerJoin.Append(joinClause);

                        res.Append("'\"").Append(field.Name).Append("\":{\"Id\":' + ISNULL(CAST(Item.");
                        res.Append(field.Name);
                        res.Append(" AS nvarchar(20)),'null') +',\"Value\":\"'");
                        var first = true;
                        foreach (var f in referedField)
                        {
                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                res.Append(" + ' ' + ");
                            }

                            res.Append(" + REPLACE(ISNULL(J").Append(joinCount).Append(".");
                            res.Append(f.Name);
                            res.Append(",''),'''','\''')");
                        }
                        res.Append("+'\"},' + ");

                        joinCount++;
                    }
                }
                else
                {
                    if (field.DataType == ItemFieldDataType.ItemDescription)
                    {
                        res.AppendFormat(
                            CultureInfo.InvariantCulture,
                            @"'""ItemId"":""' + ISNULL(CAST(Item.ItemId as nvarchar(5)), '') + '"",' +",
                            field.Name);
                    }
                    else if (field.DataType == ItemFieldDataType.ItemDefinition)
                    {
                        var joinClause = string.Format(
                        CultureInfo.InvariantCulture,
                        @"LEFT JOIN Core_Item CI WITH(NOLOCK) ON CI.Id = Item.{1}{0}",
                        field.Name,
                        Environment.NewLine);
                        innerJoin.Append(joinClause);
                        res.AppendFormat(
                            CultureInfo.InvariantCulture,
                            @"'""{0}"":{{""Id"":' + ISNULL(CAST(CI.Id  AS nvarchar(20)),'null') + 
                            ',""Value"":""' +  ISNULL(CI.[Name], '')  + '""' + 
                            ',""Icon"":""' +  ISNULL(CI.[Icon], '')  + '""}},' + ",
                            field.Name);
                    }
                    else if (field.DataType == ItemFieldDataType.ApplicationUser)
                    {
                        var joinClause = string.Format(
                            CultureInfo.InvariantCulture,
                            @"LEFT JOIN Core_Profile CP WITH(NOLOCK) ON CP.ApplicationUserId = Item.{0}{1}",
                            field.Name,
                            Environment.NewLine);
                        innerJoin.Append(joinClause);
                        res.AppendFormat(
                            CultureInfo.InvariantCulture,
                            @"'""{0}"":{{""Id"":' + ISNULL(CAST(Item.{0} AS nvarchar(20)),'null') + 
                            ',""Value"":""' +
                            LTRIM(RTRIM
                            (
                                ISNULL(CP.[Name], '') + ' ' +
                                ISNULL(CP.[LastName], '') + ' ' +
                                ISNULL(CP.[LastName2], '')
                            )) + '""}},' + ",
                            field.Name);
                    }
                    else if (field.DataType == ItemFieldDataType.SecurityGroup)
                    {
                        var joinClause = string.Format(
                        CultureInfo.InvariantCulture,
                        @"LEFT JOIN Core_Group G WITH(NOLOCK) ON G.Id = Item.{0}{1}",
                        field.Name,
                        Environment.NewLine);
                        innerJoin.Append(joinClause);

                        res.AppendFormat(
                        CultureInfo.InvariantCulture,
                            @"'""{0}"":{{""Id"":' + ISNULL(CAST(G.Id AS nvarchar(20)),'null') + ',""Value"":""' + ISNULL(G.[Name], '') + '""}},' + ",
                            field.Name);
                    }
                    else if (string.IsNullOrEmpty(column.ReplacedBy))
                    {
                        res.Append(field.SqlFieldExtractor);
                    }
                    else
                    {
                        res.Append(field.SqlFieldExtractorReplace(column.ReplacedBy));
                    }
                }

                res.Append(" +").Append(Environment.NewLine);
            }

            var additionalWhere = new StringBuilder();
            if (parameters.Count > 0)
            {
                foreach (var parameter in parameters)
                {
                    if (parameter.Value.Equals("NULL", StringComparison.OrdinalIgnoreCase))
                    {
                        additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} IS NULL", parameter.Key);
                    }
                    else if (parameter.Value.Equals("NOTNULL", StringComparison.OrdinalIgnoreCase))
                    {
                        additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} IS NOT NULL", parameter.Key);
                    }
                    else if (parameter.Value.Equals("defaultId", StringComparison.OrdinalIgnoreCase))
                    {
                        additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} ='{1}'", parameter.Key, Constant.DefaultId);
                    }
                    else
                    {
                        additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.{0} ='{1}'", parameter.Key, parameter.Value);
                    }
                }
            }

            var user = ApplicationUser.Actual;
            var scope = user.ScopeViewIdsSqlWhereData(itemDefinition.Id);
            if (!string.IsNullOrEmpty(scope))
            {
                additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND Item.Id IN ({0})", scope);
            }

            additionalWhere.AppendFormat(CultureInfo.InvariantCulture, @" AND (Item.CompanyId = {0} OR Item.CompanyId = 0)", companyId);

            var unloadable = string.Empty;
            if (itemDefinition.Features.Unloadable)
            {
                unloadable = @"'""Unloaded"":' + CASE WHEN FU.ItemId IS NULL THEN 'false' ELSE 'true' END + ',' +";
                innerJoin.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"LEFT JOIN Feature_Unloadable FU ON FU.ItemId = Item.Id AND FU.ItemName = '{0}'{1}",
                    itemDefinition.ItemName,
                    Environment.NewLine);
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT 
		              '""Id"":' +  CAST(Item.Id AS  nvarchar(20)) + ',' +
		              {1}{4}
                      '""Active"":' + CASE WHEN Item.Active = 0 THEN 'false' ELSE 'true' END
                      FROM Item_{0} Item WITH(NOLOCK){2}
                      WHERE 1 = 1 {3} {5}",
                itemDefinition.ItemName,
                res,
                innerJoin,
                additionalWhere,
                unloadable,
                " AND Item.Active = 1");
        }

        public static string ByListId(string itemDefinitionName, Dictionary<string, string> parameters, string listId, long companyId, string instanceName)
        {
            if (string.IsNullOrEmpty(itemDefinitionName))
            {
                return string.Empty;
            }

            var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(itemDefinitionName, StringComparison.OrdinalIgnoreCase));

            var list = itemDefinition.Lists.FirstOrDefault(l => l.Id.Equals(listId, StringComparison.OrdinalIgnoreCase));
            if (list == null)
            {
                return Query.All(itemDefinitionName, companyId, instanceName);
            }

            itemDefinition.InstanceName = instanceName;
            return ByList(itemDefinition, parameters, list, companyId);
        }

        public static string All(string itemDefinitionName, long companyId, string instanceName)
        {            
            var res = new StringBuilder("");

            var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(itemDefinitionName, StringComparison.OrdinalIgnoreCase));

            foreach (var field in itemDefinition.Fields)
            {
                res.Append(field.SqlFieldExtractor);
                res.Append(" +");
            }

            var user = ApplicationUser.Actual;
            var additionalWhere = string.Empty;
            var scope = user.ScopeViewIdsSqlWhereData(itemDefinition.Id);
            if (!string.IsNullOrEmpty(scope))
            {
                additionalWhere = " Item.Id IN (" + scope + ")";
            }

            additionalWhere += " AND (Item.CompanyId = " + companyId.ToString() + " OR Item.CompanyId = 0)";

            return string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT 
		              {1}
                      '""Active"":' + CASE WHEN Item.Active = 0 THEN 'false' ELSE 'true' END
                      FROM Item_{0} Item WITH(NOLOCK)
                      WHERE 1 = 1 {2} {3}",
                itemDefinition.ItemName,
                res,
                additionalWhere,
                user.AdminUser ? string.Empty : " AND Item.Active = 1");
        }

        public static string ById(string itemName, long id, long companyId, string instanceName)
        {
            var descriptionFields = FieldsDescription(itemName, instanceName);
            var itemDefinition = Persistence.ItemDefinitionByName(itemName, instanceName);
            var res = new StringBuilder(string.Empty);

            foreach (var field in itemDefinition.Fields.Where(f => !f.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)))
            {
                res.Append(field.SqlFieldExtractor);
                res.Append(" +");
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT 
                      '""Id"":{1},' +
		              {2}
                      '""Active"":' + CASE WHEN Item.Active = 0 THEN 'false' ELSE 'true' END + ','  +
                      '""ModifiedBy"":""' + LTRIM(RTRIM(MB.Name + ' ' + MB.LastName)) + '"",'  +
                      '""ModifiedOn"":' + ISNULL('""' + CONVERT(varchar(11), Item.ModifiedOn , 103) + '""', 'null') 
                      FROM Item_{0} Item WITH(NOLOCK) 
                      INNER JOIN Core_Profile MB WITH(NOLOCK)
                      ON  MB.ApplicationUserId = Item.ModifiedBy
                      WHERE Item.Id = {1} AND (Item.CompanyId = 0 OR Item.CompanyId = {3})",
                itemName,
                id,
                res,
                companyId);
        }

        public static string Activate(string itemDefinitionName, long id, long applicationUserId)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                @"UPDATE Item_{0} SET
                    Active = 1,
                    ModifiedBy = {2},
                    ModifiedOn = GETDATE()
                    WHERE Id = {1}",
                itemDefinitionName,
                id,
                applicationUserId);
        }

        public static string InsertDocumentFile(string itemName, long id, string fieldName, string value, long applicationUserId)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                @"UPDATE Item_{0} SET {3} = '{4}', ModifiedBy = {2}, ModifiedOn = GETDATE() WHERE Id = {1}",
                itemName,
                id,
                applicationUserId,
                fieldName,
                value);
        }

        public static string Inactivate(string itemDefinitionName, long id, long applicationUserId)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                @"UPDATE Item_{0} SET Active = 0, ModifiedBy = {2}, ModifiedOn = GETDATE() WHERE Id = {1}",
               itemDefinitionName,
                id,
                applicationUserId);
        }

        public static string Reload(string itemName, long id)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                @"DELETE FROM Feature_Unloadable WHERE ItemName = '{0}' AND ItemId = {1}",
                itemName,
                id);
        }

        public static string Unload(string itemName, long id)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "INSERT INTO Feature_Unloadable (ItemName, ItemId) VALUES('{0}', {1})",
                itemName,
                id);
        }

        public static string Delete(string itemDefinitionName, long id)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                @"DELETE Item_{0} WHERE Id = {1}",
                itemDefinitionName,
                id);
        }

        private static ReadOnlyCollection<ItemField> FieldsDescription(string itemDefinitionName, string instanceName)
        {
            var res = new List<ItemField>();
            var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(itemDefinitionName, StringComparison.OrdinalIgnoreCase));
            if (itemDefinition.Layout.Description != null)
            {
                foreach (var field in itemDefinition.Layout.Description.Fields)
                {
                    foreach (var fieldData in itemDefinition.Fields)
                    {
                        if (fieldData.Name == field.Name)
                        {
                            res.Add(fieldData);
                            break;
                        }
                    }
                }
            }

            return new ReadOnlyCollection<ItemField>(res);
        }

        private static ReadOnlyCollection<ItemField> FieldForeingLines(string itemDefinitionName, string instanceName)
        {
            var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(itemDefinitionName, StringComparison.Ordinal));
            var fields = itemDefinition.Fields.Where(i => itemDefinition.ForeignListNames.Contains(itemDefinitionName)).ToList();
            return new ReadOnlyCollection<ItemField>(fields.Where(f => FieldsDescription(itemDefinitionName, instanceName).Select(a => a.Name).ToList().Contains(f.Name) == false).ToList());
        }
    }
}