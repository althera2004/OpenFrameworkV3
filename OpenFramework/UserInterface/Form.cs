// --------------------------------
// <copyright file="Form.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.UserInterface
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web.Script.Serialization;
    using OpenFramework.ItemManager;
    using OpenFramework.ItemManager.Form;
    using OpenFramework.ItemManager.List;

    /// <summary></summary>
    public class Form
    {
        public TabBar TabBar { get; set; }

        private List<FormTab> tabs;

        public string Title { get; set; }

        public string Id { get; set; }
        public string Label { get; set; }

        public ReadOnlyCollection<ListDefinition> InternalLists
        {
            get
            {
                var res = new List<ListDefinition>();

                foreach(var tab in this.tabs)
                {
                    if(tab.InternalLists.Count > 0)
                    {
                        res.AddRange(tab.InternalLists);
                    }
                }

                return new ReadOnlyCollection<ListDefinition>(res);
            }
        }

        public string InternalListsDefinitions
        {
            get
            {
                var serializer = new JavaScriptSerializer
                {
                    MaxJsonLength = 500000000
                };

                var res = new StringBuilder("// Internal lists").Append(Environment.NewLine);
                foreach(var list in this.InternalLists)
                {
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"var list_{0} = {1};{2}",
                        list.Id,
                        serializer.Serialize(list),
                        Environment.NewLine);
                }

                res.Append("// ---------------------------").Append(Environment.NewLine);
                return res.ToString();
            }
        }

        public static Form FromDefinition(FormDefinition definition, ItemDefinition itemDefinition)
        {
            var res = new Form
            {
                Id = definition.Id,
                Label = definition.Label,
                tabs = new List<FormTab>()
            };

            res.TabBar = new TabBar();

            var defaultActive = !definition.Tabs.Any(t => t.DefaultSelected);

            foreach (var tab in definition.Tabs)
            {
                res.TabBar.AddTab(new Tab
                {
                    Id = tab.Id,
                    Label = tab.Label,
                    Active = true,
                    Hidden = false,
                    Available = true,
                    Selected = tab.DefaultSelected || defaultActive
                });

                res.tabs.Add(FormTab.FromDefinition(tab, itemDefinition, defaultActive));
                defaultActive = false;
            }

            return res;
        }

        /*public string Render
        {
            get
            {
                var res = new StringBuilder();
                foreach(var tab in this.tabs)
                {
                    res.Append(tab.Render);
                }

                return res.ToString();
            }
        }*/

        public string DataOrigin
        {
            get
            {
                var res = new StringBuilder("var DataOrigin= {");
                res.Append(Environment.NewLine);
                bool first = true;
                foreach (var tab in tabs)
                {
                    if (!string.IsNullOrEmpty(tab.DataOrigin))
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            res.Append(",");
                        }

                        res.Append(tab.DataOrigin);
                    }
                }

                res.Append(Environment.NewLine);
                res.Append("};");
                return res.ToString();
            }
        }

        public string SelectOptions
        {
            get
            {
                var res = new StringBuilder("var FK= {");
                res.Append(Environment.NewLine);
                bool first = true;
                foreach (var tab in tabs)
                {
                    if (!string.IsNullOrEmpty(tab.SelectOptions))
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            res.Append(",");
                        }

                        res.Append(tab.SelectOptions);
                    }
                }

                res.Append(Environment.NewLine);
                res.Append("};");
                return res.ToString();
            }
        }
    }        
}