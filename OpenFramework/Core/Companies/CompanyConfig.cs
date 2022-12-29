

namespace OpenFramework.Core.Companies
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using OpenFrameworkV3;
    using OpenFrameworkV3.Core.DataAccess;

    public class CompanyConfig
    {
        public long CompanyId { get; set; }
        public bool DocumentHistory { get; set; }
        public int DocumentDelete { get; set; }
        public int DocumentTemporalAlive { get; set; }
        public bool MassDownload { get; set; }
        public long AvailableDocuments { get; set; }
        public long AvailableImages { get; set; }
        public long DiskQuote { get; set; }
        public bool FeatureSticky { get; set; }
        public bool FeatureCustomAlerts { get; set; }
        public bool FeatureUserLock { get; set; }
        public bool FeatureFollowing { get; set; }
        public bool FeatureCustomLayout { get; set; }
        public bool OnPremise { get; set; }
        public int SAT { get; set; }
        public bool CustomConfig { get; set; }

        public static CompanyConfig Empty
        {
            get
            {
                return new CompanyConfig
                {
                    AvailableDocuments = 0,
                    AvailableImages = 0,
                    CompanyId = Constant.DefaultId,
                    CustomConfig = false,
                    DiskQuote = 100,
                    DocumentDelete = 0,
                    DocumentHistory = false,
                    DocumentTemporalAlive = 30,
                    FeatureCustomAlerts = false,
                    FeatureCustomLayout = false,
                    FeatureFollowing = false,
                    FeatureSticky = false,
                    FeatureUserLock = false,
                    MassDownload = false,
                    OnPremise = false,
                    SAT = 0
                };
            }
        }


        public static CompanyConfig Get(long companyId, string instanceName)
        {
            var res = CompanyConfig.Empty;
            res.CompanyId = companyId;
            var cns = Persistence.ConnectionString(instanceName);
            if (!string.IsNullOrEmpty(cns))
            {
                using(var cmd = new SqlCommand("Core_CompanyConfig_Get"))
                {
                    using(var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        try
                        {
                            cmd.Connection.Open();
                            using(var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    res.DocumentHistory = rdr.GetBoolean(1);
                                    res.DocumentDelete = rdr.GetInt32(2);
                                    res.DocumentTemporalAlive = rdr.GetInt32(3);
                                    res.MassDownload = rdr.GetBoolean(4);
                                    res.AvailableDocuments = rdr.GetInt64(5);
                                    res.AvailableImages = rdr.GetInt64(6);
                                    res.DiskQuote = rdr.GetInt64(7);
                                    res.FeatureSticky = rdr.GetBoolean(8);
                                    res.FeatureCustomAlerts = rdr.GetBoolean(9);
                                    res.FeatureUserLock = rdr.GetBoolean(10);
                                    res.FeatureFollowing = rdr.GetBoolean(11);
                                    res.FeatureCustomLayout = rdr.GetBoolean(12);
                                    res.OnPremise = rdr.GetBoolean(13);
                                    res.SAT = rdr.GetInt32(14);
                                    res.CustomConfig = rdr.GetBoolean(15);
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

            return res;
        }
    }
}
