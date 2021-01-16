"use strict";

$(document).ready(function () {


    var connection = new signalR.HubConnectionBuilder().withUrl("/notificationhub").build();
    connection.start().then(function () {
        //toastr["warning"]("hub connected")
        console.log('connected');
    }).catch(function (err) {
        return console.error(err.toString());
    });

    

    connection.on("add", function (message) {
        console.log(message);
        toastr["success"](message)
    });

    connection.on("newMessage", function (messageView, user) {
        console.log("connected");
        $("#sendButton").on('click', function () {
            console.log("send button clicked.");
            try {
                connection.start().then(function () {
                    //toastr["warning"]("hub connected")
                    console.log('connected');
                }).catch(function (err) {
                    return console.error(err.toString());
                });
                connection.invoke("SendToRole", "Student", $("#messageTxt").val());
            }
            catch {
                
                connection.invoke("SendToRole", "Student", $("#messageTxt").val());
            }
            //connection.invoke("SendToRole", "Student", $("#messageTxt").val());
        });

        console.log(messageView);
        $("#cUL li").append('<li><small>' + user + ':</small> ' + messageView + '</span></a></li>');
    });
    $("#sendButton").on('click', function () {
        connection.start().then(function () {
            //toastr["warning"]("hub connected")
            console.log('connected');
        }).catch(function (err) {
            return console.error(err.toString());
        });
        console.log("send button clicked.");
        connection.invoke("SendToRole", "Student", $("#messageTxt").val());
    });
    //connection.on("populateStudentsChatList", function (messageView) {

    //    console.log(messageView);
    //    console.log('on on');
    //    //$("#cUL li").append('<li><small>' + user + ':</small> ' + messageView + '</span></a></li>');
    //});


    


    

}); 