var listToSearch = null;

function SearchListChange() {
    if(debugConsole === true) { console.log("SearchListChange"); }
    SearchList();
}

function SearchListKeyUp() {
    SearchList();
}

function SearchList() {
    if (listToSearch === null) {
        listToSearch = ListSources[0];
    }

    for (var x = 0; x < ListSources.length; x++) {
        if (listToSearch.ListId === ListSources[x].ListDefinition.Id && listToSearch.ItemName === ListSources[x].ItemDefinition.ItemName) {
            listToSearch = ListSources[x];
            break;
        }
    }

    switch (PageType) {
        case PageType.Invoices:
            ListInvoicesSearch(searchTarget);
            break;
        case PageType.Alerts:
            ListAlertsSearch();
            break;
        default:
            if (listToSearch !== null) {
                /*if (list.Data.length > 10 && $("#nav-search-input").val().length < 3) {
                    console.log("3 chars");
                    return false;
                }*/

                if (ListSearch() === true) {
                    ListSources[x].PageIndex = 0;
                    ListSources[x].FillData();
                }
            }
            break;
    }
}

function filterFast(a, pattern, filter, extraFilter) {
    var match = [];

    if (typeof extraFilter !== "undefined" && extraFilter !== null && extraFilter.length > 0) {
        for (var ef = 0; ef < extraFilter.length; ef++) {
            var filterc = extraFilter[ef];
            a = a.filter(function (item) {
                if (filterc.Value === "ISNULL") {
                    return item[filterc.Field] === null;
                }
                else if(filterc.Value === "NOTNULL") {
                    return item[filterc.Field] !== null;
                }
                else if (filterc.Comparer === "BEFORE") {
                    return item[filterc.Field] < filterc.Value
                }
                else if (filterc.Comparer === "LATER") {
                    return item[filterc.Field] > filterc.Value
                }
                else if (filterc.Comparer === "BETWEEN") {
                    var dateValue = GetDate(item[filterc.Field], "/", false);
                    var res = dateValue >= filterc.Value && dateValue <= filterc.Value2;
                    return res;
                }
                else if (filterc.Comparer === "CONTAINS") {
                    return item[filterc.Field].indexOf(filterc.Value) !== -1;
                }
                else {
                    if (typeof filterc.Subfield !== "undefined") {
                        return item[filterc.Field][filterc.Subfield] === filterc.Value;
                    }
                    else {
                        return item[filterc.Field] === filterc.Value;
                    }
                }
            });
        }
    }


    for (var i = 0; i < a.length; i++) {
        var x = "";
        for (var y = 0; y < filter.length; y++) {
            var field = listToSearch.ItemDefinition.Fields.filter(function (e) { return e.Name === filter[y] });

            var valueToTest = a[i][filter[y]];

            if (valueToTest === null) { continue; }
            if (typeof valueToTest === "object") {
                x += valueToTest.Value + "|";
            }
            else if (field.length > 0) {

                if (field[0].Type.toLowerCase() === "Fixedlistmultiple") {
                    console.log("x", valueToTest);

                    var dataValue = valueToTest;
                    var listName = field[0].FixedListName;

                    var binary = (dataValue >>> 0).toString(2);
                    var test = "";
                    var first = true;
                    for (var b = binary.length - 1; b >= 0; b--) {
                        if (binary[b] === '1') {
                            test += first ? "" : ", ";
                            test += FixedLists[listName][binary.length - 1 - b];
                            first = false;
                        }
                    }

                    console.log(valueToTest, test);
                    x += test + "|";
                } else if (field[0].Type.toLowerCase() === "text") {
                    x += valueToTest + "|";
                }
            }
            else {
                x += valueToTest + "|";
            }
        }
        if (x.normalize('NFD').replace(/[\u0300-\u036f]/g, "").toLowerCase().indexOf(pattern) !== -1) {
            match.push(a[i]);
        }
    }

    return match;
}

