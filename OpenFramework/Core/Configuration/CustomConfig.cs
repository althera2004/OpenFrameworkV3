// --------------------------------
// <copyright file="CustomConfig.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using OpenFrameworkV3.Core.Bindings;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;

    [Serializable]
    public partial class CustomConfig
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Label { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public static CustomConfig Empty
        {
            get
            {
                return new CustomConfig
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    Key = string.Empty,
                    Value = string.Empty,
                    CreatedBy = ApplicationUser.OpenFramework,
                    ModifiedBy = ApplicationUser.OpenFramework,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""CompanyId"":{1},""Key"":""{2}"",""Value"":""{3}"",""Active"":{4}}}",
                    this.Id,
                    this.CompanyId,
                    Tools.Json.JsonCompliant(this.Key),
                    Tools.Json.JsonCompliant(this.Value),
                    ConstantValue.Value(this.Active));
            }
        }

        public static string JsonList(ReadOnlyCollection<CustomConfig> list)
        {
            var res = new StringBuilder("[");
            if(list != null && list.Count > 0)
            {
                bool first = true;
                foreach(var customConfig in list)
                {
                    if(first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(customConfig.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        public static string JsonListAll(string instanceName)
        {
                return JsonList(All(instanceName));
        }

        public static ReadOnlyCollection<CustomConfig> All(string instanceName)
        {
            var res = new List<CustomConfig>();
            var user = ApplicationUser.Actual;
            var cns = Persistence.ConnectionString(instanceName);

            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Core_CustomConfig_GetAll"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", user.CompanyId));
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                while(rdr.Read())
                                {
                                    var newCustomConfig = new CustomConfig
                                    {
                                        Id = rdr.GetInt64(ColumnsCustomConfigGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsCustomConfigGet.CompanyId),
                                        Key = rdr.GetString(ColumnsCustomConfigGet.Key),
                                        Value = rdr.GetString(ColumnsCustomConfigGet.Value),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsCustomConfigGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsCustomConfigGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsCustomConfigGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsCustomConfigGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsCustomConfigGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsCustomConfigGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsCustomConfigGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsCustomConfigGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsCustomConfigGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsCustomConfigGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsCustomConfigGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsCustomConfigGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsCustomConfigGet.Active)
                                    };

                                    res.Add(newCustomConfig);
                                }
                            }
                        }
                        finally
                        {
                            if(cmd.Connection.State != ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return new ReadOnlyCollection<CustomConfig>(res);
        }
    }
}