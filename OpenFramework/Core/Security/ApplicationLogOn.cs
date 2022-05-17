// --------------------------------
// <copyright file="ApplicationLogOn.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Security
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Web;
    using OpenFramework.Security.Bindings;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Companies;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Navigation;
    using OpenFrameworkV3.Feature;

    /// <summary>Implements ApplicationLogOn class</summary>
    public static class ApplicationLogOn
    {
        /// <summary>Enum for log on result</summary>
        [FlagsAttribute]
        public enum LogOnResult
        {
            /// <summary>None - 0</summary>
            None = 0,

            /// <summary>Ok - 1</summary>
            Ok = 1,

            /// <summary>NoUser - 2</summary>
            NoUser = 2,

            /// <summary>LockUser - 3</summary>
            LockUser = 3,

            /// <summary>Fail - 4</summary>
            Fail = 4,

            /// <summary>Must reset password - 5</summary>
            MustResetPassword = 5,

            /// <summary>Admin - 1001</summary>
            Admin = 1001,

            /// <summary>Administrative - 1002</summary>
            Administrative = 1002,

            /// <summary>PortalUser - 1003</summary>
            PortalUser = 1003
        }

        /// <summary>Gets the log on result from integer value</summary>
        /// <param name="value">Integer that represents a log on result</param>
        /// <returns>Log on result</returns>
        public static LogOnResult IntegerToLogOnResult(int value)
        {
            var res = LogOnResult.Fail;
            switch (value)
            {
                case 1:
                    res = LogOnResult.Ok;
                    break;
                case 2:
                    res = LogOnResult.LockUser;
                    break;
                case 5:
                    res = LogOnResult.MustResetPassword;
                    break;
                case 3:
                    res = LogOnResult.Fail;
                    break;
                case 1001:
                    res = LogOnResult.Admin;
                    break;
                case 1002:
                    res = LogOnResult.Administrative;
                    break;
                default:
                case 0:
                    res = LogOnResult.None;
                    break;
            }

            return res;
        }

        /*
        /// <summary>Trace a log on failed</summary>
        /// <param name="userId">Identifier of user that attemps to log on</param>
        public static void LogOnFailed(long userId, string email, string ip)
        {
            using (var cmd = new SqlCommand("LogonFailed"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        cmd.Connection.Open();
                        cmd.Parameters.Add("@UserId", SqlDbType.Int);
                        cmd.Parameters["@UserId"].Value = userId;
                        cmd.ExecuteNonQuery();
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
        */

        /// <summary>Log on application</summary>
        /// <param name="applicationUserId">User identifier</param>
        /// <param name="pin">Pin code</param>
        /// <param name="imei">Mobile IMEI</param>
        /// <returns>Result of action</returns>
        public static ActionResult GetApplicationAccess(long applicationUserId, string pin, string imei)
        {
            HttpContext.Current.Session["Companies"] = null;
            if (string.IsNullOrEmpty(pin))
            {
                return ActionResult.NoAction;
            }

            var res = ActionResult.NoAction;
            var result = new LogOnObject
            {
                Id = applicationUserId,
                UserName = string.Empty,
                Result = LogOnResult.NoUser,
                InstanceName = string.Empty
            };

            Instance.CheckPersistence();
            var instanceName = Instance.InstanceName;

            var companiesId = new List<string>();
            using (var cmd = new SqlCommand("LoginPIN"))
            {
                using (var cnn = new SqlConnection(Persistence.ConnectionString(instanceName)))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@PIN", pin));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            bool multiCompany = false;
                            if (rdr.HasRows)
                            {
                                while (rdr.Read())
                                {
                                    companiesId.Add(string.Format(
                                        CultureInfo.InvariantCulture,
                                        "{0}|{1}",
                                        rdr.GetInt64(ColumnsGetLogin.CompanyId),
                                        rdr.GetInt64(ColumnsGetLogin.Id)));
                                    result.Id = rdr.GetInt64(ColumnsGetLogin.Id);
                                    result.Result = IntegerToLogOnResult(rdr.GetInt32(ColumnsGetLogin.Status));
                                    result.CompanyId = rdr.GetInt64(ColumnsGetLogin.CompanyId);
                                    result.MustResetPassword = result.Result == LogOnResult.MustResetPassword;
                                    result.InstanceName = HttpContext.Current.Session["InstanceName"].ToString();

                                    if (result.Result != LogOnResult.Fail)
                                    {
                                        HttpContext.Current.Session["AntiCache"] = false;
                                        HttpContext.Current.Session["Languages"] = Language.All(instanceName);

                                        var user = ApplicationUser.ById(applicationUserId, instanceName);
                                        result.UserName = user.Email;
                                        HttpContext.Current.Session["ApplicationUser"] = user;
                                        HttpContext.Current.Session["LoggedUser"] = user;
                                        HttpContext.Current.Session["Dictionary"] = ApplicationDictionary.Load(user.Language.Iso, instanceName);
                                        ApplicationDictionary.CreateJavascriptFile(user.Language.Iso, instanceName);

                                        using (var instance = Persistence.InstanceByName(instanceName))
                                        {
                                            // weke
                                            //if (!multiCompany)
                                            //{
                                            //    if (string.IsNullOrEmpty(user.ExternalUsers))
                                            //    {
                                            Menu.Load(instance.Config.CodedQueryClean, instanceName);
                                            //    }
                                            //    else
                                            //    {
                                            //        Menu.Load(user.ExternalUsers, instance.Config.CodedQueryClean, instanceName);
                                            //    }
                                            //}
                                        }
                                    }

                                    result.MultipleCompany = multiCompany;

                                    if (!multiCompany)
                                    {
                                        HttpContext.Current.Session["Company"] = Company.Default(instanceName);
                                    }
                                }
                            }
                            else
                            {
                                result.Result = LogOnResult.NoUser;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        result.Result = LogOnResult.Fail;
                        result.Id = Constant.DefaultId;
                        result.UserName = ex.Message;
                    }
                    catch (FormatException ex)
                    {
                        result.Result = LogOnResult.Fail;
                        result.Id = Constant.DefaultId;
                        result.UserName = ex.Message;
                    }
                    catch (NullReferenceException ex)
                    {
                        result.Result = LogOnResult.Fail;
                        result.Id = Constant.DefaultId;
                        result.UserName = ex.Message;
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

            bool resultOk = result.Result == LogOnResult.Ok || result.Result == LogOnResult.Admin || result.Result == LogOnResult.Administrative;

            /*if (string.IsNullOrEmpty(imei))
            {
                imei = "no-imei";
            }*/

            HttpContext.Current.Session["Companies"] = companiesId;
            //// InsertLog(result.UserName, imei, resultOk ? 1 : 2, result.Id, result.CompanyId, connectionString);
            res.SetSuccess(result);
            return res;
        }

        /// <summary>Log on application</summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <param name="clientAddress">IP address from log on action</param>
        /// <returns>Result of action</returns>
        public static ActionResult GetApplicationAccess(string email, string password, string clientAddress)
        {
            HttpContext.Current.Session["Companies"] = null;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return ActionResult.NoAction;
            }

            var res = ActionResult.NoAction;
            var result = new LogOnObject
            {
                Id = Constant.DefaultId,
                UserName = string.Empty,
                Result = LogOnResult.NoUser,
                InstanceName = string.Empty,
                Corporative = false
            };

            Instance.CheckPersistence();
            var instanceName = Instance.InstanceName;

            var companiesId = new List<string>();
            using (var instance = Persistence.InstanceByName(instanceName))
            {
                using (var cmd = new SqlCommand(instance.Config.MultiCompany ? "LoginMulticompany" : "Login"))
                {
                    using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DataParameter.Input("@Email", email));
                            cmd.Parameters.Add(DataParameter.Input("@Password", password));
                            cmd.Parameters.Add(DataParameter.Input("@IP", clientAddress));
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                bool multiCompany = false;
                                if (rdr.HasRows)
                                {
                                    while (rdr.Read())
                                    {
                                        var companyId = rdr.GetInt64(ColumnsGetLogin.CompanyId);
                                        var userId = rdr.GetInt64(ColumnsGetLogin.Id);
                                        companiesId.Add(string.Format(CultureInfo.InvariantCulture, "{0}|{1}", companyId, userId));
                                        result.Id = userId;
                                        result.Result = IntegerToLogOnResult(rdr.GetInt32(ColumnsGetLogin.Status));
                                        result.UserName = email;
                                        result.CompanyId = companyId;

                                        if (!instance.Config.MultiCompany)
                                        {
                                            if (rdr.IsDBNull(ColumnsGetLogin.Corporative))
                                            {
                                                result.Corporative = false;
                                            }
                                            else
                                            {
                                                result.Corporative = rdr.GetBoolean(ColumnsGetLogin.Corporative);
                                            }
                                        }

                                        result.MustResetPassword = result.Result == LogOnResult.MustResetPassword;
                                        result.InstanceName = HttpContext.Current.Session["InstanceName"].ToString();

                                        if (result.Result != LogOnResult.Fail)
                                        {
                                            var user = ApplicationUser.ById(userId, instanceName);

                                            if (instance.Config.ActiveAlerts && companyId > 0)
                                            {
                                                HttpContext.Current.Session["AlertsDefinition"] = AlertDefinition.GetFromDisk(instanceName, user.Id);
                                            }
                                            else
                                            {
                                                HttpContext.Current.Session["AlertsDefinition"] = new ReadOnlyCollection<AlertDefinition>(new List<AlertDefinition>());
                                            }

                                            if (instance.Config.WhatYouMissed && companyId > 0)
                                            {
                                                HttpContext.Current.Session["WhatYouMissedDefinition"] = WhayYouMissedDeninition.GetFromDisk(instanceName);
                                            }
                                            else
                                            {
                                                HttpContext.Current.Session["WhatYouMissedDefinition"] = new ReadOnlyCollection<WhayYouMissedDeninition>(new List<WhayYouMissedDeninition>());
                                            }

                                            ApplicationDictionary.CreateJavascriptFile(user.Language.Iso, instanceName);
                                            //if (string.IsNullOrEmpty(user.ExternalUsers))
                                            //{
                                                Menu.Load(instance.Config.CodedQueryClean, instanceName);
                                            //}
                                            //else
                                            //{
                                            //    if (user.Groups.Count > 0)
                                            //    {
                                            //        Menu.Load(user.ExternalUsers, instance.Config.CodedQueryClean, instanceName);
                                            //    }
                                            //    else
                                            //    {
                                            //        result.Result = LogOnResult.PortalUser;
                                            //    }
                                            //}
                                        }

                                        result.MultipleCompany = multiCompany;

                                        // Si no es multicompany se crea una virtual con los datos de la instancia
                                        if (!multiCompany && HttpContext.Current.Session["Company"] == null)
                                        {
                                            var company = Company.Default(instanceName);
                                            HttpContext.Current.Session["Company"] = company;
                                            HttpContext.Current.Session["CompanyId"] = company.Id;
                                        }
                                    }
                                }
                                else
                                {
                                    result.Result = LogOnResult.NoUser;
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            result.Result = LogOnResult.Fail;
                            result.Id = Constant.DefaultId;
                            result.UserName = ex.Message;
                        }
                        catch (FormatException ex)
                        {
                            result.Result = LogOnResult.Fail;
                            result.Id = Constant.DefaultId;
                            result.UserName = ex.Message;
                        }
                        catch (NullReferenceException ex)
                        {
                            result.Result = LogOnResult.Fail;
                            result.Id = Constant.DefaultId;
                            result.UserName = ex.Message;
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

            bool resultOk = result.Result == LogOnResult.Ok || result.Result == LogOnResult.Admin || result.Result == LogOnResult.Administrative;

            /*if (string.IsNullOrEmpty(clientAddress))
            {
                clientAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] as string;
            }*/

            HttpContext.Current.Session["Companies"] = companiesId;
            //// InsertLog(email, clientAddress, resultOk ? 1 : 2, result.Id, result.CompanyId, instance.Config.ConnectionString);
            res.SetSuccess(result);
            return res;
        }
    }
}