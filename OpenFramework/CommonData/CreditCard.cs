namespace OpenFrameworkV3.CommonData
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;

    public class CreditCard
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string MerchantId { get; set; }
        public string AccountId { get; set; }
        public string SharedSecret { get; set; }
        public string ServiceUrl { get; set; }
        public bool Main { get; set; }
        public bool SandBox { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public static CreditCard Empty
        {
            get
            {
                return new CreditCard
                {
                    Id = Constant.DefaultId,
                    Main = false,
                    SandBox = false,
                    AccountId = string.Empty,
                    MerchantId = string.Empty,
                    SharedSecret = string.Empty,
                    ServiceUrl = string.Empty,
                    CompanyId = Constant.DefaultId,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
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
                    @"{{""Id"":{0}, ""CompanyId"":{1}, ""Main"":{2}, ""SandBox"":{3}, ""MerchantId"":""{4}"", ""AccountId"":""{5}"", ""SharedSecret"":""{6}"", ""ServiceUrl"":""{7}"", ""CreatedBy"":{8}, ""CreatedOn"":""{9:dd/MM/yyyy}"", ""ModifiedBy"": {10}, ""ModifiedOn"":""{11:dd/MM/yyyy}"", ""Active"":{12}}}",
                    this.Id,
                    this.CompanyId,
                    ConstantValue.Value(this.Main),
                    ConstantValue.Value(this.SandBox),
                    Tools.Json.JsonCompliant(this.MerchantId),
                    Tools.Json.JsonCompliant(this.AccountId),
                    Tools.Json.JsonCompliant(this.SharedSecret),
                    Tools.Json.JsonCompliant(this.ServiceUrl),
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public ActionResult Insert(CreditCard card, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                /* CREATE PROCEDURE [dbo].[Feature_CreditCard_Insert]
                 *   @Id bigint output,
                 *   @CompanyId bigint,
                 *   @Main bit,
                 *   @SandBox bit,
                 *   @MerchantId nvarchar(50),
                 *   @AccountId nvarchar(50),
                 *   @SharedSecret nvarchar(50),
                 *   @ServiceUrl nvarchar(50),
                 *   @ApplicationUserId bigint */
                using(var cmd = new SqlCommand("Feature_CreditCard_Insert"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", card.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Main", card.Main));
                        cmd.Parameters.Add(DataParameter.Input("@SandBox", card.SandBox));
                        cmd.Parameters.Add(DataParameter.Input("@MerchantId", card.MerchantId, 50));
                        cmd.Parameters.Add(DataParameter.Input("@AccountId", card.AccountId, 50));
                        cmd.Parameters.Add(DataParameter.Input("@SharedSecret", card.SharedSecret, 50));
                        cmd.Parameters.Add(DataParameter.Input("@ServiceUrl", card.ServiceUrl, 50));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            card.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(card.Id);
                        }
                        catch(Exception ex)
                        {
                            res.SetFail(ex);
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

        public ActionResult Update(CreditCard card, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                /* CREATE PROCEDURE [dbo].[Feature_CreditCard_Update]
                 *   @Id bigint,
                 *   @CompanyId bigint,
                 *   @Main bit,
                 *   @SandBox bit,
                 *   @MerchantId nvarchar(50),
                 *   @AccountId nvarchar(50),
                 *   @SharedSecret nvarchar(50),
                 *   @ServiceUrl nvarchar(50),
                 *   @ApplicationUserId bigint */
                using(var cmd = new SqlCommand("Feature_CreditCard_Update"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", card.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", card.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Main", card.Main));
                        cmd.Parameters.Add(DataParameter.Input("@SandBox", card.SandBox));
                        cmd.Parameters.Add(DataParameter.Input("@MerchantId", card.MerchantId, 50));
                        cmd.Parameters.Add(DataParameter.Input("@AccountId", card.AccountId, 50));
                        cmd.Parameters.Add(DataParameter.Input("@SharedSecret", card.SharedSecret, 50));
                        cmd.Parameters.Add(DataParameter.Input("@ServiceUrl", card.ServiceUrl, 50));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            card.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(card.Id);
                        }
                        catch(Exception ex)
                        {
                            res.SetFail(ex);
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

        public static ActionResult SetMain(long id,long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                /* CREATE PROCEDURE [dbo].[Feature_CreditCard_SetMain]
                 *   @Id bigint,
                 *   @CompanyId bigint,
                 *   @ApplicationUserId bigint */
                using(var cmd = new SqlCommand("Feature_CreditCard_SetMain"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(id);
                        }
                        catch(Exception ex)
                        {
                            res.SetFail(ex);
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

        public static CreditCard ById(long id, long companyId, string instanceName)
        {
            var res = CreditCard.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                /* CREATE PROCEDURE [dbo].[Feature_CreditCard_ById]
                 *  @Id bigint,
                 *  @CompanyId bigint */
                using(var cmd = new SqlCommand("Feature_CreditCard_ById"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                if(rdr.HasRows)
                                {
                                    rdr.Read();
                                    res.Id = rdr.GetInt64(0);
                                    res.Main = rdr.GetBoolean(1);
                                    res.SandBox = rdr.GetBoolean(2);
                                    res.MerchantId = rdr.GetString(3).Trim();
                                    res.AccountId = rdr.GetString(4).Trim();
                                    res.SharedSecret = rdr.GetString(5).Trim();
                                    res.ServiceUrl = rdr.GetString(6).Trim();
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(7),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(7),
                                            Name = rdr.GetString(8)
                                        }
                                    };
                                    res.CreatedOn = rdr.GetDateTime(10);
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(11),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(11),
                                            Name = rdr.GetString(12)
                                        }
                                    };
                                    res.ModifiedOn = rdr.GetDateTime(13);
                                    res.Active = rdr.GetBoolean(14);
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

            return res;
        }

        public static CreditCard GetMain(long companyId, string instanceName)
        {
            var res = CreditCard.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                /* CREATE PROCEDURE [dbo].[Feature_CreditCard_GetMain]
                 *  @CompanyId bigint */
                using(var cmd = new SqlCommand("Feature_CreditCard_GetMain"))
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
                                    res.Id = rdr.GetInt64(0);
                                    res.CompanyId = rdr.GetInt64(1);
                                    res.Main = rdr.GetBoolean(2);
                                    res.SandBox = rdr.GetBoolean(3);
                                    res.MerchantId = rdr.GetString(4).Trim();
                                    res.AccountId = rdr.GetString(5).Trim();
                                    res.SharedSecret = rdr.GetString(6).Trim();
                                    res.ServiceUrl = rdr.GetString(7).Trim();
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(8),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(8),
                                            Name = rdr.GetString(9)
                                        }
                                    };
                                    res.CreatedOn = rdr.GetDateTime(10);
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(11),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(11),
                                            Name = rdr.GetString(12)
                                        }
                                    };
                                    res.ModifiedOn = rdr.GetDateTime(13);
                                    res.Active = rdr.GetBoolean(14);
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

            return res;
        }
    }
}
