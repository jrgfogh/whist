import React from "react";
import Card from "./Card.js"

function cardPosition(size, index)
{
    const mid = Math.floor(size / 2);
    if (index === mid)
        return "card-0";
    if (index > mid)
        return `card-r${index - mid}`;
    return `card-l${mid - index}`;
}

export default class Hand extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            selected: null
        };
    }

    clickCard(cardName)
    {
        if (this.props.playing)
            this.toggleSelected(cardName);
    }

    doubleClickCard(cardName)
    {
        if (this.isSelected(cardName))
            this.props.playCard(cardName);
    }

    toggleSelected(cardName) {
        if (this.isSelected(cardName))
            this.setState({ selected: null });
        else
            this.setState({ selected: cardName });
    }

    isSelected(cardName) {
        return this.state.selected === cardName;
    }

    render() {
        return (<div className="hand">
                {this.props.cards.map((cardName, index) => 
                    <Card key={"card" + index}
                            position={cardPosition(this.props.cards.length, index)}
                            name={cardName} clickCard={() => this.clickCard(cardName)}
                            doubleClickCard={() => this.doubleClickCard(cardName)}
                            selected={this.state.selected === cardName} />)}
                {this.props.playing &&
                    <button className="pass-button" type="button" onClick={() => this.props.playCard("pass")}>Pass!</button>}
            </div>);
    }
}