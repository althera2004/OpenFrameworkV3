function CustomActions() {

    $("#SubscriptionStart").html(Company.SubscriptionStart);
    $("#SubscriptionEnd").html(Company.SubscriptionEnd);

    var subscriptionEnd = moment(GetDate(Company.SubscriptionEnd, "/", false));
    var b = moment();
    $("#SubscriptionRemains").html(subscriptionEnd.diff(b, "days"));

    $(".FeatureIcon").addClass("fa-ban");
    $(".FeatureIcon").addClass("btn-icon-red");

    if (HasPropertyValue(Company.Features)) {
        var features = Company.Features.split('|');
        for (var f = 0; f < features.length; f++) {
            if (features[f] !== "") {
                SERVICES_FeatureSetIconYes(features[f]);
            }
        }
    }
}

function SERVICES_FeatureSetIconYes(code) {
    $("#Check_" + code).removeClass("fa-ban");
    $("#Check_" + code).removeClass("btn-icon-red");
    $("#Check_" + code).addClass("fa-check");
    $("#Check_" + code).addClass("btn-icon-green");
}

function SERVICES_FeatureSetIconNo(code) {
    $("#Check_" + code).removeClass("fa-check");
    $("#Check_" + code).removeClass("btn-icon-green");
    $("#Check_" + code).addClass("fa-ban");
    $("#Check_" + code).addClass("btn-icon-red");
}