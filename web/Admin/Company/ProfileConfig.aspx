<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ProfileConfig.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Company.ProfileConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentScriptVars" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="col-lg-12">
             <div class="hpanel hblue">
                 <div class="panel-heading hbuilt">
                     Datos del perfil de usuario
                 </div>
                 <div class="panel-body">
                     <div class="row form-group">
                         <label class="col-sm-3 control-label">Nom de l'usuari</label>
                         <div class="col-sm-2">
                             <label>
                                 <input type="radio" checked="" value="option1" id="RBUserName1" name="RBUserName" <%= this.UserProfile.UserName == 1 ?  "checked=\"checked\"" : string.Empty %> />
                                 Nombre
                             </label>
                         </div>
                         <div class="col-sm-2">
                             <label>
                                 <input type="radio" checked="" value="option1" id="RBUserName2" name="RBUserName" <%= this.UserProfile.UserName == 2 ?  "checked=\"checked\"" : string.Empty %> />
                                 Nombre y apellidos
                             </label>
                         </div>
                         <div class="col-sm-4">
                             <label>
                                 <input type="radio" checked="" value="option1" id="RBUserName3" name="RBUserName" <%= this.UserProfile.UserName == 3 ?  "checked=\"checked\"" : string.Empty %> />
                                 Nombre, primer apellido y segundo apellido
                             </label>
                         </div>
                         <small class="col-sm-12 help-block">Cómo se muestra el nombre del usuario.</small>
                     </div>
                 </div>
                 <div class="panel-body" style="padding:6px;">
                     <h5>Dades de contacte</h5>
                     <div class="row">
                        <div class="col-xs-4" style="padding-left:12px;">
                             <input type="checkbox" style="margin:5px 2px 0 5px!important;" <%= this.UserProfile.Email ?  "checked=\"checked\"" : string.Empty %> />&nbsp;
                             <label>Correu</label>
                        </div>
                        <div class="col-xs-4">
                             <input type="checkbox" style="margin:5px 2px 0 5px!important;" <%= this.UserProfile.AlternativeEmail ?  "checked=\"checked\"" : string.Empty %> />&nbsp;
                             <label>Correu alternatiu</label>
                        </div>
                        <div class="col-xs-4" style="padding-left:12px;">
                             <input type="checkbox" style="margin:5px 2px 0 5px!important;" <%= this.UserProfile.Phone ?  "checked=\"checked\"" : string.Empty %> />&nbsp;
                             <label>Teléfon</label>
                        </div>
                        <div class="col-xs-4">
                             <input type="checkbox" style="margin:5px 2px 0 5px!important;" <%= this.UserProfile.Mobile ?  "checked=\"checked\"" : string.Empty %> />&nbsp;
                             <label>Mòbil</label>
                        </div>
                        <div class="col-xs-4">
                             <input type="checkbox" style="margin:5px 2px 0 5px!important;" <%= this.UserProfile.EmergencyPhone ?  "checked=\"checked\"" : string.Empty %> />&nbsp;
                             <label>Telèfon d'emergència</label>
                        </div>
                        <div class="col-xs-4">
                             <input type="checkbox" style="margin:5px 2px 0 5px!important;" <%= this.UserProfile.Fax ?  "checked=\"checked\"" : string.Empty %> />&nbsp;
                             <label>Fax</label>
                        </div>
                    </div>
                 </div>
                 <div class="panel-body" style="padding:6px;">
                     <h5>Dades personals</h5>
                     <div class="row">
                        <div class="col-xs-3">
                             <input type="checkbox" style="margin:5px 2px 0 5px!important;" <%= this.UserProfile.Gender ?  "checked=\"checked\"" : string.Empty %> />&nbsp;
                             <label>Género</label>
                        </div>
                        <div class="col-xs-3" style="padding-left:12px;">
                             <input type="checkbox" style="margin:5px 2px 0 5px!important;" <%= this.UserProfile.Birthday ?  "checked=\"checked\"" : string.Empty %> />&nbsp;
                             <label>Data naixement</label>
                        </div>
                        <div class="col-xs-3" style="padding-left:12px;">
                             <input type="checkbox" style="margin:5px 2px 0 5px!important;" <%= this.UserProfile.IdentificationCard ?  "checked=\"checked\"" : string.Empty %> />&nbsp;
                             <label>NIF/DNI/Passaport</label>
                        </div>
                        <div class="col-xs-3">
                             <input type="checkbox" style="margin:5px 2px 0 5px!important;" <%= this.UserProfile.Nacionality ?  "checked=\"checked\"" : string.Empty %> />&nbsp;
                             <label>Nacionalidad</label>
                        </div>
                    </div>
                 </div>
                 <div class="panel-body" style="padding:6px;">
                     <h5>Xarxes socials</h5>
                     <div class="row">
                        <div class="col-xs-3">
                             <input type="checkbox" style="margin:5px 2px 0 5px!important;" <%= this.UserProfile.LinkedIn ?  "checked=\"checked\"" : string.Empty %> />&nbsp;
                             <label>LinkedIn</label>
                        </div>
                        <div class="col-xs-3" style="padding-left:12px;">
                             <input type="checkbox" style="margin:5px 2px 0 5px!important;" <%= this.UserProfile.Twitter ?  "checked=\"checked\"" : string.Empty %> />&nbsp;
                             <label>Twitter</label>
                        </div>
                        <div class="col-xs-3" style="padding-left:12px;">
                             <input type="checkbox" style="margin:5px 2px 0 5px!important;" <%= this.UserProfile.Instagram ?  "checked=\"checked\"" : string.Empty %> />&nbsp;
                             <label>Instagram</label>
                        </div>
                        <div class="col-xs-3">
                             <input type="checkbox" style="margin:5px 2px 0 5px!important;" <%= this.UserProfile.Facebook ?  "checked=\"checked\"" : string.Empty %> />&nbsp;
                             <label>Facebook</label>
                        </div>
                    </div>
                 </div>
             </div>
             <div class="hpanel hblue">
                 <div class="panel-heading hbuilt">
                    Dades personalitzades
                 </div>
                 <div class="panel-body">
                    <h6><strong>Text</strong></h6>
                     <div class="col-xs-4">
                         <input type="checkbox" id="ChkText1" onclick="USERPROFILECONFIG_ChkTextChanged(1);" style="margin:5px 2px 0 5px!important;" <%= string.IsNullOrEmpty(this.UserProfile.Text1) ? string.Empty : " checked=\"checked\"" %> />&nbsp;
                         <label>Nom:</label>
                         <input type="text" id="Text1" value="<%= this.UserProfile.Text1 %>" <%= string.IsNullOrEmpty(this.UserProfile.Text1) ? " disabled=\"disabled\"" : string.Empty %> />
                     </div>
                     <div class="col-xs-4">
                         <input type="checkbox" id="ChkText2" onclick="USERPROFILECONFIG_ChkTextChanged(2);" style="margin:5px 2px 0 5px!important;" <%= string.IsNullOrEmpty(this.UserProfile.Text2) ? string.Empty : " checked=\"checked\"" %> />&nbsp;
                         <label>Nom:</label>
                         <input type="text" id="Text2" value="<%= this.UserProfile.Text2 %>" <%= string.IsNullOrEmpty(this.UserProfile.Text2) ? " disabled=\"disabled\"" : string.Empty %> />
                     </div>
                     <div class="col-xs-4">
                         <input type="checkbox" id="ChkText3" onclick="USERPROFILECONFIG_ChkTextChanged(3);" style="margin:5px 2px 0 5px!important;" <%= string.IsNullOrEmpty(this.UserProfile.Text3) ? string.Empty : " checked=\"checked\"" %> />&nbsp;
                         <label>Nom:</label>
                         <input type="text" id="Text3" value="<%= this.UserProfile.Text3 %>" <%= string.IsNullOrEmpty(this.UserProfile.Text3) ? " disabled=\"disabled\"" : string.Empty %> />
                     </div>
                 </div>
                 <div class="panel-body">
                    <h6><strong>Documents</strong></h6>
                     <<div class="col-xs-4">
                         <input type="checkbox" id="ChkDoc1" onclick="USERPROFILECONFIG_ChkDocChanged(1);" style="margin:5px 2px 0 5px!important;" <%= string.IsNullOrEmpty(this.UserProfile.Doc1) ? string.Empty : " checked=\"checked\"" %> />&nbsp;
                         <label>Nom:</label>
                         <input type="text" id="Doc1" value="<%= this.UserProfile.Doc1 %>" <%= string.IsNullOrEmpty(this.UserProfile.Doc1) ? " disabled=\"disabled\"" : string.Empty %> />
                     </div>
                     <div class="col-xs-4">
                         <input type="checkbox" id="ChkDoc2" onclick="USERPROFILECONFIG_ChkDocChanged(2);" style="margin:5px 2px 0 5px!important;" <%= string.IsNullOrEmpty(this.UserProfile.Doc2) ? string.Empty : " checked=\"checked\"" %> />&nbsp;
                         <label>Nom:</label>
                         <input type="text" id="Doc2" value="<%= this.UserProfile.Doc2 %>" <%= string.IsNullOrEmpty(this.UserProfile.Doc2) ? " disabled=\"disabled\"" : string.Empty %> />
                     </div>
                     <div class="col-xs-4">
                         <input type="checkbox" id="ChkDoc3" onclick="USERPROFILECONFIG_ChkDocChanged(3);" style="margin:5px 2px 0 5px!important;" <%= string.IsNullOrEmpty(this.UserProfile.Doc3) ? string.Empty : " checked=\"checked\"" %> />&nbsp;
                         <label>Nom:</label>
                         <input type="text" id="Doc3" value="<%= this.UserProfile.Doc3 %>" <%= string.IsNullOrEmpty(this.UserProfile.Doc3) ? " disabled=\"disabled\"" : string.Empty %> />
                     </div>
                 </div>
             </div>
         </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentScriptFiles" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentScripts" Runat="Server">
</asp:Content>

