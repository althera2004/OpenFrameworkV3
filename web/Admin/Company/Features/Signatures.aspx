<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Signatures.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Company.Signatures" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="row">
        <div class="col-xs-12">
            <div class="widget-box transparent ui-sortable-handle" id="widget-box-12">
												<div class="widget-header">
													<h4 class="widget-title">Proveedores de firmas</h4>
													<div class="widget-toolbar no-border">
														<a href="#" data-action="collapse">
															<i class="ace-icon fa fa-chevron-up"></i>
														</a>
													</div>
												</div>

												<div class="widget-body">
													<div class="widget-main padding-6 no-padding-left no-padding-right">
														<div class="text-center m-b-md">
                        <small>Proveedores para firmar documentos generados por la aplicación</small><br>
                    </div>
                    <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="headingOne">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                        <i class="fa fa-check btn-icon-green"></i>&nbsp;Wacom
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                                <div class="panel-body">
                                    <div class="col-xs-6">
                                        <label>Acciones:</label>&nbsp;
                                     <button class="btn btn-info btn-sm">Detectar dispositivo</button>&nbsp;
                                     <button class="btn btn-info btn-sm">Probar captura de firma</button>
                                    </div>
                                    <div class="col-xs-6">
                                        <div class="form-explanation alert alert-block alert-info" style="padding: 8px;">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td style="vertical-align: top;"><i class="ace-icon fa fa-info-circle info fa-2x"></i>&nbsp;</td>
                                                        <td>
                                                            Tenga preparada la tableta digitalizadora de firmas.
                                                            <br />
                                                            <img src="/img/wacom.png" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-footer">
                                    <label>Acciones:</label>&nbsp;
                                     <button class="btn btn-xs">Activar</button>
                                    <button class="btn btn-danger btn-xs">Desactivar</button>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="headingTwo">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="true" aria-controls="collapseOne">
                                        <i class="fa fa-ban btn-icon-red"></i>&nbsp;Firmafy
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                                <div class="panel-body">
                                    <div class="form-horizontal" role="form">
                                        <div class="col-xs-6">
                                            <div class="form-group">
                                                <label id="UserLabel" class="formFieldLabel col-sm-2">Código<span style="color: #ff0000;">*</span></label>
                                                <div class="col-sm-10">
                                                    <input type="text" id="User" class="col-xs-12 col-sm-12 codesequence tooltip-info formData" value="jcastilla@openframework.es" onblur="this.value = $.trim(this.value);" spellcheck="false" data-validation="required">
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label id="PasswordLabel" class="formFieldLabel col-sm-2">Nombre<span style="color: #ff0000;">*</span></label>
                                                <div class="col-sm-10">
                                                    <input type="password" id="Password" class="col-xs-12 col-sm-12  tooltip-info formData" value="V@nessa57" onblur="this.value = $.trim(this.value);" spellcheck="false" data-validation="required">
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label id="CIFLabel" class="formFieldLabel col-sm-2">CIF</label>
                                                <div class="col-sm-10">
                                                    <input type="text" id="CIF" class="col-xs-12 col-sm-12  tooltip-info formData" value="53072025P" onblur="this.value = $.trim(this.value);" spellcheck="false" data-validation="required" readonly="readonly">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-6">
                                            <div id="FirmafyWarning" class="form-explanation alert alert-block alert-info" style="padding: 8px;">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td style="vertical-align: top;"><i class="ace-icon fa fa-info-circle info fa-2x"></i>&nbsp;</td>
                                                            <td>Se deben introducir los datos de la cuenta de Firmafy.<br>
                                                                El correcto funcionamiento depende de <a href="http:www.firmafy.com" target="_blank">Firmafy.com</a>. OpenFramework sólo envía los datos a Firmafy para que proceda a la firma de los documentos.
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-footer">
                                    <label>Acciones:</label>&nbsp;
                            <button class="btn btn-success btn-xs">Activar</button>
                                    <button class="btn btn-xs">Desactivar</button>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="headingThree">
                                <h4 class="panel-title">
                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="true" aria-controls="collapseOne">
                                        <i class="fa fa-check btn-icon-green"></i>&nbsp;<strong><span style="color:#000;">Open</span><span style="color:#2662a7;">Framework</span></strong>&nbsp;APP QR
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                                <div class="panel-body">
                                    A wonderful serenity has taken possession of my entire soul, like these sweet mornings of spring which I enjoy with my whole heart. I am alone, and feel the charm of existence in this spot, which was created for the bliss of souls like mine.
                                </div>
                                <div class="panel-footer">
                                    <label>Acciones:</label>&nbsp;
                            <button class="btn btn-xs">Activar</button>
                                    <button class="btn btn-danger btn-xs">Desactivar</button>
                                </div>
                            </div>
                        </div>
                    </div>
													</div>
												</div>
											</div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <div class="widget-box transparent ui-sortable-handle" id="widget-box-12">
												<div class="widget-header">
													<h4 class="widget-title">Documentos con firma predeterminada</h4>
													<div class="widget-toolbar no-border">
														<a href="#" data-action="collapse">
															<i class="ace-icon fa fa-chevron-up"></i>
														</a>
													</div>
												</div>

												<div class="widget-body">
													<div class="widget-main padding-6 no-padding-left no-padding-right">
														<div class="text-center m-b-md">
                    </div>
                <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                    <div class="panel-body" style="padding: 0; border: none;">
                        <table id="example1" style="max-height: 470px; border: none;" class="scroll table table-bordered table-striped" data-page-size="8" data-filter="#filter">
                            <thead>
                                <tr>
                                    <th data-toggle="true" class="sort ASC  footable-first-column footable-sortable">Ítem<span class="footable-sort-indicator"></span></th>
                                    <th class=" sort footable-visible footable-sortable">Documento<span class="footable-sort-indicator"></span></th>
                                    <th class="footable-visible footable-sortable">Grupo<span class="footable-sort-indicator"></span></th>
                                    <th class="footable-visible footable-last-column footable-sortable">Usuario<span class="footable-sort-indicator"></span></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr class="footable-even" style="display: table-row;">
                                    <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Residente</td>
                                    <td class="footable-visible">Informe médico</td>
                                    <td class="footable-visible">Médicos</td>
                                    <td class="footable-visible footable-last-column">Alberto Monzón</td>
                                </tr>
                                <tr class="footable-odd" style="display: table-row;">
                                    <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Residente</td>
                                    <td class="footable-visible">Reglamento interno</td>
                                    <td class="footable-visible">Compañía</td>
                                    <td class="footable-visible footable-last-column">Elena Rodríguez</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="panel-footer">
                    <ul class="pagination pull-right">
                        <li class="footable-page-arrow disabled"><a data-page="first" href="#first">«</a></li>
                        <li class="footable-page-arrow disabled"><a data-page="prev" href="#prev">‹</a></li>
                        <li class="footable-page active"><a data-page="0" href="#">1</a></li>
                        <li class="footable-page"><a data-page="1" href="#">2</a></li>
                        <li class="footable-page-arrow"><a data-page="next" href="#next">›</a></li>
                        <li class="footable-page-arrow"><a data-page="last" href="#last">»</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScripts" runat="server">
    <script type="text/javascript">
        pageType = "PageAdmin";
        MenuSelectOption("AdminCompany");
    </script>
</asp:Content>