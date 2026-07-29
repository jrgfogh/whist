import { EditableText } from "./EditableText";

export function Lobby(props) {
    return (<div>
            <h1>Player Names</h1>
            <ul className="list-group">
                {props.playerNames.map(player => 
                <li className="list-group-item list-group-item-action" key={player}>
                    <EditableText text={player} saveEdit={text => props.savePlayerName(text, text)} />
                </li>)}
            </ul>
        </div>);
}