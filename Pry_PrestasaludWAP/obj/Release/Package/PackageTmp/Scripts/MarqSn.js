var msg;
function titnimation() {
    var msg1;

    var m1 = " ::PRESTASALUD S.A.:: ";
    var m2 = "Siempre Contigo, a tu Lado";
    var m3 = " ....::....";
    var m4 = "Copyright © 2015";
    if (msg == null) {
        msg = m1 + m2 + m3 + m4;
    }
    msg1 = m1 + m2 + m3 + m4;
    msg = msg.substring(2, msg.length) + msg1.substring(0, msg1.length);
    document.title = msg;
}

function marquesina() {
    var isIE4;
    isIE4 = (navigator.appVersion.charAt(0) >= 4) /*&& (navigator.appVersion).indexOf("MSIE") != -1);*/
    if (isIE4) {
        setInterval("titnimation()", 100);
    }
}