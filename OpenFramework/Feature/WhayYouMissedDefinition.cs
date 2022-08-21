// --------------------------------
// <copyright file="WhayYouMissedConfig.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Feature
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core;
    using OpenFrameworkV3.Core.Security;
    using OpenFrameworkV3.Tools;

    public class WhayYouMissedDeninition
    {
        /// <summary>Gets or sets the index of field positions</summary>
        [JsonProperty("Groups")]
        private long[] groups { get; set; }

        /// <summary>Gets or sets de company identifier</summary>
        [JsonProperty("CompanyId")]
        public int CompanyId { get; set; }

        /// <summary>Gets or sets the item definition identifier</summary>
        [JsonProperty("ItemDefinitionId")]
        public int ItemDefinitionId { get; set; }

        /// <summary>Gets or sets definition's category</summary>
        [JsonProperty("Category")]
        public string Category { get; set; }

        /// <summary>Gets or sets the query to extract alert occurrences</summary>
        [JsonProperty("Query")]
        public string Query { get; set; }

        /// <summary>Gets or sets a value indicating whether notify is showed</summary>
        [JsonProperty("Notify")]
        public bool Notify { get; set; }

        [JsonIgnore]
        public ReadOnlyCollection<long> Groups
        {
            get
            {
                if(this.groups == null || this.groups.Count()== 0)
                {
                    return new ReadOnlyCollection<long>(new List<long>());
                }

                return new ReadOnlyCollection<long>(this.groups);
            }
        }

        /// <summary>Gets alert definition from disk</summary>
        /// <returns>Alert definition structure</returns>
        public static ReadOnlyCollection<WhayYouMissedDeninition> GetFromDisk(string instanceName)
        {
            var user = ApplicationUser.Actual;
            var res = new List<WhayYouMissedDeninition>();
            Basics.VerifyFolder(Instance.Path.Alerts(instanceName));
            var myFiles = Directory.GetFiles(Instance.Path.Alerts(instanceName), "*.wym", SearchOption.TopDirectoryOnly).ToList();

            foreach (string fileName in myFiles)
            {
                var definition = GetDefinitionByFile(user.CompanyId, fileName, user.Id);

                if (user.AdminUser)
                {
                    res.Add(definition);
                }
                else
                {
                    if (definition.Groups == null)
                    {
                        continue;
                    }

                    if (definition.Groups.Count == 0)
                    {
                        res.Add(definition);
                    }
                    else
                    {
                        foreach (var g in user.Groups)
                        {
                            if (definition.Groups.Any(gr=>gr == g.Id))
                            {
                                res.Add(definition);
                                break;
                            }
                        }
                    }
                }
            }

            return new ReadOnlyCollection<WhayYouMissedDeninition>(res);
        }
        
        /// <summary>Read alert definition from file</summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="fileName">File name of alert</param>
        /// <param name="userId">Identifier of actual user</param>
        /// <returns>AlertDefinition structure</returns>
        public static WhayYouMissedDeninition GetDefinitionByFile(long companyId, string fileName, long userId)
        {
            var alert = new WhayYouMissedDeninition();
            if (File.Exists(fileName))
            {
                using (var input = new StreamReader(fileName))
                {
                    alert = JsonConvert.DeserializeObject<WhayYouMissedDeninition>(input.ReadToEnd());
                    alert.Query = alert.Query.Replace("#CompanyId#", companyId.ToString(CultureInfo.InvariantCulture));
                    alert.Query = alert.Query.Replace("#ActualUser#", userId.ToString());
                    if (alert.CompanyId != 0 && alert.CompanyId != companyId)
                    {
                        return new WhayYouMissedDeninition();
                    }
                }
            }

            return alert;
        }
    }
}