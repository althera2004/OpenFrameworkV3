// --------------------------------
// <copyright file="Task.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Task
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
    using OpenFrameworkV3.Core.Security;
    using OpenFrameworkV3.Task.Bindings;

    public class Task
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Style { get; set; }
        public string Link { get; set; }
        public long pmile { get; set; }
        public string res { get; set; }
        public decimal Comp { get; set; }
        public long Group { get; set; }
        public long Parent { get; set; }
        public int Open { get; set; }
        public string Depend { get; set; }
        public string Caption { get; set; }
        public string Notes { get; set; }
        public int PanelId { get; set; }

        public ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public string InstanceName { get; set; }

        private List<TaskMember> members;

        public static Task Empty
        {
            get
            {
                return new Task
                {
                    Id = Constant.DefaultId,
                    Name = string.Empty,
                    Notes = string.Empty,
                    Open = 0,
                    PanelId = 0,
                    members = new List<TaskMember>(),
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public static string JsonList(ReadOnlyCollection<Task> list)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach(var task in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(task.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"
                     {{
                         ""Id"": {0},
                         ""Label"": ""{1}"", 
                         ""Start"": ""{2:yyyy-MM-dd}"", 
                         ""End"": ""{3:yyyy-MM-dd}"", 
                         ""Style"": ""{4}"", 
                         ""Link"": ""{5}"", 
                         ""Notes"": ""{6}"", 
                         ""Group"": {7}, 
                         ""Comp"": {8}, 
                         ""Parent"": {9}, 
                         ""Open"": {10}, 
                         ""Depend"": ""{11}"", 
                         ""Caption"": ""{12}"" ,
                        ""PanelId"":{13}
                         }}",
                    this.Id,
                    Tools.Json.JsonCompliant(this.Name),
                    this.Start,
                    this.End,
                    this.Style,
                    this.Link,
                    Tools.Json.JsonCompliant(this.Notes),
                    this.Group,
                    this.Comp,
                    this.Parent,
                    this.Open,
                    this.Depend,
                    Tools.Json.JsonCompliant(this.Caption),
                    this.PanelId);
            }
        }

        public ReadOnlyCollection<TaskMember> Members
        {
            get
            {
                if(this.members == null)
                {
                    this.members = new List<TaskMember>();
                }

                return new ReadOnlyCollection<TaskMember>(this.members);
            }
        }

        public void ObtainMembers()
        {
            if (this.members == null)
            {
                this.members = new List<TaskMember>();
            }

            this.members = TaskMember.ByTask(this.Id, this.InstanceName).ToList();
        }

        public static ReadOnlyCollection<Task> All(string instanceName)
        {
            var res = new List<Task>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Task_GetAll"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new Task
                                    {
                                        Id = rdr.GetInt64(ColumnTaskGetAll.Id),
                                        Name = rdr.GetString(ColumnTaskGetAll.Name),
                                        Start = rdr.GetDateTime(ColumnTaskGetAll.Start),
                                        End = rdr.GetDateTime(ColumnTaskGetAll.End),
                                        Caption = rdr.GetString(ColumnTaskGetAll.Caption),
                                        Comp = rdr.GetDecimal(ColumnTaskGetAll.Complete),
                                        Depend = rdr.GetString(ColumnTaskGetAll.Depend),
                                        Group = rdr.GetInt64(ColumnTaskGetAll.Group),
                                        Link = rdr.GetString(ColumnTaskGetAll.Link),
                                        Notes = rdr.GetString(ColumnTaskGetAll.Notes),
                                        res = rdr.GetString(ColumnTaskGetAll.Resource),
                                        Open = 0,
                                        members = new List<TaskMember>(),
                                        Parent = rdr.GetInt64(ColumnTaskGetAll.Parent),
                                        Style = rdr.GetString(ColumnTaskGetAll.Style),
                                        pmile = rdr.GetInt64(ColumnTaskGetAll.Mile),
                                        PanelId = rdr.GetInt32(ColumnTaskGetAll.PanelId),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnTaskGetAll.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnTaskGetAll.CreatedBy),
                                                Name = rdr.GetString(ColumnTaskGetAll.Name),
                                                LastName = rdr.GetString(ColumnTaskGetAll.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnTaskGetAll.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnTaskGetAll.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnTaskGetAll.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnTaskGetAll.ModifiedBy),
                                                Name = rdr.GetString(ColumnTaskGetAll.ModifiedByName),
                                                LastName = rdr.GetString(ColumnTaskGetAll.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnTaskGetAll.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnTaskGetAll.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnTaskGetAll.Active)
                                    });
                                }
                            }
                        }
                        finally
                        {
                            if (cmd.Connection.State == ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return new ReadOnlyCollection<Task>(res);
        }

        public static ReadOnlyCollection<Task> ByUser(long userId, string instanceName)
        {
            var res = new List<Task>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Task_GetByUserId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", userId));
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new Task
                                    {
                                        Id = rdr.GetInt64(ColumnTaskGetAll.Id),
                                        Name = rdr.GetString(ColumnTaskGetAll.Name),
                                        Start = rdr.GetDateTime(ColumnTaskGetAll.Start),
                                        End = rdr.GetDateTime(ColumnTaskGetAll.End),
                                        Caption = rdr.GetString(ColumnTaskGetAll.Caption),
                                        Comp = rdr.GetDecimal(ColumnTaskGetAll.Complete),
                                        Depend = rdr.GetString(ColumnTaskGetAll.Depend),
                                        Group = rdr.GetInt64(ColumnTaskGetAll.Group),
                                        Link = rdr.GetString(ColumnTaskGetAll.Link),
                                        Notes = rdr.GetString(ColumnTaskGetAll.Notes),
                                        res = rdr.GetString(ColumnTaskGetAll.Resource),
                                        Open = 0,
                                        members = new List<TaskMember>(),
                                        Parent = rdr.GetInt64(ColumnTaskGetAll.Parent),
                                        Style = rdr.GetString(ColumnTaskGetAll.Style),
                                        pmile = rdr.GetInt64(ColumnTaskGetAll.Mile),
                                        PanelId = rdr.GetInt32(ColumnTaskGetAll.PanelId),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnTaskGetAll.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnTaskGetAll.CreatedBy),
                                                Name = rdr.GetString(ColumnTaskGetAll.Name),
                                                LastName = rdr.GetString(ColumnTaskGetAll.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnTaskGetAll.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnTaskGetAll.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnTaskGetAll.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnTaskGetAll.ModifiedBy),
                                                Name = rdr.GetString(ColumnTaskGetAll.ModifiedByName),
                                                LastName = rdr.GetString(ColumnTaskGetAll.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnTaskGetAll.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnTaskGetAll.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnTaskGetAll.Active)
                                    });
                                }
                            }
                        }
                        finally
                        {
                            if (cmd.Connection.State == ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return new ReadOnlyCollection<Task>(res);
        }

        public static Task ById(long taskId, string instanceName)
        {
            var res = Task.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Task_GetById"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@TaskId", taskId));
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Id = rdr.GetInt64(ColumnTaskGetAll.Id);
                                    res.Name = rdr.GetString(ColumnTaskGetAll.Name);
                                    res.Start = rdr.GetDateTime(ColumnTaskGetAll.Start);
                                    res.End = rdr.GetDateTime(ColumnTaskGetAll.End);
                                    res.Caption = rdr.GetString(ColumnTaskGetAll.Caption);
                                    res.Comp = rdr.GetDecimal(ColumnTaskGetAll.Complete);
                                    res.Depend = rdr.GetString(ColumnTaskGetAll.Depend);
                                    res.Group = rdr.GetInt64(ColumnTaskGetAll.Group);
                                    res.Link = rdr.GetString(ColumnTaskGetAll.Link);
                                    res.Notes = rdr.GetString(ColumnTaskGetAll.Notes);
                                    res.res = rdr.GetString(ColumnTaskGetAll.Resource);
                                    res.Parent = rdr.GetInt64(ColumnTaskGetAll.Parent);
                                    res.Style = rdr.GetString(ColumnTaskGetAll.Style);
                                    res.pmile = rdr.GetInt64(ColumnTaskGetAll.Mile);
                                    res.PanelId = rdr.GetInt32(ColumnTaskGetAll.PanelId);
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnTaskGetAll.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnTaskGetAll.CreatedBy),
                                            Name = rdr.GetString(ColumnTaskGetAll.Name),
                                            LastName = rdr.GetString(ColumnTaskGetAll.CreatedByLastName),
                                            LastName2 = rdr.GetString(ColumnTaskGetAll.CreatedByLastName2)
                                        }
                                    };
                                    res.CreatedOn = rdr.GetDateTime(ColumnTaskGetAll.CreatedOn);
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnTaskGetAll.ModifiedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnTaskGetAll.ModifiedBy),
                                            Name = rdr.GetString(ColumnTaskGetAll.ModifiedByName),
                                            LastName = rdr.GetString(ColumnTaskGetAll.ModifiedByLastName),
                                            LastName2 = rdr.GetString(ColumnTaskGetAll.ModifiedByLastName2)
                                        }
                                    };
                                    res.ModifiedOn = rdr.GetDateTime(ColumnTaskGetAll.ModifiedOn);
                                    res.Active = rdr.GetBoolean(ColumnTaskGetAll.Active);
                                }
                            }
                        }
                        finally
                        {
                            if (cmd.Connection.State == ConnectionState.Closed)
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
            /* CREATE PROCEDURE Core_Task_Insert
             *   @TaskId bigint output,
             *   @Name nvarchar(50),
             *   @Start datetime,
             *   @End datetime,
             *   @Style nvarchar(50),
             *   @Link nvarchar(150),
             *   @Mile bigint,
             *   @Resource nvarchar(50),
             *   @Complete decimal(5,2),
             *   @Group bigint,
             *   @Parent bigint,
             *   @Depend nvarchar(50),
             *   @Caption nvarchar(50),
             *   @Notes nvarchar(250),
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Task_Insert"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputLong("@TaskId"));
                    cmd.Parameters.Add(DataParameter.Input("@Name", this.Name, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Start", this.Start));
                    cmd.Parameters.Add(DataParameter.Input("@End", this.End));
                    cmd.Parameters.Add(DataParameter.Input("@Style", this.Style, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Link", this.Link, 150));
                    cmd.Parameters.Add(DataParameter.Input("@Mile", this.pmile));
                    cmd.Parameters.Add(DataParameter.Input("@Resource", this.res, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Complete", this.Comp));
                    cmd.Parameters.Add(DataParameter.Input("@Group", this.Group));
                    cmd.Parameters.Add(DataParameter.Input("@Parent", this.Parent));
                    cmd.Parameters.Add(DataParameter.Input("@Depend", this.Depend, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Caption", this.Caption, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, 250));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@TaskId"].Value);
                            res.SetSuccess(this.Id);
                        }
                        catch (SqlException ex)
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

        public ActionResult Update(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_Task_Update
             *   @TaskId bigint,
             *   @Name nvarchar(50),
             *   @Start datetime,
             *   @End datetime,
             *   @Style nvarchar(50),
             *   @Link nvarchar(150),
             *   @Mile bigint,
             *   @Resource nvarchar(50),
             *   @Complete decimal(5,2),
             *   @Group bigint,
             *   @Parent bigint,
             *   @Depend nvarchar(50),
             *   @Caption nvarchar(50),
             *   @Notes nvarchar(250),
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Task_Update"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@TaskId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@Name", this.Name, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Start", this.Start));
                    cmd.Parameters.Add(DataParameter.Input("@End", this.End));
                    cmd.Parameters.Add(DataParameter.Input("@Style", this.Style, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Link", this.Link, 150));
                    cmd.Parameters.Add(DataParameter.Input("@Mile", this.pmile));
                    cmd.Parameters.Add(DataParameter.Input("@Resource", this.res, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Complete", this.Comp));
                    cmd.Parameters.Add(DataParameter.Input("@Group", this.Group));
                    cmd.Parameters.Add(DataParameter.Input("@Parent", this.Parent));
                    cmd.Parameters.Add(DataParameter.Input("@Depend", this.Depend, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Caption", this.Caption, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, 250));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(this.Id);
                        }
                        catch (SqlException ex)
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

        public ActionResult Activate(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Task_Activate"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@TaskId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(this.Id);
                        }
                        catch (SqlException ex)
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

        public static ActionResult MovePanel(string instanceName, long taskId, int panelId, long applicationUserId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_Task_MovePanel
             *   @TaskIid bigint,
             *   @PanelId int,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Task_MovePanel"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@TaskId", taskId));
                    cmd.Parameters.Add(DataParameter.Input("@PanelId", panelId));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
                        }
                        catch (SqlException ex)
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

        public ActionResult Inactivate(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Task_Inactivate"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@TaskId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(this.Id);
                        }
                        catch (SqlException ex)
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
    }
}