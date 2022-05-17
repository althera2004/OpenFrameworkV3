﻿// --------------------------------
// <copyright file="SecurityService.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
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

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult AccessPolicySave(AccessPolicy accessPolicy, long applicationUserId, string instanceName)
    {
        return accessPolicy.Save(applicationUserId, instanceName);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult LogOn(string data)
    {
        var dataClean = Basics.Base64Decode(data).Replace("||||", "¶");
        var parts = dataClean.Split('¶');
        return ApplicationUser.LogOn(parts[0], parts[1], parts[2]);
    }
}