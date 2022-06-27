namespace OpenFrameworkV2.Web.Admin.Company
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddEncryptedLink("Configuració de companyia", "/Admin/Company/");
            this.master.BreadCrumb.AddLeaf("Correos electrónicos");
            this.master.BreadCrumb.SetTitle("Companyia: " + this.master.Instance.Name);

            this.master.AddScript("/Admin/Company/MailBoxes.js");

            var mailBoxes = MailBox.ByCompanyId(this.master.Company.Id, this.master.Instance.Name);

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