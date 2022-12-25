import React, { useState, useEffect } from "react";
import { Routes, Route } from "react-router";
import { Layout } from "./components/Layout";
import { Home } from "./components/Home";
import { connect } from "./network";

import "./custom.css"

export default function App(props) {
    const [connection, setConnection] = useState(null);
    const [cardsInHand, setCardsInHand] = useState([]);
    const [bids, setBids] = useState([]);
    const [gameState, setGameState] = useState("connecting");

    var synchronousBids = [];

    useEffect(() => {
        const newConnection = connect({
            receiveDealtCards: (cards) => {
                console.log("Received cards:");
                console.log(cards);
                setCardsInHand(cards);
                setGameState("bidding");
            },
            promptForBid: () => {
                console.log("Please bid!");
                setGameState("bidding-active");
            },
            promptForTrump: () => {
                console.log("Please choose trump!");
                setGameState("choosing-trump");
            },
            promptForBuddyAce: () => {
                console.log("Please choose buddy ace!");
                setGameState("choosing-buddy-ace");
            },
            receiveBiddingWinner: (winner, bid) => {
                setGameState("waiting");
                console.log(winner + " wins bidding, " + bid);
            },
            receiveChoice: (chooser, choice) => {
                console.log("bids: " + synchronousBids);
                synchronousBids = synchronousBids.concat(chooser + " bid " + choice);
                setBids(synchronousBids);
                console.log(chooser + " chose " + choice)
            }
        });
        setConnection(newConnection);
    }, []);

    return (
      <Layout>
        <Routes>
            <Route exact path="/" element={
                <div>
                    <h1>{gameState}</h1>
                    <Home bids={bids} cardsInHand={cardsInHand} connection={connection} setGameState={setGameState} gameState={gameState}></Home>
                </div>}>
            </Route>
        </Routes>
      </Layout>
    );
}
