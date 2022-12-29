var ItemDefinitions = LocalStorageGetJson("ItemDefinitions");
var CodeMirror_Context = {
    "SQL": null,
    "JavaScript": null,
    "Definition": null
};

window.onload = function () {
    $("#BreadCrumbLabel").html("Instància: " + Instance.Name);
    $("#InstanceNameTitle").html(Instance.Name);
    $("#InstanceName").html(Instance.Name);

    $("#InstanceMulticompany").html(Instance.MultiCompany ? Dictionary.Common_Yes : Dictionary.Common_No);
    $("#InstanceSessionTimeout").html(Instance.SessionTimeout);
    $("#InstanceDescription").html(Instance.Description);
    $("#InstanceDefaultLanguage").html(Instance.DefaultLanguage);

    INSTANCE_RenderItems();
    INSTANCE_RenderFixedLists();
    INSTANCE_RenderScript();
    $("#tabSelect-scripts").on("click", INSTANCE_RenderScriptRefresh);
}

function INSTANCE_RenderScript() {
    if (CodeMirror_Context.JavaScript === null) {
        console.log("CREATE", "Javascript");
        var editor = CodeMirror.fromTextArea(document.getElementById("Editor"), {
            "mode": "text/javascript",
            "lineNumbers": true,
            "matchBrackets": true,
            "continueComments": "Enter",
            "extraKeys": { "Ctrl-Q": "toggleComment" }
        });
        CodeMirror_Context.JavaScript = editor;
    }
}

function INSTANCE_RenderScriptRefresh(sender) {
    if (sender.currentTarget.id === "tabSelect-scripts") {
        setTimeout(function () { CodeMirror_Context.JavaScript.refresh(); }, 10);
    }
}

function INSTANCE_RenderItems() {
    var res = "";
    var htmlList = "";

    for (var x = 0; x < ItemDefinitions.length; x++) {
        res += INSTANCE_RenderItem(ItemDefinitions[x]);
        htmlList += ISNTANCE_RenderItemRow(ItemDefinitions[x]);
    }


    $("#ItemsDiv").html(res);
    $("#ItemsDataList").html(htmlList);
    $("#ListItemsTotal").html(ItemDefinitions.length);
}


function ISNTANCE_RenderItemRow(item) {
    var following = "light-grey";
    var faq = FeatureIsEnabled(item, "FAQ") ? "black" : "light-grey";
    var attach = FeatureIsEnabled(item, "Attach") ? "blue" : "light-grey";


    var scriptList = $.inArray(item.ItemName + '_List.js', scriptFiles) === -1 ? "ban red" : "check green";
    var scriptView = $.inArray(item.ItemName + '.js', scriptFiles) === -1 ? "ban red" : "check green";

    var iconUserAssignation = "fa fa-user light-grey";
    var iconSchedule = "fa-calendar-alt light-grey";
    var iconSticky = "fa-sticky-note light-grey";
    var iconContactPerson = "fa-users light-grey";
    if (typeof item.Features !== "undefined") {
        if (FeatureIsEnabled(item, "UserAssignation")) {
            iconUserAssignation = "fa fa-user-check blue";
        }

        if (FeatureIsEnabled(item, "Schedule")) {
            iconSchedule = "fa-calendar-alt blue";
        }

        if (FeatureIsEnabled(item, "Sticky")) {
            iconSticky = "fa-sticky-note blue";
        }

        if (FeatureIsEnabled(item, "ContactPerson")) {
            iconContactPerson = "fa-users blue";
        }
    }

    var res = " \
            <tr style=\"height:30px;\" class=\"RowItem\" onclick=\"GoEncryptedPage('/Admin/Instance/ItemDefinition.aspx', {\'Id\':"+ item.Id + "});\"> \
                <td style =\"height:25px;width:25px;\"> \
                    <i class=\""+ item.Layout.Icon + " blue\"></i></td><td>" + item.Layout.Label + "\
                </td> \
                <td>"+ item.Id + " - " + item.ItemName + "</td> \
                <td style=\"width:30px;text-align:center;\"><i class=\"fa fa-" + scriptList + "\" title=\"Script de llista\"></i></td>; \
                <td style=\"width:30px;text-align:center;\"><i class=\"fa fa-" + scriptView + "\" title=\"Script de fitxa\"></i></td>; \
                <td style=\"width:30px;text-align:center;\"><i class=\"fa fa-star "+ following + "\" title=\"Seguiment\"></i></td>; \
                <td style=\"width:30px;text-align:center;\"><i class=\"fa "+ iconSchedule + "\" title=\"Agendable\"></i></td>; \
                <td style=\"width:30px;text-align:center;\"><i class=\"fa "+ iconSticky + "\" title=\"Stickys\"></i></td>; \
                <td style=\"width:30px;text-align:center;\"><i class=\"fa "+ iconContactPerson + "\" title=\"Contact person\"></i></td>; \
                <td style=\"width:30px;text-align:center;\"><i class=\"fa fa-comment-alt "+ faq + "\" title=\"Anotacions\"></i></td>; \
                <td style=\"width:30px;text-align:center;\"><i class=\"fa fa-paperclip "+ attach + "\" title=\"Adjunts\"></i></td>; \
                <td style=\"width:30px;text-align:center;\"><i class=\"fa fa-anchor "+ faq + "\" title=\"Persistent\"></i></td>; \
                <td style=\"width:30px;text-align:center;\"><i class=\"fa fa-clipboard-check "+ faq + "\" title=\"Selecció rápida\"></i></td>; \
                <td style=\"width:30px;text-align:center;\"><i class=\"fa fa-eye light-grey\" title=\"Àmbits\"></i></td>; \
                <td style=\"width:30px;text-align:center;\"><i class=\"fa fa-map-marker-alt light-grey\" title=\"Geolocalizable\"></i></td> \
                <td style=\"width:30px;text-align:center;\"><i class=\"fa fa-list-ol light-grey\" title=\"Traces\"></i></td> \
                <td style=\"width:30px;text-align:center;\"><i class=\"fa fa-question-circle "+ faq + "\" title=\"FAQs\"></i></td>; \
                <td style=\"width:30px;text-align:center;\"><i class=\"fa fa-tag light-grey\" title=\"Tags\"></i></td> \
                <td style=\"width:30px;text-align:center;\"><i class=\""+ iconUserAssignation + "\" title=\"Assignació a usuari\"></i></td> \
            </tr>";

    return res;
}

