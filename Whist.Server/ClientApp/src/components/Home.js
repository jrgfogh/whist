import Hand from "./Hand.js";
import "./Game.css";

const currentTrick = ["C1", "SK", "HJ"];
export function Home(props) {
    return (
      <div className="game-background">
        <Hand cards={currentTrick} />
        <Hand cards={props.cardsInHand} />
      </div>
    );
}
