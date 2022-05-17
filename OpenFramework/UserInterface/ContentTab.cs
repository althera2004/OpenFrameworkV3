using AltheraFramework.ItemManager;
using AltheraFramework.ItemManager.Form;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltheraFramework.UserInterface
{
    public class ContentTab
    {
        public string Id { get; set; }
        public bool Active { get; set; }

        public string Render(ReadOnlyCollection<FormRowDefinition> rows, ItemDefinition itemDefinition)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                @"
                <div id=""{0}"" class=""tab-pane {1}"">
                    <form class=""form-horizontal"" role=""form"">
                        {2}
                    </form>
                </div>",
                this.Id,
                this.Active ? "active" : string.Empty,
                this.RenderRows(rows, itemDefinition));
        }

        private string RenderRows(ReadOnlyCollection<FormRowDefinition> rows, ItemDefinition itemDefinition)
        {
            var res = new StringBuilder();

            foreach(var row in rows)
            {
                res.Append(new FormRow
                {
                    Id = string.Empty,
                    Hidden = false,
                    FormRowDefinition = row,
                    ItemDefinition = itemDefinition
                }.Render);
            }

            return res.ToString();
        }
    }
}
