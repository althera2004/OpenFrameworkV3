// --------------------------------
// <copyright file="ApplicationUser.cs" company="OpenFramework">
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
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Xml.Serialization;
    using OpenFramework.Core.Bindings;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Bindings;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.ItemManager;
    using OpenFrameworkV3.Core.Navigation;
    using OpenFrameworkV3.Mail;
    using OpenFrameworkV3.Security;
    using OpenFrameworkV3.Tools;

    public class ApplicationUser
    {
        /// <summary>Scope view of application user</summary>
        private ScopeView scopeView;

        private List<Grant> grants;

        private MenuShortcut shortcuts;

        public ReadOnlyCollection<ScopeView.Data> ScopeView
        {
            get
            {
                if (this.scopeView == null)
                {
                    return new ReadOnlyCollection<ScopeView.Data>(new List<ScopeView.Data>());
                }

                return new ReadOnlyCollection<ScopeView.Data>(this.scopeView.All);
            }
        }

        public ReadOnlyCollection<ScopeView.Data> ScopeViewByItemDefinition(long itemDefinitionId)
        {
            if (this.scopeView == null)
            {
                return new ReadOnlyCollection<ScopeView.Data>(new List<ScopeView.Data>());
            }

            return this.scopeView.ByItemDefenitionId(itemDefinitionId);
        }

        public static ApplicationUser ChoseByGroup(long groupId, long companyId, string instanceName)
        {
            // weke
            return Empty;
        }

        /// <summary>Gets or sets application user identifier</summary>
        [XmlElement(Type = typeof(long), ElementName = "Id")]
        public long Id { get; set; }

        /// <summary>Gets or sets user profile</summary>
        [XmlElement(Type = typeof(UserProfile), ElementName = "Profile")]
        public UserProfile Profile { get; set; }

        /// <summary>Gets or sets user that creates user</summary>
        [XmlElement(Type = typeof(ApplicationUser), ElementName = "CreatedBy")]
        public ApplicationUser CreatedBy { get; set; }

        /// <summary>Gets or sets user that modifies user</summary>
        [XmlElement(Type = typeof(ApplicationUser), ElementName = "ModifiedBy")]
        public ApplicationUser ModifiedBy { get; set; }

        /// <summary>Gets or sets user date creation</summary>
        [XmlElement(Type = typeof(DateTime), ElementName = "CreatedOn")]
        public DateTime CreatedOn { get; set; }

        /// <summary>Gets or sets user date modification</summary>
        [XmlElement(Type = typeof(DateTime), ElementName = "ModifiedOn")]
        public DateTime ModifiedOn { get; set; }

        /// <summary>Gets or sets a value indicating whether if user is active</summary>
        [XmlElement(Type = typeof(bool), ElementName = "Active")]
        public bool Active { get; set; }

        /// <summary>Gets or sets a value indicating whether if user show help in interface</summary>
        [XmlElement(Type = typeof(bool), ElementName = "ShowHelp")]
        public bool ShowHelp { get; set; }

        /// <summary>Gets or sets a value indicating whether if user is core</summary>
        [XmlElement(Type = typeof(bool), ElementName = "Core")]
        public bool Core { get; set; }

        /// <summary>Gets or sets an encoded password</summary>
        [XmlElement(Type = typeof(string), ElementName = "Password")]
        public string Password { get; set; }

        /// <summary>Gets or sets a value indicating whether if user is administrator user</summary>
        [XmlElement(Type = typeof(bool), ElementName = "AdminUser")]
        public bool AdminUser { get; set; }

        /// <summary>Gets or sets a value indicating whether if user is corporative</summary>
        [XmlElement(Type = typeof(bool), ElementName = "Corporative")]
        public bool Corporative { get; set; }

        /// <summary>Gets or sets a value indicating whether if user is corporative</summary>
        [XmlElement(Type = typeof(bool), ElementName = "External")]
        public bool External { get; set; }

        /// <summary>Gets or sets the language of user</summary>
        [XmlElement(Type = typeof(Language), ElementName = "Language")]
        public Language Language { get; set; }

        private List<SecurityGroup> groups;

        public ReadOnlyCollection<SecurityGroup> Groups
        {
            get
            {
                if (this.groups == null)
                {
                    this.groups = new List<SecurityGroup>();
                }

                return new ReadOnlyCollection<SecurityGroup>(this.groups);
            }
        }

        public ReadOnlyCollection<Grant> Grants
        {
            get
            {
                if (this.grants == null)
                {
                    this.grants = new List<Grant>();
                }

                return new ReadOnlyCollection<Grant>(this.grants);
            }
        }

        public bool HasGrantToRead(long itemDefinitionId)
        {
            // weke
            return true;
        }

        public void AddGroup(long groupId)
        {
            if (this.groups == null)
            {
                this.groups =
                    new List<SecurityGroup>();
            }

            if (!this.groups.Any(g => g.Id == groupId))
            {
                this.groups.Add(new SecurityGroup { Id = groupId });
            }
        }

        public string ScopeViewIdsSqlWhereData(long itemDefinitionId)
        {
            /*var ids = ScopeViewIds(itemDefinitionId);
            if(ids.Count == 0)
            {
                return string.Empty;
            }*/

            var ids = new List<long>();

            var res = new StringBuilder();
            bool first = true;
            foreach (long id in ids)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(id);
            }

            return res.ToString();
        }

        public static ApplicationUser Empty
        {
            get
            {
                return new ApplicationUser
                {
                    Id = Constant.DefaultId,
                    Email = string.Empty,
                    Profile = UserProfile.Empty,
                    Core = false,
                    Active = false
                };
            }
        }

        public static ApplicationUser Actual
        {
            get
            {
                if (HttpContext.Current.Session["ApplicationUser"] != null)
                {
                    return HttpContext.Current.Session["ApplicationUser"] as ApplicationUser;
                }

                return Empty;
            }
        }

        public static ApplicationUser OpenFramework
        {
            get
            {
                return new ApplicationUser
                {
                    Id = 1,
                    Profile = new UserProfile
                    {
                        ApplicationUserId = 1,
                        Name = "Open",
                        LastName = "Framework",
                        LastName2 = string.Empty
                    },
                    Core = true,
                    AdminUser = true,
                    Corporative = false,
                    ShowHelp = false
                };
            }
        }

        public string JsonSimple
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"{{""Id"":{0},""Profile"":{1}}}", this.Id, this.Profile.JsonSimple);
            }
        }

        /// <summary>Gets a JSON structure compossed by key and value of application user</summary>
        public string JsonKeyValue
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"{{""Id"":{0},""Value"":""{1}"",""Email"":""{2}""}}", this.Id, Tools.Json.JsonCompliant(this.Profile.FullName), this.Email);
            }
        }

        public string Json
        {
            get
            {
                if (this.grants == null)
                {
                    this.grants = new List<Grant>();
                }

                var profileJson = (this.Profile ?? UserProfile.Empty).Json;

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"":{0},
                        ""Email"":""{1}"",
                        ""Password"":""{2}"",
                        ""Core"":{3},
                        ""AdminUser"":{4},
                        ""Profile"":{5},
                        ""Grants"":{6},
                        ""Groups"":{7},
                        ""Language"": {8}
                    }}",
                    this.Id,
                    (this.Email ?? string.Empty).ToLowerInvariant(),
                    "*******************",
                    ConstantValue.Value(this.Core),
                    ConstantValue.Value(this.AdminUser),
                    profileJson,
                    Grant.JsonList(new ReadOnlyCollection<Grant>(this.Grants)),
                    SecurityGroup.JsonListSimple(new ReadOnlyCollection<SecurityGroup>(this.Groups)),
                    this.Language.Json);
            }
        }

        public long CompanyId { get; set; }

        public string Email { get; set; }

        public void GetShortcuts(long applicationUserId, long companyId, string language, string instanceName)
        {
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                /* CREATE PROCEDURE [dbo].[Core_ShortCuts_ByUser] 
                 *   @ApplicationUserId bigint,
                 *   @CompanyId bigint */
                using (var cmd = new SqlCommand("Core_ShortCuts_ByUser"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();

                                    if (!rdr.IsDBNull(0))
                                    {
                                        var itemBlue = Persistence.ItemDefinitionByName(rdr.GetString(0), instanceName);
                                        this.shortcuts.Blue = new Shortcut
                                        {
                                            Label = ApplicationDictionary.Translate(itemBlue.Layout.LabelPlural, language, instanceName),
                                            Icon = itemBlue.Layout.Icon,
                                            Link = rdr.GetString(1),
                                            ShortcutType = ShortcutTypes.Blue
                                        };
                                    }

                                    if (!rdr.IsDBNull(2))
                                    {
                                        var itemRed = Persistence.ItemDefinitionByName(rdr.GetString(2), instanceName);
                                        this.shortcuts.Red = new Shortcut
                                        {
                                            Label = ApplicationDictionary.Translate(itemRed.Layout.LabelPlural, language, instanceName),
                                            Icon = itemRed.Layout.Icon,
                                            Link = rdr.GetString(3),
                                            ShortcutType = ShortcutTypes.Red
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
            }
        }

        public static ApplicationUser ById(long applicationUserId, string instanceName)
        {
            return ById(applicationUserId, Constant.DefaultId, instanceName);
        }

        public static ApplicationUser ById(long applicationUserId, long companyId, string instanceName)
        {
            var res = ApplicationUser.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_User_GetById"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    res.Id = rdr.GetInt64(ColumnsUserGet.Id);
                                    res.Language = new Language
                                    {
                                        Id = rdr.GetInt64(ColumnsUserGet.LanguageId),
                                        Iso = rdr.GetString(ColumnsUserGet.LanguageISO),
                                        Name = rdr.GetString(ColumnsUserGet.LanguageName)
                                    };

                                    //res.Profile = UserProfile.ByApplicationUserId(res.Id, companyId, instanceName);
                                    res.Profile = new UserProfile
                                    {
                                        ApplicationUserId = rdr.GetInt64(ColumnsUserGet.Id),
                                        Name = rdr.GetString(ColumnsUserGet.Name),
                                        LastName = rdr.GetString(ColumnsUserGet.LastName),
                                        LastName2 = rdr.GetString(ColumnsUserGet.LastName2),
                                        EmailAlternative = rdr.GetString(ColumnsUserGet.EmailAlternative),
                                        Phone = rdr.GetString(ColumnsUserGet.Phone),
                                        Mobile = rdr.GetString(ColumnsUserGet.Mobile),
                                        PhoneEmergency = rdr.GetString(ColumnsUserGet.PhoneEmergency),
                                        Fax = rdr.GetString(ColumnsUserGet.Fax)
                                    };

                                    res.AdminUser = rdr.GetBoolean(ColumnsUserGet.AdminUser);
                                    res.Core = rdr.GetBoolean(ColumnsUserGet.Core);
                                    res.Corporative = rdr.GetBoolean(ColumnsUserGet.Coporative);
                                    res.Email = rdr.GetString(ColumnsUserGet.Email);

                                    res.Password = Tools.Basics.Base64Encode(rdr.GetString(ColumnsUserGet.Password));

                                    res.Active = rdr.GetBoolean(ColumnsUserGet.Active);

                                    if (res.AdminUser)
                                    {
                                        // Si es adminUser tiene acceso a todo
                                        res.grants = new List<Grant>();
                                        foreach(var item in Persistence.ItemDefinitions(instanceName))
                                        {
                                            res.grants.Add(new Grant
                                            {
                                                ApplicationUserId = res.Id,
                                                CompanyId = companyId,
                                                Grants = "RWD",
                                                ItemId = item.Id,
                                                ItemName = item.ItemName,
                                                SecurityGroupId = Constant.DefaultId
                                            });
                                        }
                                    }
                                    else
                                    {
                                        res.grants = Grant.ByUser(res.Id, companyId, instanceName).ToList();
                                    }

                                    res.groups = SecurityGroup.ByUserId(res.Id, companyId, instanceName).ToList();

                                    // TODO: shorcuts
                                    //if (companyId != Constant.DefaultId)
                                    //{
                                    //    res.GetShortcuts(applicationUserId, companyId, res.Language.Iso, instanceName);
                                    //}

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.Trace(ex, "ApplicationUSer.ById(" + applicationUserId.ToString() + ")");
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

        public static ReadOnlyCollection<ApplicationUser> All(long companyId, string instanceName)
        {
            var res = new List<ApplicationUser>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_User_GetAllForList"))
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
                                while (rdr.Read())
                                {
                                    var newUser = new ApplicationUser
                                    {

                                        Id = rdr.GetInt64(ColumnsUserGetForList.Id),
                                        Language = new Language
                                        {
                                            Id = rdr.GetInt64(ColumnsUserGetForList.LanguageId),
                                            Iso = rdr.GetString(ColumnsUserGetForList.LanguageISO),
                                            Name = rdr.GetString(ColumnsUserGetForList.LanguageName)
                                        },
                                        Profile = new UserProfile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsUserGetForList.Id),
                                            Name = rdr.GetString(ColumnsUserGetForList.FirstName),
                                            LastName = rdr.GetString(ColumnsUserGetForList.LastName),
                                            LastName2 = rdr.GetString(ColumnsUserGetForList.LastName2)
                                        },
                                        AdminUser = rdr.GetBoolean(ColumnsUserGetForList.AdminUser),
                                        Corporative = rdr.GetBoolean(ColumnsUserGetForList.Corporative),
                                        Core = rdr.GetBoolean(ColumnsUserGetForList.Core),
                                        Email = rdr.GetString(ColumnsUserGetForList.Email),
                                        Active = rdr.GetBoolean(ColumnsUserGetForList.Active)
                                    };

                                    res.Add(newUser);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.Trace(ex, "ApplicationUSer.All(" + companyId.ToString() + ")");
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

            return new ReadOnlyCollection<ApplicationUser>(res);
        }

        public static ReadOnlyCollection<long> CompanyMemeberShip(long applicationUserId, string instanceName)
        {
            var res = new List<long>();
            var cns = string.Empty;
            using(var instance = Persistence.InstanceByName(instanceName))
            {
                cns = instance.Config.ConnectionString;
            }

            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyMemberShip_ByUser"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(rdr.GetInt64(0));
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

            return new ReadOnlyCollection<long>(res);
        }

        public static ActionResult MaintainSession(long applicationUserId, string password,long companyId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Security_MaintainSession"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        var sendPass = Encrypt.EncryptString(Basics.Reverse(password), "OpenFramework");
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@Password", sendPass, 150));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    res.SetSuccess();
                                }
                            }                            
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

        public static ActionResult LogOn(string userName, string password, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Security_LogOn"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        var sendPass = Encrypt.EncryptString(Basics.Reverse(password), "OpenFramework");
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@UserName", userName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Password", sendPass, 150));
                        cmd.Parameters.Add(DataParameter.OutputInt("@Result"));
                        cmd.Parameters.Add(DataParameter.OutputBool("@Locked"));
                        cmd.Parameters.Add(DataParameter.OutputBool("@Corporative"));
                        cmd.Parameters.Add(DataParameter.OutputBool("@Multicompany"));
                        cmd.Parameters.Add(DataParameter.OutputLong("@CompanyId"));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(string.Format(
                                CultureInfo.InvariantCulture,
                                @"{{""Id"": {0}, ""Locked"":{1}, ""Corporative"":{2}, ""Multicompany"":{2}, ""CompanyId"":{4}}}",
                                cmd.Parameters["@Result"].Value,
                                ConstantValue.Value(Convert.ToBoolean(cmd.Parameters["@Locked"].Value)),
                                ConstantValue.Value(Convert.ToBoolean(cmd.Parameters["@Corporative"].Value)),
                                ConstantValue.Value(Convert.ToBoolean(cmd.Parameters["@Multicompany"].Value)),
                                cmd.Parameters["@CompanyId"].Value));
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

        public ActionResult Save (long applicationUserId, long companyId, string instanceName)
        {
            if(this.Id == Constant.DefaultId)
            {
                return Insert(applicationUserId, companyId, instanceName);
            }
            else
            {
                return Update(applicationUserId, companyId, instanceName);
            }
        }

        private ActionResult Insert(long applicationUserId, long companyId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_User_Insert
             *   PROCEDURE Core_User_Insert
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @Email nvarchar(100),
             *   @Password nvarchar(50),
             *   @Pass  nvarchar(150),
             *   @Language bigint,
             *   @ApplicationUserId bigint,
             *   @Name varchar(50),
             *   @LastName nvarchar(50),
             *   @LastName2 nvarchar(50),
             *   @Phone nvarchar(20),
             *   @Mobile nvarchar(20),
             *   @PhoneEmergency nvarchar(20),
             *   @Fax nvarchar(20),
             *   @EmailAlternative nvarchar(100),
             *   @Twitter nvarchar(50),
             *   @LinkedIn nvarchar(50),
             *   @Instagram nvarchar(50),
             *   @Facebook nvarchar(50),
             *   @Gender int,
             *   @IdentificationCard nvarchar(15),
             *   @IMEI nvarchar(20),
             *   @BirthDate datetime,
             *   @AddressWayType int,
             *   @Address nvarchar(100),
             *   @PostalCode nvarchar(10),
             *   @City nvarchar(50),
             *   @Province nvarchar(50),
             *   @State nvarchar(50),
             *   @Country nvarchar(50),
             *   @Latitude decimal(18,8),
             *   @Longitude decimal(18,8),
             *   @Web  nvarchar(300) */

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_User_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        var sendPass = Encrypt.EncryptString(Basics.Reverse("pass"), "OpenFramework");
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@Email", this.Email, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Password", "pass", 50));
                        cmd.Parameters.Add(DataParameter.Input("@Pass", sendPass, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Language", this.Language.Id));

                        cmd.Parameters.Add(DataParameter.Input("@FirstName", this.Profile.Name, 50));
                        cmd.Parameters.Add(DataParameter.Input("@LastName", this.Profile.LastName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@LastName2", this.Profile.LastName2, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Phone", this.Profile.Phone, 20));
                        cmd.Parameters.Add(DataParameter.Input("@Mobile", this.Profile.Mobile, 20));
                        cmd.Parameters.Add(DataParameter.Input("@PhoneEmergency", this.Profile.PhoneEmergency, 20));
                        cmd.Parameters.Add(DataParameter.Input("@Fax", this.Profile.Fax, 20));
                        cmd.Parameters.Add(DataParameter.Input("@EmailAlternative", this.Profile.EmailAlternative, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Gender", this.Profile.Gender));
                        cmd.Parameters.Add(DataParameter.Input("@IdentificationCard", this.Profile.IdentificationCard, 15));

                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(string.Format(CultureInfo.InvariantCulture, "OK|{0}", this.Id));
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

        private ActionResult Update(long applicationUserId, long companyId, string instanceName)
        {
            var res = ActionResult.NoAction;
            return res;
        }

        public static ActionResult RecoverPassword(string userEmail, string instanceName, string ip)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                var applicationUser = ApplicationUser.ByEmail(userEmail, instanceName);

                using (var cmd = new SqlCommand("Core_User_RecoverPassword"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        var newPass = RandomPassword.Generate(6, 6);
                        var newPassDB = Encrypt.EncryptString(Basics.Reverse(newPass), "OpenFramework");

                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUser.Id));
                        cmd.Parameters.Add(DataParameter.Input("@NewPassword", newPassDB, 50));

                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res = MailManager.SendPaswordRecovery(applicationUser.Id, newPass, instanceName);
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

        public static ApplicationUser ByEmail(string email, string instanceName)
        {
            var res = ApplicationUser.Empty;
            var cns = string.Empty;
            using(var instance = Persistence.InstanceByName(instanceName))
            {
                cns = instance.Config.ConnectionString;
            }

            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_ApplicationUser_ByEmail"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserEmail", email, 100));
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    var applicationUserId = rdr.GetInt64(0);
                                    res = ApplicationUser.ById(applicationUserId, instanceName);
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

        public static ActionResult ChangePassword(long applicationUserId, string actual, string newPass, string instanceName, string ip)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Core_User_ChangePassword"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        var realActual = Encrypt.EncryptString(Basics.Reverse(actual), "OpenFramework");
                        var realNewPass = Encrypt.EncryptString(Basics.Reverse(newPass), "OpenFramework");

                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@Actual", realActual, 50));
                        cmd.Parameters.Add(DataParameter.Input("@NewPass", realNewPass, 50));
                        cmd.Parameters.Add(DataParameter.OutputInt("@Result"));

                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(cmd.Parameters["@Result"].Value);
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