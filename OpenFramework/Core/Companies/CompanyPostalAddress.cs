// --------------------------------
// <copyright file="CompanyPostalAddress.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Companies
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using OpenFrameworkV3.CommonData;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Bindings;
    using OpenFrameworkV3.Core.DataAccess;

    public class CompanyPostalAddress : PostalAddress
    {
        [XmlElement(Type = typeof(long), ElementName = "Id")]
        public long Id { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "CompanyId")]
        public long CompanyId { get; set; }

        [XmlElement(Type = typeof(bool), ElementName = "Main")]
        public bool Main { get; set; }

        [XmlElement(Type = typeof(bool), ElementName = "Billing")]
        public bool Billing { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Description")]
        public string Description { get; set; }
        
        public static new CompanyPostalAddress Empty
        {
            get
            {
                return new CompanyPostalAddress
                {
                    Id = Constant.DefaultId,
                    Main = false,
                    Billing = false,
                    Description = string.Empty,
                    WayTypeId = Constant.DefaultId,
                    WayType = string.Empty,
                    Address = string.Empty,
                    PostalCodeId = Constant.DefaultId,
                    PostalCode = string.Empty,
                    City = string.Empty,
                    Province = string.Empty,
                    State = string.Empty,
                    Country = string.Empty,
                    Latitude = null,
                    Longitude = null,
                    Active = false
                };
            }
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"":{0},
                        ""Main"":{1},
                        ""Billing"":{2},
                        ""Description"":""{3}"",
                        ""WayTypeId"":{4},
                        ""WayType"":""{5}"",
                        ""Address"":""{6}"",
                        ""PostalCodeId"":{7},
                        ""PostalCode"":""{8}"",
                        ""City"":""{9}"",
                        ""Province"":""{10}"",
                        ""Active"":{11}
                    }}",
                    this.Id,
                    ConstantValue.Value(this.Main),
                    ConstantValue.Value(this.Billing),
                    Tools.Json.JsonCompliant(this.Description),
                    this.WayTypeId,
                    Tools.Json.JsonCompliant(this.WayType),
                    Tools.Json.JsonCompliant(this.Address),
                    this.PostalCodeId,
                    Tools.Json.JsonCompliant(this.PostalCode),
                    Tools.Json.JsonCompliant(this.City),
                    Tools.Json.JsonCompliant(this.Province),
                    ConstantValue.Value(this.Active));
            }
        }

        public static string JsonList(ReadOnlyCollection<CompanyPostalAddress> list)
        {
            if(list == null || list.Count < 1)
            {
                return Tools.Json.EmptyJsonList;
            }

            var res = new StringBuilder("[");
            bool first = true;
            foreach(var item in list)
            {
                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "{0}{1}",
                    first ? string.Empty : ",",
                    item.Json);

                first = false;
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<CompanyPostalAddress> ByCompany(long companyId, string instanceName)
        {
            var res = new List<CompanyPostalAddress>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyAdress_ByCompanyId"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new CompanyPostalAddress
                                    {
                                        Id = rdr.GetInt64(ColumnsCompanyAdressGet.Id),
                                        Main = rdr.GetBoolean(ColumnsCompanyAdressGet.Main),
                                        Billing = rdr.GetBoolean(ColumnsCompanyAdressGet.Billing),
                                        Description = rdr.GetString(ColumnsCompanyAdressGet.Description),
                                        WayTypeId = rdr.GetInt64(ColumnsCompanyAdressGet.LocationWayTypeId),
                                        WayType = rdr.GetString(ColumnsCompanyAdressGet.LocationWayType),
                                        Address = rdr.GetString(ColumnsCompanyAdressGet.LocationAddress),
                                        PostalCodeId = rdr.GetInt64(ColumnsCompanyAdressGet.LocationPostalCodeId),
                                        PostalCode = rdr.GetString(ColumnsCompanyAdressGet.LocationPostalCode).Trim(),
                                        City = rdr.GetString(ColumnsCompanyAdressGet.LocationCity),
                                        Province = rdr.GetString(ColumnsCompanyAdressGet.LocationProvince),
                                        Active = rdr.GetBoolean(ColumnsCompanyAdressGet.Active)
                                    });
                                }
                            }
                        }
                        catch(Exception ex)
                        {

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

            return new ReadOnlyCollection<CompanyPostalAddress>(res);
        }

        public static ActionResult Activate(long id, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyAddress_Activate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(id);
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
            else
            {
                res.SetFail("No instance");
            }

            return res;
        }

        public static ActionResult Inactivate(long id, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyAddress_Inactivate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(id);
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
            else
            {
                res.SetFail("No instance");
            }

            return res;
        }

        public static ActionResult SetMain(long id, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Core_CompanyAddress_SetMain"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(id);
                        }
                        catch(Exception ex)
                        {
                            res.SetFail(ex);
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
            else
            {
                res.SetFail("No instance");
            }

            return res;
        }

        public static ActionResult SetBilling(long id, long companyId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyAddress_SetBilling"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(id);
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
            else
            {
                res.SetFail("No instance");
            }

            return res;
        }

        public ActionResult Insert(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyAddress_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        /* CREATE PROCEDURE Core_CompanyAddress_Insert
                         *   @Id bigint output,
                         *   @CompanyId bigint,
                         *   @LocationWayTypeId bigint,
                         *   @LocationAddress nvarchar(150),
                         *   @LocationAddressComplement nvarchar(100),
                         *   @LocationBloc nchar(10),
                         *   @LocationStreetNumber nchar(10),
                         *   @LocationStairs nchar(10),
                         *   @LocationStage nchar(10),
                         *   @LocationDoor nchar(10),
                         *   @LocationPostalCodeId bigint,
                         *   @Description nvarchar(50),
                         *   @Main bit,
                         *   @Billing bit,
                         *   @ApplicationUserId bigint */
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@LocationWayTypeId", this.WayTypeId));
                        cmd.Parameters.Add(DataParameter.Input("@LocationAddress", this.Address, 150));
                        cmd.Parameters.Add(DataParameter.Input("@LocationAddressComplement", string.Empty, 100));
                        cmd.Parameters.Add(DataParameter.Input("@LocationBloc", string.Empty, 10));
                        cmd.Parameters.Add(DataParameter.Input("@LocationStreetNumber", string.Empty, 10));
                        cmd.Parameters.Add(DataParameter.Input("@LocationStairs", string.Empty, 10));
                        cmd.Parameters.Add(DataParameter.Input("@LocationStage", string.Empty, 10));
                        cmd.Parameters.Add(DataParameter.Input("@LocationDoor", string.Empty, 10));
                        cmd.Parameters.Add(DataParameter.Input("@LocationPostalCodeId", this.PostalCodeId));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Main", this.Main));
                        cmd.Parameters.Add(DataParameter.Input("@Billing", this.Billing));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(this.Id);
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
            else
            {
                res.SetFail("No instance");
            }

            return res;
        }

        public ActionResult Update(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_CompanyAddress_Update"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@LocationWayTypeId", this.WayTypeId));
                        cmd.Parameters.Add(DataParameter.Input("@LocationAddress", this.Address, 150));
                        cmd.Parameters.Add(DataParameter.Input("@LocationAddressComplement", string.Empty, 100));
                        cmd.Parameters.Add(DataParameter.Input("@LocationBloc", string.Empty, 10));
                        cmd.Parameters.Add(DataParameter.Input("@LocationStreetNumber", string.Empty, 10));
                        cmd.Parameters.Add(DataParameter.Input("@LocationStairs", string.Empty, 10));
                        cmd.Parameters.Add(DataParameter.Input("@LocationStage", string.Empty, 10));
                        cmd.Parameters.Add(DataParameter.Input("@LocationDoor", string.Empty, 10));
                        cmd.Parameters.Add(DataParameter.Input("@LocationPostalCodeId", this.PostalCodeId));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Main", this.Main));
                        cmd.Parameters.Add(DataParameter.Input("@Billing", this.Billing));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(this.Id);
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
            else
            {
                res.SetFail("No instance");
            }

            return res;
        }
    }
}