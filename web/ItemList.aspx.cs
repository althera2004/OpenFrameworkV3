using System;
using System.Linq;
using System.Web.UI;
using OpenFrameworkV3;

public partial class ItemList : Page
{
    /// <summary>Master page</summary>
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

    public string InstanceName
    {
        get
        {
            return this.master.Instance.Name;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.master = this.Master as Main;
        this.ItemName = this.master.CodedQuery.GetByKey<string>("Item");
        this.ListId = this.master.CodedQuery.GetByKey<string>("List");

        var itemDefinition = Persistence.ItemDefinitionByName(this.ItemName, this.master.Instance.Name);
        this.ItemDefinitionId = itemDefinition.Id;
        var listDefinition = itemDefinition.Lists.First(l => l.Id.Equals(this.ListId, StringComparison.OrdinalIgnoreCase));
        var title = listDefinition.Title ?? itemDefinition.Layout.LabelPlural;
        this.master.BreadCrumb.AddLeaf(title);
        this.master.SetTitle(this.master.Company.Name + " - " + title);
        this.master.SetPageType("PageList");
    }
}