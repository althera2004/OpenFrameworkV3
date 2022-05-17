// --------------------------------
// <copyright file="ExecuteQuery.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using OpenFrameworkV3.Core.Activity;

    /// <summary>Implements execution of SQL commands</summary>
    public class ExecuteQuery
    {
        /// <summary>List of parameters</summary>
        private List<SqlParameter> parameters;

        /// <summary>Gets or sets SQL query or stored procedure name</summary>
        public string QueryText { get; set; }

        /// <summary>Gets or sets string connection to database</summary>
        public string ConnectionString { get; set; }

        /// <summary>Gets result of launch command based on store procedure</summary>
        public ActionResult ExecuteProcedure
        {
            get
            {
                return this.LaunchQuery(CommandType.StoredProcedure);
            }
        }

        /// <summary>Gets result of launch command based on SQL query</summary>
        public ActionResult ExecuteCommand
        {
            get
            {
                return this.LaunchQuery(CommandType.Text);
            }
        }

        /// <summary>Add parameter on command</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public void AddParameter(string name, object value)
        {
            if (this.parameters == null)
            {
                this.parameters = new List<SqlParameter>();
            }

            switch (value.GetType().ToString().ToUpperInvariant())
            {
                case "INT":
                    int valueInteger = 0;
                    if (int.TryParse(value as string, out valueInteger))
                    {
                        this.parameters.Add(DataParameter.Input(name, valueInteger));
                    }
                    else
                    {
                        this.parameters.Add(DataParameter.InputNull(name));
                    }

                    break;
                case "LONG":
                    long valueLong = 0;
                    if (long.TryParse(value as string, out valueLong))
                    {
                        this.parameters.Add(DataParameter.Input(name, valueLong));
                    }
                    else
                    {
                        this.parameters.Add(DataParameter.InputNull(name));
                    }

                    break;
                case "FLOAT":
                    float valueFloat = 0;
                    if (float.TryParse(value as string, out valueFloat))
                    {
                        this.parameters.Add(DataParameter.Input(name, valueFloat));
                    }
                    else
                    {
                        this.parameters.Add(DataParameter.InputNull(name));
                    }

                    break;
                case "DECIMAL":
                    decimal valueDecimal = 0;
                    if (decimal.TryParse(value as string, out valueDecimal))
                    {
                        this.parameters.Add(DataParameter.Input(name, valueDecimal));
                    }
                    else
                    {
                        this.parameters.Add(DataParameter.InputNull(name));
                    }

                    break;
                case "BOOLEAN":
                    bool valueBoolean = false;
                    if (bool.TryParse(value as string, out valueBoolean))
                    {
                        this.parameters.Add(DataParameter.Input(name, valueBoolean));
                    }
                    else
                    {
                        this.parameters.Add(DataParameter.InputNull(name));
                    }

                    break;
                case "DATETIME":
                    DateTime valueDateTime;
                    if (DateTime.TryParse(value as string, out valueDateTime))
                    {
                        this.parameters.Add(DataParameter.Input(name, valueDateTime));
                    }
                    else
                    {
                        this.parameters.Add(DataParameter.InputNull(name));
                    }

                    break;
                default:
                    this.parameters.Add(DataParameter.Input(name, value as string));
                    break;
            }
        }

        /// <summary>Add parameter on command</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <param name="length">Parameter length</param>
        public void AddParameter(string name, string value, int length)
        {
            if (this.parameters == null)
            {
                this.parameters = new List<SqlParameter>();
            }

            this.parameters.Add(DataParameter.Input(name, value, length));
        }

        /// <summary>Launch SQL command</summary>
        /// <param name="commandType">Type of command</param>
        /// <returns>Result of action</returns>
        private ActionResult LaunchQuery(CommandType commandType)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand(commandType == CommandType.Text ? "Core_ExecuteQuery" : this.QueryText))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (commandType == CommandType.Text)
                {
                    cmd.Parameters.Add(new SqlParameter("@statement", SqlDbType.NText));
                    cmd.Parameters["@statement"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@statement"].Value = this.QueryText;
                }

                using (var cnn = new SqlConnection(this.ConnectionString))
                {
                    if (this.parameters != null)
                    {
                        foreach (SqlParameter parameter in this.parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }

                    try
                    {
                        cmd.Connection = cnn;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
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
    }
}