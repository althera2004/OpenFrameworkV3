// --------------------------------
// <copyright file="ItemField.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core.Enums;

    /// <summary>Implements class for item fields</summary>
    [Serializable]
    public sealed class ItemField
    {
        /// <summary>Name of default field type</summary>
        [JsonIgnore]
        public const string DefaultTypeName = "TEXT";

        /// <summary>Maximum length</summary>
        [JsonProperty("Length")]
        private readonly string length;

        /// <summary>Maximum length</summary>
        [JsonProperty("Geolocalization")]
        private readonly string geolocalization;

        /// <summary>Column sorting</summary>
        [JsonProperty("Sorting")]
        private readonly string sorting;

        /// <summary>Field is part of FK list</summary>
        [JsonProperty("FK")]
        private readonly string fk;

        /// <summary>Field label from JSON item file</summary>
        [JsonProperty("Label")]
        private string label;

        /// <summary>Column data format</summary>
        [JsonProperty("DataFormat")]
        public ColumnDataFormat ColumnDataFormat;

        /// <summary>Value of field</summary>
        [JsonIgnore]
        private object fieldValue;

        /// <summary>Gets an empty field</summary>
        [JsonIgnore]
        public static ItemField Empty
        {
            get
            {
                return new ItemField
                {
                    Name = string.Empty,
                    fieldValue = string.Empty,
                    label = string.Empty,
                    TypeName = "text"
                };
            }
        }

        /// <summary>Gets a default description field</summary>
        [JsonIgnore]
        public static ItemField DefaultDescription
        {
            get
            {
                return new ItemField
                {
                    Name = "Description",
                    fieldValue = string.Empty,
                    TypeName = "text"
                };
            }
        }

        /// <summary>Gets or sets a value indicating whether field is only a reference</summary>
        [JsonProperty("Referencial")]
        public bool Referencial { get; set; }

        /// <summary>Gets or sets item name of foreign item if value is a foreign key</summary>
        [JsonProperty("FKItem")]
        public string FKItem { get; set; }

        /// <summary>Gets or sets a value indicating whether field value is used in global search</summary>
        [JsonProperty("GlobalSearch")]
        public bool GlobalSearch { get; set; }

        /// <summary>Gets column sorting</summary>
        [JsonIgnore]
        public string Sorting
        {
            get
            {
                if (string.IsNullOrEmpty(this.sorting))
                {
                    return string.Empty;
                }

                return this.sorting;
            }
        }

        /// <summary>Gets or sets the name of fixed list values</summary>
        [JsonProperty("FixedListName")]
        public string FixedListName { get; set; }

        /// <summary>Gets or sets the column data type</summary>
        [JsonProperty("ColumnDataType")]
        public string ColumnDataType { get; set; }

        /// <summary>Gets or sets a value indicating whether value is used only for internal purposes</summary>
        [JsonProperty("Internal")]
        public bool Internal { get; set; }

        /// <summary>Gets or sets a value indicating whether if field is showed in list including Id field</summary>
        [JsonProperty("ShowInList")]
        public bool ShowInList { get; set; }

        /// <summary>Gets or sets the name of linked field</summary>
        [JsonProperty("VinculatedTo")]
        public string VinculatedTo { get; set; }

        /// <summary>Gets or sets the item name</summary>
        [JsonIgnore]
        public string ItemName { get; set; }

        /// <summary>Gets or sets the name of field</summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>Gets or sets code sequence for automatic codes fields</summary>
        [JsonProperty("CodeSequence")]
        public string CodeSequence { get; set; }

        /// <summary>Gets or sets a value indicating whether data is required</summary>
        [JsonProperty("Required")]
        public bool Required { get; set; }

        /// <summary>Gets a value indicating whether if document maintains original name</summary>
        [JsonProperty("MaintainOriginalName")]
        public bool MaintainOriginalName { get; private set; }

        /// <summary>Gets or sets a value indicating whether field is linkable</summary>
        [JsonProperty("Linkable")]
        public bool Linkable { get; set; }

        /// <summary>Gets or sets the fixed list identifier</summary>
        [JsonProperty("FixedListId")]
        public string FixedListId { get; set; }

        /// <summary>Gets the maximum length of field</summary>
        [JsonIgnore]
        public int? Length
        {
            get
            {
                if (string.IsNullOrEmpty(this.length))
                {
                    return null;
                }

                return Convert.ToInt32(this.length, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>Gets geographical localization index</summary>
        [JsonIgnore]
        public int? Geolocalization
        {
            get
            {
                if (string.IsNullOrEmpty(this.geolocalization))
                {
                    return null;
                }

                return Convert.ToInt32(this.geolocalization, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>Gets or sets the label of field in lists and forms</summary>
        [JsonIgnore]
        public string Label
        {
            get
            {
                if (string.IsNullOrEmpty(this.label))
                {
                    return this.Name;
                }

                return this.label;
            }

            set
            {
                this.label = value;
            }
        }

        /// <summary>Gets label of files specify for forms</summary>
        [JsonIgnore]
        public string LabelExcel
        {
            get
            {
                if (this.Required)
                {
                    return string.Format(CultureInfo.GetCultureInfo("en-us"), "{0}*", this.Label);
                }

                return this.Label;
            }
        }

        /// <summary>Gets label of files specify for forms</summary>
        [JsonIgnore]
        public string LabelForm
        {
            get
            {
                if (this.Required)
                {
                    return string.Format(CultureInfo.InvariantCulture, @"{0}<span style=""color:#ff0000;"">*</span>", this.Label);
                }

                return this.Label;
            }
        }

        /// <summary>Gets a value indicating whether if FK</summary>
        [JsonIgnore]
        public bool FK
        {
            get
            {
                if (!string.IsNullOrEmpty(this.fk) && this.fk.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>Gets data type of field</summary>
        [JsonIgnore]
        public ItemFieldDataType DataType
        {
            get
            {
                switch (this.TypeName.ToUpperInvariant())
                {
                    case "ITEMDEFINITION":
                        return ItemFieldDataType.ItemDefinition;
                    case "ITEMDESCRIPTION":
                        return ItemFieldDataType.ItemDescription;
                    case "APPLICATIONUSER":
                        return ItemFieldDataType.ApplicationUser;
                    case "SECURITYGROUP":
                        return ItemFieldDataType.SecurityGroup;
                    case "LONG":
                        if (this.Required == false)
                        {
                            return ItemFieldDataType.NullableLong;
                        }

                        return ItemFieldDataType.Long;
                    case "INTEGER":
                    case "INT":
                        if (this.Required == false)
                        {
                            return ItemFieldDataType.NullableInteger;
                        }

                        return ItemFieldDataType.Integer;
                    case "BOOLEAN":
                    case "BOOL":
                        if (this.Required == false)
                        {
                            return ItemFieldDataType.NullableBoolean;
                        }

                        return ItemFieldDataType.Boolean;
                    case "DATETIME":
                        if (this.Required == false)
                        {
                            return ItemFieldDataType.NullableDateTime;
                        }

                        return ItemFieldDataType.DateTime;
                    case "TIME":
                        if (this.Required == false)
                        {
                            return ItemFieldDataType.NullableTime;
                        }

                        return ItemFieldDataType.Time;
                    case "MONEY":
                        if (this.Required == false)
                        {
                            return ItemFieldDataType.NullableMoney;
                        }

                        return ItemFieldDataType.Money;
                    case "DECIMAL":
                        if (this.Required == false)
                        {
                            return ItemFieldDataType.NullableDecimal;
                        }

                        return ItemFieldDataType.Decimal;
                    case "TEXTAREA":
                        return ItemFieldDataType.Textarea;
                    case "EMAIL":
                        return ItemFieldDataType.Email;
                    case "URL":
                        return ItemFieldDataType.Url;
                    case "FIXEDLIST":
                        return ItemFieldDataType.FixedList;
                    case "FIXEDLISTLINKED":
                        return ItemFieldDataType.FixedListLinked;
                    case "IMAGE":
                        return ItemFieldDataType.Image;
                    case "CDNIMAGE":
                        return ItemFieldDataType.CDNImage;
                    case "IMAGEGALLERY":
                    case "PHOTOGALLERY":
                        return ItemFieldDataType.ImageGallery;
                    case "DOCUMENTGALLERY":
                    case "DOCUMENTFILE":
                    case "DOCUMENT":
                    case "FILE":
                        return ItemFieldDataType.DocumentFile;
                    case "TREE":
                        return ItemFieldDataType.Tree;
                    case "FOLDER":
                        return ItemFieldDataType.Folder;
                    case "IBAN":
                        return ItemFieldDataType.IBAN;
                    case "COMBINEFK":
                        return ItemFieldDataType.CombineFK;
                    case "ORDEREDLIST":
                        return ItemFieldDataType.OrderedList;
                    case "LIST":
                        return ItemFieldDataType.List;
                    case "POSTALADDRESS":
                        return ItemFieldDataType.PostalAddress;
                    case "GUID":
                        if (this.Required)
                        {
                            return ItemFieldDataType.Guid;
                        }

                        return ItemFieldDataType.NullableGuid;
                    case "TEXT":
                    default:
                        return ItemFieldDataType.Text;
                }
            }
        }

        /// <summary>Gets or sets the value of field</summary>
        [JsonIgnore]
        public object Value
        {
            get
            {
                if (this.fieldValue == null)
                {
                    return null;
                }

                switch (this.DataType)
                {
                    case ItemFieldDataType.Integer:
                        return (int)this.fieldValue;
                    case ItemFieldDataType.NullableInteger:
                        return (int?)this.fieldValue;
                    case ItemFieldDataType.Decimal:
                        return (decimal)this.fieldValue;
                    case ItemFieldDataType.NullableDecimal:
                        return (decimal?)this.fieldValue;
                    case ItemFieldDataType.Guid:
                        return (Guid)this.fieldValue;
                    case ItemFieldDataType.NullableGuid:
                        return (Guid?)this.fieldValue;
                    case ItemFieldDataType.None:
                    case ItemFieldDataType.Text:
                    default:
                        return (string)this.fieldValue;
                }
            }

            set
            {
                switch (this.DataType)
                {
                    case ItemFieldDataType.None:
                    case ItemFieldDataType.Text:
                        this.fieldValue = (string)value;
                        break;
                    case ItemFieldDataType.Integer:
                        this.fieldValue = (int)value;
                        break;
                    case ItemFieldDataType.NullableInteger:
                        this.fieldValue = (int?)value;
                        break;
                    case ItemFieldDataType.Decimal:
                        this.fieldValue = (decimal)value;
                        break;
                    case ItemFieldDataType.NullableDecimal:
                        this.fieldValue = (decimal?)value;
                        break;
                    case ItemFieldDataType.Guid:
                        this.fieldValue = (Guid)value;
                        break;
                    case ItemFieldDataType.NullableGuid:
                        this.fieldValue = (Guid?)value;
                        break;
                    default:
                        this.fieldValue = value;
                        break;
                }
            }
        }

        /// <summary>Gets data type text</summary>
        [JsonIgnore]
        public string DataTypeLabel
        {
            get
            {
                return Enum.GetName(typeof(ItemFieldDataType), this.DataType);
            }
        }

        /// <summary>Gets SQL type of field</summary>
        [JsonIgnore]
        public string DataSqlType
        {
            get
            {
                switch (this.DataType)
                {
                    default:
                    case ItemFieldDataType.None:
                    case ItemFieldDataType.Text:
                    case ItemFieldDataType.Textarea:
                    case ItemFieldDataType.Url:
                    case ItemFieldDataType.Email:
                    case ItemFieldDataType.Image:
                    case ItemFieldDataType.DocumentFile:
                    case ItemFieldDataType.Folder:
                    case ItemFieldDataType.CDNImage:
                    case ItemFieldDataType.ImageGallery:
                    case ItemFieldDataType.NullableGeoposition:
                    case ItemFieldDataType.ItemDescription:
                    case ItemFieldDataType.List:
                    case ItemFieldDataType.OrderedList:
                    case ItemFieldDataType.DocumentGallery:
                        return "nvarchar";
                    case ItemFieldDataType.IBAN:
                        return "varchar";
                    case ItemFieldDataType.Integer:
                    case ItemFieldDataType.NullableInteger:
                        return "int";
                    case ItemFieldDataType.Money:
                    case ItemFieldDataType.Decimal:
                    case ItemFieldDataType.NullableDecimal:
                        return "decimal";
                    case ItemFieldDataType.Long:
                    case ItemFieldDataType.NullableLong:
                    case ItemFieldDataType.FixedList:
                    case ItemFieldDataType.FixedListLinked:
                    case ItemFieldDataType.SecurityGroup:
                    case ItemFieldDataType.ApplicationUser:
                    case ItemFieldDataType.ItemDefinition:
                    case ItemFieldDataType.PostalAddress:
                        return "bigint";
                    case ItemFieldDataType.Float:
                    case ItemFieldDataType.NullableFloat:
                        return "float";
                    case ItemFieldDataType.DateTime:
                    case ItemFieldDataType.NullableDateTime:
                        return "datetime";
                    case ItemFieldDataType.NullableTime:
                        return "time";
                    case ItemFieldDataType.TriState:
                    case ItemFieldDataType.Boolean:
                    case ItemFieldDataType.NullableBoolean:
                        return "bit";
                    case ItemFieldDataType.Geoposition:
                    case ItemFieldDataType.Guid:
                    case ItemFieldDataType.NullableGuid:
                        return "uniqueidentifier";
                    case ItemFieldDataType.CombineFK:
                        return "combineFK";
                }
            }
        }

        /// <summary>Gets type of data value</summary>
        [JsonIgnore]
        public string DataTypePropertyValue
        {
            get
            {
                return this.TypeName.ToLowerInvariant().Replace("nullable", string.Empty);
            }
        }

        /// <summary>Gets data type description for user interface</summary>
       public string DataTypeInterfaceLabel(string language, string instanceName)
        {
            return ApplicationDictionary.Translate("Core_DataType_" + Enum.GetName(typeof(ItemFieldDataType), this.DataType).ToLowerInvariant().Replace("nullable", string.Empty), language, instanceName);            
        }

        /// <summary>Gets or sets the data type of field</summary>
        [JsonProperty("Type")]
        public string TypeName { get; set; }

        /// <summary>Gets or sets path for CDN images</summary>
        [JsonProperty("CDNImagePath")]
        public string CDNImagePath { get; set; }

        /// <summary>Gets or sets mark for CDN images</summary>
        [JsonProperty("CDNImageMark")]
        public string CDNImageMark { get; set; }

        /// <summary>Gets SQL sentence to extract value of field and label</summary>
        [JsonIgnore]
        public string SqlFieldExtractor
        {
            get
            {
                string res = string.Empty;
                switch (this.DataType)
                {
                    case ItemFieldDataType.ItemDescription:
                    case ItemFieldDataType.SecurityGroup:
                    case ItemFieldDataType.ApplicationUser:
                    case ItemFieldDataType.FixedList:
                    case ItemFieldDataType.Integer:
                    case ItemFieldDataType.Long:
                    case ItemFieldDataType.Decimal:
                    case ItemFieldDataType.Float:
                    case ItemFieldDataType.Money:
                    case ItemFieldDataType.NullableInteger:
                    case ItemFieldDataType.NullableLong:
                    case ItemFieldDataType.NullableDecimal:
                    case ItemFieldDataType.NullableFloat:
                    case ItemFieldDataType.NullableMoney:
                    case ItemFieldDataType.ItemDefinition:
                        res = string.Format(
                            CultureInfo.InvariantCulture,
                            @"'""{0}"":' + ISNULL(CAST(Item.{0} AS nvarchar(20)),'null') + ','",
                            this.Name);
                        break;
                    case ItemFieldDataType.DateTime:
                    case ItemFieldDataType.NullableDateTime:
                        res = string.Format(
                            CultureInfo.InvariantCulture,
                            @"'""{0}"":' + ISNULL('""' + CONVERT(varchar(11),Item.{0}, 103) + '""', 'null') + ','",
                            this.Name);
                        break;
                    case ItemFieldDataType.Boolean:
                    case ItemFieldDataType.NullableBoolean:
                        res = string.Format(
                            CultureInfo.InvariantCulture,
                            @"'""{0}"":' +  CASE WHEN Item.{0} = 1 THEN 'true' ELSE 'false' END + ','",
                            this.Name);
                        break;
                    case ItemFieldDataType.Image:
                        res = string.Format(
                            CultureInfo.InvariantCulture,
                            @"'""{0}"":' + CASE WHEN Item.{0} IS NULL THEN 'null,' ELSE '""' + Item.{0} + '"",' END",
                            this.Name);
                        break;
                    default:
                        res = string.Format(
                            CultureInfo.InvariantCulture,
                            @"'""{0}"":""' +  REPLACE(ISNULL(Item.{0},''),'""','^""') + '"",'",
                            this.Name);
                        break;
                }

                return res;
            }
        }

        /// <summary>Gets part of SQL sentence to extract value of field</summary>
        [JsonIgnore]
        public string SqlFieldExtractorValue
        {
            get
            {
                switch (this.DataType)
                {
                    case ItemFieldDataType.Integer:
                    case ItemFieldDataType.Long:
                    case ItemFieldDataType.Decimal:
                    case ItemFieldDataType.Float:
                    case ItemFieldDataType.Money:
                    case ItemFieldDataType.NullableInteger:
                    case ItemFieldDataType.NullableLong:
                    case ItemFieldDataType.NullableDecimal:
                    case ItemFieldDataType.NullableFloat:
                    case ItemFieldDataType.NullableMoney:
                        return string.Format(
                            CultureInfo.InvariantCulture,
                            @"ISNULL(CAST(Item.{0} AS nvarchar(20)),'null')",
                            this.Name);
                    case ItemFieldDataType.DateTime:
                    case ItemFieldDataType.NullableDateTime:
                        return string.Format(
                            CultureInfo.InvariantCulture,
                            @"ISNULL('""' + CONVERT(varchar(11),Item.{0}, 102) + '""', 'null')",
                            this.Name);
                    case ItemFieldDataType.Boolean:
                    case ItemFieldDataType.NullableBoolean:
                        return string.Format(
                            CultureInfo.InvariantCulture,
                            @"CASE WHEN Item.{0} = 1 THEN 'true' ELSE 'false' END",
                            this.Name);
                    case ItemFieldDataType.Image:
                        return string.Format(
                            CultureInfo.InvariantCulture,
                            @"CASE WHEN Item.{0} IS NULL THEN 'null' ELSE '""' + Item.{0} + '""' END",
                            this.Name);
                    default:
                        return string.Format(
                            CultureInfo.InvariantCulture,
                            @"RTRIM(REPLACE(ISNULL(Item.{0},''),'""','^""'))",
                            this.Name);
                }
            }
        }

        /// <summary>Gets field linked by a foreign key</summary>
        /// <param name="definition">Item source</param>
        /// <returns>Field referred by a foreign key</returns>
        public ReadOnlyCollection<ItemField> GetReferedField(ItemDefinition definition)
        {
            if (this.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase) && !this.Name.StartsWith("Core_", StringComparison.OrdinalIgnoreCase))
            {
                var foreignItem = this.Name.Substring(0, this.Name.Length - 2);
                var foreingRelation = definition.ForeignValues.FirstOrDefault(fv => fv.ItemName.Equals(foreignItem));
                if (foreingRelation != null)
                {
                    var res = new List<ItemField>();
                    var referedItem = Persistence.ItemDefinitionByName(foreignItem, definition.InstanceName);
                    foreach (var field in referedItem.Layout.Description.Fields) {
                        var referedField = referedItem.Fields.First(f => f.Name.Equals(field.Name, StringComparison.OrdinalIgnoreCase));
                        if (referedField != null)
                        {
                            res.Add(referedField);
                        }
                    }

                    if(res.Count > 0)
                    {
                        return new ReadOnlyCollection<ItemField>(res);
                    }
                }
            }

            return null;
        }

        /// <summary>Sets text for field label</summary>
        /// <param name="text">Text for field label</param>
        public void SetLabel(string text)
        {
            this.label = text;
        }

        /// <summary>Gets field value in order to insert in item trace</summary>
        /// <param name="value">Field value</param>
        /// <returns>String representation of value</returns>
        public string TraceValue(object value)
        {
            switch (this.DataType)
            {
                case ItemFieldDataType.DateTime:
                    var dateValue = (DateTime)value;
                    return string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", dateValue);
                case ItemFieldDataType.Boolean:
                    bool boolValue = (bool)value;
                    return boolValue ? "true" : "false";
                default:
                    return value.ToString();
            }
        }

        /// <summary>Gets SQL sentence to extract value of field and label for ReplacedBy configuration</summary>
        /// <param name="replacedBy">ReplacedBy value configuration</param>
        /// <returns>SQL sentence to extract value of field and label for ReplacedBy configuration</returns>
        public string SqlFieldExtractorReplace(string replacedBy)
        {
            string res = string.Empty;
            switch (this.DataType)
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
                        @"'""{1}"":' + ISNULL(CAST(Item.{0} AS nvarchar(20)),'null') + ','",
                        this.Name,
                        replacedBy);
                    break;
                case ItemFieldDataType.DateTime:
                case ItemFieldDataType.NullableDateTime:
                    res = string.Format(
                        CultureInfo.InvariantCulture,
                        @"'""{1}"":' + ISNULL('""' + CONVERT(varchar(11),Item.{0}, 103) + '""', 'null') + ','",
                        this.Name,
                        replacedBy);
                    break;
                case ItemFieldDataType.Boolean:
                case ItemFieldDataType.NullableBoolean:
                    res = string.Format(
                        CultureInfo.InvariantCulture,
                        @"'""{1}"":' +  CASE WHEN Item.{0} = 1 THEN 'true' ELSE 'false' END + ','",
                        this.Name,
                        replacedBy);
                    break;
                default:
                    res = string.Format(
                        CultureInfo.InvariantCulture,
                        @"'""{1}"":""' +  ISNULL(Item.{0},'') + '"",'",
                        this.Name,
                        replacedBy);
                    break;
            }

            return res;
        }
    }
}