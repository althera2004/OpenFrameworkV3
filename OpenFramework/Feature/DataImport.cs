// --------------------------------
// <copyright file="Note.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Feature
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;

    [Serializable]
    public partial class DataImport
    {
        public const long ItemGrantId = 1008;
        public const string ItemGrantCode = "D";
        public const string ItemGrantName = "DataImport";

        [XmlElement(Type = typeof(long), ElementName = "Id")]
        public long Id { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "CompanyId")]
        public long CompanyId { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "ItemDefinitionId")]
        public long ItemDefinitionId { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "ItemId")]
        public long ItemId { get; set; }

        [XmlElement(Type = typeof(int), ElementName = "Result")]
        public long Result { get; set; }

        [XmlElement(Type = typeof(ApplicationUser), ElementName = "CreatedBy")]
        public ApplicationUser CreatedBy { get; set; }

        [XmlElement(Type = typeof(ApplicationUser), ElementName = "ModifiedBy")]
        public ApplicationUser ModifiedBy { get; set; }

        [XmlElement(Type = typeof(DateTime), ElementName = "CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [XmlElement(Type = typeof(DateTime), ElementName = "ModifiedOn")]
        public DateTime ModifiedOn { get; set; }

        [XmlElement(Type = typeof(bool), ElementName = "Active")]
        public bool Active { get; set; }

        public DataImport()
        {
        }

        public static DataImport Empty
        {
            get
            {
                return new DataImport
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    ItemDefinitionId = Constant.DefaultId,
                    ItemId = Constant.DefaultId,
                    Result = Constant.DefaultId,
                    CreatedBy = ApplicationUser.Empty,
                    ModifiedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
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
                        ""CompanyId"":{1},
                        ""ItemDefinitionId"":{2},
                        ""ItemId"":{3},
                        ""Result"":{4},
                        ""CreatedBy"":{5},
                        ""CreatedOn"":""{6:dd/MM/yyyy hh:mm}"",
                        ""ModifiedBy"":{7},
                        ""ModifiedOn"":""{8:dd/MM/yyyy}"",
                        ""Active"":{9}
                    }}",
                    this.Id,
                    this.CompanyId,
                    this.ItemDefinitionId,
                    this.ItemId,
                    this.Result,
                    this.CreatedBy.JsonSimple,
                    this.CreatedOn,
                    this.ModifiedBy.JsonSimple,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static string JsonList(ReadOnlyCollection<DataImport> list)
        {
            var res = new StringBuilder("[");
            if (list != null && list.Count > 0)
            {
                bool first = true;
                foreach (var faq in list)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(faq.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<DataImport> All(string instanceName)
        {
            var res = new List<DataImport>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {

                var user = ApplicationUser.Actual;
                using (var cmd = new SqlCommand("Feature_DataImport_GetAll"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", user.CompanyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    var newDataImport = new DataImport
                                    {
                                        Id = rdr.GetInt64(ColumnsDataImportGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsDataImportGet.CompanyId),
                                        ItemDefinitionId = Constant.DefaultId,
                                        ItemId = Constant.DefaultId,
                                        Result = Constant.DefaultId,
                                        Active = rdr.GetBoolean(ColumnsDataImportGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsDataImportGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsDataImportGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsDataImportGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsDataImportGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsDataImportGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsDataImportGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsDataImportGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsDataImportGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsDataImportGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsDataImportGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsDataImportGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsDataImportGet.ModifiedOn)
                                    };

                                    if (!rdr.IsDBNull(ColumnsDataImportGet.ItemDefinitionId))
                                    {
                                        newDataImport.ItemDefinitionId = rdr.GetInt64(ColumnsDataImportGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsDataImportGet.ItemId))
                                    {
                                        newDataImport.ItemId = rdr.GetInt64(ColumnsDataImportGet.ItemId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsDataImportGet.Result))
                                    {
                                        newDataImport.Result = rdr.GetInt64(ColumnsDataImportGet.Result);
                                    }

                                    res.Add(newDataImport);
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

            return new ReadOnlyCollection<DataImport>(res);
        }

        public static ReadOnlyCollection<DataImport> ByItemId(long itemDefinitionId, long itemId, long companyId, string instanceName)
        {
            var res = new List<DataImport>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_DataImport_GetByItemId"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    var dataImport = new DataImport
                                    {
                                        Id = rdr.GetInt64(ColumnsDataImportGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsDataImportGet.CompanyId),
                                        ItemDefinitionId = Constant.DefaultId,
                                        ItemId = Constant.DefaultId,
                                        Result = Constant.DefaultId,
                                        Active = rdr.GetBoolean(ColumnsDataImportGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsDataImportGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsDataImportGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsDataImportGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsDataImportGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsDataImportGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsDataImportGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsDataImportGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsDataImportGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsDataImportGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsDataImportGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsDataImportGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsDataImportGet.ModifiedOn)
                                    };

                                    if (!rdr.IsDBNull(ColumnsDataImportGet.ItemDefinitionId))
                                    {
                                        dataImport.ItemDefinitionId = rdr.GetInt64(ColumnsDataImportGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsDataImportGet.ItemId))
                                    {
                                        dataImport.ItemId = rdr.GetInt64(ColumnsDataImportGet.ItemId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsDataImportGet.Result))
                                    {
                                        dataImport.Result = rdr.GetInt64(ColumnsDataImportGet.Result);
                                    }

                                    res.Add(dataImport);
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

            return new ReadOnlyCollection<DataImport>(res);
        }

        public static ReadOnlyCollection<DataImport> ByItemDefinitionId(long itemDefinitionId, string instanceName)
        {
            var res = new List<DataImport>();
            var user = ApplicationUser.Actual;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_DataImport_GetByItemDefinitionId"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", user.CompanyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    var dataImport = new DataImport
                                    {
                                        Id = rdr.GetInt64(ColumnsDataImportGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsDataImportGet.CompanyId),
                                        ItemDefinitionId = Constant.DefaultId,
                                        ItemId = Constant.DefaultId,
                                        Result = Constant.DefaultId,
                                        Active = rdr.GetBoolean(ColumnsDataImportGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsDataImportGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsDataImportGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsDataImportGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsDataImportGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsDataImportGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsDataImportGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsDataImportGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsDataImportGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsDataImportGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsDataImportGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsDataImportGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsDataImportGet.ModifiedOn)
                                    };

                                    if (!rdr.IsDBNull(ColumnsDataImportGet.ItemDefinitionId))
                                    {
                                        dataImport.ItemDefinitionId = rdr.GetInt64(ColumnsDataImportGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsDataImportGet.ItemId))
                                    {
                                        dataImport.ItemId = rdr.GetInt64(ColumnsDataImportGet.ItemId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsDataImportGet.Result))
                                    {
                                        dataImport.Result = rdr.GetInt64(ColumnsDataImportGet.Result);
                                    }

                                    res.Add(dataImport);
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

            return new ReadOnlyCollection<DataImport>(res);
        }

        public static DataImport ById(long id, long companyId, string instanceName)
        {
            var res = DataImport.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_DataImport_GetById"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Id = rdr.GetInt64(ColumnsDataImportGet.Id);
                                    res.CompanyId = rdr.GetInt64(ColumnsDataImportGet.CompanyId);
                                    res.ItemDefinitionId = Constant.DefaultId;
                                    res.ItemId = Constant.DefaultId;
                                    res.Result = Constant.DefaultId;
                                    res.Active = rdr.GetBoolean(ColumnsDataImportGet.Active);
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsDataImportGet.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsDataImportGet.CreatedBy),
                                            Name = rdr.GetString(ColumnsDataImportGet.CreatedByName),
                                            LastName = rdr.GetString(ColumnsDataImportGet.CreatedByLastName),
                                            LastName2 = rdr.GetString(ColumnsDataImportGet.CreatedByLastName2)
                                        }
                                    };
                                    res.CreatedOn = rdr.GetDateTime(ColumnsDataImportGet.CreatedOn);
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsDataImportGet.ModifiedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsDataImportGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnsDataImportGet.ModifiedByName),
                                            LastName = rdr.GetString(ColumnsDataImportGet.ModifiedByLastName),
                                            LastName2 = rdr.GetString(ColumnsDataImportGet.ModifiedByLastName2)
                                        }
                                    };
                                    res.ModifiedOn = rdr.GetDateTime(ColumnsDataImportGet.ModifiedOn);

                                    if (!rdr.IsDBNull(ColumnsDataImportGet.ItemDefinitionId))
                                    {
                                        res.ItemDefinitionId = rdr.GetInt64(ColumnsDataImportGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsDataImportGet.ItemId))
                                    {
                                        res.ItemId = rdr.GetInt64(ColumnsDataImportGet.ItemId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsDataImportGet.Result))
                                    {
                                        res.Result = rdr.GetInt64(ColumnsDataImportGet.Result);
                                    }
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

        public ActionResult Insert(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"DataImport::insert ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_DataImport_Insert]
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @Result int,
             *   @ItemDefintionId bigint,
             *   @ItemId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_DataImport_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Result", this.Result));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@CreatedOn", DateTime.Now.ToUniversalTime()));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(this.Id);
                        }
                        catch (FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NotImplementedException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
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

            return res;
        }

        public ActionResult Update(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"Note::update ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_DataImport_Update]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @Result int,
             *   @ItemDefintionId bigint,
             *   @ItemId bigint,
             *   @ApplicationUserId bigint
             *   @ModifiedOn datetime */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_DataImport_Update"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Result", this.Result));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@ModifiedOn", DateTime.Now.ToUniversalTime()));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(this.Id);
                        }
                        catch (FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NotImplementedException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
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

            return res;
        }

        public ActionResult Activate(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"DataImport::Activate ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_DataImport_Activate]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint 
             *   @ModifiedOn nvarchar(20) */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_DataImport_Activate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.InputDateNow("@MofiedOn"));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(this.Id);
                        }
                        catch (FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NotImplementedException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
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

            return res;
        }

        public ActionResult Inactivate(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"DataImport::Inactivate ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_DataImport_Inactivate]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint
             *   @ModifiedOn nvarchar(20) */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_DataImport_Inactivate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.InputDateNow("@MofiedOn"));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(this.Id);
                        }
                        catch (FormatException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                            res.SetFail(ex);
                        }
                        catch (NotImplementedException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
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

            return res;
        }
    }
}