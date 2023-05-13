var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("ReceiveMessage", function (user, message) {
    var encodedUser = $("<div />").text(user).html();
    var encodedMsg = $("<div />").text(message).html();
    $("#chatBox").append("<p><strong>" + encodedUser + "</strong>: " + encodedMsg + "</p>");
});

$("#sendButton").click(function () {
    var user = $("#username").val();
    var message = $("#message").val();
    connection.invoke("SendMessage", user, message);
    $("#message").val("").focus();
});

connection.start().then(function () {
    console.log("Connected!");
}).catch(function (err) {
    console.error(err.toString());
});