using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Instances_SUPPORT_Pages_EmptyList : Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        this.master = this.Master as Main;
        this.master.BreadCrumb.Add("Tasques");
        this.master.BreadCrumb.AddLeaf("Incidències");
        this.master.SetPageType("pageList");
    }
}