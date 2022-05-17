// --------------------------------
// <copyright file="BreadCrumb.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Navigation
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Text;
    using OpenFrameworkV3.Tools;

    public class BreadCrumb
    {
        private List<BreadcrumbItem> items;

        public string QueryBase { get; private set; }

        public string Title { get; private set; }

        public void SetTitle(string title)
        {
            this.Title = title;
        }
        public string Subtitle { get; private set; }

        public void SetSubtitle(string subtitle)
        {
            this.Subtitle = subtitle;
        }

        public ReadOnlyCollection<BreadcrumbItem> Items
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new List<BreadcrumbItem>();
                }

                return new ReadOnlyCollection<BreadcrumbItem>(this.items);
            }
        }

        public BreadCrumb()
        {
            this.QueryBase = string.Empty;
        }

        public BreadCrumb(string querybase)
        {
            this.QueryBase = querybase;
        }

        public void Add(string label)
        {
            if (this.items == null)
            {
                this.items = new List<BreadcrumbItem>();
            }

            this.items.Add(new BreadcrumbItem
            {
                Link = "#",
                Label = ApplicationDictionary.Translate(label),
                Leaf = Constant.NotLeaft
            });
        }

        public void Add(string label, bool leaf)
        {
            if (this.items == null)
            {
                this.items = new List<BreadcrumbItem>();
            }

            this.items.Add(new BreadcrumbItem
            {
                Link = "#",
                Label = ApplicationDictionary.Translate(label),
                Leaf = leaf
            });
        }

        public void AddLeaf(string label)
        {
            if (this.items == null)
            {
                this.items = new List<BreadcrumbItem>();
            }

            this.items.Add(new BreadcrumbItem
            {
                Link = "#",
                Label = ApplicationDictionary.Translate(label),
                Leaf = Constant.BreadCrumbLeaf
            });
        }

        public void AddEncryptedList(string label, string itemId, string listId)
        {
            if (this.items == null)
            {
                this.items = new List<BreadcrumbItem>();
            }

            this.items.Add(new BreadcrumbItem
            {
                Link = string.Empty,
                ItemId = itemId,
                ListId = listId,
                Label = ApplicationDictionary.Translate(label),
                Leaf = Constant.NotLeaft,
                Encrypted = true
            });
        }

        public void AddEncryptedLink(string label, string link)
        {
            if (this.items == null)
            {
                this.items = new List<BreadcrumbItem>();
            }

            this.items.Add(new BreadcrumbItem
            {
                Link = link,
                Label = ApplicationDictionary.Translate(label),
                Leaf = Constant.NotLeaft,
                Encrypted = true
            });
        }

        public void Add(string label, string link)
        {
            if (this.items == null)
            {
                this.items = new List<BreadcrumbItem>();
            }

            this.items.Add(new BreadcrumbItem
            {
                Link = link,
                Label = ApplicationDictionary.Translate(label),
                Leaf = Constant.NotLeaft
            });
        }

        public void Add(string label, string link, bool leaf)
        {
            if (this.items == null)
            {
                this.items = new List<BreadcrumbItem>();
            }

            this.items.Add(new BreadcrumbItem
            {
                Link = link,
                Label = ApplicationDictionary.Translate(label),
                Leaf = leaf
            });
        }

        /// <summary>Gets the HTML code for breadcrumb object</summary>
        public string Render
        {
            get
            {
                var res = new StringBuilder();
                foreach (var item in this.Items)
                {
                    if (item.Leaf)
                    {
                        res.AppendFormat(CultureInfo.InvariantCulture, @"<li class=""active""><span id=""BreadCrumbLabel"">{0}</span></li>", item.Label);
                    }
                    else
                    {
                        if (item.Encrypted)
                        {
                            if (!string.IsNullOrEmpty(item.ListId))
                            {
                                res.AppendFormat(CultureInfo.InvariantCulture, @"<li><a href=""#"" title=""{2}"" onclick=""GoEncryptedList('{0}','{1}');"">{2}</a></li>", item.ItemId, item.ListId, item.Label);
                            }
                            else
                            {
                                res.AppendFormat(CultureInfo.InvariantCulture, @"<li><a href=""#"" title=""{1}"" onclick=""GoEncryptedPage('{0}');"">{1}</a></li>", item.Link, item.Label);
                            }
                        }
                        else
                        {
                            string link = "?i=" + this.QueryBase + Basics.RandomString(128);
                            if (!string.IsNullOrEmpty(item.Link))
                            {
                                link = item.Link + "?i=" + this.QueryBase + Basics.RandomString(128);
                            }

                            res.AppendFormat(CultureInfo.InvariantCulture, @"<li><a href=""{0}"" title=""{1}"">{1}</a></li>", link, item.Label);
                        }
                    }
                }

                return res.ToString();
            }
        }
    }
}