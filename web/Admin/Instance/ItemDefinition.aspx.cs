namespace OpenFrameworkV3.Web.Admin.Instance
{
    using System;
    using System.Web.UI;

    public partial class ItemDefinition : Page
    {
        /// <summary>Master page</summary>
        private Main master;

        public long ItemDefinitionId { get; private set; }

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
            this.master.BreadCrumb.AddLeaf("Ítem: ");
            this.master.SetPageType("pageInstance");

            this.ItemDefinitionId = this.master.CodedQuery.GetByKey<long>("Id");

            this.master.AddScript("/Admin/Instance/ItemDefinition.js");
        }
    }
}