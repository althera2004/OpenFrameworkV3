﻿namespace OpenFrameworkV3.Core.ItemManager
{
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core.ItemManager.List;
    using System;

    /// <summary>Implements ColumnDataFormat class</summary>
    [Serializable]
    public sealed class ColumnDataFormat
    {
        /// <summary>Column decimal precision</summary>
        [JsonProperty("Precission")]
        private string precision;

        /// <summary>Gets a default column definition for money format</summary>
        public static ColumnDataFormat MoneyFormat
        {
            get
            {
                return new ColumnDataFormat
                {
                    Name = "MONEY",
                    precision = "2",
                    Prefix = string.Empty,
                    Suffix = " &euro;"
                };
            }
        }

        /// <summary>Gets a default column definition for GPS format</summary>
        public static ColumnDataFormat GPS
        {
            get
            {
                return new ColumnDataFormat
                {
                    Name = "GPS",
                    precision = "9",
                    Prefix = string.Empty,
                    Suffix = string.Empty
                };
            }
        }

        /// <summary>Gets a default column definition for decimal format</summary>
        public static ColumnDataFormat DecimalFormat
        {
            get
            {
                return new ColumnDataFormat
                {
                    Name = "DECIMAL",
                    precision = "5",
                    Prefix = string.Empty,
                    Suffix = string.Empty
                };
            }
        }

        /// <summary>Gets or sets column name</summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>Gets or sets a suffix for data content</summary>
        [JsonProperty("Sufix")]
        public string Suffix { get; set; }

        /// <summary>Gets or sets a prefix for data content</summary>
        [JsonProperty("Prefix")]
        public string Prefix { get; set; }

        /// <summary>Gets column data type</summary>
        [JsonIgnore]
        public DataFormatType Type
        {
            get
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    return DataFormatType.None;
                }

                switch (this.Name.ToUpperInvariant())
                {
                    case "TEXT":
                        return DataFormatType.Text;
                    case "MONEY":
                        return DataFormatType.Money;
                    case "PERCENTAGE":
                        return DataFormatType.Percentage;
                    case "BOOLEAN":
                    case "BOOLEANTEXT":
                        return DataFormatType.BooleanText;
                    case "BOOLEANICON":
                        return DataFormatType.BooleanIcon;
                    case "BOOLEANCHECK":
                        return DataFormatType.BooleanCheck;
                    case "BOOLEANCHECKNULL":
                        return DataFormatType.BooleanCheckNull;
                    case "DATE":
                        return DataFormatType.Date;
                    case "DATETIME":
                        return DataFormatType.DateTime;
                    case "DATEMONTH":
                        return DataFormatType.DateMonth;
                    case "DATETEXT":
                        return DataFormatType.DateText;
                    case "DECIMAL":
                        return DataFormatType.Decimal;
                    case "URL":
                        return DataFormatType.Url;
                    case "EMAIL":
                        return DataFormatType.Email;
                    case "IMAGE":
                        return DataFormatType.Image;
                    case "DOCUMENT":
                        return DataFormatType.Document;
                    case "GPS":
                        return DataFormatType.GPS;
                    default:
                        return DataFormatType.None;
                }
            }
        }

        /// <summary>Gets the decimal precision of value if apply them</summary>
        [JsonIgnore]
        public int? Precision
        {
            get
            {
                if (string.IsNullOrEmpty(this.precision))
                {
                    return null;
                }

                if (int.TryParse(this.precision, out int res))
                {
                    return res;
                }

                return null;
            }
        }

        /// <summary>Gets column data type</summary>
        /// <param name="type">Data type</param>
        public void SetType(DataFormatType type)
        {
            switch (type)
            {
                case DataFormatType.Date:
                    this.Name = "DATE";
                    break;
                case DataFormatType.Image:
                    this.Name = "IMAGE";
                    break;
                case DataFormatType.Document:
                    this.Name = "DOCUMENT";
                    break;
            }
        }
    }
}