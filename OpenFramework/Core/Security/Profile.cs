// --------------------------------
// <copyright file="SecurityConfiguration.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Security
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Xml.Serialization;
    using OpenFrameworkV3.CommonData;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Bindings;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Enums;
    using OpenFrameworkV3.Tools;

    /// <summary>Implements user profile class</summary>
    [Serializable]
    public class Profile
    {
        /// <summary>Gets an empty instance of user profile object</summary>
        public static Profile Empty
        {
            get
            {
                return new Profile
                {
                    ApplicationUserId = Constant.DefaultId,
                    Name = string.Empty,
                    LastName = string.Empty,
                    LastName2 = string.Empty,
                    Phone = string.Empty,
                    Mobile = string.Empty,
                    PhoneEmergency = string.Empty,
                    Web = string.Empty,
                    EmailAlternative = string.Empty,
                    BirthDate = null,
                    Facebook = string.Empty,
                    Twitter = string.Empty,
                    Instagram = string.Empty,
                    LinkedIn = string.Empty,
                    Fax = string.Empty,
                    Gender = 0,
                    IdentificationCard = string.Empty,
                    IMEI = string.Empty,
                    Address = PostalAddress.Empty,
                    DataText1 = string.Empty,
                    DataText2 = string.Empty,
                    DataText3 = string.Empty,
                    DataText4 = string.Empty
                };
            }
        }

        /// <summary>Gets the fixed profile for OpenFramework system user</summary>
        public static Profile OpenFrameworkProfile
        {
            get
            {
                return new Profile
                {
                    ApplicationUserId = 1,
                    Name = "Open",
                    LastName = "Framework",
                    LastName2 = string.Empty,
                    Phone = string.Empty,
                    Mobile = string.Empty,
                    PhoneEmergency = string.Empty,
                    EmailAlternative = string.Empty,
                    Web = "https://www.openframework.es",
                    BirthDate = null,
                    Facebook = string.Empty,
                    Twitter = string.Empty,
                    Instagram = string.Empty,
                    LinkedIn = string.Empty,
                    Fax = string.Empty,
                    Gender = 2,
                    IdentificationCard = string.Empty,
                    IMEI = string.Empty,
                    Address = PostalAddress.Empty,
                    DataText1 = string.Empty,
                    DataText2 = string.Empty,
                    DataText3 = string.Empty,
                    DataText4 = string.Empty
                };
            }
        }

        /// <summary>Gets the fixed profile for anonymous mail land page user</summary>
        public static Profile AnonymousMailLandPageProfile
        {
            get
            {
                return new Profile
                {
                    ApplicationUserId = 1,
                    Name = "Anonymous",
                    LastName = string.Empty,
                    LastName2 = string.Empty,
                    Phone = string.Empty,
                    Mobile = string.Empty,
                    PhoneEmergency = string.Empty,
                    EmailAlternative = string.Empty,
                    Web = "https://www.openframework.es",
                    BirthDate = null,
                    Facebook = string.Empty,
                    Twitter = string.Empty,
                    Instagram = string.Empty,
                    LinkedIn = string.Empty,
                    Fax = string.Empty,
                    Gender = -1,
                    IdentificationCard = string.Empty,
                    IMEI = string.Empty,
                    Address = PostalAddress.Empty,
                    DataText1 = string.Empty,
                    DataText2 = string.Empty,
                    DataText3 = string.Empty,
                    DataText4 = string.Empty
                };
            }
        }

        /// <summary>Gets or sets the user identifier</summary>
        [XmlElement(Type = typeof(long), ElementName = "ApplicationUserId")]
        public long ApplicationUserId { get; set; }

        /// <summary>Gets or sets the user name</summary>
        [XmlElement(Type = typeof(string), ElementName = "Name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the user lastname</summary>
        [XmlElement(Type = typeof(string), ElementName = "LastName")]
        public string LastName { get; set; }

        /// <summary>Gets or sets the user secondary lastname</summary>
        [XmlElement(Type = typeof(string), ElementName = "LastName2")]
        public string LastName2 { get; set; }

        /// <summary>Gets or sets the user alternative email</summary>
        [XmlElement(Type = typeof(string), ElementName = "IdentificationCard")]
        public string IdentificationCard { get; set; }

        /// <summary>Gets or sets the user alternative email</summary>
        [XmlElement(Type = typeof(string), ElementName = "EmailAlternative")]
        public string EmailAlternative { get; set; }

        /// <summary>Gets or sets the user alternative email</summary>
        [XmlElement(Type = typeof(string), ElementName = "Mobile")]
        public string Mobile { get; set; }

        /// <summary>Gets or sets the user alternative email</summary>
        [XmlElement(Type = typeof(string), ElementName = "Fax")]
        public string Fax { get; set; }

        /// <summary>Gets or sets the user alternative email</summary>
        [XmlElement(Type = typeof(int), ElementName = "Gender")]
        public int Gender { get; set; }

        /// <summary>Gets or sets the user alternative email</summary>
        [XmlElement(Type = typeof(int), ElementName = "Nacionality")]
        public int Nacionality { get; set; }

        /// <summary>Gets or sets the user web page</summary>
        [XmlElement(Type = typeof(string), ElementName = "Web")]
        public string Web { get; set; }

        /// <summary>Gets or sets an extra data text</summary>
        [XmlElement(Type = typeof(string), ElementName = "DataText1")]
        public string DataText1 { get; set; }

        /// <summary>Gets or sets an extra data text</summary>
        [XmlElement(Type = typeof(string), ElementName = "DataText2")]
        public string DataText2 { get; set; }

        /// <summary>Gets or sets an extra data text</summary>
        [XmlElement(Type = typeof(string), ElementName = "DataText3")]
        public string DataText3 { get; set; }

        /// <summary>Gets or sets an extra data text</summary>
        [XmlElement(Type = typeof(string), ElementName = "DataText4")]
        public string DataText4 { get; set; }

        /// <summary>Gets full name of user</summary>
        public string FullName
        {
            get
            {
                var res = this.Name;
                if(!string.IsNullOrEmpty(this.LastName))
                {
                    if(!string.IsNullOrEmpty(res))
                    {
                        res += " ";
                    }
                }

                res += this.LastName;

                if(!string.IsNullOrEmpty(this.LastName2))
                {
                    if(!string.IsNullOrEmpty(res))
                    {
                        res += " ";
                    }
                }

                res += this.LastName2;
                return res.Trim();
            }
        }

        /// <summary>Gets the name file of avatar image</summary>
        public string Avatar(string instanceName)
        {
            var path = Instance.Path.Data(instanceName);

            path = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}{1}Core_Users\\{2}\\",
                path,
                path.EndsWith("\\", StringComparison.OrdinalIgnoreCase) ? string.Empty : "\\",
                this.ApplicationUserId);

            Basics.VerifyFolder(path);

            path = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}Avatar.jpg",
                path);

            if(File.Exists(path))
            {
                var pos = path.IndexOf(Instance.Actual.Name);
                path = path.Substring(pos);
                return "/Instances/" + Basics.PathToUrl(path);
            }

            switch(this.Gender)
            {
                case 1:
                    return "/img/avatar_female.png";
                case 0:
                    return "/img/avatar_male.png";
                default:
                    return "/img/avatar.png";
            }
        }

        /// <summary>Gets the name file of avatar image</summary>
        public string AvatarPath(string instanceName)
        {
            var path = Instance.Path.Data(instanceName);

            path = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}{1}Core_Users\\{2}\\",
                path,
                path.EndsWith("\\", StringComparison.OrdinalIgnoreCase) ? string.Empty : "\\",
                this.ApplicationUserId);

            Basics.VerifyFolder(path);

            path = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}\\Avatar.jpg",
                path);

            if(File.Exists(path))
            {
                return Basics.PathToUrl(path);
            }

            switch(this.Gender)
            {
                case 1:
                    return "/img/avatar_female.png";
                case 0:
                    return "/img/avatar_male.png";
                default:
                    return "/img/avatar.png";
            }
        }

        public string SignaturePathPdf(string instanceName)
        {
            return SignaturePath(instanceName).Replace(".png", ".jpg");
        }

        public string SignaturePath(string instanceName)
        {
            var dataPath = Instance.Path.Data(instanceName);
            var path = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}Core_Users\\{1}\\Signature.png",
                dataPath,
                this.ApplicationUserId).Replace("\\\\", "\\");

            if(!File.Exists(path))
            {

                path = string.Format(
                    CultureInfo.InvariantCulture,
                    @"{0}Core_Users\\{1}\\Signature.jpg",
                    dataPath,
                    this.ApplicationUserId).Replace("\\\\", "\\");
            }

            return path;
        }

        /// <summary>Gets the name file of avatar image</summary>
        public string Signature(string instanceName)
        {
            var path = SignaturePath(instanceName);
            if(File.Exists(path))
            {
                var pos = path.IndexOf(instanceName);
                path = path.Substring(pos);
                return "/Instances/" + Basics.PathToUrl(path);
            }

            return "/img/NoSignature.png";
        }

        /// <summary>Gets or sets the user phone</summary>
        [XmlElement(Type = typeof(string), ElementName = "Phone")]
        public string Phone { get; set; }

        /// <summary>Gets or sets the user mobile IMEI</summary>
        [XmlElement(Type = typeof(string), ElementName = "IMEI")]
        public string IMEI { get; set; }

        /// <summary>Gets or sets the user phone</summary>
        [XmlElement(Type = typeof(string), ElementName = "PhoneEmergency")]
        public string PhoneEmergency { get; set; }

        /// <summary>Gets or sets the user Twitter's profile address</summary>
        [XmlElement(Type = typeof(string), ElementName = "Twitter")]
        public string Twitter { get; set; }

        /// <summary>Gets or sets the user Facebook's profile address</summary>
        [XmlElement(Type = typeof(string), ElementName = "Facebook")]
        public string Facebook { get; set; }

        /// <summary>Gets or sets the user LinkedIn's profile address</summary>
        [XmlElement(Type = typeof(string), ElementName = "LinkedIn")]
        public string LinkedIn { get; set; }

        /// <summary>Gets or sets the user Instagram's profile address</summary>
        [XmlElement(Type = typeof(string), ElementName = "Instragram")]
        public string Instagram { get; set; }

        /// <summary>Gets or sets the user birthdate</summary>
        [XmlElement(Type = typeof(DateTime?), ElementName = "BirthDate")]
        public DateTime? BirthDate { get; set; }

        /// <summary>Gets or sets the user postal address</summary>
        [XmlElement(Type = typeof(PostalAddress), ElementName = "Address")]
        public PostalAddress Address { get; set; }

        /// <summary>Gets a JSON structure of user profile</summary>
        public string JsonSimple
        {
            get
            {
                string pattern = @"{{
                        ""ApplicationUserId"":{0},
                        ""Name"":""{1}"",
                        ""LastName"":""{2}"",
                        ""LastName2"":""{3}"",
                        ""FullName"":""{4}""
                    }}";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.ApplicationUserId,
                    Tools.Json.JsonCompliant(this.Name),
                    Tools.Json.JsonCompliant(this.LastName),
                    Tools.Json.JsonCompliant(this.LastName2),
                    Tools.Json.JsonCompliant(this.FullName));
            }
        }

        /// <summary>Gets a JSON structure of user profile</summary>
        public string Json
        {
            get
            {
                string pattern = @"{{
                        ""ApplicationUserId"":{0},
                        ""Name"":{1},
                        ""LastName"":{2},
                        ""LastName2"":{3},
                        ""FullName"":{4},
                        ""Phone"":{5},
                        ""Mobile"":{6},
                        ""Fax"":{7},
                        ""IMEI"":{7},
                        ""EmailAlternative"":{8},
                        ""IdentificationCard"":{9},
                        ""Nacionality"":{10},
                        ""Gender"":{11}
                    }}";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.ApplicationUserId,
                    Tools.Json.JsonValue(this.Name),
                    Tools.Json.JsonValue(this.LastName),
                    Tools.Json.JsonValue(this.LastName2),
                    Tools.Json.JsonValue(this.FullName),
                    Tools.Json.JsonValue(this.Phone),
                    Tools.Json.JsonValue(this.Mobile),
                    Tools.Json.JsonValue(this.IMEI),
                    Tools.Json.JsonValue(this.EmailAlternative),
                    Tools.Json.JsonValue(this.IdentificationCard),
                    this.Nacionality,
                    this.Gender);
            }
        }

        /// <summary>Obtains profile of application user</summary>
        /// <param name="applicationUserId">Application user identifier</param>
        /// <param name="companyId">Company user identifier</param>
        /// <param name="instanceName">Name of actual instance</param>
        /// <returns>Profile of application user</returns>
        public static Profile ByApplicationUserId(long applicationUserId, long companyId, string instanceName)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "Profile.ByApplicationUserId({0},{1})", applicationUserId, instanceName);
            var res = Profile.Empty;
            res.ApplicationUserId = applicationUserId;

            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                /* CREATE PROCEDURE Core_Profile_ByApplicationUserId
                 *   @ApplicationUserId bigint */
                using (var cmd = new SqlCommand("Core_Profile_ByApplicationUserId"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    res.Name = rdr.GetString(ColumnsProfileByApplicationUserId.Name);
                                    res.LastName = rdr.GetString(ColumnsProfileByApplicationUserId.LastName);
                                    res.LastName2 = rdr.GetString(ColumnsProfileByApplicationUserId.LastName2);

                                    res.EmailAlternative = rdr.GetString(ColumnsProfileByApplicationUserId.EmailAlternative);
                                    res.Web = rdr.GetString(ColumnsProfileByApplicationUserId.Web);

                                    res.Phone = rdr.GetString(ColumnsProfileByApplicationUserId.Phone);
                                    res.Mobile = rdr.GetString(ColumnsProfileByApplicationUserId.Mobile);
                                    res.IMEI = rdr.GetString(ColumnsProfileByApplicationUserId.IMEI);
                                    res.Fax = rdr.GetString(ColumnsProfileByApplicationUserId.Fax);
                                    res.PhoneEmergency = rdr.GetString(ColumnsProfileByApplicationUserId.PhoneEmergency);

                                    if (!rdr.IsDBNull(ColumnsProfileByApplicationUserId.Gender))
                                    {
                                        res.Gender = rdr.GetInt32(ColumnsProfileByApplicationUserId.Gender);
                                    }

                                    if (!rdr.IsDBNull(ColumnsProfileByApplicationUserId.IdentificationCard))
                                    {
                                        res.IdentificationCard = rdr.GetString(ColumnsProfileByApplicationUserId.IdentificationCard);
                                    }

                                    if (!rdr.IsDBNull(ColumnsProfileByApplicationUserId.BirthDate))
                                    {
                                        res.BirthDate = rdr.GetDateTime(ColumnsProfileByApplicationUserId.BirthDate);
                                    }

                                    res.Twitter = rdr.GetString(ColumnsProfileByApplicationUserId.Twitter);
                                    res.Instagram = rdr.GetString(ColumnsProfileByApplicationUserId.Instagram);
                                    res.Facebook = rdr.GetString(ColumnsProfileByApplicationUserId.Facebook);
                                    res.LinkedIn = rdr.GetString(ColumnsProfileByApplicationUserId.LinkedIn);

                                    res.Address.WayTypeId = rdr.GetInt32(ColumnsProfileByApplicationUserId.AddressWayType);
                                    res.Address.Address = rdr.GetString(ColumnsProfileByApplicationUserId.Address);
                                    res.Address.PostalCode = rdr.GetString(ColumnsProfileByApplicationUserId.PostalCode);
                                    res.Address.City = rdr.GetString(ColumnsProfileByApplicationUserId.City);
                                    res.Address.Province = rdr.GetString(ColumnsProfileByApplicationUserId.Province);
                                    res.Address.State = rdr.GetString(ColumnsProfileByApplicationUserId.State);
                                    res.Address.Country = rdr.GetString(ColumnsProfileByApplicationUserId.Country);

                                    if (!rdr.IsDBNull(ColumnsProfileByApplicationUserId.Latitude))
                                    {
                                        res.Address.Latitude = rdr.GetDecimal(ColumnsProfileByApplicationUserId.Latitude);
                                    }

                                    if (!rdr.IsDBNull(ColumnsProfileByApplicationUserId.Longitude))
                                    {
                                        res.Address.Longitude = rdr.GetDecimal(ColumnsProfileByApplicationUserId.Longitude);
                                    }
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
                        catch (NullReferenceException ex)
                        {
                            ExceptionManager.Trace(ex as Exception, source);
                        }
                        catch (NotSupportedException ex)
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
            }

            return res;
        }

        /// <summary>Save user profile into database</summary>
        /// <param name="connectionString">Connection string to database</param>
        /// <returns>Result of action</returns>
        public ActionResult Save(string connectionString)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_Profile_Update
             *   @ApplicationUserId bigint,
             *   @Name nvarchar(50),
             *   @LastName nvarchar(50),
             *   @LastName2 nvarchar(50),
             *   @Phone nvarchar(20),
             *   @Mobile nvarchar(20),
             *   @PhoneEmergency nvarchar(20),
             *   @Fax nvarchar(20),
             *   @EmailAlternative nvarchar(100),
             *   @Web nvarchar(200),
             *   @Twiter nvarchar(50),
             *   @LinkeIn nvarchar(50),
             *   @Instagram nvarchar(50),
             *   @Facebook nvarchar(50),
             *   @Gender int,
             *   @IdentificationCard nvarchar(15),
             *   @IMEI nvarchar(20),
             *   @BirthDate datetime,
             *   @AddressWayType int,
             *   @Address char(100),
             *   @PostalCode nvarchar(10),
             *   @City nvarchar(50),
             *   @Province nvarchar(50),
             *   @State nvarchar(50),
             *   @Country nvarchar(50),
             *   @Latitude decimal(18,8),
             *   @Longitude decimal (18,8) */
            using(var cmd = new SqlCommand("Core_Profile_Update"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using(var cnn = new SqlConnection(connectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", this.ApplicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@Name", this.Name, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Lastname", this.LastName, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Lastname2", this.LastName2, 50));

                        cmd.Parameters.Add(DataParameter.Input("@Gender", (int)this.Gender));
                        cmd.Parameters.Add(DataParameter.Input("@BirthDate", this.BirthDate));
                        cmd.Parameters.Add(DataParameter.Input("@IdentificationCard", this.IdentificationCard, 15));

                        cmd.Parameters.Add(DataParameter.Input("@Twiter", this.Twitter, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Facebook", this.Facebook, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Instagram", this.Instagram, 50));
                        cmd.Parameters.Add(DataParameter.Input("@LinkedIn", this.LinkedIn, 50));

                        cmd.Parameters.Add(DataParameter.Input("@Phone", this.Phone, 20));
                        cmd.Parameters.Add(DataParameter.Input("@Mobile", this.Mobile, 20));
                        cmd.Parameters.Add(DataParameter.Input("@IMEI", this.IMEI, 20));
                        cmd.Parameters.Add(DataParameter.Input("@Fax", this.Fax, 20));
                        cmd.Parameters.Add(DataParameter.Input("@PhoneEmergency", this.PhoneEmergency, 20));
                        cmd.Parameters.Add(DataParameter.Input("@EmailAlternative", this.EmailAlternative, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Web", this.Web, 200));

                        if(this.Address == null)
                        {
                            this.Address = PostalAddress.Empty;
                        }

                        cmd.Parameters.Add(DataParameter.Input("@AddressWayType", this.Address.WayType));
                        cmd.Parameters.Add(DataParameter.Input("@Address", this.Address.Address, 100));
                        cmd.Parameters.Add(DataParameter.Input("@PostalCode", this.Address.PostalCode, 10));
                        cmd.Parameters.Add(DataParameter.Input("@City", this.Address.City, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Province", this.Address.Province, 50));
                        cmd.Parameters.Add(DataParameter.Input("@State", this.Address.State, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Country", this.Address.Country, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Latitude", this.Address.Latitude));
                        cmd.Parameters.Add(DataParameter.Input("@Longitude", this.Address.Longitude));

                        // Extra data
                        // ------------------------------------------------------------------------
                        cmd.Parameters.Add(DataParameter.Input("@DataText1", this.DataText1, 50));
                        cmd.Parameters.Add(DataParameter.Input("@DataText2", this.DataText2, 50));
                        cmd.Parameters.Add(DataParameter.Input("@DataText3", this.DataText3, 50));
                        cmd.Parameters.Add(DataParameter.Input("@DataText4", this.DataText4, 50));
                        // ------------------------------------------------------------------------

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch(SqlException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch(NullReferenceException ex)
                    {
                        res.SetFail(ex);
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

            return res;
        }
    }
}