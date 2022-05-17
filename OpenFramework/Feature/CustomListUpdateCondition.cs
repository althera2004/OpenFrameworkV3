namespace OpenFrameworkV3.Feature
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Newtonsoft.Json;

    public class CustomListUpdateCondition
    {
        [JsonProperty("ItemName")]
        public string ItemName { get; set; }

        [JsonProperty("Fields")]
        private string[] fields;

        [JsonProperty("Preconditions")]
        private CustomListUpdatePrecondition[] preconditions;

        [JsonIgnore]
        public ReadOnlyCollection<string> Fields
        {
            get
            {
                if(this.fields == null)
                {
                    return new ReadOnlyCollection<string>(new List<string>());
                }

                return new ReadOnlyCollection<string>(this.fields.ToList());
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<CustomListUpdatePrecondition> Preconditions
        {
            get
            {
                if(this.preconditions == null)
                {
                    return new ReadOnlyCollection<CustomListUpdatePrecondition>(new List<CustomListUpdatePrecondition>());
                }

                return new ReadOnlyCollection<CustomListUpdatePrecondition>(this.preconditions.ToList());
            }
        }
    }
}
