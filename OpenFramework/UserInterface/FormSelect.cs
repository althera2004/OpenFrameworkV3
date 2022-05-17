// --------------------------------
// <copyright file="FormSelect.cs" company="OpenFramework">
//     Copyright (c) Althera2004. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace AltheraFramework.UserInterface
{
    using AltheraFramework.ItemManager.Form;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FormSelect
    {
        public int ColumnSpanLabel { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int ColumnSpan { get; set; }
        public string ToolTip { get; set; }
        public string Placeholder { get; set; }
        public string ChangeEvent { get; set; }
        public bool? GrantToWrite { get; set; }
        public bool Required { get; set; }
        public string RequiredMessage { get; set; }
        private List<FormSelectOption> options { get; set; }
        public FormSelectOption DefaultOption { get; set; }
        public ExtendSelect ExtendSelect { get; set; }

        public ReadOnlyCollection<FormSelectOption> Options
        {
            get
            {
                if(this.options==null)
                {
                    this.options = new List<FormSelectOption>();
                }

                return new ReadOnlyCollection<FormSelectOption>(this.options);
            }
        }

        public FormSelect()
        {
            this.options = new List<FormSelectOption>();
        }

        public void AddOption(FormSelectOption option)
        {
            if (this.options == null)
            {
                this.options = new List<FormSelectOption>();
            }

            this.options.Add(option);
        }

        public string Render
        {
            get
            {
                /*string label = string.Empty;
                if (!string.IsNullOrEmpty(this.Label))
                {
                    string requiredMark = this.Required ? "<span style=\"color:#f00\">*</span>" : string.Empty;
                    label = string.Format(@"<label id=""{2}Label"" class=""col-sm-{0} control-label no-padding-right"">{1}{3}</label>", this.ColumnSpanLabel, this.Label, this.Name, requiredMark);
                }*/

                string requiredLabel = string.Empty;
                if (this.Required)
                {
                    requiredLabel = string.Format(@"<span class=""ErrorMessage"" id=""{0}ErrorRequired"" style=""display:none;"">{1}</span>", this.Name, this.RequiredMessage);
                }

                var optionsList = new StringBuilder();
                if (this.DefaultOption != null)
                {
                    if (!string.IsNullOrEmpty(this.DefaultOption.Text))
                    {
                        optionsList.Append(this.DefaultOption.Render);
                    }
                }

                foreach (FormSelectOption option in this.options)
                {
                    optionsList.Append(option.Render);
                }

                string label = new FormLabel
                {
                    ColumnSpanLabel = this.ColumnSpanLabel,
                    Id = this.Name,
                    Required = this.Required,
                    Label = this.Label
                }.Render;

                string extendSelect = " defaultSelect";
                switch (this.ExtendSelect)
                {
                    case ExtendSelect.Select2:
                        extendSelect = " chosen-select";
                        break;
                    case ExtendSelect.Select2Add:
                        extendSelect = " chosen-select addSelect";
                        break;
                    case ExtendSelect.ButtonBAR:
                        extendSelect = " ButtonBAR";
                        break;
                    case ExtendSelect.None:
                    default:
                        break;
                }

                string pattern = @"  {7}
                            <div class=""col-sm-{1}"" id=""Div{0}"" style=""height:35px !important;"" title=""{3}"" data-rel=""tooltip""><!-- data-placement=""top""> -->
                                <select{8} class=""form-control col-xs-12 col-sm-12{9}"" id=""{0}"" data-placeholder=""{4}"" onchange=""{5}"">
                                    {6}
                                </select>
                                {2}
                            </div>";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Name,
                    this.ColumnSpan,
                    requiredLabel,
                    this.ToolTip,
                    this.Placeholder,
                    this.ChangeEvent,
                    optionsList,
                    label,
                    (this.GrantToWrite.HasValue && this.GrantToWrite == false) ? " disabled=\"disabled\"" : string.Empty,
                    extendSelect);
            }
        }
    }
}