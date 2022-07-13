<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Alerts.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Feature.Alerts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="StylesHead" Runat="Server">
    <link type="text/css" rel="stylesheet" href="/vendor/fooTable/footable.core.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="hpanel">
                <div class="panel-body">
                    <input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search in table">
                    <table id="example1" class="footable table table-stripped toggle-arrow-tiny tablet breakpoint footable-loaded" data-page-size="8" data-filter="#filter">
                        <thead>
                        <tr>

                            <th data-toggle="true" class="footable-visible footable-first-column footable-sortable">Project<span class="footable-sort-indicator"></span></th>
                            <th class="footable-visible footable-sortable">Name<span class="footable-sort-indicator"></span></th>
                            <th class="footable-visible footable-sortable">Phone<span class="footable-sort-indicator"></span></th>
                            <th class="footable-visible footable-last-column footable-sortable">Company<span class="footable-sort-indicator"></span></th>
                            <th data-hide="all" class="footable-sortable" style="display: none;">Completed<span class="footable-sort-indicator"></span></th>
                            <th data-hide="all" class="footable-sortable" style="display: none;">Task<span class="footable-sort-indicator"></span></th>
                            <th data-hide="all" class="footable-sortable" style="display: none;">Date<span class="footable-sort-indicator"></span></th>
                        </tr>
                        </thead>
                        <tbody>
                        <tr class="footable-even footable-detail-show" style="display: table-row;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Alpha project</td>
                            <td class="footable-visible">Alice Jackson</td>
                            <td class="footable-visible">0500 780909</td>
                            <td class="footable-visible footable-last-column">Nec Euismod In Company</td>
                            <td style="display: none;"><span class="pie">6,9</span></td>
                            <td style="display: none;">40%</td>
                            <td style="display: none;">Jul 16, 2013</td>
                        </tr><tr class="footable-row-detail"><td class="footable-row-detail-cell" colspan="4"><div class="footable-row-detail-inner"><div class="footable-row-detail-row"><div class="footable-row-detail-name">Completed:</div><div class="footable-row-detail-value"><span class="pie">6,9</span></div></div><div class="footable-row-detail-row"><div class="footable-row-detail-name">Task:</div><div class="footable-row-detail-value">40%</div></div><div class="footable-row-detail-row"><div class="footable-row-detail-name">Date:</div><div class="footable-row-detail-value">Jul 16, 2013</div></div></div></td></tr>
                        <tr class="footable-odd" style="display: table-row;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Betha project</td>
                            <td class="footable-visible">John Smith</td>
                            <td class="footable-visible">0800 1111</td>
                            <td class="footable-visible footable-last-column">Erat Volutpat</td>
                            <td style="display: none;"><span class="pie">3,1</span></td>
                            <td style="display: none;">75%</td>
                            <td style="display: none;">Jul 18, 2013</td>
                        </tr>
                        <tr class="footable-even" style="display: table-row;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Gamma project</td>
                            <td class="footable-visible">Anna Jordan</td>
                            <td class="footable-visible">(016977) 0648</td>
                            <td class="footable-visible footable-last-column">Tellus Ltd</td>
                            <td style="display: none;"><span class="pie">4,9</span></td>
                            <td style="display: none;">18%</td>
                            <td style="display: none;">Jul 22, 2013</td>
                        </tr>
                        <tr class="footable-odd" style="display: table-row;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Alpha project</td>
                            <td class="footable-visible">Alice Jackson</td>
                            <td class="footable-visible">0500 780909</td>
                            <td class="footable-visible footable-last-column">Nec Euismod In Company</td>
                            <td style="display: none;"><span class="pie">6,9</span></td>
                            <td style="display: none;">40%</td>
                            <td style="display: none;">Jul 16, 2013</td>
                        </tr>
                        <tr class="footable-even" style="display: table-row;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Gamma project</td>
                            <td class="footable-visible">Anna Jordan</td>
                            <td class="footable-visible">(016977) 0648</td>
                            <td class="footable-visible footable-last-column">Tellus Ltd</td>
                            <td style="display: none;"><span class="pie">4,9</span></td>
                            <td style="display: none;">18%</td>
                            <td style="display: none;">Jul 22, 2013</td>
                        </tr>
                        <tr class="footable-odd" style="display: table-row;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Alpha project</td>
                            <td class="footable-visible">Alice Jackson</td>
                            <td class="footable-visible">0500 780909</td>
                            <td class="footable-visible footable-last-column">Nec Euismod In Company</td>
                            <td style="display: none;"><span class="pie">6,9</span></td>
                            <td style="display: none;">40%</td>
                            <td style="display: none;">Jul 16, 2013</td>
                        </tr>
                        <tr class="footable-even" style="display: table-row;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Betha project</td>
                            <td class="footable-visible">John Smith</td>
                            <td class="footable-visible">0800 1111</td>
                            <td class="footable-visible footable-last-column">Erat Volutpat</td>
                            <td style="display: none;"><span class="pie">3,1</span></td>
                            <td style="display: none;">75%</td>
                            <td style="display: none;">Jul 18, 2013</td>
                        </tr>
                        <tr class="footable-odd" style="display: table-row;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Gamma project</td>
                            <td class="footable-visible">Anna Jordan</td>
                            <td class="footable-visible">(016977) 0648</td>
                            <td class="footable-visible footable-last-column">Tellus Ltd</td>
                            <td style="display: none;"><span class="pie">4,9</span></td>
                            <td style="display: none;">18%</td>
                            <td style="display: none;">Jul 22, 2013</td>
                        </tr>
                        <tr class="footable-even" style="display: none;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Alpha project</td>
                            <td class="footable-visible">Alice Jackson</td>
                            <td class="footable-visible">0500 780909</td>
                            <td class="footable-visible footable-last-column">Nec Euismod In Company</td>
                            <td style="display: none;"><span class="pie">6,9</span></td>
                            <td style="display: none;">40%</td>
                            <td style="display: none;">Jul 16, 2013</td>
                        </tr>
                        <tr class="footable-odd" style="display: none;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Gamma project</td>
                            <td class="footable-visible">Anna Jordan</td>
                            <td class="footable-visible">(016977) 0648</td>
                            <td class="footable-visible footable-last-column">Tellus Ltd</td>
                            <td style="display: none;"><span class="pie">4,9</span></td>
                            <td style="display: none;">18%</td>
                            <td style="display: none;">Jul 22, 2013</td>
                        </tr>
                        <tr class="footable-even" style="display: none;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Betha project</td>
                            <td class="footable-visible">John Smith</td>
                            <td class="footable-visible">0800 1111</td>
                            <td class="footable-visible footable-last-column">Erat Volutpat</td>
                            <td style="display: none;"><span class="pie">3,1</span></td>
                            <td style="display: none;">75%</td>
                            <td style="display: none;">Jul 18, 2013</td>
                        </tr>
                        <tr class="footable-odd" style="display: none;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Gamma project</td>
                            <td class="footable-visible">Anna Jordan</td>
                            <td class="footable-visible">(016977) 0648</td>
                            <td class="footable-visible footable-last-column">Tellus Ltd</td>
                            <td style="display: none;"><span class="pie">4,9</span></td>
                            <td style="display: none;">18%</td>
                            <td style="display: none;">Jul 22, 2013</td>
                        </tr>
                        <tr class="footable-even" style="display: none;">
                            <td class="footable-visible footable-first-column"><span class="footable-toggle"></span>Alpha project</td>
                            <td class="footable-visible">Alice Jackson</td>
                            <td class="footable-visible">0500 780909</td>
                            <td class="footable-visible footable-last-column">Nec Euismod In Company</td>
                            <td style="display: none;"><span class="pie">6,9</span></td>
                            <td style="display: none;">40%</td>
                            <td style="display: none;">Jul 16, 2013</td>
                        </tr>
                        </tbody>
                        <tfoot>
                        <tr>
                            <td colspan="5" class="footable-visible">
                                <ul class="pagination pull-right"><li class="footable-page-arrow disabled"><a data-page="first" href="#first">«</a></li><li class="footable-page-arrow disabled"><a data-page="prev" href="#prev">‹</a></li><li class="footable-page active"><a data-page="0" href="#">1</a></li><li class="footable-page"><a data-page="1" href="#">2</a></li><li class="footable-page-arrow"><a data-page="next" href="#next">›</a></li><li class="footable-page-arrow"><a data-page="last" href="#last">»</a></li></ul>
                            </td>
                        </tr>
                        </tfoot>
                    </table>

                </div>
            </div>
        </div>

    </div>
</asp:Content>

