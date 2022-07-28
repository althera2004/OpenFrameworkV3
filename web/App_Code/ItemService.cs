// --------------------------------
// <copyright file="ItemService.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using OpenFrameworkV3;
using OpenFrameworkV3.Core.Activity;
using OpenFrameworkV3.Core.DataAccess;
using OpenFrameworkV3.Core.ItemManager;
using OpenFrameworkV3.Core.ItemManager.ItemList;
using OpenFrameworkV3.Core.Security;
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
    public ActionResult ItemBarSave(string itemName, long id, string description, long applicationUserId, long companyId, string instanceName)
    {
        return OpenFrameworkV3.Core.DataAccess.Save.SaveBarItem(itemName, id, description, applicationUserId, companyId, instanceName);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult ItemBarDelete(string itemName, long id, long applicationUserId, string instanceName)
    {
        return OpenFrameworkV3.Core.DataAccess.Save.SaveBarDelete(itemName, id, applicationUserId, instanceName);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetListCustomAjaxSource(string itemName, string listDefinitionId, string parametersList, long companyId, string instanceName)
    {
        var res = string.Empty;
        var instance = Persistence.InstanceByName(instanceName);
        var itemDefinition = instance.ItemDefinitions.First(d => d.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        var listDefinition = itemDefinition.Lists[0];
        if (itemDefinition.Lists.Any(l => l.Id.Equals(listDefinitionId, StringComparison.OrdinalIgnoreCase) == true))
        {
            listDefinition = itemDefinition.Lists.First(l => l.Id.Equals(listDefinitionId, StringComparison.OrdinalIgnoreCase) == true);
        }

        if(!string.IsNullOrEmpty(listDefinition.CustomAjaxSource))
        {
            using(var cmd = new SqlCommand(listDefinition.CustomAjaxSource))
            {
                using(var cnn = new SqlConnection(instance.Config.ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));

                    var parameters = JsonConvert.DeserializeObject<ListParameter[]>(parametersList);

                    foreach (var parameter in parameters)
                    {
                        switch ((parameter.Type ?? string.Empty).ToUpperInvariant())
                        {
                            case "LONG":
                                cmd.Parameters.Add(DataParameter.Input(parameter.Name, Convert.ToInt64(parameter.Value)));
                                break;
                            case "INT":
                                cmd.Parameters.Add(DataParameter.Input(parameter.Name, Convert.ToInt32(parameter.Value)));
                                break;
                            default:
                                cmd.Parameters.Add(DataParameter.Input(parameter.Name, parameter.Value));
                                break;
                        }
                    }                    

                    var data = SqlStream.SQLJSONStream(cmd);

                    res = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{{""data"":{0},""ItemName"":""{1}"",""ListId"":""{2}""}}",
                        data,
                        itemName,
                        listDefinitionId);
                }
            }
        }
        else
        {
            GetList(itemName, listDefinitionId, parametersList, companyId, instanceName);
        }


        return res;
    }

   [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetList(string itemName, string listDefinitionId, string parametersList,long companyId, string instanceName)
    {
        var instance = Persistence.InstanceByName(instanceName);
        var itemBuilder = instance.ItemDefinitions.First(d => d.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        var res = string.Empty;
        try
        {

            //switch(itemBuilder.DataOrigin)
            //{
            //    case DataOrigin.JsonFile:
            //        res = GetListJsonFile(itemName, listDefinitionId, parametersList, instanceName);
            //        break;
            //    default:
            res = GetListSQL(itemName, listDefinitionId, parametersList, companyId, instanceName);
            //        break;
            //}
        }
        catch(Exception ex)
        {
            res = string.Format(CultureInfo.InvariantCulture, @"{{""ItemName"":""{1}"", ""ListId"":""{2}"", ""data"":""{0}""}}", Json.JsonCompliant(ex.Message), itemName, listDefinitionId);
        }

        return res.Replace("\n", "<br />").Replace("\r", string.Empty);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult CreatePersistenceScripts(string instanceName)
    {
        var res = ActionResult.NoAction;
        var itemDefinitions = Persistence.ItemDefinitions(instanceName).Where(i => i.Features.Persistence == true);
        foreach(var itemDefinition in itemDefinitions)
        {
            itemDefinition.CreatePersistenceScript(instanceName);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult Inactivate (string itemName, long itemId, long applicationUserId,long companyId, string instanceName)
    {
        return OpenFrameworkV3.Core.DataAccess.Save.Inactivate(itemName, itemId, companyId, applicationUserId, instanceName);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult GetTraces(string itemName, long itemId, string instanceName)
    {
        var res = ActionResult.NoAction;
        try
        {
            var data = ActionLog.TraceItemDataGet(instanceName, itemName, itemId);
            res.SetSuccess(data);
        }
        catch(Exception ex)
        {
            res.SetFail(ex);
        }

        return res;
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult Save(long itemDefinitionId, long itemId,string itemData, long applicationUserId, long companyId, string instanceName)
    {
        var res = ActionResult.NoAction;
        var serializer = new JavaScriptSerializer();
        serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

        var user = ApplicationUser.ById(applicationUserId, instanceName);

        var itemDefinition = Persistence.ItemDefinitionById(itemDefinitionId, instanceName);

        var dataToSave = new ItemData();
        var dataOriginal = new ItemData();
        var sqlData = string.Empty;
        var fields = string.Empty;
        var values = string.Empty;

        dynamic obj = serializer.Deserialize(itemData, typeof(object));
        foreach(var data in obj)
        {
            var fieldName = data["Field"];
            var original = data["Original"];
            var actual = data["Actual"];

            dataOriginal.Add(new KeyValuePair<string, object>(fieldName, original));
            dataToSave.Add(new KeyValuePair<string, object>(fieldName, actual));

            var field = itemDefinition.Fields.First(f => f.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
            sqlData += ", " + fieldName + " = " + SqlValue.Value(field, actual);

            fields += "," + fieldName;
            values += "," + SqlValue.Value(field, actual);
        }

        if (itemId > 0)
        {
            var query = string.Format(
                CultureInfo.InvariantCulture,
                "UPDATE Item_{0} SET CompanyId={4}, ModifiedBy={2}, ModifiedOn= GETUTCDATE() {3} WHERE Id = {1}",
                itemDefinition.ItemName,
                itemId,
                applicationUserId,
                sqlData,
                companyId);

            var cmd = new ExecuteQuery
            {
                ConnectionString = Persistence.ConnectionString(instanceName),
                QueryText = query
            };

            res = cmd.ExecuteCommand;
            if (res.Success)
            {
                res.SetSuccess(string.Format(CultureInfo.InvariantCulture,"UPDATE|{0}", itemId));
                if (res.Success)
                {
                    var trace = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{4},{{{4}{3}{3}""user"":""{0}"",{4}{3}{3}""date"": ""{1:dd/MM/yyyy hh:mm:ss}"",{4}{3}{3}""changes"":{2}}}",
                        user.Profile.FullName,
                        DateTime.UtcNow,
                        itemData,
                        '\t',
                        '\n');
                    ActionLog.TraceItemData(instanceName, itemDefinition.ItemName, itemId, trace);
                }
            }
        }
        else
        {
            var query = string.Format(
               CultureInfo.InvariantCulture,
               "INSERT INTO Item_{0} (CompanyId, Active, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn {3}) VALUES ({5}, 1,{2},GETUTCDATE(),{2},GETUTCDATE(){4}); SELECT SCOPE_IDENTITY();",
               itemDefinition.ItemName,
               itemId,
               applicationUserId,
               fields,
               values,
               companyId);

            using (var cmd = new SqlCommand(query))
            {
                using (var cnn = new SqlConnection(Persistence.ConnectionString(instanceName)))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    try
                    {
                        cmd.Connection.Open();
                        var id = Convert.ToInt64(cmd.ExecuteScalar());
                        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, "INSERT|{0}", id));
                        if (res.Success)
                        {
                            var trace = string.Format(
                                CultureInfo.InvariantCulture,
                                @"{4},{{{4}{3}{3}""user"":""{0}"",{4}{3}{3}""date"": ""{1:dd/MM/yyyy hh:mm:ss}"",{4}{3}{3}""changes"":{2}}}",
                                user.Profile.FullName,
                                DateTime.UtcNow,
                                itemData,
                                '\t',
                                '\n');
                            ActionLog.TraceItemData(instanceName, itemDefinition.ItemName, id, trace);
                        }
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
                    }
                    finally
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            };
        }

        
        
        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetFK(string itemName, string token, long companyId, string instanceName)
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
                data = Read.GetCustomFK(itemName, instanceName);
            }
            else
            {
                data = Read.JsonActive(itemName, companyId, instanceName);
            }
        }

        return string.Format(
            CultureInfo.InvariantCulture,
            @"{{""Token"":""{0}"",""Data"":{1},""ItemName"":""{2}""}}",
            token,
            data,
            itemName);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetFKApplicationUsers(long companyId, string instanceName)
    {
        var users = ApplicationUser.All(companyId, instanceName);
        var res = new StringBuilder("[");
        var first = true;
        foreach(var user in users)
        {
            res.AppendFormat(
                CultureInfo.InvariantCulture,
                "{0}{1}",
                first ? string.Empty : ",",
                user.JsonKeyValue);
            first = false;
        }      

        res.Append("]");
        return res.ToString();
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