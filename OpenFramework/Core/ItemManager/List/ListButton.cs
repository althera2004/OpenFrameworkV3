// --------------------------------
// <copyright file="SecurityConfiguration.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager.ItemList
{
    using System.Globalization;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>Implements buttons for list</summary>
    public sealed class ListButton
    {
        /// <summary>Button's function</summary>
        [JsonProperty("Function")]
        private readonly string function;

        /// <summary>Button icon</summary>
        [JsonProperty("Icon")]
        private readonly string icon;

        /// <summary>Values for button function</summary>
        [JsonProperty("FunctionValues")]
        private readonly object[] functionValues;

        /// <summary>Button label</summary>
        [JsonProperty("Label")]
        private readonly string label;

        /// <summary>Icon color</summary>
        [JsonProperty("Color")]
        private readonly string color;

        /// <summary>Gets the name of button</summary>
        [JsonProperty("Name")]
        public string Name { get; private set; }

        /// <summary>Gets the button icon</summary>
        [JsonIgnore]
        public string Icon
        {
            get
            {
                return this.icon ?? "gear";
            }
        }

        /// <summary>Gets the button label</summary>
        [JsonIgnore]
        public string Label
        {
            get
            {
                return this.label ?? this.Name;
            }
        }

        /// <summary>Gets the button label</summary>
        [JsonIgnore]
        public string Color
        {
            get
            {
                return this.color ?? "#428BCA";
            }
        }

        /// <summary>Gets the function attached to button</summary>
        [JsonIgnore]
        public string Function
        {
            get
            {
                if(string.IsNullOrEmpty(this.function))
                {
                    return string.Empty;
                }

                var res = new StringBuilder();
                if(this.functionValues != null)
                {
                    res.Append("(");
                    bool first = true;
                    foreach(object val in this.functionValues)
                    {
                        if(first)
                        {
                            first = false;
                        }
                        else
                        {
                            res.Append(",");
                        }

                        string value = val.GetType().ToString().ToUpperInvariant() == "SYSTEM.STRING" ? string.Format(CultureInfo.InvariantCulture, @"'{0}'", val) : val.ToString();
                        res.Append(value);
                    }

                    res.Append(")");
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}{1}",
                    this.function,
                    res);
            }
        }
    }
}