<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Group.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Security.Group" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentScriptVars" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="row" id="PersistentFields">
        <div class="form-group col-xs-12">
            <div class="row">
                <label id="Name_Label" for="Name" class="col-sm-1 control-label">Nom</label>
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
                    <li id="tabSelect-varificacio" class="tabSelect"><a data-toggle="tab" href="#tab-grants" aria-expanded="false">Permissos</a></li>
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
                            <div class="panel-heading">Membres</div>
                            <div>
                                <select multiple="multiple" size="10" name="duallistbox_membership" class="membership" title="duallistbox_membership">
                                    <asp:Literal runat="server" ID="LtUsers"></asp:Literal>
                                </select>
                            </div>
                    </div>
                    <div id="tab-grants" class="tab-pane">
                        <div class="panel-body panel-body-form">
                            <h5>Accés a dades</h5>
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