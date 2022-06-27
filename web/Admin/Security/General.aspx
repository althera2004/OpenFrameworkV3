<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="General.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Security.General" %>

<asp:Content ID="Content1" ContentPlaceHolderID="StylesHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <div class="row">
        <div class="col-lg-12">

            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    Política de contraseñas
                </div>
                <div class="panel-body">
                    <div class="form-group" style="clear: both">
                        <label class="col-xs-2">Complejidad</label>
                        <div class="col-sm-2">
                            <label>
                                <input type="radio" checked="" value="option1" id="RBComplexity1" name="RBHistory">
                                Básica
                            </label>
                        </div>
                        <div class="col-sm-2">
                            <label>
                                <input type="radio" checked="" value="option1" id="RBComplexity2" name="RBHistory">
                                Robusta
                            </label>
                        </div>
                        <div class="col-sm-6">
                            <label>
                                <input type="radio" checked="" value="option1" id="RBComplexity3" name="RBHistory">
                                Muy robusta
                            </label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-10">
                        <span class="help-block m-b-none">
                            <strong>Básica:</strong> Letras y cifras, tamaño mínimo de 6<br />
                            <strong>Robusta:</strong> Letras mayúsuclas, letras minúsculas y cifras, tamaño mínimo de 8<br />
                            <strong>Básica:</strong>Letras mayúsuclas, letras minúsculas, cifras y caracteres especiales, tamaño mínimo de 12<br /><br />
                        </span>
                            </div>
                    </div>
                    <div class="form-group" style="clear: both">
                        <label class="col-xs-2">Caducidad</label>
                        <div class="col-sm-1">
                            <label>
                                <input type="radio" checked="" value="option1" id="RBCaducidad1" name="RBHistory">
                                NO
                            </label>
                        </div>
                        <div class="col-sm-1">
                            <label>
                                <input type="radio" checked="" value="option1" id="RBCaducidad2" name="RBHistory">
                                SÍ
                            </label>
                        </div>
                        <label class="col-sm-2 control-label">Días de duración</label>
                        <div class="col-sm-2">
                            <input type="text" class="form-control col-sm-2" id="weke" value="" />
                        </div>
                        <div class="col-sm-4">
                            <label>
                                <input type="checkbox" checked="" value="option1" id="RBCaducidad2" name="RBHistory">
                                Permite repetir contraseña
                            </label>
                        </div>
                    </div>
                </div>
            </div>

            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    Control de accesos
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Nº intentos fallidos antes de bloquear usuario</label>
                        <div class="col-sm-2">
                            <input type="text" class="form-control" id="MainSenderName" value="" />
                        </div>
                        <label class="col-sm-2 control-label">Guardar accesos fallidos</label>
                        <div class="col-sm-1">
                            <input type="text" class="form-control" id="MainSenderName" value="" />
                        </div>
                        <label class="col-sm-3 control-label">días</label>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-4 control-label">Notificar por mail los intentos de acceso fallidos</label>
                        <div class="col-sm-1">
                            <label>
                                <input type="radio" checked="" value="option1" id="RBCaducidad1" name="RBHistory">
                                NO
                            </label>
                        </div>
                        <div class="col-sm-1">
                            <label>
                                <input type="radio" checked="" value="option1" id="RBCaducidad2" name="RBHistory">
                                SÍ
                            </label>
                        </div>
                        <label class="col-sm-2 control-label">Dirección</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" id="MainSenderName" value="" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsPlaceHolder" runat="server">
    var pageType = "Admin";

    $("#checkbox1").on("click", checkbox1Clicked);

    function checkbox1Clicked() {
        console.log($("#checkbox1").prop("checked"));
        if($("#checkbox1").prop("checked") === true){
            $(".RBMFA").visible();
        }
        else {
            $(".RBMFA").invisible();
        }
    }

</asp:Content>