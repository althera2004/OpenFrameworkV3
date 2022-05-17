// --------------------------------
// <copyright file="FormDatePicker.cs" company="OpenFramework">
//     Copyright (c) Althera2004. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace AltheraFramework.UserInterface
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;

    /// <summary>Implements FormDatePicker control</summary>
    public class FormDatePicker
    {
        public string Name { get; set; }
        public int ColumnSpan { get; set; }
        public int ColumnSpanLabel { get; set; }
        public string Label { get; set; }
        public DateTime? Value { get; set; }
        public bool? GrantToWrite { get; set; }
        public bool Required { get; set; }

        public string Render
        {
            get
            {
                var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
                
                string malformedLabel = string.Format(CultureInfo.InvariantCulture, @"<span class=""ErrorMessage"" id=""{0}DateMalformed"" style=""display:none;"">{1}</span>", this.Name, dictionary["Common_Error_DateMalformed"]);
                string requiredLabel = string.Format(CultureInfo.InvariantCulture, @"<span class=""ErrorMessage"" id=""{0}DateRequired"" style=""display:none;"">{1}</span>", this.Name, dictionary["Common_Required"]);

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{0}
                      <div class=""col-sm-{1}"">
                        <div class=""row"">
                            <div class=""col-xs-12 col-sm-12 tooltip-info"" id=""{2}Div"">
                                <div class=""input-group"">
                                    <input{5} class=""form-control date-picker"" id=""{2}"" type=""text"" data-date-format=""dd/mm/yyyy"" maxlength=""10"" value=""{3}"">
                                    <span{6} id=""{2}Btn"" class=""input-group-addon"" onclick=""document.getElementById('{2}').focus();"">
                                        <i class=""menu-icon fa fa-calendar""></i>
                                    </span>
                                </div>
                                {4}
                                {7}
                            </div>
                        </div>
                    </div>",
                    new FormLabel
                    {
                        ColumnSpanLabel = this.ColumnSpanLabel,
                        Id = this.Name,
                        Required = this.Required,
                        Label = this.Label
                    }.Render,
                    ColumnSpan,
                    this.Name,
                    this.Value.HasValue ? string.Format("{0:dd/MM/yyyy}", this.Value.Value) : string.Empty,
                    malformedLabel,
                    (this.GrantToWrite.HasValue && this.GrantToWrite == false) ? " readonly=\"readonly\"" : string.Empty,
                    (this.GrantToWrite.HasValue && this.GrantToWrite == false) ? " style=\"display:none;\"" : string.Empty,
                    this.Required ? requiredLabel : string.Empty);
            }
        }
    }
}