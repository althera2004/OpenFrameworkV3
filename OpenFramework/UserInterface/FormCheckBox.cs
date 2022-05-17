using System.Globalization;

namespace AltheraFramework.UserInterface
{
    public class FormCheckBox
    {
        public int ColumnSpanLabel { get; set; }
        public string Label { get; set; }
        public int ColumnSpan { get; set; }
        public string Name { get; set; }
        public bool Required { get; set; }
        public bool ReadOnly { get; set; }
        public bool? GrantToWrite { get; set; }
        public string RequiredMessage { get; set; }
        public string ToolTip { get; set; }
        public bool RightAlign { get; set; }

        public virtual string Render
        {
            get
            {
                string requiredLabel = string.Empty;
                if (this.Required)
                {
                    requiredLabel = string.Format(CultureInfo.InvariantCulture, @"<span class=""ErrorMessage"" id=""{0}ErrorRequired"" style=""display:none;"">{1}</span>", this.Name, this.RequiredMessage);
                }

                string pattern = @"<div class=""checkbox col-sm-{2}"">
													<label>
														<input name=""form-field-checkbox"" type=""checkbox"" id=""{0}"" class=""ace"">
														<span class=""lbl""> {7}</span>
													</label>
												</div>";

                /*string patternOld = @"{7}
                        <div class=""col-sm-{2}"" style=""align-text:left;"">                                                                                                                            
                            <input type=""checkbox""{10} id=""{0}"" class=""col-xs-12 col-sm-12 tooltip-info{8}"" {4}"" />
                            {5}                                                           
                        </div>	";*/

                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Name,
                    string.Empty,
                    this.ColumnSpan,
                    string.Empty,
                    string.Empty,
                    requiredLabel,
                    string.Empty,
                    this.Label,
                    /*new FormLabel
                    {
                        ColumnSpanLabel = this.ColumnSpanLabel,
                        Id = this.Name,
                        Required = this.Required,
                        Label = this.Label
                    }.Render,*/
                    string.Empty,
                    string.Empty,
                    (this.GrantToWrite.HasValue && this.GrantToWrite == false) ? " readonly=\"readonly\"" : string.Empty);
            }
        }
    }
}