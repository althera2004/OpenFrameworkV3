

namespace OpenFrameworkV3.Billing
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core.Enums;
    using OpenFrameworkV3.Core.ItemManager;

    /// <summary>Implements class for item fields</summary>
    [Serializable]
    public sealed class InvoicePersonDefinitionField
    {
        /// <summary>Maximum length</summary>
        [JsonProperty("BillingField")]
        private string billingField;

        /// <summary>Maximum length</summary>
        [JsonProperty("Name")]
        public string Name;

        [JsonIgnore]
        public string BillingField
        {
            get
            {
                return this.billingField ?? this.Name;
            }
        }

        /// <summary>Gets an empty field</summary>
        [JsonIgnore]
        public static InvoicePersonDefinitionField Empty
        {
            get
            {
                return new InvoicePersonDefinitionField
                {
                    Name = string.Empty,
                    billingField = string.Empty
                };
            }
        }

        /// <summary>Gets SQL sentence to extract value of field and label for ReplacedBy configuration</summary>
        /// <param name="replacedBy">ReplacedBy value configuration</param>
        /// <returns>SQL sentence to extract value of field and label for ReplacedBy configuration</returns>
        public string SqlFieldExtractor(ItemDefinition definition)
        {
            var field = definition.Fields.First(f => f.Name.Equals(this.Name, StringComparison.OrdinalIgnoreCase));
            string res = string.Empty;
            switch (field.DataType)
            {
                case ItemFieldDataType.Integer:
                case ItemFieldDataType.Long:
                case ItemFieldDataType.Decimal:
                case ItemFieldDataType.Float:
                case ItemFieldDataType.NullableInteger:
                case ItemFieldDataType.NullableLong:
                case ItemFieldDataType.NullableDecimal:
                case ItemFieldDataType.NullableFloat:
                    res = string.Format(
                        CultureInfo.InvariantCulture,
                        @" ISNULL(CAST(Item.{0} AS nvarchar(20)),'null') AS {1},",
                        this.Name,
                        this.BillingField);
                    break;
                case ItemFieldDataType.DateTime:
                case ItemFieldDataType.NullableDateTime:
                    res = string.Format(
                        CultureInfo.InvariantCulture,
                        @" ISNULL('""' + CONVERT(varchar(11),Item.{0}, 103) + '""', '')  AS {1},",
                        this.Name,
                        this.BillingField);
                    break;
                case ItemFieldDataType.Boolean:
                case ItemFieldDataType.NullableBoolean:
                    res = string.Format(
                        CultureInfo.InvariantCulture,
                        @" CASE WHEN Item.{0} = 1 THEN 'true' ELSE 'false' END AS {1},",
                        this.Name,
                        this.BillingField);
                    break;
                default:
                    res = string.Format(
                        CultureInfo.InvariantCulture,
                        @" ISNULL(Item.{0},'') AS {1},",
                        this.Name,
                        this.BillingField);
                    break;
            }

            return res;
        }
    }
}