using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using OpenFrameworkV3;
using OpenFrameworkV3.Core;
using OpenFrameworkV3.Core.DataAccess;

public partial class Instances_NYRS_API_PostCaso : Page
{
	public const string cns = "Data Source=hw120.dinaserver.com;Initial Catalog=openf_nyrs;User Id=openf_nyrs;Password=P@ssw0rd;";
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string PutFirma(string fileData)
    {
        var res = "No action";
        var httpRequest = HttpContext.Current.Request;
        if (httpRequest.Files.Count > 0)
        {
            foreach (string file in httpRequest.Files)
            {
                var postedFile = httpRequest.Files[file];
                var fileName = postedFile.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();
                var filePath = HttpContext.Current.Server.MapPath("~/TemporalExportFolder/" + fileName);
                postedFile.SaveAs(filePath);
                return "/Uploads/" + fileName;
            }
        }

        return res;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetInvestigadores()
    {
        var res = string.Empty;
        using(var cmd = new SqlCommand("SELECT Nombre FROM Item_Investigador ORDER By Id"))
        {
            using (var cnn = new SqlConnection(cns))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.Text;
                try
                {
                    cmd.Connection.Open();
                    using(var rdr = cmd.ExecuteReader())
                    {
                        var first = true;
                        while (rdr.Read())
                        {
                            res += first ? string.Empty : "|";
                            res += rdr.GetString(0);
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

        return res;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetUnidades()
    {
        var res = string.Empty;
        using (var cmd = new SqlCommand("SELECT Nombre FROM Item_Unidad ORDER By Id"))
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
                        var first = true;
                        while (rdr.Read())
                        {
                            res += first ? string.Empty : "|";
                            res += rdr.GetString(0);
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

        return res;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string PutECQ()
    {
        var res = "No action";

        return res;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string PutCasoVideo(
        string Code,
        bool Video1Played,
        bool Video1Completed,
        int Video1Duration,
        int Video1Viewed,
        bool Video2Played,
        bool Video2Completed,
        int Video2Duration,
        int Video2Viewed,
        bool Video3Played,
        bool Video3Completed,
        int Video3Duration,
        int Video3Viewed,
        bool Video4Played,
        bool Video4Completed,
        int Video4Duration,
        int Video4Viewed,
        bool Video5Played,
        bool Video5Completed,
        int Video5Duration,
        int Video5Viewed)
    {
        var res = string.Empty;
        Instance.CheckPersistence("nyrs");
        //string cns = Persistence.ConnectionString("nyrs");
        if (!string.IsNullOrEmpty(cns))
        {
            using (var cmd = new SqlCommand("Item_Caso_FromAppVideo"))
            {
                using(var cnn = new SqlConnection(cns))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Code", Code, 100));
                    cmd.Parameters.Add(DataParameter.Input("@Video1Played", Video1Played));
                    cmd.Parameters.Add(DataParameter.Input("@Video1Completed", Video1Completed));
                    cmd.Parameters.Add(DataParameter.Input("@Video1Duration", Video1Duration));
                    cmd.Parameters.Add(DataParameter.Input("@Video1Viewed", Video1Viewed));
                    cmd.Parameters.Add(DataParameter.Input("@Video2Played", Video2Played));
                    cmd.Parameters.Add(DataParameter.Input("@Video2Completed", Video2Completed));
                    cmd.Parameters.Add(DataParameter.Input("@Video2Duration", Video2Duration));
                    cmd.Parameters.Add(DataParameter.Input("@Video2Viewed", Video2Viewed));
                    cmd.Parameters.Add(DataParameter.Input("@Video3Played", Video3Played));
                    cmd.Parameters.Add(DataParameter.Input("@Video3Completed", Video3Completed));
                    cmd.Parameters.Add(DataParameter.Input("@Video3Duration", Video3Duration));
                    cmd.Parameters.Add(DataParameter.Input("@Video3Viewed", Video3Viewed));
                    cmd.Parameters.Add(DataParameter.Input("@Video4Played", Video4Played));
                    cmd.Parameters.Add(DataParameter.Input("@Video4Completed", Video4Completed));
                    cmd.Parameters.Add(DataParameter.Input("@Video4Duration", Video4Duration));
                    cmd.Parameters.Add(DataParameter.Input("@Video4Viewed", Video4Viewed));
                    cmd.Parameters.Add(DataParameter.Input("@Video5Played", Video5Played));
                    cmd.Parameters.Add(DataParameter.Input("@Video5Completed", Video5Completed));
                    cmd.Parameters.Add(DataParameter.Input("@Video5Duration", Video5Duration));
                    cmd.Parameters.Add(DataParameter.Input("@Video5Viewed", Video5Viewed));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res = "OK|" + Code;
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
        else
        {
            res= "NoCns";
        }

        return res;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string PutCaso(
        string RegistroFarmacosText,
        string Incidencias,
        bool ConsentimientoOral,
        DateTime FINI,
        string Traslado,
        string DatosClinicos,
        string EscalaEstai1,
        int Confort1,
        int Dolor1,
        int FC1,
        int FR1,
        string TA1,
        int SO1,
        string EscalaEstai2,
        int Confort2,
        int Dolor2,
        int FC2,
        int FR2,
        string TA2,
        int SO2,
        string EscalaEstai3,
        int Confort3,
        int Dolor3,
        int FC3,
        int FR3,
        string TA3,
        int SO3,
        string Code,
        string LocalizacionDolor,
        string LocalizacionInfarto,
        string Investigador,
        string Unidad,
        string PacienteNombre,
        string PacienteApellidos,
        string PacienteDNI,
        int PacienteEdad,
        string PacienteTelefono,
        string PacienteEmail,
        bool PacienteNotificacion,
        string PacienteSexo,
        string EstudioStatus,
        string FirmaData,
        string ECQData,
        string ContactoNombre,
        string ContactoTelefono)
    {
        var res = string.Empty;
        /* CREATE PROCEDURE Item_Caso_FromAppDatos
         *   @Code nvarchar(100),
         *   @InvestigadorId nvarchar(100),
         *   @UnidadId nvarchar(100),
         *   @PacienteNombre nvarchar(30),
         *   @PacienteApellidos nvarchar(50),
         *   @PacienteDNI nvarchar(15),
         *   @PacienteEdad int,
         *   @PacienteTelefono nvarchar(15),
         *   @PacienteEmail nvarchar(150),
         *   @PacienteSexo nvarchar(1),
         *   @PacienteNotificacion bit,
         *   @EstudioStatus nvarchar(5),
         *   @EscalaSTAI1 nchar(6),
         *   @Confort1 int,
         *   @Dolor1 int,
         *   @FC1 int,
         *   @TAD1 int,
         *   @TAS1 int,
         *   @TAM1 int,
         *   @FR1 int,
         *   @SO1 int,
         *   @EscalaSTAI2 nchar(6),
         *   @Confort2 int,
         *   @Dolor2 int,
         *   @FC2 int,
         *   @TAD2 int,
         *   @TAS2 int,
         *   @TAM2 int,
         *   @FR2 int,
         *   @SO2 int,
         *   @EscalaSTAI3 nchar(6),
         *   @Conform3 int,
         *   @Dolor3 int,
         *   @FC3 int,
         *   @TAD3 int,
         *   @TAS int,
         *   @TAM int,
         *   @FR3 int,
         *   @SO3 int,
         *   @LocalizacionDolor nchar(10),
         *   @LocalizacionInfarto nchar(10),
         *   @ConsentimientoOral bit,
         *   @Traslado nchar(3),
         *   @TiempoLlegada time(7),
         *   @HoraLlegada time(7),
         *   @DatosClinicos nchar(15),
         *   @HoraAparicionDolor time(7),
         *   @TipoAntecedente nvarchar(50),
         *   @HoraECQ time(7),
         *   @FINI datetime */
        
        //string cns = Persistence.ConnectionString("nyrs");           
        if (!string.IsNullOrEmpty(cns))
        {
            using (var cmd = new SqlCommand("Item_Caso_FromAppDatos"))
            {
                using (var cnn = new SqlConnection(cns))
                {
                    var TAD1 = -1;
                    var TAS1 = -1;
                    var TAM1 = -1;
                    var TAD2 = -1;
                    var TAS2 = -1;
                    var TAM2 = -1;
                    var TAD3 = -1;
                    var TAS3 = -1;
                    var TAM3 = -1;
					
					var trasladoParts = Traslado.Split('|');

                    var TrasladoData = Traslado.Split('|')[0];
                    var tiempoLlegada = Convert.ToInt32(string.IsNullOrEmpty(trasladoParts[1]) ? "0" : trasladoParts[1]);
                    var horaLlegada = Convert.ToInt32(string.IsNullOrEmpty(trasladoParts[2]) ? "0" : trasladoParts[2]);

                    if (!string.IsNullOrEmpty(TA1))
                    {
                        var TA1Parts = TA1.Split('|');
                        Int32.TryParse(TA1Parts[0], out TAD1);
                        Int32.TryParse(TA1Parts[1], out TAS1);
                        Int32.TryParse(TA1Parts[2], out TAM1);
                    }

                    if (!string.IsNullOrEmpty(TA2))
                    {
                        var TA2Parts = TA2.Split('|');
                        Int32.TryParse(TA2Parts[0], out TAD2);
                        Int32.TryParse(TA2Parts[1], out TAS2);
                        Int32.TryParse(TA2Parts[2], out TAM2);
                    }

                    if (!string.IsNullOrEmpty(TA3))
                    {
                        var TA3Parts = TA3.Split('|');
                        Int32.TryParse(TA3Parts[0], out TAD3);
                        Int32.TryParse(TA3Parts[1], out TAS3);
                        Int32.TryParse(TA3Parts[2], out TAM3);
                    }

                    var DatosClinicosData = DatosClinicos.Split('|')[0];
                    var TipoAntecedente = DatosClinicos.Split('|')[1];
                    var HoraAparicion = Convert.ToInt32(DatosClinicos.Split('|')[2]);
                    var HoraECQ = Convert.ToInt32(DatosClinicos.Split('|')[3]);

                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputLong("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@Code", Code, 100));
                    cmd.Parameters.Add(DataParameter.Input("@InvestigadorId", Investigador, 100));
                    cmd.Parameters.Add(DataParameter.Input("@UnidadId", Unidad, 100));
                    cmd.Parameters.Add(DataParameter.Input("@PacienteNombre", PacienteNombre, 30));
                    cmd.Parameters.Add(DataParameter.Input("@PacienteApellidos", PacienteApellidos, 50));
                    cmd.Parameters.Add(DataParameter.Input("@PacienteDNI", PacienteDNI, 15));
                    cmd.Parameters.Add(DataParameter.Input("@PacienteEdad", PacienteEdad));
                    cmd.Parameters.Add(DataParameter.Input("@PacienteSexo", PacienteSexo, 1));
                    cmd.Parameters.Add(DataParameter.Input("@PacienteTelefono", PacienteTelefono, 15));
                    cmd.Parameters.Add(DataParameter.Input("@PacienteEmail", PacienteEmail, 150));
                    cmd.Parameters.Add(DataParameter.Input("@PacienteNotificacion", PacienteNotificacion));
                    cmd.Parameters.Add(DataParameter.Input("@EstudioStatus", EstudioStatus, 5));
                    cmd.Parameters.Add(DataParameter.Input("@EscalaSTAI1", EscalaEstai1, 6));
                    cmd.Parameters.Add(DataParameter.Input("@Confort1", Confort1));
                    cmd.Parameters.Add(DataParameter.Input("@Dolor1", Dolor1));
                    cmd.Parameters.Add(DataParameter.Input("@FC1", FC1));
                    cmd.Parameters.Add(DataParameter.Input("@TAD1", TAD1));
                    cmd.Parameters.Add(DataParameter.Input("@TAS1", TAS1));
                    cmd.Parameters.Add(DataParameter.Input("@TAM1", TAM1));          
                    cmd.Parameters.Add(DataParameter.Input("@FR1", FR1));
                    cmd.Parameters.Add(DataParameter.Input("@SO1", SO1));
                    cmd.Parameters.Add(DataParameter.Input("@EscalaSTAI2", EscalaEstai2, 6));
                    cmd.Parameters.Add(DataParameter.Input("@Confort2", Confort2));
                    cmd.Parameters.Add(DataParameter.Input("@Dolor2", Dolor2));
                    cmd.Parameters.Add(DataParameter.Input("@FC2", FC2));
                    cmd.Parameters.Add(DataParameter.Input("@TAD2", TAD2));
                    cmd.Parameters.Add(DataParameter.Input("@TAS2", TAS2));
                    cmd.Parameters.Add(DataParameter.Input("@TAM2", TAM2));          
                    cmd.Parameters.Add(DataParameter.Input("@FR2", FR2));
                    cmd.Parameters.Add(DataParameter.Input("@SO2", SO2));
                    cmd.Parameters.Add(DataParameter.Input("@EscalaSTAI3", EscalaEstai3, 6));
                    cmd.Parameters.Add(DataParameter.Input("@Confort3", Confort3));
                    cmd.Parameters.Add(DataParameter.Input("@Dolor3", Dolor3));
                    cmd.Parameters.Add(DataParameter.Input("@FC3", FC3));
                    cmd.Parameters.Add(DataParameter.Input("@TAD3", TAD3));
                    cmd.Parameters.Add(DataParameter.Input("@TAS3", TAS3));
                    cmd.Parameters.Add(DataParameter.Input("@TAM3", TAM3));          
                    cmd.Parameters.Add(DataParameter.Input("@FR3", FR3));
                    cmd.Parameters.Add(DataParameter.Input("@SO3", SO3));
                    cmd.Parameters.Add(DataParameter.Input("@LocalizacionDolor", LocalizacionDolor, 10));
                    cmd.Parameters.Add(DataParameter.Input("@LocalizacionInfarto", LocalizacionInfarto, 10));
                    cmd.Parameters.Add(DataParameter.Input("@ConsentimientoOral", ConsentimientoOral));
                    cmd.Parameters.Add(DataParameter.Input("@Traslado", TrasladoData, 3));
                    cmd.Parameters.Add(DataParameter.Input("@TiempoLlegada", TimeSpan.FromMinutes(tiempoLlegada)));
                    cmd.Parameters.Add(DataParameter.Input("@HoraLlegada", TimeSpan.FromMinutes(horaLlegada)));          
                    cmd.Parameters.Add(DataParameter.Input("@DatosClinicos", DatosClinicosData, 15));
                    cmd.Parameters.Add(DataParameter.Input("@HoraAparicionDolor", TimeSpan.FromMinutes(HoraAparicion)));          
                    cmd.Parameters.Add(DataParameter.Input("@TipoAntecedente", TipoAntecedente, 15));
                    cmd.Parameters.Add(DataParameter.Input("@HoraECQ", TimeSpan.FromMinutes(HoraECQ)));
                    cmd.Parameters.Add(DataParameter.Input("@FINI", FINI));
                    cmd.Parameters.Add(DataParameter.Input("@ContactoNombre", ContactoNombre, 50));
                    cmd.Parameters.Add(DataParameter.Input("@ContactoTelefono", ContactoTelefono, 15));

                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        var id = Convert.ToInt64(cmd.Parameters["@Id"].Value);
                        res = id.ToString();

                        if (!string.IsNullOrEmpty(Incidencias))
                        {
                            var incidencias = Incidencias.Split('^');
                            foreach(var incidencia in incidencias)
                            {
                                IncidenciaInsert(cns, id, incidencia);
                            }
                        }

                        if (!string.IsNullOrEmpty(RegistroFarmacosText))
                        {
                            var farmacos = RegistroFarmacosText.Split('^');
                            foreach (var farmaco in farmacos)
                            {
                                FarmacoInsert(cns, id, farmaco);
                            }
                        }

                        if (!string.IsNullOrEmpty(FirmaData))
                        {
                            var path = HttpContext.Current.Request.PhysicalApplicationPath;
                            var imagepath = string.Format(
                               CultureInfo.InvariantCulture,
                               @"{0}{2}Instances\NYRS\Data\Images\Caso\{1}_Signature.png",
                               path,
                               id,
                               path.EndsWith("\\", StringComparison.OrdinalIgnoreCase) ? string.Empty : "\\");
                            byte[] bytes = Convert.FromBase64String(FirmaData);
                            File.WriteAllBytes(imagepath, bytes);

                            using(var cmdFirma = new SqlCommand("Item_Caso_UpdateFirma"))
                            {
                                using(var cnnFirma = new SqlConnection(cns))
                                {
                                    cmdFirma.Connection = cnnFirma;
                                    cmdFirma.CommandType = CommandType.StoredProcedure;
                                    cmdFirma.Parameters.Add(DataParameter.Input("@CasoId", id));
                                    try
                                    {
                                        cnnFirma.Open();
                                        cmdFirma.ExecuteNonQuery();
                                    }
                                    finally
                                    {
                                        if(cnnFirma.State != ConnectionState.Closed)
                                        {
                                            cnnFirma.Close();
                                        }
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(ECQData))
                        {
                            var path = HttpContext.Current.Request.PhysicalApplicationPath;

                            var imagepath = string.Format(
                               CultureInfo.InvariantCulture,
                               @"{0}{2}Instances\NYRS\Data\Images\Caso\{1}_ECQ.jpg",
                               path,
                               id,
                               path.EndsWith("\\", StringComparison.OrdinalIgnoreCase) ? string.Empty : "\\");
                            byte[] bytes = Convert.FromBase64String(ECQData);
                            File.WriteAllBytes(imagepath, bytes);

                            using (var cmdECQ = new SqlCommand("Item_Caso_UpdateECQ"))
                            {
                                using (var cnnECQ = new SqlConnection(cns))
                                {
                                    cmdECQ.Connection = cnnECQ;
                                    cmdECQ.CommandType = CommandType.StoredProcedure;
                                    cmdECQ.Parameters.Add(DataParameter.Input("@CasoId", id));
                                    try
                                    {
                                        cnnECQ.Open();
                                        cmdECQ.ExecuteNonQuery();
                                    }
                                    finally
                                    {
                                        if (cnnECQ.State != ConnectionState.Closed)
                                        {
                                            cnnECQ.Close();
                                        }
                                    }
                                }
                            }
                        }
						
						res = "OK";
                    }
                    catch(Exception ex)
                    {
                        res = ex.Message;
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
        else
        {
            res = "No cns";
        }

        return res;
    }

    private static void IncidenciaInsert(string cns, long casoId, string incidenciaData)
    {
        /*CREATE PROCEDURE Item_Incidencia_FromApp
         *   @CasoId bigint,
         *   @Cinetosis int,
         *   @IncidenciaUsoDispositivo int,
         *   @HoraFin datetime,
         *   @Otra nvarchar(150) */

        var parts = incidenciaData.Split('|');

        var cinetosis = Convert.ToInt32(parts[0]);
        var incidenciaUso = Convert.ToInt32(parts[1]);
        var hora = TimeSpan.FromMinutes(Convert.ToInt32(parts[2]));
        var otra = parts[3];

        using (var cmd = new SqlCommand("Item_Incidencia_FromApp"))
        {
            using (var cnn = new SqlConnection(cns))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@CasoId", casoId));
                cmd.Parameters.Add(DataParameter.Input("@Cinetosis", cinetosis));
                cmd.Parameters.Add(DataParameter.Input("@IncidenciaUsoDispositivo", incidenciaUso));
                cmd.Parameters.Add(DataParameter.Input("@HoraFin", hora));
                cmd.Parameters.Add(DataParameter.Input("@Otra", otra, 150));
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
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

    private static void FarmacoInsert(string cns, long casoId, string farmacoData)
    {
        /* CREATE PROCEDURE Item_Farmaco_FromApp
         *   @CasoId bigint,
         *   @Nombre nvarchar(100),
         *   @Via nvarchar(100),
         *   @Dosis decimal(18,3),
         *   @Unidad nvarchar(10) */

        var parts = farmacoData.Split('|');

        var farmaco = parts[0];
        var via = parts[1];
        var dosis = Convert.ToDecimal(parts[2].Replace('.',','));
        var unidad = parts[3];

        using (var cmd = new SqlCommand("Item_Farmaco_FromApp"))
        {
            using (var cnn = new SqlConnection(cns))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@CasoId", casoId));
                cmd.Parameters.Add(DataParameter.Input("@Nombre", farmaco, 100));
                cmd.Parameters.Add(DataParameter.Input("@Via", via, 100));
                cmd.Parameters.Add(DataParameter.Input("@Dosis", dosis));
                cmd.Parameters.Add(DataParameter.Input("@Unidad", unidad, 100));
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    var x = ex.Message;
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

    private static bool Exists(string code)
    {
        var res = false;
        //var cns = Persistence.ConnectionString("nyrs");
        if (!string.IsNullOrEmpty(cns))
        {
            using(var cmd = new SqlCommand("Item_Caso_Exists"))
            {
                using(var cnn  = new SqlConnection(cns))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Code", code, 100));
                    try
                    {
                        cmd.Connection.Open();
                        using(var rdr  = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res = rdr.GetInt32(0) > 0;
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