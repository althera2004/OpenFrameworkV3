// --------------------------------
// <copyright file="AccessServicePoint.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.Security
{
    using OpenFramework.Tools;
    using System;
    using System.Globalization;

    public class AccessServicePoint
    {
        public string InstanceName { get; set; }
        public string TargetType { get; set; }
        public string Email { get; set; }
        public string ListId { get; set; }
        public string FormId { get; set; }
        public string ItemDefinitionId { get; set; }
        public long ItemId { get; set; }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{ ""TargetType"": ""{0}"", ""ListId"":""{1}"", ""FormId"":""{2}"", ""ItemDefinitionId"":""{3}"", ""ItemId"":""{4}"", ""Email"":""{5}"",}}",
                    this.TargetType,
                    this.ListId,
                    this.FormId,
                    this.ItemDefinitionId,
                    this.ItemId,
                    this.Email);
            }
        }

        public string LandPage
        {
            get
            {
                string res = string.Empty;
                if (TargetType.Equals("L", StringComparison.OrdinalIgnoreCase))
                {
                    var t = string.Format(
                        CultureInfo.InvariantCulture,
                        "itemTypeId={0}&listid={1}{3}&ac={2}",
                        this.ItemDefinitionId,
                        this.ListId,
                        Guid.NewGuid(),
                        string.IsNullOrEmpty(this.Email) ? string.Empty : ("&email=" + this.Email));
                    var encodedQuery = Basics.Base64Encode(t);
                    res = "/ItemList.aspx?" + encodedQuery + "=";
                }

                if (TargetType.Equals("P", StringComparison.OrdinalIgnoreCase))
                {
                    var t = string.Format(
                        CultureInfo.InvariantCulture,
                        "&itemTypeId={0}&itemId={1}{3}&formId={2}",
                        this.ItemDefinitionId,
                        this.ItemId,
                        this.FormId,
                        string.IsNullOrEmpty(this.Email) ? string.Empty : ("&email=" + this.Email));
                    var encodedQuery = Basics.Base64Encode(t);
                    res = "/PageView.aspx?" + encodedQuery + "=";
                }

                return res;
            }
        }

        /// <summary>
        /// Initializes a new instance of the AccessServicePoint class.
        /// </summary>
        /// <param name="accessPointQuery">Query for access</param>
        public AccessServicePoint(string accessPointQuery)
        {
            this.TargetType = accessPointQuery.Substring(0, 1).ToUpperInvariant();
            var query = accessPointQuery.Substring(1, accessPointQuery.Length - 1);

            var CodedQuery = new CodedQuery();
            CodedQuery.SetQuery(query);

            this.ItemId = CodedQuery.GetByKey<long>("itemId");
            this.FormId = CodedQuery.GetByKey<string>("formId");
            this.ListId = CodedQuery.GetByKey<string>("listId");
            this.Email = CodedQuery.GetByKey<string>("email");
            this.ItemDefinitionId = CodedQuery.GetByKey<string>("itemTypeId");
        }
    }
}
