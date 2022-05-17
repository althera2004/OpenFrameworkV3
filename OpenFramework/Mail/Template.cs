namespace OpenFrameworkV3.Mail
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using OpenFrameworkV3.Core;

    public class Template
    {
        public string InstanceName { get; set; }
        public string ItemName { get; set; }
        public string Action { get; set; }
        public long ItemId { get; set; }
        public Language Language { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public bool? HtmlFormat { get; set; }

        public void LoadContentChangePassword()
        {
            var instance = Persistence.InstanceByName(this.InstanceName);
            var path = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}ChangePassword{1}.mailx",
                ConfigurationManager.AppSettings["MailTemplates"],
                string.IsNullOrEmpty(Language.Iso) ? string.Empty : ("." + Language.Iso));

            this.Subject = string.Format(
                CultureInfo.InvariantCulture,
                @"{0} - Cambio de contraseña",
                instance.Name);

            if (File.Exists(path))
            {
                using (var input = new StreamReader(path))
                {
                    this.Content = input.ReadToEnd();
                }
            }
            else
            {
                this.Content = string.Empty;
            }
        }

        public void LoadContentReset()
        {
            LoadContentReset(Language.Default.Iso);
        }

        public void LoadContentReset(string languageCode)
        {
            var instance = Persistence.InstanceByName(this.InstanceName);

            string languagePart = string.Empty;
            if (!string.IsNullOrEmpty(languageCode))
            {
                languagePart = "." + languageCode;
            }

            var path = string.Format(CultureInfo.InvariantCulture, @"{0}\Reset{1}.mailx", Instance.Path.Mail(this.InstanceName), languagePart);
            if (!File.Exists(path))
            {
                path = string.Format(
                   CultureInfo.InvariantCulture,
                   @"{0}Reset.mailx",
                   ConfigurationManager.AppSettings["MailTemplates"]);
            }

            this.Subject = string.Format(
                CultureInfo.InvariantCulture,
                @"{0} - Reinicio de contraseña",
                instance.Name);

            if (File.Exists(path))
            {
                using (var input = new StreamReader(path))
                {
                    this.Content = input.ReadToEnd();
                }
            }
            else
            {
                this.Content = string.Empty;
            }
        }

        public void LoadContentMFA()
        {
            var instance = Persistence.InstanceByName(this.InstanceName);
            var path = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}MFA.mailx",
                ConfigurationManager.AppSettings["MailTemplates"]);

            this.Subject = string.Format(
                CultureInfo.InvariantCulture,
                @"{0} - Autenticación de usuario",
                instance.Name);

            if (File.Exists(path))
            {
                using (var input = new StreamReader(path))
                {
                    this.Content = input.ReadToEnd();
                }
            }
            else
            {
                this.Content = string.Empty;
            }
        }

        public void LoadContentData()
        {
            var instance = Persistence.InstanceByName(this.InstanceName);
            var path = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}\\{1}_Data.mailx",
                    Instance.Path.Mail(this.InstanceName),
                    this.ItemName);

            if (File.Exists(path))
            {
                using (var input = new StreamReader(path))
                {
                    this.Content = input.ReadToEnd();
                }
            }
            else
            {
                this.Content = string.Empty;
            }
        }

        public void LoadContent()
        {
            LoadContent(Language.Default.Iso);
        }

        public void LoadContent(string languageCode)
        {
            if (this.Action.Equals("data", StringComparison.OrdinalIgnoreCase))
            {
                LoadContentData();
                return;
            }

            if (this.Action.Equals("reset", StringComparison.OrdinalIgnoreCase))
            {
                LoadContentReset(languageCode);
                return;
            }

            if (this.Action.Equals("mfa", StringComparison.OrdinalIgnoreCase))
            {
                LoadContentMFA();
                return;
            }

            if (this.Action.Equals("changepassword", StringComparison.OrdinalIgnoreCase))
            {
                LoadContentChangePassword();
                return;
            }

            var instance = Persistence.InstanceByName(this.InstanceName);
            var path = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}\\{1}_{2}.mailx",
                    Instance.Path.Mail(this.InstanceName),
                    this.ItemName,
                    this.Action);

            using (var input = new StreamReader(path))
            {
                this.Content = input.ReadToEnd();
            }
        }
    }
}