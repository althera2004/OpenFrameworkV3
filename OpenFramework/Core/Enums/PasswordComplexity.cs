// -----------------------------------------------------------------------
// <copyright file="PasswordComplexity.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace OpenFrameworkV3.Core.Enums
{
    using System;

    /// <summary>Enumeration of password complexity level</summary>
    [FlagsAttribute]
    public enum PasswordComplexity
    {
        /// <summary>None: No complexity</summary>
        None = 0,

        /// <summary>Simple: Letters and numbers</summary>
        Simple = 1,

        /// <summary>Strong: Letters, number and special characters</summary>
        Strong = 2
    }
}