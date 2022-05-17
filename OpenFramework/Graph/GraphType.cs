// --------------------------------
// <copyright file="GraphType.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Graph
{
    using System;

    [FlagsAttribute]
    public enum GraphType
    {
        Icon,
        Pie,
        Bar
    }
}
