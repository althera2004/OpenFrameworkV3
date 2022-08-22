// --------------------------------
// <copyright file="IPAccess.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Security
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;

    public class IPAccess
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string IP { get; set; }
        public string IMEI { get; set; }
        public string Description { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public bool Active;

        public static IPAccess Empty
        {
            get
            {
                return new IPAccess
                {
                    Id = Constant.DefaultId,
                    IP = string.Empty,
                    IMEI = string.Empty,
                    Description = string.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public string JsonJeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},
                    ""IP"":""{1}"",
                    ""IMEI"":""{2}"",
                    ""Description"":""{3}"",
                    ""Active"":{4}}}",
                    this.Id,
                    this.IP,
                    this.IMEI,
                    Tools.Json.JsonCompliant(this.Description),
                    ConstantValue.Value(this.Active));
            }
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},
                    ""IP"":""{1}"",
                    ""IMEI"":""{2}"",
                    ""Description"":""{3}"",
                    ""CreatedBy"":{4},
                    ""CreatedOn"":""{5:dd/MM/yyyy}"",
                    ""ModifiedBy"":{6},
                    ""ModifiedOn"":""{7:dd/MM/yyyy}"",
                    ""Active"":{8}}}",
                    this.Id,
                    this.IP,
                    this.IMEI,
                    Tools.Json.JsonCompliant(this.Description),
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static string JsonList(ReadOnlyCollection<IPAccess> list)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach(var ipAccess in list)
            {
                if(first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(ipAccess.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public ActionResult Insert(long applicationUserId,string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[Core_IPAccess_Insert]
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @IP nchar(15),
             *   @IMEI ncahr(40),
             *   @Description nvarchar(100),
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);

            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Core_IPAccess_Insert"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@IP", this.IP, 15));
                        cmd.Parameters.Add(DataParameter.Input("@IMEI", this.IP, 40));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(this.Id);
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

            return res;
        }

        public ActionResult Update(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[Core_IPAccess_Update]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @IP nchar(15),
             *   @MEI nchar(40),
             *   @Description nvarchar(100),
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);

            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Core_IPAccess_Update"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@IP", this.IP, 15));
                        cmd.Parameters.Add(DataParameter.Input("@IP", this.IMEI, 40));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(this.Id);
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

            return res;
        }

        public static ActionResult Activate(long id, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[Core_IPAccess_Activate]
             *   @Id bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);

            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Core_IPAccess_Activate"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(id);
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

            return res;
        }

        public static ActionResult Inactivate(long id, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[Core_IPAccess_Inactivate]
             *   @Id bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);

            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Core_IPAccess_Inactivate"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(id);
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

            return res;
        }

        public static ReadOnlyCollection<IPAccess> All(string instanceName)
        {
            var res = new List<IPAccess>();
            var cns = Persistence.ConnectionString(instanceName);

            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Core_IPAccess_All"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                while(rdr.Read())
                                {
                                    res.Add(new IPAccess
                                    {
                                        Id = rdr.GetInt64(0),
                                        CompanyId = rdr.GetInt64(1),
                                        IP = rdr.GetString(3),
                                        IMEI = rdr.GetString(4),
                                        Description = rdr.GetString(5),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(6),
                                            Profile = new UserProfile
                                            {
                                                ApplicationUserId = rdr.GetInt64(6),
                                                Name = rdr.GetString(7),
                                                LastName = rdr.GetString(8),
                                                LastName2 = rdr.GetString(9)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(10),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(11),
                                            Profile = new UserProfile
                                            {
                                                ApplicationUserId = rdr.GetInt64(11),
                                                Name = rdr.GetString(12),
                                                LastName = rdr.GetString(13),
                                                LastName2 = rdr.GetString(14)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(15),
                                        Active = rdr.GetBoolean(16)
                                    });
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

            return new ReadOnlyCollection<IPAccess>(res);
        }

        public static ActionResult CheckIP(string instanceName, string IP, long companyId)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"IPAcess::CheckIP({0}, {1}", IP, companyId);
            /* CREATE PROCEDURE Core_IPAccess_CheckIP 
             *   @CompanyId bigint,
             *   @IP nchar(15),
             *   @Result int output */
            var cns = Persistence.ConnectionString(instanceName);

            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Core_IPAccess_CheckIP"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@IP", IP, 15));
                        cmd.Parameters.Add(DataParameter.OutputInt("@Result"));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess((int)cmd.Parameters["@Result"].Value);
                        }
                        catch(Exception ex)
                        {
                            res.SetSuccess(ex);
                            ExceptionManager.Trace(ex, source);
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

            return res;
        }
    }
}
