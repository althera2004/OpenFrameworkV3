var ItemDataJson_Context = {
	"ItemDefinition": null,
	"ItemId": null,
	"GetTraceCallBack": null
};

function Item(config) {
	var _this = this;
	if (typeof config !== "undefined") {
		var normalizedData = NormalizeData(config.OriginalItemData);
		this.OriginalItemData = normalizedData;
		this.ItemDefinition = config.ItemDefinition;
		this.ActualData = { ...normalizedData };
	}

	this.Differences = function () {
		var res = [];

		function changes(object, base) {
			return _.transform(object, function (result, value, key) {
				if (!_.isEqual(value, base[key])) {
					result[key] = (_.isObject(value) && _.isObject(base[key])) ? changes(value, base[key]) : value;
				}
			});
		}
		var dif = changes(this.OriginalItemData, this.ActualData);

		var keys = Object.keys(dif);
		for (var d = 0; d < keys.length; d++) {
			res[keys[d]] = { "original": this.OriginalItemData[keys[d]], "Actual": this.ActualData[keys[d]] };
		}

		return res;
	}

	this.ItemDifferencesToSave = function () {
		var res = [];

		function changes(object, base) {
			return _.transform(object, function (result, value, key) {
				if (!_.isEqual(value, base[key])) {
					result[key] = (_.isObject(value) && _.isObject(base[key])) ? changes(value, base[key]) : value;
				}
			});
		}
		var dif = changes(this.OriginalItemData, this.ActualData);

		var keys = Object.keys(dif);
		for (var d = 0; d < keys.length; d++) {
			res.push({ "Field": keys[d], "Original": this.OriginalItemData[keys[d]], "Actual": this.ActualData[keys[d]] });
		}

		return res;
	}

	this.IsDirty = function () {
		return !_.isEqual(this.OriginalItemData, this.ActualData);
	}

	this.Refresh = function () {
		this.OriginalItemData = Object.assign({}, this.ActualData);
	}

	this.UpdateData = function (field, value) {
		if (typeof this.ActualData === "undefined") {
			return;
		}

		this.ActualData[field] = value;

		if (typeof this.OriginalItemData[field] === "undefined") {
			this.OriginalItemData[field] = null;
        }

		if (this.IsDirty()) {
			$("#FormBtnSave").enable();
			$("#FormBtnSave").removeAttr("title");
		} else {
			$("#FormBtnSave").disable();
			$("#FormBtnSave").attr("title", "No hay cambios por guardar");
        }
	}

	this.UpdateDataMassive = function (data) {
		var keys = Object.keys(data);
		for (var d = 0; d < keys.length; d++) {
			this.UpdateData([keys[d]], data[keys[d]]);
		}
	}

	this.Delete = function () {
		PopupDeleteContext.ItemDefinition = ItemDefinition;
		PopupDeleteContext.ItemId = this.OriginalItemData.Id;
		PopupDeleteContext.Message = this.GetDescription();
		PopupRenderDelete();
		$("#LauncherPopupDelete").click();
    }

	this.Save = function () {
		if (_.isEqual(this.OriginalItemData, this.ActualData)) {
			alert("iguales");
			return false;
		}
		else {
			var customSaveFunction = this.ItemDefinition.ItemName.toUpperCase() + "_CustomSave";
			var customSaveType = eval("typeof " + customSaveFunction);
			if (customSaveType !== "undefined") {
				eval(customSaveFunction + "();");
			} else {
				var differences = this.ItemDifferencesToSave();
				var data = {
					"itemDefinitionId": ItemDefinition.Id,
					"itemId": this.OriginalItemData.Id,
					"itemData": JSON.stringify(differences, null, 2),
					"applicationUserId": ApplicationUser.Id,
					"instanceName": Instance.Name,
					"companyId": Company.Id
				};

				console.log(data);

				$.ajax({
					"type": "POST",
					"url": "/Async/ItemService.asmx/Save",
					"contentType": "application/json; charset=utf-8",
					"dataType": "json",
					"data": JSON.stringify(data, null, 2),
					"success": function (msg) {
						if (msg.d.Success === true) {
							NotifySaveSuccess(msg.d.ReturnValue);

							var actionType = msg.d.ReturnValue.split('|')[0];
							if (actionType === "INSERT") {
								var newId = msg.d.ReturnValue.split('|')[1] * 1;
								ItemData.ActualData.Id = newId;
                            }

							ItemData.OriginalItemData = { ...ItemData.ActualData };
							$("#BreadCrumbLabel").html(ItemData.GetDescription());
							$("#FormBtnSave").disable();
							$("#FormBtnSave").attr("title", "No hay cambios por guardar");

							// Actualizar footer status
							$("#FooterStatusModifiedBy").html(ApplicationUser.Profile.FullName);
							$("#FooterStatusModifiedOn").html(TodayText());
						}
						else {
							NotifySaveError(msg.d.MessageError);
                        }
					},
					"error": function (msg) {
						NotifySaveError(msg.responseText);
					}
				});
			}
		}
	}

	this.FieldDefinition = function (fieldName) {
		for (var x = 0; x < this.ItemDefinition.Fields.length; x++) {
			if (this.ItemDefinition.Fields[x].Name === fieldName) {
				return this.ItemDefinition.Fields[x];
			}
		}

		return null;
	}

	this.FieldIsFK = function (fieldName) {
		var res = false;
		var field = this.FieldDefinition(fieldName);
		if (field !== null) {
			if (typeof field.IsFK !== "undefined" && field.IsFK !== null) {
				res = field.IsFK;
			}
		}

		return res;
	}

	this.FieldsInForm = function (formId) {
		var form = this.ItemDefinition.Forms.filter(function (form) {
			return form.Id === formId;
		})[0];
		var res = [];

		if (typeof form !== "undefined" && form !== null) {
			for (var t = 0; t < form.Tabs.length; t++) {
				for (var r = 0; r < form.Tabs[t].Rows.length; r++) {
					var row = form.Tabs[t].Rows[r];
					if (typeof row.Fields !== "undefined") {
						for (var f = 0; f < row.Fields.length; f++) {
							res.push(row.Fields[f]);
						}
					}
				}
			}
		}

		return res;
	}

	this.GetValue = function (fieldName) {
		var res = null;
		if (typeof this.ActualData[fieldName] !== "undefined") {
			res = this.ActualData[fieldName];
		}

		return res;
	}

	this.GetDescription = function () {

		if (typeof ItemData.OriginalItemData === "undefined") {
			return "Nou";
        }

		var res = "";
		if (typeof this.ItemDefinition.Layout.Description && this.ItemDefinition.Layout.Description !== null) {
			var pattern = this.ItemDefinition.Layout.Description.Pattern;
			var values = [];
			for (var v = 0; v < this.ItemDefinition.Layout.Description.Fields.length; v++) {
				var fieldPattern = this.ItemDefinition.Layout.Description.Fields[v];


				if (fieldPattern !== null) {
					var field = this.FieldDefinition(fieldPattern.Name);
					switch (field.Type.toUpperCase()) {
						case "MONEY":
							values.push("${ToMoneyFormat(this.ActualData." + field.Name + ")}");
							break;
						default:
							values.push("${this.ActualData." +field.Name +"}");
							break;
					}
				}

			}

			//console.log(pattern.format(values));

			res = eval("`" + pattern.format(values) + "`");
		}

		return res;
	}
}

