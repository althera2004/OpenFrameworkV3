// --------------------------------
// <copyright file="ForeignKey.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFramework.ItemManager
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;

    /// <summary>Implements simple item for foreing key purposes</summary>
    public class ForeignKey
    {
        /// <summary>Gets or sets item identifier</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets item description</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets item definition</summary>
        public ItemDefinition ItemDefinition { get; set; }

        /// <summary>Gets query to obtain data</summary>
        /// <param name="itemDefinition">Item definition</param>
        /// <returns>SQL query</returns>
        public static string Query(ItemDefinition itemDefinition)
        {
            var res = new StringBuilder("SELECT Id");

            foreach (var field in itemDefinition.Layout.Description.Fields)
            {
                res.AppendFormat(CultureInfo.InvariantCulture, ",{0}", field.Name);
            }

            res.AppendFormat(CultureInfo.InvariantCulture, " FROM Item_{0} WITH(NOLOCK)", itemDefinition.ItemName);
            return res.ToString();
        }

        /// <summary>Gets item data for foreign jey purposes</summary>
        /// <param name="itemDefinition">Item definition</param>
        /// <returns>List of simple items</returns>
        public static ReadOnlyCollection<ForeignKey> Obtain(ItemDefinition itemDefinition)
        {
            var res = new List<ForeignKey>();
            using (var cmd = new SqlCommand(Query(itemDefinition)))
            {
                cmd.CommandType = CommandType.Text;
                using (var cnn = new SqlConnection(itemDefinition.Instance.Config.ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new ForeignKey
                                {
                                    Id = rdr.GetInt64(0),
                                    Description = rdr.GetString(1)
                                });
                            }
                        }
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

            return new ReadOnlyCollection<ForeignKey>(res);
        }
    }
}