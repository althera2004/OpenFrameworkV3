var startTime = 0;
var end = new Date();
var diff = 0;
var lastTime = new Date();
var timerID = 0;
var sessionControl;
var start = Date.now();
var timePassed = null;

function chrono() {
    end = new Date();
    lastTime = end;
    diff = end - start;
    diff = new Date(diff);
    var cr_sec = diff.getSeconds();
    var cr_min = diff.getMinutes();
    timePassed = diff.getMinutes() * 1;
    var cr_hr = diff.getHours() - 1;

    if (cr_min < 10) { cr_min = "0" + cr_min; }
    if (cr_sec < 10) { cr_sec = "0" + cr_sec; }

    /*if (cr_hr === 0 && cr_min === 0) {
        $("#chronotime").html(cr_sec + " " + Dictionary.Common_Chronometer_Seconds);
    }
    else if (cr_hr === 0) {
        $("#chronotime").html(cr_min + " " + Dictionary.Common_Chronometer_Minutes + " " + cr_sec + " " + Dictionary.Common_Chronometer_Seconds);
    }
    else {
        $("#chronotime").html(cr_hr + " " + Dictionary.Common_Chronometer_Hours + " " + cr_min + " " + Dictionary.Common_Chronometer_Minutes);
    }*/

    if (timePassed >= 15) {
        PopupLoginAndContinue();
    }

    timerID = setTimeout(chrono, 300);
}

function SessionRestart() {
    start = Date.now();
}

chrono();
SessionRestart();


// Renovar sesión
$(document).on("click", SessionRestart);
$(document).mousemove(SessionRestart);