using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Instances_SUPPORT_Pages_Form : Page
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
        this.master.BreadCrumb.Add("Incidències");
        this.master.BreadCrumb.Add("Weke weke 0001");
        this.master.SetTitle("Incidències");
        this.master.SetPageType("pageForm");
    }
}