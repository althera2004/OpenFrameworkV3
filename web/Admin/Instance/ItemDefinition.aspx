<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ItemDefinition.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Instance.ItemDefinition" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
    <link rel="stylesheet" href="/assets/css/codemirror.css" />

    <style type="text/css">
        #TableHeader td {padding:4px;}

        input[type=checkbox] {
            margin: 5px 0 0 0 !important;
            height: 15px !important;
        }

        h4 {
            margin-top:12px;
            border-bottom:1px dashed;
        }

        .CodeMirror-line {margin-left: 0px!important;padding: 0 40px!important;}

        .cm-s-default{ border:1px solid #ccc;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentScriptVars" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="row" id="PersistentFields">
        <div class="form-group col-xs-12">
            <table id="TableHeader">
                <tr>
                    <td rowspan="2" style="width:60px;"><span id="ItemDefinitionIcon"></span></td>
                    <td style="width:60px;">Id: <strong><span id="ItemDefinitionId"></span></strong></td>
                    <td>Nom: <strong><span id="ItemDefinitionName"></span></strong></td>
                </tr>
                <tr>
                    <td colspan="2">Descripció: <i><span id="ItemDefinitionDescription"></span></i></td>
                </tr>
            </table>
        </div>
    </div>
    <div class="col-lg-12">
        <div class="hpanel">
            <ul class="nav nav-tabs" id="FormTabs">
                <li id="tabSelect-common" class="tabSelect active"><a data-toggle="tab" href="#tab-common" aria-expanded="false">Configuració</a></li>
                <li id="tabSelect-definition" class="tabSelect"><a data-toggle="tab" href="#tab-definition" aria-expanded="false">Definició</a></li>
                <li id="tabSelect-fields" class="tabSelect"><a data-toggle="tab" href="#tab-fields" aria-expanded="false">Camps</a></li>
                <li id="tabSelect-sql" class="tabSelect"><a data-toggle="tab" href="#tab-sql" aria-expanded="false">SQL</a></li>
                <li id="tabSelect-scripts" class="tabSelect"><a data-toggle="tab" href="#tab-scripts" aria-expanded="false">Scripts</a></li>
                <li id="tabSelect-dictionary" class="tabSelect"><a data-toggle="tab" href="#tab-dictionary" aria-expanded="false">Diccionari</a></li>
            </ul>
            <div class="tab-content" id="FormContent">
                <div id="tab-common" class="tab-pane active">
                    <div class="hpanel">
                        <form class="form-horizontal" role="form">
                            <h4>Presentació</h4>
                            <div class="row">
                                <label class="col-sm-1">Singular:&nbsp;</label>
                                <div class="col-sm-5">
                                    <input type="text" id="TxtLayoutLabel" class="col-xs-12 col-sm-12 tooltip-info" onblur="this.value=$.trim(this.value);" spellcheck="false" /></div>
                                <label class="col-sm-1">Plural:&nbsp;</label>
                                <div class="col-sm-5">
                                    <input type="text" id="TxtLayoutLabelPlural" class="col-xs-12 col-sm-12 tooltip-info" onblur="this.value=$.trim(this.value);" spellcheck="false" /></div>
                            </div>
                            <h4>Funcionalitats</h4>
                            <div class="row">
                                <div class="col-sm-2">
                                    <label>
                                        <input type="checkbox" checked="" value="0" id="RBFeature1" name="RBFeature" />
                                        Adjunts
                                    </label>
                                </div>
                                <div class="col-sm-2">
                                    <label>
                                        <input type="checkbox" checked="" value="1" id="RBFeature2" name="RBFeature" />
                                        Sticky
                                    </label>
                                </div>
                                <div class="col-sm-2">
                                    <label>
                                        <input type="checkbox" checked="" value="2" id="RBFeature3" name="RBFeature" />
                                        Seguiment
                                    </label>
                                </div>
                                <div class="col-sm-2">
                                    <label>
                                        <input type="checkbox" checked="" value="2" id="RBFeature4" name="RBFeature" />
                                        Geolocalització
                                    </label>
                                </div>
                                <div class="col-sm-2">
                                    <label>
                                        <input type="checkbox" checked="" value="2" id="RBFeature5" name="RBFeature" />
                                        Agendable
                                    </label>
                                </div>
                                <div class="col-sm-2">
                                    <label>
                                        <input type="checkbox" checked="" value="2" id="RBFeature6" name="RBFeature" />
                                        Asignar usuari
                                    </label>
                                </div>
                            </div>
                            <h4>Traçabilitat</h4>
                            <div class="row">
                                <div class="col-sm-2">
                                    <label>
                                        <input type="radio" checked="" value="0" id="RBTrace0" name="RBTrace" /> Desactivada
                                    </label>
                                </div>
                                <div class="col-sm-2">
                                    <label>
                                        <input type="radio" checked="" value="1" id="RBTrace1" name="RBTrace" /> Opcions bàsiques
                                    </label>
                                </div>
                                <div class="col-sm-8">
                                    <label>
                                        <input type="radio" checked="" value="2" id="RBTrace2" name="RBTrace" /> Complerta
                                    </label>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
                <div id="tab-definition" class="tab-pane">
                    <div class="hpanel">
                        <textarea id="ItemDefinition" rows="25"><%=this.ItemDefintionJson %></textarea>
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
                                    <tbody id="ListFieldsTableNew">
                                        <tr>
                                            <td style="width:200px;"><input type="text" id="FieldNewName" style="width:180px;" /></td>
                                            <td><input type="text" id="FieldNewLabel" /></td>
                                            <td style="width:150px;">
                                                <select id="FieldNeType" style="width:130px;">
                                                    <option value="text">Text</option>
                                                    <option value="textarea">Text llarg</option>
                                                    <option value="url">Url</option>
                                                    <option value="email">Email</option>
                                                    <option value="fixedlist">Llista fixa</option>
                                                    <option value="boolean">Sí/No</option>
                                                    <option value="int">Nombre sencer</option>
                                                    <option value="long">Nombre llarg</option>
                                                    <option value="decimal">Nombre decimal</option>
                                                    <option value="money">Moneda</option>
                                                    <option value="datetime">Data</option>
                                                    <option value="documentfile">Document</option>
                                                    <option value="imagefile">Imatge</option>
                                                    <option value="applicationuser">Usuari</option>
                                                </select>
                                            </td>
                                            <td style="width:50px;"><input type="checkbox" id="FieldNewRequired" /></td>
                                            <td style="width:90px;"><input type="text" id="FieldNewSize" style="width:70px;" /></td>
                                            <td style="width:50px;"><input type="checkbox" id="FieldNewFK" /></td>
                                            <td style="width:150px;"><input type="text" id="FieldNewReference" style="width:130px;" /></td>
                                        </tr>
                                    </tbody>
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
                        <textarea id="ItemSQLCreate" rows="25"></textarea>
                    </div>
                </div>
                <div id="tab-scripts" class="tab-pane">                    
                    <div class="hpanel">
                        <textarea id="Editor" rows="25">
// Demo code (the actual new parser character stream implementation)

function StringStream(string) {
  this.pos = 0;
  this.string = string;
}

StringStream.prototype = {
  done: function() {return this.pos >= this.string.length;},
  peek: function() {return this.string.charAt(this.pos);},
  next: function() {
    if (this.pos &lt; this.string.length)
      return this.string.charAt(this.pos++);
  },
  eat: function(match) {
    var ch = this.string.charAt(this.pos);
    if (typeof match == "string") var ok = ch == match;
    else var ok = ch &amp;&amp; match.test ? match.test(ch) : match(ch);
    if (ok) {this.pos++; return ch;}
  },
  eatWhile: function(match) {
    var start = this.pos;
    while (this.eat(match));
    if (this.pos > start) return this.string.slice(start, this.pos);
  },
  backUp: function(n) {this.pos -= n;},
  column: function() {return this.pos;},
  eatSpace: function() {
    var start = this.pos;
    while (/\s/.test(this.string.charAt(this.pos))) this.pos++;
    return this.pos - start;
  },
  match: function(pattern, consume, caseInsensitive) {
    if (typeof pattern == "string") {
      function cased(str) {return caseInsensitive ? str.toLowerCase() : str;}
      if (cased(this.string).indexOf(cased(pattern), this.pos) == this.pos) {
        if (consume !== false) this.pos += str.length;
        return true;
      }
    }
    else {
      var match = this.string.slice(this.pos).match(pattern);
      if (match &amp;&amp; consume !== false) this.pos += match[0].length;
      return match;
    }
  }
};</textarea>
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
    <!--script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.52.2/codemirror.js"></!--script-->
    <script type="text/javascript" src="/assets/js/codemirror.js"></script>
    <script type="text/javascript" src="/assets/js/codemirror_sql.js"></script>
    <script type="text/javascript" src="/assets/js/codemirror_javascript.js"></script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentScripts" Runat="Server">
    <script type="text/javascript">
        var ItemDefinitions = LocalStorageGetJson("ItemDefinitions");
        var ItemDefinitionId = <%=this.ItemDefinitionId %>;
        var ItemDefinition = ItemDefinitionById(ItemDefinitionId);
    </script>
</asp:Content>