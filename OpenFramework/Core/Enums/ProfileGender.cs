// --------------------------------
// <copyright file="ProfileGender.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Enums
{
    using System;

    /// <summary>Enumeration of profile gender options</summary>
    [FlagsAttribute]
    public enum ProfileGender
    {
        /// <summary>0 - Undefined or group or enterprise</summary>
        None = 0,

        /// <summary>1- Female</summary>
        Female = 1,

        /// <summary>2- Male</summary>
        Male = 2
    }
}