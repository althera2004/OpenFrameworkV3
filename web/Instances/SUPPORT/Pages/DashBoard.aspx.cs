namespace OpenFrameworkV3.Web.Instances.Support
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class DashBoard : Page
    {
        private Main master;

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


            var title = "Dashboard";
            this.master.SetTitle(this.master.Company.Name + " - " + title);
            this.master.SetPageType("PageView");
            this.master.SetPageType("Dashboard");
        }
    }
}