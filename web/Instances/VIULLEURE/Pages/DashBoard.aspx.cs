namespace OpenFramework.Instance.ViuLleure
{
	using System;
	using System.Web.UI;
    using OpenFrameworkV2;
    using OpenFrameworkV2.Core.Navigation;
    using OpenFrameworkV2.Core.Security;

    public partial class DashBoard : Page
	{
        /// <summary>Master page</summary>
        private Main master;

		public string ListData { get; private set; }

		public string ListDefinition { get; private set; }

		public string TableTitle { get; private set; }

		public ApplicationUser ActualUser { get; private set; }

		/// <summary>Page's load event</summary>
		/// <param name="sender">Page loaded</param>
		/// <param name="e">Arguments of event</param>
		protected void Page_Load(object sender, EventArgs e)
		{
			this.master = this.Master as Main;
			this.master.BreadCrumb.AddLeaf("Inicio");
			this.master.BreadCrumb.SetTitle("Inicio");
		}
	}
}