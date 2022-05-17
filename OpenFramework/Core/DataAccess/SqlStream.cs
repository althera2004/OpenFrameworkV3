// --------------------------------
// <copyright file="SqlStream.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.ItemManager;
    using OpenFrameworkV3.Tools;

    public static class SqlStream
    {
        public static string GetSqlStream(string storedName, ReadOnlyCollection<SqlParameter> parameters, string connectionString)
        {
            string res = Json.EmptyJsonList;
            using (var cmd = new SqlCommand(storedName))
            {
                if (parameters != null)
                {
                    if (parameters.Count > 0)
                    {
                        cmd.Parameters.Clear();
                        foreach (var parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }
                }

                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    res = SQLJSONStream(cmd);
                }
            }

            return res;
        }

        public static string GetFKStream(string itemName, Dictionary<string, string> parameters, string scopedField, string scopedItem, long applicationUserId, long companyId, string instanceName)
        {
            var query = Query.FKList(itemName, parameters, scopedField, scopedItem, applicationUserId, companyId, instanceName);
            var cns = Persistence.ConnectionString(instanceName);
            return GetSqlQueryStreamNoParams(query, cns).Replace(@"^""", @"\""");
        }

        public static string GetPrimaryKeys(ItemDefinition definition, string connectionString)
        {
            if(definition.PrimaryKeys == null || definition.PrimaryKeys.Count < 1)
            {
                return Json.EmptyJsonList;
            }

            return GetSqlQueryStreamNoParams(Query.PrimaryKeysList(definition), connectionString);
        }

        public static string GetSqlQueryStreamNoParams(string query, string connectionString)
        {
            string res = Json.EmptyJsonList;
            using (var cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.Text;
                using (var cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    res = SQLJSONStream(cmd);
                }
            }

            return res;
        }

        public static string GetSqlStreamNoParams(string storedName, string connectionString)
        {
            string res = Json.EmptyJsonList;
            using (var cmd = new SqlCommand(storedName))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    res = SQLJSONStream(cmd);
                }
            }

            return res;
        }

        public static string ByStored(string storedName, string connectionString)
        {
            string res = Json.EmptyJsonList;
            using (var cmd = new SqlCommand(storedName))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    res = SQLToJSON(cmd);
                }
            }

            return res;
        }

        public static string ByQuerySimple(string query, string connectionString)
        {
            string res = string.Empty;
            using (var cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.Text;
                using (var cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    res = SQLStreamSimple(cmd);
                }
            }

            // Si no existe se devuelve un item vacío con id -1
            if (string.IsNullOrEmpty(res))
            {
                res = "{Id:-1}";
            }

            return res;
        }

        public static string ByQuery(string query, string connectionString)
        {
            string res = Json.EmptyJsonList;
            using (var cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.Text;
                using (var cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    res = SQLStreamList(cmd);
                }
            }

            return res;
        }

        /// <summary>Get JSON format data from an stored procedure</summary>
        /// <param name="query">SQL query</param>
        /// <param name="connectionString">String connection to database</param>
        /// <returns>An "application/json" format of data</returns>
        public static string SQLQueryJSONStream(string query, string connectionString)
        {
            string res = Json.EmptyJsonList;
            if (!string.IsNullOrEmpty(query))
            {
                using (var cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    using (var cnn = new SqlConnection(connectionString))
                    {
                        cmd.Connection = cnn;
                        res = SQLJSONStream(cmd);
                    }
                }
            }

            return res;
        }

        /// <summary>Get JSON format data from an stored procedure</summary>
        /// <param name="storedName">SQL command</param>
        /// <param name="connectionString">String connection to database</param>
        /// <returns>An "application/json" format of data</returns>
        public static string SQLJSONStream(string storedName, string connectionString)
        {
            string res = Json.EmptyJsonList;
            if (!string.IsNullOrEmpty(storedName))
            {
                using (var cmd = new SqlCommand(storedName))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(connectionString))
                    {
                        cmd.Connection = cnn;
                        res = SQLJSONStream(cmd);
                    }
                }
            }

            return res;
        }

        /// <summary>Get JSON format data from SQL command</summary>
        /// <param name="cmd">SQL command</param>
        /// <returns>An "application/json" format of data</returns>
        public static string SQLJSONStream(SqlCommand cmd)
        {
            if (cmd == null)
            {
                return Tools.Json.EmptyJsonList;
            }

            string source = string.Format(CultureInfo.InvariantCulture, "OpenFramework.DataAccess.SQLJsoStream({0})", cmd.CommandText);
            var res = new StringBuilder("[");
            try
            {
                cmd.Connection.Open();
                bool first = true;
                var d0 = DateTime.Now;
                using (var table = new DataTable())
                {
                    using (var ds = new SqlDataAdapter(cmd))
                    {
                        ds.Fill(table);
                    }
                    int cont = 0;
                    foreach (DataRow dataRow in table.Rows)
                    {
                        cont++;

                        if(cont == 340)
                        {
                            var x = "";
                        }

                        if (dataRow[0] != DBNull.Value) {
                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                res.Append(",");
                            }

                            res.Append(dataRow[0].ToString().Replace("\t", string.Empty));
                        }
                    }
                }

                res.Append("]");
            }
            catch (SqlException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
                return ex.Message;
            }
            catch (FormatException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
                return ex.Message;
            }
            catch (NullReferenceException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
                return ex.Message;
            }
            catch (NotSupportedException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
                return ex.Message;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
            }

            return res.ToString();
        }

        /// <summary>Obtains a JSON array structure of stored procedure results</summary>
        /// <param name="storedName">Stored procedure name</param>
        /// <param name="connectionString">Connection string</param>
        /// <returns>JSON array structure</returns>
        public static string SQLToJSON(string storedName, string connectionString)
        {
            if (string.IsNullOrEmpty(storedName) || string.IsNullOrEmpty(connectionString))
            {
                return Tools.Json.EmptyJsonList;
            }

            string res = Json.EmptyJsonList;
            using (var cmd = new SqlCommand(storedName))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    res = SQLToJSON(cmd);
                }
            }

            return res;
        }

        /// <summary>Get JSON format data from SQL command</summary>
        /// <param name="cmd">SQL command</param>
        /// <returns>An "application/json" format of data</returns>
        public static string SQLStreamSimple(SqlCommand cmd)
        {
            if (cmd == null)
            {
                return Json.EmptyJsonObject;
            }

            var source = string.Format(CultureInfo.InvariantCulture, "AltheraFramework.DataAccess.SqlStream.SQLToJSONSimple({0})", cmd.CommandText);
            var res = new StringBuilder();
            try
            {
                cmd.Connection.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        res.AppendFormat(CultureInfo.InvariantCulture, @"{{{0}}}", rdr.GetString(0));
                    }
                }
            }
            catch (SqlException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
                return ex.Message;
            }
            catch (FormatException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
                return ex.Message;
            }
            catch (NullReferenceException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
                return ex.Message;
            }
            catch (NotSupportedException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
                return ex.Message;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
            }

            return res.ToString();
        }

        /// <summary>Get JSON format data from SQL command</summary>
        /// <param name="cmd">SQL command</param>
        /// <returns>An "application/json" format of data</returns>
        public static string SQLStreamList(SqlCommand cmd)
        {
            if (cmd == null)
            {
                return Tools.Json.EmptyJsonList;
            }

            var res = new StringBuilder();
            try
            {
                cmd.Connection.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    res.Append("[");
                    if (rdr.HasRows)
                    {
                        var first = true;
                        while (rdr.Read())
                        {
                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                res.Append(",");
                            }

                            var x = rdr.GetString(0);

                            res.AppendFormat(CultureInfo.InvariantCulture, @"{{{0}}}", x.Replace("^","\\"));
                        }
                    }

                    res.Append("]");
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Trace(ex as Exception, "AltheraFramework.DataAccess.SqlStream.SQLToJSONSimple(" + cmd.CommandText + ")");
                return ex.Message;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
            }

            return res.ToString();
        }

        /// <summary>Get JSON format data from SQL command</summary>
        /// <param name="cmd">SQL command</param>
        /// <returns>An "application/json" format of data</returns>
        public static string SQLToJSON(SqlCommand cmd)
        {
            if (cmd == null)
            {
                return Tools.Json.EmptyJsonList;
            }

            var source = string.Format(CultureInfo.InvariantCulture, "AltheraFramework.DataAccess.SqlStream.SQLToJSON({0})", cmd.CommandText);
            var res = new StringBuilder("[");
            try
            {
                cmd.Connection.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    bool first = true;
                    while (rdr.Read())
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            res.Append(",");
                        }

                        res.Append("{");

                        for (int x = 0; x < rdr.FieldCount; x++)
                        {
                            if (x > 0)
                            {
                                res.Append(",");
                            }

                            res.AppendFormat(CultureInfo.InvariantCulture, @"""{0}"":", rdr.GetName(x));
                            string fieldType = rdr.GetFieldType(x).ToString().ToUpperInvariant();

                            if (rdr.IsDBNull(x))
                            {
                                res.Append("null");
                            }
                            else
                            {
                                switch (fieldType)
                                {
                                    case "SYSTEM.BOOLEAN":
                                        res.AppendFormat(
                                            CultureInfo.InvariantCulture,
                                            @"{0}",
                                            rdr.GetBoolean(x) ? Constant.True : Constant.False);
                                        break;
                                    case "SYSTEM.INT16":
                                    case "SYSTEM.INT32":
                                    case "SYSTEM.INT64":
                                    case "SYSTEM.DECIMAL":
                                        res.AppendFormat(
                                            CultureInfo.InvariantCulture,
                                            @"{0}",
                                            rdr[x]);
                                        break;
                                    case "SYSTEM.STRING":
                                    default:
                                        res.AppendFormat(
                                            CultureInfo.InvariantCulture,
                                            @"""{0}""",
                                            Tools.Json.JsonCompliant(rdr[x].ToString()));
                                        break;
                                }
                            }
                        }

                        res.Append(Environment.NewLine);
                        res.Append("}");
                    }
                }

                res.Append("]");
            }
            catch (SqlException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
                return ex.Message;
            }
            catch (FormatException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
                return ex.Message;
            }
            catch (NullReferenceException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
                return ex.Message;
            }
            catch (NotSupportedException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
                return ex.Message;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
            }

            return res.ToString();
        }

        /// <summary>Get CSV format data from SQL command</summary>
        /// <param name="cmd">SQL command</param>
        /// <returns>Plain text in CSV format</returns>
        public static string SqlToCSV(SqlCommand cmd)
        {
            if (cmd == null)
            {
                return string.Empty;
            }

            string source = string.Format(CultureInfo.InvariantCulture, "AltheraFramework.DataAccess.SqlStream.SqlToCSV({0})", cmd.CommandText);
            var res = new StringBuilder();
            try
            {
                cmd.Connection.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    bool first = true;
                    while (rdr.Read())
                    {
                        if (first)
                        {
                            first = false;
                            for (int x = 0; x < rdr.FieldCount; x++)
                            {
                                res.AppendFormat(CultureInfo.InvariantCulture, @"{0};", rdr.GetName(x));
                            }
                        }

                        for (int x = 0; x < rdr.FieldCount; x++)
                        {
                            string fieldType = rdr.GetFieldType(x).ToString().ToUpperInvariant();

                            if (rdr.IsDBNull(x))
                            {
                                res.Append(";");
                            }
                            else
                            {
                                switch (fieldType)
                                {
                                    case "SYSTEM.BOOLEAN":
                                        res.AppendFormat(
                                            CultureInfo.InvariantCulture,
                                            @"{0};",
                                            rdr.GetBoolean(x) ? Constant.True : Constant.False);
                                        break;
                                    case "SYSTEM.INT16":
                                    case "SYSTEM.INT32":
                                    case "SYSTEM.INT64":
                                    case "SYSTEM.DECIMAL":
                                        res.AppendFormat(
                                            CultureInfo.InvariantCulture,
                                            @"{0};",
                                            rdr[x]);
                                        break;
                                    case "SYSTEM.STRING":
                                    default:
                                        res.AppendFormat(
                                            CultureInfo.InvariantCulture,
                                            @"""{0}"";",
                                            rdr[x]);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (FormatException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
                return ex.Message;
            }
            catch (NullReferenceException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
                return ex.Message;
            }
            catch (InvalidCastException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
                return ex.Message;
            }
            catch (NotSupportedException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
                return ex.Message;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
            }

            return res.ToString();
        }
    }
}