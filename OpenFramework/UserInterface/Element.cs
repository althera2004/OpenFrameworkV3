// -----------------------------------------------------------------------
// <copyright file="Element.cs" company="OpenFramework">
//     Copyright (c) Althera2004. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace AltheraFramework.UserInterface
{
    /// <summary></summary>
    public abstract class Element
    {
        public int Expand { get; set; }
        public string Id { get; set; }

        public virtual string Html
        {
            get
            {
                return string.Empty;
            }
        }
    }
}