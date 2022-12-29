// --------------------------------
// <copyright file="Group.aspx.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Web.Admin.Security
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using OpenFrameworkV3.Core.Security;

    public partial class Group : Page
    {
        /// <summary>Master page</summary>
        public Main master;

        public SecurityGroup GroupData { get; private set; }

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

            this.GroupData = SecurityGroup.ById(groupId, this.master.CompanyId, this.master.InstanceName);
            this.GetUsers();
            this.RenderAccessGrants();
        }

        private void GetUsers()
        {
            var users = ApplicationUser.All(this.master.CompanyId, this.master.InstanceName);
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

        private void RenderAccessGrants()
        {
            var res = new StringBuilder();
            foreach (var itemDefinition in this.master.Instance.ItemDefinitions.OrderBy(d => d.Layout.Label))
            {
                res.AppendFormat(CultureInfo.InvariantCulture,@"<tr class=""GrantRow"" id=""Item_{0}"">", itemDefinition.Id);
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<td style=""width:30px;""><i class=""{0} blue""></i></td><td>{1}</td>",
                    itemDefinition.Layout.Icon,
                    itemDefinition.Layout.Label);

                var r = false;
                var w = false;
                var d = false;
                if (this.GroupData.Grants.Any(g => g.ItemId == itemDefinition.Id))
                {
                    var grant = this.GroupData.Grants.First(g => g.ItemId == itemDefinition.Id);
                    r = grant.Grants.Contains("R");
                    w = grant.Grants.Contains("W");
                    d = grant.Grants.Contains("D");
                }

                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"
                        <td style=""text-align:center;width:100px;""><input type=""checkbox"" id=""chk_{0}_R"" {1} onclick=""SECURITYGROUP_GrantChanged('R',{0});"" /></td>
                        <td style=""text-align:center;width:100px;""><input type=""checkbox"" id=""chk_{0}_W"" {2} onclick=""SECURITYGROUP_GrantChanged('W',{0});""  /></td>
                        <td style=""text-align:center;width:100px;""><input type=""checkbox"" id=""chk_{0}_D"" {3} onclick=""SECURITYGROUP_GrantChanged('D',{0});""  /></td>",
                    itemDefinition.Id,
                    r ? "checked=\"checked\"" : string.Empty,
                    w ? "checked=\"checked\"" : string.Empty,
                    d ? "checked=\"checked\"" : string.Empty);
                res.Append("</tr>");
            }

            this.LtAccessGrants.Text = res.ToString();
        }
    }
}