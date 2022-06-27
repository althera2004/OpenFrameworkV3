namespace OpenFrameworkV2.Web.Admin.Security
{
    using System;
    using System.Web.UI;

    public partial class Default : Page
    {
        /// <summary>Master page</summary>
        public Main master;

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
            this.master.BreadCrumb.AddLeaf("Seguridad");
            this.master.BreadCrumb.SetTitle("Seguridad: " + this.master.Company.Name);
        }
    }
}