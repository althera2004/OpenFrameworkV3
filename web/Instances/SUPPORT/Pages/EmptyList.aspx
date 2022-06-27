<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="EmptyList.aspx.cs" Inherits="Instances_SUPPORT_Pages_EmptyList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentScriptVars" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
   <div class="row">
        <div id="Incidencia_Custom_List">
            <div class="hpanel hblue hpanel-table" style="margin:0;">
                <div class="panel-heading hbuilt">
                    <span id="Incidencia_Incidencias_ListTitle">Incidències</span>
                    <div class="panel-tools">
                        <a id="Incidencia_Incidencias_AddBtn"><i class="fa fa-plus"></i>&nbsp;<span id="Incidencia_Incidencias_AddBtnLabel">Obrir nova incidència</span></a>
                    </div>
                </div>
                <div class="tableHead">
                    <table cellpadding="1" cellspacing="1" class="table">
                        <thead id="Incidencia_Incidencias_ListHead">
                            <tr>
                                <th id="th0" style="width: 150px;" class="sort ASC">Codi</th>
                                <th id="th1" style="width: 150px;" class="sort">Instància</th>
                                <th id="th2" style="width: 150px;" class="sort">Client</th>
                                <th id="th3" class="">Nom</th>
                                <th id="th4" style="width: 100px;" class="">Fecha</th>
                                <th id="th5" style="width: 200px;" class="">Tipus</th>
                                <th style="padding: 0; width: 54px; border-right: none;"></th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div class="panel-body panel-body-list" style="height:400px;">
                    <div class="table-responsive" style="max-height: 100%; height: 100%; overflow-y: scroll; overflow-x: hidden">
                        <div class="table-body" style="max-height: 100%; height: 100%; color:#4579d3; background-color:#e6eaf1;">
                            <table cellpadding="1" cellspacing="1" class="table" style="max-height: 100%">
                                <tbody style="max-height: 100%" id="Incidencia_Incidencias_ListBody">
                                    <tr>
                                        <td colspan="7" id="EmptyCellMessage" style="text-align:center;vertical-align:middle;">
                                           <h4><i class="fa fa-info-circle"></i> no hay ná</h4>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                </div>
                <div class="panel-footer" style="padding:2px;height:40px;margin-top:-1px;">
                    <div class="col-xs-4" style="padding:8px;">Nº registros:&nbsp;<i><span id="ListDataCount_Trastero_ByAlmacen">No hay registros</span></i></div>
                    <div class="col-xs-8" id="ListPager_Trastero_ByAlmacen" style="text-align:right;"></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentScriptFiles" Runat="Server">
	
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentScripts" Runat="Server">
    <script type="text/javascript">
        function ResizeWorkArea() {
            $(".panel-body").height($(window).height() - 237);

            $("#EmptyCellMessage").height($(".table-responsive").height() - 20);
        }

        pageType = "PageList";
        ResizeWorkArea();

        window.onresize = function () {
            ResizeWorkArea();
        }


    </script>
</asp:Content>