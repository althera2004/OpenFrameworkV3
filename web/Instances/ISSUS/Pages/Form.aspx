<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Form.aspx.cs" Inherits="Instances_SUPPORT_Pages_Form" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentScriptVars" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div id="PersistentFields">
        <div id="0" class=" ">
            <div class="form-horizontal" role="form">
                <div class="form-group">
                    <label for="Title" id="TitleLabel" class="formFieldLabel col-sm-1">Titòl<span class="FieldRequired" style="color: #ff0000;">*</span></label><div class="col-sm-5">
                        <input type="text" id="Title" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="100" onblur="this.value = $.trim(this.value);" spellcheck="false" data-required="true">
                    </div>
                    <div class="checkbox col-sm-3">
                        <label id="PublishedLabelDiv">
                            <input name="form-field-checkbox" type="checkbox" id="Published" class="ace formData" spellcheck="false" value=""><span class="lbl" id="PublishedLabel"> Publicat</span></label>
                    </div>
                    <div class="checkbox col-sm-3">
                        <label id="OnlineLabelDiv">
                            <input name="form-field-checkbox" type="checkbox" id="Online" class="ace formData" spellcheck="false" value=""><span class="lbl" id="OnlineLabel"> Online</span></label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
							<div class="col-xs-12">
								<!-- PAGE CONTENT BEGINS -->
										<div class="tabbable">
											<ul class="nav nav-tabs" id="myTab" style="padding-left:12px;">
												<li class="active">
													<a data-toggle="tab" href="#home">
														<i class="green ace-icon fa fa-home bigger-120"></i>
														Home
													</a>
												</li>

												<li>
													<a data-toggle="tab" href="#messages">
														Messages
														<span class="badge badge-danger">4</span>
													</a>
												</li>

												<li class="dropdown">
													<a data-toggle="dropdown" class="dropdown-toggle" href="#">
														Dropdown &nbsp;
														<i class="ace-icon fa fa-caret-down bigger-110 width-auto"></i>
													</a>

													<ul class="dropdown-menu dropdown-info">
														<li>
															<a data-toggle="tab" href="#dropdown1">Oculto 1</a>
														</li>

														<li>
															<a data-toggle="tab" href="#dropdown2">Oculto 2</a>
														</li>
													</ul>
												</li>
												<li>
													<a data-toggle="tab" href="#attachs">
														<i class="blue ace-icon fa fa-paperclip bigger-120"></i>
														Adjunts
													</a>
												</li>
												<li class="rightTab blue" title="Afegir recordatori" onclick="Feature_Sticky_ShowPopup();"><i class="fa fa-sticky-note fa-2x"></i></li>
												<li class="rightTab red blink_me" title="Afegir recordatori" onclick="Feature_Sticky_ShowPopup();"><span style="font-size:14px;">Usuari fora del centre</span></li>
											</ul>

											<div class="tab-content">
												<div id="home" class="tab-pane fade in active">
													<div class="form-horizontal" role="form">
                                                        <div class="form-group">
                                                                <label for="EnNombreDe" id="EnNombreDeLabel" class="formFieldLabel col-sm-2">En nom de</label>
																<div class="col-sm-10">
                                                                    <input type="text" id="EnNombreDe" class="col-xs-12 col-sm-12  tooltip-info formData" value="" onblur="this.value = $.trim(this.value);" spellcheck="false" style="display: none;"><input type="radio" id="EnNombreDe0" name="EnNombreDeRB" onclick="RESIDENTE_EnNombreDeChanged(0);">En nom propi&nbsp;&nbsp;&nbsp;&nbsp;<input type="radio" id="EnNombreDe1" name="EnNombreDeRB" onclick="RESIDENTE_EnNombreDeChanged(1, true);">Tutor legal&nbsp;&nbsp;&nbsp;&nbsp;<input type="radio" id="EnNombreDe2" name="EnNombreDeRB" onclick="RESIDENTE_EnNombreDeChanged(2, true);" checked="checked">Familiar&nbsp;&nbsp;&nbsp;&nbsp;<input type="radio" id="EnNombreDe3" name="EnNombreDeRB" onclick="RESIDENTE_EnNombreDeChanged(3, true);">Guardador de fet&nbsp;
                                                                    <button type="button" class="btn btn-info btn-sm" style="border-radius:2px; padding: 0px 2px; font-size: 12px;" id="EnNombreDeBtnView" onclick="RESIDENTE_SetPersonaContacto();">Veure dades</button></div>
															</div>
                                                        <div class="form-group">
                                                           <label for="Enuresi" id="EnuresiLabel" class="formFieldLabel col-sm-1">Enuresi</label>
															<div class="col-sm-5">
																<div class="input-group">
																	<input class="form-control date-picker" id="id-date-picker-1" type="text" data-date-format="dd-mm-yyyy">
																	<span class="input-group-addon">
																		<i class="fa fa-calendar bigger-110"></i>
																	</span>
																</div>
															</div>
															<div class="col-sm-6">&nbsp;</div></div>
															<div class="form-group"> 
															<label for="Psico" id="PsicoLabel" class="formFieldLabel col-sm-1">Dificultat</label>
															<div class="col-sm-5">
																<label>
																	<input name="switch-field-1" class="ace ace-switch" type="checkbox">
																	<span class="lbl" data-lbl="&nbsp;&nbsp;SÍ&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;NO&nbsp;&nbsp;"></span>
																</label>
																</div><label for="Cansancio" id="CansancioLabel" class="formFieldLabel col-sm-1">Cansancio</label><div class="col-sm-5"><input type="text" id="Cansancio" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div></div>
															
													<!-------------------------------------------------->
													<div id="accordion" class="accordion-style1 panel-group accordion-style2">
														<div class="panel panel-form">
															<div class="panel-form-heading">
																<span class="panel-title">Salud y necesidades</span>
																<span class="panel-title-detail">Hola que tal
															</span></div>
															<div class="panel-body">
																<div class="form-horizontal" role="form"><div class="form-group"> <label for="SaludEnfermedades" id="SaludEnfermedadesLabel" class="formFieldLabel col-sm-1">Malalties</label><div class="col-sm-5"><input type="text" id="SaludEnfermedades" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><label for="SaludMedicamentos" id="SaludMedicamentosLabel" class="formFieldLabel col-sm-1">Medicació</label><div class="col-sm-5"><input type="text" id="SaludMedicamentos" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div><div class="form-group"> <label for="SaludAlergia" id="SaludAlergiaLabel" class="formFieldLabel col-sm-1">Al·lèrgies</label><div class="col-sm-5"><input type="text" id="SaludAlergia" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><label for="SaludDieta" id="SaludDietaLabel" class="formFieldLabel col-sm-1">Dieta</label><div class="col-sm-5"><input type="text" id="SaludDieta" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div><div class="form-group"> <label for="Vista" id="VistaLabel" class="formFieldLabel col-sm-1">Vista/oïda</label><div class="col-sm-5"><input type="text" id="Vista" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><label for="Insomnio" id="InsomnioLabel" class="formFieldLabel col-sm-1">Insomni</label><div class="col-sm-5"><input type="text" id="Insomnio" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div><div class="form-group"> <label for="Enuresi" id="EnuresiLabel" class="formFieldLabel col-sm-1">Enuresi</label><div class="col-sm-5"><input type="text" id="Enuresi" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><div class="col-sm-6">&nbsp;</div></div><div class="form-group"> <label for="Psico" id="PsicoLabel" class="formFieldLabel col-sm-1">Dificultat</label><div class="col-sm-5"><input type="text" id="Psico" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><label for="Cansancio" id="CansancioLabel" class="formFieldLabel col-sm-1">Cansancio</label><div class="col-sm-5"><input type="text" id="Cansancio" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div></div>
																<div class="form-group">
										<label class="col-sm-3 control-label" for="form-field-tags">Tag input</label>

										<div class="col-sm-9">
											<div class="inline"><div class="tags"><span class="tag">Tag Input Control<button type="button" class="close">×</button></span><span class="tag">Programmatically Added<button type="button" class="close">×</button></span><input type="text" name="tags" id="form-field-tags" value="Tag Input Control" placeholder="Enter tags ..." style="display: none;"><input type="text" placeholder="Enter tags ..."></div>
												
											</div>
										</div>
									</div>
															</div>
														</div>
													</div>

													<div id="accordion" class="accordion-style1 panel-group accordion-style2">
														<div class="panel panel-form">
															<div class="panel-form-heading">
																<h4 class="panel-title">
																	<a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true">
																		&nbsp;Group Item #1																		
																		<span class="panel-title-detail">Hola que tal</span>
																		<i class="bigger-110 ace-icon fa fa-chevron-down" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
																	</a>
																</h4>
															</div>

															<div class="panel-collapse collapse in" id="collapseOne" aria-expanded="true" style="">
																<div class="panel-body">
																	<div class="form-horizontal" role="form"><div class="form-group"> <label for="SaludEnfermedades" id="SaludEnfermedadesLabel" class="formFieldLabel col-sm-1">Malalties</label><div class="col-sm-5"><input type="text" id="SaludEnfermedades" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><label for="SaludMedicamentos" id="SaludMedicamentosLabel" class="formFieldLabel col-sm-1">Medicació</label><div class="col-sm-5"><input type="text" id="SaludMedicamentos" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div><div class="form-group"> <label for="SaludAlergia" id="SaludAlergiaLabel" class="formFieldLabel col-sm-1">Al·lèrgies</label><div class="col-sm-5"><input type="text" id="SaludAlergia" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><label for="SaludDieta" id="SaludDietaLabel" class="formFieldLabel col-sm-1">Dieta</label><div class="col-sm-5"><input type="text" id="SaludDieta" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div><div class="form-group"> <label for="Vista" id="VistaLabel" class="formFieldLabel col-sm-1">Vista/oïda</label><div class="col-sm-5"><input type="text" id="Vista" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><label for="Insomnio" id="InsomnioLabel" class="formFieldLabel col-sm-1">Insomni</label><div class="col-sm-5"><input type="text" id="Insomnio" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div><div class="form-group"> <label for="Enuresi" id="EnuresiLabel" class="formFieldLabel col-sm-1">Enuresi</label><div class="col-sm-5"><input type="text" id="Enuresi" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><div class="col-sm-6">&nbsp;</div></div><div class="form-group"> <label for="Psico" id="PsicoLabel" class="formFieldLabel col-sm-1">Dificultat</label><div class="col-sm-5"><input type="text" id="Psico" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><label for="Cansancio" id="CansancioLabel" class="formFieldLabel col-sm-1">Cansancio</label><div class="col-sm-5"><input type="text" id="Cansancio" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div></div>
																</div>
															</div>
														</div>
													</div>
													
													<div id="accordion2" class="accordion-style1 panel-group accordion-style2">
														<div class="panel panel-form">
															<div class="panel-form-heading">
																<h4 class="panel-title">
																	<a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseOne1" aria-expanded="true">
																		&nbsp;Group Item #1																		
																		<span class="panel-title-detail">Hola que tal</span>
																		<i class="bigger-110 ace-icon fa fa-chevron-down" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
																	</a>
																</h4>
															</div>

															<div class="panel-collapse collapse in" id="collapseOne1" aria-expanded="true" style="">
																<div class="panel-body">
																	<div class="form-horizontal" role="form"><div class="form-group"> <label for="SaludEnfermedades" id="SaludEnfermedadesLabel" class="formFieldLabel col-sm-1">Malalties</label><div class="col-sm-5"><input type="text" id="SaludEnfermedades" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><label for="SaludMedicamentos" id="SaludMedicamentosLabel" class="formFieldLabel col-sm-1">Medicació</label><div class="col-sm-5"><input type="text" id="SaludMedicamentos" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div><div class="form-group"> <label for="SaludAlergia" id="SaludAlergiaLabel" class="formFieldLabel col-sm-1">Al·lèrgies</label><div class="col-sm-5"><input type="text" id="SaludAlergia" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><label for="SaludDieta" id="SaludDietaLabel" class="formFieldLabel col-sm-1">Dieta</label><div class="col-sm-5"><input type="text" id="SaludDieta" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div><div class="form-group"> <label for="Vista" id="VistaLabel" class="formFieldLabel col-sm-1">Vista/oïda</label><div class="col-sm-5"><input type="text" id="Vista" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><label for="Insomnio" id="InsomnioLabel" class="formFieldLabel col-sm-1">Insomni</label><div class="col-sm-5"><input type="text" id="Insomnio" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div><div class="form-group"> <label for="Enuresi" id="EnuresiLabel" class="formFieldLabel col-sm-1">Enuresi</label><div class="col-sm-5"><input type="text" id="Enuresi" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><div class="col-sm-6">&nbsp;</div></div><div class="form-group"> <label for="Psico" id="PsicoLabel" class="formFieldLabel col-sm-1">Dificultat</label><div class="col-sm-5"><input type="text" id="Psico" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><label for="Cansancio" id="CansancioLabel" class="formFieldLabel col-sm-1">Cansancio</label><div class="col-sm-5"><input type="text" id="Cansancio" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div></div>
																</div>
															</div>
														</div>
													</div>
													<!-------------------------------------------------->
													
													
												</div>

												<div id="messages" class="tab-pane fade">
													<div class="form-horizontal" role="form"><div class="form-group"> <label for="SaludEnfermedades" id="SaludEnfermedadesLabel" class="formFieldLabel col-sm-1">Malalties</label><div class="col-sm-5"><input type="text" id="SaludEnfermedades" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><label for="SaludMedicamentos" id="SaludMedicamentosLabel" class="formFieldLabel col-sm-1">Medicació</label><div class="col-sm-5"><input type="text" id="SaludMedicamentos" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div><div class="form-group"> <label for="SaludAlergia" id="SaludAlergiaLabel" class="formFieldLabel col-sm-1">Al·lèrgies</label><div class="col-sm-5"><input type="text" id="SaludAlergia" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><label for="SaludDieta" id="SaludDietaLabel" class="formFieldLabel col-sm-1">Dieta</label><div class="col-sm-5"><input type="text" id="SaludDieta" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div><div class="form-group"> <label for="Vista" id="VistaLabel" class="formFieldLabel col-sm-1">Vista/oïda</label><div class="col-sm-5"><input type="text" id="Vista" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><label for="Insomnio" id="InsomnioLabel" class="formFieldLabel col-sm-1">Insomni</label><div class="col-sm-5"><input type="text" id="Insomnio" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div><div class="form-group"> <label for="Enuresi" id="EnuresiLabel" class="formFieldLabel col-sm-1">Enuresi</label><div class="col-sm-5"><input type="text" id="Enuresi" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><div class="col-sm-6">&nbsp;</div></div><div class="form-group"> <label for="Psico" id="PsicoLabel" class="formFieldLabel col-sm-1">Dificultat</label><div class="col-sm-5"><input type="text" id="Psico" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div><label for="Cansancio" id="CansancioLabel" class="formFieldLabel col-sm-1">Cansancio</label><div class="col-sm-5"><input type="text" id="Cansancio" class="col-xs-12 col-sm-12  tooltip-info formData" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false"></div></div></div>
												</div>

												<div id="dropdown1" class="tab-pane fade">
													<h4>Oculto 1</h4>
													<p>Etsy mixtape wayfarers, ethical wes anderson tofu before they sold out mcsweeney's organic lomo retro fanny pack lo-fi farm-to-table readymade.</p>
												</div>

												<div id="dropdown2" class="tab-pane fade">
													<h4>Oculto 2</h4>
													<p>Trust fund seitan letterpress, keytar raw denim keffiyeh etsy art party before they sold out master cleanse gluten-free squid scenester freegan cosby sweater. Fanny pack portland seitan DIY, art party locavore wolf cliche high life echo park Austin.</p>
												</div>

												
												<div id="attachs" class="tab-pane fade">
                                                    <div class="form-horizontal" role="form" id="AttachsContent">
                                                        <div class="form-horizontal">
                                                            <div class="row">
                                                                <div class="col-xs-6">
                                                                    <div class="tabTitle">Documents adjunts</div>
                                                                </div>
                                                                <div class="col-xs-6" style="text-align: right;">
																	<i class="blue fa fa-list" title="Veure com a llistat"></i>
																	&nbsp;|&nbsp;
																	<i class="blue fa fa-th-large" title="Veure icones"></i>
																	&nbsp;|&nbsp;
																	<a href="#" data-action="add" class="green" id="BtnAddItem" onclick="UploadFile(null,'attach');"><i class="ace-icon fa fa-plus"></i>&nbsp;Afegir document</a>
                                                                </div>
                                                            </div>
                                                            <div class="table-responsive" style="border: none;">
                                                                <div id="AttachDataContainer" _style="border:1px solid #ccc;padding:0;" style="height: 521px;">
                                                                    <div id="Feature_Attachment_List" style="">
                                                                        <div class="table-responsive" id="scrollTableDiv">
                                                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                                                <thead class="thin-border-bottom">
                                                                                    <tr id="ListDataHeader">
                                                                                        <th onclick="Sort(this,'Feature_Attachment_ListData');" id="th0" class="">Descripció</th>
                                                                                        <th onclick="Sort(this,'Feature_Attachment_ListData');" id="th1" class="" style="width: 120px;">Mida</th>
                                                                                        <th onclick="Sort(this,'Feature_Attachment_ListData');" id="th2" class="" style="width: 120px;">Data</th>
                                                                                        <th id="tdActions" style="width: 128px;" class="">&nbsp;</th>
                                                                                    </tr>
                                                                                </thead>
                                                                            </table>
                                                                            <div id="Feature_Attachment_ListDataDiv" style="overflow: hidden scroll; padding: 0px; height: 445px;">
                                                                                <div id="Feature_Attachment_ListLoading" style="width: 100%; height: 100%; text-align: center; font-size: 24px; background-color: #fff; display: none;">
                                                                                    <div style="padding-top: 40px;">Carregant dades...</div>
                                                                                </div>
                                                                                <table class="table table-bordered table-striped" style="border-top: none;" id="Feature_Attachment_ListDataTableContainer">
                                                                                    <tbody id="Feature_Attachment_ListData"></tbody>
                                                                                </table>
                                                                            </div>
                                                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                                                <thead class="thin-border-bottom">
                                                                                    <tr id="ListDataFooter">
                                                                                        <th style="color: #aaa;"><i>Nº registres:&nbsp;<span id="Feature_Attachment_ListDataCount">0</span></i></th>
                                                                                    </tr>
                                                                                </thead>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                    <div id="Feature_Attachment_Grid" style="display: none; overflow: auto; border: 1px solid rgb(204, 204, 204); height: 523px;">
                                                                        <img src="/img/arrowup-ca.png" style="float: right;"></div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
												</div>
											</div>
										</div>
									

								<!-- PAGE CONTENT ENDS -->
							</div><!-- /.col -->
						</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentScriptFiles" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentScripts" Runat="Server">
	<script type="text/javascript">
		function ResizeWorkArea() {
            $(".tab-content").height($(window).height() - 265);
		}

		pageType = "PageForm";
		$(".tab-content").css("overflow", "auto");
		$(".date-picker").localDatePicker();
		ResizeWorkArea();

		window.onresize = function () {
			ResizeWorkArea();
        }

    </script>
</asp:Content>