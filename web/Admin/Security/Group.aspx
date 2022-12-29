<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Group.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Security.Group" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentScriptVars" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="row" id="PersistentFields">
        <div class="form-group col-xs-12">
            <div class="row">
                <label id="Name_Label" for="Name" class="col-sm-1 control-label">Nom<span style="color:#f00;">*</span></label>
                <div class="col-sm-5">
                    <input id="Name" type="text" class="form-control" value="<%=this.GroupData.Name %>" />
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <div class="hpanel">
                <ul class="nav nav-tabs" id="FormTabs">
                    <li id="tabSelect-basic" class="tabSelect active"><a data-toggle="tab" href="#tab-basic" aria-expanded="false">Dades bàsiques</a></li>
                    <li id="tabSelect-membership" class="tabSelect"><a data-toggle="tab" href="#tab-membership" aria-expanded="false">Membres</a></li>
                    <li id="tabSelect-dataGrants" class="tabSelect"><a data-toggle="tab" href="#tab-dataGrants" aria-expanded="false">Accés a dades</a></li>
                    <li id="tabSelect-specialGrants" class="tabSelect"><a data-toggle="tab" href="#tab-specialGrants" aria-expanded="false">Permissos especials</a></li>
                </ul>
                <div class="tab-content">
                    <div id="tab-basic" class="tab-pane active">
                        <div class="panel-body panel-body-form">
                            <label id="Description_Label" for="Description" class="col-sm-1 control-label">Descripció</label>
                            <div class="col-sm-11">
                                <textarea id="Description" type="text" class="form-control" rows="2"><%=this.GroupData.Description %></textarea>
                            </div>

                            <div class="panel-heading" style="padding-top:12px;">Tipus de grup</div>
                            <div class="panel-body form-group" style="border: none;">
                                <div class="col-sm-2">
                                    <label class="checkbox-inline">
                                        <input type="checkbox" value="option1" id="Core" <%= this.GroupData.Core ? " checked=\"checked\"" : string.Empty %> />&nbsp;CORE
                                    </label>
                                </div>
                                <div class="col-sm-2">
                                    <label class="checkbox-inline">
                                        <input type="checkbox" value="option1" id="Deletable" <%= this.GroupData.Deletable ? " checked=\"checked\"" : string.Empty %> />&nbsp;Eliminable
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="tab-membership" class="tab-pane">
                        <div class="panel-body panel-body-form">
                            <div class="panel-heading">Membres</div>
                            <div>
                                <select multiple="multiple" size="10" name="duallistbox_membership" class="membership" title="duallistbox_membership">
                                    <asp:Literal runat="server" ID="LtUsers"></asp:Literal>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div id="tab-dataGrants" class="tab-pane">
                        <div class="panel-body panel-body-form">
                            <h5>Accés a dades</h5>
                            

                            <div id="AccessGrants_List" class="ListContainer">
                                <div class="hpanel hblue hpanel-table" style="margin: 0;">
                                    <div class="tableHead">
                                        <table cellpadding="1" cellspacing="1" class="table">
                                            <thead id="AccessGrants_ListHead">
                                                <tr>
                                                    <th id="th1" colspan="2" class="search" data-sortfield="Name" data-sorttype="text" data-tableid="AccessGrants">Nom</th>
                                                    <th id="th2" style="width: 100px;text-align:center" class="search">Lectura</th>
                                                    <th id="th3" style="width: 100px;text-align:center" class="search">Modificació</th>
                                                    <th id="th4" style="width: 100px;text-align:center" class="search">Eliminació</th>
                                                    <th style="padding: 0; width: 27px; border-right: none;"></th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>
                                    <div class="panel-body panel-body-list" style="height: 671px;">
                                        <div class="table-responsive">
                                            <div class="table-body" style="max-height: 100%; height: 100%">
                                                <table cellpadding="1" cellspacing="1" class="table" style="max-height: 100%">
                                                    <tbody style="max-height: 100%" id="AccessGrants_ListBody">
                                                        <asp:Literal runat="server" ID="LtAccessGrants"></asp:Literal>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- panel-body -->
                                    <div class="panel-footer panel-footer-list">Nº ítems: <strong id="AccessGrants_ListCount">8</strong>    </div>
                                    <!-- panel-body -->
                                </div>
                            </div>


                        </div>
                    </div>
                    <div id="tab-specialGrants" class="tab-pane">
                        <div class="panel-body panel-body-form">
                            <h5>Permissos especials</h5>
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
        var originalGroupData = <%=this.GroupData.Json %>;
        var groupData = <%=this.GroupData.Json %>;
    </script>
</asp:Content>