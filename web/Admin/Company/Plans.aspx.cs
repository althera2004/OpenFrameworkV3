namespace OpenFrameworkV3.Web.Admin.Company
{
    using System;
    using System.Web.UI;

    public partial class Plans : Page
    {
        /// <summary>Master page</summary>
        private Main master;

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
            this.master.BreadCrumb.AddLeaf("Configuración de companía");
            this.master.BreadCrumb.SetTitle("Companyia: ");
        }
    }
}