﻿namespace OpenFrameworkV3.Web.Admin.Company
{
    using System;
    using System.Web.UI;

    public partial class CompanyProfile : Page
    {
        /// <summary>Master page</summary>
        private Main master;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddEncryptedLink("Configuració de companyia", "/Admin/Company/");
            this.master.BreadCrumb.AddLeaf("Ficha de la companía");
            this.master.BreadCrumb.SetTitle("Companyia: ");
        }
    }
}