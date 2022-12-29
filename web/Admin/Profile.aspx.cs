using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Profile : Page
{
    private Main master;

    public string InstanceName
    {
        get
        {
            return this.master.InstanceName;
        }
    }

    public string Translate(string text)
    {
        return this.master.Translate(text);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.master = this.Master as Main;
        this.master.SetTitle("Perfil d'usuari");
        this.master.BreadCrumb.AddLeaf("Usuari");
        this.master.SetPageType("Profile");
    }
}