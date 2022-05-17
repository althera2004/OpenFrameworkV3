namespace OpenFrameworkV3.Feature
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core;
    using OpenFrameworkV3.Tools;

    public class CustomListUpdate
    {
        [JsonProperty("ListId")]
        public long ListId { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("UpdateConditions")]
        private CustomListUpdateCondition[] updateConditions;

        [JsonIgnore]
        public ReadOnlyCollection<CustomListUpdateCondition> UpdateConditions
        {
            get
            {
                if (this.updateConditions == null)
                {
                    return new ReadOnlyCollection<CustomListUpdateCondition>(new List<CustomListUpdateCondition>());
                }

                return new ReadOnlyCollection<CustomListUpdateCondition>(this.updateConditions.ToList());
            }
        }

        public static CustomListUpdate GetDefinitionByFile(string fileName)
        {
            var customListUpdateCondition = new CustomListUpdate();
            if (File.Exists(fileName))
            {
                using (var input = new StreamReader(fileName))
                {
                    customListUpdateCondition = JsonConvert.DeserializeObject<CustomListUpdate>(input.ReadToEnd());
                }
            }

            return customListUpdateCondition;
        }

        // weke
        //public static ReadOnlyCollection<CustomListUpdate> All(string instanceName)
        //{

        //    var res = new List<CustomListUpdate>();

        //    var path = Instance.Path.Cus

        //    Basics.VerifyFolder(instance.PathCustomListUpdate);

        //    var files = Directory.GetFiles(instance.PathCustomListUpdate, "*.listx");
        //    foreach (var filename in files)
        //    {
        //        res.Add(GetDefinitionByFile(filename));
        //    }

        //    return new ReadOnlyCollection<CustomListUpdate>(res);
        //}
    }
}
