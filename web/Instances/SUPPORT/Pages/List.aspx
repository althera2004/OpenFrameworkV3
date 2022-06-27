<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="List.aspx.cs" Inherits="Instances_SUPPORT_Pages_List" %>

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
                        <div class="table-body" style="max-height: 100%; height: 100%">
                            <table cellpadding="1" cellspacing="1" class="table" style="max-height: 100%">
                                <tbody style="max-height: 100%" id="Incidencia_Incidencias_ListBody">
                                    <tr id="1">
                                        <td style="width: 150px;" data-order="0">
                                            <div class="truncate" style="width: 130px;">OF-0001        </div>
                                        </td>
                                        <td style="width: 150px;" data-order="1">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td style="width: 150px;" data-order="2">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td data-order="3">
                                            <div class="truncate">test</div>
                                        </td>
                                        <td style="width: 100px; text-align: center;" data-order="4">
                                            <div class="truncate" style="width: 80px;">12/04/2022</div>
                                        </td>
                                        <td style="width: 200px; text-align: left;" data-order="5">
                                            <div class="truncate" style="width: 180px;" title="Incidència">Incidència</div>
                                        </td>
                                        <td class="action-buttons" data-buttons="0" style="width: 17px; white-space: nowrap;"><a class="blue" id="1" onclick="GoItemView('101', this.id)"><i class="fa fa-fal fa-pencil-alt"></i></a></td>
                                    </tr>
                                    <tr id="1">
                                        <td style="width: 150px;" data-order="0">
                                            <div class="truncate" style="width: 130px;">OF-0001        </div>
                                        </td>
                                        <td style="width: 150px;" data-order="1">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td style="width: 150px;" data-order="2">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td data-order="3">
                                            <div class="truncate">test</div>
                                        </td>
                                        <td style="width: 100px; text-align: center;" data-order="4">
                                            <div class="truncate" style="width: 80px;">12/04/2022</div>
                                        </td>
                                        <td style="width: 200px; text-align: left;" data-order="5">
                                            <div class="truncate" style="width: 180px;" title="Incidència">Incidència</div>
                                        </td>
                                        <td class="action-buttons" data-buttons="0" style="width: 17px; white-space: nowrap;"><a class="blue" id="1" onclick="GoItemView('101', this.id)"><i class="fa fa-fal fa-pencil-alt"></i></a></td>
                                    </tr>
                                    <tr id="1">
                                        <td style="width: 150px;" data-order="0">
                                            <div class="truncate" style="width: 130px;">OF-0001        </div>
                                        </td>
                                        <td style="width: 150px;" data-order="1">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td style="width: 150px;" data-order="2">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td data-order="3">
                                            <div class="truncate">test</div>
                                        </td>
                                        <td style="width: 100px; text-align: center;" data-order="4">
                                            <div class="truncate" style="width: 80px;">12/04/2022</div>
                                        </td>
                                        <td style="width: 200px; text-align: left;" data-order="5">
                                            <div class="truncate" style="width: 180px;" title="Incidència">Incidència</div>
                                        </td>
                                        <td class="action-buttons" data-buttons="0" style="width: 17px; white-space: nowrap;"><a class="blue" id="1" onclick="GoItemView('101', this.id)"><i class="fa fa-fal fa-pencil-alt"></i></a></td>
                                    </tr>
                                    <tr id="1">
                                        <td style="width: 150px;" data-order="0">
                                            <div class="truncate" style="width: 130px;">OF-0001        </div>
                                        </td>
                                        <td style="width: 150px;" data-order="1">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td style="width: 150px;" data-order="2">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td data-order="3">
                                            <div class="truncate">test</div>
                                        </td>
                                        <td style="width: 100px; text-align: center;" data-order="4">
                                            <div class="truncate" style="width: 80px;">12/04/2022</div>
                                        </td>
                                        <td style="width: 200px; text-align: left;" data-order="5">
                                            <div class="truncate" style="width: 180px;" title="Incidència">Incidència</div>
                                        </td>
                                        <td class="action-buttons" data-buttons="0" style="width: 17px; white-space: nowrap;"><a class="blue" id="1" onclick="GoItemView('101', this.id)"><i class="fa fa-fal fa-pencil-alt"></i></a></td>
                                    </tr>
                                    <tr id="1">
                                        <td style="width: 150px;" data-order="0">
                                            <div class="truncate" style="width: 130px;">OF-0001        </div>
                                        </td>
                                        <td style="width: 150px;" data-order="1">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td style="width: 150px;" data-order="2">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td data-order="3">
                                            <div class="truncate">test</div>
                                        </td>
                                        <td style="width: 100px; text-align: center;" data-order="4">
                                            <div class="truncate" style="width: 80px;">12/04/2022</div>
                                        </td>
                                        <td style="width: 200px; text-align: left;" data-order="5">
                                            <div class="truncate" style="width: 180px;" title="Incidència">Incidència</div>
                                        </td>
                                        <td class="action-buttons" data-buttons="0" style="width: 17px; white-space: nowrap;"><a class="blue" id="1" onclick="GoItemView('101', this.id)"><i class="fa fa-fal fa-pencil-alt"></i></a></td>
                                    </tr>
                                    <tr id="1">
                                        <td style="width: 150px;" data-order="0">
                                            <div class="truncate" style="width: 130px;">OF-0001        </div>
                                        </td>
                                        <td style="width: 150px;" data-order="1">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td style="width: 150px;" data-order="2">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td data-order="3">
                                            <div class="truncate">test</div>
                                        </td>
                                        <td style="width: 100px; text-align: center;" data-order="4">
                                            <div class="truncate" style="width: 80px;">12/04/2022</div>
                                        </td>
                                        <td style="width: 200px; text-align: left;" data-order="5">
                                            <div class="truncate" style="width: 180px;" title="Incidència">Incidència</div>
                                        </td>
                                        <td class="action-buttons" data-buttons="0" style="width: 17px; white-space: nowrap;"><a class="blue" id="1" onclick="GoItemView('101', this.id)"><i class="fa fa-fal fa-pencil-alt"></i></a></td>
                                    </tr>
                                    <tr id="1">
                                        <td style="width: 150px;" data-order="0">
                                            <div class="truncate" style="width: 130px;">OF-0001        </div>
                                        </td>
                                        <td style="width: 150px;" data-order="1">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td style="width: 150px;" data-order="2">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td data-order="3">
                                            <div class="truncate">test</div>
                                        </td>
                                        <td style="width: 100px; text-align: center;" data-order="4">
                                            <div class="truncate" style="width: 80px;">12/04/2022</div>
                                        </td>
                                        <td style="width: 200px; text-align: left;" data-order="5">
                                            <div class="truncate" style="width: 180px;" title="Incidència">Incidència</div>
                                        </td>
                                        <td class="action-buttons" data-buttons="0" style="width: 17px; white-space: nowrap;"><a class="blue" id="1" onclick="GoItemView('101', this.id)"><i class="fa fa-fal fa-pencil-alt"></i></a></td>
                                    </tr>
                                    <tr id="1">
                                        <td style="width: 150px;" data-order="0">
                                            <div class="truncate" style="width: 130px;">OF-0001        </div>
                                        </td>
                                        <td style="width: 150px;" data-order="1">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td style="width: 150px;" data-order="2">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td data-order="3">
                                            <div class="truncate">test</div>
                                        </td>
                                        <td style="width: 100px; text-align: center;" data-order="4">
                                            <div class="truncate" style="width: 80px;">12/04/2022</div>
                                        </td>
                                        <td style="width: 200px; text-align: left;" data-order="5">
                                            <div class="truncate" style="width: 180px;" title="Incidència">Incidència</div>
                                        </td>
                                        <td class="action-buttons" data-buttons="0" style="width: 17px; white-space: nowrap;"><a class="blue" id="1" onclick="GoItemView('101', this.id)"><i class="fa fa-fal fa-pencil-alt"></i></a></td>
                                    </tr>
                                    <tr id="1">
                                        <td style="width: 150px;" data-order="0">
                                            <div class="truncate" style="width: 130px;">OF-0001        </div>
                                        </td>
                                        <td style="width: 150px;" data-order="1">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td style="width: 150px;" data-order="2">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td data-order="3">
                                            <div class="truncate">test</div>
                                        </td>
                                        <td style="width: 100px; text-align: center;" data-order="4">
                                            <div class="truncate" style="width: 80px;">12/04/2022</div>
                                        </td>
                                        <td style="width: 200px; text-align: left;" data-order="5">
                                            <div class="truncate" style="width: 180px;" title="Incidència">Incidència</div>
                                        </td>
                                        <td class="action-buttons" data-buttons="0" style="width: 17px; white-space: nowrap;"><a class="blue" id="1" onclick="GoItemView('101', this.id)"><i class="fa fa-fal fa-pencil-alt"></i></a></td>
                                    </tr>
                                    <tr id="1">
                                        <td style="width: 150px;" data-order="0">
                                            <div class="truncate" style="width: 130px;">OF-0001        </div>
                                        </td>
                                        <td style="width: 150px;" data-order="1">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td style="width: 150px;" data-order="2">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td data-order="3">
                                            <div class="truncate">test</div>
                                        </td>
                                        <td style="width: 100px; text-align: center;" data-order="4">
                                            <div class="truncate" style="width: 80px;">12/04/2022</div>
                                        </td>
                                        <td style="width: 200px; text-align: left;" data-order="5">
                                            <div class="truncate" style="width: 180px;" title="Incidència">Incidència</div>
                                        </td>
                                        <td class="action-buttons" data-buttons="0" style="width: 17px; white-space: nowrap;"><a class="blue" id="1" onclick="GoItemView('101', this.id)"><i class="fa fa-fal fa-pencil-alt"></i></a></td>
                                    </tr>
                                    <tr id="1">
                                        <td style="width: 150px;" data-order="0">
                                            <div class="truncate" style="width: 130px;">OF-0001        </div>
                                        </td>
                                        <td style="width: 150px;" data-order="1">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td style="width: 150px;" data-order="2">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td data-order="3">
                                            <div class="truncate">test</div>
                                        </td>
                                        <td style="width: 100px; text-align: center;" data-order="4">
                                            <div class="truncate" style="width: 80px;">12/04/2022</div>
                                        </td>
                                        <td style="width: 200px; text-align: left;" data-order="5">
                                            <div class="truncate" style="width: 180px;" title="Incidència">Incidència</div>
                                        </td>
                                        <td class="action-buttons" data-buttons="0" style="width: 17px; white-space: nowrap;"><a class="blue" id="1" onclick="GoItemView('101', this.id)"><i class="fa fa-fal fa-pencil-alt"></i></a></td>
                                    </tr>
                                    <tr id="1">
                                        <td style="width: 150px;" data-order="0">
                                            <div class="truncate" style="width: 130px;">OF-0001        </div>
                                        </td>
                                        <td style="width: 150px;" data-order="1">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td style="width: 150px;" data-order="2">
                                            <div class="truncate" style="width: 130px;"></div>
                                        </td>
                                        <td data-order="3">
                                            <div class="truncate">test</div>
                                        </td>
                                        <td style="width: 100px; text-align: center;" data-order="4">
                                            <div class="truncate" style="width: 80px;">12/04/2022</div>
                                        </td>
                                        <td style="width: 200px; text-align: left;" data-order="5">
                                            <div class="truncate" style="width: 180px;" title="Incidència">Incidència</div>
                                        </td>
                                        <td class="action-buttons" data-buttons="0" style="width: 17px; white-space: nowrap;"><a class="blue" id="1" onclick="GoItemView('101', this.id)"><i class="fa fa-fal fa-pencil-alt"></i></a></td>
                                    </tr>
                                    <tr id="6">
                                        <td style="width: 150px;" data-order="0">
                                            <div class="truncate" style="width: 130px;">GNG-0007       </div>
                                        </td>
                                        <td style="width: 150px;" data-order="1">
                                            <div class="truncate" style="width: 130px;" title="Genogues">Genogues</div>
                                        </td>
                                        <td style="width: 150px;" data-order="2">
                                            <div class="truncate" style="width: 130px;" title="Medeles Gestión">Medeles Gestión</div>
                                        </td>
                                        <td style="width: undefinedpx;" data-order="3">
                                            <div class="truncate" style="width: NaNpx;">No muestra "Fora del centre"</div>
                                        </td>
                                        <td style="width: 100px; text-align: center;" data-order="4">
                                            <div class="truncate" style="width: 80px;">09/04/2022</div>
                                        </td>
                                        <td style="width: 200px; text-align: left;" data-order="5">
                                            <div class="truncate" style="width: 180px;" title="Incidència">Incidència</div>
                                        </td>
                                        <td class="action-buttons" data-buttons="0" style="width: 17px; white-space: nowrap;"><a class="blue" id="6" onclick="GoItemView('101', this.id)"><i class="fa fa-fal fa-pencil-alt"></i></a></td>
                                    </tr>
                                    <tr id="7">
                                        <td style="width: 150px;" data-order="0">
                                            <div class="truncate" style="width: 130px;">GNG-0008       </div>
                                        </td>
                                        <td style="width: 150px;" data-order="1">
                                            <div class="truncate" style="width: 130px;" title="Genogues">Genogues</div>
                                        </td>
                                        <td style="width: 150px;" data-order="2">
                                            <div class="truncate" style="width: 130px;" title="Medeles Gestión">Medeles Gestión</div>
                                        </td>
                                        <td style="width: undefinedpx;" data-order="3">
                                            <div class="truncate" style="width: NaNpx;">Cambiar "No sol·lititat" en llistat PEV</div>
                                        </td>
                                        <td style="width: 100px; text-align: center;" data-order="4">
                                            <div class="truncate" style="width: 80px;">28/04/2022</div>
                                        </td>
                                        <td style="width: 200px; text-align: left;" data-order="5">
                                            <div class="truncate" style="width: 180px;" title="Incidència">Incidència</div>
                                        </td>
                                        <td class="action-buttons" data-buttons="0" style="width: 17px; white-space: nowrap;"><a class="blue" id="7" onclick="GoItemView('101', this.id)"><i class="fa fa-fal fa-pencil-alt"></i></a></td>
                                    </tr>
                                    <tr id="8">
                                        <td style="width: 150px;" data-order="0">
                                            <div class="truncate" style="width: 130px;">GNG-0009       </div>
                                        </td>
                                        <td style="width: 150px;" data-order="1">
                                            <div class="truncate" style="width: 130px;" title="Genogues">Genogues</div>
                                        </td>
                                        <td style="width: 150px;" data-order="2">
                                            <div class="truncate" style="width: 130px;" title="Medeles Gestión">Medeles Gestión</div>
                                        </td>
                                        <td style="width: undefinedpx;" data-order="3">
                                            <div class="truncate" style="width: NaNpx;">No aparece número en dirección</div>
                                        </td>
                                        <td style="width: 100px; text-align: center;" data-order="4">
                                            <div class="truncate" style="width: 80px;">29/04/2022</div>
                                        </td>
                                        <td style="width: 200px; text-align: left;" data-order="5">
                                            <div class="truncate" style="width: 180px;" title="Incidència">Incidència</div>
                                        </td>
                                        <td class="action-buttons" data-buttons="0" style="width: 17px; white-space: nowrap;"><a class="blue" id="8" onclick="GoItemView('101', this.id)"><i class="fa fa-fal fa-pencil-alt"></i></a></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                </div>
                <div class="panel-footer" style="padding:2px;height:40px;margin-top:-1px;">
                    <div class="col-xs-4" style="padding:8px;">Nº registros:&nbsp;<span id="ListDataCount_Trastero_ByAlmacen">105 de 105</span></div>
                    <div class="col-xs-8" id="ListPager_Trastero_ByAlmacen" style="text-align: right;">
                        <div class="dataTables_paginate paging_simple_numbers" id="dynamic-table_paginate">
                            <ul class="pagination">
                                <li class="paginate_button previous disabled" aria-controls="dynamic-table" tabindex="0" id="dynamic-table_previous">
                                    <a><i class="fa fa-fast-backward"></i></a>
                                </li>
                                <li class="paginate_button active" aria-controls="dynamic-table" tabindex="0">
                                    <a onclick="ListPagerTo('Trastero_ByAlmacen',1);">1</a>
                                </li>
                                <li class="paginate_button " aria-controls="dynamic-table" tabindex="0">
                                    <a onclick="ListPagerTo('Trastero_ByAlmacen',2);">2</a>
                                </li>
                                <li class="paginate_button " aria-controls="dynamic-table" tabindex="0">
                                    <a>...</a>
                                </li>
                                <li class="paginate_button " aria-controls="dynamic-table" tabindex="0">
                                    <a onclick="ListPagerTo('Trastero_ByAlmacen',2);">23</a>
                                </li>
                                <li class="paginate_button " aria-controls="dynamic-table" tabindex="0">
                                    <a onclick="ListPagerTo('Trastero_ByAlmacen',2);">24</a>
                                </li>
                                <li class="paginate_button next" aria-controls="dynamic-table" tabindex="0" id="dynamic-table_next">
                                    <a><i class="fa fa-fast-forward"></i></a>
                                </li>
                            </ul>

                        </div>
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
        function ResizeWorkArea() {
            $(".panel-body").height($(window).height() - 237);
        }

        ResizeWorkArea();

        window.onresize = function () {
            ResizeWorkArea();
        }


    </script>
</asp:Content>