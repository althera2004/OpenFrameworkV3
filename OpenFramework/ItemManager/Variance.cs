// --------------------------------
// <copyright file="Variance.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    /// <summary>Implements Variance class</summary>
    public struct Variance
    {
        /// <summary>Gets or sets the name of property</summary>
        public string PropertyName { get; set; }

        /// <summary>Gets or sets the new value of property</summary>
        public object NewValue { get; set; }

        /// <summary>Gets or sets the old value of property</summary>
        public object OldValue { get; set; }

        /// <summary>Gets equals operator</summary>
        /// <param name="left">First value to compare</param>
        /// <param name="right">Second value to compare</param>
        /// <returns>Equals result</returns>
        public static bool operator ==(Variance left, Variance right)
        {
            return left.Equals(right);
        }

        /// <summary>Gets not equals operator</summary>
        /// <param name="left">First value to compare</param>
        /// <param name="right">Second value to compare</param>
        /// <returns>Not equals result</returns>
        public static bool operator !=(Variance left, Variance right)
        {
            return !(left == right);
        }

        /// <summary>Gets equals operator</summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>Equals result</returns>
        public override bool Equals(object obj)
        {
            return this == (Variance)obj;
        }

        /// <summary>Gets hash code</summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var result = 0;
                result = (result * 397) ^ this.PropertyName.Length;
                result = (result * 397) ^ this.NewValue.GetHashCode();
                result = (result * 397) ^ this.OldValue.GetHashCode();
                return result;
            }
        }
    }
}