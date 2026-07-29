import React, { useState, useReducer, useEffect } from "react";
import { Routes, Route } from "react-router";
import { Layout } from "./components/Layout";
import { Home } from "./components/Home";
import gameStateReducer, { type GameAction, type GameState } from "./gameStateReducer";
import type { HubConnection } from "@microsoft/signalr";
import type { Dispatch } from "react";

import "./custom.css"

interface AppProps {
    connect: (dispatch: Dispatch<GameAction>) => HubConnection;
}

export default function App({ connect }: AppProps) {
    const [connection, setConnection] = useState<HubConnection | null>(null);
    const [gameState, dispatch] = useReducer(gameStateReducer, { state: "connecting" } as GameState);

    useEffect(() => {
        setConnection(connect(dispatch));
    }, [connect]);

    return (
      <Layout>
        <Routes>
            <Route exact path="/" element={
                <div>
                    <h1>{gameState.state}</h1>
                        <Home cardsInHand={gameState.cards ?? []} connection={connection} dispatch={dispatch} gameState={gameState}></Home>
                </div>}>
            </Route>
        </Routes>
      </Layout>
    );
}
