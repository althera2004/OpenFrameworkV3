// -----------------------------------------------------------------------
// <copyright file="ActivityTrace.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace OpenFramework.Activity
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using OpenFramework.Security;
    using OpenFramework.Tools;

    /// <summary>A class that implements trace of activity in application</summary>
    public class ActivityTrace
    {
        /// <summary>Prefix for trace dictionary prefix</summary>
        public const string ItemTraceDictionaryPrefix = "Item_Trace_";

        /// <summary>Gets or sets a value indicating activity description</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets a value indicating the target identifier</summary>
        public int TargetId { get; set; }

        /// <summary>Gets or sets value indicatong the name of employee that performs action</summary>
        public string ActionEmployee { get; set; }

        /// <summary>Gets or sets de type of target</summary>
        public string Target { get; set; }

        /// <summary>Gets or sets tha action ocurred</summary>
        public string Action { get; set; }

        /// <summary>Gets or sets the date of action</summary>
        public DateTime Date { get; set; }

        /// <summary>Gets or sets a description of changes of action</summary>
        public string Changes { get; set; }

        /// <summary>Gets the html code that shows a trace</summary>
        /// <example>
        /// <tr>
        ///     <td>Date</td>
        ///     <td>Target</td>
        ///     <td>Action</td>
        ///     <td>Data changes</td>
        ///     <td>Employee</td>
        /// </tr>
        /// </example>
        public string TableRow
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", this.Date, this.Target, this.Action, this.Changes, this.ActionEmployee);
            }
        }

        /// <summary>Gets the html code that shows a trace for principal trace tables</summary>
        /// <example>
        /// <tr>
        ///     <td>Date</td>
        ///     <td>Target identifier</td>
        ///     <td>Action</td>
        ///     <td>Data changes</td>
        ///     <td>Employee</td>
        /// </tr>
        /// </example>
        public string TableTracesRow
        {
            get
            {
                string description = this.Description;
                if (string.IsNullOrEmpty(description))
                {
                    description = string.Format(CultureInfo.InvariantCulture, "[{0}]", this.TargetId);
                }

                return string.Format(CultureInfo.InvariantCulture, @"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", this.Date, this.Description, this.Action, this.Changes, this.ActionEmployee);
            }
        }

        /// <summary>Gets the html code that shows a targeted trace</summary>
        /// <example>
        /// <tr>
        ///     <td>Date</td>
        ///     <td>Action</td>
        ///     <td>Data changes</td>
        ///     <td>Employee</td>
        /// </tr>
        /// </example>
        public string TableTargetedRow
        {
            get
            {
                string pattern = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>";
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    this.Date,
                    Basics.Translate(ItemTraceDictionaryPrefix + this.Action),
                    this.Changes,
                    this.ActionEmployee);
            }
        }

        /*/// <summary>Gets html code for table traces of an item</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="targetType">Target type of item</param>
        /// <returns>Html code for table traces of an item</returns>
        public static string RenderTraceTableForItem(int itemId, TargetType targetType)
        {
            var company = HttpContext.Current.Session["company"] as Company;
            var res = new StringBuilder();
            var activities = ActivityLog.GetActivity(itemId, targetType, company.Id, null, null);
            foreach (var activity in activities)
            {
                res.Append(activity.TableTargetedRow);
            }

            return res.ToString();
        }

        /// <summary>Gets html code for table traces of an item</summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="targetType">Target type of item</param>
        /// <returns>Html code for table traces of an item</returns>
        public static string RenderTraceTableForItem(long itemId, TargetType targetType)
        {
            var company = HttpContext.Current.Session["company"] as Company;
            var res = new StringBuilder();
            var activities = ActivityLog.GetActivity(itemId, targetType, company.Id, null, null);
            foreach (var activity in activities)
            {
                res.Append(activity.TableTargetedRow);
            }

            return res.ToString();
        }*/
    }
}