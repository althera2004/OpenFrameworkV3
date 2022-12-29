// --------------------------------
// <copyright file="DynamicJsonObject.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Xml.Serialization;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;
    using static OpenFrameworkV3.Core.Bindings.Laguage;

    [Serializable]
    public sealed partial class Language
    {
        /// <summary>Gets a simple empty language object</summary>
        public static Language SimpleEmpty
        {
            get
            {
                return new Language
                {
                    Id = 0,
                    Name = string.Empty,
                    LocaleName = string.Empty,
                    Iso = string.Empty,
                    Active = false
                };
            }
        }

        /// <summary> Gets an empty language object</summary>
        public static Language Empty
        {
            get
            {
                return new Language
                {
                    Id = 0,
                    Name = string.Empty,
                    LocaleName = string.Empty,
                    Iso = string.Empty,
                    RightToLeft = false,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        /// <summary> Gets an empty language object</summary>
        public static Language Default
        {
            get
            {
                return new Language
                {
                    Id = 1,
                    Name = "Castellano",
                    LocaleName = "Castellano",
                    Iso = "es-es",
                    RightToLeft = false,
                    CreatedBy = ApplicationUser.OpenFramework,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.OpenFramework,
                    ModifiedOn = DateTime.Now,
                    Active = true
                };
            }
        }

        /// <summary>Gets all available languages</summary>
        public static ReadOnlyCollection<Language> All(string instanceName)
        {
            string source = "Language::All";
            var res = new List<Language>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Language_GetAll"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Add(new Language
                                    {
                                        Id = rdr.GetInt64(ColumnsLanguageGet.Id),
                                        Name = rdr.GetString(ColumnsLanguageGet.Name),
                                        LocaleName = rdr.GetString(ColumnsLanguageGet.LocaleName),
                                        Iso = rdr.GetString(ColumnsLanguageGet.Iso),
                                        RightToLeft = rdr.GetBoolean(ColumnsLanguageGet.RightToLeft),
                                        CreatedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsLanguageGet.CreatedBy),
                                            Profile = new UserProfile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsLanguageGet.CreatedBy),
                                                Name = rdr.GetString(ColumnsLanguageGet.CreatedByFirstName),
                                                LastName = rdr.GetString(ColumnsLanguageGet.CreatedByLastName),
                                                LastName2 = rdr.GetString(ColumnsLanguageGet.CreatedByLastName2)
                                            }
                                        },
                                        CreatedOn = rdr.GetDateTime(ColumnsLanguageGet.CreatedOn),
                                        ModifiedBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt64(ColumnsLanguageGet.ModifiedBy),
                                            Profile = new UserProfile
                                            {
                                                ApplicationUserId = rdr.GetInt64(ColumnsLanguageGet.ModifiedBy),
                                                Name = rdr.GetString(ColumnsLanguageGet.ModifiedByFirstName),
                                                LastName = rdr.GetString(ColumnsLanguageGet.ModifiedByLastName),
                                                LastName2 = rdr.GetString(ColumnsLanguageGet.ModifiedByLastName2)
                                            }
                                        },
                                        ModifiedOn = rdr.GetDateTime(ColumnsLanguageGet.ModifiedOn),
                                        Active = rdr.GetBoolean(ColumnsLanguageGet.Active)
                                    });
                                }
                            }
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (NotSupportedException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                    }
                }
            }

            return new ReadOnlyCollection<Language>(res);
        }

        /// <summary>Gets language ISO code</summary>
        public string JavaScriptIso
        {
            get
            {
                if (string.IsNullOrEmpty(this.Iso))
                {
                    return string.Empty;
                }

                return this.Iso.Split('-')[0].Trim().ToLowerInvariant();
            }
        }

        /// <summary> Gets or sets language identifier</summary>
        [XmlElement(Type = typeof(long), ElementName = "Id")]
        public long Id { get; set; }

        /// <summary>Gets or sets language name</summary>
        [XmlElement(Type = typeof(string), ElementName = "Name")]
        public string Name { get; set; }

        /// <summary>Gets or sets locale language name</summary>
        [XmlElement(Type = typeof(string), ElementName = "LocaleName")]
        public string LocaleName { get; set; }

        /// <summary>Gets or sets language ISO code</summary>
        [XmlElement(Type = typeof(string), ElementName = "Iso")]
        public string Iso { get; set; }

        /// <summary>Gets or sets a value indicating whether direction of writing language</summary>
        [XmlElement(Type = typeof(bool), ElementName = "RightToLeft")]
        public bool RightToLeft { get; set; }

        /// <summary>Gets or sets a value indicating whether language is active or not</summary>
        [XmlElement(Type = typeof(bool), ElementName = "Active")]
        public bool Active { get; set; }

        /// <summary>Gets user that created language</summary>
        [XmlElement(Type = typeof(ApplicationUser), ElementName = "CreatedBy")]
        public ApplicationUser CreatedBy { get; private set; }

        /// <summary>Gets user that make last modification of language</summary>
        [XmlElement(Type = typeof(ApplicationUser), ElementName = "ModifiedBy")]
        public ApplicationUser ModifiedBy { get; private set; }

        /// <summary>Gets date of creation</summary>
        [XmlElement(Type = typeof(DateTime), ElementName = "CreatedOn")]
        public DateTime CreatedOn { get; private set; }

        /// <summary>Gets date of last modification</summary>
        [XmlElement(Type = typeof(DateTime), ElementName = "ModifiedOn")]
        public DateTime ModifiedOn { get; private set; }

        /// <summary>Gets basic structure JSON of language data</summary>
        public string JsonKeyValue
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"{{""Id"":{0},""Value"":""{1}""}}", this.Id, this.Name);
            }
        }

        /// <summary>Gets structure JSON of language data</summary>
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0}, ""Name"":""{1}"", ""LocaleName"":""{2}"", ""ISO"":""{3}"", ""JavaScriptISO"": ""{4}"", ""RightToLeft"":{5},  ""Active"": {6}}}",
                    this.Id,
                    this.Name,
                    this.LocaleName,
                    this.Iso,
                    this.JavaScriptIso,
                    ConstantValue.Value(this.RightToLeft),
                    ConstantValue.Value(this.Active));
            }
        }

        /// <summary>Obtains language by code</summary>
        /// <param name="id">Identifier of language</param>
        /// <returns>Application language</returns>
        public static Language ById(long id, string instanceName)
        {
            string source = string.Format(CultureInfo.InvariantCulture, "Language::ById({0})", id);
            var res = Language.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Language_GetById"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@LanguageId", id));
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Id = rdr.GetInt64(ColumnsLanguageGet.Id);
                                    res.Name = rdr.GetString(ColumnsLanguageGet.Name);
                                    res.LocaleName = rdr.GetString(ColumnsLanguageGet.LocaleName);
                                    res.Iso = rdr.GetString(ColumnsLanguageGet.Iso);
                                    res.RightToLeft = rdr.GetBoolean(ColumnsLanguageGet.RightToLeft);
                                    res.ModifiedOn = rdr.GetDateTime(ColumnsLanguageGet.ModifiedOn);
                                    res.Active = rdr.GetBoolean(ColumnsLanguageGet.Active);
                                }
                            }
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (NotSupportedException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                    }
                }
            }

            return res;
        }

        /// <summary>Obtains language by code</summary>
        /// <param name="code">Code of language</param>
        /// <returns>Application languega</returns>
        public static Language ByCode(string code, string instanceName)
        {
            string source = string.Format(CultureInfo.InvariantCulture, "Language::ByCode({0})", code);
            var res = Language.Empty;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_Language_GetByCode"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res.Id = rdr.GetInt64(ColumnsLanguageGet.Id);
                                    res.Name = rdr.GetString(ColumnsLanguageGet.Name);
                                    res.LocaleName = rdr.GetString(ColumnsLanguageGet.LocaleName);
                                    res.Iso = rdr.GetString(ColumnsLanguageGet.Iso);
                                    res.RightToLeft = rdr.GetBoolean(ColumnsLanguageGet.RightToLeft);
                                    res.ModifiedOn = rdr.GetDateTime(ColumnsLanguageGet.ModifiedOn);
                                    res.Active = rdr.GetBoolean(ColumnsLanguageGet.Active);
                                }
                            }
                        }
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (NotSupportedException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                    }
                }
            }

            return res;
        }
    }
}