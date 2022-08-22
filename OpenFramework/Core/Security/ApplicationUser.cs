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

        private List<Group> groups;

        public ReadOnlyCollection<Group> Groups
        {
            get
            {
                if (this.groups == null)
                {
                    this.groups = new List<Group>();
                }

                return new ReadOnlyCollection<Group>(this.groups);
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
                    new List<Group>();
            }

            if (!this.groups.Any(g => g.Id == groupId))
            {
                this.groups.Add(new Group { Id = groupId });
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
                        ""Groups"":{7}
                    }}",
                    this.Id,
                    (this.Email ?? string.Empty).ToLowerInvariant(),
                    "*******************",
                    ConstantValue.Value(this.Core),
                    ConstantValue.Value(this.AdminUser),
                    profileJson,
                    Grant.JsonList(new ReadOnlyCollection<Grant>(this.Grants)),
                    Group.JsonListSimple(new ReadOnlyCollection<Group>(this.Groups)));
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

                                    res.Profile = UserProfile.ByApplicationUserId(res.Id, companyId, instanceName);

                                    res.AdminUser = rdr.GetBoolean(ColumnsUserGet.AdminUser);
                                    res.Core = rdr.GetBoolean(ColumnsUserGet.Core);
                                    res.Corporative = rdr.GetBoolean(ColumnsUserGet.Coporative);
                                    res.Email = rdr.GetString(ColumnsUserGet.Email);

                                    res.Password = Tools.Basics.Base64Encode(rdr.GetString(ColumnsUserGet.Password));

                                    res.Active = rdr.GetBoolean(ColumnsUserGet.Active);

                                    res.grants = Grant.ByUser(res.Id, 1, instanceName).ToList();

                                    res.groups = Group.ByUserId(res.Id, companyId, instanceName).ToList();

                                    if (companyId != Constant.DefaultId)
                                    {
                                        res.GetShortcuts(applicationUserId, companyId, res.Language.Iso, instanceName);
                                    }

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

        public ActionResult Insert(long applicationUserId, long companyId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_User_Insert
             *   @Id bigint output,
             *   @Email nvarchar(100),
             *   @Password nvarchar(50),
             *   @Language bigint,
             *   @FirstName nvarchar(50),
             *   @LastName nvarchar(50),
             *   @LastName2 nvarchar(50),
             *   @IMEI nvarchar(20),
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_User_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@Email", this.Email, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Password", "pass", 50));
                        cmd.Parameters.Add(DataParameter.Input("@Language", this.Language.Id));
                        cmd.Parameters.Add(DataParameter.Input("@FirstName", this.Profile.Name, 50));
                        cmd.Parameters.Add(DataParameter.Input("@LastName", this.Profile.LastName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@LastName2", this.Profile.LastName2, 50));
                        cmd.Parameters.Add(DataParameter.Input("@IMEI", this.Profile.IMEI, 20));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
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

        public ActionResult Update(long applicationUserId, long companyId, string instanceName)
        {
            var res = ActionResult.NoAction;
            return res;
        }
    }
}