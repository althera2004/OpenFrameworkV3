namespace OpenFrameworkV3.Web.Instances.GERMAN.Pages 
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class Dashboard : System.Web.UI.Page
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
            this.master.SetPageType("PageView");
            this.master.SetPageType("Dashboard");
        }
    }
}