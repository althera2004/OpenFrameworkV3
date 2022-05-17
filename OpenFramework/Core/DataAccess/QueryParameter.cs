// --------------------------------
// <copyright file="QueryParameter.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.DataAccess
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core.ItemManager.ItemList;
    using OpenFrameworkV3.Tools;

    /// <summary>Parameters for database query</summary>
    public class QueryParameter
    {
        /// <summary>Gets or sets parameter name</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets parameter value</summary>
        public string Value { get; set; }

        /// <summary>Gets parameters from JSON string and adds "CompanyId" parameter</summary>
        /// <param name="parameters">JSON string with parameters</param>
        /// <returns>List of SQL parameters</returns>
        public static ReadOnlyCollection<SqlParameter> FromStringAndCompany(string parameters, long companyId)
        {
            var res = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(parameters))
            {
                var paremetersClear = Basics.Base64Decode(parameters);
                var paramsList = JsonConvert.DeserializeObject<List<QueryParameter>>(paremetersClear);
                foreach (var param in paramsList)
                {
                    var sqlParam = new SqlParameter
                    {
                        ParameterName = "@" + param.Name,
                        Direction = ParameterDirection.Input,
                        Value = param.Value
                    };

                    res.Add(sqlParam);
                }
            }
            
            res.Add(new SqlParameter
            {
                ParameterName = "@CompanyId",
                Direction = ParameterDirection.Input,
                Value = companyId
            });

            return new ReadOnlyCollection<SqlParameter>(res);
        }

        /// <summary>Gets parameters from JSON string and</summary>
        /// <param name="parameters">JSON string with parameters</param>
        /// <returns>List of SQL parameters</returns>
        public static ReadOnlyCollection<SqlParameter> FromString(string parameters)
        {
            var parametersJson = Basics.Base64Decode(parameters);
            var res = new List<SqlParameter>();
            var paramsList = JsonConvert.DeserializeObject<List<QueryParameter>>(parametersJson);
            foreach (var param in paramsList)
            {
                var sqlParam = new SqlParameter
                {
                    ParameterName = "@" + param.Name,
                    Direction = ParameterDirection.Input,
                    Value = param.Value
                };

                res.Add(sqlParam);
            }

            return new ReadOnlyCollection<SqlParameter>(res);
        }

        /// <summary>Gets parameters from list parameters</summary>
        /// <param name="parameters">List parameters</param>
        /// <returns>List of SQL parameters</returns>
        public static ReadOnlyCollection<SqlParameter> FromListParameter(ReadOnlyCollection<ListParameter> parameters)
        {
            var res = new List<SqlParameter>();
            foreach (var param in parameters)
            {
                var sqlParam = new SqlParameter
                {
                    ParameterName = "@" + param.Name,
                    Direction = ParameterDirection.Input,
                    Value = param.Value
                };

                res.Add(sqlParam);
            }

            return new ReadOnlyCollection<SqlParameter>(res);
        }
    }
}