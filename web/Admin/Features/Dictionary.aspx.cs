namespace OpenFrameworkV3.Web.Admin.Feature
{
    using System;
    using System.Web.UI;

    public partial class Dictionary :Page
    {
        /// <summary>Master page</summary>
        private Main master;

        public string Corpus { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddEncryptedLink("Funcionalitats", "/Admin/Security/Default.aspx");
            this.master.BreadCrumb.AddLeaf("Diccionaris");
            this.master.BreadCrumb.SetTitle("diccionaris: " );
            this.master.SetPageType("view");

            this.master.AddScript("/Admin/Features/Dictionary.js");

            this.GetCorpus();
        }

        private void GetCorpus()
        {
            this.Corpus = ApplicationDictionary.GetCorpus("ca-es", "support");
        }
    }
}