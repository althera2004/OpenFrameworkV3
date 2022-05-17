// --------------------------------
// <copyright file="Constant.DataOrigin.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3
{
    /// <summary>Constant application values</summary>
    public static partial class Constant
    {
        /// <summary>Constant values for data origin</summary>
        public static class DataOrigin
        {
            /// <summary>SQL database origin</summary>
            public const string OriginNative = "SQL";

            /// <summary>Microsoft Navision origin</summary>
            public const string OriginNavision = "NAV";
            
            /// <summary>Microsoft CRM origin</summary>
            public const string OriginCRM = "CRM";
            
            /// <summary>SAP origin</summary>
            public const string OriginSAP = "SAP";
        }
    }
}