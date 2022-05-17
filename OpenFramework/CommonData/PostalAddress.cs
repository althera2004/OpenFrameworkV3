namespace OpenFrameworkV3.CommonData
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public class PostalAddress
    {
        [XmlElement(Type = typeof(long), ElementName = "WayTypeId")]
        public long WayTypeId { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "WayType")]
        public string WayType { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Address")]
        public string Address { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "PostalCodeId")]
        public long PostalCodeId { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "PostalCode")]
        public string PostalCode { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "City")]
        public string City { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Province")]
        public string Province { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "State")]
        public string State { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Country")]
        public string Country { get; set; }

        [XmlElement(Type = typeof(decimal?), ElementName = "Latitude")]
        public decimal? Latitude { get; set; }

        [XmlElement(Type = typeof(decimal?), ElementName = "Longitude")]
        public decimal? Longitude { get; set; }

        [XmlElement(Type = typeof(bool), ElementName = "Active")]
        public bool Active { get; set; }

        public static PostalAddress Empty
        {
            get
            {
                return new PostalAddress
                {
                    WayTypeId = Constant.DefaultId,
                    WayType = string.Empty,
                    Address = string.Empty,
                    PostalCodeId = Constant.DefaultId,
                    PostalCode = string.Empty,
                    City = string.Empty,
                    Province = string.Empty,
                    State = string.Empty,
                    Country = string.Empty,
                    Latitude = null,
                    Longitude = null
                };
            }
        }

        /// <summary>Get fulls streest address text</summary>
        public string FullStreetAddress
        {
            get
            {
                var res = string.Empty;

                if (!string.IsNullOrEmpty(this.WayType))
                {
                    res += this.WayType + " ";
                }

                res += this.Address;
                return res;
            }
        }

        /// <summary> Gets full address text</summary>
        public string FullAddress
        {
            get
            {
                var res = this.FullStreetAddress;

                if (!string.IsNullOrEmpty(this.PostalCode))
                {
                    res += ", " + PostalCode;
                }

                if (!string.IsNullOrEmpty(this.Province))
                {
                    if (string.IsNullOrEmpty(this.PostalCode))
                    {
                        res += ", " + PostalCode;
                    }
                    else
                    {
                        res += " - " + Province;
                    }
                }

                return res;
            }
        }
    }
}