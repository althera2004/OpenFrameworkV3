<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="MailBoxes.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Company.MailBoxes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    
    <div class="row">
        <div class="col-lg-6">
            <div class="hpanel hgreen">
                <div class="panel-heading hbuilt">
                    Usuarios y contraseñas
                </div>
                <div class="panel-body">
                    <p>
                        Esta dirección de correo se emplea para el envio de credenciales de acceso cuando se da de alta un usuario en la aplicación, para el renicio de contraseñas y para la doble autenticación de usuarios
                    </p>
                    <form method="get" class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Email</label>
                            <div class="col-sm-10">
                                <input type="text" class="form-control" id="MainMailMadress" value="<%=this.Main.MailAddress %>">
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label">Nombre</label>

                            <div class="col-sm-10">
                                <input type="text" class="form-control" id="MainSenderName" value="<%=this.Main.SenderName %>" />
                                <span class="help-block m-b-none">Nombre que aparecerá como remitente del correo.</span>
                            </div>
                        </div>
                        <div class="panel-body">
                            Estos datos son proporcionados por su proveedor de servicio de correo electrónico
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Usuario</label>

                                <div class="col-sm-10">
                                    <input type="text" class="form-control" id="MainMailUser" value="<%=this.Main.MailUser %>" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Password</label>

                                <div class="col-sm-10">
                                    <input type="password" class="form-control" id="MainMailPassword" value="<%=this.Main.MailPassword%>" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-2 control-label">Servidor</label>

                                <div class="col-sm-10">
                                    <input type="text" class="form-control" id="MainServer" value="<%=this.Main.Server%>" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-2 control-label">Tipo</label>

                                <div class="col-sm-10">
                                    <input type="text" class="form-control" id="MainMailBoxType" value="<%=this.Main.MailBoxType%>" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-2 control-label">Puerto de envío</label>

                                <div class="col-lg-10">
                                    <input type="number" class="form-control" id="MainSendPort" value="<%=this.Main.SendPort %>" />
                                    <span class="help-block m-b-none">Los puertos de envío son 25 (para envíos smtp) y 465 (para envíos SSL).</span>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-2 control-label">SSL/TLS</label>
                                <div class="col-lg-10" style="text-align:left;padding-top:7px;">
                                    <input type="checkbox" id="MainSSL" <%=this.Main.SSL ? " checked=\"checked\"" : string.Empty %>" />
                                </div>
                            </div>
                        </div>


                    </form>
                </div>

                <div class="panel-footer">
                    <label>Acciones:</label>&nbsp;
                    <button type="button" id="MainBtnSave" class="btn btn-xs btn-info" onclick="MAILBOXES_SaveMain();">Guardar</button>
                    <button type="button" id="MainBtnSendTest" class="btn btn-xs btn-info">Envío de comprobación</button>
                    <button type="button" id="MainBtnCheck" class="btn btn-danger btn-xs" onclick="MAILBOXES_CheckBlackListMain();">Comprobar Black List</button>
                </div>
            </div>
        </div>
        <div class="col-lg-6">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    Envíos a terceros
                </div>
                <div class="panel-body">
                    <p>
                        Esta dirección de correo se emplea para el envio a terceros como noticaciones, adjuntos de documentación, facturas, etc...
                    </p>
                    <form method="get" class="form-horizontal">
                        <div class="form-group">
                            <label class="col-xs-12">
                                <input type="checkbox" style="margin:5px 0 0 0!important;" id="ChkSameAddress" <%=this.ThirdParty.Id < 1 ? " checked=\"checked\"" : string.Empty  %> />
                                Usar la misma dirección de correo que para envíos a usuarios.</label>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Email</label>

                            <div class="col-sm-10">
                                <input type="text" id="Third_MailAddress" class="form-control" value="<%=this.ThirdParty.MailAddress %>" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-2 control-label">Nombre</label>

                            <div class="col-sm-10">
                                <input type="text" id="Third_SenderName" class="form-control" value="<%=this.ThirdParty.SenderName %>" />
                                <span class="help-block m-b-none">Nombre que aparecerá como remitente del correo.</span>
                            </div>
                        </div>
                        <div class="panel-body">
                            Estos datos son proporcionados por su proveedor de servicio de correo electrónico
                <div class="form-group">
                    <label class="col-sm-2 control-label">Usuario</label>

                    <div class="col-sm-10">
                        <input type="password" id="Third_MailUser" class="form-control" name="password" />
                    </div>
                </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Password</label>

                                <div class="col-sm-10">
                                    <input type="password" id="Third_MailPassword" class="form-control" name="password" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-2 control-label">Servidor</label>

                                <div class="col-sm-10">
                                    <input type="text" id="Third_Server" class="form-control" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Tipo</label>

                                <div class="col-sm-10">
                                    <input type="text" class="form-control" id="ThirdMailBoxType" value="<%=this.ThirdParty.MailBoxType%>" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-2 control-label">Puerto de envío</label>

                                <div class="col-lg-10">
                                    <input type="number" id="Third_SendPort" class="form-control" />
                                    <span class="help-block m-b-none">Los puertos de envío son 25 (para envíos smtp) y 465 (para envíos SSL).</span>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-2 control-label">SSL/TLS</label>
                                <div class="col-lg-10" style="text-align:left;padding-top:7px;">
                                    <input type="checkbox" id="Third_SSL" <%=this.ThirdParty.SSL ? " checked=\"checked\"" : string.Empty %>" />
                                </div>
                            </div>
                        </div>


                    </form>
                </div>

                <div class="panel-footer">
                    <label>Acciones:</label>&nbsp;
                    <button type="button" id="ThirdBtnSave" class="btn btn-xs btn-info">Guardar</button>
                    <button type="button" id="ThirdBtnSendTest" class="btn btn-xs btn-info">Envío de comprobación</button>
                    <button type="button" id="ThirdBtnCheck" class="btn btn-danger btn-xs">Comprobar Black List</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScripts" runat="server">
    <script type="text/javascript">
        pageType = "admin";
        var mainAddress = <%=this.Main.Json %>;
        var thirdAddress = <%=this.ThirdParty.Json %>;
    </script>
</asp:Content>