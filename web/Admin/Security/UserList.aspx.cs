namespace OpenFrameworkV3.Web.Admin.Security
{
    using OpenFrameworkV3.Core.Security;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web.UI;

    public partial class UserList : Page
    {
        /// <summary>Master page</summary>
        public Main master;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddEncryptedLink("Seguretat", "/Admin/Security/Default.aspx");
            this.master.BreadCrumb.AddLeaf("Usuaris");
            this.master.BreadCrumb.SetTitle("Seguretat: ");
            this.master.SetPageType("pageAdmin");
            this.GetUsers();
        }

        private void GetUsers()
        {
            var res = new StringBuilder();
            var users = ApplicationUser.All(this.master.CompanyId, this.master.InstanceName);
            foreach(var user in users.OrderByDescending(u=>u.Core))
            {
                var icon1 = user.Core? "<i class=\"fa fa-check green\"></i>" : string.Empty;
                var icon2 = user.AdminUser ? "<i class=\"fa fa-check green\"></i>" : string.Empty;

                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<tr id=""{0}"" onclick=""GoUserView(this.id);"" class=""securitytablerow""><td style=""width:300px;"">{1}</td><td>{2}</td><td style=""width:120px;"">{3}</td><td style=""width:120px;text-align:center;"">{4}</td><td style=""width:120px;text-align:center;"">{5}</td></tr>",
                    user.Id,
                    user.Profile.FullName,
                    user.Email,
                    user.Language.Name,
                    icon1,
                    icon2);
            }

            this.LtListData.Text = res.ToString();
            this.LtListCount.Text = users.Count.ToString();
        }
    }
}