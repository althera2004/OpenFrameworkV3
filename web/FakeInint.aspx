<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FakeInint.aspx.cs" Inherits="FakeInint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <!--script src="/developer/ItemDefinitions.js"></!--script-->
    <script src="/assets/js/jquery-2.1.4.min.js"></script>
    
    <script type="text/javascript" charset="utf-8" src ="/js/base64.js"></script>
    <script type="text/javascript" charset="utf-8" src ="/js/navigation.js"></script>
    <script type="text/javascript" charset="utf-8" src ="/js/common.js"></script>
        <script>
            var pageType = "fake";
            var Instance = <%=this.Instance.Config.JsonData %>;
            var user = <%=this.ActualUser.Json %>;
            var Company = <%=this.Company.Json %>;
            var ItemDefinitions = <%= this.ItemDefinitions  %>;

            localStorage.setItem('Instance', JSON.stringify(Instance));
            localStorage.setItem('ItemDefinitions', JSON.stringify(ItemDefinitions));
            localStorage.setItem('ApplicationUser', JSON.stringify(user));
            localStorage.setItem('Company', JSON.stringify(Company));

            var test = JSON.parse(localStorage.getItem("Instance"));

            function ReloadInstance() {
                var actionData = {
                    "instanceName": "support"
                };

                $.ajax({
                    "type": "POST",
                    "url": "/Async/Instance.ashx",
                    "data": {
                        "action": "Reload",
                        "actionData": JSON.stringify(actionData)
                    },
                    "dataType": "json",
                    "success": function (response) {
                        console.log(response);
                    }
                });
            }
        </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:TextBox runat="server" ID="TxtInstanceName"></asp:TextBox>
        <asp:LinkButton runat="server" ID="LnkReload" OnClick="LnkReload_Click">Reload</asp:LinkButton>
        <asp:Literal runat="server" ID="LtCns"></asp:Literal>

        <hr />
        
        <asp:Literal runat="server" ID="LtLink"></asp:Literal>
        <hr />
        
        <asp:Literal runat="server" ID="LtList"></asp:Literal>
    </form>
</body>
</html>