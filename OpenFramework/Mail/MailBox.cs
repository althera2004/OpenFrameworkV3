// --------------------------------
// <copyright file="MailBox.cs" company="OpenFramework">
//     Copyright (c) 2019 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Mail
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using static OpenFrameworkV3.Core.Bindings.MailBox;

    [Serializable]
    public sealed partial class MailBox
    {
        /// <summary>Id of mailbox</summary>
        [JsonProperty("Id")]
        public long Id { get; set; }

        /// <summary>Company of mailbox</summary>
        [JsonProperty("CompanyId")]
        public long CompanyId { get; set; }

        /// <summary>Code of mailbox</summary>
        [JsonProperty("Code")]
        public string Code { get; set; }

        /// <summary>Name of server</summary>
        [JsonProperty("Server")]
        public string Server { get; set; }

        /// <summary>Mailbox address</summary>
        [JsonProperty("MailAddress")]
        public string MailAddress { get; set; }

        /// <summary>Name of sender</summary>
        [JsonProperty("SenderName")]
        public string SenderName { get; set; }

        /// <summary>Name of user</summary>
        [JsonProperty("MailUser")]
        public string MailUser { get; set; }

        /// <summary>Password of user</summary>
        [JsonProperty("MailPassword")]
        public string MailPassword { get; set; }

        /// <summary>Protocol of mailbox</summary>
        [JsonProperty("MailBoxType")]
        public string MailBoxType { get; set; }

        /// <summary>Reader port</summary>
        [JsonProperty("ReaderPort")]
        public int ReaderPort { get; set; }

        /// <summary>Sender port</summary>
        [JsonProperty("SendPort")]
        public int SendPort { get; set; }

        /// <summary>Reaing port</summary>
        [JsonProperty("SSL")]
        public bool SSL { get; set; }

        /// <summary>Reaing port</summary>
        [JsonProperty("Main")]
        public bool Main { get; set; }

        /// <summary>Active</summary>
        [JsonProperty("Active")]
        public bool Active { get; set; }

        public static MailBox Empty
        {
            get
            {
                return new MailBox
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    Code = string.Empty,
                    MailAddress = string.Empty,
                    Server = string.Empty,
                    SendPort = 0,
                    ReaderPort = 0,
                    SenderName = string.Empty,
                    MailPassword = string.Empty,
                    MailBoxType = string.Empty,
                    MailUser = string.Empty,
                    Active = false,
                    SSL = false,
                };
            }
        }

        public string Json
        {
            get
            {
                return "{}";
            }
        }

        public static ReadOnlyCollection<MailBox> ByCompanyId(long companyId, string instanceName)
        {
            var res = new List<MailBox>();
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Core_MailBox_ByCompanyId"))
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
                                if(rdr.HasRows)
                                {
                                    rdr.Read();
                                    res.Add(new MailBox
                                    {
                                        Id = rdr.GetInt64(ColumnsMailBoxGet.Id),
                                        Code = rdr.GetString(ColumnsMailBoxGet.Code).Trim(),
                                        Main = rdr.GetBoolean(ColumnsMailBoxGet.Main),
                                        MailAddress = rdr.GetString(ColumnsMailBoxGet.MailAddress).Trim(),
                                        Server = rdr.GetString(ColumnsMailBoxGet.Server).Trim(),
                                        SenderName = rdr.GetString(ColumnsMailBoxGet.SenderName).Trim(),
                                        MailUser = rdr.GetString(ColumnsMailBoxGet.MailUser).Trim(),
                                        MailPassword = rdr.GetString(ColumnsMailBoxGet.MailPassword).Trim(),
                                        MailBoxType = rdr.GetString(ColumnsMailBoxGet.MailBoxType).Trim(),
                                        ReaderPort = rdr.GetInt32(ColumnsMailBoxGet.ReadPort),
                                        SendPort = rdr.GetInt32(ColumnsMailBoxGet.SendPort),
                                        SSL = rdr.GetBoolean(ColumnsMailBoxGet.SSL),
                                        Active = rdr.GetBoolean(ColumnsMailBoxGet.Active)
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

            return new ReadOnlyCollection<MailBox>(res);
        }

        public static ReadOnlyCollection<MailBox> Load(string instanceName)
        {
            var res = new List<MailBox>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_MailBox_GetAll"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new MailBox
                                    {
                                        Id = rdr.GetInt64(ColumnsMailBoxGet.Id),
                                        Code = rdr.GetString(ColumnsMailBoxGet.Code).Trim(),
                                        MailAddress = rdr.GetString(ColumnsMailBoxGet.MailAddress).Trim(),
                                        Server = rdr.GetString(ColumnsMailBoxGet.Server).Trim(),
                                        MailUser = rdr.GetString(ColumnsMailBoxGet.MailUser).Trim(),
                                        MailPassword = rdr.GetString(ColumnsMailBoxGet.MailPassword).Trim(),
                                        MailBoxType = rdr.GetString(ColumnsMailBoxGet.MailBoxType).Trim(),
                                        ReaderPort = rdr.GetInt32(ColumnsMailBoxGet.ReadPort),
                                        SendPort = rdr.GetInt32(ColumnsMailBoxGet.SendPort),
                                        SSL = false,
                                        Active = rdr.GetBoolean(ColumnsMailBoxGet.Active)
                                    });
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
            }

            return new ReadOnlyCollection<MailBox>(res);
        }

        public ActionResult Save(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {

                if (this.Id <= 0)
                {
                    /* CREATE PROCEDURE [dbo].[Core_MailBox_Insert]
                     *   @Id bigint output,
                     *   @CompanyId bigint,
                     *   @Main bit,
                     *   @Code nchar(20),
                     *   @MailAddress nvarchar(150),
                     *   @SenderName nvarchar(100),
                     *   @Server nchar(100),
                     *   @MailBoxType nvarchar(10),
                     *   @ReadPort int,
                     *   @SendPort int,
                     *   @SSL bit,
                     *   @MailUser nvarchar(100),
                     *   @MailPassword nvarchar(50),
                     *   @Description nvarchar(100),
                     *   @ApplicationUserId bigint */
                    using (var cmd = new SqlCommand("Core_MailBox_Insert"))
                    {
                        using (var cnn = new SqlConnection(cns))
                        {
                            cmd.Connection = cnn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                            cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                            cmd.Parameters.Add(DataParameter.Input("@Main", this.Main));
                            cmd.Parameters.Add(DataParameter.Input("@Code", this.Code, 20));
                            cmd.Parameters.Add(DataParameter.Input("@MailAddress", this.MailAddress, 150));
                            cmd.Parameters.Add(DataParameter.Input("@SenderName", this.SenderName, 100));
                            cmd.Parameters.Add(DataParameter.Input("@Server", this.Server, 150));
                            cmd.Parameters.Add(DataParameter.Input("@MailBoxType", this.MailBoxType, 10));
                            cmd.Parameters.Add(DataParameter.Input("@ReadPort", this.ReaderPort));
                            cmd.Parameters.Add(DataParameter.Input("@SendPort", this.SendPort));
                            cmd.Parameters.Add(DataParameter.Input("@SSL", this.SSL));
                            cmd.Parameters.Add(DataParameter.Input("@MailUser", this.MailUser, 100));
                            cmd.Parameters.Add(DataParameter.Input("@MailPassword", this.MailPassword, 50));
                            cmd.Parameters.Add(DataParameter.Input("@Description", string.Empty, 100));
                            cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                            try
                            {
                                cmd.Connection.Open();
                                cmd.ExecuteNonQuery();
                                this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                                res.SetSuccess(string.Format(CultureInfo.InvariantCulture, "INSERT|{0}", this.Id));
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
                }
                else
                {
                    /* CREATE PROCEDURE [dbo].[Core_MailBox_Update]
                     *   @Id bigint,
                     *   @CompanyId bigint,
                     *   @Main bit,
                     *   @Code nchar(20),
                     *   @MailAddress nvarchar(150),
                     *   @SenderName nvarchar(100),
                     *   @Server nchar(100),
                     *   @MailBoxType nvarchar(10),
                     *   @ReadPort int,
                     *   @SendPort int,
                     *   @SSL bit,
                     *   @MailUser nvarchar(100),
                     *   @MailPassword nvarchar(50),
                     *   @Description nvarchar(100),
                     *   @ApplicationUserId bigint */
                    using (var cmd = new SqlCommand("Core_MailBox_Update"))
                    {
                        using (var cnn = new SqlConnection(cns))
                        {
                            cmd.Connection = cnn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                            cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                            cmd.Parameters.Add(DataParameter.Input("@Main", this.Main));
                            cmd.Parameters.Add(DataParameter.Input("@Code", this.Code, 20));
                            cmd.Parameters.Add(DataParameter.Input("@MailAddress", this.MailAddress, 150));
                            cmd.Parameters.Add(DataParameter.Input("@SenderName", this.SenderName, 100));
                            cmd.Parameters.Add(DataParameter.Input("@Server", this.Server, 150));
                            cmd.Parameters.Add(DataParameter.Input("@MailBoxType", this.MailBoxType, 10));
                            cmd.Parameters.Add(DataParameter.Input("@ReadPort", this.ReaderPort));
                            cmd.Parameters.Add(DataParameter.Input("@SendPort", this.SendPort));
                            cmd.Parameters.Add(DataParameter.Input("@SSL", this.SSL));
                            cmd.Parameters.Add(DataParameter.Input("@MailUser", this.MailUser, 100));
                            cmd.Parameters.Add(DataParameter.Input("@MailPassword", this.MailPassword, 50));
                            cmd.Parameters.Add(DataParameter.Input("@Description", string.Empty, 100));
                            cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                            try
                            {
                                cmd.Connection.Open();
                                cmd.ExecuteNonQuery();
                                res.SetSuccess(string.Format(CultureInfo.InvariantCulture, "UPDATE|{0}", this.Id));
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

                }
            }

            return res;
        }
    }
}