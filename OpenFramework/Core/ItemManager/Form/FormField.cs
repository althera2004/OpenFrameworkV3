namespace OpenFrameworkV3.Core.ItemManager.ItemForm
{
    using System.Globalization;
    using System.Text;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core.Enums;

    /// <summary>Implements class for form fields</summary>
    public sealed class FormField
    {
        /// <summary>Size of the field (unused)</summary>
        [JsonProperty("Size")]
        private readonly int size;

        /// <summary>Checks if it's read-only</summary>
        [JsonProperty("ReadOnly")]
        private readonly bool readOnly;

        /// <summary>Text representation of type field</summary>
        [JsonProperty("TypeField")]
        private readonly string typeField;

        /// <summary>Type of select representation in HTML page</summary>
        [JsonProperty("ExtendSelect")]
        private readonly string extendSelect;

        /// <summary>Function (if the field is a button)</summary>
        [JsonProperty("Function")]
        private readonly string function;

        /// <summary>Values to send to the function (if the field is a button)</summary>
        [JsonProperty("FunctionValues")]
        private readonly object[] functionValues;

        /// <summary>Checks if the field required an item instance</summary>
        [JsonProperty("ItemRequired")]
        private bool? itemRequired;

        /// <summary>Gets or sets the name of fixed list values</summary>
        [JsonProperty("FixedListMode")]
        private string fixedListMode { get; set; }

        [JsonIgnore]
        public string FixedListMode
        {
            get
            {
                return this.fixedListMode ?? "RadioList";
            }
        }

        /// <summary>Type of the field</summary>
        [JsonProperty("Type")]
        public string Type { get; private set; }

        /// <summary>Type of the field</summary>
        [JsonProperty("Config")]
        public string Config { get; private set; }

        /// <summary>Checks if the field show "Botón Acción Rápida"</summary>
        [JsonProperty("BAR")]
        public bool? BAR { get; set; }

        /// <summary>Gets number of rows</summary>
        [JsonProperty("Rows")]
        public int? Rows { get; private set; }

        /// <summary>Gets number of columns</summary>
        [JsonProperty("ColSpan")]
        public int? ColSpan { get; private set; }

        /// <summary>Gets the name of the field</summary>
        [JsonProperty("Name")]
        public string Name { get; private set; }

        /// <summary>Gets the label of the field (used if the field is not a regular input)</summary>
        [JsonProperty("Label")]
        public string Label { get; private set; }

        /// <summary>Gets the help text of the field (used if the field is not a regular input)</summary>
        [JsonProperty("HelpText")]
        public string HelpText { get; private set; }

        /// <summary>Gets the icon of the field if is button</summary>
        [JsonProperty("Icon")]
        public string Icon { get; private set; }

        /// <summary>Gets the style of the field if is button</summary>
        [JsonProperty("Style")]
        public string Style { get; private set; }

        /// <summary>Gets a value indicating whether textarea field has wysiwyg format</summary>
        [JsonProperty("Wysiwyg")]
        public bool Wysiwyg { get; private set; }

        /// <summary>Gets a value indicating if field is rendered in compact mode</summary>
        [JsonProperty("Compact")]
        public bool? Compact { get; private set; }

        /// <summary>Gets a value indicating if field is hidden by default</summary>
        [JsonProperty("Hidden")]
        public bool Hidden { get; private set; }

        /// <summary>Gets a value indicating if field is hidden by default</summary>
        [JsonProperty("LabelAlign")]
        public string LabelAlign { get; private set; }

        /// <summary>Gets a value for field width</summary>
        [JsonProperty("Width")]
        public string Width { get; private set; }

        /// <summary>Gets a value indicating whether public access to itemRequired</summary>
        [JsonIgnore]
        public bool ItemRequired
        {
            get
            {
                if(!this.itemRequired.HasValue)
                {
                    return false;
                }

                return (bool)this.itemRequired;
            }
        }

        /// <summary>Gets code for the function (if the field is a button)</summary>
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

                // If there are any values defined...
                if(this.functionValues != null)
                {
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

                        // If the value is a string, format as string
                        string value = val.GetType().ToString().ToUpperInvariant() == "SYSTEM.STRING" ? string.Format(CultureInfo.InvariantCulture, @"'{0}'", val) : val.ToString();

                        // Append the value
                        res.Append(value);
                    }
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}({1})",
                    this.function,
                    res);
            }
        }

        /// <summary>Gets the type of select functionality</summary>
        [JsonIgnore]
        public ExtendSelect ExtendSelect
        {
            get
            {
                if(string.IsNullOrEmpty(this.extendSelect))
                {
                    return ExtendSelect.None;
                }

                switch(this.extendSelect.ToUpperInvariant().Trim())
                {
                    case "SELECT2":
                        return ExtendSelect.Select2;
                    case "SELECT2ADD":
                        return ExtendSelect.Select2Add;
                    case "CHECKBOX":
                        return ExtendSelect.ButtonBAR;
                    case "BUTTONBAR":
                    default:
                        return ExtendSelect.None;
                }
            }
        }

        /// <summary>Gets the type of the field</summary>
        [JsonIgnore]
        public ItemFieldDataType TypeField
        {
            get
            {
                if(string.IsNullOrEmpty(this.typeField))
                {
                    return ItemFieldDataType.None;
                }

                switch(this.typeField.ToUpperInvariant().Trim())
                {
                    case "DATEPICKER":
                        return ItemFieldDataType.DateTime;
                    case "SELECT":
                        return ItemFieldDataType.FixedListLinked;
                    case "NUMERIC":
                        return ItemFieldDataType.Decimal;
                    case "CHECKBOX":
                        return ItemFieldDataType.Boolean;
                    case "IBAN":
                        return ItemFieldDataType.IBAN;
                    case "COMBINEFK":
                        return ItemFieldDataType.CombineFK;
                    case "LIST":
                        return ItemFieldDataType.List;
                    case "ORDEREDLIST":
                        return ItemFieldDataType.OrderedList;
                    case "TEXT":
                    default:
                        return ItemFieldDataType.Text;
                }
            }
        }

        /// <summary>Gets field size</summary>
        [JsonIgnore]
        public int Size
        {
            get
            {
                if(this.size == 0)
                {
                    return 12;
                }

                return this.size;
            }
        }

        /// <summary>Gets a value indicating whether field is read only</summary>
        [JsonIgnore]
        public bool ReadOnly
        {
            get
            {
                return this.readOnly;
            }
        }
    }
}