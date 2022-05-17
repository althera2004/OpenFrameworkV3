// --------------------------------
// <copyright file="ActivityLog.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.Activity
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using OpenFramework.DataAccess;

    /// <summary>Implements Activity log class</summary>
    public static class ActivityLog
    {
        /// <summary>Generate a trace</summary>
        /// <param name="targetId">Identity of the insert log activity</param>
        /// <param name="userId">Identity of the user</param>
        /// <param name="companyId">Identity of the company</param>
        /// <param name="actionId">Action identity from specific action's Insert log activity</param>
        /// <param name="extraData">Extra data if needed</param>
        /// <returns>Return of the action</returns>
        public static ActionResult InsertLogActivity(int targetId, int userId, int companyId, int actionId, string extraData)
        {
            var res = ActionResult.NoAction;
            string storedProcedureName = string.Empty;

            if (string.IsNullOrEmpty(storedProcedureName))
            {
                res.MessageError = "No valid item";
            }

            using (var cmd = new SqlCommand(storedProcedureName))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@TargetId", targetId));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@ActionId", actionId));
                    cmd.Parameters.Add(DataParameter.Input("@ExtraData", extraData, 150));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch (NullReferenceException ex)
                {
                    res.SetFail(ex.Message);
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex.Message);
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return res;
        }

        /// <summary>Generate a trace</summary>
        /// <param name="targetType">Identity of the update sumary</param>
        /// <param name="targetId">Identity of the insert log activity</param>
        /// <param name="userId">Identity of the user</param>
        /// <param name="companyId">Identity of the company</param>
        /// <param name="actionId">Action identity from specific action's Insert log activity</param>
        /// <param name="extraData">Extra data if needed</param>
        /// <returns>Return of the action</returns>
        public static ActionResult InsertLogActivity(TargetType targetType, long targetId, int userId, int companyId, int actionId, string extraData)
        {
            var res = ActionResult.NoAction;
            string storedProcedureName = string.Empty;

            switch (targetType)
            {
                case TargetType.Company:
                    storedProcedureName = "ActivityCompany";
                    break;
                case TargetType.User:
                    storedProcedureName = "ActivityUser";
                    break;
                case TargetType.SecurityGroup:
                    storedProcedureName = "ActivitySecurityGroup";
                    break;
                case TargetType.Document:
                    storedProcedureName = "ActivityDocument";
                    break;
                case TargetType.Department:
                    storedProcedureName = "ActivityDepartment";
                    break;
                case TargetType.LogOn:
                    storedProcedureName = "ActivityLogin";
                    break;
                case TargetType.Employee:
                    storedProcedureName = "ActivityEmployee";
                    break;
                case TargetType.JobPosition:
                    storedProcedureName = "ActivityJobPosition";
                    break;
                case TargetType.Process:
                    storedProcedureName = "ActivityProcess";
                    break;
                case TargetType.Learning:
                    storedProcedureName = "ActivityLearning";
                    break;
                case TargetType.Provider:
                    storedProcedureName = "ActivityProvider";
                    break;
                case TargetType.Customer:
                    storedProcedureName = "ActivityCustomer";
                    break;
                default:
                    storedProcedureName = string.Empty;
                    break;
            }

            if (string.IsNullOrEmpty(storedProcedureName))
            {
                res.MessageError = "No valid item";
            }

            using (var cmd = new SqlCommand(storedProcedureName))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@TargetId", targetId));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@ActionId", actionId));
                    cmd.Parameters.Add(DataParameter.Input("@ExtraData", extraData, 150));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch (NullReferenceException ex)
                {
                    res.SetFail(ex.Message);
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex.Message);
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return res;
        }

        /// <summary>Generate a trace</summary>
        /// <param name="itemId">identifier of item search, optional</param>
        /// <param name="targetType">type of item</param>
        /// <param name="companyId">Type of companyid</param>
        /// <param name="from">Date of start periode, not required</param>
        /// <param name="to">Date of end periode, not required</param>
        /// <returns>Return a list of log activity matching filter conditions, ordered from most recent</returns>
        public static ReadOnlyCollection<ActivityTrace> GetActivity(long itemId, TargetType targetType, long companyId, DateTime? from, DateTime? to)
        {
            var res = new List<ActivityTrace>();
            /* ALTER PROCEDURE [dbo].[Get_Activity]
             * @CompanyId int,
             * @TargetType int,
             * @ItemId int,
             * @From date,
             * @To date */

            using (var cmd = new SqlCommand("Get_Activity"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@TargetType", (int)targetType));
                    cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                    cmd.Parameters.Add(DataParameter.Input("@From", from));
                    cmd.Parameters.Add(DataParameter.Input("@To", to));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new ActivityTrace
                                {
                                    Date = rdr.GetDateTime(1),
                                    Target = rdr.GetString(4),
                                    Changes = rdr.GetString(6),
                                    ActionEmployee = rdr.GetString(7),
                                    Action = rdr.GetString(5)
                                });
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

            return new ReadOnlyCollection<ActivityTrace>(res);
        }

        /// <summary>Generate a trace</summary>
        /// <param name="itemId">identifier of item search, optional</param>
        /// <param name="targetType">type of item</param>
        /// <param name="companyId">Type of companyid</param>
        /// <param name="from">Date of start periode, not required</param>
        /// <param name="to">Date of end periode, not required</param>
        /// <returns>Return a list of log activity matching filter conditions, ordered from most recent</returns>
        public static ReadOnlyCollection<ActivityTrace> GetActivity(long itemId, TargetType targetType, int companyId, DateTime? from, DateTime? to)
        {
            var res = new List<ActivityTrace>();
            /* ALTER PROCEDURE [dbo].[Get_Activity]
             * @CompanyId int,
             * @TargetType int,
             * @ItemId int,
             * @From date,
             * @To date */

            using (var cmd = new SqlCommand("Get_Activity"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@TargetType", (int)targetType));
                    cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                    cmd.Parameters.Add(DataParameter.Input("@From", from));
                    cmd.Parameters.Add(DataParameter.Input("@To", to));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new ActivityTrace
                                {
                                    Date = rdr.GetDateTime(1),
                                    Target = rdr.GetString(4),
                                    Changes = rdr.GetString(6),
                                    ActionEmployee = rdr.GetString(7),
                                    Action = rdr.GetString(5)
                                });
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

            return new ReadOnlyCollection<ActivityTrace>(res);
        }

        /// <summary>Get all activity of company in the last 24 hours</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>List of actions</returns>
        public static ReadOnlyCollection<ActivityTrace> GetActivity24H(int companyId)
        {
            var res = new List<ActivityTrace>();
            /* ALTER PROCEDURE Get_ActivityLastDay
             * @CompanyId int */

            using (var cmd = new SqlCommand("Get_ActivityLastDay"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new ActivityTrace
                                {
                                    Date = rdr.GetDateTime(1),
                                    Target = rdr.GetString(4),
                                    Changes = rdr.GetString(6),
                                    ActionEmployee = rdr.GetString(7),
                                    Action = rdr.GetString(5),
                                    TargetId = Convert.ToInt32(rdr["TargetId"].ToString(), CultureInfo.InvariantCulture),
                                    Description = rdr["Description"].ToString()
                                });
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

            return new ReadOnlyCollection<ActivityTrace>(res);
        }
    }
}