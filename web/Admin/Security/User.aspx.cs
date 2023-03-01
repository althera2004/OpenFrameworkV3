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
            this.RenderAccessGrants();
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

        private void RenderAccessGrants()
        {
            var res = new StringBuilder();
            foreach (var itemDefinition in this.master.Instance.ItemDefinitions.OrderBy(d => d.Layout.Label))
            {
                res.AppendFormat(CultureInfo.InvariantCulture, @"<tr class=""GrantRow"" id=""Item_{0}"">", itemDefinition.Id);
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<td style=""width:30px;""><i class=""{0} blue""></i></td><td>{1}</td>",
                    itemDefinition.Layout.Icon,
                    itemDefinition.Layout.Label);

                var r = false;
                var w = false;
                var d = false;
                if (this.UserData.Grants.Any(g => g.ItemId == itemDefinition.Id))
                {
                    var grant = this.UserData.Grants.First(g => g.ItemId == itemDefinition.Id);
                    r = grant.Grants.Contains("R");
                    w = grant.Grants.Contains("W");
                    d = grant.Grants.Contains("D");
                }

                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"
                        <td style=""text-align:center;width:100px;""><i class=""fa {0}""></td>
                        <td style=""text-align:center;width:100px;""><i class=""fa {1}""></td>
                        <td style=""text-align:center;width:100px;""><i class=""fa {2}""></td>",
                    r ? "fa-check green" : "fa-ban red",
                    w ? "fa-check green" : "fa-ban red",
                    d ? "fa-check green" : "fa-ban red");
                res.Append("</tr>");
            }

            this.LtAccessGrants.Text = res.ToString();
        }
    }
}