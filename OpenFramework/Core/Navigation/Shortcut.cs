// --------------------------------
// <copyright file="Shortcut.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Navigation
{
    using System;
    using System.Globalization;
    using System.Linq;
    using OpenFramework.InstanceManager;

    /// <summary>Class that implements a class for menu's shortcuts</summary>
    public class Shortcut
    {
        public static Shortcut Empty
        {
            get
            {
                return new Shortcut
                {
                    Icon = string.Empty,
                    Id = Constant.DefaultId,
                    Label = string.Empty,
                    Link = string.Empty,
                    ShortcutType = ShortcutTypes.None,
                };
            }
        }

        /// <summary>Gets or sets the shortcut's identifier</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets the shortcut's label text</summary>
        public string Label { get; set; }

        /// <summary>Gets or sets the link address of shortcut</summary>
        public string Link { get; set; }

        /// <summary>Gets or sets the icon of shortcut</summary>
        public string Icon { get; set; }

        /// <summary>Gets or sets the type of shortcut (red, blue, green, yellow, or none by default)</summary>
        public ShortcutTypes ShortcutType { get; set; }

        public static Shortcut ByItemId(long itemId, string instanceName)
        {
            var res = Shortcut.Empty;

            var items = Persistence.ItemDefinitions(instanceName);
            if (items.Any(i => i.Id == itemId))
            {
                var itemIndex = items.First(i => i.Id == itemId);
                res.Id = itemId;
                res.Icon = itemIndex.Layout.Icon;
                res.Label = itemIndex.Layout.Label;
            }

            return res;
        }

        /// <summary>Gets the HTML code for a shortcut on user's menu</summary>
        /// <returns>HTML code for a shortcut on user's menu</returns>
        public string Selector
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"<button type=""button"" class=""btn btn-info"" style=""height:32px;"" onclick=""alert('{0}');"" title=""{0}""><i class=""{1}""></i></button>",
                    ApplicationDictionary.Translate(this.Label),
                    this.Icon);
            }
        }

        /// <summary>Gets a Json structure of shortcut</summary>
        /// <returns>Json structure of shortcut</returns>
        public string Json
        {
            get
            {
                if (this.Id > 0)
                {
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        @"{{""Id"":{0},""Label"":""{1}"",""Icon"":""{2}""}}",
                        this.Id,
                        Tools.Json.JsonCompliant(ApplicationDictionary.Translate(this.Label)),
                        this.Icon);
                }

                return Constant.JavaScriptNull;
            }
        }

        public string RenderLarge
        {
            get
            {
                var color = string.Empty;
                switch (this.ShortcutType)
                {
                    case ShortcutTypes.Red:
                        color = "btn-danger";
                        break;
                    case ShortcutTypes.Blue:
                        color = "btn-info";
                        break;
                    case ShortcutTypes.Green:
                        color = "btn-success";
                        break;
                    case ShortcutTypes.Yellow:
                        color = "btn-warning";
                        break;
                    default:
                        color = string.Empty;
                        break;
                }

                var query = string.Format(
                CultureInfo.InvariantCulture,
                "itemTypeId={0}&listid={1}&optionId={2}&ac={3}",
                this.Id,
                "custom",
                this.Id,
                Guid.NewGuid());
                var encodedQuery = Tools.Basics.Base64Encode(query);
                var link = string.Format(CultureInfo.InvariantCulture, "/ItemList.aspx?{0}", encodedQuery);

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"<button type=""button"" class=""btn {0}"" title=""{2}"" id=""SC-{0}"" onclick=""document.location='{3}';""><i class=""ace-icon fa fa-{1}""></i></button>{4}",
                    color,
                    Icon,
                    Label,
                    link,
                    Environment.NewLine);
            }
        }

        public string RenderSmall
        {
            get
            {
                switch (this.ShortcutType)
                {
                    case ShortcutTypes.Blue:
                        return "<span class=\"btn btn-info\" title=\"" + this.Label + "\"></span>";
                    case ShortcutTypes.Green:
                        return "<span class=\"btn btn-success\" title=\"" + this.Label + "\"></span>";
                    case ShortcutTypes.Yellow:
                        return "<span class=\"btn btn-warning\" title=\"" + this.Label + "\"></span>";
                    case ShortcutTypes.Red:
                        return "<span class=\"btn btn-danger\" title=\"" + this.Label + "\"></span>";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}