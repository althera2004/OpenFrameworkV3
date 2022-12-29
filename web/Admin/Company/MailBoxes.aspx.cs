namespace OpenFrameworkV3.Web.Admin.Company
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using OpenFrameworkV3.Mail;

    public partial class MailBoxes : Page
    {
        /// <summary>Master page</summary>
        private Main master;

        public MailBox Main { get; private set; }

        public MailBox ThirdParty { get; private set; }

        public string Translate(string text)
        {
            return this.master.Translate(text);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add(Translate("Common_Administration"));
            this.master.BreadCrumb.AddEncryptedLink(Translate("Core_Admin_Company"), "/Admin/Company/");
            this.master.BreadCrumb.AddLeaf(Translate("Core_MailBoxes"));
            this.master.BreadCrumb.SetTitle("Companyia: " + this.master.InstanceName);
            this.master.SetPageType("pageAdmin");

            this.master.AddScript("/Admin/Company/MailBoxes.js");

            var mailBoxes = MailBox.ByCompanyId(this.master.CompanyId, this.master.InstanceName);

            if(mailBoxes.Any(m => m.Main == true))
            {
                this.Main = mailBoxes.First(m => m.Main == true);
            }
            else
            {
                this.Main = MailBox.Empty;
            }

            if(mailBoxes.Any(m => m.Main == false))
            {
                this.ThirdParty = mailBoxes.First(m => m.Main == false);
            }
            else
            {
                this.ThirdParty = MailBox.Empty;
            }
        }
    }
}