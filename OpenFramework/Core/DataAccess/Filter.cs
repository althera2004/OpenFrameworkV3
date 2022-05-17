// --------------------------------
// <copyright file="Filter.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.DataAccess
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
    using System.Xml.Serialization;
    using OpenFrameworkV3.Core.Activity;

    [Serializable]
    public class Filter
    {
        public const string FormatCsv = "CSV";
        public const string FormatPDF = "PDF";
        public const string FormatExcel = "EXCEL";

        public Guid FilterId { get; set; }
        private List<FilterCriteria> criteria;
        public string ItemName { get; set; }
        public string ListDefinitionId { get; set; }
        public string FormatType { get; set; }

        [XmlIgnore()]
        public ReadOnlyCollection<FilterCriteria> Criteria
        {
            get
            {
                if(this.criteria == null)
                {
                    this.criteria = new List<FilterCriteria>();
                }

                return new ReadOnlyCollection<FilterCriteria>(this.criteria);
            }
        }

        public void Add(FilterCriteria criteriaItem)
        {

            if (this.criteria == null)
            {
                this.criteria = new List<FilterCriteria>();
            }

            this.criteria.Add(criteriaItem);
        }

        public string FilterQuery(long companyId, string instanceName)
        {
            if (string.IsNullOrEmpty(instanceName))
            {
                return string.Empty;
            }

            var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(this.ItemName, StringComparison.OrdinalIgnoreCase));
            itemDefinition.InstanceName = instanceName;
            var list = itemDefinition.Lists.FirstOrDefault(l => l.Id.Equals(this.ListDefinitionId, StringComparison.OrdinalIgnoreCase));
            if (list == null)
            {
                return Query.All(this.ItemName, companyId, instanceName);
            }

            return Query.ByList(itemDefinition, this.Criteria, list);
        }

        public ActionResult Export(long companyId, string instanceName)
        {
            var path = ConfigurationManager.AppSettings["ExportFolder"];
            if (!path.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", path, instanceName);
            }
            else
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", path, instanceName);
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            switch (FormatType.ToUpperInvariant())
            {
                case FormatCsv:
                    return ExportCsv(companyId, instanceName, path, HeaderTitles(instanceName));
                case FormatPDF:
                    return ExportPdf(instanceName, path, HeaderTitles(instanceName));
                case FormatExcel:
                    return ExportExcel(instanceName, path, HeaderTitles(instanceName));
                default:
                    return new ActionResult
                    {
                        MessageError = string.Format(CultureInfo.InvariantCulture, "Unsupported export format ({0})", FormatType),
                        ReturnValue = null,
                        Success = false
                    };
            }
        }

        public ActionResult ExportPdf(string instanceName, string exportFolder, ReadOnlyCollection<string> headers)
        {
            var res = ActionResult.NoAction;

            return res;
        }

        public ActionResult ExportCsv(long companyId, string instanceName, string exportFolder, ReadOnlyCollection<string> headers)
        {
            var res = ActionResult.NoAction;
            var fileName = string.Format(CultureInfo.InvariantCulture, @"{0}\{1}-{2:yyyyMMddhhmmss}.csv", exportFolder, ItemName, DateTime.Now);
            using (var output = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                foreach(var header in headers)
                {
                    output.Write(header);
                    output.Write(";");
                }

                output.WriteLine();

                var cns = Persistence.ConnectionString(instanceName);
                if (!string.IsNullOrEmpty(cns))
                {
                    using (var cmd = new SqlCommand(FilterQuery(companyId, instanceName)))
                    {
                        using (var cnn = new SqlConnection(cns))
                        {
                            cmd.Connection = cnn;
                            try
                            {
                                cmd.Connection.Open();
                                using (var rdr = cmd.ExecuteReader())
                                {
                                    while (rdr.Read())
                                    {
                                        for (int columnIndex = 1; columnIndex < rdr.FieldCount; columnIndex++)
                                        {
                                            if (!rdr.IsDBNull(columnIndex))
                                            {
                                                output.Write(rdr[columnIndex]);
                                            }

                                            output.Write(";");
                                        }

                                        output.WriteLine();
                                    }
                                }

                                res.SetSuccess(fileName.Replace(ConfigurationManager.AppSettings["BaseFolder"] as string, "/").Replace("\\", "/"));
                            }
                            catch (SqlException ex)
                            {
                                res.SetFail(ex);
                            }
                            catch (NullReferenceException ex)
                            {
                                res.SetFail(ex);
                            }
                            catch (Exception ex)
                            {
                                res.SetFail(ex);
                            }
                            finally
                            {
                                if (cmd.Connection.State != ConnectionState.Closed)
                                {
                                    cmd.Connection.Close();
                                }
                            }
                        }
                    }
                }
            }

            return res;
        }

        public ActionResult ExportExcel(string instanceName, string exportFolder, ReadOnlyCollection<string> headers)
        {
            var res = ActionResult.NoAction;

            return res;
        }

        private ReadOnlyCollection<string> HeaderTitles(string instanceName)
        {
            var res = new List<string>();
            var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.ItemName.Equals(ItemName, StringComparison.OrdinalIgnoreCase));
            var listDefinition = itemDefinition.Lists.First(l => l.Id == ListDefinitionId);

            foreach (var column in listDefinition.Columns)
            {
                if (!string.IsNullOrEmpty(column.Label))
                {
                    res.Add(column.Label);
                }
                else
                {
                    res.Add(itemDefinition.Fields.First(f => f.Name.Equals(column.DataProperty, StringComparison.OrdinalIgnoreCase)).Label);
                }
            }

            return new ReadOnlyCollection<string>(res);
        }
    }
}