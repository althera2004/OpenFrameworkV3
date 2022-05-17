// --------------------------------
// <copyright file="Config.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Script.Serialization;
    using Newtonsoft.Json;
    using OpenFrameworkV3;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Enums;
    using OpenFrameworkV3.Core.InstanceManager;
    using OpenFrameworkV3.Tools;

    public sealed class Config
    {
        public string JsonData
        {
            get
            {
                var res = Json.EmptyJsonObject;
                var path = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}Instances\{1}\Instance.cfx",
                HttpContext.Current.Request.PhysicalApplicationPath,
                this.Name).Replace("\\\\", "\\");

                if(File.Exists(path))
                {
                    using(var input = new StreamReader(path))
                    {
                        res = input.ReadToEnd();
                    }
                }

                return res;
            }
        }

        /// <summary>Billing configuration</summary>
        [JsonProperty("Billing")]
        private BillingConfig billing;

        /// <summary>Security confirguration</summary>
        [JsonProperty("Security")]
        private SecurityConfiguration security;

        /// <summary>Features confirguration</summary>
        [JsonProperty("Features")]
        private Features features;

        /// <summary>Profile configuration</summary>
        [JsonProperty("Profile")]
        private ProfileConfiguration profile;

        /// <summary>Profile configuration</summary>
        [JsonProperty("CompanyProfile")]
        private CompanyProfileConfig companyProfile;

        /// <summary>Global features configuration</summary>
        [JsonProperty("GlobalFeatures")]
        private GlobalFeatures globalFeatures;

        /// <summary>Session timeout</summary>
        [JsonProperty("SessionTimeout")]
        private string sessionTimeout;

        /// <summary>Default language of instance</summary>
        [JsonProperty("DefaultLanguage")]
        private string defaultLanguage;

        /// <summary>Action of delete</summary>
        [JsonProperty("DeleteAction")]
        private string deleteAction;

        /// <summary>Action of save</summary>
        [JsonProperty("SaveAction")]
        private string saveAction;

        /// <summary>Number of list rows</summary>
        [JsonProperty("ListNumRows")]
        private int listNumRows;

        [JsonProperty("ExternalUsers")]
        private string[] externalUsers;

        [JsonProperty("Alerts")]
        private AlertMenuConfiguration[] alerts;

        /// <summary>Gets an empty customer config</summary>
        [JsonIgnore]
        public static Config Empty
        {
            get
            {
                return new Config
                {
                    Id = -1,
                    Name = string.Empty,
                    Description = string.Empty,
                    ConnectionString = string.Empty,
                    security = SecurityConfiguration.Empty,
                    profile = ProfileConfiguration.Empty,
                    companyProfile = CompanyProfileConfig.Empty,
                    globalFeatures = GlobalFeatures.Empty,
                    GoogleAPIKey = string.Empty,
                    billing = BillingConfig.Empty
                };
            }
        }

        /// <summary>Indicates if instance has multiple company configuration</summary>
        [JsonProperty("MultiCompany")]
        public bool MultiCompany { get; set; }

        /// <summary>Indicates if instance has MQTT protocol enabled</summary>
        [JsonProperty("MQTT")]
        public bool MQTT { get; set; }

        /// <summary>Indicates if instance has Facturacion protocol enabled</summary>
        [JsonProperty("Facturacion")]
        public bool Facturacion { get; set; }

        /// <summary> Gets or sets a value indicating whether if instance is in debug mode</summary>
        [JsonProperty("DebugMode")]
        public bool DebugMode { get; set; }

        /// <summary> Gets or sets a value indicating whether if public access is enabled</summary>
        [JsonProperty("PublicAccess")]
        public bool PublicAccess { get; set; }

        /// <summary> Gets or sets a value indicating whether if admin areas needs PinCode</summary>
        [JsonProperty("PinCode")]
        public bool PinCode { get; set; }

        /// <summary> Gets or sets a value indicating whether if following is enabled</summary>
        [JsonProperty("FollowEnabled")]
        public bool FollowEnabled { get; set; }

        /// <summary> Gets or sets a value indicating traces type if enabled</summary>
        [JsonProperty("TraceType")]
        public int TraceType { get; set; }

        /// <summary> Gets or sets a value indicating whether if FAQs are enabled</summary>
        [JsonProperty("FAQs")]
        public bool FAQs { get; set; }

        /// <summary> Gets or sets a value indicating whether if portal is enabled</summary>
        [JsonProperty("Portal")]
        public bool Portal { get; set; }

        /// <summary> Gets or sets a value indicating whether if scope view is enabled</summary>
        [JsonProperty("ScopeViewEnabled")]
        public bool ScopeViewEnabled { get; set; }

        /// <summary> Gets or sets a value indicating whether if "waht you missed" is enabled</summary>
        [JsonProperty("ActiveAlerts")]
        public bool ActiveAlerts { get; set; }

        /// <summary> Gets or sets a value indicating whether if scope view is enabled</summary>
        [JsonProperty("WhatYouMissed")]
        public bool WhatYouMissed { get; set; }

        /// <summary> Gets or sets a value indicating whether if messaging is enabled</summary>
        [JsonProperty("Messaging")]
        public bool Messaging { get; set; }

        /// <summary> Gets or sets a value indicating whether if use of sticks is enabled</summary>
        [JsonProperty("Sticking")]
        public bool Sticking { get; set; }

        /// <summary> Gets or sets a value indicating whether if schelude is enabled</summary>
        [JsonProperty("UserSchedule")]
        public bool UserSchedule { get; set; }

        /// <summary>Gets or sets key of Google maps API</summary>
        [JsonProperty("GoogleAPIKey")]
        public string GoogleAPIKey { get; set; }

        /// <summary>Gets or sets url of instance site</summary>
        [JsonProperty("UrlSite")]
        public string UrlSite { get; set; }

        /// <summary>Gets or sets the connection string</summary>
        [JsonProperty("ConnectionString")]
        public string ConnectionString { get; set; }

        /// <summary>Gets or sets the item that decides company membership</summary>
        [JsonProperty("MultiCompanyItem")]
        public string MultipleCompanyItem { get; set; }

        /// <summary>Gets or sets if coded query is clean</summary>
        [JsonProperty("CodedQueryClean")]
        public bool CodedQueryClean { get; set; }

        /// <summary>Gets or sets the customer identifier</summary>
        [JsonProperty("Id")]
        public long Id { get; set; }

        /// <summary>Gets number of list rows</summary>
        [JsonIgnore]
        public int ListNumRows
        {
            get
            {
                if(this.listNumRows == 0)
                {
                    return 10;
                }

                return this.listNumRows;
            }
        }

        /// <summary>Gets the action in database for delete</summary>
        [JsonIgnore]
        public DeleteAction DeleteAction
        {
            get
            {
                if(string.IsNullOrEmpty(this.deleteAction))
                {
                    return DeleteAction.Inactive;
                }

                switch(this.deleteAction.ToUpperInvariant())
                {
                    case "DELETE":
                        return DeleteAction.Delete;
                    case "INACTIVE":
                    default:
                        return DeleteAction.Inactive;
                }
            }
        }

        /// <summary>Gets behavior for save button</summary>
        [JsonIgnore]
        public SaveAction SaveAction
        {
            get
            {
                if(string.IsNullOrEmpty(this.saveAction))
                {
                    return SaveAction.SaveAndClose;
                }

                switch(this.saveAction.ToUpperInvariant())
                {
                    case "SAVEONLY":
                        return SaveAction.SaveOnly;
                    case "BOTH":
                        return SaveAction.Both;
                    case "SAVEANDNEW":
                        return SaveAction.SaveAndNew;
                    case "SAVEANDCLOSE":
                    default:
                        return SaveAction.SaveAndClose;
                }
            }
        }

        /// <summary>Gets mail configuration of instance</summary>
        [JsonIgnore]
        public BillingConfig Billing
        {
            get
            {
                if(this.billing == null)
                {
                    return BillingConfig.Empty;
                }
                else
                {
                    this.billing.Active = true;
                }

                return this.billing;
            }
        }

        /// <summary>Gets security configuration of instance</summary>
        [JsonIgnore]
        public SecurityConfiguration Security
        {
            get
            {
                if(this.security == null)
                {
                    return SecurityConfiguration.Empty;
                }

                return this.security;
            }
        }

        /// <summary>Gets features configuration of instance</summary>
        [JsonIgnore]
        public Features Features
        {
            get
            {
                if(this.features == null)
                {
                    return Features.Empty;
                }

                return this.features;
            }
        }

        /// <summary>Gets profile configuration of instance</summary>
        [JsonIgnore]
        public ProfileConfiguration Profile
        {
            get
            {
                if(this.profile == null)
                {
                    return ProfileConfiguration.Empty;
                }

                return this.profile;
            }
        }

        /// <summary>Gets profile configuration of instance</summary>
        [JsonIgnore]
        public CompanyProfileConfig CompanyProfile
        {
            get
            {
                if(this.companyProfile == null)
                {
                    return CompanyProfileConfig.Empty;
                }

                return this.companyProfile;
            }
        }

        /// <summary>Gets global features configuration of instance</summary>
        [JsonIgnore]
        public GlobalFeatures GlobalFeatures
        {
            get
            {
                if(this.globalFeatures == null)
                {
                    return GlobalFeatures.Empty;
                }

                return this.globalFeatures;
            }
        }

        /// <summary>Gets or sets the customer name</summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the customer description</summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonIgnore]
        public ReadOnlyCollection<string> ExternalUsers
        {
            get
            {
                if(this.externalUsers == null)
                {
                    return new ReadOnlyCollection<string>(new List<string>());
                }

                return new ReadOnlyCollection<string>(this.externalUsers);
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<AlertMenuConfiguration> Alerts
        {
            get
            {
                if(this.alerts == null)
                {
                    return new ReadOnlyCollection<AlertMenuConfiguration>(new List<AlertMenuConfiguration>());
                }

                return new ReadOnlyCollection<AlertMenuConfiguration>(this.alerts);
            }
        }

        public void AddAlert(DynamicJsonObject alert)
        {
            if(this.alerts == null)
            {
                this.alerts = new List<AlertMenuConfiguration>().ToArray();
            }

            var temp = this.alerts.ToList();
            temp.Add(AlertMenuConfiguration.FromDynamic(alert));
            this.alerts = temp.ToArray();
        }

        [JsonIgnore]
        public string AlertsJson
        {
            get
            {
                if(this.alerts == null)
                {
                    return Json.EmptyJsonObject;
                }

                bool first = true;
                var res = new StringBuilder("[");
                foreach(var alert in this.alerts)
                {
                    if(first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.AppendFormat(CultureInfo.InvariantCulture,
                        @"{{""Menu"":""{0}"", ""Color"":""{1}"", ""Label"":""{2}"", ""Icon"":""{3}""}}",
                        alert.Menu,
                        alert.Color ?? "grey",
                        Tools.Json.JsonCompliant(alert.Label),
                        alert.Icon ?? "fa fa-bell");
                }

                res.Append("]");
                return res.ToString();
            }
        }

        /// <summary>Gets the session timeout</summary>
        [JsonIgnore]
        public int SessionTimeout
        {
            get
            {
                if(string.IsNullOrEmpty(this.sessionTimeout))
                {
                    return Constant.DefaultTimeout;
                }

                int? extractTime = Basics.ParseTime(this.sessionTimeout);
                return extractTime ?? Constant.DefaultTimeout;
            }
        }

        /// <summary>Gets the default language</summary>
        [JsonIgnore]
        public string DefaultLanguage
        {
            get
            {
                if(string.IsNullOrEmpty(this.defaultLanguage))
                {
                    return "es-es";
                }

                return this.defaultLanguage;
            }
        }

        /// <summary>Gets the default language</summary>
        [JsonIgnore]
        public Language DefaultLanguageObject
        {
            get
            {
                /*
                if (HttpContext.Current.Session["Languages"] is ReadOnlyCollection<Language> languages && languages.Count > 0)
                {
                    if (languages.Any(l => l.Iso.Equals(this.DefaultLanguage, StringComparison.OrdinalIgnoreCase)))
                    {
                        return languages.First(l => l.Iso.Equals(this.DefaultLanguage, StringComparison.OrdinalIgnoreCase));
                    }
                }*/

                return new Language
                {
                    Id = 1,
                    Iso = "es",
                    Name = "Español",
                    LocaleName = "Español",
                    Active = true
                };
            }
        }

        /// <summary>Load customer framework configuration</summary>
        /// <param name="instanceName">Customer name</param>
        /// <returns>Customer framework configuration</returns>
        public static Config Load(string instanceName)
        {
            if(string.IsNullOrEmpty(instanceName))
            {
                return Config.Empty;
            }

            string source = string.Format(CultureInfo.InvariantCulture, @"OpenFramework.Core.Config.Load(""{0}"")", instanceName);
            var path = string.Empty;

            // Executing on IIS
            if(HttpContext.Current != null)
            {
                path = HttpContext.Current.Request.PhysicalApplicationPath;
            }

            return Load(instanceName, path);
        }

        /// <summary>Load customer framework configuration</summary>
        /// <param name="instanceName">Customer name</param>
        /// <param name="path">Path of item definitions</param>
        /// <returns>Customer framework configuration</returns>
        public static Config Load(string instanceName, string path)
        {
            if(string.IsNullOrEmpty(instanceName))
            {
                return Config.Empty;
            }

            string source = string.Format(CultureInfo.InvariantCulture, @"OpenFramework.Core.Config.Load(""{0}"")", instanceName);
            var res = Config.Empty;

            if(!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}Instances\{1}\Instance.cfx", path, instanceName);
            return LoadFromFile(path);
        }

        public static Config LoadFromFile(string path)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"OpenFramework.Core.Config.LoadFromFile(""{0}"")", path);
            var res = Config.Empty;
            try
            {
                if(File.Exists(path))
                {
                    using(var input = new StreamReader(path))
                    {
                        var json = input.ReadToEnd();
                        var serializer = new JavaScriptSerializer();
                        serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                        dynamic data = serializer.Deserialize(json, typeof(object));
                        res = new Config
                        {
                            Id = data["Id"],
                            ConnectionString = data["ConnectionString"],
                            Name = data["Name"],
                            deleteAction = data["DeleteAction"],
                            sessionTimeout = data["SessionTimeout"],
                            MultiCompany = data["MultiCompany"] ?? false,
                            MultipleCompanyItem = data["MultiCompanyItem"],
                            MQTT = data["MQTT"] ?? false,
                            Facturacion = data["Facturacion"] ?? false,
                            defaultLanguage = data["DefaultLanguage"],
                            GoogleAPIKey = data["GoogleAPIKey"],
                            security = SecurityConfiguration.Empty,
                            profile = ProfileConfiguration.Empty,
                            FollowEnabled = data["FollowEnabled"] ?? false,
                            FAQs = data["FAQs"] ?? false,
                            Portal = data["Portal"] ?? false,
                            PinCode = data["PinCode"] ?? false,
                            TraceType = data["TraceType"] ?? 0,
                            ActiveAlerts = data["ActiveAlerts"] ?? false,
                            UserSchedule = data["UserSchedule"] ?? false,
                            DebugMode = data["DebugMode"] ?? false,
                            UrlSite = data["UrlSite"] ?? string.Empty,
                            PublicAccess = data["PublicAccess"] ?? false,
                            WhatYouMissed = data["WhatYouMissed"] ?? false,
                            Messaging = data["Messaging"] ?? false,
                            Sticking = data["Sticking"] ?? false,
                            CodedQueryClean = data["CodedQueryClean"] ?? false

                        };

                        if(data["ExternalUsers"] != null)
                        {
                            var temp = new List<string>();
                            foreach(var user in data["ExternalUsers"])
                            {
                                temp.Add(user as string);
                            }

                            res.externalUsers = temp.ToArray();
                        }

                        if(data["Alerts"] != null)
                        {
                            foreach(var alert in data["Alerts"])
                            {
                                res.AddAlert(alert);
                            }
                        }

                        if(data["Description"] != null)
                        {
                            res.Description = data["Description"];
                        }
                        else
                        {
                            res.Description = res.Name;
                        }

                        if(data["ListNumRows"] != null)
                        {
                            res.listNumRows = data["ListNumRows"];
                        }

                        if(data["Billing"] != null)
                        {
                            res.billing = SetBillingConfiguration(data["Billing"]);
                        }

                        if(data["Security"] != null)
                        {
                            res.security = SetSecurityConfig(data["Security"]);
                        }

                        if(data["Features"] != null)
                        {
                            res.features = SetFeaturesConfig(data["Features"]);
                        }

                        if(data["Profile"] != null)
                        {
                            res.profile = SetConfigProfile(data["Profile"]);
                        }

                        if(data["CompanyProfile"] != null)
                        {
                            res.companyProfile = SetCompanyProfileConfig(data["CompanyProfile"]);
                        }

                        if(data["GlobalFeatures"] != null)
                        {
                            res.globalFeatures = SetGlobalFeaturesConfig(data["GlobalFeatures"]);
                        }

                        //UserConversion.Load(data["Name"]);
                    }
                }
            }
            catch(IOException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
            }
            catch(NullReferenceException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
            }
            catch(JsonSerializationException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
            }
            catch(JsonException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
            }
            catch(NotSupportedException ex)
            {
                ExceptionManager.Trace(ex as Exception, source);
            }

            return res;
        }

        /// <summary>Sets billing configuration</summary>
        /// <param name="data">Data from customer.ctx</param>
        /// <returns>Billing configuration</returns>
        private static BillingConfig SetBillingConfiguration(dynamic data)
        {
            var res = BillingConfig.Empty;
            res.Active = true;
            /*res.Month30 = data["Month30"] ?? true;
            res.MultimpleBankAccount = data["MultimpleBankAccount"] ?? false;
            res.DesgloseIVA = data["DesgloseIVA"] ?? false;
            res.InvoicePattern = data["InvoicePattern"];
            res.CustomInvoiceTemplate = data["CustomInvoiceTemplate"] ?? false;
            res.Items = data["Items"] ?? string.Empty;*/
            return res;
        }

        /// <summary>Sets security configuration</summary>
        /// <param name="data">Data from customer.ctx</param>
        /// <returns>Security configuration</returns>
        private static SecurityConfiguration SetSecurityConfig(dynamic data)
        {
            var res = SecurityConfiguration.Empty;
            /*res.FailedAttempts = data["FailedAttempts"] ?? 3;
            res.MinimumPasswordLength = data["MinimumPasswordLength"] ?? 6;
            res.MustChangePassword = data["MustChangePassword"] ?? false;
            res.GrantPermission = data["GrantPermission"] ?? "ByUser";
            res.ScopeView = data["ScopeView"] ?? "ByUser";
            res.SetPasswordComplexity(data["PasswordComplexity"] ?? "None");
            res.Traceability = data["Traceability"] ?? "None";
            res.WriteAssumeDelete = data["WriteAssumeDelete"] ?? false;
            res.IPAccess = data["IPAccess"] ?? false;
            res.IPAccessMail = data["IPAccessMail"] ?? "info@openframework.es";
            res.MFA = data["MFA"] ?? string.Empty;
            res.GroupUserMain = data["GroupUserMain"] ?? false;*/
            return res;
        }

        /// <summary>Sets features configuration</summary>
        /// <param name="data">Data from customer.ctx</param>
        /// <returns>Features configuration</returns>
        private static Features SetFeaturesConfig(dynamic data)
        {
            var res = Features.Empty;
           /* res.Wacom = data["Wacom"] ?? false;
            res.Firmafy = data["Firmafy"] ?? false;
            res.QR = data["QR"] ?? false;
            res.CreditCard = data["CreditCard"] ?? false;*/
            return res;
        }

        /// <summary>Sets company profile configuration</summary>
        /// <param name="data">Data from customer.ctx</param>
        /// <returns>Security configuration</returns>
        private static CompanyProfileConfig SetCompanyProfileConfig(dynamic data)
        {
            var res = CompanyProfileConfig.Empty;
            /*res.CompanyAddressEnabled = data["CompanyAddressEnabled"] ?? false;
            res.ContactPersonEnabled = data["ContactPersonEnabled"] ?? false;
            res.BankAccountEnabled = data["BankAccountEnabled"] ?? false;
            res.CustomConfig = data["CustomConfig"] ?? false;

            if(data["CustomFields"] != null)
            {
                var fields = data["CustomFields"];
                foreach(var fieldData in fields)
                {
                    res.AddCustomField(new CustomConfig.CustomConfigField
                    {
                        Name = fieldData["Name"],
                        Label = fieldData["Label"] ?? fieldData["Name"],
                        Type = fieldData["Type"] ?? "string",
                        Required = fieldData["Required"] ?? false
                    });
                }
            }*/

            return res;
        }

        /// <summary>Sets company profile configuration</summary>
        /// <param name="data">Data from customer.ctx</param>
        /// <returns>Security configuration</returns>
        private static GlobalFeatures SetGlobalFeaturesConfig(dynamic data)
        {
            var res = GlobalFeatures.Empty;
            //res.CustomListUpdates = data["CustomListUpdates"] ?? false;
            return res;
        }

        /// <summary>Sets user profile configuration</summary>
        /// <param name="data">Data from customer.ctx</param>
        /// <returns>Email configuration</returns>
        private static ProfileConfiguration SetConfigProfile(dynamic data)
        {
            var res = ProfileConfiguration.Empty;
            res.Twitter = data["Twiter"] ?? false;
            res.Facebook = data["Facebook"] ?? false;
            res.Instagram = data["Instagram"] ?? false;
            res.LinkedIn = data["LinkedIn"] ?? false;
            res.Phone = data["Phone"] ?? false;
            res.Mobile = data["Mobile"] ?? false;
            res.Fax = data["Fax"] ?? false;
            res.PhoneEmergency = data["PhoneEmergency"] ?? false;
            res.BirthDate = data["BirthDate"] ?? false;
            res.Address = data["Address"] ?? false;
            res.NameFormat = data["NameFormat"] ?? 0;
            res.Gender = data["Gender"] ?? false;
            res.EmailAlternative = data["EmailAlternative"] ?? false;
            res.Web = data["Web"] ?? false;
            res.Signature = data["Signature"] ?? false;
            res.IdentificationCard = data["IdentificationCard"] ?? false;
            res.Nationality = data["Nationality"] ?? false;
            res.DataText1 = data["DataText1"] ?? string.Empty;
            res.DataText2 = data["DataText2"] ?? string.Empty;
            res.DataText3 = data["DataText3"] ?? string.Empty;
            res.DataText4 = data["DataText4"] ?? string.Empty;
            return res;
        }

        /// <summary>Sets values for user profile configuration</summary>
        /// <param name="config">Profile configuration</param>
        public void SetProfileConfiguration(ProfileConfiguration config)
        {
            this.profile = config;
        }

        /// <summary>Sets values for security config</summary>
        /// <param name="config">Security config</param>
        public void SetSecuritylConfiguration(SecurityConfiguration config)
        {
            this.security = config;
        }
    }
}
