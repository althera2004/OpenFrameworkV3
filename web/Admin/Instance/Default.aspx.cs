namespace OpenFrameworkV3.Web.Admin.Instance
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web.UI;
    using OpenFrameworkV3.Core;

    public partial class Default : Page
    {
        /// <summary>Master page</summary>
        private Main master;

        public string QueryBase
        {
            get
            {
                return this.master.QueryBase;
            }
        }

        public string ScriptFiles { get; private set; }

        public string InstanceCommonScript { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;
            this.master.BreadCrumb.Add("Administració");
            this.master.BreadCrumb.AddLeaf("Instància: ");
            this.master.SetPageType("pageInstance");
            this.master.AddScript("/Admin/Instance/Instance.js");
            this.GetScripts();
            this.GetInstanceCommonScript();
        }

        private void GetScripts()
        {
            var res = new StringBuilder("[");
            var path = Instance.Path.Scripts(this.master.InstanceName);
            var fileEntries = Directory.GetFiles(path);
            var first = true;
            foreach(var file in fileEntries)
            {
                res.AppendFormat(CultureInfo.InvariantCulture, @"{1}""{0}""", Path.GetFileName(file), first ? string.Empty : ",");
                first = false;
            }

            res.Append("]");
            this.ScriptFiles = res.ToString();
        }
        private void GetInstanceCommonScript()
        {
            var path = string.Format(CultureInfo.InvariantCulture, @"{0}\InstanceCommon.js", Instance.Path.Scripts(this.master.InstanceName));
            if (File.Exists(path))
            {
                using (var input = new StreamReader(path))
                {
                    this.InstanceCommonScript = input.ReadToEnd();
                }
            }
            else
            {
                this.InstanceCommonScript = "// No existeix el fitxer: " + path;
            }
        }
    }
}