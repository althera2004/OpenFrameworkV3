// --------------------------------
// <copyright file="FilterCriteria.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.DataAccess
{
    using System;

    /// <summary>Filter criteria</summary>
    [Serializable]
    public class FilterCriteria
    {
        /// <summary>Gets or sets criteria definition</summary>
        public string Criteria { get; set; }

        /// <summary>Gets or sets field name</summary>
        public string FieldName { get; set; }

        /// <summary>Gets or sets value</summary>
        public string Value { get; set; }
    }
}