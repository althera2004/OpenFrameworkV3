// --------------------------------
// <copyright file="BreadcrumbItem.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Navigation
{
    using System.Collections.Generic;

    public struct BreadcrumbItem
    {
        /// <summary>Gets or sets a value indicating whether the link of breadcrumb item</summary>
        public string Link { get; set; }

        /// <summary>Gets or sets a value indicating whether the label of breadcrumb item</summary>
        public string Label { get; set; }

        /// <summary>Gets or sets a value indicating whether if item is leaf</summary>
        public bool Leaf { get; set; }

        /// <summary>Gets or sets a value indicating whether if prevents text translation</summary>
        public bool Invariant { get; set; }

        public bool Encrypted { get; set; }

        public string ItemId { get; set; }

        public string ListId { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is BreadcrumbItem))
            {
                return false;
            }

            var item = (BreadcrumbItem)obj;
            return Link == item.Link &&
                   Label == item.Label &&
                   Leaf == item.Leaf &&
                   Invariant == item.Invariant;
        }

        public override int GetHashCode()
        {
            var hashCode = -357588918;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Link);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Label);
            hashCode = hashCode * -1521134295 + Leaf.GetHashCode();
            hashCode = hashCode * -1521134295 + Invariant.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}