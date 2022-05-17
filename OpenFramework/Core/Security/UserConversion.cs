// --------------------------------
// <copyright file="SecurityConfiguration.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Security
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Script.Serialization;
    using Newtonsoft.Json;
    using OpenFrameworkV3;
    using OpenFrameworkV3.Core;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;

    public class UserConversion
    {
        [JsonProperty("ProfileConversion")]
        private ProfileConversion[] profileConversion;

        [JsonProperty("ItemDefinitionId")]
        public long ItemDefinitionId { get; set; }

        [JsonProperty("UserType")]
        public string UserType { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("UserMustExists")]
        public bool UserMustExists { get; set; }

        [JsonIgnore]
        public string Json
        {
            get
            {
                var profile = new StringBuilder("[");
                bool first = true;
                foreach (var profileFields in this.profileConversion)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        profile.Append(",");
                    }

                    profile.Append(profileFields.Json);
                }

                profile.Append("]");

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""ItemDefinitionId"":{0},""UserType"":""{1}"",""UserMustExists"":{2},""Email"":""{3}"",""ProfileConversion"":{4}}}",
                    this.ItemDefinitionId,
                    this.UserType,
                    ConstantValue.Value(this.UserMustExists),
                    this.Email,
                    profile);
            }
        }

        [JsonIgnore]
        public ReadOnlyCollection<ProfileConversion> ProfileConversions
        {
            get
            {
                if (this.profileConversion == null)
                {
                    this.profileConversion = new List<ProfileConversion>().ToArray();
                }

                return new ReadOnlyCollection<ProfileConversion>(this.profileConversion);
            }
        }

        public void AddFieldConversion(string fieldFrom, string fieldProfile)
        {

            if (this.profileConversion == null)
            {
                this.profileConversion = new List<ProfileConversion>().ToArray();
            }

            var temp = this.profileConversion.ToList();
            temp.Add(new ProfileConversion {
                FieldFrom = fieldFrom,
                FieldProfile = fieldProfile
            });

            this.profileConversion = temp.ToArray();
        }

        public void AddFieldConversion(ProfileConversion fields)
        {
            if (this.profileConversion == null)
            {
                this.profileConversion = new List<ProfileConversion>().ToArray();
            }

            var temp = this.profileConversion.ToList();
            temp.Add(fields);

            this.profileConversion = temp.ToArray();
        }

        public static void Load(string instanceName)
        {
            if (HttpContext.Current.Session != null)
            {
                var fileName = string.Format("{0}\\Users.Convert", Instance.Path.Base(instanceName));
                if (File.Exists(fileName))
                {
                    using (StreamReader input = new StreamReader(fileName))
                    {
                        var json = input.ReadToEnd();
                        var serializer = new JavaScriptSerializer();
                        serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                        dynamic data = serializer.Deserialize(json, typeof(object));
                        var list = new List<UserConversion>();

                        foreach (var conversion in data)
                        {
                            var newConversion = new UserConversion
                            {
                                ItemDefinitionId = (long)conversion["ItemDefinitionId"],
                                UserType = conversion["UserType"] as string,
                                UserMustExists = (bool)conversion["UserMustExists"],
                                Email = conversion["Email"] as string
                            };

                            foreach (var fieldConversion in conversion["ProfileConversion"])
                            {
                                newConversion.AddFieldConversion(fieldConversion["FieldFrom"] as string, fieldConversion["FieldProfile"] as string);
                            }

                            list.Add(newConversion);
                        }

                        HttpContext.Current.Session["UserConversions"] = list;
                    }
                }
            }
        }

        public static ActionResult SaveConversion(long itemDefinitionId, long itemId, long userId, long applicationUserId, string instanceName)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Core_UserConvsersion_Insert
             *   @Id bigint output,
             *   @ItemDefinitionId bigint,
             *   @ItemId bigint,
             *   @UserId bigint,
             *   @ApplicationUserId bigint */
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_UserConvsersion_Insert"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess(Convert.ToInt64(cmd.Parameters["@Id"].Value));
                        }
                        catch (SqlException ex)
                        {
                            res.SetFail(ex);
                        }
                        catch (NullReferenceException ex)
                        {
                            res.SetFail(ex);
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

        public static ActionResult ItemHasUser(long itemDefinitionId, long itemId, string instanceName)
        {
            var res = new ActionResult
            {
                MessageError = string.Empty,
                Success = true,
                ReturnValue = Constant.JavaScriptNull
            };

            if (HttpContext.Current.Session["UserConversions"] is List<UserConversion> userConversions && userConversions.Count > 0)
            {
                if (userConversions.Any(uc => uc.ItemDefinitionId == itemDefinitionId))
                {
                    /* CREATE PROCEDURE Core_UserConversion_ItemHasUser
                     *   @ItemDefinitionId bigint,
                     *   @ItemId bigint,
                     *   @UserId bigint output */
                    var cns = Persistence.ConnectionString(instanceName);
                    if (!string.IsNullOrEmpty(cns))
                    {
                        using (var cmd = new SqlCommand("Core_UserConversion_ItemHasUser"))
                        {
                            using (var cnn = new SqlConnection(cns))
                            {
                                cmd.Connection = cnn;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(DataParameter.Input("@ItemDefinitionId", itemDefinitionId));
                                cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                                cmd.Parameters.Add(DataParameter.OutputLong("@UserId"));
                                try
                                {
                                    cmd.Connection.Open();
                                    cmd.ExecuteNonQuery();
                                    if (cmd.Parameters["@UserId"].Value == DBNull.Value)
                                    {
                                        res.SetSuccess(Constant.JavaScriptNull);
                                    }
                                    else
                                    {
                                        res.SetSuccess(cmd.Parameters["@UserId"].Value.ToString());
                                    }
                                }
                                catch (SqlException ex)
                                {
                                    res.SetFail(ex);
                                }
                                catch (NullReferenceException ex)
                                {
                                    res.SetFail(ex);
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
                }
            }

            return res;
        }
    }
}