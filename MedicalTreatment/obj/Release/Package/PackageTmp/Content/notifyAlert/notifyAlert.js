// amd Murtada
function notifyAlert(body, alertType, title = "Notification !", length = 5, isScrollable = true) {
    if (alertType === "Error")
        alertType = "info";
var mkConfig = {
        positionY: 'top',//isTopOrBottom,
        positionX: 'right',//isLeftOrRight,
        max: length,
        scrollable: isScrollable
    };

    mkNotifications(mkConfig);

    mkNoti(
        title.toLowerCase(), body, {
            status: alertType
        }
    );
}

