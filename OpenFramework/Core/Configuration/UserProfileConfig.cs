// -----------------------------------------------------------------------
// <copyright file="ProfileConfig.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace OpenFrameworkV3.Core.Configuration
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using OpenFrameworkV3;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;

    public class UserProfileConfig
    {
        public long CompanyId { get; set; }
        public int UserName { get; set; }
        public bool Email { get; set; }
        public bool AlternativeEmail { get; set; }
        public bool Phone { get; set; }
        public bool Mobile { get; set; }
        public bool Fax { get; set; }
        public bool EmergencyPhone { get; set; }
        public bool Gender { get; set; }
        public bool Birthday { get; set; }
        public bool IdentificationCard { get; set; }
        public bool Nacionality { get; set; }
        public bool LinkedIn { get; set; }
        public bool Twitter { get; set; }
        public bool Instagram { get; set; }
        public bool Facebook { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
        public string Doc1 { get; set; }
        public string Doc2 { get; set; }
        public string Doc3 { get; set; }
        public string Doc4 { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public static UserProfileConfig Empty
        {
            get
            {
                return new UserProfileConfig
                {
                    CompanyId = Constant.DefaultId,
                    UserName = 1,
                    Email = true,
                    AlternativeEmail = false,
                    Phone = false,
                    Mobile = false,
                    Fax = false,
                    EmergencyPhone = false,
                    Gender = false,
                    Birthday = false,
                    IdentificationCard = false,
                    Nacionality = false,
                    LinkedIn = false,
                    Twitter = false,
                    Instagram = false,
                    Facebook = false,
                    Text1 = string.Empty,
                    Text2 = string.Empty,
                    Text3 = string.Empty,
                    Text4 = string.Empty,
                    Doc1 = string.Empty,
                    Doc2 = string.Empty,
                    Doc3 = string.Empty,
                    Doc4 = string.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public ActionResult Save(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = string.Empty;
            using(var instance = Persistence.InstanceByName(instanceName))
            {
                cns = instance.Config.ConnectionString;
            }

            if (!string.IsNullOrEmpty(cns))
            {
                /* CREATE PROCEDURE Core_ProfileConfig_Save
                 *   @CompanyId bigint,
                 *   @UserName int,
                 *   @Email bit,
                 *   @AlternativeEmail bit,
                 *   @Phone bit,
                 *   @Mobile bit,
                 *   @Fax bit,
                 *   @EmergencyPhone bit,
                 *   @Gender bit,
                 *   @Birthday bit,
                 *   @IdentificationCard bit,
                 *   @Nacionality bit,
                 *   @LinkedIn bit,
                 *   @Twitter bit,
                 *   @Instagram bit,
                 *   @Facebook bit,
                 *   @Text1 nvarchar(50),
                 *   @Text2 nvarchar(50),
                 *   @Text3 nvarchar(50),
                 *   @Text4 nvarchar(50),
                 *   @Doc1 nvarchar(50),
                 *   @Doc2 nvarchar(50),
                 *   @Doc3 nvarchar(50),
                 *   @Doc4 nvarchar(50),
                 *   @ApplicationUserId bigint */
                using (var cmd = new SqlCommand("Core_ProfileConfig_Save"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@UserName", this.UserName));
                        cmd.Parameters.Add(DataParameter.Input("@Email", this.Email));
                        cmd.Parameters.Add(DataParameter.Input("@AlternativeEmail", this.AlternativeEmail));
                        cmd.Parameters.Add(DataParameter.Input("@Phone", this.Phone));
                        cmd.Parameters.Add(DataParameter.Input("@Mobile", this.Mobile));
                        cmd.Parameters.Add(DataParameter.Input("@Facebook", this.Facebook));
                        cmd.Parameters.Add(DataParameter.Input("@EmergencyPhone", this.EmergencyPhone));
                        cmd.Parameters.Add(DataParameter.Input("@Gender", this.Gender));
                        cmd.Parameters.Add(DataParameter.Input("@Birthday", this.Birthday));
                        cmd.Parameters.Add(DataParameter.Input("@IdentificationCard", this.IdentificationCard));
                        cmd.Parameters.Add(DataParameter.Input("@Nacionality", this.Nacionality));
                        cmd.Parameters.Add(DataParameter.Input("@LinkedIn", this.LinkedIn));
                        cmd.Parameters.Add(DataParameter.Input("@Twitter", this.Twitter));
                        cmd.Parameters.Add(DataParameter.Input("@Instagram", this.Instagram));
                        cmd.Parameters.Add(DataParameter.Input("@Facebook", this.Facebook));
                        cmd.Parameters.Add(DataParameter.Input("@Text1", this.Text1, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Text2", this.Text2, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Text3", this.Text3, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Text4", this.Text4, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Doc1", this.Doc1, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Doc2", this.Doc2, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Doc3", this.Doc3, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Doc4", this.Doc4, 50));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
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

        public static UserProfileConfig ByCompany(long companyId, string instanceName)
        {
            var res = Empty;
            var cns = string.Empty;
            using(var instance = Persistence.InstanceByName(instanceName))
            {
                cns = instance.Config.ConnectionString;
            }

            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_ProfileConfig_ByCompany"))
                {
                    using(var cnn = new SqlConnection(cns))
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
                                    res.CompanyId = companyId;
                                    res.UserName = rdr.GetInt32(1);
                                    res.Email = rdr.GetBoolean(2);
                                    res.AlternativeEmail = rdr.GetBoolean(3);
                                    res.Phone = rdr.GetBoolean(4);
                                    res.Mobile = rdr.GetBoolean(5);
                                    res.Fax = rdr.GetBoolean(6);
                                    res.EmergencyPhone = rdr.GetBoolean(7);
                                    res.Gender = rdr.GetBoolean(8);
                                    res.Birthday = rdr.GetBoolean(9);
                                    res.IdentificationCard = rdr.GetBoolean(10);
                                    res.Nacionality = rdr.GetBoolean(11);
                                    res.LinkedIn = rdr.GetBoolean(12);
                                    res.Twitter = rdr.GetBoolean(13);
                                    res.Instagram = rdr.GetBoolean(14);
                                    res.Facebook = rdr.GetBoolean(15);
                                    res.Text1 = rdr.GetString(16);
                                    res.Text2 = rdr.GetString(17);
                                    res.Text3 = rdr.GetString(18);
                                    res.Text4 = rdr.GetString(19);
                                    res.Doc1 = rdr.GetString(20);
                                    res.Doc2 = rdr.GetString(21);
                                    res.Doc3 = rdr.GetString(22);
                                    res.Doc4 = rdr.GetString(23);
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(24),
                                        Profile = new UserProfile
                                        {
                                            ApplicationUserId = rdr.GetInt64(24),
                                            Name = rdr.GetString(25),
                                            LastName = rdr.GetString(26),
                                            LastName2 = rdr.GetString(27)
                                        }
                                    };
                                    res.CreatedOn = rdr.GetDateTime(28);
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(29),
                                        Profile = new UserProfile
                                        {
                                            ApplicationUserId = rdr.GetInt64(29),
                                            Name = rdr.GetString(30),
                                            LastName = rdr.GetString(31),
                                            LastName2 = rdr.GetString(32)
                                        }
                                    };
                                    res.ModifiedOn = rdr.GetDateTime(33);
                                    res.Active = rdr.GetBoolean(34);
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
