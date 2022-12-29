namespace OpenFrameworkV3.Web.Admin.Company.Features
{
    using System;
    using System.Web.UI;
    using OpenFramework.Core.Companies;
    using OpenFrameworkV3.Core;

    public partial class Documents : Page
    {
        /// <summary>Master page</summary>
        private Main master;

        public CompanyConfig CompanyConfig { get; private set; }

        public void Page_Init(object o, EventArgs e)
        {
            Instance.CheckPersistence();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddEncryptedLink("Configuració de companyia", "/Admin/Company/");
            this.master.BreadCrumb.AddLeaf("Gestió documental");
            this.master.BreadCrumb.SetTitle("Companyia: ");

            this.master.AddScript("/vendor/jquery-flot/jquery.flot.js");
            this.master.AddScript("/vendor/jquery-flot/jquery.flot.resize.js");
            this.master.AddScript("/vendor/jquery-flot/jquery.flot.pie.js");
            this.master.SetPageType("pageAdmin");

            this.CompanyConfig = CompanyConfig.Get(this.master.CompanyId, this.master.InstanceName);
        }
    }
}