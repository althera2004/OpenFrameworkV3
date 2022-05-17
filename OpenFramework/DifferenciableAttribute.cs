// --------------------------------
// <copyright file="DifferenciableAttribute.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3
{
    using System;

    /// <summary>Implementation of the Differenciable attribute for class properties</summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DifferenciableAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the DifferenciableAttribute class.</summary>
        /// <param name="apply">Indicates if apply</param>
        public DifferenciableAttribute(bool apply)
        {
            this.Apply = apply;
        }

        /// <summary>Initializes a new instance of the DifferenciableAttribute class.</summary>
        public DifferenciableAttribute()
        {
            this.Apply = true;
        }

        /// <summary>Gets a value indicating whether if apply</summary>
        public bool Apply { get; }

        public override object TypeId => base.TypeId;

        public override bool Equals(object obj)
        {
            return obj is DifferenciableAttribute attribute &&
                   base.Equals(obj) &&
                   Apply == attribute.Apply;
        }

        public override int GetHashCode()
        {
            var hashCode = -274100623;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Apply.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Match(object obj)
        {
            return base.Match(obj);
        }

        public override bool IsDefaultAttribute()
        {
            return base.IsDefaultAttribute();
        }
    }
}