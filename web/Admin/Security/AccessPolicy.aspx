<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="AccessPolicy.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Security.AccessPolicy" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    Política de acceso a la aplicación
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <div class="col-sm-12">
                            <label>
                                <input type="radio" checked="" value="option1" id="RBHistoryNo" name="RBHistory">
                                Desde cualquier lugar
                            </label>
                        </div>
                        <div class="col-sm-2">
                            <label>
                                <input type="radio" checked="" value="option1" id="RBHistoryYes" name="RBHistory">
                                Sólo desde estas Ips
                            </label>
                        </div>
                        <div class="col-sm-10">
                            <textarea class="form-control" id="IPS" rows="3" style="width:99%;"></textarea>
                            <i>Las IP's han de ir separadas por comas.</i>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <div class="col-sm-4">
                            <label>
                                Factor de doble autenticación
                            </label>
                        </div>
                        <div class="col-sm-2">
                            <label class="radio">
                                <input type="checkbox" id="checkbox1" data-size="sm" data-toggle="toggle" data-on="Activo" data-off="Inactivo" data-onstyle="success" />
                            </label>
                        </div>
                        <div class="col-sm-2">
                            <label class="radio RBMFA">
                                <input type="radio" value="option1" id="RBMFA2" name="RBMFA" />
                                Por mail
                            </label>
                        </div>
                        <div class="col-sm-2">
                            <label class="radio RBMFA">
                                <input type="radio" value="option1" id="RBMFA3" name="RBMFA" />
                                QR-Access
                            </label>
                        </div>
                        <div class="col-sm-2">
                            <label class="radio RBMFA" title="Subscripción Premium">
                                <input type="radio" value="option1" id="RBMFA4" name="RBMFA" disabled="disabled" />
                                Por SMS <i class="fa fa-medal" style="color:#1f8f0b;"></i>
                            </label>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <div class="col-sm-4">
                            <label>
                                Usuarios coportativos
                            </label>
                        </div>
                        <div class="col-sm-2">
                            <label class="radio">
                                <input type="checkbox" id="ChkCoporative" data-size="sm" data-toggle="toggle" data-on="Activo" data-off="Inactivo" data-onstyle="success" />
                            </label>
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

        $(function() {
            $('#checkbox1').change(checkbox1Clicked);
        })
    
        $('#checkbox1').bootstrapToggle(!AccessPolicy.MFAEmail ? "on" : "off", true);
        $('#ChkCoporative').bootstrapToggle(!AccessPolicy.CorportativeEnabled ? "on" : "off", true);
    </script>
</asp:Content>