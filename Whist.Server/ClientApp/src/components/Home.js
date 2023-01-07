import Hand from "./Hand.js";
import "./Game.css";
import { BidPicker } from "./BidPicker";
import AcePicker from "./AcePicker";
import { useCallback } from "react";

function modalDialog(props)
{
  const choosebuddyAce = useCallback(async (card) => {
      props.dispatch({ type: "user-chose-buddy-ace", choice: card });
      await props.connection.invoke("SendChoice", `Buddy ace is ${card}`);
    }, [props.dispatch, props.connection]);

  const chooseTrump = useCallback(async (card) => {
      const trump = card[0];
      props.dispatch({ type: "user-chose-trump", choice: trump });
      await props.connection.invoke("SendChoice", `Trump is ${trump}`);
    }, [props.dispatch, props.connection]);

  if (props.gameState.state.endsWith("choosing-trump"))
    return (<div className="overlay">
        <AcePicker title="Please choose trump:" onChoice={chooseTrump} />
      </div>);
  if (props.gameState.state.endsWith("choosing-buddy-ace"))
    return (<div className="overlay">
        <AcePicker title="Please choose buddy ace:" onChoice={choosebuddyAce} />
      </div>);
  if (props.gameState.state.startsWith("bidding"))
    return <BidPicker bids={props.gameState.bids || []} state={props.gameState.state}
      dispatch={props.dispatch} connection={props.connection} />;
  return "";
}

export function Home(props) {
  return (
      <div className="game-background">
        <Hand cards={[]} />
        <Hand cards={props.cardsInHand} playing={props.gameState === "playing-choosing-card"}
          playCard={props.playCard} />
        {modalDialog(props)}
      </div>
    );
}