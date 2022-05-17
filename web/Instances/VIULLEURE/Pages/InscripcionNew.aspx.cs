// --------------------------------
// <copyright file="InscripcionNew.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFramework.Web.Async
{
    using OpenFrameworkV2;
    using System;
    using System.Web.UI;

    /// <summary>Implements upload action form attach files</summary>
    public partial class InscripcionNew : Page
    {
        /// <summary>Page's load event</summary>
        /// <param name="sender">Loaded page</param>
        /// <param name="e">Event's arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var res = ActionResult.NoAction;
            try
            {
                var CasalId = Convert.ToInt64(this.Request.Form["casalId"]);
                var AsistenteNombre = this.Request.Form["AsistenteNombre"];
                var AsistenteApellidos = this.Request.Form["AsistenteApellidos"];
                var AsistenteFNAC = this.Request.Form["AsistenteFNAC"];
                var AsistenteCurso = this.Request.Form["AsistenteCurso"];
                var TutorNombre = this.Request.Form["TutorNombre"];
                var TutorApellidos = this.Request.Form["TutorApellidos"];
                var TutorNIF = this.Request.Form["TutorNIF"];
                var TutorDireccion = this.Request.Form["TutorDireccion"];
                var TutorPoblacion = this.Request.Form["TutorTelefono"];
                var TutorTelefono = this.Request.Form["TutorEmail"];
                var TutorEmail = this.Request.Form["TutorEmail"];
                var AutorizacionActividades = this.Request.Form["AutorizacionActividades"];
                var AutorizacionImagen = this.Request.Form["AutorizacionImagenSI"];
                var AutorizacionTraslado = this.Request.Form["AutorizacionTraslado"];
                var SaludEnfermedades = this.Request.Form["SaludEnfermedades"];
                var SaludAlergia = this.Request.Form["SaludAlergia"];
                var SaludDieta = this.Request.Form["SaludDieta"];
                var NecesidadesEspeciales = this.Request.Form["NecesidadesEspeciales"];
                var Observaciones = this.Request.Form["Observaciones"];
                var Vista = this.Request.Form["Vista"];
                var Insomnio = this.Request.Form["Insomnio"];
                var Enuresi = this.Request.Form["Enuresi"];
                var Ortopedia = this.Request.Form["Ortopedia"];
                var Psico = this.Request.Form["Psico"];
                var Cansancio = this.Request.Form["Cansancio"];

                var DocDNITutor = this.Request.Files["DocDNITutor"];
                var DocLibroFamilia = this.Request.Files["DocLibroFamilia"];
                var DocTarjetaSanitaria = this.Request.Files["DocTarjetaSanitaria"];
                var DocVacunas = this.Request.Files["DocVacunas"];
                var DocDGJ = this.Request.Files["DocDGJ"];
                var DocInformeMedico = this.Request.Files["DocInformeMedico"];

                /*
                 * CREATE PROCEDURE Item_InscripcionFromWeb	
            @Code nvarchar(20),
            @CasalId bigint,
            @FechaSolicitud datetime,
            @FechaConfirmacion datetime,
            @Status int,
            @StatusText nvarchar(1000),
            @DocSolicitut nvarchar(250),
            @DocDNITutor nvarchar(250),
            @DocLibroFamilia nvarchar(250),
            @DocTarjetaSanitaria nvarchar(250),
            @DocVacunas nvarchar(250),
            @DocDGJ nvarchar(250),
            @DocInformeMedico nvarchar(250),
            @AsistenteNombre nvarchar(20),
            @AsistenteApellidos nvarchar(50),
            @FNac nvarchar(1),
            @Curso nvarchar(1000),
            @TutorNombre nvarchar(20),
            @TutorApellidos nvarchar(50),
            @TutorNIF nvarchar(15),
            @TurorEmail nvarchar(150),
            @TutorTelefono nvarchar(15),
            @TutorDireccion nvarchar(150),
            @TutorPoblacion nvarchar(50),
            @SaludEnfermedades nvarchar(50),
            @SaludMedicamentos nvarchar(50),
            @SaludAlergia nvarchar(50),
            @SaludDieta nvarchar(50),
            @NecesidadesEspeciales nvarchar(2000),
            @Observaciones nvarchar(2000),
            @Vista nvarchar(50),
            @Insomnio nvarchar(50),
            @Enuresi nvarchar(50),
            @Ortopedia nvarchar(50),
            @AutorizacionActividades bit,
            @AutorizacionImagen bit,
            @AutorizacionTraslado bit,
            @CreatedBy bigint,
            @CreatedOn datetime,
            @ModifiedBy bigint,
            @ModifiedOn datetime,
            @Active bit,
            @Psico nvarchar(50),
            @Cansancio nvarchar(50) */
            }
            catch(Exception ex)
            {
                res.SetFail(ex);
            }

            this.Response.Clear();
            this.Response.ContentType = "application/json";
            this.Response.Write(res.Success ? res.ReturnValue.ToString() : res.MessageError);
            this.Response.Flush();
        }
    }
}