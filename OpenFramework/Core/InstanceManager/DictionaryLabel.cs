﻿// --------------------------------
// <copyright file="DictionaryLabel.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.InstanceManager
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>Implements language dictionaries</summary>
    public sealed class DictionaryLabel
    {
        /// <summary>Dictionary items</summary>
        private Dictionary<string, string> items;

        /// <summary>Dictionary childs items</summary>
        private Dictionary<string, DictionaryLabel> childs;

        /// <summary>Gets or sets the language of dictionary</summary>
        public Language Language { get; set; }

        /// <summary>Gets dictionary items</summary>
        public ReadOnlyDictionary<string, string> Items
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new Dictionary<string, string>();
                }

                return new ReadOnlyDictionary<string, string>(this.items);
            }
        }

        /// <summary>Gets dictionary childs</summary>
        public ReadOnlyDictionary<string, DictionaryLabel> Childs
        {
            get
            {
                if (this.childs == null)
                {
                    this.childs = new Dictionary<string, DictionaryLabel>();
                }

                return new ReadOnlyDictionary<string, DictionaryLabel>(this.childs);
            }
        }
    }
}