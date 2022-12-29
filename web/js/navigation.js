var Navigation_Context = {
    "urlAfterDirty": null,
    "detectDirty": true
};

function Go(link, adminRestriction) {
    Navigation_Context.urlAfterDirty = link;
    if (DetectDirty() === true) { return false; }

    // Si PinCode no está activado no es necesario preguntar sobre adminRestriction
    if (PinCode === true) {
        if (typeof adminRestriction !== "undefined" && adminRestriction !== null && adminRestriction === true) {
            QRNewPin(link);
            return false;
        }
    }

    document.location = link;
}

function GoNew(itemName, formId, params) {
    if (codedQueryClean === true) {
        GoCleanedNew(itemName, formId, params);
    }
    else {
        GoEncryptedNew(itemName, formId, params);
    }
}

function GoCleanedNew(itemName, formId, params) {
    GoCleanedView(itemName, -1, formId, params);
}

function GoEncryptedNew(itemName, formId, params) {
    GoEncryptedView(itemName, -1, formId, params);
}

function GoItemView(itemName, id, formId) {
    if (typeof formId === "undefined" || formId === null || formId === "") {
        formId = "custom";
    }

    if (codedQueryClean === true) {
        GoCleanedView(itemName, id, formId, null);
    }
    else {
        GoEncryptedView(itemName, id, formId, null);
    }
}

function GoDashBoard() {
    var url = "/Instances/" + InstanceName + "/Pages/DashBoard.aspx";
    document.location = url;
}

function GoCleanedDocumentSign(id) {
    var query = "&companyId=" + CompanyId;
    query += "&itemDefinitionId=" + itemDefinition.Id;
    query += "&itemId=" + itemData.Id;
    query += "&fieldName=" + id;
    var url = "/Features/DocumentSign.aspx?" + query;
    Navigation_Context.urlAfterDirty = url;
    window.open(url);
}

function GoEncryptedDocumentSign(id) {
    var query = "&companyId=" + CompanyId;
    query += "&itemDefinitionId=" + itemDefinition.Id;
    query += "&itemId=" + itemData.Id;
    query += "&fieldName=" + id;
    var url = "/Features/DocumentSign.aspx?" + $.base64.encode(guid() + query);
    Navigation_Context.urlAfterDirty = url;
    window.open(url);
}

function GoHome() {
    var query = "&I=" + Instance.Name;
    query += "&C=" + Company.Id;
    query += "&L=" + Language;
    if (typeof optionId !== "undefined" && optionId !== null && optionId > 0) {
        query += "&optionId=" + optionId;
    }
    else if (typeof ActualOptionId !== "undefined" && ActualOptionId !== null && ActualOptionId > 0) {
        query += "&optionId=" + ActualOptionId;
    }

    if (typeof params !== "undefined" && params !== null) {
        for (var x = 0; x < params.length; x++) {
            query += "&" + params[x];
        }
    }

    var url = "/Instances/" + Instance.Name + "/Pages/Dashboard.aspx?" + $.base64.encode(guid() + query);
    Navigation_Context.urlAfterDirty = url;
    if (DetectDirty() === true) { return false; }
    $("#bigmenu").hide();
    document.location = url;
}

function GoEncryptedView(itemName, listId, itemId, formId, params) {
    var query = "&I=" + Instance.Name;
    query += "&C=" + Company.Id;
    query += "&Item=" + itemName;
    query += "&List=" + ListId;
    query += "&Id=" + itemId;
    query += "&F=" + formId;
    query += "&L=" + Language;
    if (typeof optionId !== "undefined" && optionId !== null && optionId > 0) {
        query += "&optionId=" + optionId;
    }
    else if (typeof ActualOptionId !== "undefined" && ActualOptionId !== null && ActualOptionId > 0) {
        query += "&optionId=" + ActualOptionId;
    }

    if (typeof params !== "undefined" && params !== null) {
        for (var x = 0; x < params.length; x++) {
            query += "&" + params[x];
        }
    }

    var url = "/ItemView.aspx?" + $.base64.encode(guid() + query);
    Navigation_Context.urlAfterDirty = url;
    if (DetectDirty() === true) { return false; }
    $("#bigmenu").hide();
    document.location = url;
}

