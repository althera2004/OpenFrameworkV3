<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="OpenFrameworkV2.Web.Admin.Security.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="StylesHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="hpanel hgreen">
                <div class="panel-heading hbuilt">
                    Usuarios que pueden modificar la seguridad
                </div>
                <div class="panel-body" style="padding: 6px;">
                    <label class="col-sm-12">
                        <input type="checkbox" checked="" value="option1" id="RBCaducidad1" name="RBHistory">
                        Verificar con código QR el acceso a la configuración de seguridad.
                    </label>
                </div>
                <div class="panel-body">
                    <div class="form-group" style="clear: both">
                        <label class="col-xs-3">Datos de la compañía</label>
                        <div class="col-sm-9">
                            <input type="text" class="form-control col-sm-2" id="weke" value="" />
                        </div>
                    </div>
                    <div class="form-group" style="clear: both">
                        <label class="col-xs-3">Datos de facturación</label>
                        <div class="col-sm-9">
                            <input type="text" class="form-control col-sm-2" id="weke" value="" />
                        </div>
                    </div>
                    <div class="form-group" style="clear: both">
                        <label class="col-xs-3">Configuración de seguridad</label>
                        <div class="col-sm-9">
                            <input type="text" class="form-control col-sm-2" id="weke" value="" />
                        </div>
                    </div>
                    <div class="form-group" style="clear: both">
                        <label class="col-xs-3">Gestión de usuarios y grupos</label>
                        <div class="col-sm-9">
                            <input type="text" class="form-control col-sm-2" id="weke" value="" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="hpanel hgreen">
                <div class="panel-heading hbuilt">
                    Configuración general
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" style="height: 100px; font-size: 16px;" class=" col-xs-12 btn btn-app btn-success" onclick="GoEncryptedPage('/Admin/Security/General.aspx');">
                            <i class="fa fa-building"></i>&nbsp;&nbsp;Datos básicos&nbsp;&nbsp;</button>
                    </p>
                </div>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="hpanel hgreen">
                <div class="panel-heading hbuilt">
                    Política de acceso
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" style="height: 100px; font-size: 16px;" class=" col-xs-12 btn btn-app btn-success" onclick="GoEncryptedPage('/Admin/Security/AccessPolicy.aspx');">
                            <i class="fa fa-lock"></i>&nbsp;&nbsp;Política de acceso&nbsp;&nbsp;</button>
                    </p>
                </div>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="hpanel hgreen">
                <div class="panel-heading hbuilt">
                    Permisos
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" style="height: 100px; font-size: 16px;" class=" col-xs-12 btn btn-app btn-success" onclick="GoEncryptedPage('/Admin/Security/Grants.aspx');">
                            <i class="fa fa-lock"></i>&nbsp;&nbsp;Permisos de usuarios&nbsp;&nbsp;</button>
                    </p>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-4">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    Usuarios
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" style="height:100px;font-size:16px;" class=" col-xs-12 btn btn-app btn-info"  onclick="GoEncryptedPage('/Admin/Security/UserList.aspx');">
                            <i class="fa fa-user"></i>&nbsp;&nbsp;Usuarios&nbsp;&nbsp;</button>
                    </p>
                </div>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    Grupos
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" style="height:100px;font-size:16px;" class=" col-xs-12 btn btn-app btn-info" onclick="GoEncryptedPage('/Admin/Security/GroupList.aspx');">
                            <i class="fa fa-users"></i>&nbsp;&nbsp;Grupos&nbsp;&nbsp;</button>
                    </p>
                </div>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    Usuarios externos
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" style="height:100px;font-size:16px;" class=" col-xs-12 btn btn-app btn-info" onclick="GoEncryptedPage('/Instances/Storage/Pages/DashboardOcupacion.aspx?YWM9MSZvcHRpb25JZD0w');">
                            <i class="fa fa-user-circle"></i>&nbsp;&nbsp;Usuarios externos&nbsp;&nbsp;</button>
                    </p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder" runat="server">
    pageType = "admin";
</asp:Content>