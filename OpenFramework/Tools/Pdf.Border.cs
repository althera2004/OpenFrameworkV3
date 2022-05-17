// --------------------------------
// <copyright file="Border.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Tools
{
    public static partial class Pdf
    {

        /// <summary>Common border configurations</summary>
        public static class Border
        {
            /// <summary>Border in all sides</summary>
            public const int All = 15;

            /// <summary>No border</summary>
            public const int None = 0;

            /// <summary>Only border at bottom</summary>
            public const int Bottom = 2;
        }
    }
}