// --------------------------------
// <copyright file="InvoicePerson.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Billing
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using OpenFrameworkV3.Core.Companies;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.ItemManager;

    /// <summary>Implements Invoice class</summary>
    public partial class Invoice
    {
        /// <summary>Implements InvoicePerson class</summary>
        public class InvoicePerson
        {
            private long id;
            private string name;
            private string cif;
            private string iban;
            private string swift;
            private string address;
            private string postalCode;
            private string city;
            private string province;
            private string country;
            private string phone;
            private string email;

            public long Id
            {
                get
                {
                    if(this.id < 1)
                    {
                        return Constant.DefaultId;
                    }

                    return this.id;
                }

                set
                {
                    this.id = value;
                }
            }

            public string Name
            {
                get
                {
                    return this.name ?? string.Empty;
                }

                set
                {
                    this.name = value ?? string.Empty;
                }
            }

            public string CIF
            {
                get
                {
                    return this.cif ?? string.Empty;
                }

                set
                {
                    this.cif = (value ?? string.Empty).ToUpperInvariant();
                }
            }

            public string IBAN
            {
                get
                {
                    return this.iban ?? string.Empty;
                }

                set
                {
                    this.iban = (value ?? string.Empty).ToUpperInvariant();
                }
            }

            public string SWIFT
            {
                get
                {
                    return this.swift ?? string.Empty;
                }

                set
                {
                    this.swift = (value ?? string.Empty).ToUpperInvariant();
                }
            }

            public string Address
            {
                get
                {
                    return this.address ?? string.Empty;
                }

                set
                {
                    this.address = value ?? string.Empty;
                }
            }

            public string PostalCode
            {
                get
                {
                    return this.postalCode ?? string.Empty;
                }

                set
                {
                    this.postalCode = value ?? string.Empty;
                }
            }

            public string City
            {
                get
                {
                    return this.city ?? string.Empty;
                }

                set
                {
                    this.city = value ?? string.Empty;
                }
            }

            public string Province
            {
                get
                {
                    return this.province ?? string.Empty;
                }

                set
                {
                    this.province = value ?? string.Empty;
                }
            }

            public string Country
            {
                get
                {
                    return this.country ?? string.Empty;
                }

                set
                {
                    this.country = value ?? string.Empty;
                }
            }

            public string Phone
            {
                get
                {
                    return this.phone ?? string.Empty;
                }

                set
                {
                    this.phone = value ?? string.Empty;
                }
            }

            public string Email
            {
                get
                {
                    return this.email ?? string.Empty;
                }

                set
                {
                    this.email = (value ?? string.Empty).ToLowerInvariant();
                }
            }

            public static InvoicePerson Empty
            {
                get
                {
                    return new InvoicePerson
                    {
                        name = string.Empty,
                        iban = string.Empty,
                        swift = string.Empty,
                        address = string.Empty,
                        postalCode = string.Empty,
                        city = string.Empty,
                        province = string.Empty,
                        country = string.Empty,
                        cif = string.Empty,
                        email = string.Empty,
                        phone = string.Empty
                    };
                }
            }

            public string Json
            {
                get
                {
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        @"{{""Name"":""{0}"",""IBAN"":""{1}"",""SWIFT"":""{10}"",""Address"":""{2}"",""PostalCode"":""{3}"",""City"":""{4}"",""Province"":""{5}"",""Country"":""{6}"",""CIF"":""{7}"",""Phone"":""{8}"",""Email"":""{9}""}}",
                        Tools.Json.JsonCompliant(this.Name),
                        Tools.Json.JsonCompliant(this.IBAN).ToUpperInvariant(),
                        Tools.Json.JsonCompliant(this.Address),
                        Tools.Json.JsonCompliant(this.PostalCode),
                        Tools.Json.JsonCompliant(this.City),
                        Tools.Json.JsonCompliant(this.Province),
                        Tools.Json.JsonCompliant(this.Country),
                        Tools.Json.JsonCompliant(this.CIF).ToUpperInvariant(),
                        Tools.Json.JsonCompliant(this.Phone),
                        Tools.Json.JsonCompliant(this.Email).ToLowerInvariant(),
                        Tools.Json.JsonCompliant(this.SWIFT));
                }
            }

            public static InvoicePerson FromCompany(long companyId, long addressId, long accountId, string instanceName)
            {
                var res = InvoicePerson.Empty;
                var company = Company.ById(companyId, instanceName);
                var address = company.Addresses.First(a => a.Id == addressId);
                var account = CompanyBankAccount.ById(accountId, companyId, instanceName);

                res.address = address.Address;
                res.cif = company.Cif;
                res.city = address.City;
                res.country = address.Country;
                res.email = company.Email;
                res.IBAN = account.IBAN;
                res.SWIFT = account.Swift;
                res.Name = company.Name;
                res.phone = string.Empty;
                res.postalCode = address.PostalCode;
                res.province = address.Province;
                return res;
            }

            public static InvoicePerson FromDefinition(long id, string itemName, string instanceName)
            {
                var definition = InvoicePersonDefinition.LoadFromFile(itemName, instanceName);
                return FromDefinition(id, definition, instanceName);
            }

            public static InvoicePerson FromDefinition(long id, long itemDefinitionId, string instanceName)
            {
                var definition = InvoicePersonDefinition.LoadFromFile(itemDefinitionId, instanceName);
                return FromDefinition(id, definition, instanceName);
            }

            public static InvoicePerson FromDefinition(long id, InvoicePersonDefinition definition, string instanceName)
            {
                var res = InvoicePerson.Empty;
                var itemDefinition = Persistence.ItemDefinitions(instanceName).First(d => d.Id == definition.ItemDefinitionId);

                var query = new StringBuilder("SELECT Item.Id,");

                foreach(var field in definition.Fields)
                {
                    query.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "    {0}{1}",
                        field.SqlFieldExtractor(itemDefinition),
                        Environment.NewLine);
                }

                query.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"    Item.Active 
                        FROM Item_{0} AS Item 

                        WHERE Item.Id = {1}",
                    itemDefinition.ItemName,
                    id);

                var cns = Persistence.ConnectionString(instanceName);
                if (!string.IsNullOrEmpty(cns))
                {
                    using (var cmd = new SqlCommand(query.ToString()))
                    {
                        using (var cnn = new SqlConnection(cns))
                        {
                            cmd.Connection = cnn;
                            cmd.CommandType = CommandType.Text;
                            try
                            {
                                cmd.Connection.Open();
                                using (var rdr = cmd.ExecuteReader())
                                {
                                    if (rdr.HasRows)
                                    {
                                        rdr.Read();
                                        res.iban = rdr.GetString(3);
                                        res.cif = rdr.GetString(2);
                                        res.address = rdr.GetString(4);
                                        //var postalCode = ItemBuilder.ById("LocationPostalCode", Convert.ToInt64(rdr.GetString(5)), instance);
                                        var postalCode = Read.ById<ItemData>(Convert.ToInt64(rdr.GetString(5)), "LocationPostalCode", instanceName);
                                        res.postalCode = postalCode.GetValue<string>("Code");
                                        //var city = ItemBuilder.ById("LocationCity", Convert.ToInt64(postalCode["LocationLocalidadId"]), instance);

                                        var city = Read.ById<ItemData>(Convert.ToInt64(postalCode["LocationLocalidadId"]), "LocationCity", instanceName);

                                        res.city = city.GetValue<string>("Name");
                                        //res.province = ItemBuilder.ById("LocationProvince", Convert.ToInt64(city["LocationProvinciaId"]), instance).Description;
                                        var province = Read.ById<ItemData>(Convert.ToInt64(postalCode["LocationProvinciaId"]), "LocationProvince", instanceName);

                                        res.province = province.GetValue<string>("Name");
                                        res.email = rdr.GetString(14);
                                        res.phone = rdr.GetString(13);
                                        res.name = rdr.GetString(1);
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
                }

                return res;
            }
        }
    }
}