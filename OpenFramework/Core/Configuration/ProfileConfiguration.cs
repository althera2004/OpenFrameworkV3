namespace OpenFrameworkV3.Core.Configuration
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    [Serializable]
    public class ProfileConfiguration
    {
        public const int NameFormatComplete = 0;
        public const int NameFormatFirstNameLastName = 1;
        public const int NameFormatFirstName2LastName = 2;

        /// <summary>Gets an empty instance of ProfileConfiguration class</summary>
        [JsonIgnore]
        public static ProfileConfiguration Empty
        {
            get
            {
                return new ProfileConfiguration
                {
                    NameFormat = 0
                };
            }
        }

        /// <summary>Gets a JSON definition of profile configuration</summary>
        [JsonIgnore]
        public string Json
        {
            get
            {

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"""Profile"": {{
        ""Twiter"": {0},
        ""LinkedIn"": {1},
        ""Facebook"": {2},
        ""Instagram"": {3},
        ""BirthDate"": {4},
        ""Mobile"": {5},
        ""IdentificationCard"": {6},
        ""NameFormat"": {7},
        ""Gender"": {8},
        ""PhoneEmergency"": {9},
        ""EmailAlternative"": {10},
        ""Web"": {11},
        ""Nacionality"": {12},
        ""Fax"": {13},
        ""Address"": {14},
        ""Signature"": {15},
        ""PreferencesEnabled"": {16},
        ""ActivityEnabled"": {17},
        ""FollowEnabled"": {18}
    }},",

                    ConstantValue.Value(this.Twitter),
                    ConstantValue.Value(this.LinkedIn),
                    ConstantValue.Value(this.Facebook),
                    ConstantValue.Value(this.Instagram),
                    ConstantValue.Value(this.BirthDate),
                    ConstantValue.Value(this.Mobile),
                    ConstantValue.Value(this.IdentificationCard),
                    this.NameFormat,
                    ConstantValue.Value(this.Gender),
                    ConstantValue.Value(this.PhoneEmergency),
                    ConstantValue.Value(this.EmailAlternative),
                    ConstantValue.Value(this.Web),
                    ConstantValue.Value(this.Nationality),
                    ConstantValue.Value(this.Fax),
                    ConstantValue.Value(this.Address),
                    ConstantValue.Value(this.Signature),
                    ConstantValue.Value(this.PreferencesEnabled),
                    ConstantValue.Value(this.ActivityEnabled),
                    ConstantValue.Value(this.FollowEnabled));
            }
        }

        /// <summary>Gets or sets a value indicating whether if profile has Twitter</summary>
        [JsonProperty("Twiter")]
        public bool Twitter { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has mail LinkedIn</summary>
        [JsonProperty("LinkedIn")]
        public bool LinkedIn { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has user Facebook</summary>
        [JsonProperty("Facebook")]
        public bool Facebook { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has password Instagram</summary>
        [JsonProperty("Instagram")]
        public bool Instagram { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has user name</summary>
        [JsonProperty("Phone")]
        public bool Phone { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has fax</summary>
        [JsonProperty("Fax")]
        public bool Fax { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has mail address</summary>
        [JsonProperty("Mobile")]
        public bool Mobile { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has mail server port</summary>
        [JsonProperty("PhoneEmergency")]
        public bool PhoneEmergency { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has gender</summary>
        [JsonProperty("Gender")]
        public bool Gender { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has birthdate</summary>
        [JsonProperty("BirthDate")]
        public bool BirthDate { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has postal address</summary>
        [JsonProperty("Address")]
        public bool Address { get; set; }

        /// <summary>Gets or sets format of name</summary>
        [JsonProperty("NameFormat")]
        public int NameFormat { get; set; }

        /// <summary>Gets or sets a value indicating whether if preferences profile tab is enabled</summary>
        [JsonProperty("PreferencesEnabled")]
        public bool PreferencesEnabled { get; set; }

        /// <summary>Gets or sets a value indicating whether if activity profile tab is enabled</summary>
        [JsonProperty("ActivityEnabled")]
        public bool ActivityEnabled { get; set; }

        /// <summary>Gets or sets a value indicating whether if following profile tab is enabled</summary>
        [JsonProperty("FollowEnabled")]
        public bool FollowEnabled { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has the number of identification card</summary>
        public bool IdentificationCard { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has alternative email address</summary>
        public bool EmailAlternative { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has personal web url</summary>
        public bool Web { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile has signature</summary>
        public bool Signature { get; set; }

        /// <summary>Gets or sets a value indicating whether if profile contains nacionality</summary>
        public bool Nationality { get; set; }

        /// <summary>Gets or sets the label of extra data text</summary>
        [JsonProperty("DataText1")]
        public string DataText1 { get; set; }

        /// <summary>Gets or sets the label of extra data text</summary>
        [JsonProperty("DataText2")]
        public string DataText2 { get; set; }

        /// <summary>Gets or sets the label of extra data text</summary>
        [JsonProperty("DataText3")]
        public string DataText3 { get; set; }

        /// <summary>Gets or sets the label of extra data text</summary>
        [JsonProperty("DataText4")]
        public string DataText4 { get; set; }
    }
}