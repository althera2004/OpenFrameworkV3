// --------------------------------
// <copyright file="ScopeView.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Core.Security
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using OpenFrameworkV3.Core.DataAccess;

    public class ScopeView
    {
        private List<Data> data;
        public ReadOnlyCollection<Data> All
        {
            get
            {
                if (this.data == null)
                {
                    return new ReadOnlyCollection<Data>(new List<Data>());
                }

                return new ReadOnlyCollection<Data>(this.data);
            }
        }

        public ReadOnlyCollection<Data> ByItemDefenitionId(long itemDefinitionId)
        {
            if (this.data == null)
            {
                return new ReadOnlyCollection<Data>(new List<Data>());
            }

            return new ReadOnlyCollection<Data>(this.data.Where(d => d.ItemDefinitionId == itemDefinitionId).ToList());
        }

        public static ReadOnlyCollection<Data> ByApplicationUser(long applicationUserId, long companyId, string instanceName)
        {
            var res = new List<Data>();
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand("Core_ScopeView_ByUserId"))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {

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

            return new ReadOnlyCollection<Data>(res);
        }

        public struct Data
        {
            public long ItemDefinitionId { get; set; }

            public long ItemId { get; set; }

            public long SecurityGroupId { get; set; }

            public long ApplicationUserId { get; set; }
        }
    }
}
