var actualFKItem = "";
var FKLoaded = 0;

function GetFKApplicationUsers(callback) {
    var data = {
        "companyId": Company.Id,
        "instanceName": Instance.Name
    };
    $.ajax({
        "type": "POST",
        "url": "/Async/ItemService.asmx/GetFKApplicationUsers",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {            
            var result = null;
            eval("result = " + msg.d.split('\n').join('').split('\r').join("<br />") + ";");
            FK["ApplicationUser"] = { "Data": result, "Token": guid() };

            if (typeof callback !== "undefined" && callback !== null) {
                callback(result);
            }


            var res = "<option value=\"-1\" selected=\"selected\">Seleccionar</option>";
            for (var x = 0; x < FK.ApplicationUser.Data.length; x++) {
                var user = FK.ApplicationUser.Data[x];
                res += "<option value=\"" + user.Id + "\">" + user.Value + "</option>";
            }

            $(".CmbAppplicationUsers").html(res);
        },
        "error": function (msg) {
            console.log(msg.responseText);
        }
    });
}

function GetFKItem(itemName, callback) {
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
            if (typeof ItemDefinition.ForeignValues !== "undefined") {
                if (FKLoaded === ItemDefinition.ForeignValues.length) {
                    GetItemDataJson(ItemDefinition.ItemName, ItemId, FillFormItemFromJson);
                }
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
        if (fkValues[o].Id === selectedId || fkValues[o].Active === true) {
            options += "<option value=\"" + fkValues[o].Id + "\">" + fkValues[o].Description + "</option>";
        }
    }

    $("#" + comboId).html(options);

    if (typeof selectedId !== "undefined") {
        $("#" + comboId).val(selectedId);
    }
}

function GetFKById(itemName, id) {
    var res = null;

    if (typeof FK[itemName] !== "undefined") {
        var data = FK[itemName].Data.filter(function (el) {
            return el.Id === id;
        });

        res = data[0];
    }

    return res;
}

function UpdateFK(itemName, id, description) {
    if (typeof FK[itemName] !== "undefined") {
        for (var x = 0; x < FK[itemName].Data.length; x++) {
            if (FK[itemName].Data[x].Id === id) {
                FK[itemName].Data[x].Description = description;
                break;
            }
        }
    }
}