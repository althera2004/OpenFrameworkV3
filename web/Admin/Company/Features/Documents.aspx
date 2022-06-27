<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Documents.aspx.cs" Inherits="OpenFrameworkV2.Web.Admin.Company.Features.Documents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
     <div class="row">
         <div class="col-lg-6">
             <div class="hpanel hblue">
                 <div class="panel-heading hbuilt">
                     Gestió documental
                 </div>
                 <div class="panel-body">
                     <div class="row form-group">
                         <label class="col-sm-4 control-label">Activar histórico</label>

                         <div class="col-sm-2">
                             <label>
                                 <input type="radio" checked="" value="option1" id="RBHistoryNo" name="RBHistory">
                                 No
                             </label>
                         </div>
                         <div class="col-sm-2">
                             <label>
                                 <input type="radio" checked="" value="option1" id="RBHistoryYes" name="RBHistory">
                                 Sí
                             </label>
                         </div>
                         <label class="col-sm-4 control-label">&nbsp;</label>
                         <small class="col-sm-12 help-block">Los archivos históricos ocupan gran cantidad de cuota de disco.
                         </small>
                     </div>
                     <div class="row form-group">
                         <label class="col-sm-4 control-label">Eliminación</label>
                         <div class="col-sm-4">
                             <label>
                                 <input type="radio" checked="" value="option1" id="RBDeleteItem" name="RBDelete">
                                 Según permisos elemento
                             </label>
                         </div>
                         <div class="col-sm-4">
                             <label>
                                 <input type="radio" checked="" value="option1" id="RBDeleteAdmin" name="RBDelete">
                                 Sólo administradores
                             </label>
                         </div>
                     </div>
                     <div class="row form-group">
                         <label class="col-sm-4 control-label">Archivos temporales</label>
                         <div class="col-sm-4">
                             <label>
                                 <input type="radio" checked="" value="option1" id="RBDeleteTemporal1" name="RBDeleteTemporal">
                                 No borrar
                             </label>
                         </div>
                         <div class="col-sm-4">
                             <label>
                                 <input type="radio" checked="" value="option1" id="RBDeleteTemporal2" name="RBDeleteTemporal">
                                 Mantener 24 horas
                             </label>
                         </div>
                         <label class="col-sm-4 control-label">&nbsp;</label>
                         <div class="col-sm-4">
                             <label>
                                 <input type="radio" checked="" value="option1" id="RBDeleteTemporal3" name="RBDeleteTemporal">
                                 Mantener 1 semana
                             </label>
                         </div>
                         <div class="col-sm-4">
                             <label>
                                 <input type="radio" checked="" value="option1" id="RBDeleteTemporal4" name="RBDeleteTemporal">
                                 Mantener 1 mes
                             </label>
                         </div>
                     </div>
                 </div>
                 <div class="panel-body" style="padding:6px;">
                     <table>
                         <tr>
                            <td rowspan="2" style="width:30px;vertical-align:top;text-align:center;"><i id="Check_Dw" class="FeatureIcon fa fa-ban btn-icon-red"></i></td>
                            <td><label>Descarga de datos</label></td>
                        </tr>
                         <tr>
                             <td class="help-block">Ofrece la posbilidad de descarga la información de un ítem en formato XML ó JSON y todos los documentos asociados</td>
                         </tr>
                     </table>
                 </div>
             </div>
             <div class="hpanel hblue">
                 <div class="panel-heading hbuilt">
                     Tipos permitidos
                 </div>
                 <div class="panel-body tabs">
                     <ul class="nav nav-tabs">
                         <li class="active"><a data-toggle="tab" href="#tab-6">Documentos</a></li>
                         <li class=""><a data-toggle="tab" href="#tab-7">Imágenes</a></li>
                     </ul>
                     <div class="tab-content ">
                         <div id="tab-6" class="tab-pane active">
                             <div class="panel-body">
                                 <div class="row form-group">

                                     <div class="col-sm-3">
                                         <label class="checkbox-inline">
                                             <input type="checkbox" value="option1" id="inlineCheckbox1">
                                             .pdf
                                         </label>
                                     </div>
                                     <div class="col-sm-3">
                                         <label class="checkbox-inline">
                                             <input type="checkbox" value="option1" id="inlineCheckbox1">
                                             .doc/docx
                                         </label>
                                     </div>
                                     <div class="col-sm-3">
                                         <label class="checkbox-inline">
                                             <input type="checkbox" value="option1" id="inlineCheckbox1">
                                             .xls/.xlsx
                                         </label>
                                     </div>
                                     <div class="col-sm-3">
                                         <label class="checkbox-inline">
                                             <input type="checkbox" value="option1" id="inlineCheckbox1">
                                             .csv
                                         </label>
                                     </div>
                                     <div class="col-sm-3">
                                         <label class="checkbox-inline">
                                             <input type="checkbox" value="option1" id="inlineCheckbox1">
                                             .ppt/pptx
                                         </label>
                                     </div>
                                     <div class="col-sm-3">
                                         <label class="checkbox-inline">
                                             <input type="checkbox" value="option1" id="inlineCheckbox1">
                                             .png
                                         </label>
                                     </div>
                                     <div class="col-sm-3">
                                         <label class="checkbox-inline">
                                             <input type="checkbox" value="option1" id="inlineCheckbox1">
                                             .jpg
                                         </label>
                                     </div>
                                     <div class="col-sm-3">
                                         <label class="checkbox-inline">
                                             <input type="checkbox" value="option1" id="inlineCheckbox1">
                                             .gif
                                         </label>
                                     </div>
                                 </div>
                             </div>
                         </div>
                         <div id="tab-7" class="tab-pane">
                             <div class="panel-body">
                                 <div class="row form-group">
                                     <div class="col-sm-3">
                                         <label class="checkbox-inline">
                                             <input type="checkbox" value="option1" id="inlineCheckbox1">
                                             .png
                                         </label>
                                     </div>
                                     <div class="col-sm-3">
                                         <label class="checkbox-inline">
                                             <input type="checkbox" value="option1" id="inlineCheckbox1">
                                             .jpg
                                         </label>
                                     </div>
                                     <div class="col-sm-3">
                                         <label class="checkbox-inline">
                                             <input type="checkbox" value="option1" id="inlineCheckbox1">
                                             .gif
                                         </label>
                                     </div>
                                 </div>
                             </div>
                         </div>
                     </div>

                 </div>
             </div>
         </div>
         <div class="col-lg-6">
             <div class="hpanel hblue">
                 <div class="panel-heading hbuilt">
                     Cuota de disco
                 </div>   
                 <div class="panel-body">
                        <div class="stats-title pull-left">
                            <h4>Nº de archivos</h4>
                        </div>
                        <div class="stats-icon pull-right">
                            <i class="pe-7s-document fa-4x"></i>
                        </div>
                        <div style="margin-top:40px;">
                            <h1 class="text-success">1.458</h1>
                            <small>
                                Incluye los archivos de datos, informes generados y documentos de facturas.
                            </small>
                        </div>
                    </div>

                     <div class="panel-body h-200">
                        <div class="stats-title pull-left">
                            <h4>Tipos de archivos</h4>
                        </div>
                        <div class="clearfix"></div>
                        <div class="m-t-xs">
                            <div class="row">
                                <div class="col-xs-6" style="">
                                    <small class="stat-label">Datos</small>
                                    <h4>375 <i class="fa fa-level-up text-success"></i></h4>
                                </div>
                                <div class="col-xs-6" style="">
                                    <small class="stat-label">Facturas</small>
                                    <h4>12 <i class="fa fa-level-up text-success"></i></h4>
                                </div>
                                <div class="col-xs-6" style="">
                                    <small class="stat-label">Históricos</small>
                                    <h4>837 <i class="fa fa-level-up text-success"></i></h4>
                                </div>
                                <div class="col-xs-6" style="">
                                    <small class="stat-label">Ficheros temporales</small>
                                    <h4>234 <i class="fa fa-level-up text-success"></i></h4>
                                </div>
                            </div>
                        </div>
                    </div>
                 <div class="panel-body">
                     <h5>Cuota de disco ocupada: 75MB de 100MB</h5>                     
                     <div class="m">
                         <div class="progress m-t-xs full progress-striped">
                             <div style="width: 75%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="75" role="progressbar" class=" progress-bar progress-bar-warning">
                                 75%
                             </div>
                         </div>
                     </div>                     
                 </div>
                 <div class="panel-footer">
                     <label>Acciones:</label>&nbsp;
                     <button class="btn btn-info btn-sm">Aumentar cuota de disco</button>
                     <button class="btn btn-success btn-sm">Vaciar temporales</button>
                     <button class="btn btn-danger btn-sm">Eliminar antiguos</button>
                 </div>
             </div>
         </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScripts" runat="server">
    <script type="text/javascript">
        pageType = "PageAdmin";
        MenuSelectOption("AdminCompany");

    $(function () {

        /**
         * Pie Chart Data
         */
        var pieChartData = [
            { label: "Ocupado", data: 16, color: "#1a5983", },
            { label: "Disponible", data: 6, color: "#e5edf3", }
        ];

        /**
         * Pie Chart Options
         */
        var pieChartOptions = {
            series: {
                pie: {
                    show: true
                }
            },
            grid: {
                hoverable: true
            },
            tooltip: true,
            tooltipOpts: {
                content: "%p.0%, %s", // show percentages, rounding to 2 decimal places
                shifts: {
                    x: 20,
                    y: 0
                },
                defaultTheme: false
            }
        };

        $.plot($("#flot-pie-chart"), pieChartData, pieChartOptions);

    
    });
    </script>
</asp:Content>