function GetFieldDefinition(fieldName, itemDefinition) {
    for (var x = 0; x < itemDefinition.Fields.length; x++) {
        if (itemDefinition.Fields[x].Name === fieldName) {
            return itemDefinition.Fields[x];
        }
    }

    return null;
}

function IsFK(itemDefinition, fieldName) {
	if (itemDefinition !== null && typeof itemDefinition !== "undefined") {
		if (fieldName !== "" && typeof itemDefinition.ForeignValues !== "undefined") {
			if (HasPropertyValue(itemDefinition.ForeignValues) === true) {
				for (var fv = 0; fv < itemDefinition.ForeignValues.length; fv++) {
					if (itemDefinition.ForeignValues[fv].ItemName + "Id" === fieldName) {
						return true;
                    }
                }
            }
        }
    }


    return false;
}

function ItemDifferences(original, actual) {
	var res = [];

	function changes(object, base) {
		return _.transform(object, function (result, value, key) {
			if (!_.isEqual(value, base[key])) {
				result[key] = (_.isObject(value) && _.isObject(base[key])) ? changes(value, base[key]) : value;
			}
		});
	}
	var dif = changes(original, actual);

	var keys = Object.keys(dif);
	for (var d = 0; d < keys.length; d++) {
		res[keys[d]] = { "original": original[keys[d]], "Actual": actual[keys[d]] };
	}

	return res;
}



