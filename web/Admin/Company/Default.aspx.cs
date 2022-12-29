namespace OpenFrameworkV3.Web.Admin.Company
{
    using System;
    using System.Web.UI;

    public partial class Default : Page
    {
        /// <summary>Master page</summary>
        private Main master;

        public Main MasterPage
        {
            get
            {
                return this.master;
            }
        }

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
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddLeaf("Configuració de companyia");
            this.master.BreadCrumb.SetTitle("Companyia: ");
            this.master.SetPageType("pageAdmin");
        }
    }
}