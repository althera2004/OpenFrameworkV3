using System;
using System.Globalization;
using OpenFrameworkV3;
using OpenFrameworkV3.Tools;
using OpenFrameworkV3.Core;
using OpenFrameworkV3.Core.Companies;
using OpenFrameworkV3.Core.Security;

public partial class FakeInint : System.Web.UI.Page
{
    public Instance Instance { get; private set; }

    public ApplicationUser ActualUser { get; private set; }

    public Company Company { get; private set; }

    public string ItemDefinitions
    {
        get
        {
            return OpenFrameworkV3.Core.Instance.JsonDefinitions(this.Instance.Name);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Instance = Instance.Empty;
            this.ActualUser = ApplicationUser.OpenFramework;
            this.Company = Company.Empty;
        }
    }

    public string Query(string instanceName, long companyId, string extra)
    {
        var encodingQuery = string.Format(
                   CultureInfo.InvariantCulture,
                   "I={0}&C={1}{2}",
                   instanceName,
                   companyId,
                   extra);

        return Basics.Base64Encode(encodingQuery);
    }

    protected void LnkReload_Click(object sender, EventArgs e)
    {
        Instance.CheckPersistence();
        this.Instance = Persistence.InstanceByName(this.TxtInstanceName.Text);
        this.ActualUser = ApplicationUser.OpenFramework;
        this.Company = Company.ById(1, this.Instance.Name);

        this.LtCns.Text = Persistence.ConnectionString(this.TxtInstanceName.Text);

        this.LtList.Text = Instance.Path.Scripts(this.TxtInstanceName.Text) + "<bR>" + Basics.PathToUrl( Instance.Path.Scripts(this.TxtInstanceName.Text)) + "/FixedList.js";
        this.LtList.Text += "<br />";
        this.LtList.Text += "<a href=\"ItemList.aspx?" + Query(this.Instance.Name, this.Company.Id, "&Item=Incidencia&List=Datos") + "\">Datos</a>";
        this.LtList.Text += "<br />";
        this.LtList.Text += "<a href=\"ItemList.aspx?" + Query(this.Instance.Name, this.Company.Id, "&Item=Incidencia&List=NewDevelopment") + "\">Nuevo</a>";

        var pass = "root";
        var enPass = Encrypt.EncryptString(Basics.Reverse(pass), "OpenFramework");
        var dePass = Encrypt.DecryptString(enPass, "OpenFramework");

        this.LtLink.Text += "<hr>" + Basics.Reverse(dePass) + "   -->   " + enPass;


    }
}