﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationhub").build();
var user = "sksami";
var message = "testing testing 1 2 3";
//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

//connection.on("ReceiveMessage", function (user, message) {
//    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
//    var encodedMsg = user + " says " + msg;
//    var li = document.createElement("li");
//    li.textContent = encodedMsg;
//    document.getElementById("messagesList").appendChild(li);
//});

connection.on("ReceiveMessage", function (user, message) {
    console.log(user + " " + message);
});


connection.start().then(function () {
    console.log("connected");
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});