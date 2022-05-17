function Instancia_Column_Tipo(data, row) {
    var res = "";

    switch (data) {
        case "1":
            res = "OpenFramework v1";
            break;
        case "2":
            res = "Custom";
            break;
        case "3":
            res = "OpenFramework v2";
            break;
        default:
            res = "-";
            break;
    }

    return res;
}