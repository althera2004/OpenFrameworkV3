<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ItemList.aspx.cs" Inherits="ItemList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentScriptVars" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="row" id="TableList"></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentScriptFiles" Runat="Server">
    <script type="text/javascript" src="/js/Layout.js"></script>
    <script type="text/javascript" src="/js/ListTools.js"></script>
    <script type="text/javascript" src="/js/ItemTools.js"></script>
    <script type="text/javascript" src="/Instances/<%=this.InstanceName %>/Scripts/<%=this.ItemName %>_List.js"></script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentScripts" Runat="Server">
    <script type="text/javascript">
        var ItemDefinitionId = <%=this.ItemDefinitionId %>;
        var ItemDefinitions = LocalStorageGetJson("ItemDefinitions");
        var ListId = "<%= this.ListId %>";

        var ItemDefinition = ItemDefinitionById(ItemDefinitionId);
        var ListDefinition = ItemListById(ItemDefinition, ListId);

        ListSources.push(new PageList({
            "ListDefinition": ListDefinition,
            "ListId": ListDefinition.Id,
            "ItemName": ItemDefinition.ItemName,
            "ItemDefinition": ItemDefinition,
            "CustomAjaxSource": ListDefinition.CustomAjaxSource,
            "Parameters": ListDefinition.Parameters,
            "Filter": null,
            "Data": [],
            "FilteredData": []
        }));

        ListSources[0].RenderPageList();
        ListSources[0].GetData();


        ListsItemSearch.push({
            "Tabid": "0",
            "ItemName": ListSources[0].ItemName,
            "ListId": ListSources[0].ListId,
            "ColumnsIndex": [],
            "Items": []
        });

        function ResizeWorkArea() {
            $(".panel-body").height($(window).height() - 237);
        }

        window.onresize = function () {
            ResizeWorkArea();
        }        
    </script>
</asp:Content>