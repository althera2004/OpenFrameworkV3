using OpenFramework.ItemManager;
using OpenFramework.ItemManager.Form;
using OpenFramework.ItemManager.List;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFramework.UserInterface
{
    public class FormTab
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public bool Active { get; set; }
        public bool Selected { get; set; }
        private List<FormRow> rows;

        public static FormTab FromDefinition(FormTabDefinition definition, ItemDefinition itemDefinition, bool selected)
        {
            var res = new FormTab
            {
                Id = definition.Id ?? Tools.LabelToIdentifier(definition.Label),
                Label = definition.Label,
                rows = new List<FormRow>(),
                Selected = definition.DefaultSelected || selected
            };

            foreach (var row in definition.Rows)
            {
                res.rows.Add(FormRow.FromDefinition(row, itemDefinition));
            }

            return res;
        }

        public ReadOnlyCollection<ListDefinition> InternalLists
        {
            get
            {
                var res = new List<ListDefinition>();
                foreach(var row in this.rows)
                {
                    if(row.FormList != null)
                    {
                        res.Add(row.FormList.ListDefinition);
                    }
                }

                return new ReadOnlyCollection<ListDefinition>(res);
            }
        }

        /*public string Render
        {
            get
            {
                var res = new StringBuilder();
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<div id=""{0}"" class=""tab-pane{1}"">
                         <form class=""form-horizontal"" role=""form"">",
                    this.Id,
                    this.Selected ? " active" : string.Empty);

                foreach(var row in this.rows)
                {
                    res.Append(row.Render);
                }

                res.Append("</form></div>");

                return res.ToString();
            }
        }*/

        public string DataOrigin
        {
            get
            {
                var res = new StringBuilder();
                bool first = true;
                foreach (var row in rows)
                {
                    if (!string.IsNullOrEmpty(row.DataOrigin))
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            res.Append(",");
                        }

                        res.Append("// ").Append(this.Label).Append(Environment.NewLine);
                        res.Append(row.DataOrigin);
                    }
                }

                return res.ToString();
            }
        }

        public string SelectOptions
        {
            get
            {
                var res = new StringBuilder();
                bool first = true;
                foreach (var row in rows)
                {
                    if (!string.IsNullOrEmpty(row.SelectOptions))
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            res.Append(",");
                        }

                        res.Append("// ").Append(this.Label).Append(Environment.NewLine);
                        res.Append(row.SelectOptions);
                    }
                }

                return res.ToString();
            }
        }
    }
}
