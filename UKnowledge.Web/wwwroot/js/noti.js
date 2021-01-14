"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationhub").build();
var user = "sksami";
var message = "testing testing 1 2 3";
var toUser = "ff";
var method = "add";
var fromUser = "ff1";

connection.on("add", function (message) {
    console.log(message);
    toastr["success"](message)
});

connection.start().then(function () {
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    
}).catch(function (err) {
    return console.error(err.toString());
});
