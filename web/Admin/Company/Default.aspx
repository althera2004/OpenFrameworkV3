<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Company.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
    <style type="text/css">
    .row { margin-bottom:0px!important; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="row">
        <div class="col-lg-6 col-xs-4">
            <div class="hpanel hgreen">
                <div class="panel-heading hbuilt">
                    Dades bàsiques de la companyia
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" class="AdminCompanyButton col-xs-12 btn btn-app btn-success" onclick="document.location='/Instances/Storage/Pages/DashboardOcupacion.aspx?YWM9MSZvcHRpb25JZD0w';">
                            <i class="fa fa-building"></i>&nbsp;&nbsp;Datos básicos&nbsp;&nbsp;</button>
                    </p>
                </div>
                <div class="panel-footer">
                    This is standard panel footer
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-4">
            <div class="hpanel hred">
                <div class="panel-heading hbuilt">
                    Gestió documental
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" class="AdminCompanyButton col-xs-12 btn btn-app btn-danger" onclick="GoEncryptedPage('/Admin/Company/Features/Documents.aspx');">
                            <i class="fa fa-book"></i>&nbsp;&nbsp;Gestió documental&nbsp;&nbsp;</button>
                    </p>
                </div>
                <div class="panel-footer">
                    This is standard panel footer
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-4">
            <div class="hpanel hgreen">
                <div class="panel-heading hbuilt">
                    Contrato
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" class="AdminCompanyButton col-xs-12 btn btn-app btn-success" onclick="GoEncryptedPage('/Admin/Company/Services.aspx');">
                            <i class="fa fa-file-contract"></i>&nbsp;&nbsp;Serveis&nbsp;&nbsp;</button>
                    </p>
                </div>
                <div class="panel-footer">
                    Servicios contratados
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-4">
            <div class="hpanel hblue disabled">
                <div class="panel-heading hbuilt">
                    Gestión de contactos
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" class="AdminCompanyButton col-xs-12 btn btn-app btn-info" onclick="GoEncryptedPage('/Admin/Company/Contact.aspx');">
                            <i class="fa fa-users"></i>&nbsp;&nbsp;Contactes&nbsp;&nbsp;</button>
                    </p>
                </div>
                <div class="panel-footer">
                    This is standard panel footer
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-4">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    Gestión de firmas
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" class="AdminCompanyButton col-xs-12 btn btn-app btn-info" onclick="GoEncryptedPage('/Admin/Company/Features/Signatures.aspx');">
                            <i class="fa fa-file-signature"></i>&nbsp;&nbsp;Signatures&nbsp;&nbsp;</button>
                    </p>
                </div>
                <div class="panel-footer">
                    Controlar quins usuaris signen per defecte els documents
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-4">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    Gestión de direcciones
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" class="AdminCompanyButton col-xs-12 btn btn-app btn-info" onclick="GoEncryptedPage('/Admin/Company/CompanyAddress.aspx');">
                            <i class="fa fa-map-marker-alt"></i>&nbsp;&nbsp;Adreces&nbsp;&nbsp;</button>
                    </p>
                </div>
                <div class="panel-footer">
                    Gestión de direcciones postales y de facturación
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-4">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    Gestión de cuentas bancarias
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" class="AdminCompanyButton col-xs-12 btn btn-app btn-info" onclick="GoEncryptedPage('/Admin/Company/BankAccount.aspx');">
                            <i class="fa fa-credit-card"></i>&nbsp;&nbsp;Cuentas bancarias&nbsp;&nbsp;</button>
                    </p>
                </div>
                <div class="panel-footer">
                    Cuentas IBAN para la facturación de servicios ofrecidos
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-4">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    Correos electrónicos
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" class="AdminCompanyButton col-xs-12 btn btn-app btn-info" onclick="GoEncryptedPage('/Admin/Company/MailBoxes.aspx');">
                            <i class="fa fa-at"></i>&nbsp;&nbsp;Correos electrónicos&nbsp;&nbsp;</button>
                    </p>
                </div>
                <div class="panel-footer">
                    This is standard panel footer
                </div>
            </div>
        </div>

    </div>
    <div class="row">
        <div class="col-lg-3 col-xs-4">
            <div class="hpanel hyellow">
                <div class="panel-heading hbuilt">
                    Gestión de alertas
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" class="AdminCompanyButton col-xs-12 btn btn-app btn-warning" onclick="GoEncryptedPage('/Instances/Storage/Pages/DashboardOcupacion.aspx?YWM9MSZvcHRpb25JZD0w');">
                            <i class="fa fa-bell"></i>&nbsp;&nbsp;Alertas&nbsp;&nbsp;</button>
                    </p>
                </div>
                <div class="panel-footer">
                    This is standard panel footer
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-4">
            <div class="hpanel hyellow">
                <div class="panel-heading hbuilt">
                    Facturación
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" class="AdminCompanyButton col-xs-12 btn btn-app btn-warning" onclick="GoEncryptedPage('/Instances/Storage/Pages/DashboardOcupacion.aspx');">
                            <i class="fa fa-euro-sign"></i>&nbsp;&nbsp;Facturación&nbsp;&nbsp;</button>
                    </p>
                </div>
                <div class="panel-footer">
                    Controlar qué usuarios firma por defecto los documentos
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-4">
            <div class="hpanel hyellow">
                <div class="panel-heading hbuilt">
                    Configuración personalizada
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" class="AdminCompanyButton col-xs-12 btn btn-app btn-warning" onclick="document.location='/Instances/Storage/Pages/DashboardOcupacion.aspx?YWM9MSZvcHRpb25JZD0w';">
                            <i class="fa fa-cog"></i>&nbsp;&nbsp;Direcciones&nbsp;&nbsp;</button>
                    </p>
                </div>
                <div class="panel-footer">
                    Gestión de direcciones postales y de facturación
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-4">
            <div class="hpanel hyellow">
                <div class="panel-heading hbuilt">
                    Estética y presentación
                </div>
                <div class="panel-body">
                    <p>
                        <button type="button" class="AdminCompanyButton col-xs-12 btn btn-app btn-warning" onclick="document.location='/Instances/Storage/Pages/DashboardOcupacion.aspx?YWM9MSZvcHRpb25JZD0w';">
                            <i class="fa fa-images"></i>&nbsp;&nbsp;Personalización&nbsp;&nbsp;</button>
                    </p>
                </div>
                <div class="panel-footer">
                    Cuentas IBAN para la facturación de servicios ofrecidos
                </div>
            </div>
        </div>
    </div>   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScripts" runat="server">
    <script type="text/javascript">
        MenuSelectOption("1000");
    </script>
</asp:Content>