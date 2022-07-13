// --------------------------------
// <copyright file="Default.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using OpenFrameworkV3;
using OpenFrameworkV3.Core;
using OpenFrameworkV3.Core.Activity;

public partial class Default : Page
{
    /// <summary>Instance name</summary>
    private string instanceName;

    private Instance instance;

    public string MultiCompany
    {
        get
        {
            return this.instance.Config.MultiCompany ? Constant.JavaScriptTrue : Constant.JavaScriptFalse;
        }
    }

    /// <summary>Gets instance name</summary>
    public string InstanceName
    {
        get
        {
            if (string.IsNullOrEmpty(this.instanceName))
            {
                instanceName = ConfigurationManager.AppSettings["InstanceName"].ToString();
            }

            Session["InstanceName"] = instanceName;
            return instanceName;
        }
    }

    /// <summary>Gets MFA type</summary>
    public string MFA
    {
        get
        {
            if (string.IsNullOrEmpty(this.instanceName))
            {
                instanceName = ConfigurationManager.AppSettings["InstanceName"].ToString();
            }

            Session["InstanceName"] = instanceName;
            return this.instance.Config.Security.MFA;
        }
    }

    public string Email { get; private set; }

    public string LandPage { get; private set; }

    public string Debug { get; private set; }

    public string FrameworkVersion
    {
        get
        {
            //var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Assembly web = Assembly.LoadFrom(this.Request.PhysicalApplicationPath + @"\Bin\OpenFramework.dll");
            AssemblyName webName = web.GetName();

            string version = webName.Version.ToString();

            return String.Format("- version {0}", version);

        }
    }

    /// <summary>Gets instance logo</summary>
    public string Logo
    {
        get
        {
            return Instance.Logo(this.instance.Name).Replace(".png", ".png?" + Guid.NewGuid());
        }
    }

    public string BK
    {
        get
        {
            var res = new StringBuilder();
            string path = Instance.Path.Base(this.instance.Name);
            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}WelcomeBackgrounds\", path);
            var files = Directory.GetFiles(path, this.InstanceName + "*.*");
            try
            {
                if (files.Count() == 0)
                {
                    files = Directory.GetFiles(path, "OpenFramework*.*");
                }
                else
                {
                    bool first = true;
                    foreach (var file in files)
                    {
                        res.AppendFormat(
                            CultureInfo.InvariantCulture,
                            @"<div class=""slide{0}"" style=""background-image:url('/Instances/{2}/WelcomeBackgrounds/{1}');""></div>",
                            first ? " show" : string.Empty,
                            Path.GetFileName(file),
                            this.instanceName);
                        first = false;
                    }
                }

                Session["BK"] = res;
            }
            catch (Exception ex)
            {
                ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "Default::BK --> {0}", this.instanceName));
                res = new StringBuilder(path);
            }

            return res.ToString();
        }
    }

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    public string LanguageBrowser { get; private set; }

    public string IP { get; private set; }

    protected void Page_Init()
    {
        Instance.CheckPersistence();
    }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Request.UserLanguages != null)
        {
            this.LanguageBrowser = this.Request.UserLanguages[0];
        }

        if (this.Request.QueryString["c"] != null)
        {
            this.instanceName = this.Request.QueryString["c"];
        }
        else
        {
            var serverName = this.Request.Url.Host;
            if (serverName.IndexOf(".openframework") != -1)
            {
                this.instanceName = this.Request.Url.Host.Split('.')[0].ToUpperInvariant();
            }
            else
            {
                this.instanceName = ConfigurationManager.AppSettings["InstanceName"].ToUpperInvariant();
            }
        }

        this.instance = Persistence.InstanceByName(Instance.InstanceName);
        this.Session["InstanceName"] = this.instanceName;
        this.Session["Instance"] = this.instance;
        this.IP = this.GetUserIP();
        if (this.instance.Config.Security.IPAccess)
        {
            //var checkIP = IPAccess.CheckIP(this.instanceName, this.IP, 0);
            //if (checkIP.Success)
            //{
            //    if ((int)checkIP.ReturnValue == 0)
            //    {
            //        this.Response.Redirect("NOIPAccess.aspx", Constant.EndResponse);
            //        Context.ApplicationInstance.CompleteRequest();
            //    }
            //}
        }

        CleanTemporalFiles();
        this.ObtainLandPage();
    }

    private string GetUserIP()
    {
        string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (!string.IsNullOrEmpty(ipList))
        {
            return ipList.Split(',')[0];
        }

        return Request.ServerVariables["REMOTE_ADDR"];
    }

    private void ObtainLandPage()
    {
        //this.Debug = string.Empty;
        //if (this.Request.QueryString.Count > 0)
        //{
        //    var q = this.Request.QueryString.Keys[0] as string;
        //    if (q == null || q.StartsWith("c", StringComparison.OrdinalIgnoreCase))
        //    {
        //        return;
        //    }

        //    this.accessPoint = new AccessServicePoint(q);
        //    this.LandPage = accessPoint.LandPage;
        //    this.Email = this.accessPoint.Email;
        //    this.Debug = this.accessPoint.Json;
        //}
    }

    private void CleanTemporalFiles()
    {
        var path = string.Format(CultureInfo.InvariantCulture, @"{0}\TemporalExportFolder", this.Request.PhysicalApplicationPath);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var info = new DirectoryInfo(path);
        var files = info.GetFiles().Where(p => p.CreationTime < DateTime.Now.AddHours(-1)).ToArray();
        foreach (FileInfo file in files)
        {
            if (File.Exists(file.FullName))
            {
                File.Delete(file.FullName);
            }
        }

        path = string.Format(CultureInfo.InvariantCulture, @"{0}\TemporalExportFolder\{1}", this.Request.PhysicalApplicationPath, this.instanceName);
        if (Directory.Exists(path))
        {
            info = new DirectoryInfo(path);
            files = info.GetFiles().Where(p => p.CreationTime < DateTime.Now.AddHours(-1)).ToArray();
            foreach (FileInfo file in files)
            {
                if (File.Exists(file.FullName))
                {
                    File.Delete(file.FullName);
                }
            }
        }
    }
}