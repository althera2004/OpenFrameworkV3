namespace OpenFrameworkV3.Core.Companies
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using OpenFrameworkV3;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;

    public class CompanySecurityConfig
    {
        public static readonly int GrantPermissionByUser = 1;
        public static readonly int GrantPermissionByGroup = 0;

        public static readonly int MFAInactive = 0;
        public static readonly int MFAByMail = 1;
        public static readonly int MFAByQR = 2;
        public static readonly int MFABySMS = 3;

        public static readonly int PasswordComplexityLeak = 0;
        public static readonly int PasswordComplexityBasic = 1;
        public static readonly int PasswordComplexityStrong = 2;

        public long CompanyId { get; set; }
        public bool IPAccess { get; set; }
        public int MFA { get; set; }
        public bool CorportiveUsers { get; set; }
        public int GrantPermission { get; set; }
        public int Traceability { get; set; }
        public int FailedAttempts { get; set; }
        public string FailedAttepmtsMailNotification { get; set; }
        public int FailedAttemptsSaveDays { get; set; }
        public int MinimumPasswordLength { get; set; }
        public int PasswordComplexity { get; set; }
        public bool GroupUserMain { get; set; }
        public bool PasswordCaducity { get; set; }
        public int PasswordCaducityDays { get; set; }
        public bool PasswordRepeat { get; set; }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""CompanyId"":{0}}}",
                    this.CompanyId);
            }
        }

        public static CompanySecurityConfig Empty
        {
            get
            {
                return new CompanySecurityConfig
                {
                    CompanyId = Constant.DefaultId,
                    CorportiveUsers = false,
                    FailedAttempts = 3,
                    FailedAttemptsSaveDays = -1,
                    FailedAttepmtsMailNotification = string.Empty,
                    GrantPermission = GrantPermissionByGroup,
                    GroupUserMain = false,
                    IPAccess = false,
                    MFA = 0,
                    MinimumPasswordLength = 6,
                    PasswordCaducity = false,
                    PasswordCaducityDays = -1,
                    PasswordComplexity = PasswordComplexityBasic,
                    PasswordRepeat = true,
                    Traceability = 1
                };
            }
        }

        public static CompanySecurityConfig ByCompany(long companyId, string instanceName)
        {
            var res = new CompanySecurityConfig();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Company_SecurityConfig_ByCompany"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    res.CompanyId = rdr.GetInt64(0);
                                    res.IPAccess = rdr.IsDBNull(1) ? false : rdr.GetBoolean(1);
                                    res.MFA = rdr.IsDBNull(2) ? (int)Constant.DefaultId : rdr.GetInt32(2);
                                    res.CorportiveUsers = rdr.IsDBNull(3) ? false : rdr.GetBoolean(3);
                                    res.GrantPermission = rdr.IsDBNull(4) ? (int)Constant.DefaultId : rdr.GetInt32(4);
                                    res.Traceability = rdr.IsDBNull(5) ? (int)Constant.DefaultId : rdr.GetInt32(5);
                                    res.FailedAttempts = rdr.IsDBNull(6) ? (int)Constant.DefaultId : rdr.GetInt32(6);
                                    res.FailedAttepmtsMailNotification = rdr.IsDBNull(7) ? string.Empty : rdr.GetString(7);
                                    res.FailedAttemptsSaveDays = rdr.IsDBNull(8) ? (int)Constant.DefaultId : rdr.GetInt32(8);
                                    res.MinimumPasswordLength = rdr.IsDBNull(9) ? (int)Constant.DefaultId : rdr.GetInt32(9);
                                    res.PasswordComplexity = rdr.IsDBNull(10) ? (int)Constant.DefaultId : rdr.GetInt32(10);
                                    res.PasswordCaducity = rdr.IsDBNull(11) ? false : rdr.GetBoolean(11);
                                    res.PasswordCaducityDays = rdr.IsDBNull(12) ? (int)Constant.DefaultId : rdr.GetInt32(12);
                                    res.PasswordRepeat = rdr.IsDBNull(13) ? false : rdr.GetBoolean(13);
                                    res.GroupUserMain = rdr.IsDBNull(14) ? false : rdr.GetBoolean(14);
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

            return res;
        }

        public ActionResult Save(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_Company_SecurityConfig_Save
             *   @CompanyId bigint,
             *   @IPAccess bit,
             *   @PasswordComplexity int,
             *   @Traceability int,
             *   @GrantPermission int,
             *   @FailedAttempts int,
             *   @MinimumPasswordLength int,
             *   @GroupUserMain bit,
             *   @FailedAttemptsMailNotification nvarchar(150),
             *   @MFA int,
             *   @CorporativeUsers bit,
             *   @FailedAttemptsSaveDays int,
             *   @PasswordCaducity bit,
             *   @PasswordCaducityDays int,
             *   @PasswordRepeat bit,
             *   @ApplicationUserId bigint */

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Company_SecurityConfig_Save"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@IPAccess", this.IPAccess));
                        cmd.Parameters.Add(DataParameter.Input("@PasswordComplexity", this.PasswordComplexity));
                        cmd.Parameters.Add(DataParameter.Input("@GrantPermission", this.GrantPermission));
                        cmd.Parameters.Add(DataParameter.Input("@FailedAttempts", this.FailedAttempts));
                        cmd.Parameters.Add(DataParameter.Input("@MinimumPasswordLength", this.MinimumPasswordLength));
                        cmd.Parameters.Add(DataParameter.Input("@FailedAttemptsMailNotification", this.FailedAttepmtsMailNotification, 150));
                        cmd.Parameters.Add(DataParameter.Input("@MFA", this.MFA));
                        cmd.Parameters.Add(DataParameter.Input("@CorporativeUsers", this.CorportiveUsers));
                        cmd.Parameters.Add(DataParameter.Input("@FailedAttemptsSaveDays", this.FailedAttemptsSaveDays));
                        cmd.Parameters.Add(DataParameter.Input("@PasswordCaducity", this.PasswordCaducity));
                        cmd.Parameters.Add(DataParameter.Input("@PasswordCaducityDays", this.PasswordCaducityDays));
                        cmd.Parameters.Add(DataParameter.Input("@PasswordRepeat", this.PasswordRepeat));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
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

            return res;
        }
    }
}