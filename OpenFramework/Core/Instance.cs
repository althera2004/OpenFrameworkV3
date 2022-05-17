// --------------------------------
// <copyright file="SecurityConfiguration.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Configuration;
    using OpenFrameworkV3.Core.ItemManager;

    public partial class Instance : IDisposable
    {
        // To detect redundant calls
        private bool _disposed;

        ~Instance() => Dispose(false);

        private List<ItemDefinition> definitions;

        public static string InstanceName
        {
            get
            {
                var instanceName = ConfigurationManager.AppSettings["InstanceName"] as string;
                if (HttpContext.Current.Request.UserHostName.ToUpperInvariant().IndexOf("OPENFRAMEWORK") != -1)
                {
                    instanceName = HttpContext.Current.Request.UserHostName.Split('.')[0];
                }
                else
                {
                    if (HttpContext.Current.Request.QueryString["c"] != null)
                    {
                        instanceName = HttpContext.Current.Request.QueryString["c"] as string;
                    }
                }

                return instanceName;
            }
        }

        public static void CheckPersistence()
        {
            //var instanceName = ConfigurationManager.AppSettings["InstanceName"] as string;
            //if(HttpContext.Current.Request.UserHostName.ToUpperInvariant().IndexOf("OPENFRAMEWORK") != -1)
            //{
            //    instanceName = HttpContext.Current.Request.UserHostName.Split('.')[0];
            //}

            var instanceName = InstanceName;

            if (!Persistence.InstanceExists(instanceName))
            {
                var instance = Instance.LoadDefinition(instanceName, true);
                Persistence.AddInstance(instance);
            }
        }

        public ReadOnlyCollection<ItemDefinition> ItemDefinitions
        {
            get
            {
                if (this.definitions == null)
                {
                    this.definitions = new List<ItemDefinition>();
                }

                return new ReadOnlyCollection<ItemDefinition>(this.definitions);
            }
        }

        public static Instance LoadDefinition(string instanceName)
        {
            return LoadDefinition(instanceName, false);
        }

        public static Instance LoadDefinition(string instanceName, bool loadItems)
        {
            var res = Instance.Empty;
            res.Config = Config.Load(instanceName);
            res.Id = res.Config.Id;

            // Only continues if instance exists
            if (res.Id > 0)
            {
                res.Name = res.Config.Name.ToUpperInvariant();
                res.Description = res.Config.Description;
                // TODO: cargas items, alerts, etc...
                if (loadItems)
                {
                    res.LoadItemDefinitions();
                }
            }

            return res;
        }

        public static string JsonDefinitions(string instanceName)
        {
            if (string.IsNullOrEmpty(instanceName))
            {
                return Tools.Json.EmptyJsonList;
            }

            var instance = Persistence.InstanceByName(instanceName);

            if (instance.Id > 0)
            {
                var res = new StringBuilder("[");
                var first = true;
                foreach (var definition in instance.ItemDefinitions)
                {
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"{1}{0}",
                        definition.Json,
                        first ? string.Empty : ",");

                    first = false;
                }

                res.Append("]");
                return res.ToString();
            }
            else
            {
                return Tools.Json.EmptyJsonList;
            }
        }

        public static ReadOnlyCollection<string> WellcomeBackground(string instanceName)
        {
            var res = new List<string>();
            var path = HttpContext.Current.Request.PhysicalApplicationPath + "\\Instances\\" + instanceName + "\\WelcomeBackgrounds";
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                res.Add(System.IO.Path.GetFileName(file));
            }

            return new ReadOnlyCollection<string>(res);
        }

        public void LoadItemDefinitions()
        {
            var files = Directory.GetFiles(Instance.Path.Definition(this.Name), "*.item");
            this.ClearDefinitions();
            foreach (var file in files)
            {
                var itemDefinition = ItemDefinition.LoadFromFile(file, this.Name);
                this.definitions.Add(itemDefinition);
            }
        }

        public static ActionResult Reload(string instanceName)
        {
            var res = ActionResult.NoAction;
            try
            {
                var instance = LoadDefinition(instanceName);
                instance.ClearDefinitions();
                instance.LoadItemDefinitions();
                Persistence.AddInstance(instance);
                res.SetSuccess(instanceName);
            }
            catch (Exception ex)
            {
                res.SetFail(ex);
            }

            return res;
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            _disposed = true;
        }

        /// <summary>Gets actual instance</summary>
        public static Instance Actual
        {
            get
            {
                var instanceName = ConfigurationManager.AppSettings["InstanceName"] as string;
                if (HttpContext.Current.Request.UserHostName.ToUpperInvariant().IndexOf("OPENFRAMEWORK") != -1)
                {
                    instanceName = HttpContext.Current.Request.UserHostName.Split('.')[0];
                }

                if (!Persistence.InstanceExists(instanceName))
                {
                    var instance = Instance.LoadDefinition(instanceName, true);
                    Persistence.AddInstance(instance);
                }

                return Persistence.InstanceByName(instanceName);
            }
        }

        /// <summary>Gets an empty instance of customer framework</summary>
        public static Instance Empty
        {
            get
            {
                return new Instance
                {
                    Id = Constant.DefaultId,
                    Name = string.Empty,
                    Config = Config.Empty
                };
            }
        }
        
        public string Json
        {
            get
            {
                var res = string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
    ""ConnectionString"": ""{2}"",
    ""Id"": {0},
    ""Name"": ""{1}"",
    ""Description"": ""Geriatric Manager"",
    ""DeleteAction"": ""Inactive"",
    ""SessionTimeout"": ""15m"",
    ""MultiCompany"": true,
    ""ListNumRows"": 50,
    ""DefaultLanguage"": ""es-es"",
    ""Security"": {{
                    ""IPAccess"": false,
        ""IPAccessMail"": ""asistencia@openframework.es"",
        ""PasswordComplexity"": ""Strong"",
        ""MustChangePassword"": false,
        ""Traceability"": ""item"",
        ""GrantPermission"": ""ByUser"",
        ""FailedAttempts"": 3,
        ""MinimumPasswordLength"": 6,
        ""GroupUserMain"": false
    }},
    ""Profile"": {{
                    ""Twiter"": true,
        ""LinkedIn"": true,
        ""Facebook"": false,
        ""Instagram"": false,
        ""BirthDate"": true,
        ""Mobile"": true,
        ""IdentificationCard"": true,
        ""NameFormat"": 2,
        ""Gender"": true,
        ""PhoneEmergency"": true,
        ""EmailAlternative"": true,
        ""Web"": false,
        ""Nacionality"": true,
        ""Fax"": true,
        ""Address"": true,
        ""Signature"": false,
        ""PreferencesEnabled"": false,
        ""ActivityEnabled"": false,
        ""FollowEnabled"": false
    }},    
    ""CompanyProfile"": {{
                    ""CompanyAddressEnabled"": false,
        ""BankAccountEnabled"": false,
        ""ContactPersonEnabled"": false,
        ""CustomConfig"": false,
        ""CustomFields"":[]
    }}
            }}",
                    this.Id,
                    this.Name,
                    this.Config.ConnectionString);

                return res;
            }
        }

        /// <summary>Gets or sets the customer framework identifier</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets the customer framework name</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the customer framework description</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets the customer framework configuration</summary>
        public Config Config { get; set; }

        /// <summary>Clear instance's definitions</summary>
        public void ClearDefinitions()
        {
            this.definitions = new List<ItemDefinition>();
        }

        /// <summary>Add an item definition in instance</summary>
        /// <param name="definition">Item definition</param>
        public void AddDefinition(ItemDefinition definition)
        {
            if (this.definitions == null)
            {
                this.definitions = new List<ItemDefinition>
                {
                    definition
                };
            }
            else
            {
                if (!this.definitions.Any(d => d.ItemName.Equals(definition.ItemName, StringComparison.OrdinalIgnoreCase)))
                {
                    this.definitions.Add(definition);
                }
            }
        }

        public ReadOnlyCollection<ItemDefinition> ScopeView
        {
            get
            {
                return new ReadOnlyCollection<ItemDefinition>(this.ItemDefinitions.Where(df => df.ScopeView == true).ToList());
            }
        }
    }
}