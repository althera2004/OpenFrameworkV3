
namespace OpenFrameworkV3.Billing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web.Script.Serialization;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core;

    public class InvoicePersonDefinition
    {

        /// <summary>Gets the fields</summary>
        [JsonProperty("Fields")]
        public List<InvoicePersonDefinitionField> Fields;

        [JsonProperty("ItemDefinitionId")]
        public long ItemDefinitionId { get; set; }

        /*/// <summary>Gets an empty definition</summary>
        [JsonIgnore]
        public static InvoicePersonDefinition Empty
        {
            get
            {
                return new InvoicePersonDefinition
                {
                    fields = null,
                    ItemDefinitionId = Constant.DefaultId
                };
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<InvoicePersonDefinitionField> Fields
        {
            get
            {
                if(this.fields == null)
                {
                    new ReadOnlyCollection<InvoicePersonDefinitionField>(new List<InvoicePersonDefinitionField>());
                }

                return new ReadOnlyCollection<InvoicePersonDefinitionField>(this.fields);
            }
        }*/

        public static InvoicePersonDefinition LoadFromFile(long itemDefinitionId, string instanceName)
        {
            var itemName = Persistence.ItemDefinitions(instanceName).First(d => d.Id == itemDefinitionId).ItemName;
            return LoadFromFile(itemName, instanceName);
        }

        public static InvoicePersonDefinition LoadFromFile(string itemName, string instanceName)
        {
            var path = string.Format(
                CultureInfo.InvariantCulture,
                "{0}{2}{1}.billing",
                Instance.Path.Definition(instanceName),
                itemName,
                Instance.Path.Definition(instanceName).EndsWith("\\", StringComparison.OrdinalIgnoreCase) ? string.Empty : "\\");

            var jsonData = string.Empty;
            using(var input = new StreamReader(path))
            {
                jsonData = input.ReadToEnd();
            }

            var res = new JavaScriptSerializer
            {
                MaxJsonLength = 500000000
            }.Deserialize<InvoicePersonDefinition>(jsonData);

            return res;
        }
    }
}