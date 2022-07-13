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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddLeaf("Seguretat");
            this.master.BreadCrumb.SetTitle("Seguretat: ");
            this.master.SetPageType("pageAdmin");

            var securityConfig = Company.ById(this.master.CompanyId, this.master.InstanceName);
        }
    }
}