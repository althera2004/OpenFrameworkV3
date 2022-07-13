namespace OpenFrameworkV3.Web.Admin.Security
{
    using OpenFrameworkV3.Core.Security;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web.UI;

    public partial class User : Page
    {
        /// <summary>Master page</summary>
        public Main master;

        public ApplicationUser UserData { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddEncryptedLink("Seguretat", "/Admin/Security/Default.aspx");
            this.master.BreadCrumb.AddEncryptedLink("Usuaris", "/Admin/Security/UserList.aspx");
            this.master.BreadCrumb.AddLeaf("Usuaris");
            this.master.BreadCrumb.SetTitle("Seguretat: ");
            this.master.SetPageType("pageAdmin");
            this.master.AddScript("/Admin/Security/User.js");

            var userId = this.master.CodedQuery.GetByKey<long>("U");

            this.UserData = ApplicationUser.ById(userId, this.master.InstanceName);
        }
    }
}