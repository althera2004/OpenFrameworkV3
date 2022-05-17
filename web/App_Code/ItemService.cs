using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using OpenFrameworkV3;
using OpenFrameworkV3.Core.DataAccess;
using OpenFrameworkV3.Tools;

/// <summary>
/// Descripción breve de ItemService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class ItemService : WebService
{

    public ItemService()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetList(string itemName, string listDefinitionId, string parametersList,long companyId, string instanceName)
    {
        var instance = Persistence.InstanceByName(instanceName);
        var itemBuilder = instance.ItemDefinitions.First(d => d.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        var res = string.Empty;
        //switch(itemBuilder.DataOrigin)
        //{
        //    case DataOrigin.JsonFile:
        //        res = GetListJsonFile(itemName, listDefinitionId, parametersList, instanceName);
        //        break;
        //    default:
                res = GetListSQL(itemName, listDefinitionId, parametersList,companyId, instanceName);
        //        break;
        //}

        return res.Replace("\n", "<br />").Replace("\r", string.Empty);
    }



    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetFK(string itemName, string token,long companyId, string instanceName)
    {
        var data = Json.EmptyJsonList;
        if (itemName.Equals("Core_User", StringComparison.OrdinalIgnoreCase))
        {
            //data = ApplicationUser.FKList;
        }
        else
        {
            var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase) == true);
            if (!string.IsNullOrEmpty(itemDefinition.CustomFK))
            {
                data = Read.GetCustomFK(itemDefinition, instanceName);
            }
            else
            {
                data = Read.JsonActive(itemDefinition, companyId, instanceName);
            }
        }

        return string.Format(
            CultureInfo.InvariantCulture,
            @"{{""Token"":""{0}"",""Data"":{1},""ItemName"":""{2}""}}",
            token,
            data,
            itemName);
    }

    private string GetListJsonFile(string itemName, string listDefinitionId, string parametersList, string instanceName)
    {
        var data = new StringBuilder("[");
        var dataItem = Read.All<string>(itemName, instanceName);

        bool first = true;
        foreach(var item in dataItem)
        {
            if(first)
            {
                first = false;
            }
            else
            {
                data.Append(",");
            }

            data.Append(item);
        }

        data.Append("]");
        var res = string.Format(
            CultureInfo.InvariantCulture,
            @"{{""data"":{0}, ""ItemName"":""{1}"", ""ListId"":""{2}""}}",
            data,
            itemName,
            listDefinitionId);

        return res.ToString();
    }

    private string GetListSQL(string itemName, string listDefinitionId, string parametersList,long companyId, string instanceName)
    {
        var instance = Persistence.InstanceByName(instanceName);
        var definition = instance.ItemDefinitions.First(d => d.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        if(definition == null)
        {
            return Json.EmptyJsonList;
        }

        var listDefinition = definition.Lists.FirstOrDefault(l => l.Id.Equals(listDefinitionId, StringComparison.OrdinalIgnoreCase));
        if(listDefinition == null)
        {
            listDefinition = List.Default(definition);
        }

        var parameters = new Dictionary<string, string>();
        foreach(var parameter in parametersList.Split('|'))
        {
            if(!string.IsNullOrEmpty(parameter))
            {
                parameters.Add(parameter.Split('^')[0], parameter.Split('^')[1]);
            }
        }

        if(listDefinition.Parameters != null && listDefinition.Parameters.Count > 0)
        {
            foreach(var listParameter in listDefinition.Parameters)
            {
                if(!parameters.Any(p => p.Key.Equals(listParameter.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    parameters.Add(listParameter.Name, listParameter.Value);
                }
            }
        }

        definition.InstanceName = instanceName;
        var query = Query.ByList(definition, parameters, listDefinition, companyId);
        var res = string.Format(
            CultureInfo.InvariantCulture,
            @"{{""data"":{0}, ""ItemName"":""{1}"", ""ListId"":""{2}""}}",
            SqlStream.ByQuery(query, instance.Config.ConnectionString),
            itemName,
            listDefinitionId);
        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string ItemById(string itemName, long itemId, string instanceName)
    {
        using (var instance = Persistence.InstanceByName(instanceName))
        {
            if (instance.ItemDefinitions.Any(d => d.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase)))
            {
                var definition = instance.ItemDefinitions.First(d => d.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                return Read.JsonById(itemId, definition, instanceName);
            }
            else
            {
                return Json.EmptyJsonObject;
            }
        }
    }
}