// --------------------------------
// <copyright file="MenuOption.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.UserInterface
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using OpenFramework;
    using OpenFramework.ItemManager;
    using OpenFramework.Security;

    /// <summary>Implements MenuOption class</summary>
    public class MenuOption
    {
        /// <summary>Menu options</summary>
        private List<ApplicationItem> children;

        /// <summary>Gets or sets the option's item</summary>
        public ApplicationItem Item { get; set; }

        /// <summary>Gets option children</summary>
        public ReadOnlyCollection<ApplicationItem> Children
        {
            get
            {
                if (this.children == null)
                {
                    this.children = new List<ApplicationItem>();
                }

                return new ReadOnlyCollection<ApplicationItem>(this.children);
            }
        }  
 
        /// <summary>Render the HTML code for an menu option</summary>
        /// <param name="options">Menu options</param>
        /// <returns>HTML code for an menu option</returns>
        public static string RenderMenu(ReadOnlyCollection<MenuOption> options)
        {
            if (options == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder();
            foreach (var option in options)
            {
                res.Append(option.Render());
            }

            return res.ToString();
        }
 
        /// <summary>Render the HTML code for a menu</summary>
        /// <returns>HTML code for a menu</returns>
        public string Render()
        {
            if (this.Item.Container)
            {
                return this.RenderContainer();
            }

            if (this.Item.Parent == 0)
            {
                return this.RenderLevel0();
            }

            return string.Empty;
        }

        /// <summary>Render the HTML code for a menu container</summary>
        /// <returns>HTML code for a menu container</returns>
        public string RenderContainer()
        {
            if (this.children.Count == 0)
            {
                return string.Empty;
            }

            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            var user = HttpContext.Current.Session["User"] as ApplicationUser;
            var res = new StringBuilder();
            bool selected = false;
            var actualUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToUpperInvariant();

            foreach (var option in this.Children)
            {
                res.Append(option.Render());
                if (option.Url != null && !selected)
                {
                    selected = option.Url.AbsoluteUri.ToUpperInvariant() == actualUrl;
                }
            }

            string pattern = @"<li class=""hsub{2} submenu"">
                            <a href=""#"" class=""dropdown-toggle"">
                                <i class=""menu-icon fa fa-{4}""></i>
                                <span class=""menu-text""> {1}</span>
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
                dictionary[this.Item.Description],
                selected ? " open" : string.Empty,
                selected ? "block" : "none",
                this.Item.Icon);
        }

        /// <summary>Render the HTML code for the level 0 of menu</summary>
        /// <returns>HTML code for a menu</returns>
        public string RenderLevel0()
        {
            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            var actualUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            if (HttpContext.Current.Request.Url.LocalPath != "/")
            {
                string baseUrl = actualUrl.Replace(HttpContext.Current.Request.Url.LocalPath.Substring(1), string.Empty);
                if (!string.IsNullOrEmpty(baseUrl))
                {
                    actualUrl = "/" + actualUrl.Replace(baseUrl, string.Empty);
                }
            }

            bool current = false;
            if (this.Item.Url != null)
            {
                current = this.Item.Url.AbsolutePath.ToUpperInvariant() == actualUrl.ToUpperInvariant();
            }

            string currentText = current ? " class=\"active\"" : string.Empty;
            return string.Format(
                CultureInfo.InvariantCulture,
                @"<li {2} id=""menuoption-{4}""><a href=""{1}""><i class=""menu-icon fa fa-{3}""></i><span class=""menu-text""> {0} </span></a></li>",
                ApplicationDictionary.Translate(this.Item.Description),
                this.Item.Url,
                currentText,
                this.Item.Icon,
                this.Item.Id);
        }
    }
}