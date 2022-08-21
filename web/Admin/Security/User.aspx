<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="User.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Security.User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentScriptVars" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="row" id="PersistentFields">
        <div class="form-group col-xs-12">
            <div class="row">
                <label id="Email_Label" class="col-sm-1 control-label">Email</label>
                <div class="col-sm-3">
                    <input id="Email" type="text" class="form-control" value="" />
                </div>
                <label id="Name_Label" class="col-sm-1 control-label">Nom</label>
                <div class="col-sm-2">
                    <input id="FirstName" type="text" class="form-control" value="" />
                </div>
                <label id="LastName_Label" class="col-sm-1 control-label">Cognoms</label>
                <div class="col-sm-4">
                    <input id="LastName" type="text" class="form-control" style="width: 45%; float: left;" value="" />
                    <input id="LastName2" type="text" class="form-control" style="width: 45%; float: left; margin-left: 8px;" value="" />
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <div class="hpanel">
                <ul class="nav nav-tabs" id="FormTabs">
                    <li id="tabSelect-basic" class="tabSelect active"><a data-toggle="tab" href="#tab-basic" aria-expanded="false">Dades personals</a></li>
                    <li id="tabSelect-calibratge" class="tabSelect"><a data-toggle="tab" href="#tab-access" aria-expanded="false">Tipus d'usuari</a></li>
                    <li id="tabSelect-varificacio" class="tabSelect"><a data-toggle="tab" href="#tab-grants" aria-expanded="false">Permissos</a></li>
                    <li id="tabSelect-Attachs" class="tabSelect"><a data-toggle="tab" href="#tab-Attachs" aria-expanded="false"><i class="fa fa-paperclip"></i>&nbsp;Adjunts</a></li>
                </ul>
                <div class="tab-content">
                    <div id="tab-basic" class="tab-pane active">
                        <div class="panel-body panel-body-form">
                            <div class="panel-heading">Dades personals</div>
                            <div class="panel-body form-group" style="border: none;">

                                <div class="col-xs-4" id="DivGender" style="margin-top: 8px; display: none;">
                                    <label class="col-sm-3">Sexe</label>
                                    <div class="col-sm-9">
                                        <input type="radio" name="RBGener" />&nbsp;Home&nbsp;&nbsp;
                                        <input type="radio" name="RBGener" />&nbsp;Dona&nbsp;&nbsp;
                                        <input type="radio" name="RBGener" />&nbsp;Grup
                                    </div>
                                </div>

                                <div class="col-xs-4" id="DivBirthDate" style="margin-top: 8px; display: none;">
                                    <label class="col-sm-4">D.Naixement</label>
                                    <div class="col-sm-8">
                                        <input id="BirthDate" type="text" class="form-control" value="" />
                                    </div>
                                </div>

                                <div class="col-xs-4" id="DivIdentificationCard" style="margin-top: 8px; display: none;">
                                    <label class="col-sm-6">Document identitat</label>
                                    <div class="col-sm-6">
                                        <input id="IdentificationCard" type="text" class="form-control" value="" />
                                    </div>
                                </div>

                                <div class="col-xs-4" id="DivNacionality" style="margin-top: 8px; display: none;">
                                    <label class="col-sm-6">Nacionalitat</label>
                                    <div class="col-sm-6">
                                        <input id="Nacionality" type="text" class="form-control" value="" />
                                    </div>
                                </div>
                            </div>
                            <div class="panel-heading">Dades de contacte</div>
                            <div class="panel-body form-group" style="border: none;">
                                <div class="col-xs-6" id="DivEmailAlternative" style="margin-top: 8px; display: none;">
                                    <label class="col-sm-4">Email alternatiu</label>
                                    <div class="col-sm-8">
                                        <input id="EmailAlternative" type="text" class="form-control" value="" />
                                    </div>
                                </div>

                                <div class="col-xs-3" id="DivMobile" style="margin-top: 8px; display: none;">
                                    <label class="col-sm-2">Mòvil</label>
                                    <div class="col-sm-10">
                                        <input id="Mobile" type="text" class="form-control" value="" />
                                    </div>
                                </div>

                                <div class="col-xs-3" id="DivFax" style="margin-top: 8px; display: none;">
                                    <label class="col-sm-2">Fax</label>
                                    <div class="col-sm-10">
                                        <input id="Fax" type="text" class="form-control" value="" />
                                    </div>
                                </div>
                            </div>
                            <div class="panel-heading" id="SocialTitle" style="display: none;">Xarxes socials</div>
                            <div class="panel-body form-group" id="SocialData" style="border: none; display: none;">
                                <div class="col-xs-6" id="DivLinkedIn" style="margin-top: 8px; display: none;">
                                    <label class="col-sm-3"><i class="fab fa-linkedin"></i>&nbsp;LinkedIn</label>
                                    <div class="col-sm-9">
                                        <input id="LinkedIn" type="text" class="form-control" value="" />
                                    </div>
                                </div>
                                <div class="col-xs-6" id="DivTwitter" style="margin-top: 8px; display: none;">
                                    <label class="col-sm-3"><i class="fab fa-twitter"></i>&nbsp;Twitter</label>
                                    <div class="col-sm-9">
                                        <input id="Twitter" type="text" class="form-control" value="" />
                                    </div>
                                </div>
                                <div class="col-xs-6" id="DivInstagram" style="margin-top: 8px; display: none;">
                                    <label class="col-sm-3"><i class="fab fa-instagram"></i>&nbsp;Instagram</label>
                                    <div class="col-sm-9">
                                        <input id="Instagram" type="text" class="form-control" value="" />
                                    </div>
                                </div>
                                <div class="col-xs-6" id="DivFacebook" style="margin-top: 8px; display: none;">
                                    <label class="col-sm-3"><i class="fab fa-facebook"></i>&nbsp;Facebook</label>
                                    <div class="col-sm-9">
                                        <input id="Facebook" type="text" class="form-control" value="" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="tab-access" class="tab-pane">
                        <div class="panel-body panel-body-form">
                            <div class="panel-heading">Tipus d'usuari especial</div>
                            <div class="panel-body form-group" style="border: none;">
                                <div class="col-sm-2">
                                    <label class="checkbox-inline">
                                        <input type="checkbox" value="option1" id="Core" />&nbsp;CORE
                                    </label>
                                </div>
                                <div class="col-sm-2">
                                    <label class="checkbox-inline">
                                        <input type="checkbox" value="option1" id="Tech" />&nbsp;Tècnic
                                    </label>
                                </div>
                                <div class="col-sm-2">
                                    <label class="checkbox-inline">
                                        <input type="checkbox" value="option1" id="Owner" />&nbsp;Propietari
                                    </label>
                                </div>
                                <div class="col-sm-2">
                                    <label class="checkbox-inline">
                                        <input type="checkbox" value="option1" id="Corporative" />&nbsp;Corporatiu
                                    </label>
                                </div>
                                <div class="col-sm-2">
                                    <label class="checkbox-inline">
                                        <input type="checkbox" value="option1" id="External" />&nbsp;Extern
                                    </label>
                                </div>
                            </div>
                            <div class="panel-heading">Pertinença a grups</div>
                            <div>
                                <select multiple="multiple" size="10" name="duallistbox_demo2" class="demo2" title="duallistbox_demo2">
                                    <asp:Literal runat="server" ID="LtGroups"></asp:Literal>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div id="tab-grants" class="tab-pane">
                        <div class="panel-body panel-body-form">
                            <h5>Accés a dades</h5>
                            <h5>Permissos especials</h5>
                        </div>
                    </div>
                    <div id="tab-Attachs" class="tab-pane">
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
        var originalUserData = <%=this.UserData.Json %>;
        var userData = <%=this.UserData.Json %>;
    </script>
</asp:Content>