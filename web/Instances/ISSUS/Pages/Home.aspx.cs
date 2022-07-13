using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Instances_SUPPORT_Pages_Home : Page
{
    private Main master;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.master = this.Master as Main;
        this.master.SetPageType("form");
    }
}