$(".formData").on("change", UpdateFormData);

function UpdateFormData(e) {
	itemData.UpdateData(e.target.id, e.target.value);
}

String.prototype.format = function () {
	var s = this,
		i = arguments[0].length;

	while (i--) {
		s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[0][i]);
	}
	return s;
};

function ItemDefinitionById(id) {
	var result = ItemDefinitions.filter(function (v) {
		return v.Id === id * 1;
	})

	if (result.length > 0) {
		return result[0];
	}

	return null;
}

function ItemDefinitionByName(name) {
	var result = ItemDefinitions.filter(function (v) {
		return v.ItemName === name;
	})

	if (result.length > 0) {
		return result[0];
	}

	return null;
}

function ItemFormById(itemDefinition, id) {
	if (typeof itemDefinition.Forms !== "undefined") {
		var result = itemDefinition.Forms.filter(function (v) {
			return v.Id.toLowerCase() === id.toLowerCase();
		})

		if (result.length > 0) {
			return result[0];
		}
	}

	return null;
}

function ItemListById(itemDefinition, id) {
	if (typeof itemDefinition.Lists !== "undefined") {
		var result = itemDefinition.Lists.filter(function (v) {
			return v.Id.toLowerCase() === id.toLowerCase();
		})

		if (result.length > 0) {
			return result[0];
		}
	}
	else {
		return DefaultListDefinition(itemDefinition);
    }

	return null;
}

function FieldByName(itemDefinition, name) {
	if (typeof itemDefinition.Fields !== "undefined") {
		var result = itemDefinition.Fields.filter(function (v) {
			return v.Name === name;
		})

		if (result.length > 0) {
			return result[0];
		}
	}

	return null;
}

function GetItemDataJson(itemName, itemId, callBack) {
	//console.log(FK.length);
	if (typeof itemName === "undefined" || itemName === null || itemName === "") {
		itemName = ItemDataJson_Context.ItemDefinition.ItemName;
	}

	if (typeof itemId === "undefined" || itemId === null) {
		itemId = ItemDataJson_Context.ItemId;
	}

	if (itemId > 0) {
		var data = {
			"itemName": itemName,
			"itemId": itemId,
			"instanceName": Instance.Name
		};

		$.ajax({
			"type": "POST",
			"url": "/Async/ItemService.asmx/ItemById",
			"contentType": "application/json; charset=utf-8",
			"dataType": "json",
			"data": JSON.stringify(data, null, 2),
			"success": function (msg) {
				eval("ItemDataJson_Context.Data = " + msg.d + ";");

				if (typeof callBack === "function") {
					callBack(msg.d);
				}
				else if (typeof ItemDataJson_Context.Callback === "function") {
					window.ItemDataJson_Context.Callback();
				}

				ItemDataLoaded = true;
			},
			"error": function (msg) {
				PopupWarning(msg.responseText);
			}
		});
	}
	else {
		RenderBreadCrumb();

		ItemData.OriginalItemData = { "Id": -1 };
		ItemData.ActualData = { "Id": -1 };

		var FKNames = Object.keys(FK);
		for (var x = 0; x < FKNames.length; x++) {
			FillComboFromFK(FKNames[x] + "Id", FKNames[x], -1);
		}


		$("#FooterStatusModifiedBy").html(ApplicationUser.Profile.FullName);
		$("#FooterStatusModifiedOn").html(TodayText());
		PageLoadingHIde();
    }
}

function ItemFromJson(data) {

	var res = null;
	eval("res = " + data + ";");
	console.log(res);

	// Transform Zulu text to Date
	for (var f = 0; f < ItemDefinition.Fields.length; f++) {
		var field = ItemDefinition.Fields[f];
		if (field.Type === "datetime") {
			if (typeof res[field.Name] !== "undefined" && res[field.Name] !== null && res[field.Name] !== "") {
				res[field.Name] = new Date(res[field.Name]);
            }
        }
    }

	ItemData = new Item({ "OriginalItemData": res, "ItemDefinition": ItemDefinition });
}

function FillFormItemFromJson(data) {
	ItemFromJson(data);
	Form.Fill(ItemData.OriginalItemData);
	RenderBreadCrumb();
	PageLoadingHIde();
	if (ItemData.OriginalItemData.Id > 0) {
		$("#FooterStatusModifiedBy").html(ItemData.OriginalItemData.ModifiedBy);
		$("#FooterStatusModifiedOn").html(ItemData.OriginalItemData.ModifiedOn);
	}
	else {
		$("#FooterStatusModifiedBy").html(ItemData.OriginalItemData.ModifiedBy);
		$("#FooterStatusModifiedOn").html(TodayText());
    }

	console.log("Listas", ListSources.length);
	for (var l = 0; l < ListSources.length; l++) {
		ListSources[l].GetData();
    }
}

