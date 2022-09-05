<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ItemView.aspx.cs" Inherits="ItemView" EnableSessionState="False" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentScriptVars" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentWorkSpace" Runat="Server">
    <div class="row" id="PersistentFields"></div>
    <div class="col-lg-12" style="padding-left:0;">
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
        var JsonData = <%= this.JsonData %>;
        var sticks = <%=this.Stick %>;

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
        // Si tiene FK hay que esperar los FK antes de obtener los datos y rellenar el formulario

        /*var HasFK = false;

        console.log(FK);
        if (HasArrayValues(ItemDefinition.ForeignValues)) {
            for (var fk = 0; fk < ItemDefinition.ForeignValues.length; fk++) {
                var fkItemName = ItemDefinition.ForeignValues[fk].ItemName;
                var fkItemDefinition = ItemDefinitionByName(fkItemName);
                if (typeof fkItemDefinition.Features !== "undefined") {
                    if (fkItemDefinition.Features.Persistence === true) {
                        continue;
                    }
                }

                HasFK = true;
                GetFKItem(fkItemName);
            }
        }

        if (Form.HasApplicationUserFields) {
            //HasFK = true;
            GetFKApplicationUsers();
        }

        if(HasFK === false) {
            GetItemDataJson(ItemDefinition.ItemName, ItemId, FillFormItemFromJson);
        }
        // --------------------
        

*/
        for (var pl = 0; pl < ListSources.length; pl++) {
            ListSources[pl].Render();
        }


         <%=this.FK %>
        console.log(FK);
        var fks = Object.keys(FK);
        for (var f = 0; f < fks.length; f++) {
            FillComboFromFK(fks[f] + "Id", fks[f]);
        }

        FillFormItemFromJson(JsonData);
    </script>
</asp:Content>

