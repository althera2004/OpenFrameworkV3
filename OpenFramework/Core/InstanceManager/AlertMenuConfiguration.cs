// --------------------------------
// <copyright file="AlertMenuConfiguration.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.InstanceManager
{
    /// <summary>Implements configuration of menu alerts</summary>
    public class AlertMenuConfiguration
    {
        /// <summary>Gets an empty instance of menu configuration</summary>
        public static AlertMenuConfiguration Empty
        {
            get
            {
                return new AlertMenuConfiguration
                {
                    Label = string.Empty,
                    Color = string.Empty,
                    Icon = string.Empty,
                    Menu = string.Empty
                };
            }
        }

        /// <summary>Gets or sets label for menu button</summary>
        public string Label { get; set; }

        /// <summary>Gets or sets background color of menu button</summary>
        public string Color { get; set; }

        /// <summary>Gets or sets icon to identifier menu button</summary>
        public string Icon { get; set; }

        /// <summary>Gets or sets menu name for indentification purposes</summary>
        public string Menu { get; set; }

        /// <summary>Gets menu configuration from dynamic data</summary>
        /// <param name="data">dyinamic data in JSON format</param>
        /// <returns>Menu configuration</returns>
        public static AlertMenuConfiguration FromDynamic(DynamicJsonObject data)
        {
            var res = Empty;
            var values = data.Keys;
            foreach (var pair in values)
            {
                switch (pair.ToUpperInvariant())
                {
                    case "LABEL":
                        res.Label = data.GetValue<string>(pair);
                        break;
                    case "COLOR":
                        res.Color = data.GetValue<string>(pair);
                        break;
                    case "ICON":
                        res.Icon = data.GetValue<string>(pair);
                        break;
                    case "MENU":
                        res.Menu = data.GetValue<string>(pair);
                        break;
                }
            }

            return res;
        }
    }
}