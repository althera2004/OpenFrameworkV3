﻿window.onload = function () {
    $("#nav-search").remove();
    $("#ItemDefinitionId").html(ItemDefinition.Id);
    $("#ItemDefinitionName").html(ItemDefinition.ItemName);
    $("#BreadCrumbLabel").html("Ìtem: " + ItemDefinition.Id + " - " + ItemDefinition.ItemName);
    ITEMDEFINITION_RenderFields();
    ITEMDEFINTNITION_CreateSQL();
}

function ITEMDEFINITION_RenderFields() {
    var res = "";
    for (var f = 0; f < ItemDefinition.Fields.length; f++) {
        res += ITEMDEFINITION_RenderField(ItemDefinition.Fields[f]);
    }

    $("#ListFieldsTable").append(res);
    $("#ListFieldsTotal").html(ItemDefinition.Fields.length);
}

function ITEMDEFINITION_RenderField(field) {
    if (field.Name === "Id") {
        return "<tr style=\"background-color:#f0f2f9;\"><td style=\"width:200px;font-weight:bold;\">Id</td><td colspan=\"7\"><i>Camp clau de l'ìtem. No es pot modificar</i></td></tr>";
    }

    var length = "";
    if (typeof field.Length !== "undefined") { length = field.Length; }
    else if (field.Type.toLowerCase() === "decimal") {
        if (HasPropertyValue(field.Precission)) {
            length = field.Precission;
        } else {
            length = 3;
        }

        length += " decimals";
    }

    var res = "";
    res += "<tr>";
    res += "  <td style=\"width:200px;\">" + field.Name + "</td>";
    res += "  <td>" + field.Label + "</td>";
    res += "  <td style=\"width:150px;\">" + ITEMDEFINTION_FielTypeText(field.Type) + "</td>";
    res += "  <td style=\"width:50px;text-align:center;\">" + (field.Required ? "<i class=\"fa fa-check\"></i>" : "") + "</td>";
    res += "  <td style=\"width:90px;text-align:right;\">" + length + "</td>";
    res += "  <td style=\"width:50px;text-align:center;\">" + (field.FK ? "<i class=\"fa fa-check\"></i>" : "") + "</td>";
    res += "  <td style=\"width:150px;\"></td>";
    res += "  <td style=\"width:90px;\">&nbsp;</td>";
    res += "</tr>";
    return res;
}

function ITEMDEFINTION_FielTypeText(fieldType) {
    var res = fieldType;

    switch (fieldType.toLowerCase()) {
        case "text": res = "Text"; break;
        case "textarea": res = "Text llarg"; break;
        case "url": res = "Url"; break;
        case "email": res = "Email"; break;
        case "fixedlist": res = "Llista fixa"; break;
        case "boolean": res = "Sí/No"; break;
        case "int": res = "Nombre sencer"; break;
        case "long": res = "Nombre llarg"; break;
        case "decimal": res = "Nombre decimal"; break;
        case "money": res = "Moneda"; break;
        case "datetime": res = "Data"; break;
        case "documentfile": res = "Document"; break;
        case "imagefile": res = "Imatge"; break;
        case "applicationuser": res = "Usuari"; break;
        default: res = res + "*"; break;
    }

    return res;
}

function ITEMDEFINTNITION_CreateSQL() {
    var res = "CREATE TABLE Item_" + ItemDefinition.ItemName + "\n";
    res += " (\n";
    res += "  [Id][bigint] IDENTITY(1, 1) NOT NULL,\n";
    res += "  [CompanyId][bigint] NOT NULL,\n";
    res += "  -- Item fields\n";

    for (var f = 0; f < ItemDefinition.Fields.length; f++) {
        if (ItemDefinition.Fields[f].Name === "Id") {
            continue;
        }

        var field = ItemDefinition.Fields[f];

        res += "    [" + field.Name+ "] ";

        switch (ItemDefinition.Fields[f].Type.toLowerCase()) {
            case "fixedlist": res += "int"; break;
            case "int": res += " int"; break;
            case "long": res += " bigint"; break;
            case "applicationuser": res += " bigint"; break;
            case "boolean": res += " bit"; break;
            case "textarea": res += " nvarchar(2000)"; break;
            case "email": res += " nvarchar(150)"; break;
            case "url": res += " nvarchar(150)"; break;
            case "documentfile": res += " nvarchar(50)"; break;
            case "datetime": res += " datetime"; break;
            case "decimal":
                res += "decimal";
                if (HasPropertyValue(field.Precission)) {
                    length = res+= "(18," + field.Precission + ")";
                } else {
                    length = res += "(18,3)";
                }

                break;
            case "money": res += "decimal(18,3)"; break;

            case "text": res += "nvarchar(" + ItemDefinition.Fields[f].Length + ")"; break;

            default: res += "*" + ItemDefinition.Fields[f].Type + "*"; break;
        }

        if (ItemDefinition.Fields[f].Required === true) { res += " NOT NULL"; }

        res += ",\n";
    }

    res += "  -- End item fields\n";
    res += "  [CreatedBy][bigint] NOT NULL,\n";
    res += "  [CreatedOn][datetime] NOT NULL,\n";
    res += "  [ModifiedBy][bigint]  NOT NULL,\n";
    res += "  [ModifiedOn][datetime] NOT NULL,\n";
    res += "  [Active][bit] NOT NULL,\n";
    res += "  CONSTRAINT[PK_Item_" + ItemDefinition.ItemName + "] PRIMARY KEY CLUSTERED\n";
    res += "  ( [Id] ASC )\n";
    res += "  WITH\n";
    res += "  (\n";
    res += "    PAD_INDEX = OFF,\n";
    res += "    STATISTICS_NORECOMPUTE = OFF,\n";
    res += "    IGNORE_DUP_KEY = OFF,\n";
    res += "    ALLOW_ROW_LOCKS = ON,\n";
    res += "    ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]\n";
    res += "  ) ON[PRIMARY]";

    $("#ItemSQLCreate").html(res);
}