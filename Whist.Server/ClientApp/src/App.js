import React, { useState, useRef, useEffect } from "react";
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
    const [gameState, setGameState0] = useState("connecting");

    const bidsRef = useRef([]);
    const trickRef = useRef([]);
    const gameStateRef = useRef([]);

    function setGameState(newState) {
        setGameState0(newState);
        gameStateRef.current = newState;
    }

    async function playCard(card) {
        setGameState("playing");
        console.log("Played card: " + card);
        setCardsInHand(cards => {
            const index = cards.indexOf(card);
            if (index !== -1)
                cards.splice(index, 1);
            else
                throw new Error("Error: Tried to play a card not in the hand!");
            return cards;
        });
        await connection.invoke("SendChoice", card);
    }

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
            startPlaying: () => {
                console.log("Start playing!");
                setGameState("playing");
            },
            receiveBiddingWinner: (winner, bid) => {
                console.log(winner + " wins bidding, " + bid);
            },
            receiveChoice: (chooser, choice) => {
                console.log("gameState: " + gameStateRef.current);
                if (gameStateRef.current.startsWith("bidding")) {
                    console.log("bids: " + bidsRef.current);
                    bidsRef.current = bidsRef.current.concat(chooser + " bid " + choice);
                    setBids(bidsRef.current);
                    console.log(chooser + " bids " + choice);
                }
                else if (gameStateRef.current.startsWith("playing")) {
                    trickRef.current = trickRef.current.concat(choice);
                    setCurrentTrick(trickRef.current);
                    console.log(chooser + " played " + choice);
                }
                else {
                    console.log(chooser + " chose " + choice + " in state: " + gameStateRef.current);
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
                        <Home bids={bids} currentTrick={currentTrick} cardsInHand={cardsInHand}
                            connection={connection} setGameState={setGameState} gameState={gameState}
                            playCard={playCard}></Home>
                </div>}>
            </Route>
        </Routes>
      </Layout>
    );
}
