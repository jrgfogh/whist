import React, { useCallback } from "react";

export default function Card(props) {
    const clickCard = useCallback(() => props.clickCard(props.name), [props.clickCard, props.name]);
    const doubleClickCard = useCallback(() => props.doubleClickCard(props.name), [props.doubleClickCard, props.name]);
    let className = `card ${props.name} ${props.position}`;
    if (props.selected)
        className = className + " selected";
    return (<div className={className}
        onClick={clickCard}
        onDoubleClick={doubleClickCard}></div>);
}