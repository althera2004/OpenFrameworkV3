namespace OpenFrameworkV3.Web.Instances.Support
{
    using System;
    using System.Web.UI;

    public partial class DashBoard : Page
    {
        private Main master;

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


            var title = "Dashboard";
            this.master.SetTitle(title);
            this.master.SetPageType("PageView");
            this.master.SetPageType("Dashboard");
        }
    }
}