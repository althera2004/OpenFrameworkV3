// --------------------------------
// <copyright file="MenuShortcut.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Navigation
{
    /// <summary>Implementation of menu shortcut</summary>
    public class MenuShortcut
    {
        /// <summary>Gets an empty set of menu shorcuts</summary>
        public static MenuShortcut Empty
        {
            get
            {
                return new MenuShortcut
                {
                    Red = Shortcut.Empty,
                    Blue = Shortcut.Empty,
                    Green = Shortcut.Empty,
                    Yellow = Shortcut.Empty
                };
            }
        }

        /// <summary>Gets or sets the green shortcut</summary>
        public Shortcut Green { get; set; }

        /// <summary>Gets or sets the yellow shortcut</summary>
        public Shortcut Yellow { get; set; }

        /// <summary>Gets or sets the blue shortcut</summary>
        public Shortcut Blue { get; set; }

        /// <summary>Gets or sets the red shortcut</summary>
        public Shortcut Red { get; set; }

        /// <summary>Sets shortcut configruation by type</summary>
        /// <param name="type">Shorcut type</param>
        /// <param name="itemId">Item type identifier</param>
        public void SetShortcut(ShortcutTypes type, long itemId, string instanceName)
        {
            switch (type)
            {
                case ShortcutTypes.Red:
                    this.Red = Shortcut.Empty;
                    break;
                case ShortcutTypes.Blue:
                    this.Blue = Shortcut.Empty;
                    break;
                case ShortcutTypes.Green:
                    this.Green = Shortcut.Empty;
                    break;
                case ShortcutTypes.Yellow:
                    this.Yellow = Shortcut.Empty;
                    break;
            }

            if (itemId > Constant.DefaultId)
            {
                switch (type)
                {
                    case ShortcutTypes.Red:
                        this.Red = Shortcut.ByItemId(itemId, instanceName);
                        break;
                    case ShortcutTypes.Blue:
                        this.Blue = Shortcut.ByItemId(itemId, instanceName);
                        break;
                    case ShortcutTypes.Green:
                        this.Green = Shortcut.ByItemId(itemId, instanceName);
                        break;
                    case ShortcutTypes.Yellow:
                        this.Yellow = Shortcut.ByItemId(itemId, instanceName);
                        break;
                }
            }
        }
    }
}