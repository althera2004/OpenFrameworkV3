// -----------------------------------------------------------------------
// <copyright file="BillingConfig.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace OpenFrameworkV3.Core.InstanceManager
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using Newtonsoft.Json;

    /// <summary>Customer billing configuration</summary>
    [Serializable]
    public sealed class BillingConfig
    {
        /// <summary>Gets an empty instance of BillngConfiguration class</summary>
        [JsonIgnore]
        public static BillingConfig Empty
        {
            get
            {
                return new BillingConfig
                {
                    Month30 = true,
                    MultimpleBankAccount = false,
                    InvoicePattern = string.Empty,
                    CustomInvoiceTemplate = false,
                    Active = false
                };
            }
        }

        /// <summary>Gets a JSON definition of biling configuration</summary>
        [JsonIgnore]
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Month30"":{0},""MultipleBankAccount"":{1},""InvoicePattern"":""{2}"",""CustomInvoiceTemplate"":{3},""Active"":{4}}}",
                    ConstantValue.Value(this.Month30),
                    ConstantValue.Value(this.MultimpleBankAccount),
                    Tools.Json.JsonCompliant(this.InvoicePattern ?? string.Empty),
                    ConstantValue.Value(this.CustomInvoiceTemplate),
                    Tools.Json.JsonCompliant(this.Active));
            }
        }

        /// <summary>Gets or sets mode</summary>
        [JsonIgnore]
        public bool Active { get; set; }

        /// <summary>Gets or sets mode</summary>
        [JsonProperty("Month30")]
        public bool Month30 { get; set; }

        /// <summary>Gets or sets mode</summary>
        [JsonProperty("MultimpleBankAccount")]
        public bool MultimpleBankAccount { get; set; }

        /// <summary>Gets or sets "Desglose IVA"</summary>
        [JsonProperty("DesgloseIVA")]
        public bool DesgloseIVA { get; set; }

        /// <summary>Gets or sets mode</summary>
        [JsonProperty("CustomInvoiceTemplate")]
        public bool CustomInvoiceTemplate { get; set; }

        /// <summary>Gets or sets mode</summary>
        [JsonProperty("InvoicePattern")]
        public string InvoicePattern { get; set; }

        /// <summary>Gets or sets identifiers of billing items</summary>
        [JsonProperty("Items")]
        public string Items { get; set; }

        [JsonIgnore]
        public ReadOnlyCollection<long> ItemsId
        {
            get
            {
                var res = new List<long>();
                if (!string.IsNullOrEmpty(this.Items))
                {
                    var parts = this.Items.Split('|');
                    foreach(var id in parts)
                    {
                        res.Add(Convert.ToInt64(id));
                    }
                }

                return new ReadOnlyCollection<long>(res);
            }
        }
    }
}