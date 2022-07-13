using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenFrameworkV3;
using OpenFrameworkV3.Core;
using OpenFrameworkV3.Core.Companies;
using OpenFrameworkV3.Core.Navigation;

public partial class Main : MasterPage
{
    public string MainAceStyle
    {
        get
        {
            return "id=\"main-ace-style\"";
        }
    }

    private string pageType;

    public string PageType
    {
        get
        {
            return this.pageType ?? "PageCustom";
        }
    }

    private string title;

    public string Title { get { return this.title; } } 

    public void SetTitle(string title)
    {
        this.title = title;
    }

    public void SetPageType(string pageType)
    {
        this.pageType = pageType;
    }
    
    /// <summary>Encoded query string</summary>
    public CodedQuery CodedQuery { get; private set; }

    private List<string> scripts;

    /// <summary>Gets actual instance</summary>
    public string InstanceName { get; private set; }

    public long CompanyId { get; private set; }

    /// <summary>Gets de breadcrumb elements</summary>
    public BreadCrumb BreadCrumb { get; private set; }

    public string QueryBase { get; private set; }

    public string FK { get; set; }

    protected void Page_Init()
    {
        Instance.CheckPersistence();
        this.BreadCrumb = new BreadCrumb();
        this.CodedQuery = new CodedQuery(this.Request.QueryString);
        this.InstanceName = this.CodedQuery.GetByKey<string>("I");
        this.CompanyId = this.CodedQuery.GetByKey<long>("C");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public void AddScript(string scriptFile)
    {
        if (this.scripts == null)
        {
            this.scripts = new List<string>();
        }

        this.scripts.Add(scriptFile);
    }
    public string Scripts
    {
        get
        {
            var res = new StringBuilder();
            if (this.scripts != null && this.scripts.Count > 0)
            {
                foreach (var script in this.scripts)
                {
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"{1}{2}<script type=""text/javascript"" src=""{0}?{3}""></script>",
                        script,
                        Environment.NewLine,
                        "\t\t",
                        string.Empty); // weke Basics.AntiCache);
                }
            }

            return res.ToString();
        }
    }
}
