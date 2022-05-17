// --------------------------------
// <copyright file="ResetPassword.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Mail
{
    using System;
    using System.Globalization;
    using System.Net.Mail;

    public class ResetPassword
    {
        public static void Send()
        {
            var instance = Persistence.InstanceByName("AMPA");
            var mailTemplate = new Template()
            {
                InstanceName = "AMPA",
                Action = "reset",
                HtmlFormat = true
            };

            mailTemplate.LoadContent();

            mailTemplate.Content = mailTemplate.Content.Replace("#company#", instance.Description);
            mailTemplate.Content = mailTemplate.Content.Replace("#actualdate#", string.Format(CultureInfo.InvariantCulture, @"{0:dd/MM/yyyy}", DateTime.Now));
            mailTemplate.Content = mailTemplate.Content.Replace("#username#", "test@test.es");
            mailTemplate.Content = mailTemplate.Content.Replace("#password#", "#%$ekekeE");
            mailTemplate.Content = mailTemplate.Content.Replace("#nombre#", "Pepe Porras");
            mailTemplate.Content = mailTemplate.Content.Replace("#logo#", string.Format(CultureInfo.InvariantCulture, @"http://{0}.openframework.es/Instances/{0}/logo.png", instance.Name));


            var mail = new MailMessage
            {
                From = new MailAddress("info@openframework.es", "AMPA as as a sa s"),
                IsBodyHtml = mailTemplate.HtmlFormat ?? false,
                Subject = mailTemplate.Subject,
                Body = mailTemplate.Content
            };

            mail.To.Add("jcastilla@openframework.es");
            mail.To.Add("althera2004@gmail.com");

            Manager.CoreSend(mail);
        }
        public static void Send2()
        {
            var mail = new MailMessage
            {
                IsBodyHtml = true,
                Subject = "test config",
                Body = "hola que tal"
            };

            mail.To.Add("althera2004@gmail.com");

            Manager.ConfigSend(mail, "AMPvcbvbA");
        }
    }

}