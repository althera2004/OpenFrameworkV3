namespace OpenFrameworkV3.Web.Admin.Security
{
    using OpenFrameworkV3.Core.Security;
    using System;
    using System.Globalization;
    using System.Text;
    using System.Web.UI;

    public partial class GroupList : Page
    {
        /// <summary>Master page</summary>
        public Main master;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddEncryptedLink("Seguretat", "/Admin/Security/Default.aspx");
            this.master.BreadCrumb.AddLeaf("Grups");
            this.master.BreadCrumb.SetTitle("Seguretat: ");
            this.master.SetPageType("pageAdmin");
            this.GetGroups();
        }

        private void GetGroups()
        {
            var groups = OpenFrameworkV3.Core.Security.Group.All(this.master.CompanyId, this.master.InstanceName);
            var res = new StringBuilder();
            foreach(var group in groups)
            {
                var icon1 = group.Core ? "<i class=\"fa fa-check green\"></i>" : "<i class=\"fa fa-ban red\"></i>";
                var icon2 = group.Deletable ? "<i class=\"fa fa-check green\"></i>" : "<i class=\"fa fa-ban red\"></i>";

                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<tr id=""{0}"" class=""securitytablerow"" onclick=""GoGroupView({0})""><td style=""width:300px;"">{1}</td><td>{2}</td><td style=""width:120px;text-align:center;"">{3}</td><td style=""width:120px;text-align:center;"">{4}</td></tr>",
                    group.Id,
                    group.Name,
                    group.Description,
                    icon1 ,
                    icon2);
            }

            this.LTListData.Text = res.ToString();
            this.LtListCount.Text = groups.Count.ToString();
        }
    }
}