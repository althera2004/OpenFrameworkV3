// -----------------------------------------------------------------------
// <copyright file="MailConfiguration.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace OpenFrameworkV3.Core.Configuration
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    /// <summary>Customer mail configuration</summary>
    [Serializable]
    public sealed class MailConfiguration
    {
        /// <summary>Gets an empty instance of MailConfiguration class</summary>
        [JsonIgnore]
        public static MailConfiguration Empty
        {
            get
            {
                return new MailConfiguration
                {
                    Mode = string.Empty,
                    Server = string.Empty,
                    User = string.Empty,
                    Password = string.Empty,
                    UserName = string.Empty,
                    MailSender = string.Empty,
                    Port = 0
                };
            }
        }

        /// <summary>Gets a JSON definition of mail configuration</summary>
        [JsonIgnore]
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"""Mail"": {{
        ""Mode"": ""{0}"",
        ""Server"": ""{1}"",
        ""User"": ""{2}"",
        ""Password"": ""{3}"",
        ""Port"": {4},
        ""UserName"": ""{5}"",
        ""MailSender"": ""{6}""
    }},",
                    this.Mode.ToString(),
                    this.Server,
                    this.User,
                    this.Password,
                    this.Port,
                    this.UserName,
                    this.MailSender);
            }
        }

        /// <summary>Gets or sets mode</summary>
        [JsonProperty("Mode")]
        public string Mode { get; set; }

        /// <summary>Gets or sets mail server</summary>
        [JsonProperty("Server")]
        public string Server { get; set; }

        /// <summary>Gets or sets user account</summary>
        [JsonProperty("User")]
        public string User { get; set; }

        /// <summary>Gets or sets password account</summary>
        [JsonProperty("Password")]
        public string Password { get; set; }

        /// <summary>Gets or sets user name</summary>
        [JsonProperty("UserName")]
        public string UserName { get; set; }

        /// <summary>Gets or sets mail address</summary>
        [JsonProperty("MailSender")]
        public string MailSender { get; set; }

        /// <summary>Gets or sets mail server port</summary>
        [JsonProperty("Port")]
        public int Port { get; set; }
    }
}