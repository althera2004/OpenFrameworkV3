namespace OpenFrameworkV3.Web.Admin.Security
{
    using System;
    using System.Web.UI;

    public partial class General : Page
    {
        /// <summary>Master page</summary>
        private Main master;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddEncryptedLink("Seguridad", "/Admin/Security/Default.aspx");
            this.master.BreadCrumb.AddLeaf("Configuración general");
            this.master.BreadCrumb.SetTitle("Seguridad: " + this.master.Company.Name);

            this.master.AddScript("/Admin/Security/General.js");
        }
    }
}