import * as signalR from "@microsoft/signalr"
import type { Dispatch } from "react";
import type { GameAction } from "./gameStateReducer";

export function connect(dispatch: Dispatch<GameAction>): signalR.HubConnection {
    const connection = new signalR.HubConnectionBuilder()
        .configureLogging(signalR.LogLevel.Information)
        .withUrl(new URL("WhistHub", document.baseURI).href)
        .build();
    connection.on("ReceiveDealtCards", (cards: string[]) => { dispatch({ type: "receive-cards", cards: cards }); });
    connection.on("AnnounceBiddingWinner", (winner: string, bid: string) => { dispatch({ type: "bidding-winner", winner: winner, bid: bid }); });
    connection.on("AnnounceWinner", (winner: string) => { dispatch({ type: "trick-winner", winner: winner }); });
    connection.on("ReceiveChoice", (chooser: string, choice: string) => { dispatch({ type: "receive-choice", chooser: chooser, choice: choice }); });
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
