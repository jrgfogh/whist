import React, { useState, useEffect } from "react";
import { Routes, Route } from "react-router";
import { Layout } from "./components/Layout";
import { Home } from "./components/Home";
import { connect } from "./network";
import { useNavigate } from "react-router-dom";

import "./custom.css"

export default function App(props) {
    const [connection, setConnection] = useState(null);
    const [cardsInHand, setCardsInHand] = useState([]);

    useEffect(() => {
        const newConnection = connect({
            receiveDealtCards: (cards) => {
                console.log(cards);
                setCardsInHand(cards);
            }
        });
        setConnection(newConnection);
    }, []);

    return (
      <Layout>
        <Routes>
            <Route exact path="/" element={
                <Home cardsInHand={cardsInHand}></Home>}>
            </Route>
        </Routes>
      </Layout>
    );
}
