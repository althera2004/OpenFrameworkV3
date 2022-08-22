// --------------------------------
// <copyright file="BankAccount.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
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

    public partial class BankAccount
    {
        public const long ItemGrantId = 1020;
        public const string ItemGrantCode = "B";
        public const string ItemGrantName = "BankAccount";

        [XmlElement(Type = typeof(long), ElementName = "Id")]
        public long Id { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "ItemDefinitionId")]
        public long ItemDefinitionId { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "ItemId")]
        public long ItemId { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "IBAN")]
        public string IBAN { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "SWIFT")]
        public string SWIFT { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "BankName")]
        public string BankName { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Alias")]
        public string Alias { get; set; }

        [XmlElement(Type = typeof(bool), ElementName = "Main")]
        public bool Main { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "ContractId")]
        public string ContractId { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "PaymentType")]
        public string PaymentType { get; set; }

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

        public static BankAccount Empty
        {
            get
            {
                return new BankAccount
                {
                    Id = Constant.DefaultId,
                    ItemDefinitionId = Constant.DefaultId,
                    ItemId = Constant.DefaultId,
                    IBAN = string.Empty,
                    SWIFT = string.Empty,
                    BankName = string.Empty,
                    Main = false,
                    ContractId = string.Empty,
                    PaymentType = string.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public static string JsonList(ReadOnlyCollection<BankAccount> list)
        {
            if(list == null)
            {
                return Tools.Json.EmptyJsonList;
            }

            var first = true;
            var res = new StringBuilder("[");
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

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"":{0},
                        ""ItemDefinitionId"":{1},
                        ""ItemId"":{2},
                        ""IBAN"":""{3}"",
                        ""SWIFT"":""{4}"",
                        ""BankName"":""{5}"",
                        ""Alias"":""{6}"",
                        ""Main"":{7},
                        ""ContractId"":""{8}"",
                        ""PaymentType"":""{9}"",
                        ""CreatedBy"":{10},
                        ""CreatedOn"":""{11:dd/MM/yyyy}"",
                        ""ModifiedBy"":{12},
                        ""ModifiedOn"":""{13:dd/MM/yyyy}"",
                        ""Active"":{14}
                    }}",
                    this.Id,
                    this.ItemDefinitionId,
                    this.ItemId,
                    Tools.Json.JsonCompliant(this.IBAN),
                    Tools.Json.JsonCompliant(this.SWIFT),
                    Tools.Json.JsonCompliant(this.BankName),
                    Tools.Json.JsonCompliant(this.Alias),
                    ConstantValue.Value(this.Main),
                    Tools.Json.JsonCompliant(this.ContractId).Trim(),
                    Tools.Json.JsonCompliant(this.PaymentType),
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static ActionResult Activate(long id, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Feature_BankAccount_Activate"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
                        }
                        catch(Exception ex)
                        {
                            ExceptionManager.Trace(ex, string.Format(CultureInfo.InvariantCulture, "Feature_BankAccount::Inativate({0},{1}", id, applicationUserId));
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

        public static ActionResult Inactivate(long id, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Feature_BankAccount_Inactivate"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
                        }
                        catch(Exception ex)
                        {
                            ExceptionManager.Trace(ex, string.Format(CultureInfo.InvariantCulture, "Feature_BankAccount::Inativate({0},{1}", id, applicationUserId));
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

        public ActionResult SetMain(bool main, long applicationUserId, string instanceName)
        {
            return SetMain(this.Id, main, applicationUserId, instanceName);
        }

        public static ActionResult SetMain(long id, bool main, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Feature_BankAccount_SetMain"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@Main", main));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
                        }
                        catch(Exception ex)
                        {
                            ExceptionManager.Trace(ex, string.Format(CultureInfo.InvariantCulture, "Feature_BankAccount::Inativate({0},{1}", id, applicationUserId));
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

        public static ReadOnlyCollection<BankAccount> ByItemId(long itemDefinitionId, long itemId, string instanceName)
        {
            var res = new List<BankAccount>();
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                /* CREATE PROCEDURE Feature_BankAccount_ByItemId
                 *   @ItemDefinitionId bigint,
                 *   @ItemId bigint */
                using(var cmd = new SqlCommand("Feature_BankAccount_ByItemId"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                while(rdr.Read())
                                {
                                    res.Add(new BankAccount
                                    {
                                        Id = rdr.GetInt64(ColumnsBankAccountGet.Id),
                                        ItemDefinitionId = rdr.GetInt64(ColumnsBankAccountGet.ItemDefinitionId),
                                        ItemId = rdr.GetInt64(ColumnsBankAccountGet.ItemId),
                                        IBAN = rdr.GetString(ColumnsBankAccountGet.IBAN).ToUpperInvariant().Trim(),
                                        SWIFT = rdr.GetString(ColumnsBankAccountGet.Swift).ToUpperInvariant().Trim(),
                                        BankName = rdr.GetString(ColumnsBankAccountGet.BankName).Trim(),
                                        Alias = rdr.GetString(ColumnsBankAccountGet.Alias).Trim(),
                                        ContractId = rdr.GetString(ColumnsBankAccountGet.ContractId).Trim(),
                                        PaymentType = rdr.GetString(ColumnsBankAccountGet.PaymentType).ToUpperInvariant().Trim(),
                                        Main = rdr.GetBoolean(ColumnsBankAccountGet.Main),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsBankAccountGet.CreatedBy),
                                            Profile = new UserProfile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsBankAccountGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsBankAccountGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsBankAccountGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsBankAccountGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsBankAccountGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsBankAccountGet.ModifiedBy),
                                            Profile = new UserProfile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsBankAccountGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsBankAccountGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsBankAccountGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsBankAccountGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsBankAccountGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsBankAccountGet.Active)
                                    });
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

            return new ReadOnlyCollection<BankAccount>(res);
        }

        public static ReadOnlyCollection<BankAccount> All(string instanceName)
        {
            var res = new List<BankAccount>();
            var cns = Persistence.ConnectionString(instanceName);
            if(!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Feature_BankAccount_All"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                while(rdr.Read())
                                {
                                    res.Add(new BankAccount
                                    {
                                        Id = rdr.GetInt64(ColumnsBankAccountGet.Id),
                                        ItemDefinitionId = rdr.GetInt64(ColumnsBankAccountGet.ItemDefinitionId),
                                        ItemId = rdr.GetInt64(ColumnsBankAccountGet.ItemId),
                                        IBAN = rdr.GetString(ColumnsBankAccountGet.IBAN).ToUpperInvariant().Trim(),
                                        SWIFT = rdr.GetString(ColumnsBankAccountGet.Swift).ToUpperInvariant().Trim(),
                                        BankName = rdr.GetString(ColumnsBankAccountGet.BankName).Trim(),
                                        Alias = rdr.GetString(ColumnsBankAccountGet.Alias).Trim(),
                                        ContractId = rdr.GetString(ColumnsBankAccountGet.ContractId).Trim(),
                                        PaymentType = rdr.GetString(ColumnsBankAccountGet.PaymentType).ToUpperInvariant().Trim(),
                                        Main = rdr.GetBoolean(ColumnsBankAccountGet.Main),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsBankAccountGet.CreatedBy),
                                            Profile = new UserProfile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsBankAccountGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsBankAccountGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsBankAccountGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsBankAccountGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsBankAccountGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsBankAccountGet.ModifiedBy),
                                            Profile = new UserProfile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsBankAccountGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsBankAccountGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsBankAccountGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsBankAccountGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsBankAccountGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsBankAccountGet.Active)
                                    });
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

            return new ReadOnlyCollection<BankAccount>(res);
        }

        public ActionResult Save(long applicationUserId, string instanceName)
        {
            if(this.Id > 0)
            {
                return this.Update(applicationUserId, instanceName);
            }

            return this.Insert(applicationUserId, instanceName);
        }

        private ActionResult Update(long applicationUserId, string instanceName)
        {

            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            /* CREATE PROCEDURE [dbo].[Feature_BankAccount_Update]
             *   @Id bigint ,
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @IBAN nchar(40),
             *   @Swift nchar(40),
             *   @BankName nvarchar(50),
             *   @Alias nvarchar(50),
             *   @Main bit,
             *   @ContractId nvarchar(50),
             *   @PaymentType nchar(4),
             *   @ApplicationUserId bigint */
            using (var cmd = new SqlCommand("Feature_BankAccount_Update"))
            {
                using (var cnn = new SqlConnection(cns))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                    cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                    cmd.Parameters.Add(DataParameter.Input("@IBAN", this.IBAN, 40));
                    cmd.Parameters.Add(DataParameter.Input("@Swift", this.SWIFT, 40));
                    cmd.Parameters.Add(DataParameter.Input("@BankName", this.BankName, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Alias", this.Alias));
                    cmd.Parameters.Add(DataParameter.Input("@Main", this.Main));
                    cmd.Parameters.Add(DataParameter.Input("@ContractId", this.ContractId));
                    cmd.Parameters.Add(DataParameter.Input("@PaymentType", this.PaymentType));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, "UPDATE|{0}", this.Id));
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

            return res;
        }

        private ActionResult Insert(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            /* CREATE PROCEDURE [dbo].[Feature_BankAccount_Insert]
             *   @Id bigint output,
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @IBAN nchar(40),
             *   @Swift nchar(40),
             *   @BankName nvarchar(50),
             *   @Alias nvarchar(50),
             *   @Main bit,
             *   @ContractId nvarchar(50),
             *   @PaymentType nchar(4),
             *   @ApplicationUserId bigint */
            using (var cmd = new SqlCommand("Feature_BankAccount_Insert"))
            {
                using (var cnn = new SqlConnection(cns))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                    cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                    cmd.Parameters.Add(DataParameter.Input("@IBAN", this.IBAN, 40));
                    cmd.Parameters.Add(DataParameter.Input("@Swift", this.SWIFT, 40));
                    cmd.Parameters.Add(DataParameter.Input("@BankName", this.BankName, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Alias", this.Alias));
                    cmd.Parameters.Add(DataParameter.Input("@Main", this.Main));
                    cmd.Parameters.Add(DataParameter.Input("@ContractId", this.ContractId));
                    cmd.Parameters.Add(DataParameter.Input("@PaymentType", this.PaymentType));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                        res.SetSuccess(string.Format(CultureInfo.InvariantCulture, "INSERT|{0}", this.Id));
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

            return res;
        }
    }
}