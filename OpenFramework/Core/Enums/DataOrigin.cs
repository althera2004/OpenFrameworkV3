// --------------------------------
// <copyright file="DataOrigin.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Enums
{
    /// <summary>Enumeration of data origin</summary>
    public static class DataOrigin
    {
        /// <summary> 0 - From SQL Server</summary>
        public const int Sql = 0;

        /// <summary> 0 - From text file in JSON format</summary>
        public const int JsonFile = 1;
    }
}
