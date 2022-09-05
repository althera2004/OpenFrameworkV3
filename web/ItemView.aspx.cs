// -----------------------------------------------------------------------
// <copyright file="ItemView.aspx.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using OpenFrameworkV3;
using OpenFrameworkV3.Core.DataAccess;
using OpenFrameworkV3.Feature;
using OpenFrameworkV3.Tools;

public partial class ItemView : Page
{
    private Main master;

    public string QueryBase
    {
        get
        {
            return this.master.QueryBase;
        }
    }

    public long ItemDefinitionId { get; private set; }

    public string ItemName { get; private set; }

    public string ListId { get; private set; }

    public string FormId { get; private set; }

    public long ItemId { get; private set; }

    public string JsonData { get; private set; }

    public string FK { get; private set; }
    public string Stick { get; private set; }

    public string InstanceName
    {
        get
        {
            return this.master.InstanceName;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.master = this.Master as Main;
        this.ItemName = this.master.CodedQuery.GetByKey<string>("Item");
        this.ListId = this.master.CodedQuery.GetByKey<string>("List");
        this.FormId = this.master.CodedQuery.GetByKey<string>("F");
        this.ItemId = this.master.CodedQuery.GetByKey<long>("Id");

        var itemDefinition = Persistence.ItemDefinitionByName(this.ItemName, this.master.InstanceName);
        this.ItemDefinitionId = itemDefinition.Id;
        var listDefinition = itemDefinition.Lists.First();
        if (itemDefinition.Lists.Any(l => l.Id.Equals(this.ListId, StringComparison.OrdinalIgnoreCase)))
        {
            listDefinition = itemDefinition.Lists.First(l => l.Id.Equals(this.ListId, StringComparison.OrdinalIgnoreCase));
        }

        this.JsonData = Read.JsonById(this.ItemId, itemDefinition, this.master.InstanceName);

        var title = listDefinition.Title ?? itemDefinition.Layout.LabelPlural;
        this.master.SetPageType("PageView");

        if(itemDefinition.ForeignValues.Count > 0)
        {
            var itemNames = new List<string>();
            foreach(var fv in itemDefinition.ForeignValues)
            {
                var fvItem = Persistence.ItemDefinitionByName(fv.ItemName, this.master.InstanceName);
                if (fvItem.Features.Persistence)
                {
                    this.master.AddFKScript(fvItem.ItemName);
                }
                else
                {
                    itemNames.Add(fvItem.ItemName);
                }
            }

            foreach(var itemName in itemNames)
            {
                var data = string.Empty;
                var fkDefinition = Persistence.ItemDefinitionByName(itemName, this.master.InstanceName);
                if (!string.IsNullOrEmpty(fkDefinition.CustomFK))
                {
                    data = Read.GetCustomFK(itemName, this.master.InstanceName);
                }
                else
                {
                    data = Read.JsonActive(itemName, this.master.CompanyId, this.master.InstanceName);
                }

                this.FK += "FK[\"" + itemName + "\"] = {\"Data\":" + data.Replace("\n", "\\n") + "};";
            }
        }

        // weke-product if (this.master.Instance.Config.Sticking && itemDefinition.Features.Sticky && this.ItemId > 0)
        if (itemDefinition.Features.Sticky && this.ItemId > 0)
            {
            this.Stick = Sticky.JsonList(Sticky.ByItemId(itemDefinition.Id, this.ItemId, this.master.CompanyId, this.master.InstanceName));
        }
        else
        {
            this.Stick = Json.EmptyJsonList;
        }
    }
}