<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="DashBoard.aspx.cs" Inherits="OpenFramework.Instance.ViuLleure.DashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptVars" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">	
	<h4>Entono de pruebas</h4>
	<p>Los datos actuales de la aplicaicón son de pruebas y no tendrán validez.</p>
	<h5>Aplicaciones para dispositivos</h5>
	<ul>
		<li><a href="/Instances/NYRS/API/APPDatos.apk">APP de datos</a></li>
		<li><a href="/Instances/NYRS/API/APPVideo.apk">APP de videos</a></li>
	</ul>
	<asp:Literal runat="server" ID="LtDistribucion"></asp:Literal>
	<div class="row">
		<div class="col-xs-6"><asp:Literal runat="server" ID="LtNoVideo"></asp:Literal></div>
		<div class="col-xs-6"><asp:Literal runat="server" ID="LtNoDatos"></asp:Literal></div>
	</div>
	
	
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentScriptFiles" Runat="Server">
	<script type="text/javascript" src="/Instances/<%=this.InstanceName %>/Pages/Dashboard.js"></script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentScripts" Runat="Server">
	<script type="text/javascript">
    </script>
</asp:Content>