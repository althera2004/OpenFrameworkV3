<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InitSession.aspx.cs" Inherits="InitSession" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="/assets/js/jquery-2.1.4.min.js"></script>
    
    <script type="text/javascript" charset="utf-8" src ="/js/base64.js"></script>
    <script type="text/javascript" charset="utf-8" src ="/js/navigation.js"></script>
    <script type="text/javascript" charset="utf-8" src ="/js/common.js"></script>
        <script>
            var PageType = "Init";
            var Instance = <%=this.Instance.Config.JsonData %>;
            var user = <%=this.ApplicationUser.JsonSimple %>;
            var Company = <%=this.Company.Json %>;
            var ItemDefinitions = <%= this.ItemDefinitions  %>;
            var Menu = <%=this.MenuJson %>;

            localStorage.setItem("Instance", JSON.stringify(Instance));
            localStorage.setItem("ItemDefinitions", JSON.stringify(ItemDefinitions));
            localStorage.setItem("ApplicationUser", JSON.stringify(user));
            localStorage.setItem("Company", JSON.stringify(Company));
            localStorage.setItem("Menu", JSON.stringify(Menu));

            if (Company.Id > 0) {
                GoEncryptedPage("/Instances/" + Instance.Name + "/Pages/DashBoard.aspx");
            }
        </script>
</head>
<body>
    <asp:Literal runat="server" ID="LtCompanies"></asp:Literal>
</body>
</html>