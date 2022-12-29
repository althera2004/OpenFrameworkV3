namespace OpenFramework.Instance.ViuLleure
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using System.Web.UI;
    using OpenFrameworkV3;

    public partial class DashBoard : Page
    {
        private Main master;

        public string InstanceName
        {
            get
            {
                return this.master.InstanceName;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.master = this.Master as Main;


            var title = "Dashboard";
            this.master.SetTitle(title);
            this.master.SetPageType("PageView");
            this.master.SetPageType("Dashboard");
            NoVideo();
            NoDAtos();
			Balance();
        }
		
		private void Balance(){
			var query = @"SELECT 
	SUM(CASE WHEN LEFT(Code,1) = 'I' THEN 1 ELSE 0 END) AS Intervencion,
	SUM(CASE WHEN LEFT(Code,1) = 'C' THEN 1 ELSE 0 END) AS Control
from item_Caso 
where
	EstudioStatus <> 'NULO'
AND EstudioStatus IS NOT NULL";

var RES = new StringBuilder();
            string cns = Persistence.ConnectionString("nyrs");
            if (!string.IsNullOrEmpty(cns))
            {
                using (var cmd = new SqlCommand(query))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.Text;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    RES.Append("<h4>Distribución de casos</h4><table style=\"width:100%;paddinh:8px;\">");
                                    while (rdr.Read())
                                    {
										var i = rdr.GetInt32(0);
										var c = rdr.GetInt32(1);
										
                                        RES.AppendFormat(
										@"<tr>
										<td style=""text-align:center;background-color:#8686c1;color:#fff;width:{0:#0}%"">Intervención: {0:#0.00}%<br/>{2} / {4}</td>
										<td style=""text-align:center;background-color:#68bf68;color:#fff;"">Control: {1:#0.00}%<br/>{3} / {4}</td>
										</tr>",
										Convert.ToDecimal(i) * 100/(i+c),
										Convert.ToDecimal(c) * 100/(i+c),
										i,
										c,
										i+c);
										
                                            
                                    }
									
									RES.Append("</table>");

                                    this.LtDistribucion.Text = RES.ToString();
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
		}

        private void NoVideo()
        {
            var RES = new StringBuilder();
            string cns = Persistence.ConnectionString("nyrs");
            if (!string.IsNullOrEmpty(cns))
            {
                var query = @"Select
 Code, InvestigadorId, UnidadId
from Item_Caso

where

    LEFT(Code, 1) = 'I'
and(CASE WHEN HasVideo = 1 THEN 1 ELSE 0 END) <> 1";
                using (var cmd = new SqlCommand(query))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.Text;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    RES.Append("<h4>Casos de intervención si datos sobre visionado:</h4><ul>");
                                    while (rdr.Read())
                                    {
                                        RES.AppendFormat(
                                            CultureInfo.InvariantCulture,
                                            @"<li> <strong>{0}</strong>, {1} - {2}</li>",
                                            rdr.GetString(0),
                                            rdr.GetString(1),
                                            rdr.GetString(2));
                                    }
                                    RES.Append("</ul>");

                                    this.LtNoVideo.Text = RES.ToString();
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
        }

        private void NoDAtos()
        {
            var RES = new StringBuilder();
            string cns = Persistence.ConnectionString("nyrs");
            if (!string.IsNullOrEmpty(cns))
            {
                var query = @"Select Code from Item_Caso where
    LEFT(Code, 1) = 'I'
and(CASE WHEN HasData = 1 THEN 1 ELSE 0 END) <> 1";
                using (var cmd = new SqlCommand(query))
                {
                    using (var cnn = new SqlConnection(cns))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.Text;
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    RES.Append("<h4>Casos de intervención sin datos del caso:</h4><ul>");
                                    while (rdr.Read())
                                    {
                                        RES.AppendFormat(
                                            CultureInfo.InvariantCulture,
                                            @"<li> <strong>{0}</strong></li>",
                                            rdr.GetString(0));
                                    }
                                    RES.Append("</ul>");

                                    this.LtNoDatos.Text = RES.ToString();
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
        }
    }
}