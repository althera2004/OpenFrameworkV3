<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="GroupList.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Security.GroupList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
   <div class="row">
        <div class="col-lg-12" id="BankAccountList">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    <%= this.master.Translate("Core_SecurityGroup_Plural") %>
                    <div class="panel-tools">
                    </div>s
                </div>
                        <div class="tableHead">
                            <table cellpadding="1" cellspacing="1" class="table">
                                <thead>
                                    <tr>
                                        <th style="width:300px;"><%= this.master.Translate("Core_SecurityGroup_Header_Name") %></th>
                                        <th><%= this.master.Translate("Core_SecurityGroup_Header_Description") %></th>
                                        <th style="width:120px;text-align: center;"><%= this.master.Translate("Core_SecurityGroup_Header_Core") %></th>
                                        <th style="width:120px;text-align: center;"><%= this.master.Translate("Core_SecurityGroup_Header_Editable") %></th>
                                        <th class="action-buttons-header" data-buttons="2" style="width:17px;white-space:nowrap;"></th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                <div class="panel-body" style="padding:0;">
                    <div class="table-responsive" style="max-height:100%;height:100%;overflow-y:scroll;overflow-x:hidden">
                        <div class="table-body" style="max-height:100%;height:100%">
                            <table cellpadding="1" cellspacing="1" class="table" style="max-height:100%">
                                <tbody style="max-height:100%"><asp:Literal runat="server" ID="LTListData"></asp:Literal></tbody>
                            </table>
                        </div>
                    </div>

                </div>
                <div class="panel-footer">
                    <%= this.master.Translate("Common_RegisterCount") %>: <strong><asp:Literal runat="server" ID="LtListCount"></asp:Literal></strong>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScripts" runat="server">
    <script type="text/javascript">
        function ResizeWorkArea() {
            $(".panel-body").height($(window).height() - 237);
        }


        window.onresize = function () {
            ResizeWorkArea();
        }

        MenuSelectOption("1002");
    </script>
</asp:Content>