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
    const [currentTrick, setCurrentTrick] = useState([]);
    const [gameState, setGameState] = useState("connecting");

    var synchronousBids = [];
    var synchronousTrick = [];

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
                setGameState("bidding-choosing-trump");
            },
            promptForBuddyAce: () => {
                console.log("Please choose buddy ace!");
                setGameState("bidding-choosing-buddy-ace");
            },
            promptForCard: () => {
                console.log("Please play a card!");
                setGameState("playing-choosing-card");
            },
            receiveBiddingWinner: (winner, bid) => {
                setGameState("playing");
                console.log(winner + " wins bidding, " + bid);
            },
            receiveChoice: (chooser, choice) => {
                console.log("gameState: " + gameState);
                if (gameState.startsWith("bidding")) {
                    console.log("bids: " + synchronousBids);
                    synchronousBids = synchronousBids.concat(chooser + " bid " + choice);
                    setBids(synchronousBids);
                    console.log(chooser + " bids " + choice);
                }
                else {
                    synchronousTrick = synchronousTrick.concat(choice);
                    setCurrentTrick(synchronousTrick);
                    console.log(chooser + " played " + choice);
                }
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
                    <Home bids={bids} currentTrick={currentTrick} cardsInHand={cardsInHand} connection={connection} setGameState={setGameState} gameState={gameState}></Home>
                </div>}>
            </Route>
        </Routes>
      </Layout>
    );
}
