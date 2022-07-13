<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ItemDefinition.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Instance.ItemDefinition" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentScriptVars" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="row" id="PersistentFields">
        <div class="form-group col-xs-12">
            <div class="row">
                <label class="col-sm-1 control-label">Id:</label>
                <div class="col-sm-2"><span id="ItemDefinitionId"></span></div>
                <label class="col-sm-1 control-label">Nom:</label>
                <div class="col-sm-2"><span id="ItemDefinitionName"></span></div>
            </div>
        </div>
    </div>
    <div class="col-lg-12">
        <div class="hpanel">
            <ul class="nav nav-tabs" id="FormTabs">
                <li id="tabSelect-common" class="tabSelect active"><a data-toggle="tab" href="#tab-common" aria-expanded="false">Configuració</a></li>
                <li id="tabSelect-fields" class="tabSelect"><a data-toggle="tab" href="#tab-fields" aria-expanded="false">Camps</a></li>
                <li id="tabSelect-sql" class="tabSelect"><a data-toggle="tab" href="#tab-sql" aria-expanded="false">SQL</a></li>
                <li id="tabSelect-scripts" class="tabSelect"><a data-toggle="tab" href="#tab-scripts" aria-expanded="false">Scripts</a></li>
                <li id="tabSelect-dictionary" class="tabSelect"><a data-toggle="tab" href="#tab-dictionary" aria-expanded="false">Diccionari</a></li>
            </ul>
            <div class="tab-content" id="FormContent">
                <div id="tab-common" class="tab-pane active">
                    <div class="hpanel">
                        <form class="form-horizontal" role="form">
                            <div class="row">
                                <h4>Configuració general:&nbsp;</h4>
                            </div>
                            <div class="row">
                                <label class="col-sm-2">Identificador:&nbsp;</label>
                                <label class="col-sm-2"><strong>20</strong></label>
                                <label class="col-sm-1">Nom:&nbsp;</label>
                                <label class="col-sm-3"><strong id="InstanceName"></strong></label>
                                <label class="col-sm-1">Idioma:&nbsp;</label>
                                <label class="col-sm-3"><strong id="InstanceDefaultLanguage"></strong></label>
                            </div>
                            <div class="row">
                                <label class="col-sm-2">Descripció:&nbsp;</label>
                                <label class="col-sm-10"><strong id="InstanceDescription"></strong></label>
                            </div>
                            <div class="row">
                                <label class="col-sm-2">Multiempresa:&nbsp;</label>
                                <label class="col-sm-2"><strong id="InstanceMulticompany"></strong></label>
                                <label class="col-sm-1">Timeout:&nbsp;</label>
                                <label class="col-sm-3"><strong id="InstanceSessionTimeout"></strong></label>
                            </div>
                            <div class="row">
                                <div class="col-sm-5">
                                    <button type="button" class="btn btn-info" onclick="ReloadInstance();"><i class="fa fa-recycle"></i>&nbsp;Recargar</button></div>
                            </div>
                            <div class="row">
                                <label class="col-sm-2">Usuaris externs:&nbsp;</label>
                                <div class="col-sm-5">
                                    <ul></ul>
                                </div>
                            </div>
                            <div class="row">
                                <h4>Funcionalitats</h4>
                                <div class="col-xs-6">
                                    <div class="col-xs-1"></div>
                                    <div class="col-sm-11"><i class="far fa-ban red"></i>&nbsp;Preguntas frecuentes (FAQs)</div>
                                </div>
                                <div class="col-xs-6">
                                    <div class="col-xs-1"></div>
                                    <div class="col-sm-11"><i class="far fa-check green"></i>&nbsp;Permetre seguimient</div>
                                </div>
                                <div class="col-xs-6">
                                    <div class="col-xs-1"></div>
                                    <div class="col-sm-11"><i class="far fa-check green"></i>&nbsp;Alertes</div>
                                </div>
                                <div class="col-xs-6">
                                    <div class="col-xs-1"></div>
                                    <div class="col-sm-11"><i class="far fa-ban red"></i>&nbsp;Codi PIN</div>
                                </div>
                                <div class="col-xs-6">
                                    <div class="col-xs-1"></div>
                                    <div class="col-sm-11"><i class="far fa-ban red"></i>&nbsp;Portal</div>
                                </div>
                                <div class="col-xs-6">
                                    <div class="col-xs-1"></div>
                                    <div class="col-sm-11"><i class="far fa-ban red"></i>&nbsp;Visualització per àmbits activada</div>
                                </div>
                                <div class="col-xs-6">
                                    <div class="col-xs-1"></div>
                                    <div class="col-sm-11"><i class="far fa-ban red"></i>&nbsp;Recordatoris</div>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
                <div id="tab-fields" class="tab-pane">
                    <div class="hpanel">
                        <div class="table-responsive" id="ListFieldsScrollTableDiv">
                            <table class="table table-bordered table-striped" style="margin: 0">
                                <thead class="thin-border-bottom">
                                    <tr>
                                        <th onclick="Sort(this,'ListFieldsTable');" id="th0" class=" " style="width: 200px;">Nombre</th>
                                        <th onclick="Sort(this,'ListFieldsTable');" id="th1" class=" ">Etiqueta</th>
                                        <th onclick="Sort(this,'ListFieldsTable');" id="th2" class=" " style="width: 150px;">Tipo</th>
                                        <th onclick="Sort(this,'ListFieldsTable');" id="th3" class=" " style="width: 50px;">Obl.</th>
                                        <th onclick="Sort(this,'ListFieldsTable');" id="th4" class=" " style="width: 90px;">Tamaño</th>
                                        <th onclick="Sort(this,'ListFieldsTable');" id="th5" class=" " style="width: 50px;">FK</th>
                                        <th onclick="Sort(this,'ListFieldsTable');" id="th6" class=" " style="width: 150px;">Referencia a...</th>
                                        <th style="width: 107px;">&nbsp;</th>
                                    </tr>
                                </thead>
                            </table>
                            <div id="ListFieldsDiv" style="overflow: hidden scroll; padding: 0px; height: 501px;">
                                <table class="table table-bordered table-striped" style="border-top: none;">
                                    <tbody id="ListFieldsTable"></tbody>
                                </table>
                            </div>
                            <table class="table table-bordered table-striped" style="margin: 0">
                                <thead class="this-border-bottom">
                                    <tr>
                                        <th style="color: #aaa;"><i>Nº de camps:&nbsp;<span id="ListFieldsTotal"></span></i></th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                </div>
                <div id="tab-sql" class="tab-pane">
                    <div class="hpanel">
                        <pre id="ItemSQLCreate"></pre>
                    </div>
                </div>
                <div id="tab-scripts" class="tab-pane">                    
                    <div class="hpanel">
                    </div>
                </div>
                <div id="tab-dictionary" class="tab-pane">
                    <div class="hpanel">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentScriptFiles" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentScripts" Runat="Server">
    <script type="text/javascript">
        var ItemDefinitions = LocalStorageGetJson("ItemDefinitions");
        var ItemDefinitionId = <%=this.ItemDefinitionId %>;
        var ItemDefinition = ItemDefinitionById(ItemDefinitionId);
    </script>
</asp:Content>