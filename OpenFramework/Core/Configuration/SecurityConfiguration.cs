// -----------------------------------------------------------------------
// <copyright file="SecurityConfiguration.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace OpenFrameworkV3.Core.Configuration
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core.Enums;

    [Serializable]
    public class SecurityConfiguration
    {
        /// <summary>Gets or sets mode</summary>
        [JsonProperty("MinimumPasswordLength")]
        private int minimumPasswordLength;

        /// <summary>Gets or sets user name</summary>
        [JsonProperty("FailedAttempts")]
        private int failedAttempts;

        /// <summary>Gets or sets password account</summary>
        [JsonProperty("PasswordComplexity")]
        private string passwordComplexity { get; set; }

        /// <summary>Gets an empty instance of MailConfiguration class</summary>
        [JsonIgnore]
        public static SecurityConfiguration Empty
        {
            get
            {
                return new SecurityConfiguration
                {
                    minimumPasswordLength = 3,
                    failedAttempts = 3,
                    passwordComplexity = "Simple",
                    IPAccess = false,
                    MustChangePassword = false,
                    WriteAssumeDelete = false,
                    MFA = string.Empty
                };
            }
        }

        /// <summary>Gets a JSON definition of security configuration</summary>
        [JsonIgnore]
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"""Security"": {{
        ""IPAccess"": {0},
        ""IPAccessMail"": ""{1}"",
        ""PasswordComplexity"": ""{2}"",
        ""MustChangePassword"": {3},
        ""Traceability"": ""{4}"",
        ""GrantPermission"": ""{5}"",
        ""FailedAttempts"": {6},
        ""MinimumPasswordLength"": {7},
        ""GroupUserMain"": {8}
    }},",
                    ConstantValue.Value(this.IPAccess),
                    this.IPAccessMail,
                    this.PasswordComplexity.ToString(),
                    ConstantValue.Value(this.MustChangePassword),
                    this.Traceability.ToString(),
                    this.GrantPermission.ToString(),
                    this.FailedAttempts,
                    this.MinimumPasswordLength,
                    ConstantValue.Value(this.GroupUserMain));
            }
        }

        /// <summary>Gets or sets mail server</summary>
        [JsonProperty("GrantPermission")]
        public string GrantPermission { get; set; }

        /// <summary>Gets or sets mail server</summary>
        [JsonProperty("ScopeView")]
        public string ScopeView { get; set; }

        /// <summary>Gets or sets multiple factor access</summary>
        [JsonProperty("MFA")]
        public string MFA { get; set; }

        /// <summary>Gets or sets user account</summary>
        [JsonProperty("Traceability")]
        public string Traceability { get; set; }

        /// <summary>Write assume delete</summary>
        [JsonProperty("WriteAssumeDelete")]
        public bool WriteAssumeDelete { get; set; }

        /// <summary>Write assume delete</summary>
        [JsonProperty("GroupUserMain")]
        public bool GroupUserMain { get; set; }

        /// <summary>Gets the action in database for delete</summary>
        [JsonIgnore]
        public PasswordComplexity PasswordComplexity
        {
            get
            {
                if (string.IsNullOrEmpty(this.passwordComplexity))
                {
                    return PasswordComplexity.None;
                }

                switch (this.passwordComplexity.ToUpperInvariant())
                {
                    case "SIMPLE":
                        return PasswordComplexity.Simple;
                    case "STRONG":
                    default:
                        return PasswordComplexity.Strong;
                }
            }
        }

        /// <summary>Gets or sets a value indicating whether if user must change password after reset </summary>
        [JsonProperty("MustChangePassword")]
        public bool MustChangePassword { get; set; }

        /// <summary>Gets or sets a value indicating whether if IP access filter is enabled</summary>
        [JsonProperty("IPAccess")]
        public bool IPAccess { get; set; }

        /// <summary>Gets or sets a value indicating whether if IP access filter is enabled</summary>
        [JsonProperty("IPAccessMail")]
        public string IPAccessMail { get; set; }

        [JsonIgnore]
        public int MinimumPasswordLength
        {
            get
            {
                if (this.minimumPasswordLength == 0)
                {
                    return 6;
                }

                return this.minimumPasswordLength;
            }

            set
            {
                this.minimumPasswordLength = value;
            }
        }

        [JsonIgnore]
        public int FailedAttempts
        {
            get
            {
                if (this.failedAttempts == 0)
                {
                    return 3;
                }

                return this.failedAttempts;
            }

            set
            {
                this.failedAttempts = value;
            }
        }

        /// <summary>Sets password complexity</summary>
        /// <param name="value">Name of password complexity</param>
        public void SetPasswordComplexity(string value)
        {
            this.passwordComplexity = value;
        }
    }
}