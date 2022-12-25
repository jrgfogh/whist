import React from "react";
import Card from "./Card.js"

export default function AcePicker(props) {
    return (<div className="ace-picker">
        <h1>{props.title}</h1>
        {["C1", "D1", "S1", "H1"].map(cardName =>
            <Card key={ cardName } name= { cardName } clickCard={ () => props.onChoice(cardName) } />)}
        </div>);
}