function Core_ReloadInstance() {
    var data = {
        "instanceName": Instance.Name
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/InstanceService.asmx/ReloadInstance",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            var result = response.d;

            if (result.Success === true) {
                localStorage.setItem('Instance', result.ReturnValue);
            }
        }
    });
}

function Core_ReloadDefinitions() {
    var data = {
        "instanceName": Instance.Name
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/InstanceService.asmx/ReloadDefinitions",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            var result = response.d;

            if (result.Success === true) {
                localStorage.setItem('ItemDefinitions', result.ReturnValue);
            }
        }
    });
}

function Core_ReloadPersistenceScripts() {
    var data = {
        "instanceName": Instance.Name
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/ItemService.asmx/CreatePersistenceScripts",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            var result = response.d;
            console.log(result);
        }
    });
}

function Core_ReloadMenu() {
    var data = {
        "applicationUserId": ApplicationUser.Id,
        "companyId": Company.Id,
        "instanceName": Instance.Name
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/InstanceService.asmx/ReloadMenu",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            var result = response.d;

            if (result.Success === true) {
                localStorage.setItem('Menu', result.ReturnValue);
                RenderMenu();
            }
        }
    });
}

function Core_ReloadDictionary() {
    var data = {
        "instanceName": Instance.Name
    };

    $.ajax({
        "type": "POST",
        "url": "/Async/InstanceService.asmx/ReloadDictionary",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (response) {
            var result = response.d;

            if (result.Success === true) {
                localStorage.setItem('Dictionary', result.ReturnValue);
                RenderMenu();
            }
        }
    });
}