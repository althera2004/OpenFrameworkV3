using System.Globalization;

namespace AltheraFramework.UserInterface
{
    public class FormLabel
    {
        public bool Required { get; set; }
        public string Id { get; set; }
        public int ColumnSpanLabel { get; set; }
        public string Label { get; set; }

        public virtual string Render
        {
            get
            {
                string label = string.Empty;
                if (!string.IsNullOrEmpty(this.Label))
                {
                    string requiredMark = this.Required ? "<span style=\"color:#f00\">*</span>" : string.Empty;
                    label = string.Format(
                        CultureInfo.InvariantCulture,
                        @"<label id=""{2}Label"" class=""col-sm-{0}"">{1}{3}</label>",
                        this.ColumnSpanLabel,
                        this.Label,
                        this.Id,
                        requiredMark);
                }

                return label;
            }
        }
    }
}
