<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="User.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Security.User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentScriptVars" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="row">
        <div class="col-lg-12">

            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    Identificación
                </div>
                <div class="panel-body">
                    <div class="form-group row">
                        <label class="col-sm-1 control-label">Email</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control col-sm-2" id="Email" value="" />
                        </div>
                        <label class="col-sm-1 control-label">IMEI</label>
                        <div class="col-sm-3">
                            <input type="text" class="form-control col-sm-2" maxlength="20" id="IMEI" value="" />
                        </div>
                    </div>
                </div>
                <div class="panel-heading hbuilt">
                    Datos personales
                </div>
                <div class="panel-body">
                    <div class="form-group row">
                        <label class="col-sm-1 control-label">Nombre</label>
                        <div class="col-sm-3">
                            <input type="text" class="form-control col-sm-2" maxlength="50" id="FirstName" value="" />
                        </div>
                        <label class="col-sm-1 control-label">Apellido1</label>
                        <div class="col-sm-3">
                            <input type="text" class="form-control col-sm-2" maxlength="50" id="LastName" value="" />
                        </div>
                        <label class="col-sm-1 control-label">Apellido2</label>
                        <div class="col-sm-3">
                            <input type="text" class="form-control col-sm-2" maxlength="50" id="LastName2" value="" />
                        </div>
                    </div>
                </div>
                <div class="panel-heading hbuilt">
                    Tipus d'accés
                </div>
                <div class="panel-body">
                    <div class="form-group row">
                        <label class="col-sm-2">
                            <input type="checkbox" style="float:left;margin:5px 0 0 0!important;" id="Core" />
                            <span style="float:left;margin-top:3px;margin-left:4px;padding:0">CORE</span>
                        </label>
                        <label class="col-sm-2">
                            <input type="checkbox" style="float:left;margin:5px 0 0 0!important;" id="Tecnic" />
                            <span style="float:left;margin-top:3px;margin-left:4px;padding:0">Tècnic</span>
                        </label>
                        <label class="col-sm-2">
                            <input type="checkbox" style="float:left;margin:5px 0 0 0!important;" id="Principal" />
                            <span style="float:left;margin-top:3px;margin-left:4px;padding:0">Principal</span>
                        </label>
                        <label class="col-sm-2">
                            <input type="checkbox" style="float:left;margin:5px 0 0 0!important;" id="Corportatiu" />
                            <span style="float:left;margin-top:3px;margin-left:4px;padding:0">Corportatiu</span>
                        </label>
                        <label class="col-sm-2">
                            <input type="checkbox" style="float:left;margin:5px 0 0 0!important;" id="Extern" />
                            <span style="float:left;margin-top:3px;margin-left:4px;padding:0">Extern</span>
                        </label>
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