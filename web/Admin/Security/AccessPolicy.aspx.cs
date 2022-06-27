namespace OpenFrameworkV3.Web.Admin.Security
{
    using System;
    using System.Web.UI;

    public partial class AccessPolicy : Page
    {
        /// <summary>Master page</summary>
        private Main master;

        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddEncryptedLink("Seguridad", "/Admin/Security/");
            this.master.BreadCrumb.Add("Política de acceso");
            this.master.BreadCrumb.SetTitle("Seguridad: " + this.master.Company.Name);

            //Policy = AccessPolicy(this.master.Company.Id, this.master.Instance.Name);
        }
    }
}