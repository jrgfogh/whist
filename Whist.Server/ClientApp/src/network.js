import * as signalR from "@microsoft/signalr"

export function connect(client) {
    const connection = new signalR.HubConnectionBuilder()
        .configureLogging(signalR.LogLevel.Information)
        .withUrl("/WhistHub")
        .build();
    connection.on("ReceiveDealtCards", (cards) =>
        {
            cards.sort();
            client.receiveDealtCards(cards);
        });
    connection.start().then(function() {
        // TODO(jrgfogh): Do something!
    }).catch(function(err) {
        // TODO(jrgfogh): Do something!
        console.log(err);
    });
    return connection;
}