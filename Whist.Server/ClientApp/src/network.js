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
    connection.on("AnnounceBiddingWinner", client.receiveBiddingWinner);
    connection.on("ReceiveChoice", client.receiveChoice);
    connection.on("PromptForBid", client.promptForBid);
    connection.on("PromptForTrump", client.promptForTrump);
    connection.on("PromptForBuddyAce", client.promptForBuddyAce);
    connection.on("PromptForCard", client.promptForCard);
    connection.on("StartPlaying", client.startPlaying);
    connection.start().then(function() {
        // TODO(jrgfogh): Do something!
    }).catch(function(err) {
        // TODO(jrgfogh): Do something!
        console.log(err);
    });
    return connection;
}