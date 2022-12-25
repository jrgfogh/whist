import Hand from "./Hand.js";
import "./Game.css";
import { BidPicker } from "./BidPicker";

function modalDialog(props)
{
  if (props.gameState.startsWith("bidding"))
    return <BidPicker bids={props.bids} gameState={props.gameState}
      setGameState={props.setGameState} connection={props.connection} />;
  if (props.gameState === "choosing-trump")
    return <h1>Choosing trump!</h1>
  return "";
}

const currentTrick = [];
export function Home(props) {
    return (
      <div className="game-background">
        <Hand cards={currentTrick} />
        <Hand cards={props.cardsInHand} />
        {modalDialog(props)}
      </div>
    );
}