import * as signalR from "@microsoft/signalr"

export function connect(dispatch) {
    const connection = new signalR.HubConnectionBuilder()
        .configureLogging(signalR.LogLevel.Information)
        .withUrl("/WhistHub")
        .build();
    connection.on("ReceiveDealtCards", (cards) => { dispatch({ type: "receive-cards", cards: cards }); });
    connection.on("AnnounceBiddingWinner", (winner, bid) => { dispatch({ type: "bidding-winner", winner: winner, bid: bid }); });
    connection.on("AnnounceWinner", (winner) => { dispatch({ type: "trick-winner", winner: winner }); });
    connection.on("ReceiveChoice", (chooser, choice) => { dispatch({ type: "receive-choice", chooser: chooser, choice: choice }); });
    connection.on("PromptForBid", () => { dispatch({ type: "prompt-for-bid" }); });
    connection.on("PromptForTrump", () => { dispatch({ type: "prompt-for-trump" }); });
    connection.on("PromptForBuddyAce", () => { dispatch({ type: "prompt-for-buddy-ace" }); });
    connection.on("PromptForCard", () => { dispatch({ type: "prompt-for-card" }); });
    connection.on("StartPlaying", () => { dispatch({ type: "start-playing" }); });
    connection.start().then(function() {
        // TODO(jrgfogh): Do something!
    }).catch(function(err) {
        // TODO(jrgfogh): Do something!
        console.log(err);
    });
    return connection;
}