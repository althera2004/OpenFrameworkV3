// --------------------------------
// <copyright file="Event.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Calendar
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.ItemManager;
    using OpenFrameworkV3.Core.Security;

    public partial class Event
    {
        private List<long> targetUsersId;
        private List<long> targetGroupsId;

        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string EventType { get; set; }
        public long ItemDefinitionId { get; set; }
        public long ItemId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Location { get; set; }
        public string Color { get; set; }
        public string TextColor { get; set; }
        public string Icon { get; set; }
        public bool? ExtraDataBool1 { get; set; }
        public bool? ExtraDataBool2 { get; set; }
        public string ExtraDataText1 { get; set; }
        public string ExtraDataText2 { get; set; }

        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        private DateTime createdOn;
        public DateTime ModifiedOn;
        public bool Active;

        public static Event Empty
        {
            get
            {
                return new Event
                {
                    Id = Constant.DefaultId,
                    EventType = "E",
                    CompanyId = Constant.DefaultId,
                    DateStart = DateTime.Now,
                    DateEnd = DateTime.Now,
                    Title = string.Empty,
                    Text = string.Empty,
                    Location = string.Empty,
                    targetUsersId = new List<long>(),
                    targetGroupsId = new List<long>(),
                    CreatedBy = ApplicationUser.Empty,
                    ModifiedBy = ApplicationUser.Empty,
                    createdOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},
                        ""CompanyId"":{1},
                        ""EventType"":""{2}"",
                        ""ItemDefinitionId"":{3},
                        ""ItemId"":{4},
                        ""TargetUsersId"":{5},
                        ""TargetGroupsId"":{6},
                        ""DateStart"":""{7:dd/MM/yyyy}"",
                        ""DateEnd"":{8},
                        ""Title"":""{9}"",
                        ""Text"":""{10}"",
                        ""Location"":""{11}"",
                        ""Color"":""{12}"",
                        ""TextColor"":""{13}"",
                        ""Icon"":""{14}"",
                        ""ExtraDataBool1"": {15},
                        ""ExtraDataBool2"": {16},
                        ""ExtraDataText1"": ""{17}"",
                        ""ExtraDataText2"": ""{18}"",
                        ""CreatedBy"":{19},
                        ""CreatedOn"":""{20:dd/MM/yyyy}"",
                        ""ModifiedBy"":{21},
                        ""ModifiedOn"":""{22:dd/MM/yyyy}"",
                        ""Active"":{23}}}",
                    this.Id,
                    this.CompanyId,
                    this.EventType,
                    this.ItemDefinitionId,
                    this.ItemId,
                    UsersJson,
                    GroupsJson,
                    this.DateStart,
                    this.DateEnd.HasValue ? string.Format(CultureInfo.InvariantCulture, @"""{0:dd/MM/yyyy}""", this.DateEnd.Value) : Constant.JavaScriptNull,
                    Tools.Json.JsonCompliant(this.Title),
                    Tools.Json.JsonCompliant(this.Text),
                    Tools.Json.JsonCompliant(this.Location),
                    Tools.Json.JsonCompliant(this.Color).Trim(),
                    Tools.Json.JsonCompliant(this.TextColor).Trim(),
                    Tools.Json.JsonCompliant(this.Icon).Trim(),
                    this.ExtraDataBool1.HasValue ? ConstantValue.Value(this.ExtraDataBool1) : Constant.JavaScriptNull,
                    this.ExtraDataBool2.HasValue ? ConstantValue.Value(this.ExtraDataBool2) : Constant.JavaScriptNull,
                    Tools.Json.JsonCompliant(this.ExtraDataText1),
                    Tools.Json.JsonCompliant(this.ExtraDataText2),
                    this.CreatedBy.JsonKeyValue,
                    this.createdOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static Event FromDefinition(ItemDefinition item, ItemData data, long companyId, string instanceName)
        {
            var res = Event.Empty;
            var definition = EventDefinition.FromFile(item.ItemName, instanceName);

            // Detect if event definition applies on actual company
            if (definition.CompanyId == 0 || definition.CompanyId == companyId) {
                if (definition.Data.Count > 0)
                {
                    res.CompanyId = companyId;
                    res.ItemId = item.Id;
                    res.Icon = definition.Icon;
                    res.Text = definition.TextColor;
                    res.Color = definition.TagColor;
                    res.Title = item.Layout.Label;

                    if (definition.Data.Any(d => d.Name.Equals("Subject")))
                    {
                        var dataDefinition = definition.Data.First(d => d.Name.Equals("Subject"));
                        if (dataDefinition.Value.StartsWith("field.", StringComparison.OrdinalIgnoreCase))
                        {
                            var fieldName = dataDefinition.Value.Replace("field.", string.Empty);
                            res.DateStart = (DateTime)data[fieldName];
                        }
                        else if (dataDefinition.Value.Equals("today", StringComparison.OrdinalIgnoreCase))
                        {
                            res.DateStart = DateTime.Now;
                        }
                    }

                    if (definition.Data.Any(d => d.Name.Equals("DateStart")))
                    {
                        var dataDefinition = definition.Data.First(d => d.Name.Equals("DateStart"));
                        if(dataDefinition.Value.StartsWith("field.", StringComparison.OrdinalIgnoreCase))
                        {
                            var fieldName = dataDefinition.Value.Replace("field.", string.Empty);
                            res.DateStart = (DateTime)data[fieldName];
                        }
                        else if(dataDefinition.Value.Equals("today", StringComparison.OrdinalIgnoreCase)) {
                            res.DateStart = DateTime.Now;
                        }
                    }

                    if (definition.Data.Any(d => d.Name.Equals("DateEnd")))
                    {
                        var dataDefinition = definition.Data.First(d => d.Name.Equals("DateEnd"));
                        if (dataDefinition.Value.StartsWith("field.", StringComparison.OrdinalIgnoreCase))
                        {
                            var fieldName = dataDefinition.Value.Replace("field.", string.Empty);
                            res.DateStart = (DateTime)data[fieldName];
                        }
                        else if (dataDefinition.Value.Equals("today", StringComparison.OrdinalIgnoreCase))
                        {
                            res.DateStart = DateTime.Now;
                        }
                    }

                    if (definition.Data.Any(d => d.Name.Equals("Location")))
                    {
                        var dataDefinition = definition.Data.First(d => d.Name.Equals("Location"));
                        if (dataDefinition.Value.StartsWith("field.", StringComparison.OrdinalIgnoreCase))
                        {
                            var fieldName = dataDefinition.Value.Replace("field.", string.Empty);
                            res.Location = data[fieldName] as string;
                        }
                    }

                    if (definition.Data.Any(d => d.Name.Equals("ExtraDataText1")))
                    {
                        var dataDefinition = definition.Data.First(d => d.Name.Equals("ExtraDataText1"));
                        if (dataDefinition.Value.StartsWith("field.", StringComparison.OrdinalIgnoreCase))
                        {
                            var fieldName = dataDefinition.Value.Replace("field.", string.Empty);
                            res.Location = data[fieldName] as string;
                        }
                    }

                    if (definition.Data.Any(d => d.Name.Equals("ExtraDataText2")))
                    {
                        var dataDefinition = definition.Data.First(d => d.Name.Equals("ExtraDataText2"));
                        if (dataDefinition.Value.StartsWith("field.", StringComparison.OrdinalIgnoreCase))
                        {
                            var fieldName = dataDefinition.Value.Replace("field.", string.Empty);
                            res.Location = data[fieldName] as string;
                        }
                    }

                }
            }

            return res;
        }

        public static string JsonList(ReadOnlyCollection<Event> list)
        {
            var res = new StringBuilder("[");
            if(list != null && list.Count > 0)
            {
                var first = true;
                foreach(var item in list)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(item.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        private string GroupsJson
        {
            get
            {
                if (this.targetGroupsId == null)
                {
                    return Constant.JavaScriptNull;
                }

                var res = new StringBuilder("[");
                var first = true;
                foreach (var group in this.targetGroupsId)
                {
                    res.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}", first ? string.Empty : ",", group);
                    first = false;
                }

                res.Append("]");
                return res.ToString();
            }
        }

        private string UsersJson
        {
            get
            {
                if (this.targetUsersId == null)
                {
                    return Constant.JavaScriptNull;
                }

                var res = new StringBuilder("[");
                var first = true;
                foreach (var user in this.targetUsersId)
                {
                    res.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}", first ? string.Empty : ",", user);
                    first = false;
                }

                res.Append("]");
                return res.ToString();
            }
        }

        public ReadOnlyCollection<long> TargetUsersId
        {
            get
            {
                if (this.targetUsersId == null)
                {
                    this.targetUsersId = new List<long>();
                }

                return new ReadOnlyCollection<long>(this.targetUsersId);
            }
        }

        public ReadOnlyCollection<long> TargetGroupsId
        {
            get
            {
                if (this.targetGroupsId == null)
                {
                    this.targetGroupsId = new List<long>();
                }

                return new ReadOnlyCollection<long>(this.targetGroupsId);
            }
        }

        public void AddUserId(long userId)
        {
            if (this.TargetUsersId == null)
            {
                this.targetUsersId = new List<long>();
            }

            this.targetUsersId.Add(userId);
        }

        public void AddGroupId(long groupId)
        {
            if (this.targetGroupsId == null)
            {
                this.targetGroupsId = new List<long>();
            }

            this.targetGroupsId.Add(groupId);
        }

        public static ActionResult Activate(long id, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Calendar_Event_Activate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
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

        public static ActionResult Inactivate(long id, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Calendar_Event_Inactivate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
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

        public ActionResult Insert(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[Calendar_Event_Insert]
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @Type nchar(1),
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @TargetUsers nchar(100),
             *   @TargetGroups nchar(100),
             *   @DateStart datetime,
             *   @DateEnd datetime,
             *   @Title nvarchar(50),
             *   @Text nvarchar(500),
             *   @Location nvarchar(50),
             *   @Color nchar(6),
             *   @TextColor nchar(6),
             *   @Icon nchar(50),
             *   @ExtraDataBool1 bit,
             *   @ExtraDataBool2 bit,
             *   @ExtraDataText1 nvarchar(50),
             *   @ExtraDataText2 nvarchar(50),
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            using (var cmd = new SqlCommand("Calendar_Event_Insert"))
            {
                using (var cnn = new SqlConnection(cns))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Type", this.EventType, 1));
                    cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                    cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                    cmd.Parameters.Add(DataParameter.Input("@TargetUsers", string.Empty, 100));
                    cmd.Parameters.Add(DataParameter.Input("@TargetGroups", string.Empty, 100));
                    cmd.Parameters.Add(DataParameter.Input("@DateStart", this.DateStart));
                    cmd.Parameters.Add(DataParameter.Input("@DateEnd", this.DateEnd));
                    cmd.Parameters.Add(DataParameter.Input("@Title", this.Title, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Text", this.Text, 500));
                    cmd.Parameters.Add(DataParameter.Input("@Location", this.Location, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Color", this.Color, 6));
                    cmd.Parameters.Add(DataParameter.Input("@TextColor", this.TextColor, 6));
                    cmd.Parameters.Add(DataParameter.Input("@Icon", this.Icon, 50));
                    cmd.Parameters.Add(DataParameter.Input("@ExtraDataBool1", this.ExtraDataBool1));
                    cmd.Parameters.Add(DataParameter.Input("@ExtraDataBool2", this.ExtraDataBool2));
                    cmd.Parameters.Add(DataParameter.Input("@ExtraDataText1", this.ExtraDataText1, 50));
                    cmd.Parameters.Add(DataParameter.Input("@ExtraDataText2", this.ExtraDataText2, 50));
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

        public ActionResult Update(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Calendar_Event_Update
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @Type nchar(1),
             *   @DateStart datetime,
             *   @DateEnd datetime,
             *   @Title nvarchar(50),
             *   @Text nvarchar(500),
             *   @Location nvarchar(50),
             *   @Color nchar(6),
             *   @TextColor nchar(6),
             *   @Icon nchar(50),
             *   @ApplicationUserId bigint */

            var cns = Persistence.ConnectionString(instanceName);
            using (var cmd = new SqlCommand("Calendar_Event_Update"))
            {
                using (var cnn = new SqlConnection(cns))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@DateStart", this.DateStart));
                    cmd.Parameters.Add(DataParameter.Input("@DateEnd", this.DateEnd));
                    cmd.Parameters.Add(DataParameter.Input("@Title", this.Title, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Text", this.Text, 500));
                    cmd.Parameters.Add(DataParameter.Input("@Location", this.Location, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Color", this.Color, 6));
                    cmd.Parameters.Add(DataParameter.Input("@TextColor", this.TextColor, 6));
                    cmd.Parameters.Add(DataParameter.Input("@Icon", this.Icon, 50));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
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

        public static ReadOnlyCollection<Event> ByMonth(long userId, int month, int year, long companyId, string instanceName)
        {
            var res = new List<Event>();
            var cns = Persistence.ConnectionString(instanceName);
            using (var cmd = new SqlCommand("Calendar_MonthByUser"))
            {
                using (var cnn = new SqlConnection(cns))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@Month", month));
                    cmd.Parameters.Add(DataParameter.Input("@Year", year));
                    try
                    {
                        cmd.Connection.Open();
                        using(var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newEvent = new Event
                                {
                                    Id = rdr.GetInt64(ColumnsEventGet.Id),
                                    CompanyId = rdr.GetInt64(ColumnsEventGet.CompanyId),
                                    EventType = rdr.GetString(ColumnsEventGet.EventType),
                                    ItemDefinitionId = rdr.GetInt64(ColumnsEventGet.ItemDefinitionId),
                                    ItemId = rdr.GetInt64(ColumnsEventGet.ItemId),
                                    DateStart = rdr.GetDateTime(ColumnsEventGet.DateStart),
                                    Title = rdr.GetString(ColumnsEventGet.Title),
                                    Text = rdr.GetString(ColumnsEventGet.Text),
                                    Location = rdr.GetString(ColumnsEventGet.Location),
                                    Color = rdr.GetString(ColumnsEventGet.Color),
                                    TextColor = rdr.GetString(ColumnsEventGet.TextColor),
                                    Icon = rdr.GetString(ColumnsEventGet.Icon),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsEventGet.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsEventGet.CreatedBy),
                                            Name = rdr.GetString(ColumnsEventGet.CreatedByFirstName),
                                            LastName = rdr.GetString(ColumnsEventGet.CreatedByLastName),
                                            LastName2 = rdr.GetString(ColumnsEventGet.CreatedByLastName2)
                                        }
                                    },
                                    createdOn = rdr.GetDateTime(ColumnsEventGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsEventGet.ModifiedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsEventGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnsEventGet.ModifiedByFirstName),
                                            LastName = rdr.GetString(ColumnsEventGet.ModifiedByLastName),
                                            LastName2 = rdr.GetString(ColumnsEventGet.ModifiedByLastName2)
                                        }
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsEventGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsEventGet.Active),
                                    targetGroupsId = new List<long>(),
                                    targetUsersId = new List<long>()
                                };

                                if (!rdr.IsDBNull(ColumnsEventGet.DateEnd))
                                {
                                    newEvent.DateEnd = rdr.GetDateTime(ColumnsEventGet.DateEnd);
                                }

                                if (!rdr.IsDBNull(ColumnsEventGet.ExtraDataBool1))
                                {
                                    newEvent.ExtraDataBool1 = rdr.GetBoolean(ColumnsEventGet.ExtraDataBool1);
                                }

                                if (!rdr.IsDBNull(ColumnsEventGet.ExtraDataBool2))
                                {
                                    newEvent.ExtraDataBool2 = rdr.GetBoolean(ColumnsEventGet.ExtraDataBool2);
                                }

                                if (!rdr.IsDBNull(ColumnsEventGet.ExtraDataText1))
                                {
                                    newEvent.ExtraDataText1 = rdr.GetString(ColumnsEventGet.ExtraDataText1);
                                }

                                if (!rdr.IsDBNull(ColumnsEventGet.ExtraDataText2))
                                {
                                    newEvent.ExtraDataText2 = rdr.GetString(ColumnsEventGet.ExtraDataText2);
                                }

                                res.Add(newEvent);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.Trace(ex, "Event::ByMonth");
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

            return new ReadOnlyCollection<Event>(res);
        }

        public static ReadOnlyCollection<Event> ByMonth(int month, int year, long companyId, string source, string instanceName)
        {
            var res = new List<Event>();
            var cns = Persistence.ConnectionString(instanceName);
            var sourceData = Source.FromFile(source, instanceName);

            using (var cmd = new SqlCommand(sourceData.FinalQuery(month, year)))
            {
                using (var cnn = new SqlConnection(cns))
                {
                    cmd.Connection = cnn;
                    if (sourceData.QueryType.Equals("text", StringComparison.OrdinalIgnoreCase))
                    {
                        cmd.CommandType = CommandType.Text;
                    }
                    else
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                    }

                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newEvent = new Event
                                {
                                    Id = rdr.GetInt64(ColumnsEventGet.Id),
                                    CompanyId = rdr.GetInt64(ColumnsEventGet.CompanyId),
                                    EventType = rdr.GetString(ColumnsEventGet.EventType),
                                    ItemDefinitionId = rdr.GetInt64(ColumnsEventGet.ItemDefinitionId),
                                    ItemId = rdr.GetInt64(ColumnsEventGet.ItemId),
                                    DateStart = rdr.GetDateTime(ColumnsEventGet.DateStart),
                                    Title = rdr.GetString(ColumnsEventGet.Title),
                                    Text = rdr.GetString(ColumnsEventGet.Text),
                                    Location = rdr.GetString(ColumnsEventGet.Location),
                                    Color = rdr.GetString(ColumnsEventGet.Color),
                                    TextColor = rdr.GetString(ColumnsEventGet.TextColor),
                                    Icon = rdr.GetString(ColumnsEventGet.Icon),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsEventGet.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsEventGet.CreatedBy),
                                            Name = rdr.GetString(ColumnsEventGet.CreatedByFirstName),
                                            LastName = rdr.GetString(ColumnsEventGet.CreatedByLastName),
                                            LastName2 = rdr.GetString(ColumnsEventGet.CreatedByLastName2)
                                        }
                                    },
                                    createdOn = rdr.GetDateTime(ColumnsEventGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsEventGet.ModifiedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsEventGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnsEventGet.ModifiedByFirstName),
                                            LastName = rdr.GetString(ColumnsEventGet.ModifiedByLastName),
                                            LastName2 = rdr.GetString(ColumnsEventGet.ModifiedByLastName2)
                                        }
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsEventGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsEventGet.Active),
                                    targetGroupsId = new List<long>(),
                                    targetUsersId = new List<long>()
                                };

                                if (!rdr.IsDBNull(ColumnsEventGet.DateEnd))
                                {
                                    newEvent.DateEnd = rdr.GetDateTime(ColumnsEventGet.DateEnd);
                                }

                                if (!rdr.IsDBNull(ColumnsEventGet.ExtraDataBool1))
                                {
                                    newEvent.ExtraDataBool1 = rdr.GetBoolean(ColumnsEventGet.ExtraDataBool1);
                                }

                                if (!rdr.IsDBNull(ColumnsEventGet.ExtraDataBool2))
                                {
                                    newEvent.ExtraDataBool2 = rdr.GetBoolean(ColumnsEventGet.ExtraDataBool2);
                                }

                                if (!rdr.IsDBNull(ColumnsEventGet.ExtraDataText1))
                                {
                                    newEvent.ExtraDataText1 = rdr.GetString(ColumnsEventGet.ExtraDataText1);
                                }

                                if (!rdr.IsDBNull(ColumnsEventGet.ExtraDataText2))
                                {
                                    newEvent.ExtraDataText2 = rdr.GetString(ColumnsEventGet.ExtraDataText2);
                                }

                                res.Add(newEvent);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.Trace(ex, "Event::ByMonth");
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

            return new ReadOnlyCollection<Event>(res);
        }
    }
}