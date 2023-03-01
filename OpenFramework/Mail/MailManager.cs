using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using OpenFrameworkV3.Core;
using OpenFrameworkV3.Core.Activity;
using OpenFrameworkV3.Core.Security;

namespace OpenFrameworkV3.Mail
{
    public class MailManager
    {
        public static ActionResult SendPaswordRecovery(long userId, string newPassword, string instanceName)
        {
            var res = ActionResult.NoAction;
            var user = ApplicationUser.ById(userId, instanceName);
            var instance = Persistence.InstanceByName(instanceName);
            var mailTemplate = new Template()
            {
                InstanceName = instanceName,
                Action = "reset",
                HtmlFormat = true,
                Language = new Language
                {
                    Id = Constant.DefaultId,
                    Name = string.Empty,
                    Iso = user.Language.Iso
                }
            };

            mailTemplate.LoadContent(user.Language.Iso);
            mailTemplate.Content = mailTemplate.Content.Replace("#instancename#", instanceName);
            // weke: qué hago con esto?
            mailTemplate.Content = mailTemplate.Content.Replace("#company#", instanceName);
            mailTemplate.Content = mailTemplate.Content.Replace("#actualdate#", string.Format(CultureInfo.InvariantCulture, @"{0:dd/MM/yyyy}", DateTime.Now));
            mailTemplate.Content = mailTemplate.Content.Replace("#username#", user.Email);
            mailTemplate.Content = mailTemplate.Content.Replace("#password#", newPassword);
            mailTemplate.Content = mailTemplate.Content.Replace("#nombre#", user.Profile.Name);

            //---------------

            var mailBox = GetMailBoxMain(userId, instanceName);

            if (mailBox.Id > 0)
            {

                string sender = mailBox.MailUser;
                string pass = mailBox.MailPassword;
                var senderMail = new MailAddress(sender, mailBox.SenderName);
                var to = instance.Config.DebugMode ? new MailAddress("info@openframework.es") : new MailAddress(user.Email);

                using (var client = new SmtpClient
                {
                    Host = mailBox.Server,
                    Credentials = new System.Net.NetworkCredential(sender, pass),
                    Port = mailBox.SendPort,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                })
                {
                    var body = string.Empty;
                    var mail = new MailMessage
                    {
                        From = senderMail,
                        IsBodyHtml = mailTemplate.HtmlFormat ?? false,
                        Subject = mailTemplate.Subject,
                        Body = mailTemplate.Content
                    };

                    mail.To.Add(to);
                    mail.Bcc.Add("sat@openframework.cat");


                    /*if (to.Address.Contains("gmail.com"))
                    {
                        try
                        {
                            client.Port = 587;
                            client.Credentials = new System.Net.NetworkCredential(sender, pass);
                            client.EnableSsl = true;
                            client.Send(mail);
                        }
                        catch (Exception ex)
                        {
                            res.SetFail(ex);
                        }
                    }
                    else
                    {
                        try
                        {
                            client.Port = mailBox.SendPort;
                            client.Credentials = new System.Net.NetworkCredential(sender, pass);
                            client.EnableSsl = mailBox.SSL;
                            client.Send(mail);
                        }
                        catch (Exception ex)
                        {
                            res.SetFail(ex);
                        }
                    }*/

                    res.SetFail("alskjd alksjd alksd");
                }
            }

            return res;
        }

        public static ActionResult SendPaswordReset(long userId, string newPassword, string instanceName)
        {
            var res = ActionResult.NoAction;
            return res;
        }

        public static ActionResult SendWellcomeUser(long userId, string newPassword, string instanceName)
        {
            var res = ActionResult.NoAction;
            return res;
        }

        private static MailBox GetMailBoxMain(long userid, string instanceName)
        {
            var res = new MailBox();
            var companiesId = ApplicationUser.CompanyMemeberShip(userid, instanceName);
            if (companiesId.Count > 0)
            {
                var mailBoxes = MailBox.ByCompanyId(companiesId[0], instanceName);
                if (mailBoxes.Count > 0)
                {
                    res = mailBoxes.First(m => m.Main == true);
                }
            }

            return res;
        }
    }
}