// --------------------------------
// <copyright file="MenuOption.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core.Security;

    public class MenuOption
    {
        [JsonProperty("Icon")]
        private string icon;

        [JsonProperty("Leaf")]
        public bool Leaf { get; set; }

        [JsonProperty("Url")]
        public string DestinationPage { get; set; }

        [JsonProperty("Label")]
        public string Label { get; set; }

        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Item")]
        public string ItemName { get; set; }

        [JsonProperty("ListId")]
        public string ListId { get; set; }

        [JsonProperty("Options")]
        private MenuOption[] options;

        [JsonProperty("Groups")]
        private long[] groups;

        [JsonIgnore]
        public ReadOnlyCollection<long> Groups
        {
            get
            {
                if (this.groups == null)
                {
                    return new ReadOnlyCollection<long>(new List<long>());
                }

                return new ReadOnlyCollection<long>(this.groups);
            }
        }

        [JsonIgnore]
        public string Icon
        {
            get
            {
                if (string.IsNullOrEmpty(this.icon))
                {
                    return "fa fa-list";
                }

                return this.icon;
            }
        }

        [JsonProperty("OptionId")]
        public long OptionId { get; set; }

        public void SetIcon(string icon)
        {
            this.icon = icon;
        }

        [JsonIgnore]
        public ReadOnlyCollection<MenuOption> Options
        {
            get
            {
                if (this.options == null)
                {
                    return new ReadOnlyCollection<MenuOption>(new List<MenuOption>());
                }

                return new ReadOnlyCollection<MenuOption>(this.options.ToList());
            }
        }

        public void SetOptionsBulk(ReadOnlyCollection<MenuOption> optionsToAdd)
        {
            if (options == null)
            {
                return;
            }

            this.options = optionsToAdd.ToArray();
        }

        public void AddOptionsBulk(ReadOnlyCollection<MenuOption> optionsToAdd)
        {
            if (optionsToAdd == null)
            {
                return;
            }

            if (this.options == null)
            {
                this.options = optionsToAdd.ToArray();
            }
            else
            {
                var temp = this.options.ToList();
                temp.AddRange(optionsToAdd);
                this.options = temp.ToArray();
            }
        }

        public static ReadOnlyCollection<MenuOption> ByGrants(ReadOnlyCollection<MenuOption> options, ReadOnlyCollection<Grant> grants)
        {
            var user = ApplicationUser.Actual;
            if (options == null || options.Count == 0)
            {
                return new ReadOnlyCollection<MenuOption>(new List<MenuOption>());
            }

            var res = new List<MenuOption>();
            foreach (var option in options)
            {
                var optionTraspassed = option;

                // Si es administrador tiene acceso a todo
                if (!ApplicationUser.Actual.AdminUser)
                {
                    // Los permisos primero se comprueban con ItemId y en caso negativo con ItemName
                    if (!grants.Any(g => g.ItemId == option.Id && g.Grants.Contains("R")) && optionTraspassed.Leaf)
                    {
                        if (!grants.Any(g => g.ItemName.Equals(option.ItemName, StringComparison.OrdinalIgnoreCase) && g.Grants.Contains("R")) && optionTraspassed.Leaf)
                        {
                            bool byOptionGroup = false;
                            if (option.groups != null && option.groups.Count() > 0)
                            {
                                foreach (var optionGroup in option.Groups)
                                {
                                    if (user.Groups.Any(g => g == optionGroup))
                                    {
                                        byOptionGroup = true;
                                        break;
                                    }
                                }
                            }

                            if (!byOptionGroup)
                            {
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    // Las opciones con grupo -1, están vetadas expresamente sólo para administradores
                    if (option.groups != null)
                    {
                        if (option.groups.Any(g => g == -1))
                        {
                            continue;
                        }
                    }
                }

                if (option.Options != null && option.Options.Count > 0)
                {
                    option.SetOptionsBulk(ByGrants(option.Options, grants));
                }

                // Si es un grupo y no tiene subopciones no se añade
                if (!optionTraspassed.Leaf && option.Options.Count == 0)
                {
                    continue;
                }

                res.Add(optionTraspassed);
            }

            return new ReadOnlyCollection<MenuOption>(res);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}