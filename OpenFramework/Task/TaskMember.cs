// --------------------------------
// <copyright file="TaskMember.cs" company="OpenFramework">
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
    using System.Text;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;
    using OpenFrameworkV3.Task.Bindings;

    public class TaskMember
    {
        public long Id { get; set; }
        public ApplicationUser MemberUser { get; set; }
        public string MemberName { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public static string JsonList(ReadOnlyCollection<TaskMember> list)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var member in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(member.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<TaskMember> All(string instanceName)
        {
            var res = new List<TaskMember>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Task_Members_GetAll"))
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
                                    var newMember = new TaskMember
                                    {
                                        Id = rdr.GetInt64(ColumnTaskMemberGet.Id),
                                        MemberName = rdr.GetString(ColumnTaskMemberGet.Name),
                                        MemberUser = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnTaskMemberGet.UserId),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnTaskMemberGet.UserId),
                                                Name = rdr.GetString(ColumnTaskMemberGet.UserName),
                                                LastName = rdr.GetString(ColumnTaskMemberGet.UserLastName),
                                                LastName2 = rdr.GetString(ColumnTaskMemberGet.UserLastName2)
                                            }
                                        },
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnTaskMemberGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnTaskMemberGet.CreatedBy),
                                                Name = rdr.GetString(ColumnTaskMemberGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnTaskMemberGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnTaskMemberGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnTaskMemberGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnTaskMemberGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnTaskMemberGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnTaskMemberGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnTaskMemberGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnTaskMemberGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnTaskMemberGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnTaskMemberGet.Active)
                                    };

                                    res.Add(newMember);
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

            return new ReadOnlyCollection<TaskMember>(res);
        }

        public static ReadOnlyCollection<TaskMember> ByTask(long taskId, string instanceName)
        {
            var res = new List<TaskMember>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Task_GetMembers"))
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
                                    var newMember = new TaskMember
                                    {
                                        Id = rdr.GetInt64(ColumnTaskMemberGet.Id),
                                        MemberName = rdr.GetString(ColumnTaskMemberGet.Name),
                                        MemberUser = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnTaskMemberGet.UserId),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnTaskMemberGet.UserId),
                                                Name = rdr.GetString(ColumnTaskMemberGet.UserName),
                                                LastName = rdr.GetString(ColumnTaskMemberGet.UserLastName),
                                                LastName2 = rdr.GetString(ColumnTaskMemberGet.UserLastName2)
                                            }
                                        },
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnTaskMemberGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnTaskMemberGet.CreatedBy),
                                                Name = rdr.GetString(ColumnTaskMemberGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnTaskMemberGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnTaskMemberGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnTaskMemberGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnTaskMemberGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnTaskMemberGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnTaskMemberGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnTaskMemberGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnTaskMemberGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnTaskMemberGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnTaskMemberGet.Active)
                                    };

                                    res.Add(newMember);
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

            return new ReadOnlyCollection<TaskMember>(res);
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"":{0},
                        ""MemberUser"":{1},
                        ""MemberName"":""{2}"",
                        ""CreatedBy"":{3},
                        ""CreatedOn"":""{4:dd/MM/yyyy}"",
                        ""ModifiedBy"":{5},
                        ""ModifiedOn"":""{6:dd/MM/yyyy}"",
                        ""Active"":{7}}}",
                    this.Id,
                    this.MemberUser.JsonKeyValue,
                    Tools.Json.JsonCompliant(this.MemberName),
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static TaskMember Empty
        {
            get
            {
                return new TaskMember
                {
                    Id = Constant.DefaultId,
                    MemberUser = ApplicationUser.Empty,
                    MemberName = string.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public ActionResult Activate(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Task_Member_Activate"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@TaskMemberId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
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
                using (var cmd = new SqlCommand("Core_Task_Member_Inactivate"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@TaskMemberId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
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

        public ActionResult Insert(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Task_Member_Isert"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.OutputLong("@TaskMemberId"));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", this.MemberUser.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Name", this.MemberName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@TaskMemberId"].Value);
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
            /* ALTER PROCEDURE [dbo].[Core_Task_Member_Update]
             *  @TaskMemberId bigint,
             *  @UserId bigint,
             *  @Name nvarchar(50),
             *  @ApplicationUserId bigint */
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Task_Member_Update"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@TaskMemberId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", this.MemberUser.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Name", this.MemberName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
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
                        catch (NotSupportedException ex)
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