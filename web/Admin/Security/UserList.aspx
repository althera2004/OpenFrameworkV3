<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="UserList.aspx.cs" Inherits="OpenFrameworkV3.Web.Admin.Security.UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentWorkSpace" runat="Server">
    <div class="row">
        <div class="col-lg-12" id="BankAccountList">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    <%=this.master.Translate("Core_Security_ApplicationUsers") %>
                    <div class="panel-tools">
                        <a class="_showhide" style="cursor:pointer;" id="SECURITYUSER_AddBtn" onclick="GoUserNew();"><i class="fa fa-plus"></i>&nbsp;<%=this.master.Translate("Common_Add") %> <%=this.master.Translate("Core_Security_ApplicationUser") %></a>
                    </div>
                </div>
                <div class="tableHead">
                    <table cellpadding="1" cellspacing="1" class="table">
                        <thead>
                            <tr>
                                <th style="width: 300px;"><%= this.master.Translate("Core_Security_ApplicationUsers_Header_Email") %></th>
                                <th><%= this.master.Translate("Core_Security_ApplicationUsers_Header_Name") %></th>
                                <th style="width: 120px;"><%= this.master.Translate("Core_Security_ApplicationUsers_Header_Language") %></th>
                                <th style="width: 120px; text-align: center;"><%= this.master.Translate("Core_Security_ApplicationUsers_Header_Core") %></th>
                                <th style="width: 120px; text-align: center;"><%= this.master.Translate("Core_Security_ApplicationUsers_Header_Admin") %></th>
                                <th class="action-buttons-header" data-buttons="2" style="width:17px; white-space: nowrap;"></th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div class="panel-body" style="padding: 0;">
                    <div class="table-responsive" style="max-height: 100%; height: 100%; overflow-y: scroll; overflow-x: hidden">
                        <div class="table-body" style="max-height: 100%; height: 100%">
                            <table cellpadding="1" cellspacing="1" class="table" style="max-height: 100%">
                                <tbody style="max-height: 100%" id="APPLICATIONUSERS_TBody">
                                    <asp:Literal runat="server" ID="LtListData"></asp:Literal></tbody>
                            </table>
                        </div>
                    </div>

                </div>
                <div class="panel-footer">
                    <%=this.master.Translate("Common_RegisterCount") %>: <strong id="APPLICATIONUSERS_TotalRows"><asp:Literal runat="server" ID="LtListCount"></asp:Literal></strong>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScripts" runat="server">
    <script type="text/javascript">
        function ResizeWorkArea() {
            $(".panel-body").height($(window).height() - 250);
        }


        window.onresize = function () {
            ResizeWorkArea();
        }

        MenuSelectOption("1001");
    </script>
</asp:Content>