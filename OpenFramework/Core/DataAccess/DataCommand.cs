// --------------------------------
// <copyright file="DataCommand.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.DataAccess
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>Implementation of Command class.</summary>
    public class DataCommand : IDisposable
    {
        /// <summary>SQL command to build</summary>
        private SqlCommand command;

        /// <summary>Gets a SQL Command that calls a stored procedure</summary>
        /// <param name="storedName">Stored procedure name</param>
        /// <returns>A SQL command</returns>
        public SqlCommand Stored(string storedName)
        {
            if (!string.IsNullOrEmpty(storedName))
            {
                this.command = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = storedName,
                    Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString)
                };

                return this.command;
            }

            return new SqlCommand();
        }

        /// <summary>Dispose Command class</summary>
        public void Dispose()
        {
            this.Dispose(Constant.Disposable);
            GC.SuppressFinalize(this);
        }

        /// <summary>The bulk of the clean-up code is implemented in Dispose(bool)</summary>
        /// <param name="disposing">Disposing managed objects</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.command != null)
                {
                    if (this.command.Connection != null)
                    {
                        this.command.Connection.Dispose();
                    }

                    this.command.Dispose();
                }
            }
        }
    }
}