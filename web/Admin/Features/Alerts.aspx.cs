namespace OpenFrameworkV3.Web.Admin.Feature
{
    using System;
    using System.Web.UI;

    public partial class Alerts : Page
    {
        /// <summary>Master page</summary>
        private Main master;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Admin");
            this.master.BreadCrumb.AddLeaf("Alertas");
            this.master.BreadCrumb.SetTitle("Companyia: ");

            this.master.AddScript("/vendor/fooTable/dist/footable.all.min.js");
        }
    }
}