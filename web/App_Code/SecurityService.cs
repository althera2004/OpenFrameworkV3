// --------------------------------
// <copyright file="SecurityService.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using OpenFrameworkV3;
using OpenFrameworkV3.Core.Activity;
using OpenFrameworkV3.Core.Security;
using OpenFrameworkV3.Tools;

/// <summary>
/// Descripción breve de ItemService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class SecurityService : WebService
{

    public SecurityService()
    {
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult AccessPolicySave(AccessPolicy accessPolicy, long applicationUserId, string instanceName)
    {
        return accessPolicy.Save(applicationUserId, instanceName);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult ApplicationUserSave(ApplicationUser user, long applicationUserId, long companyId, string instanceName)
    {
        var res = ActionResult.NoAction;
        if(user.Id == Constant.DefaultId)
        {
            res = user.Insert(applicationUserId,companyId, instanceName);
        }
        else
        {
            res = user.Update(applicationUserId, companyId, instanceName);
        }

        return res;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult LogOn(string credential)
    {
        var dataClean = Basics.Base64Decode(credential).Replace("||||", "¶");
        var parts = dataClean.Split('¶');
        return ApplicationUser.LogOn(parts[0], parts[1], parts[2]);
    }
}