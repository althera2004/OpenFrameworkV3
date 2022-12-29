// --------------------------------
// <copyright file="Services.aspx.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Web.Admin.Company
{
    using System;
    using System.Web.UI;

    public partial class Services : Page
    {
        /// <summary>Master page</summary>
        private Main master;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddEncryptedLink("Configuració de companyia", "/Admin/Company/");
            this.master.BreadCrumb.AddLeaf("Servicios contratados");
            this.master.BreadCrumb.SetTitle("Companyia: ");

            this.master.AddScript("/vendor/jquery-flot/jquery.flot.js");
            this.master.AddScript("/vendor/jquery-flot/jquery.flot.resize.js");
            this.master.AddScript("/vendor/jquery-flot/jquery.flot.pie.js");
            this.master.AddScript("/Admin/Company/Services.js");

            this.master.SetPageType("pageAdmin");
        }
    }
}