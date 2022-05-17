namespace OpenFrameworkV3.Mail
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Configuration;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;
    using OpenFrameworkV3.Tools;
    using OpenPop.Pop3;
    using S22.Imap;

    public class Manager
    {
        public static ActionResult CoreSend(MailMessage mail)
        {
            var res = ActionResult.NoAction;
            var smtpServer = new SmtpClient("mail.openframework.es")
            {
                Port = 25,
                Credentials = new System.Net.NetworkCredential("info@openframework.es", "P@ssw0rd")
            };

            smtpServer.Send(mail);
            return res;
        }

        public static string MostRecent(string connectionString, string mailboxCode)
        {
            var res = string.Empty;
            using(var cmd = new SqlCommand("Core_MailHeaders_GetMostRecent"))
            {
                using(var cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@MailBoxCode", mailboxCode, 50));
                    try
                    {
                        cmd.Connection.Open();
                        using(var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res = string.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0}|{1:dd/MM/yyyy}",
                                    rdr.GetString(1).Trim(),
                                    rdr.GetDateTime(2));
                            }
                        }
                    }
                    finally
                    {
                        if(cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return res;
        }

        public static ActionResult ConfigSend(MailMessage mail, string instanceName)
        {
            var config = MailConfiguration.Empty;
            var instance = Persistence.InstanceByName(instanceName);
            if(instance.Id == Constant.DefaultId)
            {
                try
                {
                    instance = Persistence.InstanceByName(instanceName);
                }
                catch
                {
                    if(instance.Id == Constant.DefaultId)
                    {
                        // weke instance = Instance.e
                    }
                }

                if(instance.Id != Constant.DefaultId)
                {
                    config = instance.Config.Mail;
                }
                else
                {
                    config.MailSender = "info@openframework.es";
                    config.Password = "P@ssw0rd";
                    config.Port = 25;
                    config.Server = "mail.openframework.es";
                    config.User = "info@openframework.es";
                    config.UserName = "OpenFramework";
                }
            }
            else
            {
                config = instance.Config.Mail;
            }

            mail.From = new MailAddress(config.MailSender);//, config.UserName);
            var res = ActionResult.NoAction;
            var smtpServer = new SmtpClient(config.Server)
            {
                Port = config.Port,
                Credentials = new System.Net.NetworkCredential(config.User, config.Password)
            };

            smtpServer.Send(mail);
            return res;
        }

        public static ActionResult FetchAllMessages(string mailBoxCode, string hostname, string protocol, int port, bool useSsl, string username, string password, string instanceName)
        {
            var res = ActionResult.NoAction;
            var instance = CustomerFramework.Actual;

            // We want to download all messages
            var allMessages = new List<MailHeader>();
            try
            {
                if (protocol.Equals("pop3", StringComparison.OrdinalIgnoreCase))
                {
                    // The client disconnects from the server when being disposed
                    using (Pop3Client client = new Pop3Client())
                    {
                        // Connect to the server
                        client.Connect(hostname, port, useSsl);

                        // Authenticate ourselves towards the server
                        client.Authenticate(username, password);

                        // Get the number of messages in the inbox
                        int messageCount = client.GetMessageCount();

                        var mostRecentData = MostRecent(connectionString, mailBoxCode);
                        var lastHeader = string.Empty;
                        var lastDate = DateTime.Now.AddMonths(-1);
                        if (!string.IsNullOrEmpty(mostRecentData))
                        {
                            lastHeader = mostRecentData.Split('|')[0];
                        }

                        // Messages are numbered in the interval: [1, messageCount]
                        // Ergo: message numbers are 1-based.
                        // Most servers give the latest message the highest number
                        var indexes = new List<int>();
                        for (var i = messageCount; i > messageCount - 200; i--)
                        {
                            indexes.Add(i);
                        }

                        //for (int i = messageCount; i > 0; i--)
                        foreach (var i in indexes)
                        {
                            if (i > 0)
                            {
                                var messageHeader = client.GetMessageHeaders(i);
                                var message = client.GetMessage(i);

                                /*if (messageHeader.MessageId.Equals(lastHeader, StringComparison.OrdinalIgnoreCase))
                                {
                                   //break;
                                }*/

                                var toText = string.Empty;
                                var toAddres = string.Empty;
                                if (messageHeader.To != null && messageHeader.To.Count > 0)
                                {
                                    toAddres = messageHeader.To[0].Address;
                                    toText = messageHeader.To[0].DisplayName;
                                }

                                var header = new MailHeader
                                {
                                    MailDate = messageHeader.DateSent,
                                    From = messageHeader.From.Address,
                                    FromText = messageHeader.From.DisplayName,
                                    To = toAddres,
                                    ToText = toText,
                                    CC = string.Empty,
                                    BCC = string.Empty,
                                    Subject = messageHeader.Subject,
                                    MessageId = messageHeader.MessageId,
                                    HasAttachments = messageHeader.ContentDisposition != null,
                                    MailBoxId = mailBoxCode,
                                    Uid = (uint)i
                                };

                                header.Insert(ApplicationUser.OpenFramework.Id, connectionString);

                                // FileInfo about the location to save/load message
                                var path = instance.PathMailTemplate;
                                if (!path.EndsWith("\\"))
                                {
                                    path += "\\";
                                }

                                Basics.VerifyFolder(path);
                                var fileName = string.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0}Bodies\\{1}_{2}.eml",
                                    path,
                                    mailBoxCode,
                                    i);
                                FileInfo file = new FileInfo(fileName);

                                // Save the full message to some file
                                message.Save(file);


                                allMessages.Add(header);
                            }
                        }
                    }
                }

                if (protocol.Equals("imap", StringComparison.OrdinalIgnoreCase))
                {
                    using (ImapClient client = new ImapClient(hostname, port, username, password, AuthMethod.Login, true))
                    {
                        // Returns a collection of identifiers of all mails matching the specified search criteria.
                        IEnumerable<uint> uids = client.Search(SearchCondition.SentSince(DateTime.Now.AddDays(-3)));

                        // Download mail messages from the default mailbox.
                        foreach (var uid in uids)
                        {
                            var uids2 = new List<uint>
                        {
                            uid
                        };

                            var x = client.GetMessages(uids2 as IEnumerable<uint>, FetchOptions.Normal);
                            var m = x.First();
                            if (!string.IsNullOrEmpty(m.Headers["Message-ID"]))
                            {
                                var toText = string.Empty;
                                var toAddres = string.Empty;
                                var messageId = string.Empty;
                                if (m.To != null && m.To.Count > 0)
                                {
                                    toAddres = m.To[0].Address;
                                    toText = m.To[0].DisplayName;
                                }

                                var header = new MailHeader
                                {
                                    MailDate = m.Date().Value,
                                    From = m.From.Address,
                                    FromText = m.From.DisplayName,
                                    To = toAddres,
                                    ToText = toText,
                                    CC = string.Empty,
                                    BCC = string.Empty,
                                    Subject = m.Subject,
                                    MessageId = m.Headers["Message-ID"],
                                    HasAttachments = m.Attachments.Count > 0,
                                    MailBoxId = mailBoxCode,
                                    Uid = uid
                                };

                                allMessages.Add(header);

                                header.Insert(ApplicationUser.OpenFramework.Id, connectionString);
                                var body = m.Body;

                                var path = instance.PathMailTemplate;
                                if (!path.EndsWith("\\"))
                                {
                                    path += "\\";
                                }

                                Basics.VerifyFolder(path);
                                var fileName = string.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0}Bodies\\{1}.body",
                                    path,
                                    header.Uid);

                                using (var output = new StreamWriter(fileName, false))
                                {
                                    output.Write(body);
                                }

                                /*if (header.HasAttachments)
                                {
                                    foreach(var a in m.Attachments)
                                    {

                                        var fileAttach = string.Format(CultureInfo.InvariantCulture, @"{0}\{2}.{1}", path, Path.GetFileName(a.Name), header.Uid);
                                        FileStream Stream = new FileStream(fileAttach, FileMode.Create);
                                        BinaryWriter BinaryStream = new BinaryWriter(a.ContentStream);
                                        BinaryStream.Write(fileAttach);
                                        BinaryStream.Close();
                                    }
                                }*/
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.Trace(ex, "OpenFramework.Mail.FetchAllMessages");
                if(ex.Message.IndexOf("NO LOGIN",StringComparison.OrdinalIgnoreCase) != -1)
                {
                    res.SetFail("Acceso no permitido al buzón de correo.");
                }
                else
                {
                    res.SetFail(ex);
                }
            }

            // Now return the fetched messages
            return res;
        }

        public static ActionResult FetchAllMessages(string instanceName, string code, string connectionString)
        {
            var mailBox = Persistence.MailBoxByCode(instanceName, code);
            return FetchAllMessages(mailBox, connectionString);
        }

        public static ActionResult FetchAllMessages(MailBox mailBox, string connectionString)
        {
            return FetchAllMessages(mailBox.Code, mailBox.Server, mailBox.Protocol, mailBox.ReaderPort, mailBox.SSL, mailBox.User, mailBox.Password, connectionString);
        }

        public static ActionResult RefreshMails(string instanceName, string mailBoxId, string connectionString)
        {
            return FetchAllMessages(instanceName, mailBoxId, connectionString);
        }

        public static ActionResult RefreshMails(MailBox mailBox, string connectionString)
        {
            var instance = CustomerFramework.Actual;
            return FetchAllMessages(instance.Name, mailBox.Code, connectionString);
        }

        //public static void InsertOnDataBase(ReadOnlyCollection<MailHeader> mails, string connectionString)
        //{
        //    foreach (var mail in mails)
        //    {
        //        mail.Insert(ApplicationUser.OpenFramework.Id, connectionString);
        //    }
        //}
    }
}
