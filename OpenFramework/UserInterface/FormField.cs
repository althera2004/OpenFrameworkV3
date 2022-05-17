// --------------------------------
// <copyright file="FormField.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.UserInterface
{
    using System;
    using System.Linq;
    using System.Web;
    using OpenFramework.DataAccess;
    using OpenFramework.InstanceManager;
    using OpenFramework.ItemManager;
    using OpenFramework.ItemManager.Form;

    public class FormField
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public bool Required { get; set; }
        public string TypeName { get; set; }
        public bool FK { get; set; }
        public int ColSpan { get; set; }
        public int? Length { get; set; }
        private ItemDefinition itemDefinition;
        public string SelectOptions { get; private set; }
        public string DataOrigin { get; set; }
        public ExtendSelect ExtendSelect { get; set; }
        public bool Linkable { get; set; }
        public int? Rows { get; set; }
        public int? Cols { get; set; }

        public static FormField FromDefinition(FormFieldDefinition definition, ItemDefinition itemDefinition, int colSpan)
        {
            var itemField = itemDefinition.Fields.First(f => f.Name.Equals(definition.Name, StringComparison.OrdinalIgnoreCase));
            var res = new FormField
            {
                Id = definition.Name,
                Label = definition.Label ?? itemField.LabelForm,
                Required = itemField.Required,
                TypeName = itemField.TypeName.ToLowerInvariant(),
                FK = itemField.FK,
                ColSpan = colSpan,
                itemDefinition = itemDefinition,
                ExtendSelect = definition.ExtendSelect,
                Linkable = itemField.Linkable,
                Cols = definition.ColSpan,
                Rows = definition.Rows
            };

            if (res.TypeName == "fixedlist")
            {
                res.DataOrigin = "fixedlist-" + itemField.FixedListId;
            }

            if (res.FK)
            {
                if (itemDefinition.ForeignValues.Any(fv => fv.LocalName.Equals(res.Id)))
                {
                    var instance = CustomerFramework.Actual;
                    var fv = itemDefinition.ForeignValues.First(f => f.LocalName.Equals(res.Id));
                    res.DataOrigin = "fk-" + fv.ItemName;
                }
            }

            if (itemField.Length != null)
            {
                res.Length = itemField.Length;
            }

            return res;
        }

        /*public string Render
        {
            get
            {
                switch (this.TypeName)
                {
                    case "text":
                        return new FormText
                        {
                            Id = this.Id,
                            ColumnSpanLabel = 1,
                            ColumnSpan = ColSpan,
                            Label = this.Label,
                            MaximumLength = this.Length ?? Constant.DefaultDatabaseVarChar
                        }.Render;
                    case "textarea":
                        return new FormTextArea
                        {
                            Id = this.Id,
                            ColumnSpanLabel = 1,
                            ColumnSpan = ColSpan,
                            Label = this.Label,
                            Rows = this.Rows
                        }.Render;
                    case "email":
                        return new FormTextEmail
                        {
                            Id = this.Id,
                            ColumnSpanLabel = 1,
                            ColumnSpan = ColSpan,
                            Label = this.Label,
                            MaximumLength = this.Length ?? Constant.DefaultDatabaseVarChar,
                            Linkable = this.Linkable
                        }.Render;
                    case "select":
                        ObtainSelectOptions();
                        if (this.ExtendSelect == ExtendSelect.ButtonBAR)
                        {
                            return new RapidActionButton
                            {
                                Id = this.Id,
                                Expand = ColSpan
                            }.Html;
                        }

                        return new FormSelect
                        {
                            Name = this.Id,
                            GrantToWrite = true,
                            Label = this.Label,
                            ColumnSpan = ColSpan,
                            ColumnSpanLabel = 1,
                            ExtendSelect = this.ExtendSelect
                        }.Render;
                    case "fixedlist":
                        return new FormSelect
                        {
                            Name = this.Id,
                            GrantToWrite = true,
                            Label = this.Label,
                            ColumnSpan = ColSpan,
                            ColumnSpanLabel = 1,
                            ExtendSelect = this.ExtendSelect
                        }.Render;
                    case "long":
                        if (this.FK)
                        {
                            ObtainSelectOptions();

                            if (this.ExtendSelect == ExtendSelect.ButtonBAR)
                            {
                                return new RapidActionButton
                                {
                                    Id = this.Id,
                                    Expand = ColSpan
                                }.Html;
                            }

                            return new FormSelect
                            {
                                Name = this.Id,
                                GrantToWrite = true,
                                Label = this.Label,
                                ColumnSpan = this.ColSpan,
                                ColumnSpanLabel = 1,
                                ExtendSelect = this.ExtendSelect
                            }.Render;
                        }

                        return new FormTextInteger
                        {
                            Id = this.Id,
                            ColumnSpanLabel = 1,
                            ColumnSpan = this.ColSpan,
                            Label = this.Label,
                            MaximumLength = this.Length ?? Constant.DefaultDatabaseVarChar
                        }.Render;
                    case "int":
                    case "integer":
                        return new FormTextInteger
                        {
                            Id = this.Id,
                            ColumnSpan = this.ColSpan,
                            ColumnSpanLabel = 1,
                            Label = this.Label,
                            GrantToWrite = true,
                            Numeric = true,
                            IsInteger = true
                        }.Render;
                    case "decimal":
                    case "money":
                        return new FormTextDecimal
                        {
                            Id = this.Id,
                            ColumnSpan = this.ColSpan,
                            ColumnSpanLabel = 1,
                            Label = this.Label,
                            GrantToWrite = true,
                            Numeric = true
                        }.Render;
                    case "boolean":
                    case "bool":
                        return new FormCheckBox
                        {
                            Name = this.Id,
                            ColumnSpan = this.ColSpan,
                            ColumnSpanLabel = 1,
                            Label = this.Label,
                            GrantToWrite = true
                        }.Render;
                    case "datetime":
                        return new FormDatePicker
                        {
                            Name = this.Id,
                            ColumnSpan = this.ColSpan,
                            ColumnSpanLabel = 1,
                            Label = this.Label,
                            GrantToWrite = true
                        }.Render;
                    default:
                        return this.Label + "_" + this.TypeName;
                }
            }
        }*/

        private void ObtainSelectOptions()
        {
            if (itemDefinition.ForeignValues.Any(fv => fv.LocalName.Equals(this.Id)))
            {
                var instance = CustomerFramework.Actual;
                var fv = itemDefinition.ForeignValues.First(f => f.LocalName.Equals(this.Id));
                this.SelectOptions = SqlStream.GetFKStream(new ItemBuilder(fv.ItemName, instance.Name).Definition, null, instance.Config.ConnectionString);
            }
        }
    }
}