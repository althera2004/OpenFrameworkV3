<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="Admin_Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentScriptVars" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="col-lg-12">
        <div class="hpanel">
            <ul class="nav nav-tabs" id="FormTabs">
                <li id="tabSelect-basic" class="tabSelect active"><a data-toggle="tab" href="#tab-basic" aria-expanded="false">Dades personals</a></li>
                <li id="TabPreferences" class="tabSelecr"><a data-toggle="tab" href="#tab-preferences">Preferències</a></li>
            </ul>
            <div class="tab-content" id="FormContent">
                <div id="tab-basic" class="tab-pane active">
                    <div class="hpanel">
                        <div class="panel-body panel-body-form" style="height: 294px;">
                            <div id="user-profile-1" class="user-profile row">
                                            <div class="col-xs-12 col-sm-3 center">
                                                <div>
                                                    <span class="profile-picture">
                                                        <img id="avatar" class="editable img-responsive editable-click editable-empty" alt="Juan Castilla" src="/img/avatar.png" style="display: block;">
                                                    </span>

                                                    <div class="space-4"></div>

                                                    <div class="width-80 label label-info label-xlg arrowed-in arrowed-in-right">
                                                        <div class="inline position-relative">
                                                            <a href="#" class="user-title-label dropdown-toggle" data-toggle="dropdown">
                                                                <span class="white" id="SpanUserName">Juan Castilla</span>
                                                            </a>
                                                            <ul class="align-left dropdown-menu dropdown-caret dropdown-lighter">
                                                                <li class="dropdown-header">Historial</li>
                                                                <li>
                                                                    <a href="#" title="Creació">
                                                                        <i class="ace-icon fa fa-check-square-o green"></i>&nbsp;
                                                                        <span class="green">06/02/2019</span>
                                                                    </a>
                                                                </li>
                                                                <li>
                                                                    <a href="#" title="Darrera modificació">
                                                                        <i class="ace-icon fa fa-pencil-square-o grey"></i>&nbsp;
                                                                        <span class="grey">06/02/2019</span>
                                                                    </a>
                                                                </li>
                                                                <li>
                                                                    <a href="#" title="Darrera conexió">
                                                                        <i class="ace-icon fa fa-plug red"></i>&nbsp;
                                                                        <span class="red">21/06/2022</span>
                                                                    </a>
                                                                </li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </div>

                                                
                                                <div>
                                                    <hr>
                                                    <h4 style="text-align:left;">Signatura</h4>
                                                    <span class="profile-picture">
                                                        <img id="signature" class="editable img-responsive editable-click editable-empty" alt="Signatura" src="/img/NoSignature.png" style="display: block;">
                                                    </span>
                                                </div>
                                                
                                            </div>

                                            <div class="col-xs-12 col-sm-9">

                                                <div class="profile-info-title">&nbsp;Dades personals&nbsp;</div>
                                                <div class="profile-user-info profile-user-info-striped">

                                                    <div class="profile-info-row">
                                                        <div class="profile-info-name">&nbsp;Usuari&nbsp;</div>
                                                        <div class="profile-info-value">
                                                            <span class="editable editable-click" id="username">jcastilla@openframework.es</span>
                                                        </div>
                                                    </div>

                                                    <div class="profile-info-row">
                                                        <div class="profile-info-name">&nbsp;Nom&nbsp;</div>
                                                        <div class="profile-info-value">
                                                            <span class="editable editable-click" id="firstName"></span>
                                                            <span class="editable editable-click" id="lastName"></span>
                                                            <span class="editable editable-click" id="lastName2"></span>
                                                            
                                                        </div>
                                                    </div>                                                    
                                                    
                                                    <div class="profile-info-row">
                                                        <div class="profile-info-name">&nbsp;Sexe</div>
                                                        <div class="profile-info-value">
                                                            <input type="radio" name="RBGender" id="RBGender2" checked="checked" spellcheck="false"> Home
                                                            &nbsp;
                                                            <input type="radio" name="RBGender" id="RBGender1" spellcheck="false"> Dona
                                                            &nbsp;                                                            
                                                            <input type="radio" name="RBGender" id="RBGender0" spellcheck="false"> Grup
                                                        </div>
                                                    </div>
                                                    
                                                </div>
                                                
                                                <div class="profile-info-title">&nbsp;Contacte&nbsp;</div>
                                                <div class="profile-user-info profile-user-info-striped">
                                                    
                                                    <div class="profile-info-row">
                                                        <div class="profile-info-name">&nbsp;Email alternatiu&nbsp;</div>
                                                        <div class="profile-info-value">
                                                            <span class="editable editable-click editable-empty" id="TxtEmailAlternative">buit</span>
                                                        </div>
                                                    </div>
                                                    
                                                    <div class="profile-info-row">
                                                        <div class="profile-info-name">&nbsp;Móvil&nbsp;</div>
                                                        <div class="profile-info-value">
                                                            <span class="editable editable-click" id="TxtMobile">600000001</span>
                                                        </div>
                                                    </div>
                                                    
                                                    <div class="profile-info-row">
                                                        <div class="profile-info-name">&nbsp;Telèfon d´emergència&nbsp;</div>
                                                        <div class="profile-info-value">
                                                            <span class="editable editable-click editable-empty" id="TxtPhoneEmergency">buit</span>
                                                        </div>
                                                    </div>
                                                    
                                                </div>

                                                
                                                <div class="profile-info-title">&nbsp;Extra&nbsp;</div>
                                                <div class="profile-user-info profile-user-info-striped">
                                                    
                                                    <div class="profile-info-row">
                                                        <div class="profile-info-name">Nº col·legiat&nbsp;</div>
                                                        <div class="profile-info-value">
                                                            <span class="editable editable-click" id="TxtDataText1">45645645</span>
                                                        </div>
                                                    </div>
                                                    
                                                    <div class="profile-info-row">
                                                        <div class="profile-info-name">Entitat&nbsp;</div>
                                                        <div class="profile-info-value">
                                                            <span class="editable editable-click" id="TxtDataText2">OpenFramework</span>
                                                        </div>
                                                    </div>
                                                    
                                                </div>
                                                
                                            </div>
                                        </div>
                        </div>
                    </div>
                </div>
                <div id="tab-preferences" class="tab-pane">
                    pref
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentScriptFiles" Runat="Server">
    <script type="text/javascript" src="/assets/js/ace-editable.min.js"></script>
    <script type="text/javascript" src="/Admin/Profile.js"></script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentScripts" Runat="Server">
</asp:Content>