function filterFastInvoice(pattern, searchTarget, columns) {
    var total = 0;
    var rows = $("#ListDataTable_" + searchTarget + "_Custom TR");
    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        var x = "";

        for (var c = 0; c < columns.length; c++) {
            x +=  $(row)[0].cells[columns[c]].textContent;
            x += "|";
        }
        //x += $(row)[0].cells[4].textContent;

        if (x.normalize('NFD').replace(/[\u0300-\u036f]/g, "").toLowerCase().indexOf(pattern) !== -1) {
            $(row).show();
            total++;
        }
        else {
            $(row).hide();
        }
    }

    return total;
}

function filterFastAlert(pattern) {
    var total = 0;
    var rows = $("#ListDataTable_Alerts_Custom TR");
    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        var x = $(row)[0].cells[0].textContent;
        x += "|";
        x += $(row)[0].cells[1].textContent;
        x += "|";
        x += $(row)[0].cells[3].textContent;

        if (x.normalize('NFD').replace(/[\u0300-\u036f]/g, "").toLowerCase().indexOf(pattern) !== -1) {
            $(row).show();
            total++;
        }
        else {
            $(row).hide();
        }
    }

    return total;
}

function ListInvoicesSearch(target) {
    var itemSearch = makeComp($("#nav-search-input").val());

    var columns = [2, 4, 5];
    if (searchTarget === "BillingConcept") { columns = [0, 1]; }
    if (searchTarget === "BillingConceptCategory") { columns = [0]; }
    if (searchTarget === "Receipt") { columns = [2]; }

    var total = filterFastInvoice(itemSearch, searchTarget, columns);
    var rows = $("#ListDataTable_" + target +"_Custom TR");

    if (total < rows.length) {
        $("#ListDataCount_" + target +"_Custom").html(total + " de " + rows.length);
    }
    else {
        $("#ListDataCount_" + target +"_Custom").html(total);
    }
    return true;
}

function ListAlertsSearch() {
    var itemSearch = makeComp($("#nav-search-input").val());
    var total = filterFastAlert(itemSearch);
    var rows = $("#ListDataTable_Alerts_Custom TR");

    if (total < rows.length) {
        $("#ListDataCount_Alerts_Custom").html(total + " de " + rows.length);
    }
    else {
        $("#ListDataCount_Alerts_Custom").html(total);
    }
    return true;
}

function ListSearch() {
    var itemSearch = makeComp($("#nav-search-input").val());
    var list = PageListById(listToSearch.ItemName, listToSearch.ListId);
    //for (var x = 0; x < listSources.length; x++) {
    //    if (listToSearch.ListId === listSources[x].ListId && listToSearch.ItemName === listSources[x].ItemName) {
    //        list = listSources[x];
    //        break;
    //    }
    //}

    if (list === null) {
        return false;
    }

    list.FilteredData = filterFast(list.Data, itemSearch, list.Filter, list.ExtraFilter);
    return true;
}

function foo() {
    // Evitar búsquedas repetidas
    if (LastItemSearch === itemSearch) { return false; }
    var total = list.Data.length;

    LastItemSearch = itemSearch;
    if (itemSearch === "") {
        listData = list.Data;
    }
    else {
        listData = [];
        total = list.Data.length;
        console.time("search");

        var items = list.Data;
        if (itemSearch.indexOf(LastItemSearch) !== -1 && LastItemSearch.length <= itemSearch) {
            items = list.FilteredData;
        }

        var listData = items.filter(function (item) {
            for (var data in item) {
                if ($.inArray(data, list.Filter) !== -1) {
                    var realData = item[data];
                    if (realData === null) { continue; }

                    if (typeof realData === "object") {
                        realData = realData.Value;
                    }

                    if (makeComp(realData).indexOf(itemSearch) !== -1) {
                        return true;
                    }
                }
            };

            return false;
        });

        console.timeEnd("search");
    }

    for (var l = 0; l < listSources.length; l++) {
        if (listToSearch.ListId === listSources[l].ListId && listToSearch.ItemName === listSources[l].ItemName) {
            list.FilteredData = listData;
            break;
        }
    }

    $("#" + actualTab).data("searchItem", itemSearch);
    return true;
}

function ListSearchAddList(itemName, listId, tabId) {
    ListsItemSearch.push({
        "ItemName": itemName,
        "ListId": listId,
        "TabId": tabId,
        "Items": [],
        "ColumnsIndex": []
    });
}

