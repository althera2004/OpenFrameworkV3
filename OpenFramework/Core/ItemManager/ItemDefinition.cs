// --------------------------------
// <copyright file="ItemDefinition.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.ItemManager.ItemForm;
    using OpenFrameworkV3.Tools;
    using L = OpenFrameworkV3.Core.ItemManager.ItemList;

    /// <summary>Implements item definition</summary>
    [Serializable]
    public sealed class ItemDefinition
    {
        [JsonIgnore]
        private static bool generateItemDefinitionInUse;

        /// <summary>Indicates if item is master</summary>
        [JsonProperty("MasterTable")]
        private readonly string masterTable;

        /// <summary>Foreign keys of item</summary>
        [JsonProperty("ForeignValues")]
        private ForeignList[] foreignValues;

        /// <summary>Indicates if item needs others foreign keys</summary>
        [JsonProperty("NeedFK")]
        private readonly string[] needFK;

        [JsonProperty("Removes")]
        private readonly string[] removes;

        //[JsonProperty("GeoLocation")]
        //public bool GeoLocation { get; private set; }

        /// <summary>Primary keys of item</summary>
        [JsonProperty("PrimaryKeys")]
        private readonly string[] primaryKeys;

        /// <summary>Gets or sets item features</summary>
        [JsonProperty("Features")]
        public ItemFeatures Features { get; set; }

        /// <summary>Parent item</summary>
        [JsonProperty("ParentItem")]
        public string ParentItem { get; private set; }

        [JsonProperty("InheritedItems")]
        private readonly List<string> inheritedItems;

        /// <summary>Gets the fields</summary>
        [JsonProperty("Fields")]
        private List<ItemField> fields;

        /// <summary>Gets the list</summary>
        [JsonProperty("Lists")]
        private List<L.List> lists;

        /// <summary>Gets the forms for the item</summary>
        [JsonProperty("Forms")]
        private List<Form> forms;

        /// <summary>Item data adapter for SQL server</summary>
        [JsonProperty("DataAdapter")]
        private readonly DataAdapter dataAdapter;

        [JsonIgnore]
        public string InstanceName { get; set; }

        /// <summary>Gets or sets the name of stored procedure for custom FK list purposses</summary>
        [JsonProperty("CustomFK")]
        public string CustomFK { get; set; }

        /// <summary>Gets an empty definition</summary>
        [JsonIgnore]
        public static ItemDefinition Empty
        {
            get
            {
                return new ItemDefinition
                {
                    fields = new List<ItemField>(),
                    foreignValues = null,
                    ItemName = string.Empty,
                    lists = new List<L.List>(),
                    Layout = ItemDefinitionLayout.Empty
                };
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<string> Removes
        {
            get
            {
                if (this.removes == null)
                {
                    return new ReadOnlyCollection<string>(new List<string>());
                }

                return new ReadOnlyCollection<string>(this.removes.ToList());
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<string> NeedFK
        {
            get
            {
                if (this.needFK == null)
                {
                    return new ReadOnlyCollection<string>(new List<string>());
                }

                return new ReadOnlyCollection<string>(this.needFK.ToList());
            }
        }

        /// <summary>Get or sets a value indicating whether item is in scope view</summary>
        public bool ScopeView { get; set; }

        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("DataOrigin")]
        public int DataOrigin { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("OnlyAdmin")]
        public bool OnlyAdmin { get; set; }

        /// <summary>Gets or sets geolocation configuration</summary>
        //[JsonProperty("Geolocation")]
        //public ItemGeolocation Geolocation { get; set; }

        /// <summary>Gets or sets the name of stored procedure for custom FK list purposses</summary>
        [JsonProperty("PersistentFK")]
        public string PersistentFK { get; set; }

        /// <summary>Gets or sets the JSON structure of item definition</summary>
        public string Json { get; set; }

        /// <summary>Gets or sets the item name</summary>
        public string ItemName { get; set; }

        /// <summary>Gets or sets a value indicating whether items has core values for multicompany instances</summary>
        public bool CoreValues { get; set; }

        /// <summary>Gets the lists to show items</summary>
        [JsonIgnore]
        public ReadOnlyCollection<L.List> Lists
        {
            get
            {
                if (this.lists == null)
                {
                    return new ReadOnlyCollection<L.List>(new List<L.List>());
                }

                return new ReadOnlyCollection<L.List>(this.lists);
            }
        }

        /// <summary>Gets the forms to edit items</summary>
        [JsonIgnore]
        public ReadOnlyCollection<Form> Forms
        {
            get
            {
                if (this.forms == null)
                {
                    return new ReadOnlyCollection<Form>(new List<Form>());
                }

                return new ReadOnlyCollection<Form>(this.forms);
            }
        }

        /// <summary>Gets or sets the layout of item definition</summary>
        [JsonProperty("Layout")]
        public ItemDefinitionLayout Layout { get; set; }

        [JsonIgnore]
        public ReadOnlyCollection<string> InheritedItems
        {
            get
            {
                if (this.inheritedItems == null)
                {
                    return new ReadOnlyCollection<string>(new List<string>());
                }

                if (this.inheritedItems.Count == 0)
                {
                    return new ReadOnlyCollection<string>(new List<string>());
                }

                return new ReadOnlyCollection<string>(this.inheritedItems);
            }
        }

        /// <summary>Gets the foreign values</summary>
        [JsonIgnore]
        public ReadOnlyCollection<ForeignList> ForeignValues
        {
            get
            {
                if (this.foreignValues == null)
                {
                    return new ReadOnlyCollection<ForeignList>(new List<ForeignList>());
                }

                return new ReadOnlyCollection<ForeignList>(this.foreignValues);
            }
        }

        /// <summary>Gets the primary keys</summary>
        [JsonIgnore]
        public ReadOnlyCollection<string> PrimaryKeys
        {
            get
            {
                var res = new List<string>();
                if (this.primaryKeys != null)
                {
                    foreach (var pk in this.primaryKeys)
                    {
                        res.Add(pk);
                    }
                }

                return new ReadOnlyCollection<string>(res);
            }
        }

        /// <summary>Gets the name of the foreign keys</summary>
        [JsonIgnore]
        public ReadOnlyCollection<string> ForeignListNames
        {
            get
            {
                var res = new List<string>();
                if (this.foreignValues != null)
                {
                    foreach (ForeignList foreigList in this.foreignValues)
                    {
                        if (!res.Contains(foreigList.ItemName))
                        {
                            res.Add(foreigList.ItemName);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(foreigList.RemoteItem) && !res.Contains(foreigList.RemoteItem))
                            {
                                res.Add(foreigList.RemoteItem);
                            }
                        }
                    }
                }

                return new ReadOnlyCollection<string>(res);
            }
        }

        /// <summary>Gets the item data adapter for SQL server</summary>
        [JsonIgnore]
        public DataAdapter DataAdapter
        {
            get
            {
                if (this.dataAdapter == null)
                {
                    return DataAdapter.Empty;
                }

                return this.dataAdapter;
            }
        }

        /// <summary>Gets the fields</summary>
        [JsonIgnore]
        public ReadOnlyCollection<ItemField> Fields
        {
            get
            {
                if (this.fields == null)
                {
                    return new ReadOnlyCollection<ItemField>(new List<ItemField>());
                }

                return new ReadOnlyCollection<ItemField>(this.fields);
            }
        }

        public ReadOnlyCollection<ForeignList> ImportReferences
        {
            get
            {
                var res = new List<ForeignList>();
                foreach (ForeignList fl in this.ForeignValues)
                {
                    if (!string.IsNullOrEmpty(fl.ImportReference))
                    {
                        if (!fl.ImportReference.Equals(fl.FieldRetrieved, StringComparison.OrdinalIgnoreCase))
                        {
                            res.Add(fl);
                        }
                    }
                }

                return new ReadOnlyCollection<ForeignList>(res);
            }
        }

        public static ItemDefinition ById(long id, string instanceName)
        {
            var instance = Persistence.InstanceByName(instanceName);
            if (instance.ItemDefinitions.Any(d => d.Id == id))
            {
                return instance.ItemDefinitions.First(d => d.Id == id);
            }

            return ItemDefinition.Empty;
        }

        /// <summary>Retrieves a form by identifier if exists</summary>
        /// <param name="id">Identifier of the form</param>
        /// <returns>Definition of a list</returns>
        public Form FormById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Form.Empty;
            }

            if (this.Forms.Any(f => f.Id.ToUpperInvariant() == id.ToUpperInvariant()))
            {
                return this.Forms.First(f => f.Id.ToUpperInvariant() == id.ToUpperInvariant());
            }

            return Form.Empty;
        }

        /// <summary>Retrieves a list by identifier if exists</summary>
        /// <param name="id">Identifier of the list</param>
        /// <returns>Definition of a list</returns>
        public L.List ListById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return L.List.Empty;
            }

            if (this.Lists.Any(f => f.Id.Equals(id, StringComparison.OrdinalIgnoreCase)))
            {
                return this.Lists.First(f => f.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            }

            return L.List.Empty;
        }

        /// <summary>Retrieves a field by identifier if exists</summary>
        /// <param name="id">Identifier of the list</param>
        /// <returns>Definition of a list</returns>
        public ItemField FieldById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return ItemField.Empty;
            }

            if (this.fields.Any(f => f.Name.Equals(id, StringComparison.OrdinalIgnoreCase)))
            {
                return this.fields.First(f => f.Name.Equals(id, StringComparison.OrdinalIgnoreCase));
            }

            return ItemField.Empty;
        }

        /*public static ItemDefinition Load(long itemTypeId, string instanceName)
        {
            if (HttpContext.Current.Session["ItemDefinitionIndex"] == null)
            {
                ItemDefinition.LoadSessionItems(instanceName);
            }

            var definitionIndex = HttpContext.Current.Session["ItemDefinitionIndex"] as List<ItemDefinition.ItemDefinitonIndex>;
            var itemName = definitionIndex.First(d => d.Id == itemTypeId).Name;
            return Load(itemName, instanceName);
        }*/

        public static ItemDefinition Load(string itemName)
        {
            using (var instance = Instance.Actual)
            {
                return Load(itemName, instance);
            }
        }

        public static ItemDefinition Load(string itemName, string instanceName)
        {
            using (var instance = Persistence.InstanceByName(instanceName))
            {
                return Load(itemName, instance);
            }
        }

        public static ItemDefinition ByName(string itemName, Instance instance)
        {
            return instance.ItemDefinitions.First(d => d.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>Load a item definition from file</summary>
        /// <param name="itemName">Item name</param>
        /// <param name="instance">Instance of session</param>
        /// <returns>Item definition for item</returns>
        public static string LoadJson(string itemName, string instanceName)
        {
            if (string.IsNullOrEmpty(itemName) || string.IsNullOrEmpty(instanceName))
            {
                return Tools.Json.EmptyJsonObject;
            }

            string path = Instance.Path.Definition(instanceName);
            string jsonDefinition = string.Empty;

            if (!File.Exists(path))
            {
                return null;
            }

            using (var input = new StreamReader(path))
            {
                jsonDefinition = input.ReadToEnd();
            }

            return jsonDefinition;
        }

        /// <summary>Load a item definition from file</summary>
        /// <param name="itemName">Item name</param>
        /// <param name="instance">Instance of session</param>
        /// <returns>Item definition for item</returns>
        public static ItemDefinition Load(string itemName, Instance instance)
        {
            if (string.IsNullOrEmpty(itemName) || instance == null)
            {
                return ItemDefinition.Empty;
            }

            string path = Instance.Path.Definition(instance.Name) + "\\" + itemName + ".item";
            string jsonDefinition = string.Empty;

            if (!File.Exists(path))
            {
                return null;
            }

            using (var input = new StreamReader(path))
            {
                jsonDefinition = input.ReadToEnd();
            }

            if (!string.IsNullOrEmpty(jsonDefinition))
            {
                var res = ItemDefinition.Empty;
                try
                {
                    res = JsonConvert.DeserializeObject<ItemDefinition>(jsonDefinition);
                    res.InstanceName = instance.Name;
                    foreach (var field in res.fields)
                    {
                        field.ItemName = itemName.ToUpperInvariant();
                    }

                    if (res.lists != null)
                    {
                        foreach (var list in res.lists)
                        {
                            list.ItemName = res.ItemName.ToUpperInvariant();
                        }
                    }

                    string path2 = HttpContext.Current.Request.PhysicalApplicationPath;
                    if (!path2.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                    {
                        path2 = string.Format(CultureInfo.InvariantCulture, @"{0}\", path2);
                    }

                    res.Json = jsonDefinition;
                    string templateFileName = string.Format(CultureInfo.InvariantCulture, @"{0}InstanceStructure\Template.js", path2);
                    string destinationFileName = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{0}Instances\{1}\Scripts\{2}.js",
                        path2,
                        instance.Name,
                        itemName);

                    if (!File.Exists(destinationFileName))
                    {
                        using (var input = new StreamReader(templateFileName))
                        {
                            var content = input.ReadToEnd().Replace("#ITEMNAME", itemName.ToUpperInvariant());
                            using (var output = new StreamWriter(destinationFileName))
                            {
                                output.Write(content);
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "IOException -- ItemDefinition::Load({0})", itemName));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "NullReferenceException -- ItemDefinition::Load({0})", itemName));
                }
                catch (JsonException ex)
                {
                    ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "JsonException -- ItemDefinition::Load({0})", itemName));
                }
                catch (NotSupportedException ex)
                {
                    ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "NotSupportedException -- ItemDefinition::Load({0})", itemName));
                }

                res.InstanceName = instance.Name;

                res.SetEmptyProperties();
                return res;
            }

            return null;
        }

        /// <summary>Load a item definition from file</summary>
        /// <param name="fileName">Item's name</param>
        /// <returns>Item definition for item</returns>
        /// 
        public static ItemDefinition LoadFromFile(string fileName, string instanceName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return ItemDefinition.Empty;
            }

            string jsonDefinition = string.Empty;
            using (var input = new StreamReader(fileName))
            {
                jsonDefinition = input.ReadToEnd();
            }

            // Extract item name from file name
            var path = fileName.Split('\\');
            string itemName = path[path.Length - 1].ToUpperInvariant().Replace(".ITEM", string.Empty);
            if (!string.IsNullOrEmpty(jsonDefinition))
            {
                var res = ItemDefinition.Empty;
                try
                {
                    res = JsonConvert.DeserializeObject<ItemDefinition>(jsonDefinition);
                    res.InstanceName = instanceName;
                    foreach (var field in res.fields)
                    {
                        field.ItemName = itemName;
                    }

                    res.Json = jsonDefinition;
                }
                catch (IOException ex)
                {
                    ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "IOException -- ItemDefinition::Load({0})", fileName));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "NullReferenceException -- ItemDefinition::Load({0})", fileName));
                }
                catch (JsonException ex)
                {
                    ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "JsonException -- ItemDefinition::Load({0})", fileName));
                }
                catch (NotSupportedException ex)
                {
                    ExceptionManager.Trace(ex as Exception, string.Format(CultureInfo.InvariantCulture, "NotSupportedException -- ItemDefinition::Load({0})", fileName));
                }

                res.SetEmptyProperties();
                return res;
            }

            return null;
        }

        /// <summary>Creates script file "ItemDefinition.js" on defintions filer</summary>
        /// <param name="instance">Instance of items</param>
        public static void GenerateDefinitionsScript(string instanceName)
        {
            var pathScript = string.Format(CultureInfo.InvariantCulture, @"{0}\ItemDefinition.js", Instance.Path.Definition(instanceName));
            bool first = true;
            var fileContent = new StringBuilder("");
            fileContent.AppendFormat(CultureInfo.InvariantCulture, "// Autogenerated by OpenFramework Core, do not manually modify, under your resposability.{0}", Environment.NewLine);
            fileContent.AppendFormat(CultureInfo.InvariantCulture, @"// Instance: {0} - {1:dd/MM/yyyy hh:mm:ss}{2}", instanceName, DateTime.Now, Environment.NewLine);
            fileContent.Append("var ItemDefinitions = [").Append(Environment.NewLine);

            using (var instance = Persistence.InstanceByName(instanceName))
            {
                foreach (var definition in instance.ItemDefinitions)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        fileContent.AppendFormat(CultureInfo.InvariantCulture, ",{0}\t", Environment.NewLine);
                    }

                    fileContent.Append(definition.Json);
                }
            }

            fileContent.Append("];");

            if (!generateItemDefinitionInUse)
            {
                generateItemDefinitionInUse = true;
                try
                {
                    using (var output = new StreamWriter(pathScript, false))
                    {
                        output.Write(fileContent);
                    }
                }
                catch (IOException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.InvariantCulture, "ItemDefinition --> GenerateDefinitionsScript({0})", instanceName));
                }
                finally
                {
                    generateItemDefinitionInUse = false;
                }
            }
        }

        /// <summary>Sets default values for non requierd properties without value</summary>
        public void SetEmptyProperties()
        {
            // Fix empty properties
            if (string.IsNullOrEmpty(this.Layout.LabelPlural))
            {
                this.Layout.LabelPlural = this.Layout.Label;
            }

            if (string.IsNullOrEmpty(this.Layout.Icon))
            {
                this.Layout.Icon = "fa fa-cog";
            }

            if (this.Layout.Description == null)
            {
                var fieldName = this.Fields.First(f => !f.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)).Name;
                this.Layout.Description = new ItemLayoutDescription
                {
                    Pattern = "{0}"
                };

                this.Layout.Description.AddField(fieldName);
            }

            if (this.Features == null)
            {
                this.Features = ItemFeatures.Empty;
            }

            if (this.Forms == null)
            {
                this.forms = new List<Form>();
            }

            if (this.lists == null)
            {
                this.lists = new List<L.List>();
            }

            if (this.foreignValues == null)
            {
                this.foreignValues = new List<ForeignList>().ToArray();
            }

            foreach (var list in this.lists)
            {
                if (string.IsNullOrEmpty(list.Label))
                {
                    list.Label = this.Layout.LabelPlural;
                }
            }
        }

        public bool LinkedField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return false;
            }

            if (this.foreignValues == null || this.foreignValues.Length == 0)
            {
                return false;
            }

            foreach (var foreignList in this.foreignValues.Where(fl => !string.IsNullOrEmpty(fl.LinkField)))
            {
                if (foreignList.LocalName.Equals(fieldName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public ActionResult CreatePersistenceScript(string instanceName)
        {
            return CreatePersistenceScript(this.ItemName, instanceName);
        }

        public static ActionResult CreatePersistenceScript(string itemDefinitionName, string instanceName)
        {
            var res = ActionResult.NoAction;
            var data = Read.GetCustomFK(itemDefinitionName, instanceName);
            var path = Instance.Path.Scripts(instanceName);
            Basics.VerifyFolder(path);
            var fileName = string.Format(CultureInfo.InvariantCulture, @"{0}_FK.js", itemDefinitionName);
            using (var output = new StreamWriter(path + "\\" + fileName))
            {
                output.WriteLine(string.Format(CultureInfo.InvariantCulture, "// {0:dd/MM/yyyy hh:mm}", DateTime.UtcNow));
                output.WriteLine(string.Format(CultureInfo.InvariantCulture, @"FK[""{0}""] = {{""Data"":", itemDefinitionName));
                output.Write(data);
                output.Write(",\"Token\":\"Persistence\"};");
            }

            res.SetSuccess();

            return res;
        }

        public static ActionResult ReloadSessionItems(string instanceName)
        {
            var res = ActionResult.NoAction;
            var actualItem = string.Empty;
            try
            {
                var path = string.Format(CultureInfo.InvariantCulture, ConfigurationManager.AppSettings["ItemsDefinitionPath"] as string, instanceName);
                var files = Directory.GetFiles(path, "*.item");
                var index = new List<ItemDefinitonIndex>();
                var definitions = new List<ItemDefinition>();
                var definitionsJson = new List<string>();
                var instance = Instance.Actual;
                instance.ClearDefinitions();
                foreach (var file in files)
                {
                    var itemName = Path.GetFileNameWithoutExtension(file).ToLowerInvariant();
                    actualItem = itemName;
                    var definition = ItemDefinition.Empty;

                    definition = ItemDefinition.LoadFromFile(file, instanceName);
                    instance.AddDefinition(definition);
                    if (definition.Id > 0)
                    {
                        definitionsJson.Add(definition.Json);
                        definitions.Add(definition);
                        index.Add(new ItemDefinitonIndex
                        {
                            Id = definition.Id,
                            Name = definition.ItemName,
                            OnlyAdmin = definition.OnlyAdmin,
                            Icon = definition.Layout.Icon,
                            Label = definition.Layout.Label
                        });
                    }
                }

                GenerateDefinitionsScript(instance.Name);

                HttpContext.Current.Session["ItemDefinitionIndex"] = index;
                HttpContext.Current.Session["ItemDefinitionsJson"] = definitionsJson;
                res.SetSuccess();
            }
            catch (Exception ex)
            {
                res.SetFail(actualItem + "::>" + ex.Message);
            }

            return res;
        }

        public static void LoadSessionItems(string instanceName)
        {
            var index = new List<ItemDefinitonIndex>();
            var definitions = new List<ItemDefinition>();
            var definitionsJson = new List<string>();
            var instance = Instance.Actual;
            if (instance.Id < 0)
            {
                instance = Instance.LoadDefinition(instanceName, true);
            }

            foreach (var definition in instance.ItemDefinitions)
            {
                definitionsJson.Add(definition.Json);
                index.Add(new ItemDefinitonIndex
                {
                    Id = definition.Id,
                    Name = definition.ItemName,
                    OnlyAdmin = definition.OnlyAdmin,
                    Icon = definition.Layout.Icon,
                    Label = definition.Layout.Label
                });
            }

            var fileName = string.Format(
                CultureInfo.InvariantCulture,
                ConfigurationManager.AppSettings["ItemsDefinitionPath"].ToString(),
                instanceName);

            using (var output = new StreamWriter(fileName + "ItemDefinition.js"))
            {
                bool first = true;
                output.WriteLine("var ItemDefinitions =[");
                foreach (var definition in definitionsJson)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        output.Write(",");
                    }

                    output.Write(definition);
                }

                output.WriteLine("];");
            }

            HttpContext.Current.Session["ItemDefinitionIndex"] = index;
            HttpContext.Current.Session["ItemDefinitionsJson"] = definitionsJson;
        }

        public static string NameById(long id)
        {
            var instance = Instance.Actual;
            if (instance.ItemDefinitions.Any(d => d.Id == id))
            {
                return instance.ItemDefinitions.First(d => d.Id == id).ItemName;
            }

            return string.Empty;
        }

        public struct ItemDefinitonIndex
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public bool OnlyAdmin { get; set; }
            public string Icon { get; set; }
            public string Label { get; set; }
        }

        public bool FieldIsFK(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return false;
            }

            if (this.foreignValues == null || this.foreignValues.Count() == 0)
            {
                return false;
            }

            if (this.foreignValues.Any(fv => fv.LocalName.Equals(fieldName, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return false;
        }

        public void CreatePersistenceScript(long companyId, string instanceName)
        {
            var path = Instance.Path.Scripts(instanceName).Replace("\\\\", "\\" + instanceName + "\\");

            if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, "{0}\\", path);
            }

            var filePath = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}{1}_FK.js",
                path,
                this.ItemName);

            var data = OpenFrameworkV3.Tools.Json.EmptyJsonList;
            if (!string.IsNullOrEmpty(this.PersistentFK))
            {
                data = Read.GetCustomFK(this, this.InstanceName);
            }
            else
            {
                data = Read.JsonActive(this.ItemName, companyId, this.InstanceName);
            }

            var content = string.Format(
            CultureInfo.InvariantCulture,
                @"FK[""{0}""] = {{""Persistence"":true,""Data"":{1}}};",
                this.ItemName,
                data);

            using (var output = new StreamWriter(filePath, false))
            {
                output.Write(content);
            }

            using (var cmd = new SqlCommand("Core_PersistenceScript_Save"))
            {
                using (var cnn = new SqlConnection(Persistence.ConnectionString(instanceName)))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("ItemName", ItemName, 50));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
            }
        }
        public struct PersistenceInfo
        {
            public string ItemName { get; set; }
            public DateTime LastSync { get; set; }
        }


        public static ItemData FromDynamic(string instanceName, string itemName, dynamic data)
        {
            var res = new ItemData();
            var definition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            if (data != null)
            {
                var values = (IDictionary<string, object>)data;
                foreach (var pair in values)
                {
                    if (!pair.Key.StartsWith("DynamicTxT", StringComparison.OrdinalIgnoreCase))
                    {
                        var finalData = pair;
                        //if (pair.Value == null)
                        //{
                        //    if (definition.SqlMappings.Any(s => s.ItemField == pair.Key))
                        //    {
                        //        finalData = new KeyValuePair<string, object>(pair.Key, definition.SqlMappings.Where(s => s.ItemField == pair.Key).First().DefaultValue);
                        //    }
                        //}

                        res.Add(finalData);
                    }
                }
            }

            return res;
        }
    }
}