using iTextSharp.text.pdf;

namespace OpenFrameworkV3.NYRS.Export
{

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using OpenFrameworkV3.Core;
    using OpenFrameworkV3.Core.Activity;
    using OpenFrameworkV3.Core.Companies;
    using OpenFrameworkV3.Core.DataAccess;
    using OpenFrameworkV3.Core.Security;
    using OpenFrameworkV3.Tools;
    using Image = iTextSharp.text.Image;

    public partial class ConsentimientoInformado : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ActionResult Generate(long casoId)
        {
            var instance = Persistence.InstanceByName("nyrs");
            var company = Company.ById(1, instance.Name);
            var applicationUser = ApplicationUser.Actual;

            string res = "0";
            try
            {
                // Se preparan los objetos para el PDF
                var fontName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "calibri.ttf");
                var fontNameBold = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "calibrib.ttf");
                var fontNameItalic = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "calibrii.ttf");
                var fontNameCheckbox = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "webdings.ttf");
                var bf = BaseFont.CreateFont(fontName, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                var bfBold = BaseFont.CreateFont(fontNameBold, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                var bfItalic = BaseFont.CreateFont(fontNameItalic, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                var baseFontCheckBox = BaseFont.CreateFont(fontNameCheckbox, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                var fontNormal = new Font(bf, 10, Font.NORMAL, BaseColor.BLACK);
                var fontItalic = new Font(bfItalic, 10, Font.NORMAL, BaseColor.BLACK);
                var fontBold = new Font(bfBold, 10, Font.NORMAL, BaseColor.BLACK);
                var fontCheckbox = new Font(baseFontCheckBox, 12, Font.NORMAL, BaseColor.BLACK);
                var fontApartado = new Font(bfBold, 14, Font.NORMAL, BaseColor.BLACK);

                string path = HttpContext.Current.Request.PhysicalApplicationPath;

                if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
                {
                    path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
                }

                var targetFolder = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}Instances\\{1}\\Data\\Alquiler",
                    HttpContext.Current.Request.PhysicalApplicationPath,
                    instance.Name);

                Basics.VerifyFolder(targetFolder);
                targetFolder = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}\\{1}\\SignaturesTemporal",
                    targetFolder,
                    casoId);

                Basics.VerifyFolder(targetFolder);

                var fileNameNew = string.Format(CultureInfo.InvariantCulture, "{0}\\ConsentimientoInformado_{1}.pdf", targetFolder, Guid.NewGuid());

                var pacienteNombre = string.Empty;
                var pacienteDNI = string.Empty;
                var pacienteTelefono = string.Empty;
                var contactoTelefono = string.Empty;
                var pacienteEmail = string.Empty;
                DateTime FINI = DateTime.Now;
                var investigador = "Investigador 1";
                var notificacion = false;


                using (var cmd = new SqlCommand("Export_Caso_ConsentimientoInformado"))
                {
                    using (var cnn = new SqlConnection(instance.Config.ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("CasoId", casoId));
                        try
                        {
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();

                                    pacienteNombre = rdr.GetString(0);
                                    pacienteDNI = rdr.GetString(1);
                                    pacienteTelefono = rdr.GetString(2);
                                    contactoTelefono = rdr.GetString(3);
                                    pacienteEmail = rdr.GetString(4);

                                    if (!rdr.IsDBNull(5))
                                    {
                                        FINI = rdr.GetDateTime(5);
                                    }

                                    notificacion = rdr.GetBoolean(6);

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

                using (var pdfDoc = new Document(PageSize.A4, 40, 40, 80, 50))
                {
                    var pdfMedicacion = fileNameNew;
                    var writer = PdfWriter.GetInstance(pdfDoc, new FileStream(pdfMedicacion, FileMode.Create));
                    Pdf.SetMetadata(pdfDoc, "Consentimiento informado", "OpenFramework - " + instance.Name);
                    Pdf.SuperCompression(writer);

                    var headerFooter =

                    writer.PageEvent = new ConsentiminetoInformadoHeaderFooter
                    {
                        Destinatario = string.Format(CultureInfo.InvariantCulture, "{0} - {1}", pacienteNombre, pacienteDNI),
                        //LogoPath = company.LogoPathPdf,
                        TitlePart1 = "CONSENTIMIENTO INFORMADO"
                    };

                    pdfDoc.Open();

                    // Fecha larga
                    var p0 = new Paragraph();
                    p0.SetLeading(0, 1.2f);
                    p0.SpacingBefore = 15f;
                    p0.Add(new Chunk("Hoja de Información al Paciente", fontApartado));
                    pdfDoc.Add(p0);

                    var p1a = new Paragraph();
                    p1a.SetLeading(0, 1.2f);
                    p1a.SpacingBefore = 10f;
                    p1a.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    p1a.Add(new Chunk("Nos dirigimos a usted para informarle sobre un estudio de investigación en el que se le invita a participar.El estudio ha sido aprobado por el Comité Ético de Investigación.", fontNormal));
                    pdfDoc.Add(p1a);

                    var p1b = new Paragraph();
                    p1b.SetLeading(0, 1.2f);
                    p1b.SpacingBefore = 10f;
                    p1b.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    p1b.Add(new Chunk("Nuestra intención es que usted reciba la información correcta y suficiente para que pueda decidir si quiere o no participar en este estudio.Para ello lea esta hoja informativa con atención y le aclararemos las dudas que le puedan surgir después de la explicación.Además, puede consultar con las personas que considere oportunas.", fontNormal));
                    pdfDoc.Add(p1b);


                    var t2 = new Paragraph();
                    t2.SetLeading(0, 1.2f);
                    t2.SpacingBefore = 15f;
                    t2.Alignment = Rectangle.ALIGN_LEFT;
                    t2.Add(new Chunk("PROPÓSITO DEL ESTUDIO", fontBold));
                    pdfDoc.Add(t2);

                    var p2a = new Paragraph();
                    p2a.SetLeading(0, 1.2f);
                    p2a.SpacingBefore = 10f;
                    p2a.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    p2a.Add(new Chunk("El estudio consiste en comparar si visualizar a través de una pantalla contenidos multimedia (video y audio) desarrollados ad hoc para la situación que nos ocupa, consigue reducir la posible ansiedad y dolor que pueda sufrir durante el traslado, así como mejorar el confort y la experiencia del traslado, respecto a un traslado habitual.", fontNormal));
                    pdfDoc.Add(p2a);

                    var t3 = new Paragraph();
                    t3.SetLeading(0, 1.2f);
                    t3.SpacingBefore = 10f;
                    t3.Alignment = Rectangle.ALIGN_LEFT;
                    t3.Add(new Chunk("PROCEDIMIENTO", fontBold));
                    pdfDoc.Add(t3);

                    var p3a = new Paragraph();
                    p3a.SetLeading(0, 1.2f);
                    p3a.SpacingBefore = 10f;
                    p3a.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    p3a.Add(new Chunk("Utilizando un sistema de asignación aleatoria, usted puede ser asignado/a por azar al grupo de pacientes a los que se le muestra el contenido multimedia a través de pantalla gráfica o al grupo de pacientes que será trasladado de forma habitual, es decir, sin la exposición a dichos contenidos.Por tanto, la posibilidad de ser asignado / a a uno u otro grupo es del 50 %.Nos gustaría señalar que su participación es igual de valiosa para el estudio independientemente del grupo al que sea asignado.", fontNormal));
                    pdfDoc.Add(p3a);

                    var p3b = new Paragraph();
                    p3b.SetLeading(0, 1.2f);
                    p3b.SpacingBefore = 10f;
                    p3b.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    p3b.Add(new Chunk("La visualización tiene una duración de 7 minutos y ha sido desarrollada teniendo en cuenta la información de pacientes que han pasado por esta misma situación, así como profesionales expertos en emergencias sanitarias y patología cardiaca.Una vez terminada la visualización se le proyectará una secuencia de imágenes relajantes con música hasta la llegada al hospital.", fontNormal));
                    pdfDoc.Add(p3b);

                    var p3c = new Paragraph();
                    p3c.SetLeading(0, 1.2f);
                    p3c.SpacingBefore = 10f;
                    p3c.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    p3c.Add(new Chunk("Independientemente del grupo al que sea asignado/a, el personal registrará sus constantes vitales durante el traslado y le realizarán una serie de preguntas sobre cómo se siente. Además, por el hecho de participar usted acepta que nos pongamos en contacto con usted en unas semanas para hacerle una breve entrevista telefónica sobre su experiencia.", fontNormal));
                    pdfDoc.Add(p3c);

                    var t4 = new Paragraph();
                    t4.SetLeading(0, 1.2f);
                    t4.SpacingBefore = 15f;
                    t4.Alignment = Rectangle.ALIGN_LEFT;
                    t4.Add(new Chunk("BENEFICIOS Y POSIBLES RIESGOS", fontBold));
                    pdfDoc.Add(t4);

                    var p4a = new Paragraph();
                    p4a.SetLeading(0, 1.2f);
                    p4a.SpacingBefore = 10f;
                    p4a.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    p4a.Add(new Chunk("A día de hoy no existen estudios previos sobre visualización de contenidos multimedia a través de pantallas gráficas en pacientes con su misma patología y atendidos fuera del entorno hospitalario.Es por ello que ", fontNormal));
                    p4a.Add(new Chunk("se desconoce si obtendría algún beneficio para su Salud", fontBold));
                    p4a.Add(new Chunk(" por participar en este estudio", fontNormal));
                    pdfDoc.Add(p4a);

                    var p4b = new Paragraph();
                    p4b.SetLeading(0, 1.2f);
                    p4b.SpacingBefore = 10f;
                    p4b.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    p4b.Add(new Chunk("La falta de estudios en pacientes con las mismas características que usted que están siendo trasladados en ambulancias hace imposible determinar si pudiera existir algún efecto adverso en este caso.Por tanto, deberá comunicar a los profesionales que le atienden cualquier efecto negativo que usted considere que puede estar experimentado durante el traslado.", fontNormal));
                    pdfDoc.Add(p4b);

                    var p4c = new Paragraph();
                    p4c.SetLeading(0, 1.2f);
                    p4c.SpacingBefore = 10f;
                    p4c.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    p4c.Add(new Chunk("En el caso de que el uso de la pantalla gráfica pueda interferir o dilatar la atención que recibe por parte de los profesionales o su estado empeore durante el traslado, los profesionales le retirarán los auriculares y la pantalla de forma inmediata.", fontNormal));
                    pdfDoc.Add(p4c);

                    var t5 = new Paragraph();
                    t5.SetLeading(0, 1.2f);
                    t5.SpacingBefore = 15f;
                    t5.Alignment = Rectangle.ALIGN_LEFT;
                    t5.Add(new Chunk("PARTICIPACIÓN VOLUNTARIA Y RETIRADA DEL ESTUDIO", fontBold));
                    pdfDoc.Add(t5);

                    var p5a = new Paragraph();
                    p5a.SetLeading(0, 1.2f);
                    p5a.SpacingBefore = 10f;
                    p5a.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    p5a.Add(new Chunk("Debe saber que su participación en este estudio es ", fontNormal));
                    p5a.Add(new Chunk("voluntaria", fontBold));
                    p5a.Add(new Chunk("y que puede decidir NO participar sin consecuencias en la atención que va a recibir.Si decide participar, puede cambiar de opinión y retirar su consentimiento en cualquier momento, sin que por ello se altere la relación con los profesionales sanitarios que le atienden ni se produzca perjuicio alguno en su atención sanitaria.Es por ello que puede pedir a los profesionales que le retiren los auriculares y la pantalla en cualquier momento del traslado.", fontNormal));
                    pdfDoc.Add(p5a);

                    var t6 = new Paragraph();
                    t6.SetLeading(0, 1.2f);
                    t6.SpacingBefore = 10f;
                    t6.Alignment = Rectangle.ALIGN_LEFT;
                    t6.Add(new Chunk("CONFIDENCIALIDAD", fontBold));
                    pdfDoc.Add(t6);

                    var p6a = new Paragraph();
                    p6a.SetLeading(0, 1.2f);
                    p6a.SpacingBefore = 10f;
                    p6a.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    p6a.Add(new Chunk("El estudio cumplirá lo establecido en la Ley Orgánica 15/1999, de 13 de diciembre de protección de datos de carácter personal y al Real Decreto que la desarrolla(RD 1720 / 2007). Los datos recogidos para el estudio estarán identificados mediante un código, de manera que no incluya información que pueda identificarle.El tratamiento, la comunicación y la cesión de los datos de carácter personal de todos los participantes se ajustarán a lo dispuesto en esta ley.", fontNormal));
                    pdfDoc.Add(p6a);

                    var t7 = new Paragraph();
                    t7.SetLeading(0, 1.2f);
                    t7.SpacingBefore = 15f;
                    t7.Alignment = Rectangle.ALIGN_LEFT;
                    t7.Add(new Chunk("A QUIÉN CONTACTAR", fontBold));
                    pdfDoc.Add(t7);

                    var p7a = new Paragraph();
                    p7a.SetLeading(0, 1.2f);
                    p7a.SpacingBefore = 10f;
                    p7a.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    p7a.Add(new Chunk("Si requiere información adicional o desea ejercer sus derechos de acceso, rectificación, consulta u oposición de los datos, puede ponerse en contacto con la Investigadora Principal del proyecto Dra.Olga Paloma o el Sr.Sergio Cazorla en el teléfono 647862311 o en el correo electrónico: sergio.cazorla@gmail.com.", fontNormal));
                    pdfDoc.Add(p7a);

                    pdfDoc.NewPage();

                    var p1 = new Paragraph();
                    p1.SetLeading(0, 1.2f);
                    p1.SpacingBefore = 15f;
                    p1.Add(new Chunk("Hoja de Consentimiento de Participante / CONSENTIMIENTO INFORMADO", fontApartado));
                    pdfDoc.Add(p1);

                    var p8a = new Paragraph();
                    p8a.SetLeading(0, 1.2f);
                    p8a.SpacingBefore = 10f;
                    p8a.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    p8a.Add(new Chunk("Yo, ", fontNormal));
                    p8a.Add(new Chunk(pacienteNombre, fontBold));
                    pdfDoc.Add(p8a);

                    var l1 = new Paragraph();
                    l1.SetLeading(0, 2f);
                    l1.SpacingBefore = 10f;
                    l1.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    l1.Add(new Chunk("  a ", fontCheckbox));
                    l1.Add(new Chunk("  He leído la hoja de información que se me ha entregado sobre el estudio.", fontNormal));
                    pdfDoc.Add(l1);

                    var l2 = new Paragraph();
                    l2.SetLeading(0, 2f);
                    l2.SpacingBefore = 5f;
                    l2.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    l2.Add(new Chunk("  a ", fontCheckbox));
                    l2.Add(new Chunk("  He podido hacer preguntas sobre el estudio.", fontNormal));
                    pdfDoc.Add(l2);

                    var l3 = new Paragraph();
                    l3.SetLeading(0, 2f);
                    l3.SpacingBefore = 5f;
                    l3.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    l3.Add(new Chunk("  a ", fontCheckbox));
                    l3.Add(new Chunk("  He recibido suficiente información sobre el estudio.", fontNormal));
                    pdfDoc.Add(l3);

                    var l4 = new Paragraph();
                    l4.SetLeading(0, 2f);
                    l4.SpacingBefore = 5f;
                    l4.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    l4.Add(new Chunk("  a ", fontCheckbox));
                    l4.Add(new Chunk("  He hablado con ", fontNormal));
                    l4.Add(new Chunk(investigador, fontBold));
                    pdfDoc.Add(l4);

                    var l5 = new Paragraph();
                    l5.SetLeading(0, 2f);
                    l5.SpacingBefore = 5f;
                    l5.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    l5.Add(new Chunk("  a ", fontCheckbox));
                    l5.Add(new Chunk("  He comprendo que mi participación es voluntaria.", fontNormal));
                    pdfDoc.Add(l5);

                    var l6 = new Paragraph();
                    l6.SetLeading(0, 2f);
                    l6.SpacingBefore = 5f;
                    l6.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    l6.Add(new Chunk("  a ", fontCheckbox));
                    l6.Add(new Chunk("  Comprendo que puedo retirarme del estudio:", fontNormal));
                    l6.Add(new Chunk("\n                 - Cuando quiera.", fontNormal));
                    l6.Add(new Chunk("\n                 - Sin tener que dar explicaciones.", fontNormal));
                    l6.Add(new Chunk("\n                 - Sin que esto repercuta en mis cuidados de salud / médicos.", fontNormal));
                    pdfDoc.Add(l6);

                    var l7 = new Paragraph();
                    l7.SetLeading(0, 2f);
                    l7.SpacingBefore = 10f;
                    l7.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    l7.Add(new Chunk("  a ", fontCheckbox));
                    l7.Add(new Chunk("  Recibiré́ una copia firmada y fechada de este documento de consentimiento informado.", fontNormal));
                    pdfDoc.Add(l7);

                    var l8 = new Paragraph();
                    l8.SetLeading(0, 2f);
                    l8.SpacingBefore = 10f;
                    l8.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    l8.Add(new Chunk("  a ", fontCheckbox));
                    l8.Add(new Chunk("  Presto libremente mi conformidad para participar en el estudio.", fontNormal));
                    pdfDoc.Add(l8);



                    var p9a = new Paragraph();
                    p9a.SetLeading(0, 1.2f);
                    p9a.SpacingBefore = 10f;
                    p9a.Alignment = Rectangle.ALIGN_JUSTIFIED;
                    p9a.Add(new Chunk("Deseo que me comuniquen la información derivada de la investigación que pueda ser relevante para mi salud:\n", fontNormal));

                    if (notificacion)
                    {
                        p9a.Add(new Chunk("r", fontCheckbox));
                        p9a.Add(new Chunk("  SÍ  ", fontBold));
                        p9a.Add(new Chunk("              ", fontNormal));
                        p9a.Add(new Chunk(" ", fontCheckbox));
                        p9a.Add(new Chunk("  NO  ", fontNormal));
                    }
                    else
                    {
                        p9a.Add(new Chunk(" ", fontCheckbox));
                        p9a.Add(new Chunk(" SÍ  ", fontNormal));
                        p9a.Add(new Chunk("              ", fontNormal));
                        p9a.Add(new Chunk("r", fontCheckbox));
                        p9a.Add(new Chunk("NO  ", fontBold));

                    }

                    pdfDoc.Add(p9a);

                    var signaturPath = string.Format(
                        CultureInfo.InvariantCulture,
                        @"{0}\Images\Caso\{1}_Signature.png",
                        Instance.Path.Data("nyrs"),
                        casoId);

                    if (File.Exists(signaturPath))
                    {
                        var logo = Image.GetInstance(signaturPath);

                        var scaleX = 12000f / logo.Width;
                        var scaleY = 9000f / logo.Height;

                        if(scaleX < scaleY)
                        {
                            logo.ScalePercent(scaleX);
                        }
                        else
                        {
                            logo.ScalePercent(scaleY);
                        }

                        //logo.ScaleAbsolute(160f, 120f);
                        var pageSize = pdfDoc.PageSize;
                        logo.SetAbsolutePosition(pageSize.GetLeft(60), pageSize.GetBottom(200));
                        var contentByte = writer.DirectContent;
                        contentByte.AddImage(logo);
                    }

                    var tableWidths = new float[] { 50f, 20f };
                    var table = new PdfPTable(tableWidths.Length)
                    {
                        WidthPercentage = 100,
                        HorizontalAlignment = 1,
                        SpacingBefore = 20f,
                        SpacingAfter = 0f
                    };
                    table.SetWidths(tableWidths);

                    table.AddCell(CellTable("Firma del paciente", bf, 1, 0));
                    table.AddCell(CellTable("Firma del investigador", bf, 1, 0));
                    table.AddCell(CellTable(string.Format(CultureInfo.InvariantCulture, @"Fecha: {0:dd/MM/yyyy}", FINI), bf, 1, 0));
                    table.AddCell(CellTable(string.Format(CultureInfo.InvariantCulture, @"Fecha: {0:dd/MM/yyyy}", FINI), bf, 1, 0));
                    table.AddCell(CellTable(" ", bf, 2, 0));
                    table.AddCell(CellTable(" ", bf, 2, 0));
                    table.AddCell(CellTable(" ", bf, 2, 0));
                    table.AddCell(CellTable(" ", bf, 2, 0));
                    table.AddCell(CellTable(" ", bf, 2, 0));
                    table.AddCell(CellTable(" ", bf, 2, 0));
                    table.AddCell(CellTable(string.Format(CultureInfo.InvariantCulture, "Teléfono de contacto 1: {0}", pacienteTelefono), bf, 2, 0));
                    table.AddCell(CellTable(string.Format(CultureInfo.InvariantCulture, "Teléfono de contacto 2: {0}", contactoTelefono), bf, 2, 0));
                    table.AddCell(CellTable(string.Format(CultureInfo.InvariantCulture, "Correo electrónico contacto: {0}", pacienteEmail), bf, 2, 0));

                    pdfDoc.Add(table);

                    pdfDoc.CloseDocument();
                }

                res = Basics.PathToUrl(fileNameNew);
            }
            catch (Exception ex)
            {
                ExceptionManager.Trace(ex, "ConsentimientoInformado");
                res += ex.Message;
            }

            res=  Basics.PathToUrl(res, true);

            return new ActionResult
            {
                Success = true,
                ReturnValue = res
            };
        }



        /// <summary>Creates a cell table</summary>
        /// <param name="value">Value to show</param>
        /// <param name="font">Font for content</param>
        /// <returns>Cell table</returns>
        public static PdfPCell CellTable(string value, BaseFont font, int colSpan, int border)
        {
            var finalValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                finalValue = value;
            }

            return new PdfPCell(new Phrase(finalValue, new Font(font, 9, Font.NORMAL, BaseColor.BLACK)))
            {
                Border = border,
                Padding = 6f,
                PaddingTop = 6f,
                Colspan = colSpan
            };
        }

        public class ConsentiminetoInformadoHeaderFooter : PdfPageEventHelper
        {
            public string Destinatario { get; set; }
            public string LogoPath { get; set; }
            public string TitlePart1 { get; set; }

            [CLSCompliant(false)]
            public override void OnStartPage(PdfWriter writer, Document pdfDoc)
            {
                if (writer == null || pdfDoc == null)
                {
                    return;
                }

                var fontName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "calibri.ttf");
                var fontNameBold = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "calibrib.ttf");
                var bf = BaseFont.CreateFont(fontName, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                var bfBold = BaseFont.CreateFont(fontNameBold, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                base.OnStartPage(writer, pdfDoc);
                var contentByte = writer.DirectContent;
                var pageSize = pdfDoc.PageSize;

                if (File.Exists(this.LogoPath))
                {
                    var logo = Image.GetInstance(this.LogoPath);
                    logo.ScaleAbsolute(150f, 75f);
                    logo.SetAbsolutePosition(pageSize.GetLeft(30), pageSize.GetTop(70));
                    contentByte.AddImage(logo);
                }

                contentByte.BeginText();
                contentByte.SetFontAndSize(bfBold, 16);
                contentByte.ShowTextAligned(
                    PdfContentByte.ALIGN_CENTER,
                    this.TitlePart1,
                    pageSize.GetRight(pdfDoc.PageSize.Width / 2),
                    pageSize.GetTop(40),
                    0);
                contentByte.EndText();
            }

            [CLSCompliant(false)]
            public override void OnEndPage(PdfWriter writer, Document pdfDoc)
            {
                if (writer == null || pdfDoc == null)
                {
                    return;
                }

                var fontName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "calibri.ttf");
                var fontNameBold = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + @"Fonts", "calibrib.ttf");
                var bf = BaseFont.CreateFont(fontName, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                var bfBold = BaseFont.CreateFont(fontNameBold, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                base.OnEndPage(writer, pdfDoc);
                var pageSize = pdfDoc.PageSize;
                var contentByte = writer.DirectContent; PutText(contentByte, "Página " + writer.PageNumber.ToString(), bf, 8, PdfContentByte.ALIGN_CENTER, pageSize.GetRight(pageSize.Width / 2f), pageSize.GetBottom(32));
                PutText(contentByte, "Consentimiento informado", bf, 8, PdfContentByte.ALIGN_RIGHT, pageSize.GetRight(40), pageSize.GetBottom(32));

                contentByte.SetColorStroke(BaseColor.LIGHT_GRAY);
                contentByte.MoveTo(pageSize.GetLeft(30f), pageSize.GetBottom(42));
                contentByte.LineTo(pageSize.GetRight(30f), pageSize.GetBottom(42));
                contentByte.Stroke();

                PutText(contentByte, Destinatario, bf, 10, PdfContentByte.ALIGN_LEFT, pageSize.GetLeft(40), pageSize.GetBottom(32));
            }

            private void PutText(PdfContentByte contentByte, string text, BaseFont bf, int fontSize, int aligment, float positionX, float positionY)
            {
                contentByte.BeginText();
                contentByte.SetFontAndSize(bf, fontSize);
                contentByte.ShowTextAligned(
                    aligment,
                    text,
                    positionX,
                    positionY,
                    0);
                contentByte.EndText();
            }
        }
    }
}