// --------------------------------
// <copyright file="DashBoard.aspx.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Web.Instances.Support
{
    using System;
    using System.Web.UI;

    public partial class DashBoard : Page
    {
        private Main master;

        public string InstanceName
        {
            get
            {
                return this.master.InstanceName;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;


            var title = "Dashboard";
            this.master.SetTitle(title);
            this.master.SetPageType("PageView");
            this.master.SetPageType("Dashboard");
        }
    }
}