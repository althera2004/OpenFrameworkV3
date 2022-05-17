// --------------------------------
// <copyright file="Access.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.Security
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using OpenFramework.DataAccess;
    using OpenFramework.InstanceManager;

    /// <summary>Implements Access control to application</summary>
    public partial class Access
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string IP { get; set; }
        public string IMEI { get; set; }
        public ApplicationUser User { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public static Access Empty
        {
            get
            {
                return new Access
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    IP = string.Empty,
                    IMEI = string.Empty,
                    User = ApplicationUser.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    ModifiedBy = ApplicationUser.Empty,
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
                    @"{{""Id"":{0},
                    ""CompanyId"":{1},
                    ""IP"":""{2}"",
                    ""IMEI"":""{3}"";
                    ""User"":{4},
                    ""CreatedBy"":{5},
                    ""CreatedOn"":""{6:dd/MM/yyyy}"",
                    ""ModifiedBy"":{7},
                    ""ModifiedOn"":""{8:dd/MM/yyyy}"",
                    ""Active"":{9}}}",
                    this.Id,
                    this.CompanyId,
                    this.IP,
                    this.IMEI,
                    this.User.JsonSimple,
                    this.CreatedBy.JsonSimple,
                    this.CreatedOn,
                    this.ModifiedBy.JsonSimple,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static string JsonList(ReadOnlyCollection<Access> list)
        {
            if(list == null)
            {
                return Tools.Json.EmptyJsonList;
            }

            var res = new StringBuilder("[");
            bool first = true;
            foreach(var item in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(item.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static Access ById(long id, long companyId)
        {
            /* CREATE PROCEDURE [dbo].[Core_Access_ById]
             *   @Id bigint,
             *   @CompanyId bigint */
            var res = Access.Empty;
            var cns = string.Empty;
            using (var instance = CustomerFramework.Actual)
            {
                cns = instance.Config.ConnectionString;
            }

            using (var cmd = new SqlCommand("Core_Access_ById"))
            {
                using (var cnn = new SqlConnection(cns))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res = new Access
                                {
                                    Id = rdr.GetInt64(ColumnsAccessGet.Id),
                                    CompanyId = rdr.GetInt64(ColumnsAccessGet.CompanyId),
                                    IP = rdr.GetString(ColumnsAccessGet.IP),
                                    IMEI = rdr.GetString(ColumnsAccessGet.IMEI),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsAccessGet.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsAccessGet.CreatedBy),
                                            Name = rdr.GetString(ColumnsAccessGet.CreatedByName),
                                            LastName = rdr.GetString(ColumnsAccessGet.CreatedByLastName),
                                            LastName2 = rdr.GetString(ColumnsAccessGet.CreatedByLastName2)
                                        }
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsAccessGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsAccessGet.ModifiedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsAccessGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnsAccessGet.ModifiedByName),
                                            LastName = rdr.GetString(ColumnsAccessGet.ModifiedByLastName),
                                            LastName2 = rdr.GetString(ColumnsAccessGet.ModifiedByLastName2)
                                        }
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsAccessGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsAccessGet.Active)
                                };

                                if (!rdr.IsDBNull(ColumnsAccessGet.UserId))
                                {
                                    res.User = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsAccessGet.UserId),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsAccessGet.UserId),
                                            Name = rdr.GetString(ColumnsAccessGet.UsuarioName),
                                            LastName = rdr.GetString(ColumnsAccessGet.UsuarioLastName),
                                            LastName2 = rdr.GetString(ColumnsAccessGet.UsuarioLastName2)
                                        }
                                    };
                                }
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

            return res;
        }

        public static ReadOnlyCollection<Access> All(long companyId)
        {
            /* CREATE PROCEDURE [dbo].[Core_Access_All]
             *   @CompanyId bigint */
            var res = new List<Access>();
            var cns = string.Empty;
            using (var instance = CustomerFramework.Actual)
            {
                cns = instance.Config.ConnectionString;
            }

            using (var cmd = new SqlCommand("Core_Access_All"))
            {
                using(var cnn = new SqlConnection(cns))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    try
                    {
                        cmd.Connection.Open();
                        using(var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newAccess = new Access
                                {
                                    Id = rdr.GetInt64(ColumnsAccessGet.Id),
                                    CompanyId = rdr.GetInt64(ColumnsAccessGet.CompanyId),
                                    IP = rdr.GetString(ColumnsAccessGet.IP),
                                    IMEI = rdr.GetString(ColumnsAccessGet.IMEI),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsAccessGet.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsAccessGet.CreatedBy),
                                            Name= rdr.GetString(ColumnsAccessGet.CreatedByName),
                                            LastName = rdr.GetString(ColumnsAccessGet.CreatedByLastName),
                                            LastName2 = rdr.GetString(ColumnsAccessGet.CreatedByLastName2)
                                        }
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsAccessGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsAccessGet.ModifiedBy),
                                        Profile =new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsAccessGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnsAccessGet.ModifiedByName),
                                            LastName = rdr.GetString(ColumnsAccessGet.ModifiedByLastName),
                                            LastName2 = rdr.GetString(ColumnsAccessGet.ModifiedByLastName2)
                                        }
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsAccessGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsAccessGet.Active)
                                };

                                if (!rdr.IsDBNull(ColumnsAccessGet.UserId))
                                {
                                    newAccess.User = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsAccessGet.UserId),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsAccessGet.UserId),
                                            Name = rdr.GetString(ColumnsAccessGet.UsuarioName),
                                            LastName = rdr.GetString(ColumnsAccessGet.UsuarioLastName),
                                            LastName2 = rdr.GetString(ColumnsAccessGet.UsuarioLastName2)
                                        }
                                    };
                                }
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

            return new ReadOnlyCollection<Access>(res);
        }
    }
}