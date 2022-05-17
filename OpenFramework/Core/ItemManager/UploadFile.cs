﻿// --------------------------------
// <copyright file="UploadFile.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Bindings;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;

    public class UploadFile
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public int ItemLinked { get; set; }
        public long ItemId { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public static UploadFile Empty
        {
            get
            {
                return new UploadFile
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    ItemLinked = 0,
                    ItemId = Constant.DefaultId,
                    Description = string.Empty,
                    Extension = string.Empty,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""Description"":""{1}"",""Active"":{2}}}",
                    this.Id,
                    Tools.Json.JsonCompliant(this.Description),
                    this.Active ? "true" : "false");
            }
        }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"": {0},
                        ""CompanyId"": {1},
                        ""ItemLinked"": {2},
                        ""ItemId"": {3},
                        ""Description"": ""{4}"",
                        ""Extension"": ""{5}"",
                        ""FileName"": ""{6}"",
                        ""CreatedBy"": {7},
                        ""CreatedOn"": ""{8:dd/MM/yyyy}"",
                        ""ModifiedBy"": {9},
                        ""ModifiedOn"": ""{10:dd/MM/yyyy}"",
                        ""Active"": {11},
                        ""Size"": {12}
                    }}",
                       this.Id,
                       this.CompanyId,
                       this.ItemLinked,
                       this.ItemId,
                       Tools.Json.JsonCompliant(this.Description),
                       this.Extension,
                       this.FileName,
                       this.CreatedBy.JsonKeyValue,
                       this.CreatedOn,
                       this.ModifiedBy.JsonKeyValue,
                       this.ModifiedOn,
                       this.Active ? "true" : "false",
                       this.Size);
            }
        }

        public static ReadOnlyCollection<UploadFile> GetByItem(int itemLinked, long itemId, int companyId, string instanceName)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                "UploadFile.GetByItem(itemLinked:{0}, itemId:{1}, companyId{2})",
                itemLinked,
                itemId,
                companyId);

            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}DOCS\{1}\", path, companyId);
            var res = new List<UploadFile>();

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("UploadFiles_GetByItem"))
                {
                    try
                    {
                        cmd.Connection = new SqlConnection(cns);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ItemLinked", itemLinked));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var attach = new UploadFile
                                {
                                    Id = rdr.GetInt64(ColumnsUploadFileGet.Id),
                                    ItemLinked = rdr.GetInt32(ColumnsUploadFileGet.ItemLinked),
                                    ItemId = rdr.GetInt64(ColumnsUploadFileGet.ItemId),
                                    CompanyId = Convert.ToInt32(ColumnsUploadFileGet.CompanyId),
                                    FileName = rdr.GetString(ColumnsUploadFileGet.FileName),
                                    Description = rdr.GetString(ColumnsUploadFileGet.Description),
                                    Extension = rdr.GetString(ColumnsUploadFileGet.Extension).Trim().ToUpperInvariant(),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsUploadFileGet.CreatedBy),
                                        Email = rdr.GetString(ColumnsUploadFileGet.CreatdByLogin)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsUploadFileGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsUploadFileGet.ModifiedBy),
                                        Email = rdr.GetString(ColumnsUploadFileGet.ModifiedByLogin)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsUploadFileGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsUploadFileGet.Active)
                                };

                                string fileName = string.Format(CultureInfo.InvariantCulture, @"{0}{1}", path, attach.FileName);
                                if (File.Exists(fileName))
                                {
                                    long length = new System.IO.FileInfo(fileName).Length;
                                    attach.Size = length;
                                }
                                else
                                {
                                    attach.Size = 0;
                                    attach.Description = Path.GetFileName(fileName);
                                    attach.Extension = "nofile";
                                }

                                res.Add(attach);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (InvalidCastException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
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

            return new ReadOnlyCollection<UploadFile>(res);
        }

        public static bool HasAttachemnts(long itemLinked, long itemId, long companyId, string instanceName)
        {
            string source = string.Format(CultureInfo.InvariantCulture, "UploadFile.HasAttachemnts(itemLinked:{0}, itemId:{1}, companyId{2})", itemLinked, itemId, companyId);
            bool res = false;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("UploadFiles_GetByItem"))
                {
                    try
                    {
                        cmd.Connection = new SqlConnection(cns);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ItemLinked", itemLinked));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                res = true;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (InvalidCastException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
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

        public static UploadFile GetById(long id, int companyId, string instanceName)
        {
            string source = string.Format(CultureInfo.InvariantCulture, "UploadFile.GetByItem(id:{0}, companyId{1})", id, companyId);
            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}DOCS\{1}\", path, companyId);
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("UploadFiles_GetById"))
                {
                    try
                    {
                        cmd.Connection = new SqlConnection(cns);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var attach = new UploadFile
                                {
                                    Id = rdr.GetInt64(ColumnsUploadFileGet.Id),
                                    ItemLinked = rdr.GetInt32(ColumnsUploadFileGet.ItemLinked),
                                    ItemId = rdr.GetInt64(ColumnsUploadFileGet.ItemId),
                                    CompanyId = Convert.ToInt32(ColumnsUploadFileGet.CompanyId),
                                    FileName = rdr.GetString(ColumnsUploadFileGet.FileName),
                                    Description = rdr.GetString(ColumnsUploadFileGet.Description),
                                    Extension = rdr.GetString(ColumnsUploadFileGet.Extension).Trim().ToUpperInvariant(),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsUploadFileGet.CreatedBy),
                                        Email = rdr.GetString(ColumnsUploadFileGet.CreatdByLogin)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsUploadFileGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsUploadFileGet.ModifiedBy),
                                        Email = rdr.GetString(ColumnsUploadFileGet.ModifiedByLogin)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsUploadFileGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsUploadFileGet.Active)
                                };

                                long length = 0;
                                string finalPath = string.Format(CultureInfo.InvariantCulture, @"{0}{1}", path, attach.FileName);
                                if (File.Exists(finalPath))
                                {
                                    length = new System.IO.FileInfo(finalPath).Length;
                                }

                                attach.Size = length;

                                return attach;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                    }
                    catch (InvalidCastException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
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

            return UploadFile.Empty;
        }

        public ActionResult Insert(int applicationUserId, string instanceName)
        {
            string source = string.Format(CultureInfo.InvariantCulture, "UploadFile.Insert(ApplicationUserId:{0}", applicationUserId);
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("UploadFiles_Insert"))
                {
                    /* CREATE PROCEDURE UploadFiles_Insert
                     *   @Id bigint output,
                     *   @CompanyId bigint,
                     *   @ItemLinked int,
                     *   @ItemId bigint,
                     *   @FileName nvarchar(250),
                     *   @Description nvarchar(100),
                     *   @Extension nvarchar(10),
                     *   @ApplicationUserId int */
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@ItemLinked", this.ItemLinked));
                    cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                    cmd.Parameters.Add(DataParameter.Input("@FileName", this.FileName, 250));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                    cmd.Parameters.Add(DataParameter.Input("@Extension", this.Extension.Trim().ToUpperInvariant(), 10));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        using (var cnn = new SqlConnection(cns))
                        {
                            cmd.Connection = cnn;
                            cnn.Open();
                            cmd.ExecuteNonQuery();
                            this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                            res.SetSuccess(this.Json);
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                        res.SetFail(ex);
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                        res.SetFail(ex);
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                        res.SetFail(ex);
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                        res.SetFail(ex);
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex as Exception, source);
                        res.SetFail(ex);
                    }
                    catch (InvalidCastException ex)
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

            return res;
        }

        public static ActionResult Delete(long attachId, int companyId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE UploadFiled_Inactive
             *   @Id bigint,
             *   @CompanyId bigint */

            var uploadFile = GetById(attachId, companyId, instanceName);
            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}DOCS\{1}\", path, companyId);

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("UploadFiled_Inactive"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", attachId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(attachId);
                            try
                            {
                                path = string.Format(CultureInfo.InvariantCulture, "{0}{1}", path, uploadFile.FileName);
                                if (File.Exists(path))
                                {
                                    File.Delete(path);
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionManager.Trace(ex as Exception, "UploadDile");
                            }
                            finally
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            res.SetFail(ex);
                        }
                    }
                }
            }

            return res;
        }
    }
}