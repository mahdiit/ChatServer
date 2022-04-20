"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chathub")
    .withHubProtocol(new signalR.protocols.msgpack.MessagePackHubProtocol())
    .build();

var currentUser;
connection.on("LoginSucess", function (userObj) {
    currentUser = userObj;
    $('#form-login-data').hide();
    $('#form-chat-data').show();

    $('#form-me').html(currentUser.Name);
});

var templateA = '<li class="chat-right"><div class="chat-hour">{data-time} <span class="fa fa-check-circle"></span></div>' +
    '<div class="chat-text">{data-content}</div>' +
    '<div class="chat-avatar">' +
    '<img src="/images/{data-image}.png" alt="{data-name}">' +
    '<div class="chat-name">{data-name}</div>' +
    '</div>' +
    '</li>';
var templateB = '<li class="chat-left"><div class="chat-avatar">' +
    '<img src="/images/{data-image}.png" alt="{data-name}">' +
    '<div class="chat-name">{data-name}</div>' +
    '</div>' +
    '<div class="chat-text">{data-content}</div>' +
    '<div class="chat-hour">{data-time} <span class="fa fa-check-circle"></span></div>' +
    '</li>';
var templateC = '<li class="person" data-chat="{data-id}">' +
    '<div class="user">' +
    '<img src="/images/{data-image}.png" alt="{data-name}">' +
    '<span class="status"></span>' +
    '</div>' +
    '<p class="name-time">' +
    '<span class="name">{data-name}</span>' +
    '<span class="time">&nbsp;</span>' +
    '</p>' +
    '</li>';

connection.on("ReceiveMessage", function (msgObj) {
    if (connection.connectionId == msgObj.User.ConnectionId) {
        $('#form-chat-box').append(templateA
            .replace(/{data-time}/g, msgObj.Time)
            .replace(/{data-content}/g, msgObj.Content)
            .replace(/{data-image}/g, msgObj.User.ImageUrl)
            .replace(/{data-name}/g, msgObj.User.Name));
    } else {
        $('#form-chat-box').append(templateB
            .replace(/{data-time}/g, msgObj.Time)
            .replace(/{data-content}/g, msgObj.Content)
            .replace(/{data-image}/g, msgObj.User.ImageUrl)
            .replace(/{data-name}/g, msgObj.User.Name));
    }
});

connection.on("UpdateContactList", function (userObj) {
    $('#form-contact').html("");
    for (var i = 0; i < userObj.length; i++) {
        var item = userObj[i];
        $('#form-contact').append(
            templateC
                .replace(/{data-image}/g, item.ImageUrl)
                .replace(/{data-name}/g, item.Name)
                .replace(/{data-id}/g, item.ConnectionId)
        );
    }
});

$(function () {

    connection.start();

    $('#form-login-button').click(function () {
        var userName = $('#username').val();
        connection.invoke("Login", connection.connectionId, userName, "").catch(function (err) {
            return console.error(err.toString());
        });
    });

    $('#form-chat-text').keypress(function (event) {
        if (event.which == 13) {
            var date = new Date();
            var minutes = date.getMinutes();
            var hour = date.getHours();
            var obj = {
                User: currentUser,
                Content: $('#form-chat-text').val(),
                Time: hour + ":" + minutes
            };
            connection.invoke("SendMessage", obj).catch(function (err) {
                return console.error(err.toString());
            });
            event.preventDefault();
            $('#form-chat-text').val("");
        }
    });
})



