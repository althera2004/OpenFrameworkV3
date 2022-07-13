<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Dictionary.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Feature.Dictionary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentScriptVars" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="row" id="PersistentFields">
        <div class="form-group col-xs-12">
            <div class="row">
                <label id="Code_Label" class="col-sm-2 control-label">Seleccionar idioma</label>
                <div class="col-sm-6">
                    <select id="CmbLanguage" onchange="GetDictionaryData();">
                        <option value="ca-es">Català</option>
                        <option value="es-es">Castellano</option>
                    </select>
                </div>
                <div class="col-xs-4">
                </div>
            </div>
        </div>
    </div>
    <div class="col-lg-12">
        <div class="hpanel">
            <ul class="nav nav-tabs" id="FormTabs">
                <li id="tabSelect-common" class="tabSelect active"><a data-toggle="tab" href="#tab-common" aria-expanded="false">Comuns</a></li>
                <li id="tabSelect-core" class="tabSelect"><a data-toggle="tab" href="#tab-core" aria-expanded="false">Core</a></li>
                <li id="tabSelect-billing" class="tabSelect"><a data-toggle="tab" href="#tab-billing" aria-expanded="false">Facturació</a></li>
                <li id="tabSelect-features" class="tabSelect"><a data-toggle="tab" href="#tab-features" aria-expanded="false">Funcionalitats</a></li>
            </ul>
            <div class="tab-content" id="FormContent">
                <div id="tab-common" class="tab-pane active">
                    <div class="hpanel">
                        <div class="row">
                            <div id="CorpusCommon_List">
                                <div class="hpanel hblue" style="margin: 0;" id="CorpusCommon_PanelBody">
                                    <div class="panel-heading hbuilt"><span id="CorpusCommon_ListTitle">Instàncies de client</span>
                                        <div class="panel-tools"></div>
                                    </div>
                                    <div class="tableHead">
                                        <table class="table">
                                            <thead id="CorpusCommon_ListHead">
                                                <tr>
                                                    <th id="th0" style="width: -47px;" class="">Clau</th>
                                                    <th style="padding: 0; width: 61px; border-right: none;"></th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>
                                    <div class="panel-body2 panel-body-list-inForm" id="CorpusCommon_PanelBodyList">
                                        <div class="table-responsive" style="max-height: 100%; height: 100%; overflow-y: scroll; overflow-x: hidden">
                                            <div class="table-body" style="max-height: 100%; height: 100%">
                                                <table class="table" style="max-height: 100%" cellpadding="1" cellspacing="1">
                                                    <tbody id="CorpusCommon_ListBody" style="display: none;"></tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel-footer">Nº de claves: <strong id="CorpusCommon_ListCount">0</strong>      </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="tab-core" class="tab-pane">
                    <div class="hpanel">
                        <div class="row">
                            <div id="CorpusCore_List">
                                <div class="hpanel hblue" style="margin: 0;" id="CorpusCore_PanelBody">
                                    <div class="panel-heading hbuilt"><span id="CorpusCore_ListTitle">Instàncies de client</span>
                                        <div class="panel-tools"></div>
                                    </div>
                                    <div class="tableHead">
                                        <table class="table">
                                            <thead id="CorpusCore_ListHead">
                                                <tr>
                                                    <th id="th0" style="width: -47px;" class="">Clau</th>
                                                    <th style="padding: 0; width: 61px; border-right: none;"></th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>
                                    <div class="panel-body2 panel-body-list-inForm" id="CorpusCore_PanelBodyList">
                                        <div class="table-responsive" style="max-height: 100%; height: 100%; overflow-y: scroll; overflow-x: hidden">
                                            <div class="table-body" style="max-height: 100%; height: 100%">
                                                <table class="table" style="max-height: 100%" cellpadding="1" cellspacing="1">
                                                    <tbody id="CorpusCore_ListBody" style="display: none;"></tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel-footer">Nº de claves: <strong id="CorpusCore_ListCount">0</strong>      </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="tab-billing" class="tab-pane">                    
                    <div class="hpanel">
                        <div class="row">
                            <div id="CorpusBilling_List">
                                <div class="hpanel hblue" style="margin: 0;" id="CorpusBilling_PanelBody">
                                    <div class="panel-heading hbuilt"><span id="CorpusBilling_ListTitle">Instàncies de client</span>
                                        <div class="panel-tools"></div>
                                    </div>
                                    <div class="tableHead">
                                        <table class="table">
                                            <thead id="CorpusBilling_ListHead">
                                                <tr>
                                                    <th id="th0" style="width: -47px;" class="">Clau</th>
                                                    <th style="padding: 0; width: 61px; border-right: none;"></th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>
                                    <div class="panel-body2 panel-body-list-inForm" id="CorpusBilling_PanelBodyList">
                                        <div class="table-responsive" style="max-height: 100%; height: 100%; overflow-y: scroll; overflow-x: hidden">
                                            <div class="table-body" style="max-height: 100%; height: 100%">
                                                <table class="table" style="max-height: 100%" cellpadding="1" cellspacing="1">
                                                    <tbody id="CorpusBilling_ListBody" style="display: none;"></tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel-footer">Nº de claves: <strong id="CorpusBilling_ListCount">0</strong>      </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="tab-features" class="tab-pane">
                    <div class="hpanel">
                        <div class="row">
                            <div id="CorpusFeature_List">
                                <div class="hpanel hblue" style="margin: 0;" id="CorpusFeature_PanelBody">
                                    <div class="panel-heading hbuilt"><span id="CorpusFeature_ListTitle">Funcionalitats</span>
                                        <div class="panel-tools"></div>
                                    </div>
                                    <div class="tableHead">
                                        <table class="table">
                                            <thead id="CorpusFeature_ListHead">
                                                <tr>
                                                    <th id="th0" style="width: -47px;" class="">Clau</th>
                                                    <th style="padding: 0; width: 61px; border-right: none;"></th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>
                                    <div class="panel-body2 panel-body-list-inForm" id="CorpusFeature_PanelBodyList">
                                        <div class="table-responsive" style="max-height: 100%; height: 100%; overflow-y: scroll; overflow-x: hidden">
                                            <div class="table-body" style="max-height: 100%; height: 100%">
                                                <table class="table" style="max-height: 100%" cellpadding="1" cellspacing="1">
                                                    <tbody id="CorpusFeature_ListBody" style="display: none;"></tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel-footer">Nº de claves: <strong id="CorpusFeature_ListCount">0</strong>      </div>
                                </div>
                            </div>
                        </div>
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
        var CorpusData = [];
    </script>
</asp:Content>