function ItemUpdateDataFormRadio(sender) {
	var id = sender.target.id.split('_')[0];
	var value = null;

	if (sender.target.id.split('_')[1] === "No") {
		if ($("#" + sender.target.id).prop("checked") === true) {
			value = false;
        }
	}

	if (sender.target.id.split('_')[1] === "Yes") {
		if ($("#" + sender.target.id).prop("checked") === true) {
			value = true;
		}
	}

	ItemData.UpdateData(id, value);
}

function ItemUpdateData(sender) {
	var id = "";

	if (HasPropertyValue(sender.currentTarget)) {
		id = sender.currentTarget.id;
	}
	else if (HasPropertyValue(sender.id)) {
		id = sender.id;
	}
	else {
		id = sender;
    }

	var value = $("#" + id).val();
	var field = FieldByName(ItemDefinition, id);
	var dataType = field.Type;

	switch (dataType.toLowerCase()) {
		case "long":
		case "int":
		case "range":
		case "fixedlist":
			value = value * 1;
			break;
		case "datetime":
			value = GetDate(value, "/", true);
			break;
		case "boolean":
			value = $("#" + id).prop("checked") === true;
			break;
		default:
			value = value.trim();
			break;
    }

	console.log(id, value);
	ItemData.UpdateData(id, value);
}

function GetTrace(itemId, itemName, instanceName, callBack) {

	if (typeof callBack !== "undefined") {
		ItemDataJson_Context.GetTraceCallBack = callBack;
	} else {
		ItemDataJson_Context.GetTraceCallBack = null;
    }

	var data = {
		"itemId": itemId,
		"itemName": itemName,
		"instanceName": instanceName
	};

	$.ajax({
		"type": "POST",
		"url": "/Async/ItemService.asmx/GetTraces",
		"contentType": "application/json; charset=utf-8",
		"dataType": "json",
		"data": JSON.stringify(data, null, 2),
		"success": function (msg) {
			if (msg.d.Success === true) {
					var traceData = null;

					eval("traceData = " + msg.d.ReturnValue + ";");
					console.log(traceData);
				if (ItemDataJson_Context.GetTraceCallBack !== null) {

					console.log("CallBack", ItemDataJson_Context.GetTraceCallBack)
				} else {
					PopupTracesRender(traceData);
                }
			}
			else {
				NotifySaveError(msg.d.MessageError);
			}
		},
		"error": function (msg) {
			NotifySaveError(msg.responseText);
		}
	});
}

function PopupTracesRender(data) {
	var res = "";
	res += "<div class=\"row\">";

	var lastDate = "";

	var existsMessage = false;

	if (data.length === 1) {
		if (HasPropertyValue(data[0].Message)) {
			existsMessage = true;
        }
    }

	if (existsMessage === false) {
		for (var x = data.length; x > 0; x--) {
			var trace = data[x - 1];

			if (typeof trace === "undefined") {
				continue;
            }

			if (lastDate !== trace.date.split(' ')[0]) {
				if (lastDate !== "") {
					res += timeLineres;
					res += "</div></div>";
				}

				var timeLineres = "";
				lastDate = trace.date.split(' ')[0];
				res += "<div class=\"timeline-container timeline-style2\">";
				res += "<span class=\"timeline-label\"><strong>" + DateHumanText(GetDate(lastDate, "/", false)) + "</strong></span>";
				res += "<div class=\"timeline-items\">";
			}

			timeLineres += "  <div class=\"timeline-item clearfix\">";
			timeLineres += "    <div class=\"timeline-info\"><span class=\"timeline-date\">" + trace.date.split(' ')[1] + "</span><i class=\"timeline-indicator btn btn-info no-hover\"></i></div>";
			timeLineres += "    <div class=\"widget-box transparent\">";
			timeLineres += "      <div class=\"widget-body\">";
			timeLineres += "        <div class=\"widget-main no-padding\">";
			timeLineres += "          <span class=\"bigger-110 blue bolder\">" + trace.user + "</span>";
			for (var d = 0; d < trace.changes.length; d++) {
				var change = trace.changes[d];
				timeLineres += "<div><strong>" + change.Field + ":</strong>&nbsp;"
				timeLineres += change.Original;
				timeLineres += "&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;";
				timeLineres += change.Actual;
				timeLineres += "</div>";
			}

			timeLineres += "       </div>";
			timeLineres += "     </div>";
			timeLineres += "     </div>";
			timeLineres += "     </div>";
		}


		if (lastDate !== "") {
			res += timeLineres;
			res += "</div></div>";
		}

		res += "</div>";
		res += "</div>";
	}
	else {
		res += "<div style=\"padding:24px;\">" + data[0].Message + "</div>";
    }

	res += "</div>";

	$("#PopupDefaultBody").html(res);

	PopupDefault({
		"Title": "Traces d'activitats",
		"BtnDelete": false,
		"BtnOk": false
	});
}

