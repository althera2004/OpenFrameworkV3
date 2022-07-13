namespace OpenFrameworkV3.Web.Admin.Instance
{
    using System;
    using System.Web.UI;

    public partial class Default : Page
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
            this.master.BreadCrumb.AddLeaf("Instància: ");
            this.master.SetPageType("pageInstance");

            this.master.AddScript("/Admin/Instance/Instance.js");
        }
    }
}