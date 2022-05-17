namespace OpenFrameworkV3.Communications
{
    using System;
    using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;

    public partial class Message
    {
        private List<long> targetGroups;
        private List<long> targetUsers;

        public long Id { get; set; }
        public long CompanyId { get; set; }
        public ApplicationUser Author { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public long ResponseOf { get; set; }
        public int Importance { get; set; }
        public long ItemDefinitionId { get; set; }
        public long ItemId { get; set; }
        public string targetGroupsIds { get; set; }
        public string targetUsersIds { get; set; }
        public bool FromBoss { get; set; }

        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public static Message Empty
        {
            get
            {
                return new Message
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    ItemDefinitionId = Constant.DefaultId,
                    ItemId = Constant.DefaultId,
                    Author = ApplicationUser.Empty,
                    Subject = string.Empty,
                    Text = string.Empty,
                    Importance = 0,
                    targetGroupsIds = string.Empty,
                    targetUsersIds = string.Empty,
                    targetGroups = new List<long>(),
                    targetUsers = new List<long>(),
                    ResponseOf = Constant.DefaultId,
                    CreatedBy = ApplicationUser.Empty,
                    ModifiedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public ActionResult ResponseTo(long applicationUserId)
        {
            var res = ActionResult.NoAction;
            var instance = CustomerFramework.Actual;
            /* CREATE PROCEDURE [dbo].[Core_Messaging_ResponseTo]
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @AuthorId bigint,
             *   @Message nvarchar(2000),
             *   @ResponseOf bigint,
             *   @ApplicationUserId bigint */
            using (var cmd = new SqlCommand("Core_Messaging_ResponseTo"))
            {
                using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@AuthorId", this.Author.Id));
                    cmd.Parameters.Add(DataParameter.Input("@Message", this.Text, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@ResponseOf", this.ResponseOf));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                        res.SetSuccess(this.Id);
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }

            }

            return res;
        }

        public ActionResult Insert(long applicationUserId)
        {
            var res = ActionResult.NoAction;
            var instance = CustomerFramework.Actual;
            /* CREATE PROCEDURE [dbo].[Core_Messaging_Insert]
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @AuthorId bigint,
             *   @TargetGroups nchar(100),
             *   @TargetUsers nchar(100),
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @Subject nvarchar(150),
             *   @Message nvarchar(2000),
             *   @ResponseOf bigint,
             *   @Importance int,
             *   @FromBoss int,
             *   @ApplicationUserId bigint */
            using (var cmd = new SqlCommand("Core_Messaging_Insert"))
            {
                using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@AuthorId", this.Author.Id));
                    cmd.Parameters.Add(DataParameter.Input("@TargetGroups", this.targetGroupsIds, 100));
                    cmd.Parameters.Add(DataParameter.Input("@TargetUsers", this.targetUsersIds, 100));
                    cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                    cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                    cmd.Parameters.Add(DataParameter.Input("@Subject", this.Subject, 150));
                    cmd.Parameters.Add(DataParameter.Input("@Message", this.Text, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@ResponseOf", this.ResponseOf));
                    cmd.Parameters.Add(DataParameter.Input("@Importance", this.Importance));
                    cmd.Parameters.Add(DataParameter.Input("@FromBoss", this.FromBoss));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                        res.SetSuccess(this.Id);
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }

            }

            return res;
        }

        public static ActionResult SetViewed(long id, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Messaging_SetViewd"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@MessagingId", id));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(id);
                        }
                        catch (Exception ex)
                        {
                            res.SetFail(ex);
                        }
                        finally
                        {
                            if (cmd.Connection.State != ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return res;
        }

        public static ActionResult Activate(long id, long companyId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Messaging_Activate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", ApplicationUser.Actual.Id));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
                        }
                        catch (Exception ex)
                        {
                            res.SetFail(ex);
                        }
                        finally
                        {
                            if (cmd.Connection.State != ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return res;
        }

        public static ActionResult Inactivate(long id, long companyId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Messaging_Inactivate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", ApplicationUser.Actual.Id));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
                        }
                        catch (Exception ex)
                        {
                            res.SetFail(ex);
                        }
                        finally
                        {
                            if (cmd.Connection.State != ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return res;
        }

        public static Message ById(long Id, long companyId, string instanceNAme)
        {
            var res = Message.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Messaging_ById"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res = new Message
                                    {
                                        Id = rdr.GetInt64(ColumnsMessagingGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsMessagingGet.CompanyId),
                                        ItemDefinitionId = rdr.GetInt64(ColumnsMessagingGet.ItemDefinitionId),
                                        ItemId = rdr.GetInt64(ColumnsMessagingGet.ItemId),
                                        Author = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsMessagingGet.AuthorId),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsMessagingGet.AuthorId),
                                                Name = rdr.GetString(ColumnsMessagingGet.AuthorName),
                                                LastName = rdr.GetString(ColumnsMessagingGet.AuthorLastName),
                                                LastName2 = rdr.GetString(ColumnsMessagingGet.AuthorLastName2)
                                            }
                                        },
                                        Subject = rdr.GetString(ColumnsMessagingGet.Subject),
                                        Text = rdr.GetString(ColumnsMessagingGet.Message),
                                        ResponseOf = rdr.GetInt64(ColumnsMessagingGet.ResponseOf),
                                        Importance = rdr.GetInt32(ColumnsMessagingGet.Importance),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsMessagingGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsMessagingGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsMessagingGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsMessagingGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsMessagingGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsMessagingGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsMessagingGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsMessagingGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsMessagingGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsMessagingGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsMessagingGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsMessagingGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsMessagingGet.Active)
                                    };
                                }
                            }
                        }
                        finally
                        {
                            if (cmd.Connection.State != ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return res;
        }

        public static ReadOnlyCollection<Message> ByCompany(long applicationUSerId, long companyId, string instanceName)
        {
            var res = new List<Message>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Messaging_ByCompany"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUSerId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    var newMessage = new Message
                                    {
                                        Id = rdr.GetInt64(ColumnsMessagingGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsMessagingGet.CompanyId),
                                        ItemDefinitionId = rdr.GetInt64(ColumnsMessagingGet.ItemDefinitionId),
                                        ItemId = rdr.GetInt64(ColumnsMessagingGet.ItemId),
                                        Author = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsMessagingGet.AuthorId),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsMessagingGet.AuthorId),
                                                Name = rdr.GetString(ColumnsMessagingGet.AuthorName),
                                                LastName = rdr.GetString(ColumnsMessagingGet.AuthorLastName),
                                                LastName2 = rdr.GetString(ColumnsMessagingGet.AuthorLastName2)
                                            }
                                        },
                                        Subject = rdr.GetString(ColumnsMessagingGet.Subject),
                                        Text = rdr.GetString(ColumnsMessagingGet.Message),
                                        ResponseOf = rdr.GetInt64(ColumnsMessagingGet.ResponseOf),
                                        Importance = rdr.GetInt32(ColumnsMessagingGet.Importance),
                                        FromBoss = rdr.GetBoolean(ColumnsMessagingGet.FromBoss),
                                        targetGroupsIds = rdr.GetString(ColumnsMessagingGet.TargetGroups),
                                        targetUsersIds = rdr.GetString(ColumnsMessagingGet.TargetUsers),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsMessagingGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsMessagingGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsMessagingGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsMessagingGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsMessagingGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsMessagingGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsMessagingGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsMessagingGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsMessagingGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsMessagingGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsMessagingGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsMessagingGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsMessagingGet.Active)
                                    };

                                    res.Add(newMessage);
                                }
                            }
                        }
                        finally
                        {
                            if (cmd.Connection.State != ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return new ReadOnlyCollection<Message>(res);
        }

        public static ReadOnlyCollection<Message> ByItem(long itemDefinitionId, long itemId, long companyId, string instanceName)
        {
            var res = new List<Message>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Messaging_ByItemId"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    var newMessage = new Message
                                    {
                                        Id = rdr.GetInt64(ColumnsMessagingGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsMessagingGet.CompanyId),
                                        ItemDefinitionId = rdr.GetInt64(ColumnsMessagingGet.ItemDefinitionId),
                                        ItemId = rdr.GetInt64(ColumnsMessagingGet.ItemId),
                                        Author = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsMessagingGet.AuthorId),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsMessagingGet.AuthorId),
                                                Name = rdr.GetString(ColumnsMessagingGet.AuthorName),
                                                LastName = rdr.GetString(ColumnsMessagingGet.AuthorLastName),
                                                LastName2 = rdr.GetString(ColumnsMessagingGet.AuthorLastName2)
                                            }
                                        },
                                        Subject = rdr.GetString(ColumnsMessagingGet.Subject),
                                        Text = rdr.GetString(ColumnsMessagingGet.Message),
                                        ResponseOf = rdr.GetInt64(ColumnsMessagingGet.ResponseOf),
                                        Importance = rdr.GetInt32(ColumnsMessagingGet.Importance),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsMessagingGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsMessagingGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsMessagingGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsMessagingGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsMessagingGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsMessagingGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsMessagingGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsMessagingGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsMessagingGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsMessagingGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsMessagingGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsMessagingGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsMessagingGet.Active)
                                    };

                                    res.Add(newMessage);
                                }
                            }
                        }
                        finally
                        {
                            if (cmd.Connection.State != ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return new ReadOnlyCollection<Message>(res);
        }

        public static int TotalByUser(long userId, long companyId, string instanceName)
        {
            var res = 0;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Messaging_TotalByUser"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res++;
                                }
                            }
                        }
                        finally
                        {
                            if (cmd.Connection.State != ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return res;
        }

        public static ReadOnlyCollection<Message> ByUserId(long userId, long companyId, string instanceName)
        {
            var user = ApplicationUser.ById(userId, instanceName);
            var res = new List<Message>();

            var messages = ByCompany(userId, companyId, instanceName);

            foreach (var message in messages)
            {
                if (message.targetUsersIds.IndexOf("|" + user.Id.ToString() + "|") != -1 || message.Author.Id == user.Id)
                {
                    res.Add(message);
                }
                else
                {
                    var found = false;
                    foreach (var group in user.Groups)
                    {
                        if (message.targetGroupsIds.IndexOf("|" + group.ToString() + "|") != -1)
                        {
                            res.Add(message);
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        if(message.ResponseOf > 0)
                        {
                            if(messages.Any(m=>m.Id == message.ResponseOf))
                            {
                                var original = messages.First(m => m.Id == message.ResponseOf);
                                if(original.Author.Id == user.Id)
                                {
                                    res.Add(original);
                                    res.Add(message);
                                }
                            }
                        }
                    }
                }
            }

            return new ReadOnlyCollection<Message>(res);
        }

       /* public static string ImportantByUserId(long userId, long companyId)
        {
            var user = ApplicationUser.ById(userId);
            var instance = CustomerFramework.Actual;
            var res = new StringBuilder("");

            var ids = new List<long>();
            using (var cmd = new SqlCommand("Core_Messaging_ImportantByUser"))
            {
                using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", userId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.AppendFormat(
                                    CultureInfo.InvariantCulture,)
                                @"{{""Label"":""' + X.Label + '"",""Importance"":' + CAST(X.Importance AS nvarchar(6)) + ',""Category"":""' + X.Category + '"",' +
    '""Menu"":""' + X.CustomMenu + '"",""Groups"":[' + X.Groups + '],' +
    '""Acion"":""' + X.CustomAction + '"",' +
    '""Nombre"":""' + X.Nombre + '"",' +
    '""Icon"":""' + X.Icon + '"",' +
    '""ItemId"":' + CAST(x.Id as nvarchar(10)) + ',' +
    '""ItemName"":""' + X.ItemName + '""' +
    '}"),

                            }
                        }
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return res.ToString();
        }*/
    }
}