function ListSearchFind(itemName, listId) {
    for (var x = 0; x < ListsItemSearch.length; x++) {
        if (ListsItemSearch[x].ItemName === itemName && ListsItemSearch[x].ListId === listId) {
            return ListsItemSearch[x];
        }
    }

    return null;
}

function SearchSetupPrepare(e) {
    console.log(e.target.id);
}

function SearchSetup(id) {
    ace.vars["Search"] = [];
    var tabId = id.toLowerCase();
    if (id.length > 3 && tabId.substr(0, 3) === "tab") {
        tabId = id.substr(3);
    }
    console.log("tabid", tabId);
    for (var x = 0; x < ListsItemSearch.length; x++) {
        if (ListsItemSearch[x].TabId === tabId) {
            listToSearch = ListsItemSearch[x];
            break;
        }
    }

    if (listToSearch !== null) {
        var itemName = listToSearch.ItemName;
        var itemDefinition = GetItemDefinitionByName(itemName);
        var searchPlaceHolder = Dictionary.Common_Search + " " + itemDefinition.Layout.LabelPlural.toLowerCase();

        ace.vars["Search"] = listToSearch.Items;
        ace.vars["Search"].sort();

        $("#nav-search-input").typeahead("destroy");
        $("#nav-search-input").typeahead({
            "hint": true,
            "highlight": true,
            "accent": true,
            "minLength": 1
        },
            {
                "name": itemName,
                "displayKey": "displayValue",
                "value": "value",
                "source": substringMatcher(ace.vars["Search"]),
                "limit": 10
            });

        $(".typeahead").on("typeahead:selected", function () {
            SearchList();
        });

        $("#nav-search-input").attr("placeholder", searchPlaceHolder);
        $("#nav-search").show();
    }
}

function ListSearchAddItem(itemName, listId, itemValue) {
    var list = ListSearchFind(itemName, listId);
    if (list !== null) {
        list.Items.push(itemValue);
    }
}

function ListSearchAddColumnIndex(itemName, listId, columnIndex) {
    var list = ListSearchFind(itemName, listId);
    if (list !== null) {
        list.ColumnsIndex.push(columnIndex);
    }
}

var substringMatcher = function (strs) {
    return function findMatches(q, cb) {
        var matches, substringRegex;

        // an array that will be populated with substring matches
        matches = [];
        finalMatches = [];
        // regex used to determine if a string contains the substring `q`
        substrRegex = new RegExp(normalize(q), 'i');

        $.each(strs, function (i, str) {
            if (substrRegex.test(normalize(str))) {
                // the typeahead jQuery plugin expects suggestions to a
                // JavaScript object, refer to typeahead docs for more info
                matches.push({ "value": normalize(str), "displayValue": str });
            }
        });

        for (var x = 0; x < matches.length; x++) {
            var exists = false;
            var d = matches[x].displayValue;
            for (var f = 0; f < finalMatches.length; f++) {
                if (finalMatches[f].displayValue === d) {
                    exists = true;
                    break;
                }
            }

            if (exists === false) {
                finalMatches.push(matches[x]);
            }
        }

        cb(finalMatches);
    };
};

function makeComp(input) {
    if (input === null) { return null; }
    input = input.toLowerCase();
    var output = "";
    for (var i = 0; i < input.length; i++) {
        if (jQuery.inArray(input.charAt(i), "àáâãäåæ") !== -1)
            output = output + "a";
        else if (jQuery.inArray(input.charAt(i), "ç") !== -1)
            output = output + "c";
        else if (jQuery.inArray(input.charAt(i), "èéêëæ") !== -1)
            output = output + "e";
        else if (jQuery.inArray(input.charAt(i), "ìíîï") !== -1)
            output = output + "i";
        else if (jQuery.inArray(input.charAt(i), "òóôõöø") !== -1)
            output = output + "o";
        else if (jQuery.inArray(input.charAt(i), "ùúûü") !== -1)
            output = output + "u";
        else if (jQuery.inArray(input.charAt(i), "ñ") !== -1)
            output = output + "n";
        else if (jQuery.inArray(input.charAt(i), "ß") !== -1)
            output = output + "s";
        else
            output = output + input.charAt(i);
    }
    return output;
}