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
        public long CompanyId { get; set; }
        public bool IPAccess { get; set; }
        public int PasswordComplexity { get; set; }
        public bool PasswordCaducity { get; set; }
        public int PasswordCaducityDays { get; set; }
        public bool PasswordRepeat { get; set; }
        public int Traceability { get; set; }
        public int GrantPermission { get; set; }
        public int FailedAttempts { get; set; }
        public int MinimumPasswordLength { get; set; }
        public bool GroupUserMain { get; set; }

        public string FailedAttemptsMailNotification { get; set; }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""CompanyId"":{0},""IPAccess"":{1},""PasswordComplexity"":{2}, ""PasswordCaducity"":{8},""PasswordCaducityDays"":{9},""PasswordRepeat"":{10},""Traceability"":{3},""GrantPermission"":{4},""FailedAttempts"":{5},""MinimumPasswordLength"":{6},""GroupUserMain"":{7}}}",
                    this.CompanyId,
                    ConstantValue.Value(this.IPAccess),
                    this.PasswordComplexity,
                    this.Traceability,
                    this.GrantPermission,
                    this.FailedAttempts,
                    this.MinimumPasswordLength,
                    ConstantValue.Value(this.GroupUserMain),
                    ConstantValue.Value(this.PasswordCaducity),
                    this.PasswordCaducityDays,
                    ConstantValue.Value(this.PasswordRepeat));
            }
        }

        public static CompanySecurityConfig Empty
        {
            get
            {
                return new CompanySecurityConfig
                {
                    CompanyId = Constant.DefaultId,
                    IPAccess = false,
                    PasswordComplexity = 0,
                    PasswordCaducity = false,
                    PasswordCaducityDays = 0,
                    PasswordRepeat = false,
                    Traceability = 0,
                    GrantPermission = 0,
                    FailedAttempts = 0,
                    MinimumPasswordLength = 0,
                    GroupUserMain = false,
                    FailedAttemptsMailNotification = string.Empty,
                };
            }
        }

        public static CompanySecurityConfig Get(long companyId, string instanceName)
        {
            var res = Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Company_SecurityConfig_Get"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    res.CompanyId = companyId;
                                    res.IPAccess = rdr.GetBoolean(1);
                                    res.PasswordComplexity = rdr.GetInt32(2);
                                    res.PasswordCaducity = rdr.GetBoolean(3);
                                    res.PasswordCaducityDays = rdr.GetInt32(4);
                                    res.PasswordRepeat = rdr.GetBoolean(5);
                                    res.Traceability = rdr.GetInt32(6);
                                    res.GrantPermission = rdr.GetInt32(7);
                                    res.FailedAttempts = rdr.GetInt32(8);
                                    res.MinimumPasswordLength = rdr.GetInt32(9);
                                    res.GroupUserMain = rdr.GetBoolean(10);
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
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Core_Company_SecurityConfig_Save"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@IPAccess", this.IPAccess));
                        cmd.Parameters.Add(DataParameter.Input("@PasswordComplexity", this.PasswordComplexity));
                        cmd.Parameters.Add(DataParameter.Input("@PasswordCaducity", this.PasswordCaducity));
                        cmd.Parameters.Add(DataParameter.Input("@PasswordCaducityDays", this.PasswordCaducityDays));
                        cmd.Parameters.Add(DataParameter.Input("@PasswordRepeat", this.PasswordRepeat));
                        cmd.Parameters.Add(DataParameter.Input("@Traceability", this.Traceability));
                        cmd.Parameters.Add(DataParameter.Input("@GrantPermission", this.GrantPermission));
                        cmd.Parameters.Add(DataParameter.Input("@MinimumPasswordLength", this.MinimumPasswordLength));
                        cmd.Parameters.Add(DataParameter.Input("@GroupUserMain", this.GroupUserMain));
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
