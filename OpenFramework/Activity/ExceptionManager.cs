// -----------------------------------------------------------------------
// <copyright file="ExceptionManager.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace OpenFramework.Activity
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using OpenFramework.Tools;

    /// <summary>Implements the ExceptionManager class</summary>
    public static class ExceptionManager
    {
        /// <summary>Write a trace line on a log daily file</summary>
        /// <param name="ex">Exception occurred</param>
        /// <param name="source">Source of exception</param>
        public static void Trace(Exception ex, string source)
        {
            Trace(ex as Exception, source, string.Empty);
        }

        /// <summary>Trace a exception into log file</summary>
        /// <param name="ex">Exception occurred</param>
        /// <param name="source">Source of exception</param>
        /// <param name="extraData">Data extra of exception</param>
        public static void Trace(Exception ex, string source, string extraData)
        {
            string message = string.Empty;
            if (ex == null)
            {
                message = string.Empty;
            }

            if (ex != null)
            {
                message = ex.Message;
            }

            if (string.IsNullOrEmpty(source))
            {
                source = string.Empty;
            }

            if (string.IsNullOrEmpty(extraData))
            {
                extraData = string.Empty;
            }

            try
            {
                string path = ConfigurationManager.AppSettings["LogPath"] as string;
                string instanceName = string.Empty;
                if(HttpContext.Current.Session["InstanceName"] != null)
                {
                    instanceName = HttpContext.Current.Session["InstanceName"].ToString();
                }

                if (!path.EndsWith("\\", StringComparison.Ordinal))
                {
                    path = string.Format(CultureInfo.InstalledUICulture, @"{0}\LogError\{1}", path, instanceName);
                }
                else
                {
                    path = string.Format(CultureInfo.InstalledUICulture, @"{0}LogError\{1}", path, instanceName);
                }

                Basics.VerifyFolder(path);
                path = string.Format(CultureInfo.InstalledUICulture, @"{0}\Errors_{1}.txt", path, DateTime.Now.ToString("yyyyMMdd", CultureInfo.GetCultureInfo("es-es")));
                string line = string.Format(CultureInfo.InstalledUICulture, "{0}::{1}::{2}::{3}", DateTime.Now.ToString("hh:mm:ss", CultureInfo.GetCultureInfo("es-es")), ex.Message, source, extraData);
                using (var output = new StreamWriter(path, true))
                {
                    output.WriteLine(line);
                    output.Flush();
                }
            }
            finally
            {
            }
        }
    }
}