function INSTANCE_RenderItem(item) {
    var res = " \
            <div class=\"col col-xs-3 itemDiv\" onclick=\"GoEncryptedPage('/Admin/Instance/ItemDefinition.aspx', {\'Id\':"+ item.Id+"});\"> \
                <table> \
                    <tbody> \
                        <tr style =\"height:25px;\"> \
                            <td rowspan =\"4\" style=\"padding-top:4px;width:30px;text-align:center;vertical-align:top;color:#999;\"><i class=\""+ item.Layout.Icon+" fa-15x blue\"></i></td> \
                            <td style =\"font-weight:bold;font-size:14px;vertical-align:top;\">"+ item.Layout.Label+"</td> \
                        </tr > \
                        <tr><td>"+ item.Id + " - "+ item.ItemName+"</td></tr> \
                        <tr style=\"height:25px;\"> \
                            <td> \
                                <i class=\"fa fa-star light-grey\" title=\"No permet seguiment\"></i>&nbsp; \
                                <i class=\"fa fa-question-circle light-grey\" title=\"FAQs desctivadas\"></i>&nbsp; \
                                <i class=\"fa fa-clip light-grey\" title=\"Sense adjunts\"></i>&nbsp; \
                                <i class=\"fa fa-eye light-grey\" title=\"No disponible per a àmbits\"></i>&nbsp; \
                                <i class=\"fa fa-map-marker-alt light-grey\" title=\"No geolocalizable\"></i> &nbsp; \
                                <i class=\"fa fa-list-ol light-grey\" title=\"Traces desctivades\"></i> \&nbsp; \
                                <i class=\"fa fa-tag light-grey\" title=\"Tags desactivats\"></i> \
                            </td> \
                        </tr> \
                        <tr> \
                            <td style=\"vertical-align:top;font-size:12px;\"><i>"+ item.Description+"</i></td> \
                        </tr> \
                    </tbody> \
                </table> \
            </div>";

    return res;
}

function INSTANCE_RenderFixedLists() {
    var lists = Object.keys(FixedLists);
    var res = "";
    for (var x = 0; x < lists.length; x++) {
        res += "<div class=\"col-xs-6\">";
        res += "<strong>" + lists[x] + ":</strong>&nbsp;<ul style=\"padding-left:24px;\">";
        var list = FixedLists[lists[x]];
        for (var y = 0; y < list.length; y++) {
            res += "<li><i style=\"color:#777;\">(" + y +")</i>&nbsp;"  + list[y] + "</li>";
        }

        res += "</ul></div>";
    }

    $("#FixedListsDiv").html(res);
}