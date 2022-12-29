using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI;
using OpenFrameworkV3;
using OpenFrameworkV3.Core;
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
        this.title = string.Format(CultureInfo.InvariantCulture, "{0}-{1}", this.InstanceName, title);
    }

    public void SetPageType(string pageType)
    {
        this.pageType = pageType;
    }
    
    /// <summary>Encoded query string</summary>
    public CodedQuery CodedQuery { get; private set; }

    public string Language { get; private set; }

    private List<string> scripts;
    private List<string> FKscripts;

    /// <summary>Gets actual instance</summary>
    public string InstanceName { get; private set; }

    public Instance Instance { get; private set; }

    public long CompanyId { get; private set; }

    /// <summary>Gets de breadcrumb elements</summary>
    public BreadCrumb BreadCrumb { get; private set; }

    public string QueryBase { get; private set; }

    public string FK { get; set; }

    protected void Page_Init()
    {
        Instance.CheckPersistence();
        this.CodedQuery = new CodedQuery(this.Request.QueryString);
        this.InstanceName = this.CodedQuery.GetByKey<string>("I");
        this.CompanyId = this.CodedQuery.GetByKey<long>("C");
        this.Language = this.CodedQuery.GetByKey<string>("L");
        this.Instance = Persistence.InstanceByName(this.InstanceName);
        this.BreadCrumb = new BreadCrumb()
        {
            InstanceName = this.InstanceName,
            Language = this.Language
        };
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

    public void AddFKScript(string scriptFile)
    {
        if (this.FKscripts == null)
        {
            this.FKscripts = new List<string>();
        }

        this.FKscripts.Add(scriptFile);
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
    public string FKScripts
    {
        get
        {
            var res = new StringBuilder();
            if (this.FKscripts != null && this.FKscripts.Count > 0)
            {
                foreach (var script in this.FKscripts)
                {
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"{1}{2}<script type=""text/javascript"" src=""/Instances/" + this.InstanceName + "/Scripts/" + script + "_FK.js\"></script>",
                        script,
                        Environment.NewLine,
                        "\t\t",
                        string.Empty); // weke Basics.AntiCache);
                }
            }

            return res.ToString();
        }
    }

    public string Translate(string key)
    {
        return OpenFrameworkV3.ApplicationDictionary.Translate(key, this.Language, this.InstanceName);
    }
}
