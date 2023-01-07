import React, { useState, useReducer, useEffect } from "react";
import { Routes, Route } from "react-router";
import { Layout } from "./components/Layout";
import { Home } from "./components/Home";
import { connect } from "./network";
import gameStateReducer from "./gameStateReducer";

import "./custom.css"

export default function App(props) {
    const [connection, setConnection] = useState(null);
    const [gameState, dispatch] = useReducer(gameStateReducer, { state: "connecting" });

    useEffect(() => {
        setConnection(connect(dispatch));
    }, []);

    return (
      <Layout>
        <Routes>
            <Route exact path="/" element={
                <div>
                    <h1>{gameState.state}</h1>
                        <Home cardsInHand={gameState.cards || []} connection={connection} dispatch={dispatch} gameState={gameState}></Home>
                </div>}>
            </Route>
        </Routes>
      </Layout>
    );
}
