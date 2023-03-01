// --------------------------------
// <copyright file="SecurityService.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
using System;
using System.Web.Script.Services;
using System.Web.Services;
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

    [WebMethod(EnableSession = false)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult AccessPolicySave(AccessPolicy accessPolicy, long applicationUserId, string instanceName)
    {
        return accessPolicy.Save(applicationUserId, instanceName);
    }

    [WebMethod(EnableSession = false)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult ApplicationUserSave(ApplicationUser user, long applicationUserId, long companyId, string instanceName)
    {
        return user.Save(applicationUserId, companyId, instanceName);
    }

    [WebMethod(EnableSession = false)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult LogOn(string credential)
    {
        var dataClean = Basics.Base64Decode(credential).Replace("||||", "¶");
        var parts = dataClean.Split('¶');
        return ApplicationUser.LogOn(parts[0], parts[1], parts[2]);
    }

    [WebMethod(EnableSession = false)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult MaintainSession(string credential)
    {
        var dataClean = Basics.Base64Decode(credential).Replace("||||", "¶");
        var parts = dataClean.Split('¶');
        return ApplicationUser.MaintainSession(Convert.ToInt64(parts[0]), parts[1], Convert.ToInt64(parts[2]), parts[3]);
    }

    [WebMethod(EnableSession = false)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult RecoverPassword(string credential)
    {
        var dataClean = Basics.Base64Decode(credential).Replace("||||", "¶");
        var parts = dataClean.Split('¶');
        return ApplicationUser.RecoverPassword(parts[0], parts[1], parts[2]);
    }

    [WebMethod(EnableSession = false)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult ChangePassword(string credential)
    {
        var dataClean = Basics.Base64Decode(credential).Replace("||||", "¶");
        var parts = dataClean.Split('¶');
        return ApplicationUser.ChangePassword(Convert.ToInt64(parts[0]), parts[1], parts[2], parts[3], parts[4]);
    }

    [WebMethod(EnableSession = false)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult ApplicationUserGetName(long userId, string instanceName)
    {
        var res = ActionResult.NoAction;
        try
        {
            var user = ApplicationUser.ById(userId, instanceName);
            if (user.Id > 0)
            {
                res.SetSuccess(user.Profile.FullName);
            }
        }
        catch (Exception ex)
        {
            res.SetFail(ex);
        }

        return res;
    }

    [WebMethod(EnableSession = false)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult ApplicationUserById(long applicationUserId, string instanceName)
    {
        var res = ActionResult.NoAction;
        try
        {
            var data = ApplicationUser.ById(applicationUserId, instanceName).Json;
            res.SetSuccess(data);
        }
        catch(Exception ex)
        {
            res.SetFail(ex);
        }

        return res;
    }

    [WebMethod(EnableSession = false)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult SecurityGroupUpdate(long securityGroupId, string name, string description, string membership, string grants, long companyId, long applicationUserId, string instanceName)
    {
        var res = ActionResult.NoAction;

        Grant.SaveGroupGrants(securityGroupId, grants, applicationUserId, companyId, instanceName);

        return res;
    }
}