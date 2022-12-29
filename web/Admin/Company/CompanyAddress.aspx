<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="CompanyAddress.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Company.CompanyAddress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="row">
        <div class="col-lg-12" id="CompanyAddressList">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    Acreces
                    <div class="panel-tools">
                        <a class="_showhide" id="COMPANYADDRESS_AddBtn" onclick="COMPANYADDRESS_Add();"><i class="fa fa-plus"></i>&nbsp;Añadir cuenta bancaria</a>
                    </div>
                </div>
                        <div class="tableHead">
                            <table cellpadding="1" cellspacing="1" class="table">
                                <thead>
                                    <tr>
                                        <th>Adreça</th>
                                        <th style="width:100px;">Principal</th>
                                        <th style="width:100px;">Facturació</th>
                                        <th class="action-buttons-header" data-buttons="2" style="width:77px;white-space:nowrap;"></th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                <div class="panel-body">
                    <div class="table-responsive" style="max-height:100%;height:100%;overflow-y:scroll;overflow-x:hidden">
                        <div class="table-body" style="max-height:100%;height:100%">
                            <table cellpadding="1" cellspacing="1" class="table" style="max-height:100%">
                                <tbody style="max-height:100%" id="COMPANYADDRESS_TBody"></tbody>
                            </table>
                        </div>
                    </div>

                </div>
                <div class="panel-footer">
                    Nº de registros: <strong id="BANKACCOUNT_TotalRows"></strong>
                </div>
            </div>
        </div>
        <div class="col-lg-12" id="CompanyAddressForm" style="display:none;">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    Editar cuenta bancaria
                </div>
                <div class="panel-body">
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <label id="BankNameLabel" class="formFieldLabel col-sm-1">Entitat<span style="color: #ff0000;">*</span></label>
                            <div class="col-sm-3">
                                <input type="text" id="BankName" class="form-control" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false" data-required="true">
                            </div>
                            <label id="IBANLabel" class="formFieldLabel col-sm-1">IBAN<span style="color: #ff0000;">*</span></label>
                            <div class="col-sm-4">
                                <div class="col-xs-12 col-sm-12 tooltip-info" id="DivIBAN" style="padding: 0; margin: 0;">
                                    <input class="form-control formData" id="IBAN" type="text" data-validation="iban" spellcheck="false">
                                </div>
                            </div>
                            <label id="SWIFTLabel" class="formFieldLabel col-sm-1">SWIFT<span style="color: #ff0000;">*</span></label>
                            <div class="col-sm-2">
                                <input type="text" id="SWIFT" class="form-control" value="" maxlength="40" onblur="this.value = $.trim(this.value);" spellcheck="false">
                            </div>
                        </div>
                        <div class="form-group">

                            <label id="ContractIdLabel" class="formFieldLabel col-sm-1">Contracte</label>
                            <div class="col-sm-3">
                                <input type="text" id="ContractId" class="form-control" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false">
                            </div>
                            <label id="PaymentTypeLabel" class="formFieldLabel col-sm-8">
                                Tipus de cobrament&nbsp;&nbsp;
                                <input type="text" id="PaymentType" style="display: none;" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false">
                                <input type="radio" name="RBPayment" id="RBPaymentPRE" spellcheck="false">&nbsp;Fitxer de descripció de la càrrega
                                &nbsp;&nbsp;
                                <input type="radio" name="RBPayment" id="RBPaymentFSDD" spellcheck="false">&nbsp;FSDD
                            </label>
                        </div>
                        <div class="form-group">

                            <label id="AliasLabel" class="formFieldLabel col-sm-1">Alias</label>
                            <div class="col-sm-3">
                                <input type="text" id="Alias" class="form-control" value="" maxlength="50" onblur="this.value = $.trim(this.value);" spellcheck="false">
                            </div>
                            <div class="col-sm-4">
                                <div class="col-sm-1">
                                    <input type="checkbox" id="CBMain" style="font-size: 12px;" spellcheck="false">
                                </div>
                                <label id="MainLabel" class="formFieldLabel col-sm-10" style="margin-top: 3px;">Compte principal</label>
                            </div>
                            <div class="col-sm-4">&nbsp;</div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <label>Acciones:</label>&nbsp;
                                     <button class="btn btn-xs btn-info">Guardar</button>
                    <button class="btn btn-xs" onclick="BANKACCOUNT_AddCancel();">Cancelar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentScripts" runat="server">
    <script type="text/javascript">
        pageType = "admin";
    
    window.onresize = function () {
        RedrawTable("BankAccount");
    }

    window.onload = function () {
        RedrawTable("BankAccount");
        COMPANYADDRESS_RenderTable();
    }
    
    
    function RedrawTable (tableName) {
        $(".content").height( $(window).height() - $("#header").height() -67);
        $("#" + tableName + "List .panel-body").css("padding", "0");
        $("#" + tableName + "List .panel-body").height($(".content").height() - $("#" + tableName + "List .panel-heading").outerHeight() - $("#" + tableName + "List .panel-footer").outerHeight() - 63)
    }

        var CompanyAddreses = <%=this.CompanyAddresses %>;
    </script>
</asp:Content>