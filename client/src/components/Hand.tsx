import React from "react";
import Card from "./Card";

function cardPosition(size: number, index: number): string
{
    const mid = Math.floor(size / 2);
    if (index === mid)
        return "card-0";
    if (index > mid)
        return `card-r${index - mid}`;
    return `card-l${mid - index}`;
}

interface HandProps {
    cards: string[];
    playing?: boolean;
    playCard?: (card: string) => void;
}

interface HandState {
    selected: string | null;
}

export default class Hand extends React.Component<HandProps, HandState> {
    constructor(props: HandProps) {
        super(props);
        this.state = {
            selected: null
        };
    }

    clickCard(cardName: string)
    {
        if (this.props.playing)
            this.toggleSelected(cardName);
    }

    doubleClickCard(cardName: string)
    {
        if (this.isSelected(cardName))
            this.props.playCard?.(cardName);
    }

    toggleSelected(cardName: string) {
        if (this.isSelected(cardName))
            this.setState({ selected: null });
        else
            this.setState({ selected: cardName });
    }

    isSelected(cardName: string) {
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
            </div>);
    }
}
