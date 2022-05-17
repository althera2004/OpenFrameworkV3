namespace OpenFramework.Mail
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net.Mail;
    using System.Text;

    public class Item
    {
        public string InstanceName { get; set; }
        public string ItemName { get; set; }
        public long ItemId { get; set; }
        public string FormId { get; set; }
        public ItemDefinition ItemDefinition { get; set; }
        public ItemBuilder Data { get; set; }
        public CustomerFramework Instance { get; set; }

        private void ObtatinData()
        {
            this.Instance = CustomerFramework.Use(this.InstanceName);
            this.ItemDefinition = this.Instance.Definitions.First(d => d.ItemName.Equals(this.ItemName, StringComparison.OrdinalIgnoreCase));
            this.Data = Read.ById<ItemBuilder>(ItemId, this.ItemDefinition, this.Instance);
        }

        private string RenderContent()
        {
            var res = new StringBuilder("<h1>Ficha de ").Append(this.ItemDefinition.Layout.Label).Append("</h1><hr />");

            foreach (var field in this.ItemDefinition.Fields.Where(f => !f.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)))
            {
                var fieldData = this.Data[field.Name];
                if (field.FK)
                {
                    var fkItemDefinition = ItemDefinition.Empty;

                    foreach (var fk in this.ItemDefinition.ForeignValues)
                    {
                        if (field.Name.Equals(fk.ItemName + "id", StringComparison.OrdinalIgnoreCase))
                        {
                            fkItemDefinition = this.Instance.Definitions.First(d => d.ItemName.Equals(fk.ItemName, StringComparison.OrdinalIgnoreCase));
                        }
                    }

                    fieldData = Read.ById<ItemBuilder>(ItemId, fkItemDefinition, this.Instance).Description;
                }

                if(field.TypeName.Equals("bool", StringComparison.OrdinalIgnoreCase) || field.TypeName.Equals("boolean", StringComparison.OrdinalIgnoreCase))
                {
                    fieldData = (bool)fieldData ? "Sí" : "No";
                }
                else if (field.TypeName.Equals("date",StringComparison.OrdinalIgnoreCase) || field.TypeName.Equals("datetime",StringComparison.OrdinalIgnoreCase))
                {
                    fieldData = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", (DateTime)fieldData);
                }
                else if (field.TypeName.Equals("documentfile", StringComparison.OrdinalIgnoreCase))
                {
                    fieldData = string.Format(
                        CultureInfo.InvariantCulture,
                        @"<a href=""http://{0}.openframework.es/instances/{0}/Data/Documents/{1}"">{1}</a>",
                        this.InstanceName,
                        fieldData);                        
                }

                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<br />&nbsp&nbsp;{0}: {1}",
                    field.Label,
                    fieldData);
            }

            res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"<hr />{0}  {1:dd/MM/yyyy}",
                    this.Instance.Description,
                    DateTime.Now);

            return res.ToString();
        }

        public ActionResult Send(string mailDestination)
        {
            var res = ActionResult.NoAction;
            try
            {
                var instance = Persistence.ByName("AMPA");
                var mailTemplate = new Template()
                {
                    InstanceName = "AMPA",
                    Action = "data",
                    HtmlFormat = true
                };

                this.ObtatinData();

                var mail = new MailMessage
                {
                    From = new MailAddress("info@openframework.es", this.Instance.Description),
                    IsBodyHtml = mailTemplate.HtmlFormat ?? false,
                    Subject = "Ficha de " + this.ItemDefinition.Layout.Label + ": " + this.Data.Description,
                    Body = this.RenderContent()
                };

                mail.To.Add(mailDestination);
                mail.To.Add("althera2004@gmail.com");

                Manager.CoreSend(mail);
                res.SetSuccess("Se ha enviado la ficha de " + this.ItemDefinition.Layout.Label.ToLowerInvariant() + " " + this.Data.Description + " al mail " + mailDestination);
            }
            catch (SmtpException ex)
            {
                res.SetFail(ex);
            }
            catch (NullReferenceException ex)
            {
                res.SetFail(ex);
            }
            catch (Exception ex)
            {
                res.SetFail(ex);
            }

            return res;
        }
    }
}
