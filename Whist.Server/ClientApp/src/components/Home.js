import Hand from "./Hand.js";
import "./Game.css";
import { BidPicker } from "./BidPicker";

function modalDialog(props)
{
  if (props.gameState.startsWith("bidding"))
    return <BidPicker bids={props.bids} gameState={props.gameState}
      setGameState={props.setGameState} connection={props.connection} />;
  return "";
}

const currentTrick = ["C1", "SK", "HJ"];
export function Home(props) {
    return (
      <div className="game-background">
        <Hand cards={currentTrick} />
        <Hand cards={props.cardsInHand} />
        {modalDialog(props)}
      </div>
    );
}