function GoEncryptedView(itemName, listId, itemId, formId, params) {
    if (typeof formId === "undefined" || formId === null || formId === "") {
        formId = "custom";
    }

    var query = "&I=" + Instance.Name;
    query += "&C=" + Company.Id;
    query += "&Item=" + itemName;
    query += "&List=" + ListId;
    query += "&Id=" + itemId;
    query += "&F=" + formId;
    query += "&L=" + Language;
    if (typeof optionId !== "undefined" && optionId !== null && optionId > 0) {
        query += "&optionId=" + optionId;
    }
    else if (typeof ActualOptionId !== "undefined" && ActualOptionId !== null && ActualOptionId > 0) {
        query += "&optionId=" + ActualOptionId;
    }

    if (typeof params !== "undefined" && params !== null) {
        for (var x = 0; x < params.length; x++) {
            query += "&" + params[x];
        }
    }

    var url = "/ItemView.aspx?" + $.base64.encode(guid() + query);
    Navigation_Context.urlAfterDirty = url;
    if (DetectDirty() === true) { return false; }
    $("#bigmenu").hide();
    document.location = url;
}

function GoEncryptedList(itemName, listId, params) {
    if (typeof listId === "undefined" || listId === null || listId === "") {
        listId = "custom";
    }

    var query = "&I=" + Instance.Name;
    query += "&C=" + Company.Id;
    query += "&Item=" + itemName;
    query += "&List=" + listId;
    query += "&L=" + Language;

    if (typeof params !== "undefined" && params !== null) {
        for (var x = 0; x < params.length; x++) {
            query += "&" + params[x];
        }
    }

    var url = "/ItemList.aspx?" + $.base64.encode(guid() + query);
    Navigation_Context.urlAfterDirty = url;
    if (DetectDirty() === true) { return false; }
    $("#bigmenu").hide();
    document.location = url;
}

function GoEncryptedImport(itemName) {
    var query = "&itemTypeId=" + itemName;
    var url = "/ImportItem.aspx?" + $.base64.encode(guid() + query);
    Navigation_Context.urlAfterDirty = url;
    if (DetectDirty() === true) { return false; }
    document.location = url;
}

function GoCleanedPage(url, params) {
    var keys = Object.keys(params);
    var query = "";
    for (var k = 0; k < keys.length; k++) {
        query += "&" + keys[k] + "=" + params[keys[k]];
    }

    url += "?z=1" + query;
    Navigation_Context.urlAfterDirty = url;
    if (DetectDirty() === true) { return false; }
    document.location = url;
}

function GoEncryptedPage(url, params) {
    var query = "&I=" + Instance.Name;
    query += "&C=" + Company.Id;
    query += "&L=" + Language;

    if (typeof params !== "undefined" && params !== null) {
        var keys = Object.keys(params);
        for (var k = 0; k < keys.length; k++) {
            query += "&" + keys[k] + "=" + params[keys[k]];
        }
    }

    url += "?" + $.base64.encode(guid() + query);
    Navigation_Context.urlAfterDirty = url;
    if (DetectDirty() === true) { return false; }

    $("#bigmenu").hide();
    document.location = url;
}

function GoPageBlank(url, params) {
    if (codedQueryClean === true) {
        GoCleanedPageBlank(url, params);
    }
    else {
        GoEncryptedPageBlank(url, parmas);
    }
}

function GoCleanedPageBlank(url, params) {
    var query = "";
    if (typeof params !== "undefined" && params !== null) {
        var keys = Object.keys(params);
        for (var k = 0; k < keys.length; k++) {
            query += "&" + keys[k] + "=" + params[keys[k]];
        }
    }

    url += "?pb=1" + query;
    if (Navigation_Context.detectDirty !== true) {

        // Si es de urgencia no hay control dirty
        if (HasPropertyEnabled(Navigation_Context.Urgent)) {
            // Se desactiva contexto de urgencia para la próxima llamada
            Navigation_Context.Urgent = false;
        }
        else {
            Navigation_Context.detectDirty = false;
            if (DetectDirty() === true) { return false; }
        }
    }

    window.open(url, "custom");
}

