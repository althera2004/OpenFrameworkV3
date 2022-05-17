// --------------------------------
// <copyright file="FormList.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.UserInterface
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web.Script.Serialization;
    using OpenFramework.ItemManager;
    using OpenFramework.ItemManager.List;

    public class FormList
    {
        public string Id { get; private set; }
        public string ItemId { get; private set; }
        public string InstanceName { get; private set; }
        public ListDefinition ListDefinition { get; private set; }
        public ItemDefinition ItemDefinition { get; private set; }

        /// <summary>Gets a JSON definition of list</summary>
        public string Json
        {
            get
            {
                var serializer = new JavaScriptSerializer
                {
                    MaxJsonLength = 500000000
                };

                return string.Format(
                    CultureInfo.InvariantCulture,
                    "var list_{0} = {1};{2}",
                    this.Id,
                    serializer.Serialize(this.ListDefinition),
                    Environment.NewLine);
            }
        }

        public FormList(string id, string itemId, string instanceName)
        {
            this.Id = id;
            this.ItemId = itemId;
            this.InstanceName = instanceName;
            this.ItemDefinition = ItemDefinition.Load(this.ItemId, this.InstanceName);
            this.ListDefinition = this.ItemDefinition.ListById(this.Id);
        }

        public string Render
        {
            get
            {                
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"<div class=""table-responsive"" id=""scrollTableDiv_{0}"">
                          <table class=""table table-bordered table-striped"" style=""margin:0"">
                               <thead class=""thin-border-bottom"" id=""TableHeader_{0}"">
                               {1}
                               </thead>
                            </table>
                            <div id=""ListDataDiv{0}"" style=""overflow-y:scroll;overflow-x:hidden;padding: 0px;"">
                                <table class=""table table-bordered table-striped"" style=""border-top:none;"">
                                    <tbody id=""ListDataTable_{0}""></tbody>
                                </table>
                            </div>
                            <table class=""table table-bordered table-striped"" style=""margin: 0"">
                                <thead class=""this-border-bottom"">
                                    <tr>
                                        <th style=""color:#aaa;""><i>{2}:&nbsp;<span id=""ListDataCount_{0}""></span></i></th>
                                    </tr>
                                </thead>
                            </table>
                        </div>",
                    this.Id,
                    RenderHeader(),
                    ApplicationDictionary.Translate("Common_RegisterCount"));
            }
        }

        public string RenderHeader()
        {
            var res = new StringBuilder();
            var count = 0;
            foreach (var column in this.ListDefinition.Columns)
            {
                var label = this.ItemDefinition.Fields.First(f => f.Name.Equals(column.DataProperty, StringComparison.OrdinalIgnoreCase)).Label;
                if (!string.IsNullOrEmpty(column.Label))
                {
                    label = column.Label;
                }

                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<th onclick=""Sort(this,'ListDataTable');"" id=""th{0}"" class=""{3} {4}""{2}>{1}</th>",
                    count,
                    label,
                    CssStyle(column),
                    column.Orderable ? "sort" : string.Empty,
                    column.Search ? "search" : string.Empty);
                count++;
            }

            res.Append(@"<th style=""width:108px;"">&nbsp</th>");
            return res.ToString();
        }

        private static string CssStyle(Column column)
        {
            var style = string.Empty;

            if (column.Width > 0)
            {
                style = string.Format(CultureInfo.InvariantCulture, @" style=""width:{0}px;", column.Width);
            }

            if (!string.IsNullOrEmpty(column.Align))
            {
                if (string.IsNullOrEmpty(style))
                {
                    style = string.Format(CultureInfo.InvariantCulture, @" style=""text-align:{0};", column.Align);
                }
                else
                {
                    style += string.Format(CultureInfo.InvariantCulture, @"text-align:{0};", column.Align);
                }
            }

            if (!string.IsNullOrEmpty(style))
            {
                style += "\"";
            }

            return style;
        }
    }
}