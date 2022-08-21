<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InitSession.aspx.cs" Inherits="InitSession" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="ca-es">
<head runat="server">
    <title>OpenFramework - Init</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta charset="utf-8" />
    <script src="/assets/js/jquery-2.1.4.min.js"></script>
    
    <script type="text/javascript" charset="utf-8" src ="/js/base64.js"></script>
    <script type="text/javascript" charset="utf-8" src ="/js/navigation.js"></script>
    <script type="text/javascript" charset="utf-8" src ="/js/common.js"></script>
        <script>
            var PageType = "Init";
            var Instance = <%=this.Instance.Config.JsonData %>;
            var user = <%=this.ApplicationUser.Json %>;
            var Company = <%=this.Company.Json %>;
            var ItemDefinitions = <%= this.ItemDefinitions  %>;
            var Menu = <%=this.MenuJson %>;
            var Language = "<%=this.Language %>";

            localStorage.setItem("Instance", JSON.stringify(Instance));
            localStorage.setItem("ItemDefinitions", JSON.stringify(ItemDefinitions));
            localStorage.setItem("ApplicationUser", JSON.stringify(user));
            localStorage.setItem("Company", JSON.stringify(Company));
            localStorage.setItem("Menu", JSON.stringify(Menu));

            if (Company.Id > 0) {
                GoLandingPage();
            }

            function GoLandingPage() {
                GoEncryptedPage("/Instances/" + Instance.Name + "/Pages/DashBoard.aspx");
            }
        </script>
</head>
<body>
    <asp:Literal runat="server" ID="LtCompanies"></asp:Literal>
    <button type="button" onclick="GoLandingPage()">Continuar</button>
</body>
</html>