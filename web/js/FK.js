var actualFKItem = "";
var FKLoaded = 0;

function GetFKItem(itemName, callback) {
    // console.log("GetFKItem", itemName);
    actualFKItem = itemName;
    var data = {
        "itemName": itemName,
        "companyId": Company.Id,
        "instanceName": Instance.Name,
        "token": guid()
    };

    FK[actualFKItem] = { "Data": [], "Token": data.token };

    $.ajax({
        "type": "POST",
        "url": "/Async/ItemService.asmx/GetFK",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            //console.log(msg);
            var result = null;
            eval("result = " + msg.d.split('\n').join('').split('\r').join("<br />") + ";");

            if (typeof FK[result.ItemName] !== "undefined" && FK[result.ItemName] !== null) {
                FK[result.ItemName].Data = result.Data;
            }
            else {
                for (fk in FK) {
                    if (typeof FK[fk].Token !== "undefined") {
                        if (FK[fk].Token === result.Token) {
                            FK[fk].Data = result.Data;
                            break;
                        }
                    }
                }
            }

            if (typeof callback !== "undefined" && callback !== null) {
                callback(result);
            }

            FKLoaded++;
            if (FKLoaded === ItemDefinition.ForeignValues.length) {
                GetItemDataJson(ItemDefinition.ItemName, ItemId, FillFormItemFromJson);
            }
        },
        "error": function (msg) {
            console.log(msg.responseText);
        }
    });
}

function FillComboFromFK(comboId, fkItemName, selectedId) {
    var fkValues = FK[fkItemName].Data;

    var options = "<option value=\"-1\">" + Dictionary.Common_SelectOne + "</option>";
    for (var o = 0; o < fkValues.length; o++) {
        options += "<option value=\"" + fkValues[o].Id + "\">" + fkValues[o].Description + "</option>";
    }

    $("#" + comboId).html(options);

    if (typeof selectedId !== "undefined") {
        $("#" + comboId).val(selectedId);
    }
}