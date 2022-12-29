namespace OpenFrameworkV3.Web.Admin.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using OpenFrameworkV3.Core.Companies;

    public partial class Default : Page
    {
        /// <summary>Master page</summary>
        public Main master;

        public Main MasterPage
        {
            get
            {
                return this.master;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add(this.MasterPage.Translate("Common_Administration"));
            this.master.BreadCrumb.AddLeaf(this.MasterPage.Translate("Core_Security"));
            this.master.BreadCrumb.SetTitle(this.MasterPage.Translate("Core_Security"));
            this.master.SetPageType("pageAdmin");

            var securityConfig = Company.ById(this.master.CompanyId, this.master.InstanceName);
        }
    }
}