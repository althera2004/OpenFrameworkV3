// --------------------------------
// <copyright file="Following.cs" company="OpenFramework">
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
    using System.Linq;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.DataAccess;

    public static class Following
    {
        public static ActionResult FollowItem(long applicationUserId, int itemTypeId, long itemId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_User_ItemFollow "))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemTypeId", itemTypeId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
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

        public static ActionResult UnfollowItem(long applicationUserId, int itemTypeId, long itemId, string instanceName)
        {
            var res = ActionResult.NoAction;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_User_ItemUnfollow "))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemTypeId", itemTypeId));
                        cmd.Parameters.Add(DataParameter.Input("@ItemId", itemId));
                        try
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            res.SetSuccess();
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

        public static ReadOnlyCollection<FollowedItem> AddFollowedItem(long itemTypeId, long itemId, ReadOnlyCollection<FollowedItem> items)
        {
            var followedItem = new FollowedItem
            {
                ItemTypeId = itemTypeId,
                ItemId = itemId
            };

            followedItem.FullData();

            var res = new List<FollowedItem>();
            if(items != null)
            {
                res = items.ToList();
            }

            res.Add(followedItem);
            return new ReadOnlyCollection<FollowedItem>(res);
        }

        public static ReadOnlyCollection<FollowedItem> RemoveFollowedItem(long itemTypeId, long itemId, ReadOnlyCollection<FollowedItem> items)
        {
            var res = new List<FollowedItem>();
            if(items != null)
            {
                res = items.ToList();
            }

            res = res.Where(f => f.ItemId != itemId || f.ItemTypeId != itemTypeId).ToList();

            return new ReadOnlyCollection<FollowedItem>(res);
        }
    }
}