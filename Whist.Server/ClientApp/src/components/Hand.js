import React from "react";
import Card from "./Card.js"

function cardPosition(size, index)
{
    const mid = Math.floor(size / 2);
    if (index === mid)
        return "card-0";
    if (index > mid)
        return "card-r" + (index - mid);
    return "card-l" + (mid - index);
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
        if (this.state.selected === cardName)
            this.setState({ selected: null });
        else
            this.setState({ selected: cardName });
    }

    render() {
        return (<div className="hand">
                {this.props.cards.map((cardName, index) => <Card key={"card" + index}
                    position={cardPosition(this.props.cards.length, index)}
                    name={cardName} clickCard={() => this.clickCard(index)}
                    selected={this.state.selected === index} />)}
            </div>);
    }
}