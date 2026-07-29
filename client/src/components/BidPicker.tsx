import React from "react";
import type { HubConnection } from "@microsoft/signalr";
import type { Dispatch } from "react";
import type { Bid, GameAction } from "../gameStateReducer";

function range(min: number, max: number): number[] {
  return [...Array(max - min + 1).keys()].map(i => i + min);
}

interface BidPickerProps {
    state: string;
    bids: Bid[];
    dispatch: Dispatch<GameAction>;
    connection: HubConnection | null;
}

export function BidPicker(props: BidPickerProps) {
  function sendBid(bid: string) {
    return async function() {
      props.dispatch({ type: "user-chose-bid" });
      await props.connection?.invoke("SendChoice", bid);
    }
  }

  function button(bid: string | number) {
    const bidStr = String(bid);
    return <td key={bidStr}><button type="button"
      disabled={!props.state.endsWith("choosing-bid")} onClick={sendBid(bidStr)}>Bid!</button></td>;
  }

  function buttonRow(postfix: string) {
    return range(6, 13).map(element =>
      button(element + postfix));
  }

  return <div className="overlay">
    <div className="bidding-dialog">
      <h1>Please Bid!</h1>
      <table>
        <tbody>
          <tr>
            <th key="head"></th>
            {range(6, 13).map(element => <th key={element}>{element}</th>)}
          </tr>
          <tr>
            <th key="head">Common</th>
            {buttonRow(" Common")}
          </tr>
          <tr>
            <th key="head">Good</th>
            {buttonRow(" Good")}
          </tr>
          <tr>
            <th key="head">Vip</th>
            {buttonRow(" Vip")}
          </tr>
          <tr>
            <th key="head">Pass</th>
            {button("pass")}
          </tr>
        </tbody>
      </table>
      <div>
        {props.bids.map((bid) => {
          const bidDescription = bid.bidder + " bid " + bid.bid;
          return <p key={bidDescription}>{bidDescription}</p>;
        })}
      </div>
    </div>
  </div>;
}
