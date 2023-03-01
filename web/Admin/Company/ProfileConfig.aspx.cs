namespace OpenFrameworkV3.Web.Admin.Company
{
    using System;
    using System.Web.UI;
    using OpenFrameworkV3.Core.Configuration;

    public partial class ProfileConfig : Page
    {
        /// <summary>Master page</summary>
        private Main master;

        public string Translate(string text)
        {
            return this.master.Translate(text);
        }

        public UserProfileConfig UserProfile { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add(Translate("Common_Administration"));
            this.master.BreadCrumb.AddEncryptedLink(Translate("Core_Admin_Company"), "/Admin/Company/");
            this.master.BreadCrumb.AddLeaf(Translate("Common_UserProfile"));
            this.master.BreadCrumb.SetTitle("Companyia: " + this.master.InstanceName);
            this.master.SetPageType("pageAdmin");

            this.master.AddScript("/Admin/Company/ProfileConfig.js");

            this.UserProfile = UserProfileConfig.ByCompany(this.master.CompanyId, this.master.InstanceName);
        }
    }
}