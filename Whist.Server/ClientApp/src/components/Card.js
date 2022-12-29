import React from "react";

export default function Card(props) {
    let className = `card ${props.name} ${props.position}`;
    if (props.selected)
        className = className + " selected";
    return (<div className={className}
        onClick={() => props.clickCard(props.name)}
        onDoubleClick={() => props.doubleClickCard(props.name)}></div>);
}