// --------------------------------
// <copyright file="Attach.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using OpenFrameworkV3;
using OpenFrameworkV3.Core;
using OpenFrameworkV3.Core.Activity;
using OpenFrameworkV3.Core.DataAccess;
using OpenFrameworkV3.Core.Security;
using OpenFrameworkV3.Tools;

namespace OpenFrameworkV3.Feature
{
    public class Attach
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long ItemDefinitionId { get; set; }
        public long ItemId { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public string Path(string instanceName)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                @"{0}\{1}\{2}\Attachs\{3}",

                Instance.Path.Data(instanceName),
                Persistence.ItemDefinitionById(this.ItemDefinitionId, instanceName).ItemName,
                ItemId,
                FileName
            );
        }

        

        public static Attach Empty
        {
            get
            {
                return new Attach
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    ItemDefinitionId = Constant.DefaultId,
                    ItemId = Constant.DefaultId,
                    FileName = string.Empty,
                    Size = 0,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public static string JsonList(ReadOnlyCollection<Attach> list)
        {
            var res = new StringBuilder("[");

            if (list != null && list.Count > 0)
            {
                var first = true;
                foreach (var item in list)
                {
                    res.AppendFormat(CultureInfo.InvariantCulture, "{1}{0}", item.Json, first ? string.Empty : ",");
                    first = false;
                }
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
                        ""Id"": {0},
                        ""CompanydId"": {1},
                        ""ItemDefinitionId"": {2},
                        ""ItemId"": {3},
                        ""FileName"":""{4}"",
                        ""Size"":{5},
                        ""CreatedBy"": {6},
                        ""CreatedOn"": ""{7:dd/MM/yyyy}"",
                        ""ModifiedBy"": {8},
                        ""ModifiedOn"": ""{9:dd/MM/yyyy}"",
                        ""Active"": {10}
                    }}",
                    this.Id,
                    this.CompanyId,
                    this.ItemDefinitionId,
                    this.ItemId,
                    Tools.Json.JsonCompliant(this.FileName),
                    this.Size,
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));                    
            }
        }

        public ActionResult Insert(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Feature_Attach_Insert"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@FileName", this.FileName, 150));
                        cmd.Parameters.Add(DataParameter.Input("@Size", this.Size));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(this.Id);
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

            return res;
        }

        public static Attach ById(long id, string instanceName)
        {
            var res = Attach.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_Attach_ById"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res = new Attach
                                    {
                                        Id = rdr.GetInt64(0),
                                        CompanyId = rdr.GetInt64(1),
                                        ItemDefinitionId = rdr.GetInt64(15),
                                        ItemId = rdr.GetInt64(16),
                                        FileName = rdr.GetString(2),
                                        Size = rdr.GetInt64(3),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(4),
                                            Profile = new UserProfile
                                            {
                                                ApplicationUserId = rdr.GetInt64(4),
                                                Name = rdr.GetString(5),
                                                LastName = rdr.GetString(6),
                                                LastName2 = rdr.GetString(7)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(8),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(9),
                                            Profile = new UserProfile
                                            {
                                                ApplicationUserId = rdr.GetInt64(9),
                                                Name = rdr.GetString(10),
                                                LastName = rdr.GetString(11),
                                                LastName2 = rdr.GetString(12)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(13),
                                        Active = rdr.GetBoolean(14)
                                    };
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

        public static ReadOnlyCollection<Attach> ByItem (long itemDefinitionId, long itemId, string instanceName)
        {
            var res = new List<Attach>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Feature_Attach_ByItemId"))
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
                                while (rdr.Read())
                                {
                                    res.Add(new Attach
                                    {
                                        Id = rdr.GetInt64(0),
                                        CompanyId = rdr.GetInt64(1),
                                        ItemDefinitionId = itemDefinitionId,
                                        ItemId = itemId,
                                        FileName = rdr.GetString(2),
                                        Size = rdr.GetInt64(3),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(4),
                                            Profile = new UserProfile
                                            {
                                                ApplicationUserId = rdr.GetInt64(4),
                                                Name = rdr.GetString(5),
                                                LastName = rdr.GetString(6),
                                                LastName2 = rdr.GetString(7)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(8),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(9),
                                            Profile = new UserProfile
                                            {
                                                ApplicationUserId = rdr.GetInt64(9),
                                                Name = rdr.GetString(10),
                                                LastName = rdr.GetString(11),
                                                LastName2 = rdr.GetString(12)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(13),
                                        Active = rdr.GetBoolean(14)
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

            return new ReadOnlyCollection<Attach>(res);
        }
    }
}