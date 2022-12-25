import Hand from "./Hand.js";
import "./Game.css";
import { BidPicker } from "./BidPicker";
import AcePicker from "./AcePicker";

function modalDialog(props)
{
  if (props.gameState.startsWith("bidding"))
    return <BidPicker bids={props.bids} gameState={props.gameState}
      setGameState={props.setGameState} connection={props.connection} />;
  if (props.gameState === "choosing-trump")
    return (<div className="overlay">
        <AcePicker title="Please choose trump:" onChoice={async (card) => {
            props.setGameState("waiting");
            const trump = card[0];
            await props.connection.invoke("SendChoice", "Trump is " + trump);
            console.log("The trump is " + trump + ".");
          }} />
      </div>);
  if (props.gameState === "choosing-buddy-ace")
    return (<div className="overlay">
        <AcePicker title="Please choose buddy ace:" onChoice={async (card) => {
            props.setGameState("waiting");
            await props.connection.invoke("SendChoice", "Buddy ace is " + card);
            console.log("The buddy ace is " + card + ".");
        }} />
      </div>);
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