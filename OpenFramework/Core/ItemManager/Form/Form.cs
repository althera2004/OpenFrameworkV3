// --------------------------------
// <copyright file="SecurityConfiguration.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager.ItemForm
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text.RegularExpressions;

    /// <summary>Implements form definition</summary>
    public sealed class Form
    {
        /// <summary>Identifier of the form</summary>
        [JsonProperty("Id")]
        private readonly string id;

        /// <summary>Form type</summary>
        [JsonProperty("FormType")]
        private readonly string formType;

        /// <summary>List of tabs of the form</summary>
        [JsonProperty("Tabs")]
        private readonly List<FormTab> tabs;

        /// <summary>List of actions of the form</summary>
        [JsonProperty("Actions")]
        private readonly List<FormAction> actions;

        /// <summary>Checks if the form must have an accept button on the footer</summary>
        [JsonProperty("Footer")]
        private bool? footer;

        /// <summary>Gets an empty form definition</summary>
        [JsonIgnore]
        public static Form Empty
        {
            get
            {
                return new Form();
            }
        }

        /// <summary>Gets form identifier</summary>
        [JsonIgnore]
        public string Id
        {
            get
            {
                if(string.IsNullOrEmpty(this.id))
                {
                    return string.Empty;
                }

                return this.id;
            }
        }

        /// <summary>Gets form type</summary>
        [JsonIgnore]
        public string FormType
        {
            get
            {
                if(string.IsNullOrEmpty(this.formType))
                {
                    return "None";
                }

                return Regex.Replace(this.FormType, "^[a-z]", m => m.Value.ToUpper());
            }
        }

        /// <summary>Gets form type</summary>
        [JsonIgnore]
        public string FormTypeName
        {
            get
            {
                if(string.IsNullOrEmpty(this.formType))
                {
                    return "Custom";
                }

                switch(this.formType.ToUpperInvariant())
                {
                    case "POPUP":
                        return "Popup";
                    case "INFORM":
                        return "InForm";
                    default:
                        return "Custom";
                }
            }
        }

        /// <summary>Gets form tabs</summary>
        [JsonIgnore]
        public ReadOnlyCollection<FormTab> Tabs
        {
            get
            {
                if(this.tabs == null)
                {
                    return new ReadOnlyCollection<FormTab>(new List<FormTab>());
                }

                return new ReadOnlyCollection<FormTab>(this.tabs);
            }
        }

        /// <summary>Gets form actions</summary>
        [JsonIgnore]
        public ReadOnlyCollection<FormAction> Actions
        {
            get
            {
                if(this.actions == null)
                {
                    return new ReadOnlyCollection<FormAction>(new List<FormAction>());
                }

                return new ReadOnlyCollection<FormAction>(this.actions);
            }
        }

        /// <summary>Gets form label</summary>
        [JsonProperty("Label")]
        public string Label { get; private set; }

        /// <summary>Gets a value indicating whether form footer</summary>
        [JsonIgnore]
        public bool Footer
        {
            get
            {
                if(this.footer == null)
                {
                    return true;
                }

                return (bool)this.footer;
            }
        }

        /// <summary>Gets fields with document file type signable</summary>
        [JsonIgnore]
        public ReadOnlyCollection<FormField> FieldsDocumentSign
        {
            get
            {
                var res = new List<FormField>();
                if(this.tabs != null)
                {
                    foreach(var tab in this.tabs)
                    {
                        foreach(var row in tab.Rows)
                        {
                            foreach(var field in row.Fields)
                            {
                                if(!string.IsNullOrEmpty(field.Config))
                                {
                                    if(field.Config.Contains("S"))
                                    {
                                        res.Add(field);
                                    }
                                }
                            }
                        }
                    }
                }

                return new ReadOnlyCollection<FormField>(res);
            }
        }

        /// <summary>Gets a value indicating if exists fields with document file type signable</summary>
        [JsonIgnore]
        public bool HasFieldsDocumentSign
        {
            get
            {
                if(this.tabs == null) { return false; }
                foreach(var tab in this.tabs)
                {
                    foreach(var row in tab.Rows)
                    {
                        foreach(var field in row.Fields)
                        {
                            if(!string.IsNullOrEmpty(field.Config))
                            {
                                if(field.Config.Contains("S"))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }

                return false;
            }
        }

        /// <summary>Gets a value indicating if exists fields with document file type and historical</summary>
        [JsonIgnore]
        public bool HasFieldsDocumentHistorical
        {
            get
            {
                if(this.tabs == null) { return false; }
                foreach(var tab in this.tabs)
                {
                    foreach(var row in tab.Rows)
                    {
                        foreach(var field in row.Fields)
                        {
                            if(!string.IsNullOrEmpty(field.Config))
                            {
                                if(field.Config.Contains("H"))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }

                return false;
            }
        }
    }
}