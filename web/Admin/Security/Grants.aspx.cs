namespace OpenFrameworkV3.Web.Admin.Security
{
    using System;
    using System.Web.UI;

    public partial class Grants : Page
    {
        /// <summary>Master page</summary>
        private Main master;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddEncryptedLink("Seguretat", "/Admin/Security/Default.aspx");
            this.master.BreadCrumb.AddLeaf("Permissos");
            this.master.BreadCrumb.SetTitle("Seguretat: ");

            this.master.AddScript("/Admin/Security/Grants.js");
        }
    }
}