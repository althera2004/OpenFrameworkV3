// --------------------------------
// <copyright file="EventDefinition.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Calendar
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web.Script.Serialization;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core;

    public class EventDefinition
    {
        [JsonProperty("CompanyId")]
        public long CompanyId { get; set; }

        [JsonProperty("ItemDefintionId")]
        public long ItemDefinitionId { get; set; }

        [JsonProperty("TextColor")]
        public string TextColor { get; set; }

        [JsonProperty("TagColor")]
        public string TagColor { get; set; }

        [JsonProperty("Icon")]
        private string icon;

        [JsonProperty("Data")]
        private EventDefinitionData[] data;

        public static EventDefinition Empty
        {
            get
            {
                return new EventDefinition
                {
                    CompanyId = 0,
                    ItemDefinitionId = Constant.DefaultId,
                    TextColor = "#fff",
                    TagColor = "#333"
                };
            }
        }


        [JsonIgnore]
        public string Icon
        {
            get
            {
                if (string.IsNullOrEmpty(this.icon))
                {
                    this.icon = "far fa-calendar";
                }

                return this.icon;
            }
        }

        public ReadOnlyCollection<EventDefinitionData> Data
        {
            get
            {
                if(this.data != null)
                {
                    return new ReadOnlyCollection<EventDefinitionData>(data.ToList());
                }

                return new ReadOnlyCollection<EventDefinitionData>(new List<EventDefinitionData>());
            }
        }

        public static EventDefinition FromFile(string fileName, string instanceName)
        {
            var path = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}.event", Instance.Path.Calendar(instanceName), fileName);
            var res = EventDefinition.Empty;
            if (File.Exists(path))
            {
                if (File.Exists(path))
                {
                    using (var input = new StreamReader(path))
                    {
                        var data = input.ReadToEnd();
                        if (data != null)
                        {
                            res = new JavaScriptSerializer
                            {
                                MaxJsonLength = 500000000
                            }.Deserialize<EventDefinition>(data);
                        }
                    }
                }
            }

            return res;
        }
    }
}
