var ItemDefinitions = LocalStorageGetJson("ItemDefinitions");

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
}

function INSTANCE_RenderItems() {
    var res = "";

    for (var x = 0; x < ItemDefinitions.length; x++) {
        res += INSTANCE_RenderItem(ItemDefinitions[x]);
    }


    $("#ItemsDiv").html(res);
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
        res += "<strong>" + lists[x] + ":</strong>&nbsp;<ul style=\"padding-left:24px;\">";
        var list = FixedLists[lists[x]];
        for (var y = 0; y < list.length; y++) {
            res += "<li><i style=\"color:#777;\">(" + y +")</i>&nbsp;"  + list[y] + "</li>";
        }

        res += "</ul><hr>";
    }

    $("#FixedListsDiv").html(res);
}