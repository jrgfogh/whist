import React from "react";
import Card from "./Card.js"

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
            {this.props.cards.map((cardName, index) => <Card key={"card" + index} name={cardName} clickCard={() => this.clickCard(cardName)} selected={this.state.selected === cardName} />)}
            </div>);
    }
}