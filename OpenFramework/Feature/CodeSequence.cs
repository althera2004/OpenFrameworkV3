// --------------------------------
// <copyright file="CodeSequence.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------

namespace OpenFramework.Features
{
    using OpenFramework.Activity;
    using OpenFramework.DataAccess;
    using OpenFramework.InstanceManager;
    using OpenFramework.Security;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CodeSequence
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Prefix { get; set; }
        public string Sufix { get; set; }
        public long LastCounter { get; set; }
        public int FillNumeric { get; set; }
        public long Increment { get; set; }
        public CodeSequenceType Type { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public static CodeSequence Empty
        {
            get
            {
                return new CodeSequence
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    Description = string.Empty,
                    Notes = string.Empty,
                    Prefix = string.Empty,
                    Sufix = string.Empty,
                    LastCounter = 0,
                    FillNumeric = 0,
                    Increment = 1,
                    Type = CodeSequenceType.Numerical,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
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
                    @"{{""Id"":{0},
                    ""CompanyId"":{1},
                    ""Description"":""{2}"",
                    ""Notes"":""{3}"",
                    ""Prefix"":""{4}"",
                    ""Sufix"":""{5}"",
                    ""LastCounter"":{6},
                    ""FillNumeric"":{7},
                    ""Increment"":{8},
                    ""Type"":{9},
                    ""CreatedBy"":{10},
                    ""CreatedOn"":""{11:dd/MM/yyyy}"",
                    ""ModifiedBy"":{12},
                    ""ModifiedOn"":""{13:dd/MM/yyyy}"",
                    ""Active"":{14}}}",
                    this.Id,
                    this.CompanyId,
                    Tools.Json.JsonCompliant(this.Description),
                    Tools.Json.JsonCompliant(this.Notes),
                    this.Prefix,
                    this.Sufix,
                    this.LastCounter,
                    this.FillNumeric,
                    this.Increment,
                    (int)this.Type,
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public string NextValue
        {
            get
            {
                var res = string.Empty;
                var pattern = "{0:";
                for(var x = 0; x < this.FillNumeric; x++)
                {
                    pattern += "0";
                }

                pattern += "}";
                var value = string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    LastCounter += Increment);

                return this.Prefix + value + Sufix;
            }
        }

        public ActionResult Insert(long applicationUserId)
        {
            /* CREATE PROCEDURE Feature_CodeSequence_Insert
             *   @Id bigint output,
             *   @CompanyId bigint,
             *   @Description nvarchar(50),
             *   @Notes nvarchar(300),
             *   @Type int,
             *   @Increment bigint,
             *   @LastCounter bigint,
             *   @FillNumeric int,
             *   @Prefix nvarchar(50),
             *   @Sufix nvarchar(50),
             *   @ApplicationUserId bigint */
            var res = ActionResult.NoAction;
            var instance = CustomerFramework.Actual;
            using (var cmd = new SqlCommand("Feature_CodeSequence_Insert"))
            {
                using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, 300));
                    cmd.Parameters.Add(DataParameter.Input("@Type", (int)this.Type));
                    cmd.Parameters.Add(DataParameter.Input("@Increment", this.Increment));
                    cmd.Parameters.Add(DataParameter.Input("@LastCounter", this.LastCounter));
                    cmd.Parameters.Add(DataParameter.Input("@FillNumeric", this.FillNumeric));
                    cmd.Parameters.Add(DataParameter.Input("@Prefix", this.Prefix, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Sufix", this.Sufix, 50));
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


            return res;
        }

        public ActionResult Update(long applicationUserId)
        {
            /* CREATE PROCEDURE Feature_CodeSequence_Update
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @Description nvarchar(50),
             *   @Notes nvarchar(300),
             *   @Type int,
             *   @Increment bigint,
             *   @LastCounter bigint,
             *   @FillNumeric int,
             *   @Prefix nvarchar(50),
             *   @Sufix nvarchar(50),
             *   @ApplicationUserId bigint */
            var res = ActionResult.NoAction;
            var instance = CustomerFramework.Actual;
            using (var cmd = new SqlCommand("Feature_CodeSequence_Update"))
            {
                using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, 300));
                    cmd.Parameters.Add(DataParameter.Input("@Type", (int)this.Type));
                    cmd.Parameters.Add(DataParameter.Input("@Increment", this.Increment));
                    cmd.Parameters.Add(DataParameter.Input("@LastCounter", this.LastCounter));
                    cmd.Parameters.Add(DataParameter.Input("@FillNumeric", this.FillNumeric));
                    cmd.Parameters.Add(DataParameter.Input("@Prefix", this.Prefix, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Sufix", this.Sufix, 50));
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


            return res;
        }

        public static ActionResult Activate(long codeSequenceId, long companyId, long applicationUserId)
        {
            /* CREATE PROCEDURE Feature_CodeSequence_Activate
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            var res = ActionResult.NoAction;
            var instance = CustomerFramework.Actual;
            using (var cmd = new SqlCommand("Feature_CodeSequence_Activate"))
            {
                using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", codeSequenceId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(Convert.ToInt64(cmd.Parameters["@Id"].Value));
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


            return res;
        }

        public static ActionResult Inactivate(long codeSequenceId, long companyId, long applicationUserId)
        {
            /* CREATE PROCEDURE Feature_CodeSequence_Activate
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            var res = ActionResult.NoAction;
            var instance = CustomerFramework.Actual;
            using (var cmd = new SqlCommand("Feature_CodeSequence_Inactivate"))
            {
                using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", codeSequenceId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(Convert.ToInt64(cmd.Parameters["@Id"].Value));
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


            return res;
        }
    }
}