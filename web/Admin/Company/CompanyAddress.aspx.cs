namespace OpenFrameworkV3.Web.Admin.Company
{
    using System;
    using System.Text;
    using System.Web.UI;
    using OpenFrameworkV3.Core.Companies;

    public partial class CompanyAddress : Page
    {
        /// <summary>Master page</summary>
        private Main master;

        public string CompanyAddresses { get; private set; }

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
            this.master.BreadCrumb.AddEncryptedLink("Configuració de companyia", "/Admin/Company/");
            this.master.BreadCrumb.AddLeaf("Adreces");
            this.master.BreadCrumb.SetTitle("Companyia: ");
            this.master.AddScript("/Admin/Company/CompanyAddress.js");

            this.GetCompanyAddresses();
        }

        private void GetCompanyAddresses()
        {
            this.CompanyAddresses = CompanyPostalAddress.JsonList(CompanyPostalAddress.ByCompany(this.master.CompanyId, this.master.InstanceName));
        }
    }
}