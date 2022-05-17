namespace OpenFrameworkV3.Core.Security
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;

    public class AccessPolicy
    {
        public long Id { get; set; }

        public long CompanyId { get; set; }

        public string AllowIPs { get; set; }

        public bool MFAEmail { get; set; }

        public bool MFAQR { get; set; }

        public bool MFASMS { get; set; }

        public ApplicationUser CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public ApplicationUser ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool Active { get; set; }

        public bool CorportativeEnabled { get; set; }

        public static AccessPolicy Empty
        {
            get
            {
                return new AccessPolicy
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    AllowIPs = string.Empty,
                    MFAEmail = false,
                    MFAQR = false,
                    MFASMS = false,
                    CreatedBy = ApplicationUser.OpenFramework,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.OpenFramework,
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
                    @"{{""Id"":{0}, ""CompanyId"":{1}, ""AllowIPs"": {2}, ""MFAEmail"":{3}, ""MFAQR"":{4}, ""MFASMS"":{5},""CorporativeEnabled"":{6}, ""CreatedBy"":{7}, ""CreatedOn"":""{8:dd/MM/yyyy}"", ""ModifiedBy"": {9}, ""ModifiedOn"":""{10:dd/MM/yyyy}"", ""Active"":{11}}}",
                    this.Id,
                    this.CompanyId,
                    string.IsNullOrEmpty(this.AllowIPs) ? Constant.JavaScriptNull : "\"" + this.AllowIPs + "\"",
                    ConstantValue.Value(this.MFAEmail),
                    ConstantValue.Value(this.MFAQR),
                    ConstantValue.Value(this.MFASMS),
                    ConstantValue.Value(this.CorportativeEnabled),
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static AccessPolicy ByCompany(long companyId, string instanceName)
        {
            var res = Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Core_Company_AccessPolicy_ByCompanyId"))
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
                                    res.AllowIPs = rdr.GetString(2);
                                    res.CorportativeEnabled = rdr.GetBoolean(3);
                                    res.MFAEmail = rdr.GetBoolean(4);
                                    res.MFAQR = rdr.GetBoolean(5);
                                    res.MFASMS = rdr.GetBoolean(6);
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(7),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(7),
                                            Name = string.Empty,
                                            LastName = string.Empty,
                                            LastName2 = string.Empty
                                        }
                                    };
                                    res.CreatedOn = rdr.GetDateTime(8);
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(9),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(9),
                                            Name = string.Empty,
                                            LastName = string.Empty,
                                            LastName2 = string.Empty
                                        }
                                    };
                                    res.ModifiedOn = rdr.GetDateTime(10);
                                    res.Active = rdr.GetBoolean(11);
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

        public ActionResult Save(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoResult;
            var cns = Persistence.ConnectionString(instanceName);

            if (!string.IsNullOrEmpty(cns))
            {
                /* CREATE PROCEDURE Core_Company_AccessPolicy_Save
                 *   @CompanyId bigint,
                 *   @AllowIPs nchar(2000),
                 *   @CorportativeEnabled bit,
                 *   @MFAMail bit,
                 *   @MFAQR bit,
                 *   @MFASMS bit,
                 *   @ApplicationUserId bigint */
                using(var cmd = new SqlCommand("Core_Company_AccessPolicy_Save"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@AllowIPs", this.AllowIPs, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@CorporativeEnabled", this.CorportativeEnabled));
                        cmd.Parameters.Add(DataParameter.Input("@MFAMail", this.MFAEmail));
                        cmd.Parameters.Add(DataParameter.Input("@MFAQR", this.MFAQR));
                        cmd.Parameters.Add(DataParameter.Input("@MFASMS", this.MFASMS));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
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
    }
}