import React, { useCallback } from "react";
import Hand from "./Hand";
import "./Game.css";
import { BidPicker } from "./BidPicker";
import AcePicker from "./AcePicker";
import type { HubConnection } from "@microsoft/signalr";
import type { Dispatch } from "react";
import type { GameAction, GameState } from "../gameStateReducer";

interface ModalDialogProps {
    dispatch: Dispatch<GameAction>;
    connection: HubConnection | null;
    gameState: GameState;
}

function ModalDialog({ dispatch, connection, gameState }: ModalDialogProps)
{
  const choosebuddyAce = useCallback(async (card: string) => {
      dispatch({ type: "user-chose-buddy-ace" });
      await connection?.invoke("SendChoice", `Buddy ace is ${card}`);
    }, [dispatch, connection]);

  const chooseTrump = useCallback(async (card: string) => {
      const trump = card[0];
      dispatch({ type: "user-chose-trump" });
      await connection?.invoke("SendChoice", `Trump is ${trump}`);
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
    return <BidPicker bids={gameState.bids ?? []} state={gameState.state}
      dispatch={dispatch} connection={connection} />;
  return null;
}

interface HomeProps {
    dispatch: Dispatch<GameAction>;
    connection: HubConnection | null;
    gameState: GameState;
    cardsInHand: string[];
}

export function Home({ dispatch, connection, gameState, cardsInHand }: HomeProps) {
  const playCard = useCallback(async (card: string) => {
    dispatch({ type: "user-chose-card", card: card });
    await connection?.invoke("SendChoice", card);
  }, [dispatch, connection]);

  return (
      <div className="game-background">
        <Hand cards={gameState.currentTrick ?? []} />
        <Hand cards={cardsInHand} playing={gameState.state === "playing-choosing-card"}
          playCard={playCard} />
        <ModalDialog dispatch={dispatch} connection={connection} gameState={gameState} />
      </div>
    );
}
