import React from "react";
import Card from "./Card";

function clickCard(_cardName: string)
{
    // TODO(JRGF): Implement this!
}

interface CatProps {
    cards: string[];
}

export default function Cat(props: CatProps) {
    return (<div className="cat">
        { props.cards.map((card) => <Card key={card} name={card} clickCard={clickCard} doubleClickCard={() => {}}></Card>) }
        </div>);
}
