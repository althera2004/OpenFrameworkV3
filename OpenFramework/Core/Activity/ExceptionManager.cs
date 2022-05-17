// --------------------------------
// <copyright file="DynamicJsonObject.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Activity
{
    using System;

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

        }
    }
}