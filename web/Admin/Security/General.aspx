<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="General.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Security.General" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
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
                                <input type="radio" checked="" value="option1" id="RBComplexity1" name="RBPasswordComplexity">
                                Básica
                            </label>
                        </div>
                        <div class="col-sm-2">
                            <label>
                                <input type="radio" checked="" value="option1" id="RBComplexity2" name="RBPasswordComplexity">
                                Robusta
                            </label>
                        </div>
                        <div class="col-sm-6">
                            <label>
                                <input type="radio" checked="" value="option1" id="RBComplexity3" name="RBPasswordComplexity">
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
                                <input type="radio" checked="" value="option1" id="RBCaducidad1" name="RBPasswordCaducity" />
                                NO
                            </label>
                        </div>
                        <div class="col-sm-1">
                            <label>
                                <input type="radio" checked="" value="option1" id="RBCaducidad2" name="RBPasswordCaducity" />
                                SÍ
                            </label>
                        </div>
                        <label class="col-sm-2 control-label PaswordCaducity" style="visibility:hidden;">Días de duración</label>
                        <div class="col-sm-2 PaswordCaducity" style="visibility:hidden;">
                            <input type="text" class="form-control col-sm-2" id="PasswordCaducityDays" value="" />
                        </div>
                        <div class="col-sm-4 PaswordCaducity" style="visibility:hidden;">
                            <label>
                                <input type="checkbox" checked="" value="option1" id="PasswordRepeat" />
                                Permite repetir contraseña
                            </label>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="form-group row">
                        <label class="col-sm-4 control-label">Nº intentos fallidos antes de bloquear usuario</label>
                        <div class="col-sm-2">
                            <input type="text" class="form-control" id="FailedAttempts" value="" />
                        </div>
                        <label class="col-sm-2 control-label">Guardar accesos fallidos</label>
                        <div class="col-sm-1">
                            <input type="text" class="form-control" id="MainSenderName" value="" />
                        </div>
                        <label class="col-sm-2 control-label">días</label>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-4 control-label">Notificar por mail los intentos de acceso fallidos</label>
                        <div class="col-sm-1">
                            <label>
                                <input type="radio" checked="" value="option1" id="RBNotificar1" name="RBHistory" />
                                No
                            </label>
                        </div>
                        <div class="col-sm-1">
                            <label>
                                <input type="radio" checked="" value="option1" id="RBNotificar2" name="RBHistory" />
                                Sí
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
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScripts" runat="server">
    <script type="text/javascript">
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
    </script>
</asp:Content>