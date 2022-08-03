// --------------------------------
// <copyright file="InstanceService.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using OpenFrameworkV3;
using OpenFrameworkV3.Core;
using OpenFrameworkV3.Core.Activity;
using OpenFrameworkV3.Core.DataAccess;
using OpenFrameworkV3.Core.ItemManager.ItemList;
using OpenFrameworkV3.Core.Navigation;
using OpenFrameworkV3.Tools;

/// <summary>
/// Descripción breve de InstanceService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class InstanceService : WebService
{

    public InstanceService()
    {

        //Elimine la marca de comentario de la línea siguiente si utiliza los componentes diseñados 
        //InitializeComponent(); 
    }

    [WebMethod]
    public ActionResult DictionaryGetCorpus(string language, string instanceName)
    {
        var res = ActionResult.NoAction;
        try
        {
            var data = ApplicationDictionary.GetCorpus(language, instanceName);
            ApplicationDictionary.CreateJavascriptFile(language, instanceName);
            res.SetSuccess(data);
        }
        catch (Exception ex)
        {
            res.SetFail(ex);
        }

        return res;
    }

    [WebMethod]
    public ActionResult ReloadInstance(string instanceName)
    {
        var res = ActionResult.NoAction;
        try
        {
            res = Instance.Reload(instanceName);
            if (res.Success)
            {
                res.SetSuccess(Persistence.InstanceByName(instanceName).Json);
            }
        }
        catch (Exception ex)
        {
            res.SetFail(ex);
        }

        return res;
    }

    [WebMethod]
    public ActionResult ReloadDefinitions(string instanceName)
    {
        var res = ActionResult.NoAction;
        try
        {
            res = Instance.Reload(instanceName);
            if (res.Success)
            {
                res.SetSuccess(Instance.JsonDefinitions(instanceName));
            }
        }
        catch (Exception ex)
        {
            res.SetFail(ex);
        }

        return res;
    }

    [WebMethod]
    public ActionResult ReloadMenu(long applicationUserId, long companyId, string instanceName)
    {
        var res = ActionResult.NoAction;
        try
        {
            res = Instance.Reload(instanceName);
            if (res.Success)
            {
                var menu = Menu.Load(applicationUserId, companyId, false, instanceName);
                res.SetSuccess(menu.GetJson());
            }
        }
        catch (Exception ex)
        {
            res.SetFail(ex);
        }

        return res;
    }
}
