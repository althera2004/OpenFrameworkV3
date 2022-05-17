// --------------------------------
// <copyright file="CompanyProfileConfig.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using Newtonsoft.Json;
    using static OpenFrameworkV3.Core.Configuration.CustomConfig;

    /// <summary>Implements company's profile configuration</summary>
    [Serializable]
    public class CompanyProfileConfig
    {
        /// <summary>Gets an empty instance of MailConfiguration class</summary>
        [JsonIgnore]
        public static CompanyProfileConfig Empty
        {
            get
            {
                return new CompanyProfileConfig
                {
                    CompanyAddressEnabled = false,
                    BankAccountEnabled = false,
                    ContactPersonEnabled = false
                };
            }
        }

        /// <summary>Gets a JSON definition of profile configuration</summary>
        [JsonIgnore]
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"
    ""CompanyProfile"": {{
        ""CompanyAddressEnabled"": {0},
        ""BankAccountEnabled"": {1},
        ""ContactPersonEnabled"": {2},
        ""CustomConfig"": {3},
        ""CustomFields"":{4}
    }}",

                    ConstantValue.Value(this.CompanyAddressEnabled),
                    ConstantValue.Value(this.BankAccountEnabled),
                    ConstantValue.Value(this.ContactPersonEnabled),
                    ConstantValue.Value(this.CustomConfig),
                    CustomConfigField.JsonList(this.CustomFields));
            }
        }
        /// <summary>Gets or sets a value indicating whether if profile has ContactPerson enabled</summary>
        [JsonProperty("ContactPersonEnabled")]
        public bool ContactPersonEnabled { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has BankAccount enabled</summary>
        [JsonProperty("BankAccountEnabled")]
        public bool BankAccountEnabled { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has CompanyAddress enabled</summary>
        [JsonProperty("CompanyAddressEnabled")]
        public bool CompanyAddressEnabled { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has custom config</summary>
        [JsonProperty("CustomConfig")]
        public bool CustomConfig { get; set; }

        [JsonProperty("CustomFields")]
        private CustomConfig.CustomConfigField[] customFields;

        [JsonIgnore]
        public ReadOnlyCollection <CustomConfig.CustomConfigField> CustomFields
        {
            get
            {
                if(this.customFields == null)
                {
                    return new ReadOnlyCollection<CustomConfig.CustomConfigField>(new List<CustomConfig.CustomConfigField>());
                }

                return new ReadOnlyCollection<CustomConfig.CustomConfigField>(this.customFields);
            }
        }

        public void AddCustomField(CustomConfigField field)
        {
            if (this.customFields == null)
            {
                this.customFields = new List<CustomConfigField>().ToArray();
            }

            var temp  = this.customFields.ToList();
            temp.Add(field);
            this.customFields = temp.ToArray();

        }
    }
}