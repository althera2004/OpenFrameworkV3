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

public partial class Check : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GetListInstances();
    }

    protected void BtnLoadInstance_Click(object sender, EventArgs e)
    {
        var instance = Instance.LoadDefinition(this.TxtInstanceName.Text, true);
        Persistence.AddInstance(instance);
        GetListInstances();
    }

    private void GetListInstances()
    {
        var instances = Persistence.ListOfInstances;
        var res = new StringBuilder();
        var first = true;
        foreach (var instance in instances)
        {
            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"{0}{1}  ",
                first ? string.Empty : ",",
                instance);

            first = false;

        }

        this.LtInstances.Text = res.ToString();
    }
}