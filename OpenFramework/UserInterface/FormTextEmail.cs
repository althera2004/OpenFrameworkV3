// --------------------------------
// <copyright file="FormTextEmail.cs" company="OpenFramework">
//     Copyright (c) Althera2004. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace AltheraFramework.UserInterface
{
    using System.Globalization;

    public class FormTextEmail : FormText
    {
        public bool Linkable { get; set; }

        public override string Render
        {
            get
            {
                string label = string.Empty;
                if (!string.IsNullOrEmpty(this.Label))
                {
                    string requiredMark = this.Required ? "<span style=\"color:#f00\">*</span>" : string.Empty;
                    label = string.Format(
                        CultureInfo.InvariantCulture,
                        @"<label id=""{2}Label"" class=""col-sm-{0}{4}"">{1}{3}</label>",
                        this.ColumnSpanLabel,
                        this.Label,
                        this.Id,
                        requiredMark,
                        this.RightAlign ? " control-label no-padding-right" : string.Empty);
                }


                string requiredLabel = string.Empty;
                if (this.Required)
                {
                    requiredLabel = string.Format(CultureInfo.InvariantCulture, @"<span class=""ErrorMessage"" id=""{0}ErrorRequired"" style=""display:none;"">{1}</span>", this.Id, this.RequiredMessage);
                }

                string duplicatedLabel = string.Empty;
                if (this.Duplicated)
                {
                    duplicatedLabel = string.Format(CultureInfo.InvariantCulture, @"<span class=""ErrorMessage"" id=""{0}ErrorDuplicated"" style=""display:none;"">{1}</span>", this.Id, this.DuplicatedMessage);
                }

                string btnLinkable = this.Linkable ? string.Format(CultureInfo.InvariantCulture, @" id=""AuxButton-{0}"" style=""cursor:pointer;""", this.Id) : " style=\"color:#aaa\"";
                string iconLinkable = this.Linkable ? string.Format(CultureInfo.InvariantCulture, @" id=""AuxButtonIcon-{0}""", this.Id) : string.Empty;
                
                return string.Format(CultureInfo.InvariantCulture,
                    @"{7}
                        <div class=""col-sm-{2}"">
                            <div class=""input-group "">
                                <span class=""input-group-addon email-addon""{8}>
                                    <i class=""ace-icon fa fa-envelope""{9}></i>
                                </span>
                                <input type=""text"" id=""{0}"" placeholder=""{3}"" class=""col-xs-12 col-sm-12 tooltip-info emailText"" value=""{1}"" {4} />
                            </div>                                                                                                                         
                            
                            {5}
                            {6}                                                           
                        </div>	",
                             this.Id,
                             this.Value,
                             this.ColumnSpan,
                             this.Placeholder,
                             this.MaximumLength > 0 ? string.Format(CultureInfo.GetCultureInfo("en-us"), @" maxlength=""{0}""", this.MaximumLength) : string.Empty,
                             requiredLabel,
                             duplicatedLabel,
                             label,
                             btnLinkable,
                             iconLinkable);
            }
        }
    }
}
