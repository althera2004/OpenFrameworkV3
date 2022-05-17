// --------------------------------
// <copyright file="Bank.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Billing
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using OpenFrameworkV3;

    public class Bank
    {
        /// <summary>Gets or sets Bank identifier</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets bank name</summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets bank code
        /// </summary>
        public string Code { get; set; }
        public string Swift { get; set; }

        public static Bank Empty
        {
            get
            {
                return new Bank
                {
                    Id = Constant.DefaultId,
                    Name = string.Empty,
                    Code = string.Empty,
                    Swift = string.Empty
                };
            }
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""Name"":""{1}"",""Code"":""{2}"",""Swift"":""{3}""}}",
                    this.Id,
                    Tools.Json.JsonCompliant(this.Name),
                    this.Code,
                    this.Swift);
            }
        }

        public static Bank ByCode(string code, string instanceName)
        {
            var res = Bank.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand())
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = string.Format(
                            CultureInfo.InvariantCulture,
                            "SELECT Id, ISNULL(BankName,''), Code, ISNULL(SWIFT,'') FROM OpenData_Bank WHERE Code LIKE '%{0}%'",
                            code);
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    res.Id = rdr.GetInt64(0);
                                    res.Name = rdr.GetString(1).Trim();
                                    res.Code = rdr.GetString(2).Trim();
                                    res.Swift = rdr.GetString(3).Trim();
                                }
                            }
                        }
                        finally
                        {
                            if(cmd.Connection.State != ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
                    }
                }
            }

            return res;
        }
    }
}