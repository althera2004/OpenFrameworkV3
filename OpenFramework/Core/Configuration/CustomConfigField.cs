

namespace OpenFrameworkV3.Core.Configuration
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public partial class CustomConfig
    {

        public class CustomConfigField
        {
            [JsonProperty("Name")]
            public string Name { get; set; }

            [JsonProperty("Label")]
            public string Label { get; set; }

            [JsonProperty("Type")]
            public string Type { get; set; }

            [JsonProperty("Required")]
            public bool Required { get; set; }

            [JsonIgnore]
            public static CustomConfigField Empty
            {
                get
                {
                    return new CustomConfigField
                    {
                        Name = string.Empty,
                        Label = string.Empty,
                        Type = string.Empty,
                        Required = false
                    };
                }
            }

            [JsonIgnore]
            public string Json
            {
                get
                {
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        @"{{""Name"":""{0}"", ""Label"":""{1}"", ""Type"":""{2}"", ""Required"":{3}}}",
                        this.Name,
                        Tools.Json.JsonCompliant(this.Label),
                        this.Type,
                        ConstantValue.Value(this.Required));
                }
            }

            public static string JsonList(IReadOnlyCollection<CustomConfigField> list)
            {
                if(list == null)
                {
                    return Constant.JavaScriptNull;
                }

                if(list.Count == 0)
                {
                    return Tools.Json.EmptyJsonList;
                }

                var res = new StringBuilder("[");
                var first = true;
                foreach(var item in list)
                {
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "{0}{1}",
                        first ? string.Empty : ",",
                        item.Json);

                    first = false;
                }

                res.Append("]");
                return res.ToString();
            }
        }
    }
}