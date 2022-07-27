var CodeMirror_Context = {
    "SQL": null,
    "JavaScript": null
};

window.onload = function () {
    $("#nav-search").remove();
    $("#ItemDefinitionId").html(ItemDefinition.Id);
    $("#ItemDefinitionIcon").html("<i class=\"" + ItemDefinition.Layout.Icon + " blue\" style=\"font-size:30px;\"></i>");
    $("#ItemDefinitionName").html(ItemDefinition.ItemName);
    $("#TxtLayoutLabel").val(ItemDefinition.Layout.Label);
    $("#TxtLayoutLabelPlural").val(ItemDefinition.Layout.LabelPlural);
    $("#ItemDefinitionDescription").html(ItemDefinition.Description);
    $("#BreadCrumbLabel").html("Ìtem: " + ItemDefinition.Id + " - " + ItemDefinition.ItemName);
    ITEMDEFINITION_RenderFields();
    ITEMDEFINTNITION_CreateSQL();
    ITEMDEFINITION_RenderScript();

    $("#tabSelect-sql").on("click", ITEMDEFINITION_RenderScriptRefresh);
    $("#tabSelect-scripts").on("click", ITEMDEFINITION_RenderScriptRefresh);
}

function ITEMDEFINITION_RenderScript(sender) {

    /*CodeMirror(document.querySelector('#Editor'), {
        lineNumbers: true,
        tabSize: 2,
        value: 'console.log("Hello, World");'
    });

    CodeMirror(document.querySelector('#ItemSQLCreate'), {
        lineNumbers: true,
        tabSize: 2,
        value: sqlScript
    });*/

    /*var sqlScript = $("#ItemSQLCreate").html();
    $("#ItemSQLCreate").html("");
    var myCodeMirror = CodeMirror(document.body, {
        value: sqlScript,
        mode: "javascript"
    });*/

    if (CodeMirror_Context.SQL === null) {
        console.log("CREATE", "SQL");
        var mime = 'text/x-mariadb';
        // get mime type
        if (window.location.href.indexOf('mime=') > -1) {
            mime = window.location.href.substr(window.location.href.indexOf('mime=') + 5);
        }
        var editor = CodeMirror.fromTextArea(document.getElementById('ItemSQLCreate'), {
            mode: mime,
            indentWithTabs: true,
            smartIndent: true,
            lineNumbers: true,
            matchBrackets: true,
            autofocus: true,
            extraKeys: { "Ctrl-Space": "autocomplete" },
            hintOptions: {
                tables: {
                    users: ["name", "score", "birthDate"],
                    countries: ["name", "population", "size"]
                }
            }
        });
        CodeMirror_Context.SQL = editor;
    }

    if (CodeMirror_Context.JavaScript === null) {
        console.log("CREATE", "Javascript");
        var editor = CodeMirror.fromTextArea(document.getElementById("Editor"), {
            mode: "text/javascript",
            lineNumbers: true,
            matchBrackets: true,
            continueComments: "Enter",
            extraKeys: { "Ctrl-Q": "toggleComment" }
        });
        CodeMirror_Context.JavaScript = editor;
    }
}

function ITEMDEFINITION_RenderScriptRefresh(sender) {
    if (sender.currentTarget.id === "tabSelect-sql") {
        setTimeout(function () { CodeMirror_Context.SQL.refresh(); }, 10);
    }
    if (sender.currentTarget.id === "tabSelect-scripts") {
        setTimeout(function () { CodeMirror_Context.JavaScript.refresh(); }, 10);
    }
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

    $("#ItemSQLCreate").val(res);
}