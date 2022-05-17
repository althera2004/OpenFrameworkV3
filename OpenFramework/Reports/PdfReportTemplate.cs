// --------------------------------
// <copyright file="PdfReportTemplate.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Reports
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using OpenFrameworkV3.Core;
    using OpenFrameworkV3.Tools;

    public class PdfReportTemplate
    {
        [JsonProperty("Template")]
        public string Template { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Subject")]
        public string Subject { get; set; }

        [JsonProperty("ResultFile")]
        public string ResultFile { get; set; }

        [JsonProperty("Destination")]
        public ReportDestination Destination { get; set; }

        [JsonProperty("StoredProcedure")]
        public string StoredProcedure { get; set; }

        [JsonProperty("StoredParameters")]
        private readonly StoredParameter[] storedParameters;

        [JsonProperty("Signatures")]
        private readonly Signature[] signatures;

        [JsonProperty("Fields")]
        private readonly Field[] fields;

        [JsonProperty("Footers")]
        private readonly Field[] footers;

        [JsonProperty("Images")]
        private readonly ImageData[] images;

        [JsonIgnore]
        public string Name { get; set; }

        [JsonIgnore]
        public bool MultiSign
        {
            get
            {
                return Signatures.Count > 1;
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<Signature> Signatures
        {
            get
            {
                if (this.signatures == null)
                {
                    return new ReadOnlyCollection<Signature>(new List<Signature>());
                }

                return new ReadOnlyCollection<Signature>(this.signatures.ToList());
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<StoredParameter> StoredParameters
        {
            get
            {
                if (this.storedParameters == null)
                {
                    return new ReadOnlyCollection<StoredParameter>(new List<StoredParameter>());
                }

                return new ReadOnlyCollection<StoredParameter>(this.storedParameters.ToList());
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<Field> Fields
        {
            get
            {
                if (this.fields == null)
                {
                    return new ReadOnlyCollection<Field>(new List<Field>());
                }

                return new ReadOnlyCollection<Field>(this.fields.ToList());
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<Field> Footers
        {
            get
            {
                if (this.footers == null)
                {
                    return new ReadOnlyCollection<Field>(new List<Field>());
                }

                return new ReadOnlyCollection<Field>(this.footers.ToList());
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<ImageData> Images
        {
            get
            {
                if (this.images == null)
                {
                    return new ReadOnlyCollection<ImageData>(new List<ImageData>());
                }

                return new ReadOnlyCollection<ImageData>(this.images.ToList());
            }
        }

        public static string GetJsonDefinition(string name, string instanceName)
        {
            var res = Json.EmptyJsonObject;

            var path = Instance.Path.PdfTemplates(instanceName);
            if (!path.EndsWith("\\"))
            {
                path += "\\";
            }

            var templateFileName = string.Format(CultureInfo.InvariantCulture, "{0}{1}.pdfx", path, name);

            if (File.Exists(templateFileName))
            {
                using (var input = new StreamReader(templateFileName))
                {
                    res = input.ReadToEnd();
                }
            }

            return res;
        }

        public static PdfReportTemplate Load(string name, string instanceName)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new PdfReportTemplate();
            }

            if (name.EndsWith(".pdfx", System.StringComparison.OrdinalIgnoreCase))
            {
                return LoadByFile(name);
            }

            return LoadByName(name, instanceName);
        }

        private static PdfReportTemplate LoadByName(string name, string instanceName)
        {
            var path = Instance.Path.PdfTemplates(instanceName);
            if (!path.EndsWith("\\"))
            {
                path += "\\";
            }

            var templateFileName = string.Format(CultureInfo.InvariantCulture, "{0}{1}.pdfx", path, name);
            return LoadByFile(templateFileName);
        }

        private static PdfReportTemplate LoadByFile(string name)
        {
            var res = new PdfReportTemplate();
            if (File.Exists(name))
            {
                using (var input = new StreamReader(name))
                {
                    var json = input.ReadToEnd();
                    res = JsonConvert.DeserializeObject<PdfReportTemplate>(json);
                    res.Name = Path.GetFileNameWithoutExtension(name);
                }
            }

            return res;
        }

        public static ReadOnlyCollection<PdfReportTemplate> All(string instanceName)
        {
            var res = new List<PdfReportTemplate>();
            var files = Directory.GetFiles(Instance.Path.PdfTemplates(instanceName), "*.pdfx");
            foreach (var file in files)
            {
                res.Add(LoadByFile(file));
            }

            return new ReadOnlyCollection<PdfReportTemplate>(res);
        }
    }
}