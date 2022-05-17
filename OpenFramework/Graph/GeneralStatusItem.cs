// --------------------------------
// <copyright file="GeneralStatusItem.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Graph
{
    using System.Globalization;

    public class GeneralStatusItem
    {
        public string Style { get; set; }
        public string Icon { get; set; }
        public decimal MainData { get; set; }
        public string Label { get; set; }
        public string StatStyle { get; set; }
        public decimal DeltaVariation { get; set; }

        public string Render
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"
                    <div class=""infobox infobox-{0}"">
                        <div class=""infobox-icon""> <i class=""ace-icon fa fa-{1}""></i></div>
                        <div class=""infobox-data"">
                            <span class=""infobox-data-number"">{2}</span>
                            <div class=""infobox-content"">{3}</div>
                        </div>
                        <div class=""stat stat-{4}"">{5}</div>
                    </div>",
                    this.Style,
                    this.Icon,
                    this.MainData,
                    this.Label,
                    this.StatStyle,
                    this.DeltaVariation);
            }
        }
    }
}