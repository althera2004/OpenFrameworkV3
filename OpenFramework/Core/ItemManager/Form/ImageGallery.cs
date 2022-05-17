// --------------------------------
// <copyright file="ImageGallery.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace OpenFrameworkV3.Core.ItemManager.ItemForm
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.ItemManager.ItemForm.Binding;
    using OpenFrameworkV3.Core.Security;

    public class ImageGallery
    {
        public long Id { get; set; }
        public long ItemTypeId { get; set; }
        public long ItemId { get; set; }
        public string GalleryId { get; set; }
        public int GalleryIndex { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }

        public static ImageGallery Empty
        {
            get
            {
                return new ImageGallery
                {
                    Id = Constant.DefaultId,
                    ItemTypeId = Constant.DefaultId,
                    ItemId = Constant.DefaultId,
                    GalleryId = string.Empty,
                    GalleryIndex = 0,
                    Description = string.Empty,
                    ImageFile = string.Empty,
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
                    @"{{""Id"": {0},
                        ""ItemTypeId"": {1},
                        ""ItemId"": {2},
                        ""GalleryId"": ""{3}"",
                        ""GalleryIndex"": {4},
                        ""Description"": ""{5}"",
                        ""ImageFile"": ""{6}"",
                        ""CreatedBy"": {7},
                        ""CreatedOn"": ""{8:dd/MM/yyyy}"",
                        ""ModifiedBy"": {9},
                        ""ModifiedOn"": ""{10:dd/MM/yyyy}"",
                        ""Active"": {11}}}",
                    this.Id,
                    this.ItemTypeId,
                    this.ItemId,
                    this.GalleryId,
                    this.GalleryIndex,
                    Tools.Json.JsonCompliant(this.Description),
                    this.ImageFile,
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static string JsonList(ReadOnlyCollection<ImageGallery> list)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach(var image in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(image.Json);
            }

            return res.Append("]").ToString();
        }

        public static ActionResult Delete(long id, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_ImageGallery_Delete
             *   @Id bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_ImageGallery_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
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

            return res;
        }

        public static ReadOnlyCollection<ImageGallery> ByItemId(long itemTypeId, long itemId, string instanceName)
        {
            var res = new List<ImageGallery>();
            /* CREATE PROCEDURE Core_ImageGallery_ByItem
             *   @ItemTypeId bigint,
             *   @ItemId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_ImageGallery_ByItem"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        try
                        {
                            cmd.Parameters.Add(DataParameter.Input("@ItemTypeId", itemTypeId));
                            cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new ImageGallery
                                    {
                                        Id = rdr.GetInt64(ColumnsImageGalleryGet.Id),
                                        ItemTypeId = itemTypeId,
                                        ItemId = itemId,
                                        GalleryId = rdr.GetString(ColumnsImageGalleryGet.GalleryId),
                                        GalleryIndex = rdr.GetInt32(ColumnsImageGalleryGet.GalleryIndex),
                                        Description = rdr.GetString(ColumnsImageGalleryGet.Description),
                                        ImageFile = rdr.GetString(ColumnsImageGalleryGet.ImageFile),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsImageGalleryGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsImageGalleryGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsImageGalleryGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsImageGalleryGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsImageGalleryGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsImageGalleryGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsImageGalleryGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsImageGalleryGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsImageGalleryGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsImageGalleryGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsImageGalleryGet.ModifiedLastName2)
                                            }
                                        },
                                        Active = rdr.GetBoolean(ColumnsImageGalleryGet.Active)
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
            }

            return new ReadOnlyCollection<ImageGallery>(res);
        }

        public ActionResult Insert(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_ImageGallery_Insert
             *   @Id bigint output,
             *   @ItemTypeId bigint,
             *   @ItemId bigint,
             *   @GalleryId nvarchar(50),
             *   @GalleryIndex int,
             *   @Description nvarchar(100),
             *   @ImageFile nvarchar(100),
             *   @ApplictionUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_ImageGallery_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@ItemTypeId", this.ItemTypeId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@GalleryId", this.GalleryId, 100));
                        cmd.Parameters.Add(DataParameter.Input("@GalleryIndex", this.GalleryIndex));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@ImageFile", this.ImageFile, 100));
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

            return res;
        }
    }
}