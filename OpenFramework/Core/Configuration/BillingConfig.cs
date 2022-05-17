namespace OpenFrameworkV3.Core.Configuration
{
    using Newtonsoft.Json;

    public class BillingConfig
    {
        /// <summary>Gets or sets mode</summary>
        [JsonIgnore]
        public bool Active { get; set; }

        public static BillingConfig Empty
        {
            get
            {
                return new BillingConfig();
            }
        }
    }
}
