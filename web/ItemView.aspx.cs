using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenFrameworkV3;

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
        this.ListId = this.master.CodedQuery.GetByKey<string>("L");
        this.FormId = this.master.CodedQuery.GetByKey<string>("F");
        this.ItemId = this.master.CodedQuery.GetByKey<long>("Id");


        var itemDefinition = Persistence.ItemDefinitionByName(this.ItemName, this.master.InstanceName);
        this.ItemDefinitionId = itemDefinition.Id;
        var listDefinition = itemDefinition.Lists.First();
        if (itemDefinition.Lists.Any(l => l.Id.Equals(this.ListId, StringComparison.OrdinalIgnoreCase)))
        {
            listDefinition = itemDefinition.Lists.First(l => l.Id.Equals(this.ListId, StringComparison.OrdinalIgnoreCase));
        }

        var title = listDefinition.Title ?? itemDefinition.Layout.LabelPlural;
        this.master.SetPageType("PageView");
    }
}