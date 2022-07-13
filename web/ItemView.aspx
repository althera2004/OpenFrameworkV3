<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ItemView.aspx.cs" Inherits="ItemView" EnableSessionState="False" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentScriptVars" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="row" id="PersistentFields"></div>
    <div class="col-lg-12">
        <div class="hpanel">
            <ul class="nav nav-tabs" id="FormTabs"></ul>
            <div class="tab-content" id="FormContent"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentScriptFiles" Runat="Server">
    <script type="text/javascript" src="/js/Layout.js"></script>
    <script type="text/javascript" src="/js/ListTools.js"></script>
    <script type="text/javascript" src="/js/ItemTools.js"></script>
    <script type="text/javascript" src="/js/FormTools.js"></script>
    <script type="text/javascript" src="/Instances/<%=this.InstanceName %>/Scripts/<%=this.ItemName %>.js"></script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentScripts" Runat="Server">
    <script type="text/javascript">
        var ItemDefinitionId = <%=this.ItemDefinitionId %>;
        var ItemDefinitions = LocalStorageGetJson("ItemDefinitions");
        var ListId = "<%= this.ListId %>";
        var FormId = "<%= this.FormId %>";
        var ItemId = <%= this.ItemId %>;

        var ItemDefinition = ItemDefinitionById(ItemDefinitionId);
        var ListDefinition = ItemListById(ItemDefinition, ListId);
        var FormDefinition = ItemFormById(ItemDefinition, FormId);

        $("#FormBtnCancel").on("click", function () {
            GoEncryptedList(ItemDefinition.ItemName, ListId);
        })

        var Form = new PageForm({ "ItemId": ItemDefinitionId, "FormId": FormId });
        var ItemData = new Item();
        ItemData.ItemDefinition = ItemDefinition;
        Form.Init();
        Form.Render("Form");

        function ResizeWorkArea() {
            $(".panel-body").height($(window).height() - 277);
        }


        window.onresize = function () {
            ResizeWorkArea();
        }

        
        if (typeof <%= this.ItemName.ToUpperInvariant()%>_CustomActions === "function") {
            <%= this.ItemName.ToUpperInvariant()%>_CustomActions();
        } else {
            console.log("No custom actions", "<%= this.ItemName.ToUpperInvariant()%>_CustomActions");
        }

        var ItemDataLoaded = false;

        /* Obtener FK */
        // --------------------
        // Si tiene FL hay que esperar los FK antes de obtener los datos y rellenar el formulario
        if (HasArrayValues(ItemDefinition.ForeignValues)) {
            for (var fk = 0; fk < ItemDefinition.ForeignValues.length; fk++) {
                GetFKItem(ItemDefinition.ForeignValues[fk].ItemName);
            }
        }
        else {
            GetItemDataJson(ItemDefinition.ItemName, ItemId, FillFormItemFromJson);
        }
        // --------------------
        


        for (var pl = 0; pl < ListSources.length; pl++) {
            ListSources[pl].Render();
        }
    </script>
</asp:Content>

