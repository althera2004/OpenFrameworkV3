// --------------------------------
// <copyright file="Menu.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.ItemManager;
    using OpenFrameworkV3.Core.Security;
    using OpenFrameworkV3.Tools;

    public class Menu
    {
        private string instanceName;

        /// <summary>Menu options</summary>
        [JsonProperty("Options")]
        private MenuOption[] options;

        /// <summary>Menu options</summary>
        [JsonIgnore]
        public ReadOnlyCollection<MenuOption> Options
        {
            get
            {
                if (this.options == null)
                {
                    this.options = new List<MenuOption>().ToArray();
                }

                return new ReadOnlyCollection<MenuOption>(this.options);
            }
        }

        /// <summary>Gets an empty Menu object</summary>
        [JsonIgnore]
        public static Menu Empty
        {
            get
            {
                return new Menu
                {
                    options = new List<MenuOption>().ToArray()
                };
            }
        }

        public void AddOptionsBulk(ReadOnlyCollection<MenuOption> options)
        {
            if (options == null)
            {
                return;
            }

            var realOptions = new List<MenuOption>();
            foreach (var option in options)
            {
                if (option.Groups == null || option.Groups.Count == 0)
                {
                    realOptions.Add(option);
                }
                else
                {
                    var user = ApplicationUser.Actual;
                    foreach (var group in option.Groups)
                    {
                        if (user.Groups.Any(g => g.Id == group))
                        {
                            realOptions.Add(option);
                            break;
                        }
                    }
                }
            }

            if (this.options == null)
            {
                this.options = realOptions.ToArray();
            }
            else
            {
                var temp = this.options.ToList();
                temp.AddRange(realOptions.ToList());
                this.options = temp.ToArray();
            }
        }

        public string GetJson()
        {
            return Json(new ReadOnlyCollection<MenuOption>(this.options));
        }

        public static string Json(ReadOnlyCollection<MenuOption> options)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var option in options)
            {
                if (option.Options.Count > 0)
                {
                    var resOptions = Json(option.Options);
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"{7}{{""Id"": ""{0}"", ""Label"": ""{1}"", ""Icon"":""{2}"", ""Link"":""{3}"",""ItemName"":""{4}"",""ListId"":""{5}"", ""Options"":{6}}}",
                        option.Id,
                        option.Label,
                        option.Icon,
                        option.Link,
                        option.ItemName,
                        option.ListId,
                        resOptions,
                        first ? string.Empty : ", ");
                }
                else
                {
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"{6}{{""Id"": ""{0}"", ""Label"": ""{1}"", ""Icon"":""{2}"", ""Link"":""{3}"",""ItemName"":""{4}"",""ListId"":""{5}""}}",
                        option.Id,
                        option.Label,
                        option.Icon,
                        option.Link,
                        option.ItemName,
                        option.ListId,
                        first ? string.Empty : ", ");
                }

                first = false;
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>Load menu</summary>
        /// <param name="externalUsers">External users identifiers</param>
        /// <param name="codedQueryClean">Indicates if query is clean</param>
        /// <returns>Main menu</returns>
        public static Menu Load(string externalUsers, bool codedQueryClean, string instanceName)
        {
            var applicationUser = HttpContext.Current.Session["ApplicationUser"] as ApplicationUser;
            var menuPath = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!menuPath.EndsWith(@"\"))
            {
                menuPath = string.Format(CultureInfo.InvariantCulture, @"{0}\Instances\{1}\", menuPath, instanceName);
            }
            else
            {
                menuPath = string.Format(CultureInfo.InvariantCulture, @"{0}Instances\{1}\", menuPath, instanceName);
            }

            var res = Menu.Empty;
            var temp = Menu.Empty;

            if (!string.IsNullOrEmpty(externalUsers))
            {
                foreach (string externalUser in externalUsers.Split('|'))
                {
                    if (string.IsNullOrEmpty(externalUser))
                    {
                        continue;
                    }

                    try
                    {
                        string menuFile = string.Format(CultureInfo.InvariantCulture, "{0}menu_{1}.json", menuPath, externalUser.Replace(' ', '_'));
                        var menuContentJson = Tools.Json.EmptyJsonObject;

                        if (File.Exists(menuFile))
                        {
                            using (var input = new StreamReader(menuFile))
                            {
                                menuContentJson = input.ReadToEnd();
                            }

                            menuContentJson = string.Format(CultureInfo.InvariantCulture, @"{{""Options"":{0}}}", menuContentJson);
                            temp = JsonConvert.DeserializeObject<Menu>(menuContentJson);
                            ValidateOptions(temp, instanceName);
                            res.AddOptionsBulk(MenuOption.ByGrants(temp.Options, applicationUser));
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "Default.aspx::RenderMenu::[{0}]", instanceName));
                    }
                }
            }

            HttpContext.Current.Session["Menu"] = res;
            return res;
        }

        /// <summary>Load menu</summary>
        /// <param name="codedQueryClean">Indicates if query is clean</param>
        /// <returns>Main menu</returns>
        public static Menu Load(long applicationUserId,long companyId, bool codedQueryClean, string instanceName)
        {
            var applicationUser = ApplicationUser.ById(applicationUserId, instanceName);
            return Load(applicationUser, companyId, codedQueryClean, instanceName);
        }

        /// <summary>Load menu</summary>
        /// <param name="codedQueryClean">Indicates if query is clean</param>
        /// <returns>Main menu</returns>
        public static Menu Load(ApplicationUser applicationUser, long companyId, bool codedQueryClean, string instanceName)
        {

            // weke if (applicationUser.External)
            //{
            //    return Load(applicationUser.ExternalUsers, instance.Config.CodedQueryClean);
            //}

            var menuPath = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!menuPath.EndsWith(@"\"))
            {
                menuPath = string.Format(CultureInfo.InvariantCulture, @"{0}\Instances\{1}\menu.json", menuPath, instanceName);
            }
            else
            {
                menuPath = string.Format(CultureInfo.InvariantCulture, @"{0}Instances\{1}\menu.json", menuPath, instanceName);
            }

            var res = Menu.Empty;
            var temp = Menu.Empty;

            res.instanceName = instanceName;
            temp.instanceName = instanceName;
            try
            {
                var menuContentJson = Tools.Json.EmptyJsonObject;
                using (var input = new StreamReader(menuPath))
                {
                    menuContentJson = input.ReadToEnd();
                }

                menuContentJson = string.Format(CultureInfo.InvariantCulture, @"{{""Options"":{0}}}", menuContentJson);
                temp = JsonConvert.DeserializeObject<Menu>(menuContentJson);
                ValidateOptions(temp, instanceName);
                res.AddOptionsBulk(MenuOption.ByGrants(temp.Options, applicationUser));
            }
            catch (Exception ex)
            {
                ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "Default.aspx::RenderMenu::[{0}]", instanceName));
            }

            if (applicationUser.AdminUser || applicationUser.AdminUser)
            {
                var administrationOptions = new List<MenuOption>();
                var instance = Persistence.InstanceByName(instanceName);
                if (instance.Config.CompanyProfile.CustomConfig)
                {
                    var customConfigOption = new MenuOption
                    {
                        Id = 1011,
                        OptionId = 1011,
                        ItemName = "Core_CompanyCustomConfig",
                        Label = "Core_CompanyCustomConfig_MenuLabel",
                        Leaf = true,
                        DestinationPage = "/Admin/CompanyCustomConfig.aspx"
                    };
                    customConfigOption.SetIcon("fa fa-cog");
                    administrationOptions.Add(customConfigOption);
                }

                var companyOption = new MenuOption
                {
                    Id = 1000,
                    OptionId = 1000,
                    ItemName = "Core_CompanyProfile",
                    Label = "Core_CompanyProfile_MenuLabel",
                    Leaf = true,
                    DestinationPage = "/Admin/CompanyProfile.aspx?" + Tools.Basics.Base64Encode("I=" + instanceName + "&C=" + companyId.ToString())
                };

                companyOption.SetIcon("fa fa-user-tie");
                administrationOptions.Add(companyOption);

                if (applicationUser.AdminUser)
                {
                    var usersOption = new MenuOption
                    {
                        Id = 1001,
                        OptionId = 1001,
                        ItemName = "Core_ApplicationUser",
                        Label = "Core_ApplicationUser_MenuLabel",
                        Leaf = true,
                        DestinationPage = "/Admin/ApplicationUserList.aspx"
                    };

                    usersOption.SetIcon("fa fa-user");
                    administrationOptions.Add(usersOption);

                    administrationOptions.Add(new MenuOption
                    {
                        Id = 1003,
                        OptionId = 1003,
                        ItemName = "Core_Security",
                        Label = "Core_Security_MenuLabel",
                        Leaf = true,
                        DestinationPage = "/Admin/Security"
                    });

                    if (applicationUser.Core)
                    {
                        administrationOptions.Add(new MenuOption
                        {
                            Id = 1004,
                            OptionId = 1004,
                            ItemName = "Core_Feature_Dictionary",
                            Label = "Core_Feature_Dictionary_Label",
                            Leaf = true,
                            DestinationPage = "/Admin/Funcionality/Dictionary.aspx"
                        });
                    }

                    administrationOptions.Add(new MenuOption
                    {
                        Id = 1011,
                        OptionId = 1011,
                        ItemName = "Core_Backup",
                        Label = "Core_BackUp_MenuLabel",
                        Leaf = true,
                        DestinationPage = "/Admin/BackupHome.aspx"
                    });

                    if (applicationUser.AdminUser)
                    {
                        administrationOptions.Add(new MenuOption
                        {
                            Id = 1020,
                            OptionId = 1020,
                            ItemName = "Core_Group",
                            Label = "Core_Import_MenuLabel",
                            Leaf = true,
                            DestinationPage = "/Admin/ImportHome.aspx"
                        });

                        var groupsOption = new MenuOption
                        {
                            Id = 1002,
                            OptionId = 1002,
                            ItemName = "Core_Group",
                            Label = "Core_SecurityGroup_MenuLabel",
                            Leaf = true,
                            DestinationPage = "/Admin/SecurityGroupList.aspx"
                        };

                        groupsOption.SetIcon("fa fa-users");
                        administrationOptions.Add(groupsOption);
                    }
                }

                var administrationOption = new MenuOption
                {
                    Leaf = false,
                    Label = "Common_Administration"
                };

                administrationOption.SetIcon("fad fa-cogs");

                administrationOption.AddOptionsBulk(new ReadOnlyCollection<MenuOption>(administrationOptions.OrderBy(ao => ao.Id).ToList()));

                var options = res.options.ToList();
                options.Add(administrationOption);
                res.options = options.ToArray();
            }

            return res;
        }

        /// <summary>Validate menu options readed from menu file</summary>
        /// <param name="menu">Menu destination</param>
        /// <param name="instanceName">NAme of actual instance</param>
        private static void ValidateOptions(Menu menu, string instanceName)
        {
            long optionId = 1;
            var instance = Persistence.InstanceByName(instanceName);
            foreach (var option in menu.options)
            {
                try
                {
                    if (!string.IsNullOrEmpty(option.ItemName))
                    {
                        var itemDefinition = instance.ItemDefinitions.First(d => d.ItemName.Equals(option.ItemName));
                        if (!string.IsNullOrEmpty(option.ItemName))
                        {
                            optionId++;
                            if (option.OptionId == 0)
                            {
                                option.OptionId = optionId;
                            }

                            if (string.IsNullOrEmpty(option.ListId))
                            {
                                option.ListId = "Custom";
                            }

                            if (string.IsNullOrEmpty(option.Label))
                            {
                                if (!string.IsNullOrEmpty(option.ListId))
                                {
                                    if (itemDefinition.Lists.Any(l => l.Id.Equals(option.ListId, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        var list = itemDefinition.Lists.First(l => l.Id.Equals(option.ListId, StringComparison.OrdinalIgnoreCase));
                                        var label = list.Title;
                                        if(string.IsNullOrEmpty(label))
                                        {
                                            label = itemDefinition.Layout.LabelPlural;
                                        }
                                        
                                        if (!string.IsNullOrEmpty(label))
                                        {
                                            option.Label = label;
                                        }
                                    }
                                }
                            }

                            if (string.IsNullOrEmpty(option.Label))
                            {
                                option.Label = itemDefinition.Layout.LabelPlural;
                            }

                            if (option.Icon.Equals("fa fa-list", StringComparison.OrdinalIgnoreCase))
                            {
                                option.SetIcon(itemDefinition.Layout.Icon);
                            }

                            if (option.Id < 1)
                            {
                                option.Id = itemDefinition.Id;
                            }

                            if (instance.Config.CodedQueryClean)
                            {
                                option.DestinationPage = string.Format(
                                    CultureInfo.InvariantCulture,
                                    "/ItemList.aspx?d={0}.{1}.{2}&ac={3}",
                                    option.Id,
                                    string.IsNullOrEmpty(option.ListId) ? "custom" : option.ListId,
                                    option.OptionId,
                                    Guid.NewGuid());
                            }
                            else
                            {
                                var query = Tools.Basics.Base64Encode(string.Format(
                                    CultureInfo.InvariantCulture,
                                    "itemTypeId={0}&listid={1}&optionId={2}&ac={3}",
                                    option.Id,
                                    string.IsNullOrEmpty(option.ListId) ? "custom" : option.ListId,
                                    option.OptionId,
                                    1));

                                var link = string.Format(CultureInfo.InvariantCulture, "/ItemList.aspx?{0}", query);
                                option.DestinationPage = link;
                            }
                        }
                    }
                    else
                    {
                        optionId++;
                        if (option.OptionId == 0)
                        {
                            option.OptionId = optionId;
                        }
                    }

                    if (option.Options != null && option.Options.Count > 0)
                    {
                        optionId = ValidateOptions(option.Options, instanceName, optionId);
                        optionId++;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.Trace(ex as Exception, "Menu:ValidateOptions - " + instance.Name + "::" + option.ItemName);
                }
            }
        }

        /// <summary>Validates option in order to add on menu</summary>
        /// <param name="options">Set of options</param>
        /// <param name="instance">Actual instance</param>
        /// <param name="optionId">Option identifier</param>
        /// <returns>Identifier for option</returns>
        private static long ValidateOptions(ReadOnlyCollection<MenuOption> options, string instanceName, long optionId)
        {
            // Validate default values
            var instance = Persistence.InstanceByName(instanceName);
            foreach (var option in options)
            {
                var itemDefinition = ItemDefinition.Empty;
                if (!string.IsNullOrEmpty(option.ItemName))
                {
                    itemDefinition = instance.ItemDefinitions.First(d => d.ItemName.Equals(option.ItemName));
                }

                // Si la opción tiene item tiene página de destino, si no, se prueba con la url customizada en "DestinationPage"
                if (!string.IsNullOrEmpty(option.ItemName) || !string.IsNullOrEmpty(option.DestinationPage))
                {
                    optionId++;
                    if (option.OptionId == 0)
                    {
                        option.OptionId = optionId;
                    }

                    if (string.IsNullOrEmpty(option.ListId))
                    {
                        option.ListId = "Custom";
                    }

                    if (string.IsNullOrEmpty(option.Label))
                    {
                        if (!string.IsNullOrEmpty(option.ListId))
                        {
                            if (itemDefinition.Lists.Any(l => l.Id.Equals(option.ListId, StringComparison.OrdinalIgnoreCase)))
                            {
                                var list = itemDefinition.Lists.First(l => l.Id.Equals(option.ListId, StringComparison.OrdinalIgnoreCase));
                                string label = list.Title;

                                if (string.IsNullOrEmpty(label))
                                {
                                    label = itemDefinition.Layout.LabelPlural;
                                }

                                if (!string.IsNullOrEmpty(label))
                                {
                                    option.Label = label;
                                }
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(option.Label))
                    {
                        option.Label = itemDefinition.Layout.LabelPlural;
                    }

                    if (option.Icon.Equals("fa fa-list", StringComparison.OrdinalIgnoreCase))
                    {
                        option.SetIcon(itemDefinition.Layout.Icon);
                    }

                    if (option.Id < 1)
                    {
                        option.Id = itemDefinition.Id;
                    }
                }
            }

            return optionId;
        }

        /// <summary>Render HTML for option menu</summary>
        /// <param name="actualOptionId">Identifier of actual option</param>
        /// <param name="language">Language for label</param>
        /// <param name="codedQueryClean">Indicated if query is clean</param>
        /// <returns>HTML for option</returns>
        public string Render(long actualOptionId, string language, bool codedQueryClean, string instanceName)
        {
            if (options == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder();
            foreach (var option in this.options)
            {
                res.Append(this.Render(option, actualOptionId, language, codedQueryClean, instanceName));
            }

            return res.ToString();
        }

        /// <summary>Render HTML for option menu</summary>
        /// <param name="option">Option menu</param>
        /// <param name="actualOptionId">Identifier of actual option</param>
        /// <param name="language">Language for label</param>
        /// <param name="codedQueryClean">Indicated if query is clean</param>
        /// <returns>HTML for option</returns>
        public string Render(MenuOption option, long actualOptionId, string language, bool codedQueryClean, string instanceName)
        {
            var res = new StringBuilder();
            if (option.Options != null && option.Options.Count > 0)
            {
                res.Append(this.RenderContainer(option, actualOptionId, language, codedQueryClean, instanceName));
            }
            else
            {
                res.Append(this.RenderLevel0(option, actualOptionId, language, codedQueryClean, instanceName));
            }

            return res.ToString();
        }

        /// <summary>Render HTML for option menu for level 0</summary>
        /// <param name="option">Option menu</param>
        /// <param name="actualOptionId">Identifier of actual option</param>
        /// <param name="language">Language for label</param>
        /// <param name="codedQueryClean">Indicated if query is clean</param>
        /// <returns>HTML for option</returns>
        public string RenderLevel0(MenuOption option, long actualOptionId, string language, bool codedQueryClean, string instanceName)
        {
            bool adminRestricion = false;
            var instance = Persistence.InstanceByName(instanceName);
            if (instance.ItemDefinitions.Any(d => d.ItemName.Equals(option.ItemName, StringComparison.OrdinalIgnoreCase)))
            {
                adminRestricion = instance.ItemDefinitions.First(d => d.ItemName.Equals(option.ItemName, StringComparison.OrdinalIgnoreCase)).Features.AdminRestriction;
            }

            option.DestinationPage = DestinationPage(option, codedQueryClean);

            return string.Format(
                CultureInfo.InvariantCulture,
                @"
                <li class=""hightlight{3}"" ""data-itemname""=""{5}"" data-optionid=""{6}"">
                    <a href=""#"" lang=""{4}"" onclick=""Go('{0}'{8});"">
                        <i class=""menu-icon {2}""></i>&nbsp;<span>{1}</span>{7}							
					</a>
					<b class=""arrow""></b>
                </li>",
                option.DestinationPage,
                ApplicationDictionary.Translate(option.Label, language, instanceName),
                option.Icon,
                actualOptionId == option.OptionId ? " active" : string.Empty,
                language,
                option.ItemName,
                option.OptionId,
                adminRestricion ? "<i class=\"fa fa-lock red\"></i>" : string.Empty,
                adminRestricion ? ",true" : string.Empty);
        }

        /// <summary>Render HTML for option menu for level 1</summary>
        /// <param name="option">Option menu</param>
        /// <param name="actualOptionId">Identifier of actual option</param>
        /// <param name="language">Language for label</param>
        /// <param name="codedQueryClean">Indicated if query is clean</param>
        /// <returns>HTML for option</returns>
        public string RenderLevel1(MenuOption option, long actualOptionId, string language, bool codedQueryClean, string instanceName)
        {
            bool adminRestricion = false;
            var instance = Persistence.InstanceByName(instanceName);
            if (instance.ItemDefinitions.Any(d => d.ItemName.Equals(option.ItemName, StringComparison.OrdinalIgnoreCase)))
            {
                adminRestricion = instance.ItemDefinitions.First(d => d.ItemName.Equals(option.ItemName, StringComparison.OrdinalIgnoreCase)).Features.AdminRestriction;
            }

            option.DestinationPage = DestinationPage(option, codedQueryClean);

            return string.Format(
                CultureInfo.InvariantCulture,
                @"
                <li class=""hightlight{3}"" ""data-itemname""=""{5}"" data-optionid=""{6}"">
                    <a href=""#"" lang=""{4}""style=""padding-left:30px;""  onclick=""Go('{0}'{8});"">
                        <i class=""menu-icon {2}""></i>&nbsp;{1}{7}							
					</a>
					<b class=""arrow""></b>
                </li>",
                option.DestinationPage,
                ApplicationDictionary.Translate(option.Label, language, instanceName),
                option.Icon,
                actualOptionId == option.OptionId ? " active" : string.Empty,
                language,
                option.ItemName,
                option.OptionId,
                adminRestricion ? "<i class=\"fa fa-lock red\"></i>" : string.Empty,
                adminRestricion ? ",true" : string.Empty);
        }

        /// <summary>Render HTML container for options of level 1</summary>
        /// <param name="option">Option menu</param>
        /// <param name="actualOptionId">Identifier of actual option</param>
        /// <param name="language">Language for label</param>
        /// <param name="codedQueryClean">Indicated if query is clean</param>
        /// <returns>HTML for option</returns>
        public string RenderContainer(MenuOption option, long actualOptionId, string language, bool codedQueryClean, string instanceName)
        {
            if (option.Options.Count == 0)
            {
                return string.Empty;
            }

            var res = new StringBuilder();
            bool selected = false;
            foreach (var optionContainer in option.Options)
            {
                res.Append(RenderLevel1(optionContainer, actualOptionId, language, codedQueryClean, instanceName));
                if (optionContainer.OptionId == actualOptionId)
                {
                    selected = true;
                }
            }

            string pattern = @"<li class=""highlight hsub{2}"">
                            <a href=""#"" class=""dropdown-toggle"">
                                <i class=""menu-icon {4}""></i><span>{1}</span>
                                <b class=""arrow fa fa-angle-down""></b>
                            </a>
                            <ul class=""submenu nav-show"" style=""display:{3};"">
                                {0}
                            </ul>
                        </li>";

            return string.Format(
                CultureInfo.InvariantCulture,
                pattern,
                res,
                ApplicationDictionary.Translate(option.Label, language, instanceName),
                selected ? " open active" : string.Empty,
                selected ? "block" : "none",
                option.Icon);
        }

        /// <summary>Render url for destination page</summary>
        /// <param name="option">Option menu</param>
        /// <param name="codedQueryClean">Indicated if query is clean</param>
        /// <returns>Url of destination page</returns>
        public string DestinationPage(MenuOption option, bool codedQueryClean)
        {
            if (!string.IsNullOrEmpty(option.DestinationPage))
            {
                if (option.DestinationPage.IndexOf("ItemList.aspx?") != -1)
                {
                    return option.DestinationPage;
                }

                if (option.DestinationPage.IndexOf("/Features/") != -1)
                {
                    return option.DestinationPage;
                }

                if (option.DestinationPage.IndexOf("/Admin/") != -1)
                {
                    return option.DestinationPage;
                }

                if (option.DestinationPage.IndexOf("?") != -1)
                {
                    return option.DestinationPage;
                }

                var encodingQuery = string.Format(
                    CultureInfo.InvariantCulture,
                    "ac={0}&optionId={1}",
                    1,
                    option.Id);

                return string.Format(
                    CultureInfo.InvariantCulture, "{0}?{1}",
                    option.DestinationPage,
                    Tools.Basics.Base64Encode(encodingQuery));
            }

            var pattern = codedQueryClean ? "d={0}.{1}.{2}" : "itemTypeId={0}&listid={1}&optionId={2}&ac={3}";

            var query = string.Format(
                CultureInfo.InvariantCulture,
                pattern,
                option.Id,
                string.IsNullOrEmpty(option.ListId) ? "custom" : option.ListId,
                option.OptionId,
                1);

            if (codedQueryClean)
            {
                return string.Format(CultureInfo.InvariantCulture, "/ItemList.aspx?{0}", query);
            }

            var encodedQuery = Tools.Basics.Base64Encode(query);
            return string.Format(CultureInfo.InvariantCulture, "/ItemList.aspx?{0}", encodedQuery);
        }

        /// <summary>Obtains menu option by id</summary>
        /// <param name="id">Option identifier</param>
        /// <returns>Menu option</returns>
        public MenuOption OptionById(long id)
        {
            foreach (var option in this.options)
            {
                if (option.OptionId == id)
                {
                    return option;
                }

                if (option.Options != null && option.Options.Count > 0)
                {
                    foreach (var suboption in option.Options)
                    {
                        if (suboption.OptionId == id)
                        {
                            return suboption;
                        }
                    }
                }
            }

            return null;
        }
    }
}