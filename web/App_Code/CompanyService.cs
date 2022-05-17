using OpenFrameworkV2;
using OpenFrameworkV2.Core;
using OpenFrameworkV2.Core.Companies;
using System.Web.Script.Services;
using System.Web.Services;

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
    public ActionResult BankAccountActivate(long bankAccountId, bool active, long applicationUserId, long companyId, string instanceName)
    {
        return CompanyBankAccount.Activate(bankAccountId, active, applicationUserId, companyId, instanceName);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult BankAccountSetMain(long bankAccountId, long applicationUserId, long companyId, string instanceName)
    {
        return CompanyBankAccount.SetMain(bankAccountId, applicationUserId, companyId, instanceName);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult MailBox(MailBox mailBox, long applicationUserId, string instanceName)
    {
        return mailBox.Save(applicationUserId, instanceName);
    }
}