

namespace OpenFramework.Communications
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Communications;
    using OpenFrameworkV3.Core.Activity;

    [Serializable]
    public class Configuration
    {
        [JsonProperty("ItemDefinitionId")]
        public long ItemDefinitionId { get; set; }

        [JsonIgnore]
        public string ItemName { get; set; }

        [JsonProperty("CombineSources")]
        public bool CombineSources { get; set; }

        [JsonProperty("Sources")]
        private readonly Source[] sources;

        [JsonIgnore]
        public ReadOnlyCollection<Source> Sources
        {
            get
            {
                var res = new List<Source>();
                if (this.sources != null)
                {
                    foreach (var source in this.sources)
                    {
                        res.Add(source);
                    }
                }

                return new ReadOnlyCollection<Source>(res);
            }
        }

        [JsonIgnore]
        public string Json
        {
            get
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        public static ReadOnlyCollection<Configuration> Load(string instanceName)
        {
            var res = new List<Configuration>();
            var path = string.Format(
                CultureInfo.InvariantCulture,
                ConfigurationManager.AppSettings["InstancesFolder"].ToString(),
                instanceName);

            var fileName = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}Communications.config",
                path);

            string jsonDefinition = string.Empty;
            try
            {
                if (File.Exists(fileName))
                {
                    using (var input = new StreamReader(fileName))
                    {
                        jsonDefinition = input.ReadToEnd();
                    }

                    res = JsonConvert.DeserializeObject<List<Configuration>>(jsonDefinition);
                }
            }
            catch (IOException ex)
            {
                ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "IOException -- ItemDefinition::Load({0})", fileName));
            }
            catch (NullReferenceException ex)
            {
                ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "NullReferenceException -- ItemDefinition::Load({0})", fileName));
            }
            catch (JsonException ex)
            {
                ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "JsonException -- ItemDefinition::Load({0})", fileName));
            }
            catch (NotSupportedException ex)
            {
                ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "NotSupportedException -- ItemDefinition::Load({0})", fileName));
            }

            return new ReadOnlyCollection<Configuration>(res);
        }
    }
}