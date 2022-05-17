// --------------------------------
// <copyright file="Instance.Path.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core
{
    using System.Globalization;
    using System.Web;

    public partial class Instance
    {
        public static class Path
        {
            /// <summary>Gets path of instance</summary>
            public static string Base(string instanceName)
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{0}Instances\{1}\",
                    HttpContext.Current.Request.PhysicalApplicationPath,
                    instanceName.ToUpperInvariant());
            }

            /// <summary>Gets path of items definition</summary>
            public static string Definition(string instanceName)
            {
                return string.Format(CultureInfo.InvariantCulture, @"{0}ItemDefinitions", Base(instanceName));
            }

            /// <summary>Gets path of alerts definition</summary>
            public static string Alerts(string instanceName)
            {
                return string.Format(CultureInfo.InvariantCulture, @"{0}Alerts", Base(instanceName));
            }

            /// <summary>Gets path of items definition</summary>
            public static string Calendar(string instanceName)
            {
                return string.Format(CultureInfo.InvariantCulture, @"{0}Calendar", Base(instanceName));
            }

            /// <summary>Gets path of items definition</summary>
            public static string PdfTemplates(string instanceName)
            {
                return string.Format(CultureInfo.InvariantCulture, @"{0}Calendar", Base(instanceName));
            }

            /// <summary>Gets path of items definition</summary>
            public static string Dictionary(string instanceName)
            {
                return string.Format(CultureInfo.InvariantCulture, @"{0}Dicc", Base(instanceName));
            }

            /// <summary>Gets path of items definition</summary>
            public static string Data(string instanceName)
            {
                return string.Format(CultureInfo.InvariantCulture, @"{0}Data", Base(instanceName));
            }

            /// <summary>Gets path of items definition</summary>
            public static string Mail(string instanceName)
            {
                return string.Format(CultureInfo.InvariantCulture, @"{0}Mail", Base(instanceName));
            }

            /// <summary>Gets path of data in Json files</summary>
            public static string DataJson(string instanceName)
            {
                return string.Format(CultureInfo.InvariantCulture, @"{0}Data\NoSql", Base(instanceName));
            }

            /// <summary>Gets path of items definition</summary>
            public static string Scripts(string instanceName)
            { 
                return string.Format(CultureInfo.InvariantCulture, @"{0}Scripts", Base(instanceName));
            }
        }
    }
}