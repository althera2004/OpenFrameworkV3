namespace OpenFrameworkV2.Web.Support.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class DashBoard : Page
    {
        /// <summary>Master page</summary>
        private Main master;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Inici");
            this.master.BreadCrumb.SetTitle("Inici: " + this.master.Company.Name);

            this.master.AddScript("/Instances/Support/Pages/DashBoard.js");
        }
    }
}