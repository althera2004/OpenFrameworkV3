// --------------------------------
// <copyright file="CodedQueryItem.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core
{
    using System.Collections.Generic;

    /// <summary>Implements item of a coded query</summary>
    public struct CodedQueryItem
    {
        /// <summary>Gets or sets item key</summary>
        public string Key { get; set; }

        /// <summary>Gets or sets item value</summary>
        public object Value { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is CodedQueryItem))
            {
                return false;
            }

            var item = (CodedQueryItem)obj;
            return Key == item.Key &&
                   EqualityComparer<object>.Default.Equals(Value, item.Value);
        }

        public override int GetHashCode()
        {
            var hashCode = 206514262;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Key);
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Value);
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}