

namespace OpenFrameworkV3.Calendar
{
    using System.Globalization;
    using System.IO;
    using System.Web.Script.Serialization;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core;

    public class Source
    {
        /*"Title":"Visita médica",
	"Icon": "fa fa-exclamation-triangle",
	"Color": "#f00",
	"TextColor": "#fff",
	"QueryType": "Text",
    "Query":"*/

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Icon")]
        public string Icon { get; set; }

        [JsonProperty("Color")]
        public string Color { get; set; }

        [JsonProperty("TextColor")]
        public string TextColor { get; set; }

        [JsonProperty("QueryType")]
        public string QueryType { get; set; }

        [JsonProperty("Query")]
        public string Query { get; set; }

        public string FinalQuery(int month, int year)
        {
            var res = this.Query;
            res = res.Replace("@Month", month.ToString());
            res = res.Replace("@Year", year.ToString());
            return res;
        }

        public static Source FromFile(string fileName, string instanceName)
        {            
            var path = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}.calendar", Instance.Path.Calendar(instanceName), fileName);

            if (File.Exists(path))
            {
                using (var input = new StreamReader(path))
                {
                    var json = input.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                    dynamic data = serializer.Deserialize(json, typeof(object));

                    return new Source
                    {
                        Color = data["Color"] ?? "00f",
                        TextColor = data["TextColor"] ?? "fff",
                        Icon = data["Icon"] ?? "fa fa-calendar",
                        Query = data["Query"],
                        QueryType = data["QueryType"],
                        Title = data["Title"]
                    };
                }
            }

            return null;
        }
    }
}