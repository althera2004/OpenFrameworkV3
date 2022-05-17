<%@ WebHandler Language="C#" Class="Instance" %>

using System;
using System.Globalization;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using OpenFrameworkV2;
using System.Data;
using System.Data.SqlClient;

public class Instance : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        string action = context.Request.Form["action"].ToUpperInvariant();

        var res = ActionResult.NoAction;
        if(!string.IsNullOrEmpty(action))
        {
            switch(action)
            {
                case "RELOAD":
                    res = Reload(context.Request.Form["actionData"]);
                    break;
                case "CONECTIONSTRINGTEST":
                    res = ConectionStringTest(context.Request.Form["actionData"]);
                    break;
                case "PATHVERIFY":
                    res = PathVerify(context.Request.Form["actionData"]);
                    break;
                default:
                    res.SetFail(string.Format(CultureInfo.InvariantCulture, "{0} - No action implemented", action));
                    break;
            }
        }

        // simulate Microsoft XSS protection
        var wrapper = new { d = res };
        context.Response.Write(res.Json);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public ActionResult Reload(string actionData)
    {
        var res = ActionResult.NoAction;
        dynamic json = JsonConvert.DeserializeObject(actionData);
        var instanceName = json.SelectToken("instanceName").ToString();
        return OpenFrameworkV2.Core.Instance.Reload(instanceName);
    }

    public ActionResult PathVerify(string actionData)
    {
        var res = ActionResult.NoAction;
        dynamic json = JsonConvert.DeserializeObject(actionData);
        var path = json.SelectToken("path").ToString();

        if(Directory.Exists(path))
        {
            res.SetSuccess();
        }
        else
        {
            res.SetFail("No");
        }

        return res;
    }

    public ActionResult ConectionStringTest(string actionData)
    {
        var res = ActionResult.NoAction;
        dynamic json = JsonConvert.DeserializeObject(actionData);
        var cns = json.SelectToken("cns").ToString();

        using(var cnn = new SqlConnection(cns))
        {
            try
            {
                cnn.Open();
                res.SetSuccess("Conexión realizada con éxito.");
            }
            catch(Exception ex)
            {
                res.SetFail(ex);
            }
            finally
            {
                if(cnn.State != ConnectionState.Closed)
                {
                    cnn.Close();
                }
            }
        }

        return res;
    }
}