namespace OpenFrameworkV3.Web.Admin.Security
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web.UI;

    public partial class Group : Page
    {
        /// <summary>Master page</summary>
        public Main master;

        public OpenFrameworkV3.Core.Security.Group GroupData { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddEncryptedLink("Seguretat", "/Admin/Security/Default.aspx");
            this.master.BreadCrumb.AddEncryptedLink("Grups", "/Admin/Security/GroupList.aspx");
            this.master.BreadCrumb.AddLeaf("Grups");
            this.master.BreadCrumb.SetTitle("Seguretat: ");
            this.master.SetPageType("pageAdmin");
            this.master.AddScript("/Admin/Security/Group.js");

            var groupId = this.master.CodedQuery.GetByKey<long>("G");

            this.GroupData = OpenFrameworkV3.Core.Security.Group.ById(groupId, this.master.CompanyId, this.master.InstanceName);
            this.GetUsers();
        }

        private void GetUsers()
        {
            var users = OpenFrameworkV3.Core.Security.ApplicationUser.All(1, this.master.InstanceName);
            var res = new StringBuilder();
            foreach (var user in users)
            {
                var selected = this.GroupData.Membership.Any(g => g == user.Id);

                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<option value=""{0}""{2}{3}>{1}</option>",
                    user.Id,
                    user.Profile.FullName,
                    selected || user.Core ? " selected=\"selected\"" : string.Empty,
                    user.Core ? " disabled=\"disabled\"" : string.Empty);
            }

            this.LtUsers.Text = res.ToString();
        }
    }
}