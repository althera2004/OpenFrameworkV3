// --------------------------------
// <copyright file="CompanyService.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using OpenFrameworkV3.Core.Activity;
using OpenFrameworkV3.Core.Companies;
using OpenFrameworkV3.Mail;

/// <summary>
/// Descripción breve de CompanyService
/// </summary>
[WebService(Namespace = "http://openframework.cat/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class CompanyService : WebService
{
    public CompanyService()
    {
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string BankAccountById(long bankAccountId, long companyId, string instanceName)
    {
        return CompanyBankAccount.ById(bankAccountId, companyId, instanceName).Json;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string BankAccountMainByCompany(long companyId, string instanceName)
    {
        return CompanyBankAccount.MainByCompany(companyId, instanceName).Json;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string BankAccountByCompany(long companyId, string instanceName)
    {
        return CompanyBankAccount.JsonList(CompanyBankAccount.ByCompany(companyId, instanceName));
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult BankAccountSave(CompanyBankAccount account, long applicationUserId, string instanceName)
    {
        return account.Save(applicationUserId, instanceName);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult BankAccountActivate(long bankAccountId, long applicationUserId, long companyId, string instanceName)
    {
        return CompanyBankAccount.Activate(bankAccountId, applicationUserId, companyId, instanceName);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult BankAccountSetMain(long bankAccountId, long applicationUserId, long companyId, string instanceName)
    {
        return CompanyBankAccount.SetMain(bankAccountId, applicationUserId, companyId, instanceName);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult MailBoxSave(MailBox mailBox, long applicationUserId, string instanceName)
    {
        return mailBox.Save(applicationUserId, instanceName);
    }
}