Office.initialize = function () {
}

// Helper function to add a status message to the info bar.
function statusUpdate(icon, text) {
  Office.context.mailbox.item.notificationMessages.replaceAsync("status", {
    type: "informationalMessage",
    icon: icon,
    message: text,
    persistent: false
  });
}

function defaultStatus(event) {
    function defaultStatus(event) {
        debugger;
        statusUpdate("icon16", "Launching Client Billabale Matter Dialog");

        Office.context.ui.displayDialogAsync(window.location.origin + '/dist/taskpane.html', { height: 50, width: 50, promptBeforeOpen: false }, function (result) {

            dialog = result.value;
            result.value.height = 15;
            result.value.width = 15;
            dialog.addEventHandler(Office.EventType.DialogMessageReceived, processMessage);
        });

    }
}