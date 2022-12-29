import React from "react";
import Card from "./Card.js";

function clickCard(cardName)
{
    // TODO(JRGF): Implement this!
}

export default function Cat(props) {
    let cards = [];
    for (const card of props.cards)
        cards.push(<Card key={card} name={props.cards[i]} clickCard={clickCard}></Card>);
    return (<div className="cat">
        { cards }
        </div>);
}