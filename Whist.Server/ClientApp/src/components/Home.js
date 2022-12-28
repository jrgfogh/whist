import Hand from "./Hand.js";
import "./Game.css";
import { BidPicker } from "./BidPicker";
import AcePicker from "./AcePicker";

function modalDialog(props)
{
  async function chooseAce(card) {
    props.setGameState("waiting");
    console.log("The buddy ace is " + card + ".");
    await props.connection.invoke("SendChoice", "Buddy ace is " + card);
  }

  async function chooseTrump(card) {
    props.setGameState("waiting");
    const trump = card[0];
    console.log("The trump is " + trump + ".");
    await props.connection.invoke("SendChoice", "Trump is " + trump);
  }

  if (props.gameState.endsWith("choosing-trump"))
    return (<div className="overlay">
        <AcePicker title="Please choose trump:" onChoice={chooseTrump} />
      </div>);
  if (props.gameState.endsWith("choosing-buddy-ace"))
    return (<div className="overlay">
        <AcePicker title="Please choose buddy ace:" onChoice={chooseAce} />
      </div>);
  if (props.gameState.startsWith("bidding"))
    return <BidPicker bids={props.bids} gameState={props.gameState}
      setGameState={props.setGameState} connection={props.connection} />;
  return "";
}

export function Home(props) {

  async function playCard(card) {
    props.setGameState("waiting");
    console.log("Played card: " + card);
    await props.connection.invoke("SendChoice", card);
  }

  return (
      <div className="game-background">
        <Hand cards={props.currentTrick} />
        <Hand cards={props.cardsInHand} playing={props.gameState === "playing-choosing-card"}
          playCard={playCard} />
        {modalDialog(props)}
      </div>
    );
}