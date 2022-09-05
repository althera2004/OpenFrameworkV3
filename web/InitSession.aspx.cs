using System;
using System.Globalization;
using System.Text;
using System.Web.UI;
using OpenFrameworkV3;
using OpenFrameworkV3.Core;
using OpenFrameworkV3.Core.Companies;
using OpenFrameworkV3.Core.Navigation;
using OpenFrameworkV3.Core.Security;

public partial class InitSession : Page
{
    /// <summary>Gets actual instance</summary>
    public Instance Instance { get; private set; }

    public Company Company { get; private set; }

    public ApplicationUser ApplicationUser { get; private set; }

    public string MenuJson { get; private set; }

    /// <summary>Encoded query string</summary>
    public CodedQuery CodedQuery { get; private set; }

    public string Language
    {
        get
        {
            return this.ApplicationUser.Language.Iso;
        }
    }

    public string ItemDefinitions
    {
        get
        {
            return Instance.JsonDefinitions(this.Instance.Name);
        }
    }

    public string Dictionary { get; private set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.CodedQuery = new CodedQuery(this.Request.QueryString);
        var instanceName = this.CodedQuery.GetByKey<string>("I");
        var companyId = this.CodedQuery.GetByKey<long>("C");
        var userId = this.CodedQuery.GetByKey<long>("U");
        var languageId = this.CodedQuery.GetByKey<string>("L");
        this.Instance = Persistence.InstanceByName(instanceName);


        if (companyId == 0)
        {
            this.Company = Company.Empty;
            var companies = Company.ByUser(userId, instanceName);

            if (companies.Count == 1)
            {
                this.Company = Company.ById(companies[0].Id, instanceName);
            }
            else
            {
                var res = new StringBuilder();
                foreach (var company in companies)
                {
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        company.Name);
                }

                this.LtCompanies.Text = res.ToString();
            }
        }
        else
        {
            this.Company = Company.ById(companyId, instanceName);
        }

        this.ApplicationUser = ApplicationUser.ById(userId, companyId, instanceName);
        Persistence.DictionaryCheck(this.ApplicationUser.Language.Iso, instanceName);

        if (this.Company.Id > 0)
        {
            var menu = Menu.Load(this.ApplicationUser,this.Company.Id, false, instanceName);
            this.MenuJson = menu.GetJson();
        }
        else
        {
            this.MenuJson = "[]";
        }
    }
}