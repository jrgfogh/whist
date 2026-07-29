import React, { useCallback } from "react";

interface CardProps {
    name: string;
    position?: string;
    selected?: boolean;
    clickCard: (name: string) => void;
    doubleClickCard: (name: string) => void;
}

export default function Card(props: CardProps) {
    const clickCard = useCallback(() => props.clickCard(props.name), [props.clickCard, props.name]);
    const doubleClickCard = useCallback(() => props.doubleClickCard(props.name), [props.doubleClickCard, props.name]);
    let className = `card ${props.name} ${props.position ?? ""}`;
    if (props.selected)
        className = className + " selected";
    return (<div className={className}
        onClick={clickCard}
        onDoubleClick={doubleClickCard}></div>);
}
