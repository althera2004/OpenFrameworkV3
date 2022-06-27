<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Grants.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Security.Grants" %>

<asp:Content ID="Content1" ContentPlaceHolderID="StylesHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    Asignación de permisos
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <div class="col-sm-6">
                            <label>
                                <input type="radio" checked="" value="option1" id="RBHistoryNo" name="RBHistory">
                                A nivel de usuario
                            </label>
                        </div>
                        <div class="col-sm-6">
                            <label>
                                <input type="radio" checked="" value="option1" id="RBHistoryYes" name="RBHistory">
                                A nivel de grupos
                            </label>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <div class="col-sm-4">
                            <label>
                                Permiso de escritura implica eliminación
                            </label>
                        </div>
                        <div class="col-sm-4">
                            <label class="radio RBMFA">
                                <input type="radio" value="option1" id="RBMFA2" name="RBMFA" />
                                SÍ
                            </label>
                        </div>
                        <div class="col-sm-4">
                            <label class="radio RBMFA">
                                <input type="radio" value="option1" id="RBMFA3" name="RBMFA" />
                                NO
                            </label>
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