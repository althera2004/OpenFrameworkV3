// --------------------------------
// <copyright file="PdfTemplate.cs" company="OpenFramework">
//     Copyright (c) 2019 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Export
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web.Script.Serialization;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core;

    [Serializable]
    public partial class PdfTemplateDefinition
    {
        [JsonProperty("Id")]
        [XmlElement(Type = typeof(long), ElementName = "Id")]
        public long Id { get; set; }

        [JsonProperty("Template")]
        [XmlElement(Type = typeof(string), ElementName = "Template")]
        public string Template { get; set; }

        [JsonProperty("StoredName")]
        [XmlElement(Type = typeof(string), ElementName = "StoredName")]
        public string StoredName { get; set; }

        /// <summary>Primary keys of item</summary>
        [JsonProperty("StoredParameters")]
        private readonly SqlStoredParameter[] storedParameters;

        [JsonProperty("FileNameResult")]
        [XmlElement(Type = typeof(string), ElementName = "FileNameResult")]
        public string FileNameResult { get; set; }

        /// <summary>Gets the primary keys</summary>
        [JsonIgnore]
        public ReadOnlyCollection<SqlStoredParameter> StoreParameters
        {
            get
            {                
                if (this.storedParameters != null)
                {
                    return new ReadOnlyCollection<SqlStoredParameter>(new List<SqlStoredParameter>());
                }

                return new ReadOnlyCollection<SqlStoredParameter>(this.storedParameters);
            }
        }

        public static PdfTemplateDefinition Empty
        {
            get
            {
                return new PdfTemplateDefinition
                {
                    Id = Constant.DefaultId,
                    Template = string.Empty,
                    StoredName = string.Empty,
                    FileNameResult = string.Empty
                };
            }
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"":{0},
                        ""Template"":{1},
                        ""StoredName"":""{2}"",
                        ""FileNameResult"":""{3}""
                    }}",
                    this.Id,
                    Tools.Json.JsonCompliant(this.Template),
                    Tools.Json.JsonCompliant(this.StoredName),
                    Tools.Json.JsonCompliant(this.FileNameResult));
            }
        }

        public static string JsonList(ReadOnlyCollection<PdfTemplateDefinition> list)
        {
            var res = new StringBuilder("[");
            if (list != null && list.Count > 0)
            {
                bool first = true;
                foreach (var template in list)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(template.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        public static PdfTemplateDefinition Load(string fileName, string instanceName)
        {
            PdfTemplateDefinition res = null;
            using (var instance = Persistence.InstanceByName(instanceName))
            {
                var path = Instance.Path.PdfTemplates(instanceName);
                string fileNameTemplate = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{0}{2}{1}.pdfx",
                        path,
                        fileName,
                        path.EndsWith("\\") ? string.Empty : "\\");

                if (File.Exists(fileNameTemplate))
                {
                    using (var input = new StreamReader(fileNameTemplate))
                    {
                        var data = input.ReadToEnd();
                        if (data != null)
                        {
                            res = new JavaScriptSerializer
                            {
                                MaxJsonLength = 500000000
                            }.Deserialize<PdfTemplateDefinition>(data);
                        }
                    }
                }
            }

            return res;
        }

        public static ReadOnlyCollection<PdfTemplateDefinition> All(string instanceName)
        {
            var res = new List<PdfTemplateDefinition>();
            var files = Directory.GetFiles(Instance.Path.PdfTemplates(instanceName), "*.pdfx");
            foreach (var file in files)
            {
                res.Add(Load(file, instanceName));
            }

            return new ReadOnlyCollection<PdfTemplateDefinition>(res);
        }
    }
}