function GoEncryptedPageBlank(url, params) {
    var query = "";
    if (typeof params !== "undefined" && params !== null) {
        var keys = Object.keys(params);
        for (var k = 0; k < keys.length; k++) {
            query += "&" + keys[k] + "=" + params[keys[k]];
        }
    }

    url += "?" + $.base64.encode(guid() + query);
    //Navigation_Context.urlAfterDirty = url;
    if (Navigation_Context.detectDirty !== true) {

        // Si es de urgencia no hay control dirty
        if (HasPropertyEnabled(Navigation_Context.Urgent)) {
            // Se desactiva contexto de urgencia para la próxima llamada
            Navigation_Context.Urgent = false;
        }
        else {
            Navigation_Context.detectDirty = false;
            if (DetectDirty() === true) { return false; }
        }
    }

    $("#bigmenu").hide();
    window.open(url, "custom");
}

function GoEncryptedImportAction(itemName, fileName) {
    var query = "&itemTypeId=" + itemName;
    query += "&filename=" + fileName;
    var url = "ImportItemGo.aspx?" + $.base64.encode(guid() + query);
    Navigation_Context.urlAfterDirty = url;
    if (DetectDirty() === true) { return false; }
    document.location = url;
}

function RefreshPage() {
    var url = document.location.toString();
    if (url.indexOf("#") !== -1) {
        url = url.split('#')[0];
    }

    Navigation_Context.urlAfterDirty = url;
    if (DetectDirty() === true) { return false; }
    document.location = url;
}

function GoReferrer() {
    Navigation_Context.urlAfterDirty = referrer;
    if (DetectDirty(referrer) === true) { return false; }
    document.location = referrer;
}

function GoUserNew() {
    GoUserView(-1);
}

function GoUserView(userId) {
    var query = "&I=" + Instance.Name;
    query += "&C=" + Company.Id;
    query += "&U=" + userId;
    query += "&L=" + Language;
    url += "?" + $.base64.encode(guid() + query);
    var url = "/Admin/Security/User.aspx?" + $.base64.encode(guid() + query);
    document.location = url;
}

function GoGroupNew() {
    GoGroupView(-1);
}

function GoGroupView(userId) {
    var query = "&I=" + Instance.Name;
    query += "&C=" + Company.Id;
    query += "&G=" + userId;
    query += "&L=" + Language;
    url += "?" + $.base64.encode(guid() + query);
    var url = "/Admin/Security/Group.aspx?" + $.base64.encode(guid() + query);
    document.location = url;
}

function GoSecurityGroupNew() {
    GoSecurityGroupView(-1);
}

function GoSecurityGroupView(securityGroupId) {
    var query = "&id=" + securityGroupId;
    var url = "/Admin/SecurityGroupView.aspx?" + $.base64.encode(guid() + query);
    document.location = url;
}

function GoFAQView(faqId) {
    var query = "&id=" + faqId;
    var url = "/Features/FAQsView.aspx?" + $.base64.encode(guid() + query);
    document.location = url;
}

function GoNewsView(faqId) {
    var query = "&id=" + faqId;
    var url = "/Features/NewsView.aspx?" + $.base64.encode(guid() + query);
    document.location = url;
}

function GoAfterDirty() {
    if (Navigation_Context.urlAfterDirty !== null) {
        document.location = Navigation_Context.urlAfterDirty;
    }
}

function DetectDirty() {
    if (PageType === "PageForm" && excludeDirty !== true) {
        if (FormDirty() === true) {
            ShowPopupDirty();
            return true;
        }
    }

    return false;
}

function GoProfilePage() {
    var query = "&I=" + Instance.Name;
    query += "&C=" + Company.Id;
    query += "&L=" + Language;

    if (typeof params !== "undefined" && params !== null) {
        var keys = Object.keys(params);
        for (var k = 0; k < keys.length; k++) {
            query += "&" + keys[k] + "=" + params[keys[k]];
        }
    }

    var url = "/Admin/Profile.aspx" + "?" + $.base64.encode(guid() + query);
    Navigation_Context.urlAfterDirty = url;
    if (DetectDirty() === true) { return false; }
    document.location = url;
}