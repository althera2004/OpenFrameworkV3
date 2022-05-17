using OpenFramework.InstanceManager;
using OpenFramework.ItemManager;
using OpenFramework.ItemManager.Form;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFramework.UserInterface
{
    public class FormRow
    {
        public string Id { get; set; }
        public bool Hidden { get; set; }
        public Collection<FormField> Fields { get; private set; }
        public FormRowDefinition FormRowDefinition { get; set; }
        public ItemDefinition ItemDefinition { get; set; }
        public FormList FormList { get; set; }
        private CustomerFramework instance;

        public static FormRow FromDefinition(FormRowDefinition definition, ItemDefinition itemDefinition)
        {
            var res = new FormRow
            {
                Fields = new Collection<FormField>()
            };

            res.FormRowDefinition = definition;
            res.instance = itemDefinition.Instance;

            int colspan = 10;

            int fieldsCount = definition.Fields.Count;
            if (definition.Fields.Any(df => df.ColSpan.HasValue))
            {
                foreach(var field in definition.Fields.Where(df => df.ColSpan.HasValue).ToList())
                {
                    fieldsCount += field.ColSpan.Value - 1;
                }
            }

            switch (fieldsCount)
            {
                case 3:
                    colspan = 3;
                    break;
                case 2:
                    colspan = 5;
                    break;
                default:
                    colspan = 11;
                    break;
            }

            foreach (var field in definition.Fields)
            {
                if (!string.IsNullOrEmpty(field.Name))
                {
                    res.Fields.Add(FormField.FromDefinition(field, itemDefinition, field.ColSpan ?? colspan));
                }
            }

            if (definition.IsItemList)
            {
                res.FormList = new FormList(definition.ListId, definition.ItemList, itemDefinition.Instance.Name);
            }

            return res;
        }

        /*public string Render
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"
                        <div class=""form-group""{0}{1}>
                        {2}
                        </div>",
                    string.IsNullOrEmpty(this.Id) ? string.Empty : " id=\"" + this.Id + "\"",
                    this.Hidden ? " style=\"display:none;\"" : string.Empty,
                    RenderContent());
            }
        }*/

        public string DataOrigin
        {
            get
            {
                var res = new StringBuilder();
                bool first = true;
                foreach (var field in this.Fields)
                {
                    if (!string.IsNullOrEmpty(field.DataOrigin))
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            res.Append(",").Append(Environment.NewLine);
                        }

                        res.AppendFormat(
                            CultureInfo.InvariantCulture,
                            @"""{0}"": {{""Type"":""{1}"",""Origin"":""{2}""}}",
                            field.Id,
                            field.DataOrigin.Split('-')[0],
                            field.DataOrigin.Split('-')[1]);
                    }
                }

                return res.ToString();
            }
        }

        public string SelectOptions
        {
            get
            {
                var res = new StringBuilder();
                bool first = true;
                foreach(var field in this.Fields)
                {
                    if (!string.IsNullOrEmpty(field.SelectOptions))
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            res.Append(",").Append(Environment.NewLine);
                        }

                        res.AppendFormat(
                            CultureInfo.InvariantCulture,
                            @"""{0}"": {1}",
                            field.Id.Substring(0, field.Id.Length-2),
                            field.SelectOptions);
                    }
                }

                return res.ToString();
            }
        }

        /*private string RenderContent()
        {
            var res = new StringBuilder();
            if (this.Fields.Count > 0)
            {
                foreach (var field in this.Fields)
                {
                    res.Append(field.Render);
                }
            }

            if(this.FormList != null)
            {
                res.Append(this.FormList.Render);
            }


            return res.ToString();
        }*/
    }
}
