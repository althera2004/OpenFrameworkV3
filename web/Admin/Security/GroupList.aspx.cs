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

        public string QueryBase
        {
            get
            {
                return this.master.QueryBase;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddEncryptedLink("Seguridad", "/Admin/Security/Default.aspx");
            this.master.BreadCrumb.AddLeaf("Grupos");
            this.master.BreadCrumb.SetTitle("Seguridad: " + this.master.Company.Name);
            this.GetGroups();
        }

        private void GetGroups()
        {
            var groups = Group.All(this.master.Company.Id, this.master.Instance.Name);
            var res = new StringBuilder();
            foreach(var group in groups)
            {
                var icon1 = group.Core ? "<i class=\"fa fa-check success\"></i>" : "<i class=\"fa fa-ban green\"></i>";
                var icon2 = group.Deletable ? "<i class=\"fa fa-check green\"></i>" : "<i class=\"fa fa-ban green\"></i>";

                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<tr id=""{0}""><td style=""width:300px;"">{1}</td><td>{2}</td><td style=""width:120px;"">{3}</td><td style=""width:120px;"">{4}</td style=""width:77px;""><td></td></tr>",
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