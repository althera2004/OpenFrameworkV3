﻿// --------------------------------
// <copyright file="Tab.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.UserInterface
{
    using System.Globalization;

    /// <summary>Implements Tab class</summary>
    public class Tab
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public bool Selected { get; set; }

        public bool Active { get; set; }

        public bool Available { get; set; }

        public bool Hidden { get; set; }

        public string Render
        {
            get
            {
                if (this.Available)
                {
                    string pattern = @"<li class=""{1}"" id=""Tab{0}""{4}>
                           <a {2}href=""#{0}"">{3}</a>
                          </li>";
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        pattern,
                        this.Id,
                        this.Selected ? "active" : string.Empty,
                        "data-toggle=\"tab\" ",
                        this.Label,
                        this.Hidden ? " style=\"display:none;\"" : string.Empty);
                }

                return string.Empty;
            }
        }
    }
}