// --------------------------------
// <copyright file="ActionLog.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Activity
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;
    using OpenFrameworkV3.Core.Security;
    using OpenFrameworkV3.Tools;

    /// <summary>Log actions on files</summary>
    public static class ActionLog
    {
        /// <summary>Gets a value indicating whether if loggin actions are enabled</summary>
        private static bool LogEnabled
        {
            get
            {
                if (ConfigurationManager.AppSettings["EnableLog"] == null)
                {
                    return true;
                }

                return ConfigurationManager.AppSettings["EnableLog"].ToUpperInvariant() == "1";
            }
        }

        /// <summary>Inserts a trace for an item action</summary>
        /// <param name="action">Action performed</param>
        /// <param name="itemId">Item identifier</param>
        /// <param name="data">Data of action</param>
        /// <param name="instanceName">Name of instance</param>
        /// <param name="applicationUser">User that realizes action</param>
        public static void Trace(string action, object itemId, string data, string instanceName, ApplicationUser applicationUser)
        {
            if (!LogEnabled)
            {
                return;
            }

            data = data.Replace(",\"", ",\n    \"");
            data = data.Replace("{", "{\n    ");
            data = data.Replace("}", "\n}");

            Trace(string.Format(CultureInfo.InvariantCulture, "{0}{1}", action, itemId), data, instanceName, applicationUser);
        }

        /// <summary>Inserts a trace for an item action</summary>
        /// <param name="action">Action performed</param>
        /// <param name="itemId">Item identificer</param>
        /// <param name="data">Data of action</param>
        /// <param name="instanceName">Name of instance</param>
        /// <param name="applicationUser">User that realizes action</param>
        public static void Trace(string action, long itemId, string data, string instanceName, ApplicationUser applicationUser)
        {
            if (!LogEnabled)
            {
                return;
            }

            data = data.Replace(",\"", ",\n    \"");
            data = data.Replace("{", "{\n    ");
            data = data.Replace("}", "\n}");

            Trace(string.Format(CultureInfo.InvariantCulture, "{0}{1}", action, itemId), data, instanceName, applicationUser);
        }

        /// <summary>Inserts a trace for an item action</summary>
        /// <param name="action">Action performed</param>
        /// <param name="itemId">Item identificer</param>
        /// <param name="data">Data of action</param>
        /// <param name="instanceName">Instance name</param>
        /// <param name="applicationUser">User that realizes action</param>
        public static void Trace(string action, string itemId, string data, string instanceName, ApplicationUser applicationUser)
        {
            if (!LogEnabled)
            {
                return;
            }

            data = data.Replace(",\"", ",\n    \"");
            data = data.Replace("{", "{\n    ");
            data = data.Replace("}", "\n}");

            Trace(string.Format(CultureInfo.InvariantCulture, "{0}{1}", action, itemId), data, instanceName, applicationUser);
        }

        /// <summary>Inserts a trace for an action</summary>
        /// <param name="action">Action performed</param>
        /// <param name="data">Data of action</param>
        /// <param name="instanceName">Instance name</param>
        /// <param name="applicationUser">User that realizes action</param>
        public static void Trace(string action, string data, string instanceName, ApplicationUser applicationUser)
        {
            if (!LogEnabled)
            {
                return;
            }

            string fileName = string.Format(
                CultureInfo.InvariantCulture,
                "{0}.log",
                action);

            string path = HttpContext.Current.Request.PhysicalApplicationPath;

            if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, "{0}\\Log\\{1}\\", path, instanceName);
            }
            else
            {
                path = string.Format(CultureInfo.InvariantCulture, "{0}Log\\{1}\\", path, instanceName);
            }

            Basics.VerifyFolder(path);
            string completeFileName = string.Format(
                CultureInfo.InvariantCulture,
                "{0}{1}",
                path,
                fileName);

            using (var output = new StreamWriter(completeFileName, true, Encoding.UTF8))
            {
                output.WriteLine(ApplicationUserTrace(applicationUser));
                data = data.Replace(",\"", ",\n    \"");
                data = data.Replace("{", "{\n    ");
                data = data.Replace("}", "\n}");
                output.WriteLine(data);
            }
        }
        
        public static void Trace(string data, string instanceName, string itemName, long itemId, long companyId)
        {
            if (!LogEnabled)
            {
                return;
            }

            string fileName = string.Format(
                CultureInfo.InvariantCulture,
                "{0}_{1}.log",
                itemName,
                itemId);

            string path = HttpContext.Current.Request.PhysicalApplicationPath;

            if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, "{0}\\Log\\{1}\\", path, instanceName);
            }
            else
            {
                path = string.Format(CultureInfo.InvariantCulture, "{0}Log\\{1}\\", path, instanceName);
            }

            Basics.VerifyFolder(path);
            string completeFileName = string.Format(
                CultureInfo.InvariantCulture,
                "{0}{1}",
                path,
                fileName);

            using (var output = new StreamWriter(completeFileName, true, Encoding.UTF8))
            {
                var user = ApplicationUser.Actual;
                output.WriteLine("--------");
                output.Write(string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Date"":""{0:dd/MM/yyy-hh:mm:ss}"",""Company"":{1},""User"":{{""Id"":{2},""Name"":""{3}""}},""Data"":""{4}""}}{5}",
                    DateTime.Now.ToUniversalTime(),
                    companyId,
                    user.Id,
                    Tools.Json.JsonCompliant(user.Profile.FullName),
                    data,
                    Environment.NewLine));
            }
        }

        public static void TraceSecurity(string data, string instanceName, long companyId)
        {
            if (!LogEnabled)
            {
                return;
            }

            string path = HttpContext.Current.Request.PhysicalApplicationPath;

            if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, "{0}\\Log\\{1}\\", path, instanceName);
            }
            else
            {
                path = string.Format(CultureInfo.InvariantCulture, "{0}Log\\{1}\\", path, instanceName);
            }

            Basics.VerifyFolder(path);
            string completeFileName = string.Format(CultureInfo.InvariantCulture, "{0}Security_{1}.log", path, companyId);

            using (var output = new StreamWriter(completeFileName, true, Encoding.UTF8))
            {
                var user = ApplicationUser.Actual;
                output.WriteLine("--------");
                output.Write(string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Date"":""{0:dd/MM/yyy-hh:mm:ss}"",""Company"":{1},""User"":{{""Id"":{2},""Name"":""{3}""}},""Data"":""{4}""}}{5}",
                    DateTime.Now.ToUniversalTime(),
                    companyId,
                    user.Id,
                    Tools.Json.JsonCompliant(user.Profile.FullName),
                    data,
                    Environment.NewLine));
            }
        }

        /// <summary>Inserts a trace with actual date for an action</summary>
        /// <param name="action">Action performed</param>
        /// <param name="data">Data of action</param>
        /// <param name="instanceName">Instance name</param>
        /// <param name="applicationUser">User that realizes action</param>
        public static void TraceDated(string action, string data, string instanceName, ApplicationUser applicationUser)
        {
            if (!LogEnabled)
            {
                return;
            }

            string fileName = string.Format(
                CultureInfo.InvariantCulture,
                "{0}_{1}_{2:yyyyMMdd}.log",
                instanceName,
                action,
                DateTime.Now);

            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, "{0}\\", path);
            }

            string completeFileName = string.Format(
                CultureInfo.InvariantCulture,
                "{0}Log\\{1}",
                path,
                fileName);

            using (var output = new StreamWriter(completeFileName, true, Encoding.UTF8))
            {
                output.WriteLine(ApplicationUserTrace(applicationUser));
                output.Write("\t");
                data = data.Replace(",\"", ",\n    \"");
                data = data.Replace("{", "{\n    ");
                data = data.Replace("}", "\n}");
                output.WriteLine(data);
            }
        }

        /// <summary>Inserts a trace with actual date for an action</summary>
        /// <param name="action">Action performed</param>
        /// <param name="data">Data of action</param>
        /// <param name="instanceName">Instance name</param>
        /// <param name="applicationUser">User that realizes action</param>
        public static void TraceDatedForced(string action, string data, string instanceName, ApplicationUser applicationUser)
        {
            string fileName = string.Format(
                CultureInfo.InvariantCulture,
                "{0}_{1}_{2:yyyyMMdd}.log",
                instanceName,
                action,
                DateTime.Now);

            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, "{0}\\", path);
            }

            string completeFileName = string.Format(
                CultureInfo.InvariantCulture,
                "{0}Log\\{1}",
                path,
                fileName);

            using (var output = new StreamWriter(completeFileName, true, Encoding.UTF8))
            {
                output.WriteLine(ApplicationUserTrace(applicationUser));
                output.Write("\t");
                data = data.Replace(",\"", ",\n    \"");
                data = data.Replace("{", "{\n    ");
                data = data.Replace("}", "\n}");
                output.WriteLine(data);
            }
        }

        /// <summary>Gets application user data for trace</summary>
        /// <param name="applicationUser">User that realizes action</param>
        /// <returns>Formatted user description</returns>
        private static string ApplicationUserTrace(ApplicationUser applicationUser)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0:dd/MM/yyyy - hh:mm:ss} - Id:{1} - CompanyId:{2} - {3}",
                DateTime.Now,
                applicationUser.Id,
                applicationUser.CompanyId,
                applicationUser.Profile.FullName);
        }
    }
}