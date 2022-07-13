<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Instance.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
    <style type="text/css">
        .itemDiv {
            padding: 6px;
            cursor:pointer;
        }

        .itemDiv table {
            border:1px solid #aaa;
            background-color:#eee;
            width:100%;
            padding:6px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentScriptVars" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="row" id="PersistentFields">
        <div class="form-group col-xs-12">
            <div class="row">
                <label id="Code_Label" class="col-sm-2 control-label">Código</label>
                <div class="col-sm-6"><span id="ItemDefinitionDescriptionTitle"></span></div>
                <div class="col-xs-4">
                </div>
            </div>
        </div>
    </div>
    <div class="col-lg-12">
        <div class="hpanel">
            <ul class="nav nav-tabs" id="FormTabs">
                <li id="tabSelect-common" class="tabSelect active"><a data-toggle="tab" href="#tab-common" aria-expanded="false">Configuració</a></li>
                <li id="tabSelect-items" class="tabSelect"><a data-toggle="tab" href="#tab-items" aria-expanded="false">Ítems</a></li>
                <li id="tabSelect-fixedLists" class="tabSelect"><a data-toggle="tab" href="#tab-fixedLists" aria-expanded="false">Llistes</a></li>
                <li id="tabSelect-scripts" class="tabSelect"><a data-toggle="tab" href="#tab-scripts" aria-expanded="false">Scripts</a></li>
                <li id="tabSelect-dictionary" class="tabSelect"><a data-toggle="tab" href="#tab-dictionary" aria-expanded="false">Diccionari</a></li>
                <li id="tabSelect-profile" class="tabSelect"><a data-toggle="tab" href="#tab-features" aria-expanded="false">Perfil d'usuari</a></li>
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
                <div id="tab-items" class="tab-pane">
                    <div class="hpanel">
                        <div id="ItemsDiv"></div>
                    </div>
                </div>
                <div id="tab-fixedLists" class="tab-pane">
                    <div class="hpanel">
                        <div id="FixedListsDiv"></div>
                    </div>
                </div>
                <div id="tab-scripts" class="tab-pane">                    
                    <div class="hpanel">
                       <dl>
 <dt><dfn>happiness</dfn></dt>
 <dd class="pronunciation">/ˈhæpinəs/</dd>
 <dd class="part-of-speech"><i><abbr>n.</abbr></i></dd>
 <dd>The state of being happy.</dd>
 <dd>Good fortune; success. <q>Oh <b>happiness</b>! It worked!</q></dd>
 <dt><dfn>rejoice</dfn></dt>
 <dd class="pronunciation">/rɪˈdʒɔɪs/</dd>
 <dd><i class="part-of-speech"><abbr>v.intr.</abbr></i> To be delighted oneself.</dd>
 <dd><i class="part-of-speech"><abbr>v.tr.</abbr></i> To cause one to be delighted.</dd>
</dl>
                    </div>
                </div>
                <div id="tab-dictionary" class="tab-pane">
                    <div class="hpanel">
                    </div>
                </div>
                <div id="tab-profile" class="tab-pane">
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
    </script>
</asp:Content>