function DefaultListDefinition(itemDefinition) {
	var columns = [];
	for (var f = 0; f < itemDefinition.Fields; f++) {
		if (itemDefinition.Fields[f].Name === "Id") {
			continue;
		}

		columns.push({ "DataProperty": itemDefinition.Fields[f] });
	}

	var res =
	{
		"Id": "Custom",
		"FormId": "Custom",
		"Layout": 1,
		"EditAction": "FormPage",
		"Columns": columns
	};

	return res;
}

function Item_Inactivate(itemName, itemId) {
	PopupDeleteContext.ItemDefinition = ItemDefinitionByName(itemName);
	PopupDeleteContext.ItemId = itemId;
	PopupRenderDelete();
	$("#LauncherPopupDelete").click();
}

function Item_InactivateConfirm() {
	//public ActionResult Inactivate (string itemName, long itemId, long applicationUserId,long companyId, string instanceName)
	var data = {
		"itemId": PopupDeleteContext.ItemId,
		"itemName": PopupDeleteContext.ItemDefinition.ItemName,
		"applicationUserId": ApplicationUser.Id,
		"companyId": Company.Id,
		"instanceName": Instance.Name
	};

	$.ajax({
		"type": "POST",
		"url": "/Async/ItemService.asmx/Inactivate",
		"contentType": "application/json; charset=utf-8",
		"dataType": "json",
		"data": JSON.stringify(data, null, 2),
		"success": function (msg) {
			$("#PopupDeleteBtnCancel").click();

			if (PopupDeleteContext.Mode === "list") {
				PopupSuccess("<strong>&quot;" + PopupDeleteContext.Message + "&quot;</strong> eliminat.", Dictionary.Common_Success);
				PageListById(PopupDeleteContext.ItemDefinition.ItemName, PopupDeleteContext.ListId).GetData();
			}
			else {
				PopupRenderDeleteResponse();
				$("#LauncherPopupDeleteResponse").click();
			}
		},
		"error": function (msg) {
			NotifySaveError(msg.responseText);
		}
	});
}

function ItemDefinition_HasFeature(itemDefinition, feature) {
	if (typeof itemDefinition !== "undefined" && itemDefinition !== null) {
		if (typeof itemDefinition.Features !== "undefined" && itemDefinition.Features !== null) {
			if (typeof itemDefinition.Features[feature] !== "undefined" && itemDefinition.Features[feature] !== null) {
				return itemDefinition.Features[feature];
            }
        }
    }

	return false;
}

function ItemGetDescription(itemDefinition, itemData) {
	var res = "";
	if (typeof itemDefinition.Layout.Description && itemDefinition.Layout.Description !== null) {
		var pattern = itemDefinition.Layout.Description.Pattern;
		var values = [];
		for (var v = 0; v < itemDefinition.Layout.Description.Fields.length; v++) {
			var fieldPattern = itemDefinition.Layout.Description.Fields[v];


			if (fieldPattern !== null) {
				var field = itemDefinition.Fields.filter(function (f) { return f.Name == fieldPattern.Name })[0];
				switch (field.Type.toUpperCase()) {
					case "MONEY":
						values.push("${ToMoneyFormat(" + itemData[field.Name] + ")}");
						break;
					default:
						var data = itemData[field.Name];
						if (typeof data === "object") {
							data = data.Value;
						}

						if (typeof data === "string") {
							data = "'" + data + "'";
                        }

						values.push("${" + data + "}");
						break;
				}
			}

		}

		//console.log(pattern.format(values));

		res = eval("`" + pattern.format(values) + "`");
	}

	return res;
}