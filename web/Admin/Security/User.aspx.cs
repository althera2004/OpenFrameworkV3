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
            this.master.BreadCrumb.AddEncryptedLink(this.master.Translate("Core_Security"), "/Admin/Security/Default.aspx");
            this.master.BreadCrumb.AddEncryptedLink(this.master.Translate("Core_Security_ApplicationUsers"), "/Admin/Security/UserList.aspx");
            this.master.BreadCrumb.AddLeaf("");
            this.master.BreadCrumb.SetTitle(this.master.Translate("Core_Security_ApplicationUsers"));
            this.master.SetPageType("pageAdmin");
            this.master.AddScript("/Admin/Security/User.js");

            var userId = this.master.CodedQuery.GetByKey<long>("U");

            this.UserData = ApplicationUser.ById(userId,this.master.CompanyId, this.master.InstanceName);
            this.GetGroups();
        }

        private void GetGroups()
        {
            var groups = OpenFrameworkV3.Core.Security.SecurityGroup.All(1, this.master.InstanceName);
            var res = new StringBuilder();
            foreach (var group in groups)
            {
                var selected = this.UserData.Groups.Any(g=> g.Id == group.Id);

                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<option value=""{0}""{2}{3}>{1}</option>",
                    group.Id,
                    group.Name,
                    selected ? " selected=\"selected\"" : string.Empty,
                    group.Core ? " disabled=\"disabled\"" : string.Empty);
            }

            this.LtGroups.Text = res.ToString();
        }
    }
}