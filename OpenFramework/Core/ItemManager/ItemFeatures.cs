// --------------------------------
// <copyright file="ItemFeatures.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager
{
    using Newtonsoft.Json;

    public class ItemFeatures
    {
        [JsonProperty("Attachs")]
        public bool Attachs { get; set; }

        [JsonProperty("BankAccount")]
        public bool BankAccount { get; set; }

        [JsonProperty("Invoices")]
        public bool Invoices { get; set; }

        [JsonProperty("Following")]
        public bool Following { get; set; }

        [JsonProperty("MailLink")]
        public bool MailLink { get; set; }

        [JsonProperty("Geolocation")]
        public bool Geolocation { get; set; }

        [JsonProperty("AdminRestriction")]
        public bool AdminRestriction { get; set; }

        [JsonProperty("Notes")]
        public bool Notes { get; set; }

        [JsonProperty("Sticky")]
        public bool Sticky { get; set; }

        [JsonProperty("FAQs")]
        public bool FAQs { get; set; }

        [JsonProperty("Traces")]
        public bool Traces { get; set; }

        [JsonProperty("ScopeView")]
        public bool ScopeView { get; set; }

        [JsonProperty("EventCalendar")]
        public bool EventCalendar { get; set; }

        [JsonProperty("Unloadable")]
        public bool Unloadable { get; set; }

        [JsonProperty("Importable")]
        public bool Importable { get; set; }

        [JsonProperty("News")]
        public bool News { get; set; }

        [JsonProperty("ShowOnPortal")]
        public bool ShowOnPortal { get; set; }

        [JsonProperty("Persistence")]
        public bool Persistence { get; set; }

        [JsonProperty("Tags")]
        public bool Tags { get; set; }

        [JsonProperty("Mailable")]
        public bool Mailable { get; set; }

        [JsonProperty("ContactPerson")]
        public bool ContactPerson { get; set; }

        [JsonProperty("DocumentSign")]
        public bool DocumentSign { get; set; }

        [JsonProperty("DocumentHistory")]
        public bool DocumentHistory { get; set; }

        [JsonProperty("Schedule")]
        public bool Schedule { get; set; }

        [JsonProperty("Firmafy")]
        public bool Firmafy { get; set; }

        [JsonIgnore]
        public static ItemFeatures Empty
        {
            get
            {
                return new ItemFeatures
                {
                    AdminRestriction = false,
                    Attachs = false,
                    BankAccount = false,
                    Invoices = false,
                    FAQs = false,
                    Notes = false,
                    Sticky = false,
                    Following = false,
                    Firmafy = false,
                    Geolocation = false,
                    Traces = false,
                    ScopeView = false,
                    EventCalendar = false,
                    Unloadable = false,
                    Importable = false,
                    News = false,
                    ShowOnPortal = false,
                    ContactPerson = false,
                    Mailable = false,
                    MailLink = false,
                    Persistence = false,
                    Tags = false,
                    Schedule = false
                };
            }
        }
    }
}