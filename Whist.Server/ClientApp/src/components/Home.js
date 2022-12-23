import Hand from "./Hand.js";
import "./Game.css";

export function Home(props) {
    return (
      <div className="game-background">
        <Hand cards={props.cardsInHand}></Hand>
      </div>
    );
}
