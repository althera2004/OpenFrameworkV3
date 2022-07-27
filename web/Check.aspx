<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Check.aspx.cs" Inherits="Check" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Literal runat="server" ID="LtInstances"></asp:Literal>
        </div>
        <asp:TextBox runat="server" ID="TxtInstanceName"></asp:TextBox>
        <asp:Button runat="server" ID="BtnLoadInstance" Text="Cargar" OnClick="BtnLoadInstance_Click" />
    </form>
</body>
</html>
