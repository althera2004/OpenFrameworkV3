namespace OpenFrameworkV3.Core.ItemManager.ItemList
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>Implements ListDefinition class</summary>
    public sealed class List
    {
        /// <summary>Gets list form identifier</summary>
        [JsonProperty("FormId")]
        private readonly string formId;

        /// <summary>Edit action</summary>
        [JsonProperty("GridMode")]
        private readonly string gridMode;

        /// <summary>Edit action</summary>
        [JsonProperty("EditAction")]
        private string editAction;

        /// <summary>Label for Add button</summary>
        [JsonProperty("ButtonAddLabel")]
        public string ButtonAddLabel { get; set; }

        /// <summary>List's columns</summary>
        [JsonProperty("Columns")]
        private Column[] columns;

        /// <summary>Configuration of sorting of list's columns</summary>
        [JsonProperty("Sorting")]
        private Sorting[] sorting;

        /// <summary>Parameters for a custom data extraction</summary>
        [JsonProperty("Parameters")]
        private readonly ListParameter[] parameters;

        /// <summary>List buttons</summary>
        [JsonProperty("Buttons")]
        private readonly List<ListButton> buttons;

        /// <summary>List layout</summary>
        [JsonProperty("Layout")]
        private string layout;

        /// <summary>Gets an empty list definition</summary>
        [JsonIgnore]
        public static List Empty
        {
            get
            {
                return new List
                {
                    Id = string.Empty,
                    columns = null,
                    sorting = null,
                    editAction = string.Empty,
                    layout = string.Empty
                };
            }
        }

        /// <summary>Gets or sets minimum height</summary>
        [JsonProperty("MinHeight")]
        public int MinHeight;

        /// <summary>Gets or sets forced height</summary>
        [JsonProperty("ForceHeight")]
        public int ForceHeight;

        /// <summary>Gets or sets size of list pagination</summary>
        [JsonProperty("Pagesize")]
        public int PageSize { get; set; }

        /// <summary>Gets a value indicating whether if list is readonly</summary>
        [JsonProperty("ReadOnly")]
        public bool ReadOnly { get; private set; }

        /// <summary>Gets a value indicating whether if list is exportable</summary>
        [JsonProperty("Exportable")]
        public bool Exportable { get; private set; }

        /// <summary>Gets a value indicating whether if list is importable</summary>
        [JsonProperty("Importable")]
        public bool Importable { get; private set; }

        /// <summary>Gets a value indicating whether if list is filtrable</summary>
        [JsonProperty("Filtrable")]
        public bool Filtrable { get; private set; }

        /// <summary>Gets a value indicating whether if list has gelolocation enabled</summary>
        [JsonProperty("GeoLocation")]
        public bool GeoLocation { get; private set; }

        /// <summary>Gets or sets list label</summary>
        [JsonProperty("Label")]
        public string Label { get; set; }

        /// <summary>Gets or sets a css class</summary>
        [JsonProperty("RowClass")]
        public string RowClass { get; set; }

        /// <summary>Gets or sets name of linked item y edit actions is ItemLinked</summary>
        [JsonProperty("ItemLink")]
        public string ItemLink { get; set; }

        /// <summary>Gets or sets a template for data list</summary>
        [JsonProperty("Template")]
        public string Template { get; set; }

        /// <summary>Gets or sets list title</summary>
        [JsonProperty("Title")]
        public string Title { get; set; }

        /// <summary>Gets or sets name of item list</summary>
        [JsonIgnore]
        public string ItemName { get; set; }

        /// <summary>Gets or sets explanation for list</summary>
        [JsonProperty("Explanation")]
        public string Explanation { get; set; }

        /// <summary>Gets or sets message for no data restuls</summary>
        [JsonProperty("NoDataMessage")]
        public string NoDataMessage { get; set; }

        /// <summary>Gets the buttons of list</summary>
        [JsonIgnore]
        public ReadOnlyCollection<ListButton> Buttons
        {
            get
            {
                if(this.buttons == null || this.buttons.Count == 0)
                {
                    return new ReadOnlyCollection<ListButton>(new List<ListButton>());
                }

                return new ReadOnlyCollection<ListButton>(this.buttons);
            }
        }

        /// <summary>Gets or sets list identifier</summary>
        [JsonProperty("Id")]
        public string Id { get; set; }

        /// <summary>Gets or sets a custom AJAX source</summary>
        [JsonProperty("CustomAjaxSource")]
        public string CustomAjaxSource { get; set; }

        /// <summary>Gets a value indicating whether if list shows as a grid</summary>
        [JsonIgnore]
        public bool GridMode
        {
            get
            {
                if(string.IsNullOrEmpty(this.gridMode))
                {
                    return false;
                }

                if(this.gridMode.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }


        [JsonIgnore]
        public string EditAction
        {
            get
            {
                return EditAction ?? "CustomForm";
            }
        }


        [JsonIgnore]
        public string Layout
        {
            get
            {
                return this.layout ?? "Basic";
            }
        }

        /// <summary>Gets the form identifier</summary>
        [JsonIgnore]
        public string FormId
        {
            get
            {
                if(string.IsNullOrEmpty(this.formId))
                {
                    return this.Id;
                }

                return this.formId;
            }
        }

        /// <summary>Gets the columns of list</summary>
        [JsonIgnore]
        public ReadOnlyCollection<Column> Columns
        {
            get
            {
                if(this.columns == null)
                {
                    return new ReadOnlyCollection<Column>(new List<Column>());
                }

                return new ReadOnlyCollection<Column>(this.columns);
            }
        }

        /// <summary> Gets the sorting configuration of columns</summary>
        [JsonIgnore]
        public ReadOnlyCollection<Sorting> Sorting
        {
            get
            {
                if(this.sorting == null)
                {
                    return new ReadOnlyCollection<Sorting>(new List<Sorting>());
                }

                return new ReadOnlyCollection<Sorting>(this.sorting);
            }
        }

        /// <summary> Gets the parameters of list</summary>
        [JsonIgnore]
        public ReadOnlyCollection<ListParameter> Parameters
        {
            get
            {
                if(this.parameters == null)
                {
                    return new ReadOnlyCollection<ListParameter>(new List<ListParameter>());
                }

                return new ReadOnlyCollection<ListParameter>(this.parameters);
            }
        }

        /// <summary>Gets default list configuration based onitem definition</summary>
        /// <param name="itemDefinition">Item definition</param>
        /// <returns>List configuration definition</returns>
        public static List Default(ItemDefinition itemDefinition)
        {
            var res = new List
            {
                ItemName = itemDefinition.ItemName,
                Label = itemDefinition.Layout.LabelPlural,
                Id = "Custom",
                layout = "Basic",
                editAction = "FormPage"
            };

            var columns = new List<Column>();

            foreach(var field in itemDefinition.Fields.Where(f => !f.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)))
            {
                columns.Add(new Column { DataProperty = field.Name, Label = field.Label });
            }

            res.columns = columns.ToArray();
            return res;
        }
    }
}