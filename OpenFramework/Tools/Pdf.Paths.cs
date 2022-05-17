// --------------------------------
// <copyright file="ToolsPdf.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Tools
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web;

    /// <summary>Implements tools for PDF documents creation</summary>
    public static partial class Pdf
    {
        public static class Paths
        {
            public static string Temporal(string instanceName, string fileName)
            {
                var targetFolder = string.Format(CultureInfo.InvariantCulture, "{0}TemporalExportFolder\\{1}", HttpContext.Current.Request.PhysicalApplicationPath, instanceName);
                Basics.VerifyFolder(targetFolder);
                var res = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", targetFolder, fileName);
                if (File.Exists(res))
                {
                    File.Delete(res);
                }

                return res;
            }

            public static string TargetField(string instanceName, string itemName, string fieldName, long itemId)
            {
                var targetFolder = string.Format(CultureInfo.InvariantCulture, "{0}Instances\\{1}\\Data\\{2}", HttpContext.Current.Request.PhysicalApplicationPath, instanceName, itemName);
                Basics.VerifyFolder(targetFolder);
                targetFolder = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", targetFolder, itemId);
                Basics.VerifyFolder(targetFolder);
                var fileNameNew = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}.pdf", targetFolder, fieldName);
                return fileNameNew;
            }
        }
    }
}