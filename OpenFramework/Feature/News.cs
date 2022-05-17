// --------------------------------
// <copyright file="News.cs" company="OpenFramework">
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
    public partial class News
    {
        public const long ItemGrantId = 1001;
        public const string ItemGrantCode = "N";
        public const string ItemGrantName = "News";

        [XmlElement(Type = typeof(long), ElementName = "Id")]
        public long Id { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "CompanyId")]
        public long CompanyId { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Title")]
        public string Title { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Subtitle")]
        public string Subtitle { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Abstract")]
        public string Abstract { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Text")]
        public string Text { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Media")]
        public string Media { get; set; }

        [XmlElement(Type = typeof(int), ElementName = "MediaType")]
        public int MediaType { get; set; }

        [XmlElement(Type = typeof(string), ElementName = "Publisher")]
        public string Publisher { get; set; }

        [XmlElement(Type = typeof(bool), ElementName = "Published")]
        public bool Published { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "ItemDefinitionId")]
        public long ItemDefinitionId { get; set; }

        [XmlElement(Type = typeof(long), ElementName = "ItemId")]
        public long ItemId { get; set; }

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

        public News()
        {
        }

        public static News Empty
        {
            get
            {
                return new News
                {
                    Id = Constant.DefaultId,
                    CompanyId = Constant.DefaultId,
                    Title = string.Empty,
                    Subtitle = string.Empty,
                    Abstract = string.Empty,
                    Text = string.Empty,
                    Media = string.Empty,
                    MediaType = 0,
                    ItemDefinitionId = Constant.DefaultId,
                    ItemId = Constant.DefaultId,
                    Publisher = string.Empty,
                    Published = false,
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
                        ""Title"":""{2}"",
                        ""Substitle"":""{3}"",
                        ""Abstract"":""{4}"",
                        ""Text"":""{5}"",
                        ""Media"":""{6}"",
                        ""MediaType"":{7},
                        ""ItemDefinitionId"":{8},
                        ""ItemId"":{9},
                        ""Published"":{10},
                        ""Publisher"":""{11}"",
                        ""CreatedBy"":{12},
                        ""CreatedOn"":""{13:dd/MM/yyyy}"",
                        ""ModifiedBy"":{14},
                        ""ModifiedOn"":""{15:dd/MM/yyyy}"",
                        ""Active"":{16}
                    }}",
                    this.Id,
                    this.CompanyId,
                    Tools.Json.JsonCompliant(this.Title),
                    Tools.Json.JsonCompliant(this.Subtitle),
                    Tools.Json.JsonCompliant(this.Abstract),
                    Tools.Json.JsonCompliant(this.Text),
                    Tools.Json.JsonCompliant(this.Media),
                    this.MediaType,
                    this.ItemDefinitionId,
                    this.ItemId,
                    ConstantValue.Value(this.Published),
                    Tools.Json.JsonCompliant(this.Publisher),
                    this.CreatedBy.JsonSimple,
                    this.CreatedOn,
                    this.ModifiedBy.JsonSimple,
                    this.ModifiedOn,
                    ConstantValue.Value(this.Active));
            }
        }

        public static string JsonList(ReadOnlyCollection<News> list)
        {
            var res = new StringBuilder("[");
            if (list != null && list.Count > 0)
            {
                bool first = true;
                foreach (var news in list)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(news.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<News> All(long companyId, string instanceName)
        {
            var res = new List<News>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_News_GetAll"))
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
                                    var newNews = new News
                                    {
                                        Id = rdr.GetInt64(ColumnsNewsGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsNewsGet.CompanyId),
                                        ItemDefinitionId = Constant.DefaultId,
                                        ItemId = Constant.DefaultId,
                                        Title = rdr.GetString(ColumnsNewsGet.Title),
                                        Subtitle = rdr.GetString(ColumnsNewsGet.Subtitle),
                                        Abstract = rdr.GetString(ColumnsNewsGet.Abstract),
                                        Media = rdr.GetString(ColumnsNewsGet.Media),
                                        MediaType = rdr.GetInt32(ColumnsNewsGet.MediaType),
                                        Publisher = rdr.GetString(ColumnsNewsGet.Publisher),
                                        Published = rdr.GetBoolean(ColumnsNewsGet.Published),
                                        Active = rdr.GetBoolean(ColumnsNewsGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsNewsGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsNewsGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsNewsGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsNewsGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsNewsGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsNewsGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsNewsGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsNewsGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsNewsGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsNewsGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsNewsGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsNewsGet.ModifiedOn)
                                    };

                                    if (!rdr.IsDBNull(ColumnsNewsGet.ItemDefinitionId))
                                    {
                                        newNews.ItemDefinitionId = rdr.GetInt64(ColumnsNewsGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsNewsGet.ItemId))
                                    {
                                        newNews.ItemId = rdr.GetInt64(ColumnsNewsGet.ItemId);
                                    }

                                    res.Add(newNews);
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

            return new ReadOnlyCollection<News>(res);
        }

        public static ReadOnlyCollection<News> ByItemId(long itemDefinitionId, long itemId, long companyId, string instanceName)
        {
            var res = new List<News>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_News_GetByItemId"))
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
                                    var newNews = new News
                                    {
                                        Id = rdr.GetInt64(ColumnsNewsGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsNewsGet.CompanyId),
                                        ItemDefinitionId = Constant.DefaultId,
                                        ItemId = Constant.DefaultId,
                                        Title = rdr.GetString(ColumnsNewsGet.Title),
                                        Subtitle = rdr.GetString(ColumnsNewsGet.Subtitle),
                                        Text = rdr.GetString(ColumnsNewsGet.Text),
                                        Media = rdr.GetString(ColumnsNewsGet.Media),
                                        MediaType = rdr.GetInt32(ColumnsNewsGet.MediaType),
                                        Abstract = rdr.GetString(ColumnsNewsGet.Abstract),
                                        Published = rdr.GetBoolean(ColumnsNewsGet.Published),
                                        Publisher = rdr.GetString(ColumnsNewsGet.Publisher),
                                        Active = rdr.GetBoolean(ColumnsNewsGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsNewsGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsNewsGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsNewsGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsNewsGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsNewsGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsNewsGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsNewsGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsNewsGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsNewsGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsNewsGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsNewsGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsNewsGet.ModifiedOn)
                                    };

                                    if (!rdr.IsDBNull(ColumnsNewsGet.ItemDefinitionId))
                                    {
                                        newNews.ItemDefinitionId = rdr.GetInt64(ColumnsNewsGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsNewsGet.ItemId))
                                    {
                                        newNews.ItemId = rdr.GetInt64(ColumnsNewsGet.ItemId);
                                    }

                                    res.Add(newNews);
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

            return new ReadOnlyCollection<News>(res);
        }

        public static ReadOnlyCollection<News> ByItemDefinitionId(long itemDefinitionId, long companyId, string instanceName)
        {
            var res = new List<News>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_News_GetbyItemDefinitionId"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    var newNews = new News
                                    {
                                        Id = rdr.GetInt64(ColumnsNewsGet.Id),
                                        CompanyId = rdr.GetInt64(ColumnsNewsGet.CompanyId),
                                        ItemDefinitionId = Constant.DefaultId,
                                        ItemId = Constant.DefaultId,
                                        Title = rdr.GetString(ColumnsNewsGet.Title),
                                        Subtitle = rdr.GetString(ColumnsNewsGet.Subtitle),
                                        Abstract = rdr.GetString(ColumnsNewsGet.Abstract),
                                        Text = rdr.GetString(ColumnsNewsGet.Text),
                                        Publisher = rdr.GetString(ColumnsNewsGet.Publisher),
                                        Published = rdr.GetBoolean(ColumnsNewsGet.Published),
                                        Media = rdr.GetString(ColumnsNewsGet.Media),
                                        MediaType = rdr.GetInt32(ColumnsNewsGet.MediaType),
                                        Active = rdr.GetBoolean(ColumnsNewsGet.Active),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsNewsGet.CreatedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsNewsGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsNewsGet.CreatedByName),
                                                LastName = rdr.GetString(ColumnsNewsGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsNewsGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsNewsGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsNewsGet.ModifiedBy),
                                            Profile = new Profile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsNewsGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsNewsGet.ModifiedByName),
                                                LastName = rdr.GetString(ColumnsNewsGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsNewsGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsNewsGet.ModifiedOn)
                                    };

                                    if (!rdr.IsDBNull(ColumnsNewsGet.ItemDefinitionId))
                                    {
                                        newNews.ItemDefinitionId = rdr.GetInt64(ColumnsNewsGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsNewsGet.ItemId))
                                    {
                                        newNews.ItemId = rdr.GetInt64(ColumnsNewsGet.ItemId);
                                    }

                                    res.Add(newNews);
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

            return new ReadOnlyCollection<News>(res);
        }

        public static News ById(long id, long companyId, string instanceName)
        {
            var res = News.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_News_GetById"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@NewsId", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Id = rdr.GetInt64(ColumnsNewsGet.Id);
                                    res.CompanyId = rdr.GetInt64(ColumnsNewsGet.CompanyId);
                                    res.ItemDefinitionId = Constant.DefaultId;
                                    res.ItemId = Constant.DefaultId;
                                    res.Title = rdr.GetString(ColumnsNewsGet.Title);
                                    res.Subtitle = rdr.GetString(ColumnsNewsGet.Subtitle);
                                    res.Abstract = rdr.GetString(ColumnsNewsGet.Subtitle);
                                    res.Text = rdr.GetString(ColumnsNewsGet.Subtitle);
                                    res.Media = rdr.GetString(ColumnsNewsGet.Media);
                                    res.MediaType = rdr.GetInt32(ColumnsNewsGet.MediaType);
                                    res.Publisher = rdr.GetString(ColumnsNewsGet.Publisher);
                                    res.Published = rdr.GetBoolean(ColumnsNewsGet.Published);
                                    res.Active = rdr.GetBoolean(ColumnsNewsGet.Active);
                                    res.CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsNewsGet.CreatedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsNewsGet.CreatedBy),
                                            Name = rdr.GetString(ColumnsNewsGet.CreatedByName),
                                            LastName = rdr.GetString(ColumnsNewsGet.CreatedByLastName),
                                            LastName2 = rdr.GetString(ColumnsNewsGet.CreatedByLastName2)
                                        }
                                    };
                                    res.CreatedOn = rdr.GetDateTime(ColumnsNewsGet.CreatedOn);
                                    res.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt64(ColumnsNewsGet.ModifiedBy),
                                        Profile = new Profile
                                        {
                                            ApplicationUserId = rdr.GetInt64(ColumnsNewsGet.ModifiedBy),
                                            Name = rdr.GetString(ColumnsNewsGet.ModifiedByName),
                                            LastName = rdr.GetString(ColumnsNewsGet.ModifiedByLastName),
                                            LastName2 = rdr.GetString(ColumnsNewsGet.ModifiedByLastName2)
                                        }
                                    };
                                    res.ModifiedOn = rdr.GetDateTime(ColumnsNewsGet.ModifiedOn);

                                    if (!rdr.IsDBNull(ColumnsNewsGet.ItemDefinitionId))
                                    {
                                        res.ItemDefinitionId = rdr.GetInt64(ColumnsNewsGet.ItemDefinitionId);
                                    }

                                    if (!rdr.IsDBNull(ColumnsNewsGet.ItemId))
                                    {
                                        res.ItemId = rdr.GetInt64(ColumnsNewsGet.ItemId);
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
            var source = string.Format(CultureInfo.InvariantCulture, @"FAQ::insert ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_News_Insert]
             *   @NewsId bigint output,
             *   @CompanyId bigint,
             *   @Title nvarchar(50),
             *   @Subtitle nvarchar(100),
             *   @Abstract nvarchar(500),
             *   @Text text,
             *   @Publisher nvarchar(100),
             *   @Published bit,
             *   @PublishedFrom datetime,
             *   @PublishedTo datetime,
             *   @Media nvarchar(200),
             *   @MediaType int,
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_News_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Title", this.Title, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Subtitle", this.Subtitle, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Abstract", this.Abstract, 500));
                        cmd.Parameters.Add(DataParameter.Input("@Text", this.Text, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@Media", this.Media, 200));
                        cmd.Parameters.Add(DataParameter.Input("@MediaType", this.MediaType));
                        cmd.Parameters.Add(DataParameter.Input("@Publisher", this.Publisher, 100));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@Published", this.Published));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
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
            var source = string.Format(CultureInfo.InvariantCulture, @"News::update ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_News_Update]
             *   @NewsId bigint,
             *   @CompanyId bigint,
             *   @Title nvarchar(50),
             *   @Subtitle nvarchar(100),
             *   @Abstract nvarchar(500),
             *   @Text text,
             *   @Publisher nvarchar(100),
             *   @Published bit,
             *   @PublishedFrom datetime,
             *   @PublishedTo datetime,
             *   @Media nvarchar(200),
             *   @MediaType int,
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_News_Update"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Title", this.Title, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Subtitle", this.Subtitle, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Abstract", this.Abstract, 500));
                        cmd.Parameters.Add(DataParameter.Input("@Text", this.Text, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@Media", this.Media, 200));
                        cmd.Parameters.Add(DataParameter.Input("@MediaType", this.MediaType));
                        cmd.Parameters.Add(DataParameter.Input("@Publisher", this.Publisher, 100));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", this.ItemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", this.ItemId));
                        cmd.Parameters.Add(DataParameter.Input("@Published", this.Published));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
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

        public ActionResult Publish(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"News::publish ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_News_Publish]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_News_Publish"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
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

        public ActionResult Unpublish(long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var source = string.Format(CultureInfo.InvariantCulture, @"News::Unpublish ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_News_Unpublish]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_News_Unpublish"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
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
            var source = string.Format(CultureInfo.InvariantCulture, @"News::Activate ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_News_Activate]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_News_Activate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
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
            var source = string.Format(CultureInfo.InvariantCulture, @"News::Inactivate ==> {0}", this.Json);
            /* CREATE PROCEDURE [dbo].[Feature_News_Inactivate]
             *   @Id bigint,
             *   @CompanyId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Feature_News_Inactivate"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
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