import Hand from "./Hand.js";
import "./Game.css";
import { BidPicker } from "./BidPicker";
import AcePicker from "./AcePicker";
import { useCallback } from "react";

function ModalDialog({ dispatch, connection, gameState })
{
  const choosebuddyAce = useCallback(async (card) => {
      dispatch({ type: "user-chose-buddy-ace", choice: card });
      await connection.invoke("SendChoice", `Buddy ace is ${card}`);
    }, [dispatch, connection]);

  const chooseTrump = useCallback(async (card) => {
      const trump = card[0];
      dispatch({ type: "user-chose-trump", choice: trump });
      await connection.invoke("SendChoice", `Trump is ${trump}`);
    }, [dispatch, connection]);

  if (gameState.state.endsWith("choosing-trump"))
    return (<div className="overlay">
        <AcePicker title="Please choose trump:" onChoice={chooseTrump} />
      </div>);
  if (gameState.state.endsWith("choosing-buddy-ace"))
    return (<div className="overlay">
        <AcePicker title="Please choose buddy ace:" onChoice={choosebuddyAce} />
      </div>);
  if (gameState.state.startsWith("bidding"))
    return <BidPicker bids={gameState.bids || []} state={gameState.state}
      dispatch={dispatch} connection={connection} />;
  return "";
}

export function Home({ dispatch, connection, gameState, cardsInHand }) {
  const playCard = useCallback(async (card) => {
    dispatch({ type: "user-chose-card", card: card });
    await connection.invoke("SendChoice", card);
  }, [dispatch, connection]);

  return (
      <div className="game-background">
        <Hand cards={gameState.currentTrick || []} />
        <Hand cards={cardsInHand} playing={gameState.state === "playing-choosing-card"}
          playCard={playCard} />
        <ModalDialog dispatch={dispatch} connection={connection} gameState={gameState} />
      </div>
    );
}