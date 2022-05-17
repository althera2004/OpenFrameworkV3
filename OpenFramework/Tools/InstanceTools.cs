// --------------------------------
// <copyright file="Tools.InstanceTools.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Tools
{
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Text;

    /// <summary>Framework tools</summary>
    public static class InstanceTools
    {
        public static string Json(string instanceName)
        {
            if(string.IsNullOrEmpty(instanceName))
            {
                return Constant.JavaScriptNull;
            }

            using (var instance = Persistence.InstanceByName(instanceName))
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
    ""ConnectionString"": ""{4}"",
    ""ConnectionStringPRE"": ""{5}"",
    ""Id"": {6},
    ""Name"": ""{7}"",
    ""Description"": ""{8}"",
    ""DeleteAction"": ""{9}"",
    ""SessionTimeout"": ""15m"",
    ""MultiCompany"": ""true"",
    ""MultiCompanyItem"": ""Actividad"",
    ""MQTT"": {24},
    ""UserSchedule"": {25},
    ""ListNumRows"": 50,
    ""FAQS"": {13},
    ""DefaultLanguage"": ""{14}"",
    ""GoogleAPIKey"": ""{15}"",
    ""ActiveAlerts"": ""{16}"",
    ""SaveAction"": {17},
    ""FollowEnabled"": {19},
    ""Portal"": {20},
    ""CodedQueryClean"":{32},
    ""PinCode"": {21},
    ""TraceType"": {22},
    ""PublicAccess"": {23},
    {1}
    {2}
    {31}
    {33}
    ""WhatYouMissed"":{27},
    ""Facturacion"":{28},
    ""Messaging"":{29},
    ""Sticking"":{30},
    ""ExternalUsers"": [{3}],
    ""Alerts"": [{26}]
}}",
                    "", //weke instance.Config.Mail.Json,
                    instance.Config.Security.Json,
                    instance.Config.Profile.Json,
                    JsonExternalUsers(instance.Config.ExternalUsers),
                    instance.Config.ConnectionString,
                    instance.Config.ConnectionString,
                    instance.Config.Id,
                    instance.Config.Name,
                    instance.Config.Description,
                    instance.Config.DeleteAction,
                    instance.Config.SessionTimeout,
                    instance.Config.MultiCompany,
                    instance.Config.MultipleCompanyItem,
                    ConstantValue.Value(instance.Config.FAQs),
                    instance.Config.DefaultLanguage,
                    instance.Config.GoogleAPIKey,
                    ConstantValue.Value(instance.Config.ActiveAlerts),
                    (int)instance.Config.SaveAction,
                    instance.Config.ListNumRows,
                    ConstantValue.Value(instance.Config.FollowEnabled),
                    ConstantValue.Value(instance.Config.Portal),
                    ConstantValue.Value(instance.Config.PinCode),
                    instance.Config.TraceType,
                    ConstantValue.Value(instance.Config.PublicAccess),
                    ConstantValue.Value(instance.Config.MQTT),
                    ConstantValue.Value(instance.Config.UserSchedule),
                    instance.Config.AlertsJson,
                    ConstantValue.Value(instance.Config.WhatYouMissed),
                    ConstantValue.Value(instance.Config.Facturacion),
                    ConstantValue.Value(instance.Config.Messaging),
                    ConstantValue.Value(instance.Config.Sticking),
                    instance.Config.CompanyProfile.Json + ",",
                    ConstantValue.Value(instance.Config.CodedQueryClean),
                    instance.Config.Features.Json);
            }
        }

        public static string JsonExternalUsers(ReadOnlyCollection<string> externalUsers)
        {
            var res = new StringBuilder();
            if (externalUsers != null && externalUsers.Count > 0)
            {
                bool first = true;
                foreach (string user in externalUsers)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(", ");
                    }

                    res.AppendFormat(CultureInfo.InvariantCulture, @"""{0}""", user);
                }
            }

            return res.ToString();
        }
    }
}